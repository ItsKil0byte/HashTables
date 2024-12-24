using HashTables.Models;

namespace HashTables
{
    public enum CollisionResolutionMethod
    {
        Linear,
        Quadratic,
        DoubleHashing,
        Distance,
        Sparse    
    }

    
    public class OpenAddressingHashTable<K, V> : IHashTable<K, V> where K : class where V : class
    {
        private int size;
        private (K key, V value)?[] table;
        private int count;
        private readonly CollisionResolutionMethod collisionMethod;
        private readonly Func<K, int> hashFunction;

        public int timesRehashed { get; private set; } = 0;

        public OpenAddressingHashTable(int initialSize, CollisionResolutionMethod method, Func<K, int> hashFunction)
        {
            size = initialSize;
            table = new (K key, V value)?[size];
            count = 0;
            collisionMethod = method;
            this.hashFunction = hashFunction; 
        }
        
        public int Size => size;

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
            if (count >= size * 0.7) 
            {
                Resize(); 
            }
        
            int index = Hash(key);

            for (int i = 0; i < size; i++)
            {
                int probeIndex = GetProbeIndex(key, index, i);
                
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

        private void Resize()
        {
            timesRehashed++;

            int newSize = size * 2;
            var newTable = new (K key, V value)?[newSize];
            
            for (int i = 0; i < size; i++)
            {
                if (table[i] != null)
                {
                    int newIndex = Math.Abs(hashFunction(table[i]?.key) % newSize);
                    for (int j = 0; j < newSize; j++)
                    {
                        int probeIndex = GetProbeIndex(table[i]?.key, newIndex, j);
                        if (newTable[probeIndex] == null)
                        {
                            newTable[probeIndex] = table[i];
                            break;
                        }
                    }
                }
            }

            
            size = newSize;
            table = newTable;
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
        public string FilledCellsCount()
        {
            return $"{count}/{size}"; 
        }
        
        public double FillPercentage()
        {
            if (size == 0) return 0;
            return (double)count / size * 100; 
        }


        private int Hash(K key)
        {
            return Math.Abs(hashFunction(key) % size); 
        }

        private int GetProbeIndex(K key, int index, int attempt)
        {
            int probeIndex = collisionMethod switch
            {
                CollisionResolutionMethod.Linear => CollisionResolutionMethods.LinearProbing(index, attempt, size),
                CollisionResolutionMethod.Quadratic => CollisionResolutionMethods.QuadraticProbing(index, attempt, size, 1, 3),
                CollisionResolutionMethod.DoubleHashing => CollisionResolutionMethods.DoubleHashing(key, size, attempt, hashFunction),
                CollisionResolutionMethod.Distance => CollisionResolutionMethods.DistanceProbing(index, attempt) % size,
                CollisionResolutionMethod.Sparse => CollisionResolutionMethods.SparseProbing(index, attempt) % size,
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
        
        public int ShortestClusterLength()
        {
            int minLength = int.MaxValue; 
            int currentLength = 0; 
            bool inCluster = false;

            for (int i = 0; i < size; i++)
            {
                if (table[i] != null) 
                {
                    currentLength++;
                    inCluster = true; 
                }
                else
                {
                    if (inCluster)
                    {
                        if (currentLength < minLength)
                        {
                            minLength = currentLength; 
                        }
                        currentLength = 0; 
                        inCluster = false; 
                    }
                }
            }

            // Проверка последнего кластера в конце таблицы
            if (inCluster && currentLength < minLength)
            {
                minLength = currentLength; 
            }

            // Если не было кластеров, возвращаем 0
            return minLength == int.MaxValue ? 0 : minLength; 
        }
    }
}
