namespace HashTables.Models
{
    public static class HashFunctions
    {
        public static int StringKeyToInt(string key)
        {
            int result = 0;
            int baseValue = 128;

            for (int i = 0; i < key.Length; i++)
            {
                result = result * baseValue + key[i];
            }

            return result;
        }

        public static int DivisionFunction(string key, int size)
        {
            int intKey = StringKeyToInt(key);

            return Math.Abs(intKey % size);
        }

        public static int MultiplicationFunction(string key, int size)
        {
            int intKey = StringKeyToInt(key);
            double A = 0.6180339887;

            return Math.Abs((int)(size * (intKey * A % 1)));
        }

        public static int XORFunction(string key, int size)
        {
            int hash = 0;

            foreach (char c in key)
            {
                hash ^= c;
            }

            return Math.Abs(hash % size);
        }


        public static int DJB2Function(string key, int size)
        {
            int hash = 5381;

            foreach (char symbol in key)
            {
                hash = (hash << 5) + hash + symbol;
            }

            return Math.Abs(hash % size);
        }

        public static int FNV1AFunction(string key, int size)
        {
            const uint FNVPrime = 16777619;
            const uint OffsetBasis = 2166136261;
            uint hash = OffsetBasis;

            foreach (char symbol in key)
            {
                hash ^= symbol;
                hash *= FNVPrime;
            }

            return Math.Abs((int)(hash % size));
        }

        public static int JenkinsFunction(string key, int size)
        {
            uint hash = 0;

            foreach (char symbol in key)
            {
                hash += symbol;
                hash += hash << 10;
                hash ^= hash >> 6;
            }

            hash += hash << 3;
            hash ^= hash >> 11;
            hash += hash << 15;

            return Math.Abs((int)(hash % size));
        }

        public static int StandartFunction(string key, int size)
        {
            return Math.Abs(key.GetHashCode() % size);
        }

        public static int LengthFunction(string key, int size)
        {
            return key.Length % size;
        }
    }
}
