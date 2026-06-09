"""Camada de View.

Nesta aplicação de console, a View é responsável por:
- exibir o menu;
- pedir opções ao usuário;
- mostrar mensagens e resultados.
"""

from __future__ import annotations

from pprint import pprint
from typing import Iterable, Any


class MenuView:
    def show_menu(self) -> None:
        print("\n" + "=" * 72)
        print(" Laboratório de BDs Não Relacionais — SQLite + MongoDB + Redis")
        print("                        + Cassandra + Neo4j")
        print("=" * 72)

        print("\n  [ SQLite ]")
        print("  1. Testar configuração do SQLite")
        print("  2. Recriar e popular SQLite")

        print("\n  [ MongoDB — Documento ]")
        print("  3. Testar configuração do MongoDB")
        print("  4. Recriar coleções Mongo com schema")
        print("  5. Migrar SQLite -> Mongo (modelo simplificado e achatado)")
        print("  6. Mostrar amostra de documentos")
        print("  7. Executar consultas de exemplo")
        print("  8. Mostrar caminhos de configuração carregados")

        print("\n  [ Redis — Chave-Valor ]")
        print("  9. Consultar produto por ID (Cache)")
        print(" 10. Adicionar item ao carrinho temporário")
        print(" 11. Visualizar carrinho de um cliente")
        print(" 12. Exibir ranking de produtos mais consultados")

        print("\n  [ Cassandra — Colunar ]")
        print(" 13. Testar conexão e criar tabelas")
        print(" 14. Popular histórico de preços e vendas por categoria")
        print(" 15. Consultar histórico de preços de um produto")
        print(" 16. Relatório de vendas por categoria")

        print("\n  [ Neo4j — Grafo ]")
        print(" 17. Testar conexão e verificar grafo")
        print(" 18. Popular grafo a partir do SQLite")
        print(" 19. Recomendar produtos (co-compras)")
        print(" 20. Explorar rede de compras de um cliente")

        print("\n  0. Sair")
        print("=" * 72)

    def ask_option(self) -> str:
        return input("Escolha uma opção: ").strip()

    def show_message(self, message: str) -> None:
        print(message)

    def show_error(self, message: str) -> None:
        print(f"[ERRO] {message}")

    def show_success(self, message: str) -> None:
        print(f"[OK] {message}")

    def show_documents(self, title: str, documents: Iterable[Any]) -> None:
        print(f"\n=== {title} ===")
        found = False
        for doc in documents:
            pprint(doc)
            found = True
        if not found:
            print("Nenhum documento encontrado.")
