using System.Globalization;
using System;

namespace Vector_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            double[] heights = new double[N];
            char[] genders = new char[N];

            for (int i = 0; i < N; i++)
            {
                string[] input = Console.ReadLine().Split(' ');
                heights[i] = double.Parse(input[0], CultureInfo.InvariantCulture);
                genders[i] = char.Parse(input[1]);
            }

            double minHeight = heights[0];
            double maxHeight = heights[0];
            double womenHeightsSum = 0;
            int womenCount = 0;
            int menCount = 0;

            for (int i = 0; i < N; i++)
            {
                if (heights[i] < minHeight) minHeight = heights[i];
                if (heights[i] > maxHeight) maxHeight = heights[i];

                if (genders[i] == 'F')
                {
                    womenHeightsSum += heights[i];
                    womenCount++;
                }
                else if (genders[i] == 'M')
                {
                    menCount++;
                }
            }

            double womenAverageHeight = womenCount > 0 ? womenHeightsSum / womenCount : 0.0;

            Console.WriteLine("Lowest height = " + minHeight.ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine("Highest height = " + maxHeight.ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine("Average height of women = " + womenAverageHeight.ToString("F2", CultureInfo.InvariantCulture));
            Console.WriteLine("Number of men = " + menCount);
        }
    }
}
