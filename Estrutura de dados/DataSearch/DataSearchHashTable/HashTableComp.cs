using System;
using System.Collections.Generic;

namespace DataSearchHashTable
{
    public class HashTable<T>
    {
        private List<LinkedList<T>> buckets;
        private int capacity;

        public HashTable(int capacity = 10)
        {
            this.capacity = capacity;
            buckets = new List<LinkedList<T>>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                buckets.Add(new LinkedList<T>());
            }
        }

        private int Hash(T value)
        {
            return Math.Abs(value.GetHashCode()) % capacity;
        }

        public void Insert(T value)
        {
            int index = Hash(value);
            if (!buckets[index].Contains(value))
            {
                buckets[index].AddLast(value);
                Console.WriteLine($"{value} inserted at position: {index}");
            }
            else
            {
                Console.WriteLine($"{value} already exists at position: {index}");
            }
        }

        public bool Search(T value)
        {
            int index = Hash(value);
            return buckets[index].Contains(value);
        }

        public bool Remove(T value)
        {
            int index = Hash(value);
            return buckets[index].Remove(value);
        }

        public void Display()
        {
            for (int i = 0; i < capacity; i++)
            {
                Console.Write($"[{i}]: ");
                foreach (var item in buckets[i])
                {
                    Console.Write(item + " -> ");
                }
                Console.Write("null");
                Console.WriteLine();
            }
        }
    }
}
