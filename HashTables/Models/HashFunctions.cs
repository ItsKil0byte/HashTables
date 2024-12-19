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

        public static int CustomFunction(string key, int size)
        {
            int intKey = StringKeyToInt(key);
            
            // Потом что-нибудь для неё придумаю

            return Math.Abs(intKey * 2 % size);
        }
    }
}
