using System;

namespace ManualQueue
{
    class Program
    {
        class Queue
        {
            private char[] elements;
            private int front, rear, size, capacity;

            public Queue(int capacity)
            {
                this.capacity = capacity;
                elements = new char[capacity];
                front = 0;
                rear = -1;
                size = 0;
            }

            public bool IsEmpty()
            {
                return size == 0;
            }

            public void Enqueue(char character)
            {
                if (size == capacity)
                {
                    Console.WriteLine("Queue is full");
                    return;
                }
                rear = (rear + 1) % capacity;
                elements[rear] = character;
                size++;
            }

            public char Dequeue()
            {
                if (IsEmpty())
                {
                    Console.WriteLine("The queue is empty!");
                    return '\0';
                }
                char value = elements[front];
                front = (front + 1) % capacity;
                size--;
                return value;
            }

            public void Display()
            {
                if (IsEmpty())
                {
                    Console.WriteLine("The queue is empty!");
                    return;
                }
                Console.Write("Queue: ");
                for (int i = 0; i < size; i++)
                {
                    int index = (front + i) % capacity;
                    Console.Write(elements[index] + " ");
                }
                Console.WriteLine();
            }

            public int Size()
            {
                return size;
            }

            public char Peek()
            {
                if (IsEmpty())
                {
                    Console.WriteLine("The queue is empty! No element to peek.");
                    return '\0'; 
                }
                return elements[front];
            }
        }

        static void Main(string[] args)
        {
            Queue queue = new Queue(10);
            char value, option;

            do
            {
                Console.WriteLine("\nChoose one of the following options:");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Enqueue");
                Console.WriteLine("2 - Peek");
                Console.WriteLine("3 - Dequeue");
                Console.WriteLine("4 - Queue size");
                Console.WriteLine("5 - Display queue elements");
                option = Console.ReadKey().KeyChar;
                Console.WriteLine(); 

                switch (option)
                {
                    case '0':
                        Console.WriteLine("Exiting the system...");
                        break;
                    case '1':
                        Console.Write("Enter the value to enqueue: ");
                        value = Console.ReadKey().KeyChar;
                        queue.Enqueue(value);
                        Console.WriteLine();
                        break;
                    case '2':
                        if (!queue.IsEmpty())
                        {
                            value = queue.Peek();
                            Console.WriteLine("Front of the queue: " + value);
                        }
                        break;
                    case '3':
                        if (!queue.IsEmpty())
                        {
                            value = queue.Dequeue();
                            Console.WriteLine("Dequeued element: " + value);
                        }
                        break;
                    case '4':
                        Console.WriteLine("Number of elements in the queue: " + queue.Size());
                        break;
                    case '5':
                        queue.Display();
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            } while (option != '0');
        }
    }
}
