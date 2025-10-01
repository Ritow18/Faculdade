using System;

namespace Vector_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            int[] numbers = new int[N];
            string[] input = Console.ReadLine().Split(' ');

            for (int i = 0; i < N; i++)
            {
                numbers[i] = int.Parse(input[i]);
            }

            int evenCount = 0;
            for (int i = 0; i < N; i++)
            {
                if (numbers[i] % 2 == 0)
                {
                    Console.Write(numbers[i] + " ");
                    evenCount++;
                }
            }
            Console.WriteLine();
            Console.WriteLine(evenCount);
        }
    }
}
