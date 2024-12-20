namespace HashTables.Models
{
    public interface IHashTable<K, V>
    {
        V Search(K key);
        void Insert(K key, V value);
        bool Delete(K key);
        void Print();
    }
}