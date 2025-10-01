using System;

namespace Vector_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            double[] vector = new double[N];
            string[] input = Console.ReadLine().Split(' ');

            for (int i = 0; i < N; i++)
            {
                vector[i] = double.Parse(input[i]);
            }

            double sum = 0.0;
            for (int i = 0; i < N; i++)
            {
                sum += vector[i];
            }

            double average = sum / N;
            Console.WriteLine(average.ToString("F3"));

            for (int i = 0; i < N; i++)
            {
                if (vector[i] < average)
                {
                    Console.WriteLine(vector[i].ToString("F1"));
                }
            }
        }
    }
}
