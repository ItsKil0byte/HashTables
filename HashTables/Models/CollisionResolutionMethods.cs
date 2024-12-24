namespace HashTables
{
    public static class CollisionResolutionMethods
    {
       public static int LinearProbing(int index, int attempt, int size)
        {
        return (index + attempt) % size;
        }

        public static int QuadraticProbing(int index, int attempt, int size, int c1, int c2)
        {
            return (index + c1 * attempt + c2 * attempt * attempt) % size;
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