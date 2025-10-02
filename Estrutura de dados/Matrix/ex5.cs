using System;

namespace Matrix_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int rows, cols;
            int[,] matrixA, matrixB, matrixC;
            string[] values;

            values = Console.ReadLine().Split(' ');
            rows = int.Parse(values[0]);
            cols = int.Parse(values[1]);

            matrixA = new int[rows, cols];
            matrixB = new int[rows, cols];
            matrixC = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                values = Console.ReadLine().Split(' ');
                for (int j = 0; j < cols; j++)
                {
                    matrixA[i, j] = int.Parse(values[j]);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                values = Console.ReadLine().Split(' ');
                for (int j = 0; j < cols; j++)
                {
                    matrixB[i, j] = int.Parse(values[j]);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixC[i, j] = matrixA[i, j] + matrixB[i, j];
                    Console.Write(matrixC[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
