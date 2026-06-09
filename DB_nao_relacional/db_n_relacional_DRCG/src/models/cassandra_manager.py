"""Gerenciador de conexão com Apache Cassandra.

Tenta conectar ao servidor real; se indisponível e o fallback estiver ativo,
usa FakeCassandraSession — um emulador em memória que imita a interface do
driver oficial para as queries específicas deste projeto.

Conceito colunar demonstrado:
  - Dados organizados por famílias de colunas (Column Families / Tables)
  - Partition key define o nó de armazenamento; clustering columns a ordenação
  - Ideal para séries temporais e leituras analíticas em colunas específicas
"""

from __future__ import annotations

import logging
from collections import defaultdict
from datetime import datetime


# ─────────────────────────────────────────────────────────────────────────────
# Emulação em memória (FakeCassandra)
# ─────────────────────────────────────────────────────────────────────────────

class _FakeRow:
    """Simula uma linha retornada pelo cassandra-driver (acesso por atributo)."""

    def __init__(self, data: dict) -> None:
        for key, value in data.items():
            setattr(self, key, value)

    def __repr__(self) -> str:
        return f"FakeRow({self.__dict__})"


class _FakeResult:
    """Simula o ResultSet do cassandra-driver."""

    def __init__(self, rows: list[dict]) -> None:
        self._rows = [_FakeRow(r) for r in rows]

    def __iter__(self):
        return iter(self._rows)

    def __len__(self) -> int:
        return len(self._rows)


class FakeCassandraSession:
    """
    Sessão Cassandra em memória.

    Armazena duas tabelas internas e interpreta as queries CQL do projeto
    sem depender de um cluster real. Respeita a mesma interface pública
    do objeto Session do cassandra-driver:
        session.execute(query, params)
        session.prepare(query)   → devolve a query sem alteração
    """

    def __init__(self) -> None:
        # historico_precos: list de dicts com chaves
        #   id_produto, data_registro, preco, origem
        self._historico_precos: list[dict] = []

        # vendas_por_categoria: aggregação em memória
        #   chave: (categoria, data_pedido) → {total_vendas, qtd_pedidos}
        self._vendas_por_categoria: dict[tuple, dict] = defaultdict(
            lambda: {"total_vendas": 0.0, "qtd_pedidos": 0}
        )

    # ------------------------------------------------------------------
    def prepare(self, query: str) -> str:
        """Compatibilidade com cassandra-driver: devolve a query sem alteração."""
        return query

    # ------------------------------------------------------------------
    def execute(self, query, params=None) -> _FakeResult:
        params = list(params) if params else []
        q = str(query).strip().upper()

        # DDL (CREATE / DROP / USE / ALTER) → sem-op
        if q.startswith(("CREATE", "DROP", "USE", "ALTER")):
            return _FakeResult([])

        # INSERT INTO historico_precos
        if "INSERT" in q and "HISTORICO_PRECOS" in q:
            self._historico_precos.append(
                {
                    "id_produto": int(params[0]),
                    "data_registro": str(params[1]),
                    "preco": float(params[2]),
                    "origem": str(params[3]),
                }
            )
            return _FakeResult([])

        # INSERT INTO vendas_por_categoria  (upsert por (categoria, data))
        if "INSERT" in q and "VENDAS_POR_CATEGORIA" in q:
            key = (str(params[0]), str(params[1]))
            self._vendas_por_categoria[key]["total_vendas"] += float(params[2])
            self._vendas_por_categoria[key]["qtd_pedidos"] += int(params[3])
            return _FakeResult([])

        # SELECT historico_precos WHERE id_produto = ?
        if "SELECT" in q and "HISTORICO_PRECOS" in q and params:
            prod_id = int(params[0])
            rows = [r for r in self._historico_precos if r["id_produto"] == prod_id]
            rows.sort(key=lambda r: r["data_registro"], reverse=True)
            return _FakeResult(rows)

        # SELECT vendas_por_categoria  (agregação por categoria)
        if "SELECT" in q and "VENDAS_POR_CATEGORIA" in q:
            agg: dict[str, dict] = defaultdict(lambda: {"total_vendas": 0.0, "qtd_pedidos": 0})
            for (cat, _data), vals in self._vendas_por_categoria.items():
                agg[cat]["total_vendas"] += vals["total_vendas"]
                agg[cat]["qtd_pedidos"] += vals["qtd_pedidos"]
            rows = [
                {
                    "categoria": cat,
                    "total_vendas": round(v["total_vendas"], 2),
                    "qtd_pedidos": v["qtd_pedidos"],
                }
                for cat, v in sorted(agg.items(), key=lambda x: -x[1]["total_vendas"])
            ]
            return _FakeResult(rows)

        return _FakeResult([])


# ─────────────────────────────────────────────────────────────────────────────
# Manager principal
# ─────────────────────────────────────────────────────────────────────────────

class CassandraManager:
    """
    Encapsula a conexão com o Apache Cassandra.

    Ao instanciar, tenta conectar ao cluster real; em caso de falha e com
    use_fake_on_failure=True, substitui transparentemente pelo
    FakeCassandraSession em memória.
    """

    def __init__(
        self,
        hosts: list[str] | None = None,
        port: int = 9042,
        keyspace: str = "ecommerce",
        use_fake_on_failure: bool = True,
    ) -> None:
        self.hosts = hosts or ["localhost"]
        self.port = port
        self.keyspace = keyspace
        self.use_fake_on_failure = use_fake_on_failure

        self._session = None
        self.using_fake = False

        self._connect()

    # ------------------------------------------------------------------
    def _connect(self) -> None:
        try:
            from cassandra.cluster import Cluster  # type: ignore

            cluster = Cluster(self.hosts, port=self.port)
            session = cluster.connect()
            session.execute(
                f"""
                CREATE KEYSPACE IF NOT EXISTS {self.keyspace}
                WITH replication = {{'class': 'SimpleStrategy', 'replication_factor': 1}}
                """
            )
            session.set_keyspace(self.keyspace)
            self._session = session
            self.using_fake = False
            logging.info("Conectado ao Apache Cassandra com sucesso.")
        except Exception as exc:
            logging.warning("Falha ao conectar ao Cassandra real: %s", exc)
            if self.use_fake_on_failure:
                logging.info("Iniciando FakeCassandraSession (modo mock em memória).")
                self._session = FakeCassandraSession()
                self.using_fake = True
            else:
                raise RuntimeError(f"Cassandra indisponível e fallback desativado: {exc}") from exc

    # ------------------------------------------------------------------
    def get_session(self):
        return self._session

    # ------------------------------------------------------------------
    def create_tables(self) -> None:
        """
        Cria (ou verifica) as tabelas do projeto.

        Tabela 1 — historico_precos
            Partition key : id_produto
            Clustering key: data_registro DESC
            → Série temporal de preços; query eficiente por produto.

        Tabela 2 — vendas_por_categoria
            Partition key : categoria
            Clustering key: data_pedido DESC
            → Wide-row por categoria; query analítica sem full-scan.
        """
        s = self._session

        # DDL executado no Fake ou no Cassandra real sem diferença de interface
        s.execute(
            """
            CREATE TABLE IF NOT EXISTS historico_precos (
                id_produto    INT,
                data_registro TIMESTAMP,
                preco         DECIMAL,
                origem        TEXT,
                PRIMARY KEY   (id_produto, data_registro)
            ) WITH CLUSTERING ORDER BY (data_registro DESC)
            """
        )
        s.execute(
            """
            CREATE TABLE IF NOT EXISTS vendas_por_categoria (
                categoria    TEXT,
                data_pedido  DATE,
                total_vendas DECIMAL,
                qtd_pedidos  INT,
                PRIMARY KEY  (categoria, data_pedido)
            ) WITH CLUSTERING ORDER BY (data_pedido DESC)
            """
        )

    # ------------------------------------------------------------------
    def test_connection(self) -> tuple[bool, str]:
        try:
            self.create_tables()
            modo = "FakeCassandraSession (memória)" if self.using_fake else "Apache Cassandra"
            return True, f"Conexão Cassandra OK — usando {modo}. Tabelas criadas/verificadas."
        except Exception as exc:
            return False, f"Erro na conexão Cassandra: {exc}"
