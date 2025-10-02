using System;

namespace Matrix_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] dimensions = Console.ReadLine().Split(' ');
            int M = int.Parse(dimensions[0]);
            int N = int.Parse(dimensions[1]);
            int[,] platoon = new int[M, N];

            for (int i = 0; i < M; i++)
            {
                string[] row = Console.ReadLine().Split(' ');
                for (int j = 0; j < N; j++)
                {
                    platoon[i, j] = int.Parse(row[j]);
                }
            }

            int rowToRotate = int.Parse(Console.ReadLine()) - 1;
            int lastElement = platoon[rowToRotate, N - 1];

            for (int j = N - 1; j > 0; j--)
            {
                platoon[rowToRotate, j] = platoon[rowToRotate, j - 1];
            }
            platoon[rowToRotate, 0] = lastElement;

            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    Console.Write(platoon[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
