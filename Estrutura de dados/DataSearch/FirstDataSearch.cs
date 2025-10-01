using System;

namespace algoritmoDeBusca
{

    public class staticList<T> where T : IComparable<T>
    {
        private T[] elements;
        private int size, capacity;
        public staticList(int capacity)
        {
            this.capacity = capacity;
            elements = new T[capacity];
            size = 0;
        }
        //insert
        public void insert(T Value)
        {
            if (size == capacity)
            {
                Console.WriteLine("lista cheia.");
                return;
            }
            elements[size] = Value;
            size++;
        }
        //display
        public void display()
        {
            if (size == 0)
            {
                Console.WriteLine("Lista Vazia.");
                return;
            }
            Console.WriteLine("Lista: ");
            for (int i = 0; i < size; i++)
            {
                Console.Write(elements[i] + " ");
            }
            Console.WriteLine();
        }
        //sequential search algorithm
        public int sequencialSearch(T Value)
        {
            for (int i = 0; i < size; i++)
            {
                if (elements[i].CompareTo(Value) == 0)
                {
                    return i;
                }
            }
            return -1;
        }
        //binary search algorithm
        public int binarySearch(T Value)
        {
            int start = 0;
            int end = size - 1;
            while (start <= end)
            {
                int middle = (start + end) / 2;
                if (elements[middle].CompareTo(Value) == 0)
                {
                    return middle;
                }
                if (elements[middle].CompareTo(Value) > 0)
                {
                    start = middle - 1;
                }
                else
                {
                    start = middle + 1;
                }
            }
            return -1;
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("digite a quantidade de elementos da lista: ");
            int n = int.Parse(Console.ReadLine());
            staticList<int> list = new staticList<int>(n);
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Digite o {i + 1} elemento.");
                int value = int.Parse(Console.ReadLine());
                list.insert(value);
            }
            list.display();

            Console.WriteLine("digite o valor a ser procurado.");
            int elementSearch = int.Parse(Console.ReadLine());
            int sequencialResult = list.sequencialSearch(elementSearch);
            if (sequencialResult != -1)
            {
                Console.WriteLine($"o elemento é o: {elementSearch} " +
                    $"encontrado na posição {sequencialResult} " +
                    $" - (Busca sequencial)");
            }
            else
            {
                Console.WriteLine($"Elementos {elementSearch} + " +
                    $"não encontrado - (Busca Sequencial)");
            }
            int binaryResult = list.binarySearch(elementSearch);
            if (sequencialResult != -1)
            {
                Console.WriteLine($"o elemento é o: {elementSearch} " +
                    $"encontrado na posição {sequencialResult} " +
                    $" - (Busca sequencial)");
            }
            else
            {
                Console.WriteLine($"Elementos {elementSearch} + " +
                    $"não encontrado - (Busca binária)");
            }
        }
    }
}
