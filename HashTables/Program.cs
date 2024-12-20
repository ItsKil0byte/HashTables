﻿using HashTables.Models;

namespace HashTables
{
    public class Program
    {
        // Размер табличек для "поиграться" в менюшках.
        private static readonly int userSize = 10; 

        static void Main()
        {
            // Размер хэш-таблицы
            const int hashTableSize = 10000;

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
                                var hashTable = new ChainHashTable<string, string>(userSize, chainFunction);
                                RunHashMenu(hashTable);
                            }
                            break;
                        case 2:
                            var collisionMethod = GetOpenAddressingCollisionMethod();
                            var hashTableOpen = new OpenAddressingHashTable<string, string>(hashTableSize, collisionMethod, key => key.Length);
                            RunHashMenu(hashTableOpen);
                            break;
                        case 3:
                            // Логика для тестов
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
                Console.WriteLine("1. Линейное исследование.");
                Console.WriteLine("2. Квадратичное исследование.");
                Console.WriteLine("3. Двойное хеширование.");
                Console.WriteLine("4. Полиномиальное хеширование.");
                Console.WriteLine("5. Хеширование на основе простых чисел.");
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
                            4 => CollisionResolutionMethod.Polynomial,
                            5 => CollisionResolutionMethod.Simple,
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
        
        private static Func<string, int> GetChainHashTableFunc()
        {
            int userInput;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите хэш-функцию:\n");
                Console.WriteLine("1. Метод деления.");
                Console.WriteLine("2. Метод умножения.");
                Console.WriteLine("3. Метод исключающего или.");
                Console.WriteLine("4. Функция DJB2.");
                Console.WriteLine("5. Функция FNV-1A.");
                Console.WriteLine("6. Функция Дженкинса.");
                Console.WriteLine("7. Стандартная функция C#.");
                Console.WriteLine("8. Стандартная функция на основе длинны ключа.");
                Console.WriteLine("0. Вернуться в главное меню.\n");

                Console.Write("Введите число от 0 до 8: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out userInput) && userInput >= 0 && userInput <= 8)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\nОшибка: введите корректное число от 0 до 8.");
                    Console.WriteLine("Нажмите любую клавишу, чтобы продолжить...");
                    Console.ReadKey();
                }
            }

            return userInput switch
            {
                1 => key => HashFunctions.DivisionFunction(key, userSize),
                2 => key => HashFunctions.MultiplicationFunction(key, userSize),
                3 => key => HashFunctions.XORFunction(key, userSize),
                4 => key => HashFunctions.DJB2Function(key, userSize),
                5 => key => HashFunctions.FNV1AFunction(key, userSize),
                6 => key => HashFunctions.JenkinsFunction(key, userSize),
                7 => key => HashFunctions.StandartFunction(key, userSize),
                8 => key => HashFunctions.LengthFunction(key, userSize),
                0 => null,
            };
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

                Console.Write("Введите число от 0 до 5: ");
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
                                Console.WriteLine($"\nДлина самого длинного кластера: {longestClusterLength}");
                                Console.WriteLine($"Длина самого короткого кластера: {shortestClusterLength}"); 
                                Console.WriteLine($"Размер таблицы: {openAddressingHashTable.Size}"); 
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
