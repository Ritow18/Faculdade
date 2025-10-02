using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//ex 6 est dados
namespace c__Lista_ex
{
    class Program
    {
        static void Main(string[] args)
        {
            int m, n, fila, ult;
            int[,] mat;
            String[] valores;

            m = int.Parse(Console.ReadLine());
            n = int.Parse(Console.ReadLine());

            mat = new int[m, n];

            for (int i = 0; i < m; i++)
            {
                valores = Console.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    mat[i, j] = int.Parse(valores[j]);
                }
            }
            fila = int.Parse(Console.ReadLine());
            fila--;

            // ult esta pegando o ultimo elemento da fila

            ult = mat[fila, n - 1];
        //    mat[fila, n - 1] = 0;

            for (int k = n-1; k > 0; k--)
            {
                mat[fila, k] = mat[fila, k - 1];

                if (k == 1) { mat[fila, 0 ] = ult; }
            }
            for (int i =0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(mat[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
