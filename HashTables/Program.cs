using HashTables.Models;
using System.Reflection.Emit;

namespace HashTables
{
    public class Program
    {
        // Размер таблички для первого задания.
        private static readonly int chainSize = 1000;
        // Размер таблички для второго задания. Простое число для двойного хэширования.
        private static readonly int openSize = 10001;

        //Переключение режима тестирования
        private static bool testingMode = false;

        static void Main()
        {
           while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите опцию:\n");
                Console.WriteLine("1. Хэш-таблица с цепочками.");
                Console.WriteLine("2. Хэш-таблица с открытой адресацией.");
                Console.WriteLine("3. Запустить тесты.");
                Console.WriteLine("0. Выход.\n");

                Console.Write("Введите число от 0 до 3: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int userInput) && userInput >= 0 && userInput <= 3)
                {
                    switch (userInput)
                    {
                        case 0:
                            Console.WriteLine("\nЗавершение программы...");
                            return;
                        case 1:
                            var chainFunction = GetChainHashTableFunc();
                            if (chainFunction != null)
                            {
                                var hashTable = new ChainHashTable<string, string>(chainSize, chainFunction);
                                if (testingMode)
                                {
                                    RunHashTesting(hashTable);
                                }
                                else
                                {
                                    RunHashMenu(hashTable);
                                }
                            }
                            break;
                        case 2:
                            var hashFunction = GetOpenHashTableFunc();
                            if (hashFunction != null)
                            {
                                var collisionMethod = GetOpenAddressingCollisionMethod();
                                var hashTableOpen = new OpenAddressingHashTable<string, string>(openSize, collisionMethod, hashFunction);
                                if (testingMode)
                                {
                                    RunHashTesting(hashTableOpen);
                                }
                                else
                                {
                                    RunHashMenu(hashTableOpen);
                                }
                            }
                            break;
                        case 3:
                            TestingMenu();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 3.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
        }


        private static CollisionResolutionMethod GetOpenAddressingCollisionMethod()
        {
            int userInput;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите метод разрешения коллизий:\n");
                Console.WriteLine("1. Линейное пробирование.");
                Console.WriteLine("2. Квадратичное пробированиее.");
                Console.WriteLine("3. Двойное хеширование.");
                Console.WriteLine("4. Пробирование с расстоянием.");
                Console.WriteLine("5. Разреженное пробирование.");
                Console.WriteLine("0. Вернуться в главное меню.\n");

                Console.Write("Введите число от 0 до 5: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out userInput) && userInput >= 0 && userInput <= 5)
                {
                    if (userInput == 0)
                    {
                        return CollisionResolutionMethod.Linear;
                    }
                    else
                    {
                        return userInput switch
                        {
                            1 => CollisionResolutionMethod.Linear,
                            2 => CollisionResolutionMethod.Quadratic,
                            3 => CollisionResolutionMethod.DoubleHashing,
                            4 => CollisionResolutionMethod.Distance,
                            5 => CollisionResolutionMethod.Sparse,
                            _ => throw new InvalidOperationException("Неизвестный метод.")
                        };
                    }
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 5.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
        }

        private static void TestingMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите опцию:\n");
                Console.WriteLine("1. Запустить тесты хеш-таблиц с адресацией цепочками.");
                Console.WriteLine("2. Запустить тесты хеш-таблиц с открытой адресацией.");
                if (!testingMode)
                {
                    Console.WriteLine("3. Запустить режим ручного тестирования.");
                }
                else
                {
                    Console.WriteLine("3. Отключить режим ручного тестирования.");
                }
                Console.WriteLine("0. Назад.\n");

                Console.Write("Введите число от 0 до 3: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int userInput) && userInput >= 0 && userInput <= 3)
                {
                    switch (userInput)
                    {
                        case 0:
                            return;
                        case 1:
                            PrintChainHashTableTests();
                            break;
                        case 2:
                            PrintOpenHashTableTests();
                            break;
                        case 3:
                            testingMode = !testingMode;
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 3.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
        }

        private static void PrintOpenHashTableTests()
        {
            int itemsCount;

            while (true)
            {
                Console.Clear();
                Console.Write("Введите количество элементов для вставки: \n");
                string input = Console.ReadLine();

                if (int.TryParse(input, out itemsCount))
                {
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("Некоректное значение, введите число");
                    Console.ReadKey();
                }
            }

            Dictionary<string, Func<string, int>> hashFunctions = new Dictionary<string, Func<string, int>>()
            {
                { "Деление", (key) => HashFunctions.DivisionFunction(key, openSize)},
                { "Умножение", (key) => HashFunctions.MultiplicationFunction(key, openSize)},
                { "Полиномиальная", (key) => HashFunctions.PolynomialHashFunction(key, openSize)},
                { "DJB2", (key) => HashFunctions.DJB2Function(key, openSize)},
                { "FNV-1a", (key) => HashFunctions.FNV1AFunction(key, openSize)},
                { "Jenkins", (key) => HashFunctions.JenkinsFunction(key, openSize) },
                { "Стандартная", (key) => HashFunctions.StandartFunction(key, openSize) }
            };

            Dictionary<string, CollisionResolutionMethod> resolutionMethods = new Dictionary<string, CollisionResolutionMethod>()
            {
                { "Линейный", CollisionResolutionMethod.Linear },
                { "Квадратичный", CollisionResolutionMethod.Quadratic },
                { "Двойное хеширование", CollisionResolutionMethod.DoubleHashing },
                { "Расстояние", CollisionResolutionMethod.Distance },
                { "Разрежённое", CollisionResolutionMethod.Sparse }

            };


            Console.WriteLine($"Выбранное количество элементов: {itemsCount}");

            foreach (var resMethodName in resolutionMethods.Keys)
            {
                Console.WriteLine(resMethodName);

                string header = string.Format(
                "| {0,-15} | {1,-10} | {2,-25} | {3,-20} | {4,-20} | {5,-20} |",
                "Пробирование", "Размер", "Коэффициент заполнения", "Длиннейшая цепочка", "Кратчайшая цепочка", "Рехеширований"
                );


                string separator = new string('-', header.Length);

                Console.WriteLine(separator);
                Console.WriteLine(header);
                Console.WriteLine(separator);
                foreach (var hashFuncName in hashFunctions.Keys)
                {
                    var ht = new OpenAddressingHashTable<string, string>(openSize, resolutionMethods[resMethodName], hashFunctions[hashFuncName]);

                    var randomItems = new RandomStringIterator(5);

                    foreach (var i in randomItems.Take(itemsCount))
                    {
                        ht.Insert(i.Item1, i.Item2);
                    }

                    Console.WriteLine(
                        string.Format(
                            "| {0,-15} | {1,-10} | {2,-25} | {3,-20} | {4,-20} | {5,-20} |",
                            hashFuncName, ht.Size, ht.FillPercentage(), ht.LongestClusterLength(), ht.ShortestClusterLength(), ht.timesRehashed
                            ));
                    Console.WriteLine(separator);
                    
                }
                Console.WriteLine("\n");
            }
            Console.ReadLine();
            return;

        }

        private static void PrintChainHashTableTests()
        {
            int itemsCount;

            while (true)
            {
                Console.Clear();
                Console.Write("Введите количество элементов для вставки: \n");
                string input = Console.ReadLine();

                if (int.TryParse(input, out itemsCount))
                {
                    Console.Clear();
                    break;
                }
                else { 
                    Console.WriteLine("Некоректное значение, введите число"); 
                    Console.ReadKey();
                }
            }

            Dictionary<string, ChainHashTable<string, string>> chainHashTables = new Dictionary<string, ChainHashTable<string, string>>()
            {
                { "Деление", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.DivisionFunction(key, chainSize)) },
                { "Умножение", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.MultiplicationFunction(key, chainSize)) },
                { "Полиномиальная", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.PolynomialHashFunction(key, chainSize)) },
                { "DJB2", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.DJB2Function(key, chainSize)) },
                { "FNV-1a", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.FNV1AFunction(key, chainSize)) },
                { "Jenkins", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.JenkinsFunction(key, chainSize)) },
                { "Стандартная", new ChainHashTable<string, string>(chainSize, (key) => HashFunctions.StandartFunction(key, chainSize)) }
            };


            Console.WriteLine($"Выбранное количество элементов: {itemsCount}");

            string header = string.Format(
            "| {0,-15} | {1,-10} | {2,-25} | {3,-20} | {4,-20} |",
            "Название", "Размер", "Коэффициент заполнения", "Длиннейшая цепочка", "Кратчайшая цепочка"
            );

            string separator = new string('-', header.Length);

            Console.WriteLine(separator);
            Console.WriteLine(header);
            Console.WriteLine(separator);
            foreach (var key in chainHashTables.Keys) {
                var ht = chainHashTables[key];

                var randomItems = new RandomStringIterator(5);

                foreach(var i in randomItems.Take(itemsCount))
                {
                    ht.Insert(i.Item1, i.Item2);
                }

                Console.WriteLine(
                    string.Format(
                        "| {0,-15} | {1,-10} | {2,-25} | {3,-20} | {4,-20} |",
                        key, chainSize, $"{ht.CalculateLoadFactor():F4}", ht.GetLongestChainLength(), ht.GetShortestChainLength()
                        ));
                Console.WriteLine(separator);
            }

            Console.ReadLine();
            return;
        }

        private static Func<string, int> GetChainHashTableFunc()
        {
            int userInput;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите хэш-функцию:\n");
                Console.WriteLine("1. Метод деления.");
                Console.WriteLine("2. Метод умножения.");
                Console.WriteLine("3. Метод полинома.");
                Console.WriteLine("4. Функция DJB2.");
                Console.WriteLine("5. Функция FNV-1A.");
                Console.WriteLine("6. Функция Дженкинса.");
                Console.WriteLine("7. Стандартная функция C#.");
                Console.WriteLine("0. Вернуться в главное меню.\n");

                Console.Write("Введите число от 0 до 8: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out userInput) && userInput >= 0 && userInput <= 7)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 7.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }

            return userInput switch
            {
                1 => key => HashFunctions.DivisionFunction(key, chainSize),
                2 => key => HashFunctions.MultiplicationFunction(key, chainSize),
                3 => key => HashFunctions.PolynomialHashFunction(key, chainSize),
                4 => key => HashFunctions.DJB2Function(key, chainSize),
                5 => key => HashFunctions.FNV1AFunction(key, chainSize),
                6 => key => HashFunctions.JenkinsFunction(key, chainSize),
                7 => key => HashFunctions.StandartFunction(key, chainSize),
                0 => null,
            };
        }

        // Копипаст, извините, делаю на коленке. Её использовать для открытой адресации.
        private static Func<string, int> GetOpenHashTableFunc()
        {
            int userInput;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите хэш-функцию:\n");
                Console.WriteLine("1. Метод деления.");
                Console.WriteLine("2. Метод умножения.");
                Console.WriteLine("3. Метод полинома.");
                Console.WriteLine("4. Функция DJB2.");
                Console.WriteLine("5. Функция FNV-1A.");
                Console.WriteLine("6. Функция Дженкинса.");
                Console.WriteLine("7. Стандартная функция C#.");
                Console.WriteLine("0. Вернуться в главное меню.\n");

                Console.Write("Введите число от 0 до 8: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out userInput) && userInput >= 0 && userInput <= 7)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 7.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }

            return userInput switch
            {
                1 => key => HashFunctions.DivisionFunction(key, openSize),
                2 => key => HashFunctions.MultiplicationFunction(key, openSize),
                3 => key => HashFunctions.PolynomialHashFunction(key, openSize),
                4 => key => HashFunctions.DJB2Function(key, openSize),
                5 => key => HashFunctions.FNV1AFunction(key, openSize),
                6 => key => HashFunctions.JenkinsFunction(key, openSize),
                7 => key => HashFunctions.StandartFunction(key, openSize),
                0 => null,
            };
        }

        private static void RunHashTesting(IHashTable<string, string> hashTable)
        {
            Console.Clear();

            int itemsCount = hashTable is ChainHashTable<string, string> ? openSize : chainSize;
            var items = new RandomStringIterator(10);

            foreach (var item in items.Take(itemsCount))
            {
                hashTable.Insert(item.Item1, item.Item2);
            }

            if (hashTable is ChainHashTable<string, string> chainHashTable)
            {
                Console.WriteLine($"\nКоэффициент заполнения: {chainHashTable.CalculateLoadFactor():F4}%");
                Console.WriteLine($"Самая длинная цепочка: {chainHashTable.GetLongestChainLength()}");
                Console.WriteLine($"Самая короткая цепочка: {chainHashTable.GetShortestChainLength()}");
            }

            else if (hashTable is OpenAddressingHashTable<string, string> openAddressingHashTable)
            {
                int longestClusterLength = openAddressingHashTable.LongestClusterLength();
                int shortestClusterLength = openAddressingHashTable.ShortestClusterLength();
                Console.WriteLine($"\nДлина самого длинного кластера: {longestClusterLength}");
                Console.WriteLine($"Длина самого короткого кластера: {shortestClusterLength}");
                Console.WriteLine($"Размер таблицы: {openAddressingHashTable.Size}");
                Console.WriteLine($"Заполненные ячейки: {openAddressingHashTable.FilledCellsCount()}");
                Console.WriteLine($"Процент заполнения: {openAddressingHashTable.FillPercentage():F4}%");
            }

            Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
            Console.ReadKey();
        }

        private static void RunHashMenu(IHashTable<string, string> hashTable)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Меню работы с хэш-таблицей:\n");
                Console.WriteLine("1. Вставить элемент.");
                Console.WriteLine("2. Найти элемент.");
                Console.WriteLine("3. Удалить элемент.");
                Console.WriteLine("4. Вывод таблицы.");
                Console.WriteLine("0. Вернуться в главное меню.\n");

                Console.Write("Введите число от 0 до 4: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int userInput) && userInput >= 0 && userInput <= 5)
                {
                    switch (userInput)
                    {
                        case 0:
                            return;
                        case 1:
                            InsertElement(hashTable);
                            break;
                        case 2:
                            SearchElement(hashTable);
                            break;
                        case 3:
                            DeleteElement(hashTable);
                            break;
                        case 4:
                            Console.Clear();
                            hashTable.Print();

                            if (hashTable is ChainHashTable<string, string> chainHashTable)
                            {
                                Console.WriteLine($"\nКоэффициент заполнения: {chainHashTable.CalculateLoadFactor()}");
                                Console.WriteLine($"Самая длинная цепочка: {chainHashTable.GetLongestChainLength()}");
                                Console.WriteLine($"Самая короткая цепочка: {chainHashTable.GetShortestChainLength()}");
                            }

                            else if (hashTable is OpenAddressingHashTable<string, string> openAddressingHashTable)
                            {
                                int longestClusterLength = openAddressingHashTable.LongestClusterLength();
                                int shortestClusterLength = openAddressingHashTable.ShortestClusterLength();
                                int tableSize = openAddressingHashTable.Size;
                                Console.WriteLine($"\nДлина самого длинного кластера: {longestClusterLength}");
                                Console.WriteLine($"Длина самого короткого кластера: {shortestClusterLength}");
                                Console.WriteLine($"Размер таблицы: {tableSize}");
                                Console.WriteLine($"Заполненные ячейки: {openAddressingHashTable.FilledCellsCount()}");
                                Console.WriteLine($"Процент заполнения: {openAddressingHashTable.FillPercentage():F4}%");
                            }

                            break;
                    }

                    Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 5.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }
        }



        private static void InsertElement(IHashTable<string, string> hashTable)
        {
            Console.Clear();
            Console.WriteLine("Вставка элемента в хэш-таблицу.\n");
            Console.Write("Введите ключ: ");
            string key = Console.ReadLine();
            Console.Write("Введите значение: ");
            string value = Console.ReadLine();

            try
            {
                hashTable.Insert(key, value);
                Console.WriteLine($"\nЭлемент с ключом '{key}' успешно добавлен.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        private static void SearchElement(IHashTable<string, string> hashTable)
        {
            Console.Clear();
            Console.WriteLine("Поиск элемента в хэш-таблице.\n");
            Console.Write("Введите ключ: ");
            string key = Console.ReadLine();

            try
            {
                string value = hashTable.Search(key);
                Console.WriteLine($"\nЗначение для ключа '{key}': {value}");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"\nОшибка: {ex.Message}");
            }
        }

        private static void DeleteElement(IHashTable<string, string> hashTable)
        {
            Console.Clear();
            Console.WriteLine("Удаление элемента из хэш-таблицы.\n");
            Console.Write("Введите ключ: ");
            string key = Console.ReadLine();

            if (hashTable.Delete(key))
            {
                Console.WriteLine($"\nЭлемент с ключом '{key}' успешно удалён.");
            }
            else
            {
                Console.WriteLine($"\nОшибка: элемент с ключом '{key}' не найден.");
            }
        }
    }
}
