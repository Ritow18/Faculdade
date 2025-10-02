using System;
using System.Collections.Generic;

namespace StackFunctions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stack<char> stack = new Stack<char>();
            char option;
            do
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1 - Push element onto Stack: ");
                Console.WriteLine("2 - Pop element from Stack: ");
                Console.WriteLine("3 - Display Stack elements: ");
                Console.WriteLine("4 - Show top of Stack: ");
                Console.WriteLine("5 - Show Stack size: ");
                Console.WriteLine("0 - Exit: ");
                option = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (option)
                {
                    case '1':
                        Console.WriteLine("Enter the character you want to push:");
                        char characterToPush = Console.ReadKey().KeyChar;
                        Console.WriteLine();
                        stack.Push(characterToPush);
                        break;
                    case '2':
                        if (stack.Count > 0)
                            Console.WriteLine("Top element removed: " + stack.Pop());
                        else
                            Console.WriteLine("Stack is empty");
                        break;
                    case '3':
                        if (stack.Count == 0)
                            Console.WriteLine("Stack is empty");
                        else
                            Console.WriteLine("Stack elements: ");
                        foreach (var item in stack)
                        {
                            Console.WriteLine(" " + item);
                        }
                        break;
                    case '4':
                        if (stack.Count > 0)
                            Console.WriteLine("The top of the stack is: " + stack.Peek());
                        else
                            Console.WriteLine("There are no items in the stack");

                        Console.WriteLine();

                        break;
                    case '5':
                        Console.WriteLine("The size of the stack is: " + stack.Count);
                        Console.WriteLine();
                        break;
                    case '0':
                        Console.WriteLine("Exiting....");
                        break;

                }
                Console.WriteLine();

            } while (option != '0');

        }
    }
}
