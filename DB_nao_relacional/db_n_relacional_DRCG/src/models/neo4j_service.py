"""Serviço de operações de grafo — população, recomendações e análise de rede.

Demonstra os casos de uso típicos de um banco de grafos:

  1. Modelagem de relacionamentos
     Nós: Cliente, Produto, Pedido, Categoria
     Arestas: REALIZOU | CONTEM | PERTENCE_A

  2. Recomendação por co-compra
     "Quem comprou X também comprou Y" — pattern matching de 2 saltos.

  3. Exploração de rede de um cliente
     Navega Cliente → Pedido → Produto em dois níveis de aresta.
"""

from __future__ import annotations


class Neo4jService:
    def __init__(self, neo4j_manager, sqlite_repository) -> None:
        self.manager = neo4j_manager
        self.sqlite = sqlite_repository

    # ──────────────────────────────────────────────────────────────────
    # Populando o grafo
    # ──────────────────────────────────────────────────────────────────

    def popular_grafo(self) -> dict:
        """
        Constrói o grafo de e-commerce a partir dos dados do SQLite.
        Delega para o backend correto (Neo4j real ou FakeGraph).
        """
        if self.manager.using_fake:
            return self._popular_grafo_fake()
        return self._popular_grafo_neo4j()

    # ------------------------------------------------------------------
    def _popular_grafo_fake(self) -> dict:
        """
        Popula o FakeGraphSession (networkx) com nós e arestas.

        Estrutura criada:
            (:Produto)  -[:PERTENCE_A]-> (:Categoria)
            (:Cliente)  -[:REALIZOU]->   (:Pedido)
            (:Pedido)   -[:CONTEM]->     (:Produto)
        """
        session = self.manager.get_fake_session()
        produtos = self.sqlite.list_produtos()
        pedidos = self.sqlite.list_pedidos()

        produtos_nos = 0
        clientes_nos: set[int] = set()
        pedidos_nos = 0
        arestas = 0

        # ── Produtos e Categorias ──────────────────────────────────────
        for p in produtos:
            session.add_node("Produto", id=p.id_produto, nome=p.nome, preco=p.preco_atual)
            prod_nid = f"Produto:{p.id_produto}"
            for cat in p.categorias:
                cat_nid = session.add_node("Categoria", id=cat, nome=cat)
                session.add_edge(prod_nid, cat_nid, "PERTENCE_A")
                arestas += 1
            produtos_nos += 1

        # ── Clientes, Pedidos e Itens ──────────────────────────────────
        for pedido in pedidos:
            cli = pedido.cliente

            if cli.id_cliente not in clientes_nos:
                session.add_node(
                    "Cliente",
                    id=cli.id_cliente,
                    nome=cli.nome,
                    email=cli.email,
                )
                clientes_nos.add(cli.id_cliente)

            session.add_node(
                "Pedido",
                id=pedido.id_pedido,
                status=pedido.status_pedido,
                valor_total=pedido.valor_total,
                data=pedido.data_pedido,
            )

            cli_nid = f"Cliente:{cli.id_cliente}"
            ped_nid = f"Pedido:{pedido.id_pedido}"

            session.add_edge(cli_nid, ped_nid, "REALIZOU")
            arestas += 1

            for item in pedido.itens:
                prod_nid = f"Produto:{item.id_produto}"
                session.add_edge(ped_nid, prod_nid, "CONTEM", quantidade=item.quantidade)
                arestas += 1

            pedidos_nos += 1

        return {
            "produtos": produtos_nos,
            "clientes": len(clientes_nos),
            "pedidos": pedidos_nos,
            "arestas": arestas,
        }

    # ------------------------------------------------------------------
    def _popular_grafo_neo4j(self) -> dict:
        """
        Popula o Neo4j real usando Cypher (MERGE idempotente).
        """
        # Constraints de unicidade
        for constraint in [
            "CREATE CONSTRAINT IF NOT EXISTS FOR (n:Produto)  REQUIRE n.id IS UNIQUE",
            "CREATE CONSTRAINT IF NOT EXISTS FOR (n:Cliente)  REQUIRE n.id IS UNIQUE",
            "CREATE CONSTRAINT IF NOT EXISTS FOR (n:Pedido)   REQUIRE n.id IS UNIQUE",
            "CREATE CONSTRAINT IF NOT EXISTS FOR (n:Categoria) REQUIRE n.nome IS UNIQUE",
        ]:
            self.manager.run_cypher(constraint)

        produtos = self.sqlite.list_produtos()
        pedidos = self.sqlite.list_pedidos()

        # ── Produtos e Categorias ──────────────────────────────────────
        for p in produtos:
            self.manager.run_cypher(
                "MERGE (n:Produto {id: $id}) SET n.nome = $nome, n.preco = $preco",
                id=p.id_produto, nome=p.nome, preco=p.preco_atual,
            )
            for cat in p.categorias:
                self.manager.run_cypher(
                    """
                    MERGE (c:Categoria {nome: $cat})
                    WITH c
                    MATCH (p:Produto {id: $prod_id})
                    MERGE (p)-[:PERTENCE_A]->(c)
                    """,
                    cat=cat, prod_id=p.id_produto,
                )

        # ── Clientes, Pedidos e Itens ──────────────────────────────────
        clientes_inseridos: set[int] = set()
        for pedido in pedidos:
            cli = pedido.cliente
            if cli.id_cliente not in clientes_inseridos:
                self.manager.run_cypher(
                    "MERGE (c:Cliente {id: $id}) SET c.nome = $nome, c.email = $email",
                    id=cli.id_cliente, nome=cli.nome, email=cli.email,
                )
                clientes_inseridos.add(cli.id_cliente)

            self.manager.run_cypher(
                """
                MERGE (o:Pedido {id: $id})
                SET o.status = $status, o.valor_total = $vt, o.data = $data
                """,
                id=pedido.id_pedido, status=pedido.status_pedido,
                vt=pedido.valor_total, data=pedido.data_pedido,
            )
            self.manager.run_cypher(
                """
                MATCH (c:Cliente {id: $cli_id}), (o:Pedido {id: $ped_id})
                MERGE (c)-[:REALIZOU]->(o)
                """,
                cli_id=cli.id_cliente, ped_id=pedido.id_pedido,
            )
            for item in pedido.itens:
                self.manager.run_cypher(
                    """
                    MATCH (o:Pedido {id: $ped_id}), (p:Produto {id: $prod_id})
                    MERGE (o)-[:CONTEM {quantidade: $qtd}]->(p)
                    """,
                    ped_id=pedido.id_pedido,
                    prod_id=item.id_produto,
                    qtd=item.quantidade,
                )

        return {
            "produtos": len(produtos),
            "clientes": len(clientes_inseridos),
            "pedidos": len(pedidos),
        }

    # ──────────────────────────────────────────────────────────────────
    # Consultas de grafo
    # ──────────────────────────────────────────────────────────────────

    def recomendar_produtos(self, produto_id: int, limit: int = 5) -> list[dict]:
        """
        Recomenda produtos co-comprados com o produto informado.

        Algoritmo:
          Produto ← (CONTEM) ← Pedido → (CONTEM) → Produto diferente
          Ordena pela frequência de co-ocorrência nos pedidos.
        """
        if self.manager.using_fake:
            return self.manager.get_fake_session().find_co_purchased(produto_id, limit=limit)

        results = self.manager.run_cypher(
            """
            MATCH (alvo:Produto {id: $id})<-[:CONTEM]-(o:Pedido)-[:CONTEM]->(outro:Produto)
            WHERE outro.id <> $id
            RETURN outro.id   AS id_produto,
                   outro.nome AS nome,
                   outro.preco AS preco,
                   COUNT(o)   AS co_compras
            ORDER BY co_compras DESC
            LIMIT $limit
            """,
            id=produto_id, limit=limit,
        )
        return results

    def explorar_rede_cliente(self, cliente_id: int) -> list[dict]:
        """
        Retorna todos os produtos comprados por um cliente, percorrendo
        dois níveis de relacionamentos no grafo.
        """
        if self.manager.using_fake:
            return self.manager.get_fake_session().cliente_compras_path(cliente_id)

        results = self.manager.run_cypher(
            """
            MATCH (c:Cliente {id: $id})-[:REALIZOU]->(o:Pedido)-[:CONTEM]->(p:Produto)
            RETURN o.id  AS pedido_id,
                   p.id  AS id_produto,
                   p.nome AS nome,
                   p.preco AS preco
            ORDER BY o.id
            """,
            id=cliente_id,
        )
        return results

    def estatisticas_grafo(self) -> dict:
        """Retorna contagens de nós e arestas do grafo atual."""
        if self.manager.using_fake:
            s = self.manager.get_fake_session()
            return {"total_nos": s.node_count(), "total_arestas": s.edge_count()}

        nos = self.manager.run_cypher("MATCH (n) RETURN COUNT(n) AS total")[0]["total"]
        arestas = self.manager.run_cypher(
            "MATCH ()-[r]->() RETURN COUNT(r) AS total"
        )[0]["total"]
        return {"total_nos": nos, "total_arestas": arestas}
