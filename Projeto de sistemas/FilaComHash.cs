using System;
using System.Collections.Generic;
using System.Linq;

namespace ex_proj_aula7
{
    class Program
    {
        class Fila
        {
            private HashSet<char> elementosSet; 
            private Dictionary<int, char> elementosMap;
            private int inicio, fim, tamanho, capacidade;
            private int currentIndex; 

            public Fila(int capacidade)
            {
                this.capacidade = capacidade;
                elementosSet = new HashSet<char>();
                elementosMap = new Dictionary<int, char>();
                inicio = 0;
                fim = -1;
                tamanho = 0;
                currentIndex = 0;
            }
            public bool Empty()
            {
                return tamanho == 0;
            }

            public void Enqueue(char carac)
            {
                if (tamanho == capacidade)
                {
                    Console.WriteLine("Fila cheia");
                    return;
                }
                if (elementosSet.Contains(carac))
                {
                    Console.WriteLine($"Elemento '{carac}' já está na fila");
                    return;
                }

                fim = (fim + 1) % capacidade;
                currentIndex++;
                elementosMap[currentIndex] = carac;
                elementosSet.Add(carac);
                tamanho++;
            }
            public char Dequeue()
            {
                if (Empty())
                {
                    Console.WriteLine("A fila está vazia!");
                    return '\0';
                }
                var oldest = elementosMap.OrderBy(kvp => kvp.Key).First();
                char valor = oldest.Value;
                elementosMap.Remove(oldest.Key);
                elementosSet.Remove(valor);
                inicio = (inicio + 1) % capacidade;
                tamanho--;
                return valor;
            }
            public void Exibir()
            {
                if (Empty())
                {
                    Console.WriteLine("A fila está vazia!");
                    return;
                }
                Console.WriteLine("Fila: ");
                foreach (var item in elementosMap.OrderBy(kvp => kvp.Key))
                {
                    Console.Write(item.Value + " ");
                }
                Console.WriteLine();
            }
            public int Size()
            {
                return tamanho;
            }
            public char Head()
            {
                if (Empty())
                {
                    Console.WriteLine("A fila está vazia!");
                    return '\0';
                }
                return elementosMap.OrderBy(kvp => kvp.Key).First().Value;
            }
        }
        static void Main(string[] args)
        {
            Fila fila = new Fila(10);
            char valor, opcao;
            do
            {
                Console.WriteLine("\nEscolha uma das seguintes opções: ");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("1 - Enfileirar");
                Console.WriteLine("2 - Head");
                Console.WriteLine("3 - Desenfileirar");
                Console.WriteLine("4 - Tamanho da fila");
                Console.WriteLine("5 - Exibir elementos da fila");
                opcao = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (opcao)
                {
                    case '0':
                        Console.WriteLine("Finalizando o sistema...");
                        break;
                    case '1':
                        Console.WriteLine("Digite o valor para entrar na fila");
                        valor = Console.ReadKey().KeyChar;
                        fila.Enqueue(valor);
                        break;
                    case '2':
                        valor = fila.Head();
                        Console.WriteLine("Inicio da fila: " + valor);
                        break;
                    case '3':
                        valor = fila.Dequeue();
                        Console.WriteLine("Elemento desenfileirado: " + valor);
                        break;
                    case '4':
                        Console.WriteLine("Quantidade de elementos da fila: " + fila.Size());
                        break;
                    case '5':
                        fila.Exibir();
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }
            } while (opcao != '0');
        }
    }
}
