using System;

namespace MatrixLargestElement
{
    class Program
    {
        static void Main(string[] args)
        {
            int n, largest = 0;
            int[,] matrix;
            string[] values;

            n = Convert.ToInt32(Console.ReadLine());
            //n = int.Parse(Console.ReadLine())
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
                largest = matrix[i, 0];
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] > largest)
                    {
                        largest = matrix[i, j];
                    }
                }
                Console.WriteLine(largest);
            }
            Console.ReadKey();
        }
    }
}
