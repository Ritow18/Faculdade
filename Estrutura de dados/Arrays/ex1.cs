using System;
using System.Globalization;

namespace Vector_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the quantity of numbers:");
            int n = int.Parse(Console.ReadLine());

            double[] vector = new double[n];

            Console.WriteLine($"Enter the {n} real numbers (separated by space):");
            string[] input = Console.ReadLine().Split(' ');

            for (int i = 0; i < n; i++)
            {
                vector[i] = double.Parse(input[i], CultureInfo.InvariantCulture);
            }

            double largest = vector[0];
            int position = 0;

            for (int i = 1; i < n; i++)
            {
                if (vector[i] > largest)
                {
                    largest = vector[i];
                    position = i;
                }
            }

            Console.WriteLine(largest.ToString("F1", CultureInfo.InvariantCulture));
            Console.WriteLine(position);
        }
    }
}
