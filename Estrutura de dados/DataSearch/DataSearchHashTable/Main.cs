using System;

namespace StaticList
{
    public class List
    {
        private char[] elements;
        private int size;
        private int capacity;
        public List(int capacity)
        {
            this.capacity = capacity;
            this.elements = new char[capacity];
            size = 0;
        }
        public void Insert(char value)
        {
            if (size == capacity)
            {
                Console.WriteLine("List full!");
                return;
            }
            elements[size] = value;
            size++;
        }

        public void InsertAtPosition(int pos, char value)
        {
            if (size == capacity)
            {
                Console.WriteLine("List full!");
                return;
            }
            if (pos < 0 || pos > size)
            {
                Console.WriteLine("Invalid position!");
                return;
            }

            for (int i = size; i > pos; i--)
            {
                elements[i] = elements[i - 1];
            }

            elements[pos] = value;
            size++;
        }
        public void Remove(int pos)
        {
            if (pos < 0 || pos >= size)
            {
                Console.WriteLine("Invalid position!");
                return;
            }

            for (int i = pos; i < size - 1; i++)
            {
                elements[i] = elements[i + 1];
            }
            size--;
        }
        public void Display()
        {
            if (size == 0)
            {
                Console.WriteLine("List empty");
                return;
            }
            Console.Write("List: ");
            for (int i = 0; i < size; i++)
            {
                Console.Write(elements[i] + " ");
            }
            Console.WriteLine();
        }
        public int Size()
        {
            return size;
        }
        public bool IsEmpty()
        {
            return size == 0;
        }
        public char ElementAtPosition(int pos)
        {
            if (pos < 0 || pos >= size) // Added boundary check
            {
                Console.WriteLine("Invalid position!");
                return '\0'; // Return null char for invalid position
            }
            return elements[pos]; // Return the element at the specified position
        }
        internal class Program
        {
            static void Main(string[] args)
            {
                List list = new List(10);
                int option;
                char value;
                int pos;

                do
                {
                    Console.WriteLine("\nSTATIC LINEAR LIST");
                    Console.WriteLine("0. Exit");
                    Console.WriteLine("1. Insert at end");
                    Console.WriteLine("2. Insert at specific position");
                    Console.WriteLine("3. Remove by position");
                    Console.WriteLine("4. List Size");
                    Console.WriteLine("5. Display list");
                    Console.WriteLine("6. Display element at position");
                    Console.Write("Choose an option: ");
                    option = int.Parse(Console.ReadLine());

                    switch (option)
                    {
                        case 0:
                            Console.WriteLine("Exiting...");
                            break;

                        case 1:
                            Console.Write("Enter the value: ");
                            value = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            list.Insert(value);
                            break;

                        case 2:
                            Console.Write("Enter the position: ");
                            pos = int.Parse(Console.ReadLine());
                            Console.Write("Enter the value: ");
                            value = Console.ReadKey().KeyChar;
                            Console.WriteLine();
                            list.InsertAtPosition(pos, value);
                            break;

                        case 3:
                            Console.Write("Enter the position to remove: ");
                            pos = int.Parse(Console.ReadLine());
                            list.Remove(pos);
                            break;

                        case 4:
                            Console.WriteLine("Current size: " + list.Size());
                            break;

                        case 5:
                            list.Display();
                            break;

                        case 6:
                            Console.Write("Enter the position: ");
                            pos = int.Parse(Console.ReadLine());
                            value = list.ElementAtPosition(pos);
                            if (value != '\0') // Check if a valid character was returned
                            {
                                Console.WriteLine("Element: " + value);
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid option!");
                            break;
                    }

                } while (option != 0);
            }
        }
    }
}
