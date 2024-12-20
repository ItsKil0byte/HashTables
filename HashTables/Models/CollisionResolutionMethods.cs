using System;

namespace HashTables
{
    public static class CollisionResolutionMethods
    {
        public static int LinearProbing(int index, int i)
        {
            return index + i;
        }

        public static int QuadraticProbing(int index, int i)
        {
            return index + i * i;
        }

        public static int DoubleHashing<K>(K key, int size, int attempt)
        {
            int hash1 = key.GetHashCode() % size;
            int hash2 = 1 + (key.GetHashCode() % (size - 1)); // Второй хеш должен быть не нулевым
            return (hash1 + attempt * hash2) % size;
        }
    }
}