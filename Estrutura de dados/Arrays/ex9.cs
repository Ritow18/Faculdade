using System.Globalization;
using System;

namespace Vector_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the number of goods:");
            int n = int.Parse(Console.ReadLine());

            int profitBelow10 = 0, profitBetween10and20 = 0, profitAbove20 = 0;
            double totalPurchaseValue = 0, totalSaleValue = 0;

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Enter the data for good {i + 1} (name, purchase price, sale price):");
                string[] data = Console.ReadLine().Split(' ');

                string name = data[0];
                double purchasePrice = double.Parse(data[1], CultureInfo.InvariantCulture);
                double salePrice = double.Parse(data[2], CultureInfo.InvariantCulture);

                double profitPercentage = ((salePrice - purchasePrice) / purchasePrice) * 100;

                if (profitPercentage < 10)
                {
                    profitBelow10++;
                }
                else if (profitPercentage <= 20)
                {
                    profitBetween10and20++;
                }
                else
                {
                    profitAbove20++;
                }

                totalPurchaseValue += purchasePrice;
                totalSaleValue += salePrice;
            }

            double totalProfit = totalSaleValue - totalPurchaseValue;

            Console.WriteLine($"Profit below 10%: {profitBelow10}");
            Console.WriteLine($"Profit between 10% and 20%: {profitBetween10and20}");
            Console.WriteLine($"Profit above 20%: {profitAbove20}");
            Console.WriteLine($"Total purchase value: {totalPurchaseValue.ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Total sale value: {totalSaleValue.ToString("F2", CultureInfo.InvariantCulture)}");
            Console.WriteLine($"Total profit: {totalProfit.ToString("F2", CultureInfo.InvariantCulture)}");
        }
    }
}
