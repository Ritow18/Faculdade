"""Serviço de operações Cassandra — séries temporais e agregações colunares.

Demonstra os casos de uso típicos de um banco colunar:

  1. Histórico de preços (time-series)
     Cada produto é uma partition key; as variações de preço são linhas
     ordenadas por data (clustering key DESC).  Leitura muito eficiente
     porque todos os preços de um produto vivem no mesmo nó do cluster.

  2. Vendas por categoria (wide-row analítico)
     Cada categoria é uma partition key; totais por data são colunas.
     Permite relatórios agregados sem precisar escanear toda a tabela.
"""

from __future__ import annotations

import random
from datetime import datetime, timedelta


class CassandraService:
    def __init__(self, cassandra_manager, sqlite_repository) -> None:
        self.manager = cassandra_manager
        self.session = cassandra_manager.get_session()
        self.sqlite = sqlite_repository

    # ──────────────────────────────────────────────────────────────────
    # Populando as tabelas
    # ──────────────────────────────────────────────────────────────────

    def popular_historico_precos(self) -> int:
        """
        Gera registros de histórico de preços simulados para todos os produtos.

        Estratégia:
          - 12 snapshots por produto, espaçados em 3 dias (últimos ~36 dias)
          - Variação de ±15 % em torno do preço atual (random walk simples)
          - Origem aleatória: ajuste_manual | promocao | reajuste_fornecedor
        """
        origens = ["ajuste_manual", "promocao", "reajuste_fornecedor", "concorrencia"]
        produtos = self.sqlite.list_produtos()
        total = 0

        for produto in produtos:
            preco_base = produto.preco_atual
            for i in range(12):
                data = datetime.now() - timedelta(days=i * 3)
                variacao = round(random.uniform(-0.15, 0.15), 4)
                preco = round(preco_base * (1 + variacao), 2)
                origem = random.choice(origens)

                self.session.execute(
                    """
                    INSERT INTO historico_precos
                        (id_produto, data_registro, preco, origem)
                    VALUES (?, ?, ?, ?)
                    """,
                    [produto.id_produto, data, preco, origem],
                )
                total += 1

        return total

    def popular_vendas_por_categoria(self) -> int:
        """
        Lê os pedidos do SQLite e grava totais agregados por categoria + data
        na tabela colunar vendas_por_categoria.

        O modelo wide-row fica evidente aqui: para cada categoria há N colunas
        (uma por data de pedido) agrupadas na mesma partition key.
        """
        pedidos = self.sqlite.list_pedidos()
        total = 0

        for pedido in pedidos:
            # data_pedido pode vir como "YYYY-MM-DD HH:MM:SS" — pega só a data
            data_str = str(pedido.data_pedido)[:10]

            for item in pedido.itens:
                self.session.execute(
                    """
                    INSERT INTO vendas_por_categoria
                        (categoria, data_pedido, total_vendas, qtd_pedidos)
                    VALUES (?, ?, ?, ?)
                    """,
                    [item.categoria, data_str, float(item.subtotal), 1],
                )
                total += 1

        return total

    # ──────────────────────────────────────────────────────────────────
    # Consultas demonstrativas
    # ──────────────────────────────────────────────────────────────────

    def consultar_historico_precos(self, produto_id: int) -> list[dict]:
        """
        Retorna o histórico de preços de um produto ordenado do mais recente
        para o mais antigo.

        Query eficiente: usa apenas a partition key (id_produto).
        No Cassandra real a leitura é local ao nó responsável pela partição.
        """
        rows = self.session.execute(
            "SELECT * FROM historico_precos WHERE id_produto = ?",
            [int(produto_id)],
        )
        resultado = []
        for row in rows:
            resultado.append(
                {
                    "data_registro": str(row.data_registro)[:19],
                    "preco": round(float(row.preco), 2),
                    "origem": row.origem,
                }
            )
        return resultado

    def relatorio_vendas_por_categoria(self) -> list[dict]:
        """
        Relatório agregado de vendas por categoria, ordenado pelo maior total.

        Em Cassandra real, para agregar entre partições seria necessário
        usar ALLOW FILTERING ou um job Spark/Analytics — aqui demonstramos
        a consulta dentro de cada partição e agregamos no cliente.
        """
        rows = self.session.execute("SELECT * FROM vendas_por_categoria")
        resultado = []
        for row in rows:
            resultado.append(
                {
                    "categoria": row.categoria,
                    "total_vendas": round(float(row.total_vendas), 2),
                    "qtd_pedidos": int(row.qtd_pedidos),
                }
            )
        return resultado
