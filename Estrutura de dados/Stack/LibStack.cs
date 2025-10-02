using System;

namespace ManualStack
{
    class Stack
    {
        private char[] elements;
        private int top, size;

        public Stack(int capacity)
        {
            elements = new char[capacity];
            top = -1;
            size = 0;
        }
        public void Push(char character)
        {
            if (size == elements.Length)
            {
                Console.WriteLine("The stack is full!");
                return;
            }
            elements[++top] = character;
            size++;
        }
        public void Display()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Your stack is empty!");
                return;
            }
            Console.WriteLine("Stack: ");
            for (int i = top; i >= 0; i--)
            {
                Console.WriteLine(" " + elements[i]);
            }
        }
        public int Size()
        {
            return size;
        }
        public char Peek()
        {
            if (IsEmpty())
            {
                Console.WriteLine("The stack has no elements to display!");
                return '\0'; 
            }
            else
            {
                return elements[top];
            }
        }
        public char Pop()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Your stack is empty! No elements to remove.");
                return '\0';
            }

            char c = elements[top--];
            size--;
            return c;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }
    }

    class Program
    {
        static void Main()
        {
            Stack stack = new Stack(10);
            char option;

            do
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1 - Push an element onto the Stack: ");
                Console.WriteLine("2 - Pop an element from the Stack: ");
                Console.WriteLine("3 - Display the Stack elements: ");
                Console.WriteLine("4 - Show the top of the Stack: ");
                Console.WriteLine("5 - Show the Stack size: ");
                Console.WriteLine("0 - Exit: ");

                option = Console.ReadKey().KeyChar;
                Console.ReadLine(); 
                switch (option)
                {
                    case '1':
                        Console.WriteLine("Enter the character to push:");
                        char characterToPush = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        stack.Push(characterToPush);
                        break;
                    case '2':
                        Console.WriteLine("Element removed: " + stack.Pop());
                        break;
                    case '3':
                        stack.Display();
                        break;
                    case '4':
                        Console.WriteLine("Top of the stack: " + stack.Peek());
                        break;
                    case '5':
                        Console.WriteLine("Stack size: " + stack.Size());
                        break;
                    case '0':
                        Console.WriteLine("Exiting...");
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            } while (option != '0');
        }
    }
}
