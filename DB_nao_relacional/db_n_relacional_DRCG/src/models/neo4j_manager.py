"""Gerenciador de conexão com Neo4j.

Tenta conectar ao servidor real via driver oficial (neo4j>=5); se indisponível
e o fallback estiver ativo, usa FakeGraphSession — grafo em memória construído
sobre networkx, com a mesma interface pública exigida pelo Neo4jService.

Conceito de grafo demonstrado:
  - Nós (Node): Cliente, Produto, Pedido, Categoria
  - Arestas (Relationship): REALIZOU, CONTEM, PERTENCE_A
  - Consultas por padrão de vizinhança (pattern matching) → recomendações
  - Algoritmos de grafos: co-compras (frequência de co-ocorrência em pedidos)
"""

from __future__ import annotations

import logging
from collections import Counter


# ─────────────────────────────────────────────────────────────────────────────
# Emulação em memória (FakeGraph com networkx)
# ─────────────────────────────────────────────────────────────────────────────

class FakeGraphSession:
    """
    Grafo dirigido em memória usando networkx.DiGraph como backend.

    Convenção de node-id: "{Label}:{primary_key}"
        Ex.: "Produto:42"   "Cliente:7"   "Pedido:100"   "Categoria:Eletronicos"

    Interface pública usada pelo Neo4jService:
        add_node(label, **props) -> node_id
        add_edge(from_id, to_id, relation, **props)
        find_co_purchased(produto_id, limit) -> list[dict]
        cliente_compras_path(cliente_id) -> list[dict]
        node_count() -> int
        edge_count() -> int
    """

    def __init__(self) -> None:
        try:
            import networkx as nx  # type: ignore
        except ImportError as exc:
            raise ImportError(
                "networkx não está instalado. Execute: pip install networkx"
            ) from exc

        self._g: "nx.DiGraph" = nx.DiGraph()

    # ------------------------------------------------------------------
    def add_node(self, label: str, **props) -> str:
        """Adiciona (ou atualiza) um nó. Retorna o node_id canônico."""
        raw_id = props.get("id", id(props))
        node_id = f"{label}:{raw_id}"
        self._g.add_node(node_id, label=label, **props)
        return node_id

    def add_edge(self, from_id: str, to_id: str, relation: str, **props) -> None:
        """Adiciona uma aresta dirigida entre dois nós já existentes."""
        self._g.add_edge(from_id, to_id, relation=relation, **props)

    # ------------------------------------------------------------------
    def find_co_purchased(self, produto_id: int, limit: int = 5) -> list[dict]:
        """
        Encontra produtos frequentemente comprados no mesmo pedido que
        o produto dado.

        Padrão de grafo equivalente ao Cypher:
            MATCH (p:Produto {id: $id})<-[:CONTEM]-(o:Pedido)-[:CONTEM]->(outro:Produto)
            WHERE outro.id <> $id
            RETURN outro.id, outro.nome, COUNT(o) AS co_compras
            ORDER BY co_compras DESC LIMIT $limit
        """
        prod_node = f"Produto:{produto_id}"
        if not self._g.has_node(prod_node):
            return []

        # Pedidos que contêm este produto (arestas de entrada com relação CONTEM)
        pedidos_com_produto: set[str] = set()
        for src, _, data in self._g.in_edges(prod_node, data=True):
            if data.get("relation") == "CONTEM":
                pedidos_com_produto.add(src)

        # Para cada pedido, conta os demais produtos presentes
        co_counter: Counter = Counter()
        for pedido_node in pedidos_com_produto:
            for _, dst, data in self._g.out_edges(pedido_node, data=True):
                if data.get("relation") == "CONTEM" and dst != prod_node:
                    co_counter[dst] += 1

        # Monta resultado com atributos do nó
        resultado: list[dict] = []
        for prod_node_id, count in co_counter.most_common(limit):
            if self._g.has_node(prod_node_id):
                attrs = dict(self._g.nodes[prod_node_id])
                resultado.append(
                    {
                        "id_produto": attrs.get("id"),
                        "nome": attrs.get("nome"),
                        "preco": attrs.get("preco"),
                        "co_compras": count,
                    }
                )
        return resultado

    # ------------------------------------------------------------------
    def cliente_compras_path(self, cliente_id: int) -> list[dict]:
        """
        Retorna todos os produtos comprados por um cliente, navegando
        dois níveis de arestas:
            Cliente -[REALIZOU]-> Pedido -[CONTEM]-> Produto

        Padrão equivalente ao Cypher:
            MATCH (c:Cliente {id: $id})-[:REALIZOU]->(o:Pedido)-[:CONTEM]->(p:Produto)
            RETURN o.id AS pedido, p.id, p.nome, p.preco
        """
        cli_node = f"Cliente:{cliente_id}"
        if not self._g.has_node(cli_node):
            return []

        resultado: list[dict] = []
        for _, pedido_node, d1 in self._g.out_edges(cli_node, data=True):
            if d1.get("relation") != "REALIZOU":
                continue
            ped_id = pedido_node.split(":", 1)[1] if ":" in pedido_node else pedido_node
            for _, prod_node, d2 in self._g.out_edges(pedido_node, data=True):
                if d2.get("relation") != "CONTEM":
                    continue
                attrs = dict(self._g.nodes[prod_node])
                resultado.append(
                    {
                        "pedido_id": ped_id,
                        "id_produto": attrs.get("id"),
                        "nome": attrs.get("nome"),
                        "preco": attrs.get("preco"),
                    }
                )
        return resultado

    # ------------------------------------------------------------------
    def node_count(self) -> int:
        return self._g.number_of_nodes()

    def edge_count(self) -> int:
        return self._g.number_of_edges()


# ─────────────────────────────────────────────────────────────────────────────
# Manager principal
# ─────────────────────────────────────────────────────────────────────────────

class Neo4jManager:
    """
    Encapsula a conexão com o Neo4j.

    Em caso de falha e com use_fake_on_failure=True, substitui
    transparentemente pelo FakeGraphSession (networkx em memória).
    """

    def __init__(
        self,
        uri: str = "bolt://localhost:7687",
        user: str = "neo4j",
        password: str = "neo4j_password",
        database: str = "neo4j",
        use_fake_on_failure: bool = True,
    ) -> None:
        self.uri = uri
        self.user = user
        self.password = password
        self.database = database
        self.use_fake_on_failure = use_fake_on_failure

        self._driver = None
        self._fake_session: FakeGraphSession | None = None
        self.using_fake = False

        self._connect()

    # ------------------------------------------------------------------
    def _connect(self) -> None:
        try:
            from neo4j import GraphDatabase  # type: ignore

            driver = GraphDatabase.driver(self.uri, auth=(self.user, self.password))
            driver.verify_connectivity()
            self._driver = driver
            self.using_fake = False
            logging.info("Conectado ao Neo4j com sucesso.")
        except Exception as exc:
            logging.warning("Falha ao conectar ao Neo4j: %s", exc)
            if self.use_fake_on_failure:
                logging.info("Iniciando FakeGraphSession com networkx (modo mock em memória).")
                self._fake_session = FakeGraphSession()
                self.using_fake = True
            else:
                raise RuntimeError(f"Neo4j indisponível e fallback desativado: {exc}") from exc

    # ------------------------------------------------------------------
    def get_fake_session(self) -> FakeGraphSession:
        """Retorna a sessão fake (só válido quando using_fake=True)."""
        assert self._fake_session is not None
        return self._fake_session

    def run_cypher(self, query: str, **params) -> list:
        """Executa uma query Cypher no Neo4j real e retorna lista de registros."""
        assert self._driver is not None
        with self._driver.session(database=self.database) as session:
            result = session.run(query, **params)
            return [dict(record) for record in result]

    # ------------------------------------------------------------------
    def test_connection(self) -> tuple[bool, str]:
        try:
            if self.using_fake:
                modo = "FakeGraphSession com networkx (memória)"
            else:
                assert self._driver is not None
                self._driver.verify_connectivity()
                modo = "Neo4j"
            return True, f"Conexão Grafo OK — usando {modo}."
        except Exception as exc:
            return False, f"Erro na conexão Neo4j: {exc}"

    # ------------------------------------------------------------------
    def close(self) -> None:
        if self._driver:
            self._driver.close()
