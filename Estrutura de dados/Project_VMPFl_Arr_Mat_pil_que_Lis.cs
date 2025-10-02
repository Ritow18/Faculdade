using System;
using System.Collections.Generic;
using System.Linq;

namespace Interactive_Data_Structure_Menu_VMFPL
{
    class Program
    {
        static void Main(string[] args)
        {
            char choice;
            ShowMainMenu();
            do
            {
                choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '1':
                        CreateList();
                        break;
                    case '2':
                        CreateQueue();
                        break;
                    case '3':
                        CreateStack();
                        break;
                    case '4':
                        CreateVector();
                        break;
                    case '5':
                        MatrixOperationsWhyThisIsComplex.CreateMatrix();
                        break;
                    case '0':
                        Console.WriteLine("Exiting program...");
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        ShowMainMenu();
                        break;
                }
            } while (choice != '0');
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\n Main Menu");
            Console.WriteLine("Choose an option: ");
            Console.WriteLine("1 - Create List");
            Console.WriteLine("2 - Create Queue");
            Console.WriteLine("3 - Create Stack");
            Console.WriteLine("4 - Create Vector");
            Console.WriteLine("5 - Create Matrix");
            Console.WriteLine("0 - Exit");
        }

        static void CreateList()
        {
            List<char> list = new List<char>();
            Console.WriteLine("\nCreate your List");
            while (true)
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1 - Add item");
                Console.WriteLine("2 - Display list");
                Console.WriteLine("3 - Remove element from list");
                Console.WriteLine("4 - Query/Search Element");
                Console.WriteLine("0 - Return to Main Menu");

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '1':
                        Console.Write("Enter an item to be added: ");
                        char addItem = Console.ReadKey().KeyChar;

                        list.Add(addItem);

                        Console.WriteLine($"'{addItem}' added to the list.");
                        break;

                    case '2':
                        if (list.Count > 0)
                        {
                            Console.WriteLine("\nList elements");
                            foreach (var item in list)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your list is empty.");
                        }
                        break;

                    case '3':
                        if (list.Count > 0)
                        {
                            Console.WriteLine("Which list element would you like to remove? ");
                            foreach (var item in list)
                            {
                                Console.WriteLine(item);
                            }
                            char removeItem = Console.ReadKey().KeyChar;
                            Console.WriteLine($"Item removed: {removeItem}");
                            list.Remove(removeItem);
                        }
                        else
                        {
                            Console.WriteLine("\nYour list is empty.");
                        }
                        break;

                    case '4':
                        if (list.Count > 0)
                        {
                            Console.Write("Enter which element you want to query: ");
                            char query = Console.ReadKey().KeyChar;
                            if (list.Contains(query))
                            {
                                Console.WriteLine($"{query} is in the list!");
                            }
                            else
                            {
                                Console.WriteLine($"{query} was not found in the list");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your list is empty!");
                        }
                        break;

                    case '0':
                        ShowMainMenu();
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void CreateQueue()
        {
            Queue<char> queue = new Queue<char>();
            Console.WriteLine("\nCreate your Queue");

            while (true)
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1 - Add item");
                Console.WriteLine("2 - Remove item");
                Console.WriteLine("3 - Display queue");
                Console.WriteLine("4 - Query/Search Element");
                Console.WriteLine("0 - Return to Main Menu\n");

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '1':
                        Console.Write("\nEnter the item to be enqueued: ");
                        char enqueueItem = Console.ReadKey().KeyChar;

                        queue.Enqueue(enqueueItem);
                        Console.WriteLine($"\n{enqueueItem} enqueued.");

                        break;
                    case '2':
                        if (queue.Count > 0)
                        {
                            char dequeueItem = queue.Dequeue();
                            Console.WriteLine($"\n{dequeueItem} dequeued.");
                        }
                        else
                        {
                            Console.WriteLine("\nThe queue is empty.");
                        }
                        break;
                    case '3':
                        if (queue.Count > 0)
                        {
                            Console.WriteLine("Queue elements: ");
                            foreach (var item in queue)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nThe queue is empty.");
                        }
                        break;

                    case '4':
                        if (queue.Count > 0)
                        {
                            Console.Write("Enter which element you want to query: ");
                            char query = Console.ReadKey().KeyChar;
                            if (queue.ToArray().Contains(query))
                            {
                                Console.WriteLine($"{query} is present in your Queue!");
                            }
                            else
                            {
                                Console.WriteLine($"Your Queue does not contain the element {query}!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your queue is empty!");
                        }
                        break;

                    case '0':
                        ShowMainMenu();
                        return;
                    default:
                        Console.WriteLine("\nInvalid option. Please try again.");
                        break;
                }
            }
        }

        static void CreateStack()
        {
            Stack<char> stack = new Stack<char>();
            Console.WriteLine("\nCreate your Stack");

            while (true)
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1 - Add item");
                Console.WriteLine("2 - Remove item");
                Console.WriteLine("3 - Display stack");
                Console.WriteLine("4 - Query/Search Element");
                Console.WriteLine("0 - Return to Main Menu");
                Console.WriteLine();

                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case '1':
                        Console.Write("Enter the item to be pushed: ");
                        char pushItem = Console.ReadKey().KeyChar;
                        stack.Push(pushItem);
                        Console.WriteLine($"{pushItem} pushed.");
                        break;
                    case '2':
                        if (stack.Count > 0)
                        {
                            char popItem = stack.Pop();
                            Console.WriteLine($"{popItem} popped.");
                        }
                        else
                        {
                            Console.WriteLine("Your stack is empty.");
                        }
                        break;
                    case '3':
                        if (stack.Count > 0)
                        {
                            Console.WriteLine("\nStack elements");
                            foreach (var item in stack)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nYour stack is empty.");
                        }
                        break;

                    case '4':
                        if (stack.Count > 0)
                        {
                            Console.Write("Enter which element you want to query: ");
                            char query = Console.ReadKey().KeyChar;
                            if (stack.ToArray().Contains(query))
                            {
                                Console.WriteLine($"{query} is in your stack!");
                            }
                            else
                            {
                                Console.WriteLine($"Your Stack does not contain the element {query}!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your stack is empty!");
                        }
                        break;

                    case '0':
                        ShowMainMenu();
                        return;

                    default:
                        Console.WriteLine("Invalid option");
                        break;
                }
            }
        }

        static void CreateVector()
        {
            Console.WriteLine("Create Vector");
            Console.Write("Please, enter the desired size for your vector: ");
            int vectorSize;
            int currentItemIndex = 0; 
            while (!int.TryParse(Console.ReadLine(), out vectorSize) || vectorSize <= 0)
            {
                Console.Write("Invalid input. Please enter a positive integer for the vector size: ");
            }

            string[] vector = new string[vectorSize];
            string vectorItem; 

            while (true)
            {
                Console.WriteLine("Choose an option: ");
                Console.WriteLine("1 - Add item");
                Console.WriteLine("2 - Remove item");
                Console.WriteLine("3 - Display Vector");
                Console.WriteLine("4 - Query/Search Element");
                Console.WriteLine("0 - Return to Main Menu");
                Console.WriteLine();

                char choice = Console.ReadKey().KeyChar;
                Console.WriteLine();

                switch (choice)
                {
                    case '1':
                        if (currentItemIndex < vectorSize)
                        {
                            Console.Write("Enter the item you want to add to your vector: ");
                            vector[currentItemIndex] = Console.ReadLine();
                            currentItemIndex++;
                            Console.WriteLine("Item added successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Your vector is full!");
                        }
                        break;

                    case '2':
                        if (currentItemIndex > 0)
                        {
                            currentItemIndex--; 
                            Console.WriteLine($"Removing '{vector[currentItemIndex]}' from position {currentItemIndex}...");
                            vector[currentItemIndex] = null; 
                            Console.WriteLine("Last inserted item in the vector was removed.");
                        }
                        else
                        {
                            Console.WriteLine("Your vector is empty!");
                        }
                        break;

                    case '3':
                        if (vector.Length > 0)
                        {
                            Console.WriteLine("\nVector elements:");
                            for (int k = 0; k < vectorSize; k++)
                            {
                                Console.WriteLine($"[{k}]: {vector[k] ?? "Empty"}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your vector is empty!");
                        }
                        break;

                    case '4':
                        if (vector.Length > 0)
                        {
                            Console.Write("Enter the item you want to check if it exists in the vector: ");
                            vectorItem = Console.ReadLine();
                            int foundIndex = Array.IndexOf(vector, vectorItem); 

                            if (foundIndex != -1)
                            {
                                Console.WriteLine($"The vector contains your item '{vectorItem}' at position {foundIndex}!");
                            }
                            else
                            {
                                Console.WriteLine($"'{vectorItem}' does not exist in your vector.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Your vector is empty!");
                        }
                        break;

                    case '0':
                        ShowMainMenu();
                        return;

                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        public class MatrixOperationsWhyThisIsComplex
        {
            private static string[,] matrix;
            private static int rowCount; 
            private static int colCount; 

            public static void CreateMatrix()
            {
                Console.Clear();
                Console.WriteLine("Create matrix");
                bool isValidSize = false;
                do
                {
                    Console.WriteLine("Enter the number of rows and columns for your matrix, separated by a space (e.g., 3 4):");
                    string[] rowColInput = Console.ReadLine().Split(' ');

                    if (rowColInput.Length == 2 &&
                        int.TryParse(rowColInput[0], out rowCount) && rowCount > 0 &&
                        int.TryParse(rowColInput[1], out colCount) && colCount > 0)
                    {
                        isValidSize = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter valid positive integers for rows and columns.");
                        isValidSize = false;
                    }
                } while (isValidSize == false);

                matrix = new string[rowCount, colCount];
                Console.WriteLine($"You successfully created a {rowCount}x{colCount} matrix." +
                                  $"\nPress any key to continue...");
                Console.ReadKey();

                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Choose an option: ");
                    Console.WriteLine("1 - Add item");
                    Console.WriteLine("2 - Remove item");
                    Console.WriteLine("3 - Display Matrix");
                    Console.WriteLine("4 - Query/Search Element");
                    Console.WriteLine("0 - Return to Main Menu");
                    Console.WriteLine();

                    char choice;
                    bool isValidOption = false;
                    do
                    {
                        choice = Console.ReadKey().KeyChar;
                        Console.WriteLine(); 

                        if ((choice == '1') || (choice == '2') || (choice == '3') || (choice == '4') || (choice == '0'))
                        {
                            isValidOption = true;
                        }
                        else
                        {
                            Console.WriteLine("Please choose a valid option.");
                        }
                    } while (isValidOption == false);

                    switch (choice)
                    {
                        case '1':
                            AddItem();
                            break;

                        case '2':
                            RemoveItem();
                            break;

                        case '3':
                            DisplayItems();
                            break;

                        case '4':
                            SearchItem();
                            break;

                        case '0':
                            Console.WriteLine("Press any key to return to the main menu...");
                            return;
                        default:
                            Console.WriteLine("Enter a valid option!");
                            break;
                    }
                }
            }

            private static void AddItem()
            {
                Console.Clear();
                if (matrix != null && !IsMatrixEmpty(matrix))
                {
                    bool validChar = false;
                    Console.WriteLine("A matrix already exists. Adding items again will\n" +
                                      "replace the previous matrix content. Do you want to proceed? (1-yes / 2-no)");
                    do
                    {
                        char addAgainChoice = Console.ReadKey().KeyChar;
                        Console.WriteLine(); 

                        if (addAgainChoice == '1')
                        {
                            Console.WriteLine("Starting the process of replacing your matrix values...");
                            validChar = true;
                        }
                        else if (addAgainChoice == '2')
                        {
                            Console.Write("Understood. Press any key to return to the menu...");
                            Console.ReadKey();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid value (1-yes / 2-no)");
                        }
                    } while (validChar == false);
                }

                Console.WriteLine("You can now enter the elements for your matrix!");
                for (int j = 0; j < rowCount; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        Console.Write($"Enter value for [{j},{k}]: ");
                        matrix[j, k] = Console.ReadLine();
                    }
                }
                Console.WriteLine("Matrix populated. Press any key to continue...");
                Console.ReadKey();
                return;
            }

            private static void RemoveItem()
            {
                Console.Clear();
                if (matrix == null || IsMatrixEmpty(matrix))
                {
                    Console.WriteLine("Your matrix is empty or has not been created!");
                    Console.ReadKey();
                    return;
                }

                bool continueRemoval = true;
                do
                {
                    DisplayItems();
                    Console.WriteLine("\nTo remove, enter the row and column of the item you want to remove (starting from 0 and separated by space, e.g., 0 1 will remove the element at row 0 column 1).");
                    Console.WriteLine("Type 'exit' to return to the matrix menu.");

                    string input = Console.ReadLine();
                    if (input.ToLower() == "exit")
                    {
                        continueRemoval = false;
                        continue;
                    }

                    string[] coords = input.Split(' ');
                    int rowToRemove, colToRemove;

                    if (coords.Length == 2 &&
                        int.TryParse(coords[0], out rowToRemove) &&
                        int.TryParse(coords[1], out colToRemove) &&
                        rowToRemove >= 0 && rowToRemove < rowCount &&
                        colToRemove >= 0 && colToRemove < colCount)
                    {
                        if (!string.IsNullOrEmpty(matrix[rowToRemove, colToRemove]))
                        {
                            Console.WriteLine($"Removing '{matrix[rowToRemove, colToRemove]}' from position [{rowToRemove},{colToRemove}]");
                            matrix[rowToRemove, colToRemove] = null;
                            Console.WriteLine("Item removed successfully!");
                        }
                        else
                        {
                            Console.WriteLine("This position is already empty!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid coordinates. Please enter the row and column correctly.");
                    }
                    Console.ReadKey();
                } while (continueRemoval);
            }

            private static void DisplayItems()
            {
                Console.Clear();
                if (matrix == null || IsMatrixEmpty(matrix))
                {
                    Console.WriteLine("Your matrix is empty or has not been created!");
                    Console.ReadKey();
                    return;
                }

                Console.WriteLine("Matrix Elements:");
                for (int j = 0; j < rowCount; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        Console.Write($"[{j},{k}]: {matrix[j, k] ?? "Empty"}\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            private static void SearchItem()
            {
                Console.Clear();
                if (matrix == null || IsMatrixEmpty(matrix))
                {
                    Console.WriteLine("Your matrix is empty or has not been created!");
                    Console.ReadKey();
                    return;
                }

                Console.Write("Enter the item you want to search for in the matrix: ");
                string itemToSearch = Console.ReadLine();
                bool found = false;

                Console.WriteLine($"\nSearching for '{itemToSearch}' in the matrix...");
                for (int j = 0; j < rowCount; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        if (matrix[j, k] != null && matrix[j, k].Equals(itemToSearch, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"The item '{itemToSearch}' was found at position [{j},{k}].");
                            found = true;
                        }
                    }
                }
                if (found == false)
                {
                    Console.WriteLine($"The item '{itemToSearch}' was not found in the matrix.");
                }
                Console.ReadKey();
            }
            private static bool IsMatrixEmpty(string[,] mat)
            {
                if (mat == null) return true; 

                if (mat.Length == 0) 
                {
                    return true;
                }

                foreach (var item in mat)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        return false;
                    }
                }
                return true; 
            }
        }
    }
}
