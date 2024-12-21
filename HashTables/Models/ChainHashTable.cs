namespace HashTables.Models
{
    public class ChainHashTable<K, V> : IHashTable<K, V>
    {
        private int size;
        private LinkedList<KeyValuePair<K, V>>[] table;
        private Func<K, int> hashFunction;

        public ChainHashTable(int size, Func<K, int> hashFunction)
        {
            this.size = size;
            this.hashFunction = hashFunction;

            table = new LinkedList<KeyValuePair<K, V>>[size];

            for (int i = 0; i < size; i++)
            {
                table[i] = new LinkedList<KeyValuePair<K, V>>();
            }
        }

        private int GetIndex(K key)
        {
            return Math.Abs(hashFunction(key) % size);
        }

        public void Insert(K key, V value)
        {
            int index = GetIndex(key);

            foreach (var pair in table[index])
            {
                if (pair.Key.Equals(key))
                {
                    throw new ArgumentException($"Ключ {key} уже находится в хэш-таблице.");
                }
            }

            table[index].AddFirst(new KeyValuePair<K, V>(key, value));
        }

        public V Search(K key)
        {
            int index = GetIndex(key);

            foreach (var pair in table[index])
            {
                if (pair.Key.Equals(key))
                {
                    return pair.Value;
                }
            }

            throw new KeyNotFoundException($"Ключ {key} не найден в хэш-таблице.");
        }

        public bool Delete(K key)
        {
            int index = GetIndex(key);
            LinkedList<KeyValuePair<K, V>> chain = table[index];

            foreach (var pair in chain)
            {
                if (pair.Key.Equals(key))
                {
                    chain.Remove(pair);
                    return true;
                }
            }

            return false;
        }

        public void Print()
        {
            for (int i = 0; i < table.Length; i++)
            {
                Console.Write($"[ {i}: ");
                if (table[i] != null && table[i].Count > 0)
                {
                    foreach (var kvp in table[i])
                    {
                        Console.Write($"({kvp.Key}: {kvp.Value}) -> ");
                    }
                    Console.WriteLine("null ]");
                }
                else
                {
                    Console.WriteLine("... ]");
                }
            }
        }

        public double CalculateLoadFactor()
        {
            int chains = table.Count(chain => chain.Count > 0);
            return (double) chains / table.Length;
        }

        public int GetLongestChainLength()
        {
            return table.Max(chain => chain.Count);
        }

        public int GetShortestChainLength()
        {
            var chains = table.Where(chain => chain.Count > 0);

            if (!chains.Any())
            {
                return 0; // Если все цепочки пусты, вернуть 0
            }

            return chains.Min(chain => chain.Count);
        }
    }
}
