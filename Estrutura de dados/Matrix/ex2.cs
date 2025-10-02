using System;

namespace MatrixSumRows
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            int[,] matrix = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                string[] rowElements = Console.ReadLine().Split(' ');
                for (int j = 0; j < N; j++)
                {
                    matrix[i, j] = int.Parse(rowElements[j]);
                }
            }
            for (int i = 0; i < N; i++)
            {
                int rowSum = 0; 
                for (int j = 0; j < N; j++)
                {
                    rowSum += matrix[i, j]; 
                }
                Console.WriteLine(rowSum);
            }
        }
    }
}
