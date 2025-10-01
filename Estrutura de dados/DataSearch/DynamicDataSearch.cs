using System.Collections.Generic;
using System;

namespace DataSearchDynamic
{
    public class DynamicList<T> where T : IComparable<T>
    {
        private List<T> elements;
        public DynamicList()
        {
            elements = new List<T>();
        }
        public void Insert (T value)
        {
            elements.Add(value);
        }
        public void display()
        {
            if (elements.Count == 0)
            {
                Console.WriteLine("Empty List!");
                return;
            }
            else
            {
                Console.WriteLine("List: ");
                foreach (T element in elements) 
                {
                    Console.WriteLine($"{element}"+" ");
                }
                Console.WriteLine();
            }
        }
        public int SequencialSearch(T value)
        {
            for(int i=0; i<elements.Count;i++){
                if (elements[i].CompareTo(value)==0)
                {
                    return i;
                }
            }
            return -1;
        }
        public int BinarySearch(T value)
        {
            int start = 0,end = elements.Count-1;
            while (start<=end)
            {
                int middle = (start + end)/ 2;
                if (elements[middle].CompareTo(value)==0)
                {
                    return middle;
                }
                else
                {
                    start = middle + 1;
                }
            }
            return -1;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Type the elements quantity of your List: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine();
            DynamicList<int> list = new DynamicList<int>();

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"type the {i+1} element: ");
                int value = int.Parse(Console.ReadLine());
                list.Insert(value);
            }
            list.display();

            Console.Write("type the element that your searching: ");
            int searchedElement = int.Parse(Console.ReadLine());
            int expectedResult = list.SequencialSearch(searchedElement);

            if (expectedResult != -1)
            {
                Console.WriteLine($"Element: {searchedElement} " +
                    $" founded on the position: {expectedResult + 1}");
            }
            else
            {
                Console.WriteLine($"Element {searchedElement}" +
                    $" Was not founded (sequencial search)");
            }
            int BinaryResult = list.BinarySearch(searchedElement);

            if (expectedResult != -1)
            {
                Console.WriteLine($"Element: {searchedElement} " +
                    $" founded on the position: {expectedResult + 1}");
            }
            else
            {
                Console.WriteLine($"Element {searchedElement}" +
                    $" Was not founded (binary search)");
            }
        }
    }
}
