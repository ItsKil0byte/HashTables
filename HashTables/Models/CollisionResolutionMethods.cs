namespace HashTables
{
    public static class CollisionResolutionMethods
    {
        public static int LinearProbing(int index, int i)
        {
            return index + i;
        }

        public static int QuadraticProbing(int index, int i, int size)
        {
            return (index + i * i) % size;
        }

        public static int DoubleHashing<K>(K key, int size, int attempt, Func<K, int> hashFunction)
        {
            int hash1 = Math.Abs(hashFunction(key) % size);
            int hash2 = 1 + (Math.Abs(hashFunction(key) % (size - 1))); // Второй хеш должен быть не нулевым
            return (hash1 + attempt * hash2) % size;
        }
        
        public static int DistanceProbing(int index, int i)
        {
            return index + (i * (i + 1)) / 2; 
        }

        public static int SparseProbing(int index, int i)
        {
            return index + (i * 2); 
        }
    }
}