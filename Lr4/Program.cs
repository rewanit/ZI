namespace Lr4
{
    using System;

    using System;

    class Program
    {
        static void Main()
        {
            // Инициализация первой ступени (7-разрядный линейный сдвиговый регистр)
            ushort firstStageRegister = (ushort)0xACE1u; // Произвольное начальное значение

            // Инициализация второй ступени (конгруэнтный генератор)
            ulong secondStageSeed = GenerateSecondStageSeed(firstStageRegister);
            ulong secondStageState = secondStageSeed;

            // Генерация гаммы и гаммирование текста
            string plaintext = "Hello, World!";
            Console.WriteLine($"Исходный текст: {plaintext}");
            Console.WriteLine("Бинарное представление исходного текста: " + StringToBinary(plaintext));

            string ciphertext = Encrypt(plaintext, firstStageRegister, secondStageState);
            Console.WriteLine($"Зашифрованный текст: {ciphertext}");
            Console.WriteLine("Бинарное представление зашифрованного текста: " + StringToBinary(ciphertext));

            string decryptedText = Decrypt(ciphertext, firstStageRegister, secondStageState);
            Console.WriteLine($"Расшифрованный текст: {decryptedText}");
            Console.WriteLine("Бинарное представление расшифрованного текста: " + StringToBinary(decryptedText));
        }

        // Генерация 64-битного значения для второй ступени
        static ulong GenerateSecondStageSeed(ushort firstStageRegister)
        {
            ulong seed = firstStageRegister;
            seed <<= 9; // Сдвиг на 9 бит для получения 16 бит значения
            return seed;
        }

        // Конгруэнтный генератор для второй ступени
        static ulong CongruentialGenerator(ulong state)
        {
            // Коэффициент "a" - большое простое число, обеспечивающее хорошую статистику
            const ulong a = 6364136223846793005;

            // Коэффициент "c" - константа, обеспечивающая разнообразие последовательности
            const ulong c = 1;

            // Модуль "m" - определяет период последовательности; должен быть степенью 2 для обеспечения равномерного распределения
            const ulong m = 1 << 16;

            // Рассчитываем новое состояние с использованием конгруэнтного генератора
            state = (a * state + c) % m;

            // Возвращаем новое состояние
            return state;
        }

        // Шифрование текста
        static string Encrypt(string plaintext, ushort firstStageRegister, ulong secondStageState)
        {
            char[] encryptedText = new char[plaintext.Length];

            for (int i = 0; i < plaintext.Length; i++)
            {
                // Генерация гаммы
                ulong gamma = CongruentialGenerator(secondStageState);

                // XOR операция для шифрования
                char encryptedChar = (char)(plaintext[i] ^ (char)gamma);

                // Обновление состояний для следующей итерации
                firstStageRegister = (ushort)(((firstStageRegister << 1) | ((firstStageRegister & 0x4000) >> 14)) & 0x7FFF);
                secondStageState = CongruentialGenerator(secondStageState);

                encryptedText[i] = encryptedChar;

                // Вывод состояний для отладки
                Console.WriteLine($"Шаг {i + 1}: Gamma = {gamma}\nFirst Stage Register = {ToBinaryString(firstStageRegister)}\nSecond Stage State   = {ToBinaryString(secondStageState)}");
            }

            return new string(encryptedText);
        }

        // Расшифрование текста
        static string Decrypt(string ciphertext, ushort firstStageRegister, ulong secondStageState)
        {
            // Дешифрование идентично шифрованию
            return Encrypt(ciphertext, firstStageRegister, secondStageState);
        }

        // Преобразование числа в строку бинарного представления
        static string ToBinaryString(ulong value)
        {
            return Convert.ToString((long)value, 2).PadLeft(64, '0');
        }

        // Преобразование строки в бинарное представление
        static string StringToBinary(string text)
        {
            string binaryString = "";
            foreach (char c in text)
            {
                binaryString += Convert.ToString(c, 2).PadLeft(8, '0') + " ";
            }
            return binaryString.Trim();
        }
    }


}
