using System;

namespace StaticList
{
    public class Lista
    {
        private char[] elementos;
        private int tamanho;
        private int capacidade;
        public Lista(int capacidade)
        {
            this.capacidade = capacidade;
            this.elementos = new char[capacidade];
            tamanho = 0;
        }
        public void Inserir(char valor)
        {
            if (tamanho == capacidade)
            {
                Console.WriteLine("Lista cheia!");
                return;
            }
            elementos[tamanho] = valor;
            tamanho++;
        }

        public void InserirNaPosicao(int pos, char valor) 
        {
            if (tamanho == capacidade)
            {
                Console.WriteLine("Lista cheia!");
                return;
            }
            if (pos < 0 || pos > tamanho)
            {
                Console.WriteLine("Posição invalida!");
                return;
            }

            for (int i = tamanho; i > pos; i--)
            {
                elementos[i] = elementos[i - 1];
            }
            
            elementos[pos] = valor;
            tamanho++;
        }
        public void Remover(int pos)
        {
            if (pos < 0 || pos >= tamanho)
            {
                Console.WriteLine("Posição invalida!");
                return;
            }

            for (int i = pos; i < tamanho - 1; i++)
            {
                elementos[i] = elementos[i + 1];
            }
            tamanho--;
        }
        public void Exibir()
        {
            if (tamanho == 0)
            {
                Console.WriteLine("Lista vazia");
                return;
            }
            Console.Write("Lista: ");
            for (int i = 0; i < tamanho; i++)
            {
                Console.Write(elementos[i] + " ");
            }
            Console.WriteLine();
        }
        public int Size()
        {
            return tamanho;
        }
        public bool Empty()
        {
            return tamanho == 0;
        }
        public char ElementosNaPosicao(int pos)
        {
            Console.WriteLine("Posição invalida!");
            return '\0';
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                Lista Lista = new Lista(10);
                int opcao;
                char valor;
                int pos;

                do
                {
                    Console.WriteLine("\nLISTA LINEAR ESTATICA");
                    Console.WriteLine("0. Sair");
                    Console.WriteLine("1. Inserir no final");
                    Console.WriteLine("2. Inserir na posição especifica");
                    Console.WriteLine("3. Remover por posição");
                    Console.WriteLine("4. Tamanho da Lista");
                    Console.WriteLine("5. Exibir lista");
                    Console.WriteLine("6. Exibir elementos na posição");
                    Console.WriteLine("Escolha uma opção: ");
                    opcao = int.Parse(Console.ReadLine());

                    switch (opcao)
                    {
                        case 0:
                            Console.WriteLine("Saindo...");
                            break;

                        case 1:
                            Console.WriteLine("Digite o valor");
                            valor = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            Lista.Inserir(valor);
                            break;

                        case 2:
                            Console.WriteLine("Digite a posição");
                            pos = int.Parse(Console.ReadLine());
                            Console.WriteLine("Digite o valor: ");
                            valor = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            Lista.InserirNaPosicao(pos, valor);
                            break;

                        case 3:
                            Console.WriteLine("Digite a posicão para remover");
                            pos = int.Parse(Console.ReadLine());
                            Lista.Remover(pos);
                            break;

                        case 4:
                            Console.WriteLine("Tamanho atual: " + Lista.Size());
                            break;

                        case 5:
                            Lista.Exibir();
                            break;

                        case 6:
                            Console.WriteLine("Digite a posição: ");
                            pos = int.Parse(Console.ReadLine());
                            valor = Lista.ElementosNaPosicao(pos);
                            Console.WriteLine("Elementos: " + valor);
                            break;

                        default:
                            Console.WriteLine("Opçãp invalida!");
                            break;
                    }

                } while (opcao != 0);
            }
        }
    }
}
