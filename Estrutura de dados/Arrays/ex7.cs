using System;

namespace Vector_7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            string[] names = new string[N];
            double[] grade1 = new double[N];
            double[] grade2 = new double[N];

            for (int i = 0; i < N; i++)
            {
                string[] input = Console.ReadLine().Split(' ');
                names[i] = input[0];
                grade1[i] = double.Parse(input[1]);
                grade2[i] = double.Parse(input[2]);
            }

            Console.WriteLine("Approved students:");

            for (int i = 0; i < N; i++)
            {
                double average = (grade1[i] + grade2[i]) / 2.0;
                if (average >= 6.0)
                {
                    Console.WriteLine(names[i]);
                }
            }
        }
    }
}
