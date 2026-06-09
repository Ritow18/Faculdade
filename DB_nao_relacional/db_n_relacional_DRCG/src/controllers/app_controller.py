"""Controller principal.

Responsável por coordenar View e Model.
"""

from __future__ import annotations

from pathlib import Path

from src.models.config_loader import ConfigLoader
from src.models.mongo_manager import MongoManager
from src.models.migration_service import MigrationService
from src.models.query_service import QueryService
from src.models.sqlite_manager import SQLiteManager
from src.models.sqlite_repository import SQLiteRepository
from src.views.menu_view import MenuView
from src.models.redis_manager import RedisManager
from src.models.redis_service import RedisService
from src.models.cassandra_manager import CassandraManager
from src.models.cassandra_service import CassandraService
from src.models.neo4j_manager import Neo4jManager
from src.models.neo4j_service import Neo4jService


class AppController:
    def __init__(self) -> None:
        self.project_root = Path(__file__).resolve().parents[2]

        config_loader = ConfigLoader(self.project_root)
        sqlite_cfg = config_loader.load_sqlite_config()
        mongo_cfg = config_loader.load_mongodb_config()
        cassandra_cfg = config_loader.load_cassandra_config()
        neo4j_cfg = config_loader.load_neo4j_config()

        self.sqlite_cfg = sqlite_cfg
        self.mongo_cfg = mongo_cfg
        self.cassandra_cfg = cassandra_cfg
        self.neo4j_cfg = neo4j_cfg

        # ── SQLite (fonte de verdade relacional) ──────────────────────
        self.sqlite_manager = SQLiteManager(sqlite_cfg["db_path"])
        self.sqlite_repository = SQLiteRepository(self.sqlite_manager)

        # ── MongoDB (documento) ───────────────────────────────────────
        self.mongo_manager = MongoManager(
            uri=mongo_cfg["uri"],
            database_name=mongo_cfg["database"],
            use_mongomock_on_failure=mongo_cfg["use_mongomock_on_failure"],
        )
        self.migration_service = MigrationService(self.sqlite_repository, self.mongo_manager)
        self.query_service = QueryService(self.mongo_manager)

        # ── Redis (chave-valor) ───────────────────────────────────────
        self.redis_manager = RedisManager()
        self.redis_service = RedisService(self.redis_manager, self.sqlite_repository)

        # ── Cassandra (colunar) ───────────────────────────────────────
        self.cassandra_manager = CassandraManager(
            hosts=cassandra_cfg["hosts"],
            port=cassandra_cfg["port"],
            keyspace=cassandra_cfg["keyspace"],
            use_fake_on_failure=cassandra_cfg["use_fake_on_failure"],
        )
        self.cassandra_service = CassandraService(
            self.cassandra_manager, self.sqlite_repository
        )

        # ── Neo4j (grafo) ─────────────────────────────────────────────
        self.neo4j_manager = Neo4jManager(
            uri=neo4j_cfg["uri"],
            user=neo4j_cfg["user"],
            password=neo4j_cfg["password"],
            database=neo4j_cfg["database"],
            use_fake_on_failure=neo4j_cfg["use_fake_on_failure"],
        )
        self.neo4j_service = Neo4jService(self.neo4j_manager, self.sqlite_repository)

        self.view = MenuView()

    # ──────────────────────────────────────────────────────────────────
    # Loop principal
    # ──────────────────────────────────────────────────────────────────

    def run(self) -> None:
        while True:
            try:
                self.view.show_menu()
                option = self.view.ask_option()

                # ── SQLite ────────────────────────────────────────────
                if option == "1":
                    self._test_sqlite()
                elif option == "2":
                    self._recreate_sqlite()
                # ── MongoDB ───────────────────────────────────────────
                elif option == "3":
                    self._test_mongo()
                elif option == "4":
                    self._recreate_mongo_collections()
                elif option == "5":
                    self._migrate()
                elif option == "6":
                    self._show_samples()
                elif option == "7":
                    self._run_example_queries()
                elif option == "8":
                    self._show_config_paths()
                # ── Redis ─────────────────────────────────────────────
                elif option == "9":
                    self._consultar_produto_redis()
                elif option == "10":
                    self._gerenciar_carrinho()
                elif option == "11":
                    self._visualizar_carrinho()
                elif option == "12":
                    self._ver_ranking()
                # ── Cassandra (Colunar) ───────────────────────────────
                elif option == "13":
                    self._test_cassandra()
                elif option == "14":
                    self._popular_cassandra()
                elif option == "15":
                    self._historico_precos_cassandra()
                elif option == "16":
                    self._relatorio_categorias_cassandra()
                # ── Neo4j (Grafo) ─────────────────────────────────────
                elif option == "17":
                    self._test_neo4j()
                elif option == "18":
                    self._popular_grafo()
                elif option == "19":
                    self._recomendar_produtos_grafo()
                elif option == "20":
                    self._explorar_rede_cliente_grafo()
                # ── Sair ──────────────────────────────────────────────
                elif option == "0":
                    self.view.show_message("Encerrando o sistema.")
                    break
                else:
                    self.view.show_error("Opção inválida.")
            except Exception as exc:
                self.view.show_error(str(exc))

    # ──────────────────────────────────────────────────────────────────
    # Handlers — Redis
    # ──────────────────────────────────────────────────────────────────

    def _consultar_produto_redis(self):
        prod_id = input("Digite o ID do produto: ")
        produto = self.redis_service.buscar_produto_com_cache(prod_id)
        if produto:
            print(
                f"Produto Encontrado: {produto['nome']} "
                f"| Preço: R${produto['preco_atual']} "
                f"| Estoque: {produto['estoque_total']}"
            )
        else:
            print("Produto não encontrado.")

    def _gerenciar_carrinho(self):
        cliente_id = input("ID do Cliente: ")
        produto_id = input("ID do Produto: ")
        qtd = int(input("Quantidade: "))
        sucesso, msg = self.redis_service.adicionar_ao_carrinho(cliente_id, produto_id, qtd)
        print(msg)

    def _visualizar_carrinho(self):
        cliente_id = input("ID do Cliente: ")
        itens, total = self.redis_service.ver_carrinho(cliente_id)
        print(f"\n--- Carrinho do Cliente {cliente_id} ---")
        if not itens:
            print("Carrinho vazio ou expirado.")
        else:
            for item in itens:
                print(
                    f"- {item['nome']} (Qtd: {item['quantidade']}) "
                    f"-> Subtotal: R${item['subtotal']:.2f}"
                )
            print(f"TOTAL: R${total:.2f}")
        print("-----------------------------------")

    def _ver_ranking(self):
        ranking = self.redis_service.ver_ranking()
        print("\n--- Ranking de Consultas ---")
        if not ranking:
            print("Nenhuma consulta registrada ainda.")
        else:
            for i, p in enumerate(ranking, 1):
                print(f"{i}º | {p['nome']} (R${p['preco']}) - {p['total_consultas']} consultas")

    # ──────────────────────────────────────────────────────────────────
    # Handlers — Cassandra (Colunar)
    # ──────────────────────────────────────────────────────────────────

    def _test_cassandra(self):
        ok, message = self.cassandra_manager.test_connection()
        if ok:
            self.view.show_success(message)
            if self.cassandra_manager.using_fake:
                self.view.show_message(
                    "Observação: usando FakeCassandraSession em memória; "
                    "o comportamento distribuído real do Cassandra não é emulado."
                )
        else:
            self.view.show_error(message)

    def _popular_cassandra(self):
        self.view.show_message("Populando Cassandra com histórico de preços e vendas...")
        total_precos = self.cassandra_service.popular_historico_precos()
        total_vendas = self.cassandra_service.popular_vendas_por_categoria()
        self.view.show_success(
            f"Cassandra populado. "
            f"Registros de preço: {total_precos} | "
            f"Itens de venda por categoria: {total_vendas}"
        )

    def _historico_precos_cassandra(self):
        prod_id = input("ID do produto para consultar histórico de preços: ")
        historico = self.cassandra_service.consultar_historico_precos(int(prod_id))
        print(f"\n--- Histórico de preços — Produto {prod_id} ---")
        if not historico:
            print("Nenhum registro encontrado. Popule os dados primeiro (opção 14).")
        else:
            for r in historico:
                print(
                    f"  {r['data_registro']}  |  R$ {r['preco']:.2f}  |  {r['origem']}"
                )
        print("------------------------------------------------")

    def _relatorio_categorias_cassandra(self):
        relatorio = self.cassandra_service.relatorio_vendas_por_categoria()
        print("\n--- Relatório Colunar: Vendas por Categoria ---")
        if not relatorio:
            print("Sem dados. Popule o Cassandra primeiro (opção 14).")
        else:
            print(f"  {'Categoria':<30} {'Total Vendas':>14} {'Qtd Pedidos':>12}")
            print("  " + "-" * 58)
            for r in relatorio:
                print(
                    f"  {r['categoria']:<30} "
                    f"R$ {r['total_vendas']:>11.2f} "
                    f"{r['qtd_pedidos']:>12}"
                )
        print("------------------------------------------------")

    # ──────────────────────────────────────────────────────────────────
    # Handlers — Neo4j (Grafo)
    # ──────────────────────────────────────────────────────────────────

    def _test_neo4j(self):
        ok, message = self.neo4j_manager.test_connection()
        if ok:
            self.view.show_success(message)
            if self.neo4j_manager.using_fake:
                self.view.show_message(
                    "Observação: usando FakeGraphSession com networkx em memória; "
                    "as queries Cypher reais do Neo4j não são executadas."
                )
        else:
            self.view.show_error(message)

    def _popular_grafo(self):
        self.view.show_message("Construindo grafo de e-commerce a partir do SQLite...")
        resultado = self.neo4j_service.popular_grafo()
        stats = self.neo4j_service.estatisticas_grafo()
        self.view.show_success(
            f"Grafo populado. "
            f"Produtos: {resultado.get('produtos')} | "
            f"Clientes: {resultado.get('clientes')} | "
            f"Pedidos: {resultado.get('pedidos')} | "
            f"Arestas: {resultado.get('arestas', 'N/A')} | "
            f"Total nós: {stats['total_nos']} | "
            f"Total arestas: {stats['total_arestas']}"
        )

    def _recomendar_produtos_grafo(self):
        prod_id = input("ID do produto-base para recomendação: ")
        recomendacoes = self.neo4j_service.recomendar_produtos(int(prod_id), limit=5)
        print(f"\n--- Recomendações para o Produto {prod_id} (co-compras) ---")
        if not recomendacoes:
            print(
                "Nenhuma recomendação encontrada. "
                "Popule o grafo primeiro (opção 18) e use um ID válido."
            )
        else:
            for i, r in enumerate(recomendacoes, 1):
                print(
                    f"  {i}. {r['nome']}  (ID: {r['id_produto']}) "
                    f"| R$ {r['preco']:.2f} "
                    f"| Co-compras: {r['co_compras']}"
                )
        print("----------------------------------------------------------")

    def _explorar_rede_cliente_grafo(self):
        cliente_id = input("ID do cliente para explorar a rede: ")
        compras = self.neo4j_service.explorar_rede_cliente(int(cliente_id))
        print(f"\n--- Rede do Cliente {cliente_id} (Pedido → Produto) ---")
        if not compras:
            print(
                "Nenhuma compra encontrada. "
                "Popule o grafo primeiro (opção 18) e use um ID válido."
            )
        else:
            pedido_atual = None
            for item in compras:
                if item["pedido_id"] != pedido_atual:
                    pedido_atual = item["pedido_id"]
                    print(f"\n  Pedido #{pedido_atual}:")
                print(
                    f"    └─ {item['nome']}  (ID: {item['id_produto']}) "
                    f"| R$ {item['preco']:.2f}"
                )
        print("------------------------------------------------------")

    # ──────────────────────────────────────────────────────────────────
    # Handlers — SQLite
    # ──────────────────────────────────────────────────────────────────

    def _test_sqlite(self) -> None:
        ok, message = self.sqlite_manager.test_connection()
        if ok:
            self.view.show_success(message)
        else:
            self.view.show_error(message)

    def _recreate_sqlite(self) -> None:
        self.view.show_message("Recriando e populando o SQLite...")
        self.sqlite_manager.recreate_database()
        self.view.show_success("SQLite recriado e populado com dados fictícios.")

    # ──────────────────────────────────────────────────────────────────
    # Handlers — MongoDB
    # ──────────────────────────────────────────────────────────────────

    def _test_mongo(self) -> None:
        ok, message = self.mongo_manager.test_connection()
        if ok:
            self.view.show_success(message)
            if self.mongo_manager.using_mock:
                self.view.show_message(
                    "Observação: o sistema está usando mongomock em memória; "
                    "validators não serão aplicados."
                )
        else:
            self.view.show_error(message)

    def _recreate_mongo_collections(self) -> None:
        message = self.mongo_manager.recreate_collections_with_schema()
        self.view.show_success(message)

    def _migrate(self) -> None:
        result = self.migration_service.migrate()
        self.view.show_success(
            "Migração concluída. "
            f"Produtos: {result['produtos']} | "
            f"Documentos em pedidos: {result['pedidos']} | "
            f"Pedidos de origem: {result['pedidos_origem']}"
        )

    def _show_samples(self) -> None:
        self.view.show_documents("Produtos (amostra)", self.query_service.sample_produtos())
        self.view.show_documents(
            "Pedidos simplificados (1 documento por item)",
            self.query_service.sample_pedidos(),
        )

    def _run_example_queries(self) -> None:
        self.view.show_documents(
            "[1] Pedidos com status pago", self.query_service.example_filter_paid_orders()
        )
        self.view.show_documents(
            "[2] Pedidos cuja cidade de entrega seja Curitiba",
            self.query_service.example_projection_curitiba(),
        )
        updated = self.query_service.example_update_first_order()
        if updated is None:
            self.view.show_message("[3] Nenhum pedido encontrado para atualizar.")
        else:
            self.view.show_documents("[3] Atualização do primeiro pedido", [updated])
        self.view.show_documents(
            "[4] Produtos mais vendidos",
            self.query_service.example_aggregation_best_sellers(),
        )
        self.view.show_documents(
            "[5] Paginação de pedidos: página 2, tamanho 5",
            self.query_service.example_pagination(),
        )

    def _show_config_paths(self) -> None:
        self.view.show_message("\nArquivos de configuração carregados:")
        self.view.show_message(f"SQLite ini     : {self.sqlite_cfg['source_file']}")
        self.view.show_message(f"SQLite db      : {self.sqlite_cfg['db_path']}")
        self.view.show_message(f"Mongo ini      : {self.mongo_cfg['source_file']}")
        self.view.show_message(f"Mongo uri      : {self.mongo_cfg['uri']}")
        self.view.show_message(f"Database       : {self.mongo_cfg['database']}")
        self.view.show_message(f"Cassandra ini  : {self.cassandra_cfg['source_file']}")
        self.view.show_message(f"Cassandra hosts: {self.cassandra_cfg['hosts']}")
        self.view.show_message(f"Cassandra ks   : {self.cassandra_cfg['keyspace']}")
        self.view.show_message(f"Neo4j ini      : {self.neo4j_cfg['source_file']}")
        self.view.show_message(f"Neo4j uri      : {self.neo4j_cfg['uri']}")
        self.view.show_message(f"Neo4j database : {self.neo4j_cfg['database']}")
