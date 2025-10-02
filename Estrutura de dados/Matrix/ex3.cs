using System;

namespace Matrix_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            int[,] matrix = new int[N, N];

            for (int i = 0; i < N; i++)
            {
                string[] row = Console.ReadLine().Split(' ');
                for (int j = 0; j < N; j++)
                {
                    matrix[i, j] = int.Parse(row[j]);
                }
            }

            for (int i = 0; i < N; i++)
            {
                int largest = matrix[i, 0];
                for (int j = 1; j < N; j++)
                {
                    if (matrix[i, j] > largest)
                    {
                        largest = matrix[i, j];
                    }
                }
                Console.WriteLine(largest);
            }
        }
    }
}
