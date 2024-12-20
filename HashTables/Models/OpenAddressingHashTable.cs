using HashTables.Models;

namespace HashTables
{
    public enum CollisionResolutionMethod
    {
        Linear,
        Quadratic,
        DoubleHashing,
        Polynomial,
        Simple
    }
    
    public class OpenAddressingHashTable<K, V> : IHashTable<K, V> where K : class where V : class
    {
        private readonly int size;
        private readonly (K key, V value)?[] table;
        private int count;
        private readonly CollisionResolutionMethod collisionMethod;

        public OpenAddressingHashTable(int size, CollisionResolutionMethod method)
        {
            this.size = size;
            table = new (K key, V value)?[size];
            count = 0;
            collisionMethod = method;
        }

        public V Search(K key)
        {
            int index = Hash(key);
            for (int i = 0; i < size; i++)
            {
                int probeIndex = GetProbeIndex(key, index, i);
                if (table[probeIndex] == null) return default;
                if (table[probeIndex]?.key.Equals(key) == true)
                    return table[probeIndex]?.value;
            }
            throw new KeyNotFoundException("Ключ не найден.");
        }

        public void Insert(K key, V value)
        {
            if (count >= size) throw new ArgumentException("Таблица полна.");
            int index = Hash(key);

            for (int i = 0; i < size; i++)
            {
                int probeIndex = GetProbeIndex(key, index, i);
        
                // Проверка на выход за пределы массива
                if (probeIndex < 0 || probeIndex >= size)
                {
                    throw new IndexOutOfRangeException("Индекс выходит за пределы массива.");
                }

                if (table[probeIndex] == null || table[probeIndex]?.key.Equals(key) == true)
                {
                    table[probeIndex] = (key, value);
                    count++;
                    return;
                }
            }
            throw new ArgumentException("Не удалось вставить элемент.");
        }


        public bool Delete(K key)
        {
            int index = Hash(key);
            for (int i = 0; i < size; i++)
            {
                int probeIndex = GetProbeIndex(key, index, i);
                if (table[probeIndex] == null) return false;
                if (table[probeIndex]?.key.Equals(key) == true)
                {
                    table[probeIndex] = null;
                    count--;
                    return true;
                }
            }
            return false;
        }

        public void Print()
        {
            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    Console.WriteLine($"Индекс {i}: Ключ = {table[i]?.key}, Значение = {table[i]?.value}");
                }
            }
        }

        private int Hash(K key)
        {
            return key.GetHashCode() % size;
        }

        private int GetProbeIndex(K key, int index, int attempt)
        {
            int probeIndex = collisionMethod switch
            {
                CollisionResolutionMethod.Linear => CollisionResolutionMethods.LinearProbing(index, attempt) % size,
                CollisionResolutionMethod.Quadratic => CollisionResolutionMethods.QuadraticProbing(index, attempt) % size,
                CollisionResolutionMethod.DoubleHashing => CollisionResolutionMethods.DoubleHashing(key, size, attempt),
                CollisionResolutionMethod.Polynomial => CollisionResolutionMethods.PolynomialHash(key.ToString(), size) + attempt % size,
                CollisionResolutionMethod.Simple => CollisionResolutionMethods.SimpleSumHash(key.ToString(), size) + attempt % size, // Используем SimpleSumHash
                _ => throw new InvalidOperationException("Неизвестный метод разрешения коллизий.")
            };

            return (probeIndex + size) % size; // Гарантируем положительный индекс
        }
        
        public int LongestClusterLength()
        {
            int maxLength = 0; 
            int currentLength = 0; 

            for (int i = 0; i < size; i++)
            {
                if (table[i] != null) 
                {
                    currentLength++; 
                }
                else
                {
                    
                    
                    if (currentLength > maxLength)
                    {
                        maxLength = currentLength;
                    }
                    currentLength = 0; 
                }
            }

            
            if (currentLength > maxLength)
            {
                maxLength = currentLength;
            }

            return maxLength; 
        }
    }
}
