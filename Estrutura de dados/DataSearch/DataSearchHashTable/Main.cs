
using System;

namespace DataSearchHashTable
{
    public class Program
    {
        static void Main(string[] args)
        {
            HashTable<int> hashTable = new HashTable<int>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Hash Table");
                Console.WriteLine("1: Insert Value");
                Console.WriteLine("2: Search Value");
                Console.WriteLine("3: Remove Value");
                Console.WriteLine("4: Display Table");
                Console.WriteLine("0: Exit");
                Console.Write("Enter one of the options above: ");

                char option = Console.ReadKey().KeyChar;
                Console.WriteLine();
                Console.Clear();

                switch (option)
                {
                    case '1':
                        Console.Write("Value to insert: ");
                        if (int.TryParse(Console.ReadLine(), out int valueToInsert))
                        {
                            hashTable.Insert(valueToInsert);
                        }
                        else
                        {
                            Console.WriteLine("Invalid value!");
                        }
                        break;
                    case '2':
                        Console.Write("Value to search: ");
                        if (int.TryParse(Console.ReadLine(), out int valueToSearch))
                        {
                            if (hashTable.Search(valueToSearch))
                            {
                                Console.WriteLine($"{valueToSearch} Found");
                            }
                            else
                            {
                                Console.WriteLine($"{valueToSearch} not in table");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid value!");
                        }
                        break;
                    case '3':
                        Console.Write("Value to remove: ");
                        if (int.TryParse(Console.ReadLine(), out int valueToRemove))
                        {
                            if (hashTable.Remove(valueToRemove))
                            {
                                Console.WriteLine($"{valueToRemove} removed");
                            }
                            else
                            {
                                Console.WriteLine($"{valueToRemove} not found");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid value!");
                        }
                        break;
                    case '4':
                        hashTable.Display();
                        break;
                    case '0':
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
                Console.WriteLine("\nPress Enter to continue....");
                Console.ReadKey();
            }
        }
    }
}
