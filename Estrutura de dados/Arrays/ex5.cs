using System;

namespace Vector_5
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

            int sum = 0;
            int evenCount = 0;

            for (int i = 0; i < N; i++)
            {
                if (numbers[i] % 2 == 0)
                {
                    sum += numbers[i];
                    evenCount++;
                }
            }

            if (evenCount > 0)
            {
                double average = (double)sum / evenCount;
                Console.WriteLine(average.ToString("F1"));
            }
            else
            {
                Console.WriteLine("No even numbers were entered.");
            }
        }
    }
}
