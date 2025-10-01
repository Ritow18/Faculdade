using System;

namespace Vector_3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            int[] A = new int[N];
            string[] inputA = Console.ReadLine().Split(' ');
            for (int i = 0; i < N; i++)
            {
                A[i] = int.Parse(inputA[i]);
            }

            int[] B = new int[N];
            string[] inputB = Console.ReadLine().Split(' ');
            for (int i = 0; i < N; i++)
            {
                B[i] = int.Parse(inputB[i]);
            }

            int[] C = new int[N];
            for (int i = 0; i < N; i++)
            {
                C[i] = A[i] + B[i];
            }

            for (int i = 0; i < N; i++)
            {
                Console.Write(C[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
