using System.Collections.Generic;
using System;

namespace Queue_Implementation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Queue<char> queue = new Queue<char>();

            int option;
            char value;
            Console.WriteLine("Queue using Queue");

            do
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Enqueue");
                Console.WriteLine("2. Peek");
                Console.WriteLine("3. Dequeue");
                Console.WriteLine("4. Queue Size");
                Console.WriteLine("5. Display Queue");
                option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 0:
                        Console.WriteLine("Exiting...");
                        break;

                    case 1:
                        Console.WriteLine("Enter a value to enqueue: ");
                        value = Console.ReadKey().KeyChar;
                        queue.Enqueue(value);
                        Console.WriteLine();
                        break;

                    case 2:
                        if (queue.Count > 0)
                            Console.WriteLine("Front of the queue: " + queue.Peek());
                        else
                            Console.WriteLine("Queue is Empty");
                        break;

                    case 3:
                        if (queue.Count > 0)
                            Console.WriteLine("Dequeued element: " + queue.Dequeue());
                        else
                            Console.WriteLine("Queue is Empty");
                        break;

                    case 4:
                        Console.WriteLine("Number of elements in the queue: " + queue.Count);
                        break;

                    case 5:
                        Console.WriteLine("Queue: ");
                        foreach (var item in queue)
                        {
                            Console.WriteLine(item + " ");
                        }
                        Console.WriteLine();
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;

                }
            } while (option != 0);
        }
    }
}
