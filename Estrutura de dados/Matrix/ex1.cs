using System;

namespace MatrixOperations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] dimensions = Console.ReadLine().Split(' ');
            int M = int.Parse(dimensions[0]); 
            int N = int.Parse(dimensions[1]); 
            int[,] matrix = new int[M, N];
            for (int i = 0; i < M; i++)
            {
                string[] row = Console.ReadLine().Split(' ');
                for (int j = 0; j < N; j++)
                {
                    matrix[i, j] = int.Parse(row[j]);
                }
            }
            Console.WriteLine("NEGATIVE VALUES:");
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] < 0)
                    {
                        Console.WriteLine(matrix[i, j]);
                    }
                }
            }
        }
    }
}
