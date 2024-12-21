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
        
        public static int PolynomialHash(string key, int size)
        {
            const int prime = 31; // Простое число в качестве основания
            int hash = 0;

            for (int i = 0; i < key.Length; i++)
            {
                hash = (hash * prime + key[i]) % size; // Полиномиальная формула
            }

            return hash;
        }
        
        public static int SimpleSumHash(string key, int size)
        {
            int hash = 0;
            foreach (char c in key)
            {
                hash += c; 
            }
            return hash % size;
        }
    }
}