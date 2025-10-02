using System;
using System.Globalization;

namespace Matrix_6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
            double[,] matrix = new double[N, N];

            for (int i = 0; i < N; i++)
            {
                string[] row = Console.ReadLine().Split(' ');
                for (int j = 0; j < N; j++)
                {
                    matrix[i, j] = double.Parse(row[j], CultureInfo.InvariantCulture);
                }
            }

            int chosenRow = int.Parse(Console.ReadLine());
            int chosenColumn = int.Parse(Console.ReadLine());
            double positiveSum = 0.0;

            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] > 0.0)
                    {
                        positiveSum += matrix[i, j];
                    }
                }
            }

            Console.WriteLine($"SUM OF POSITIVES: {positiveSum.ToString("F1", CultureInfo.InvariantCulture)}");

            Console.Write("CHOSEN ROW: ");
            for (int j = 0; j < N; j++)
            {
                Console.Write(matrix[chosenRow, j].ToString("F1", CultureInfo.InvariantCulture) + " ");
            }
            Console.WriteLine();

            Console.Write("CHOSEN COLUMN: ");
            for (int i = 0; i < N; i++)
            {
                Console.Write(matrix[i, chosenColumn].ToString("F1", CultureInfo.InvariantCulture) + " ");
            }
            Console.WriteLine();

            Console.Write("MAIN DIAGONAL: ");
            for (int i = 0; i < N; i++)
            {
                Console.Write(matrix[i, i].ToString("F1", CultureInfo.InvariantCulture) + " ");
            }
            Console.WriteLine();

            Console.WriteLine("CHANGED MATRIX:");
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (matrix[i, j] < 0.0)
                    {
                        matrix[i, j] = Math.Pow(matrix[i, j], 2.0);
                    }
                    Console.Write(matrix[i, j].ToString("F1", CultureInfo.InvariantCulture) + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
