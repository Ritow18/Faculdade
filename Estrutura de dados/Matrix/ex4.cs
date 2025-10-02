using System;

namespace Matrix_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n, sum = 0;
            int[,] matrix;
            string[] values;

            n = Convert.ToInt32(Console.ReadLine());
            matrix = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                values = Console.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(values[j]);
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j > i)
                    {
                        sum += matrix[i, j];
                    }
                }
            }

            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}
