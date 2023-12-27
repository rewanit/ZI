using System.Text;
using ConsoleProgress;


namespace Lr3
{

    

    class Program
    {
        static char[] alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ .-".ToCharArray();
        static int M = alphabet.Length;


        /// <summary>
        /// 36 символов (А…Я, пробел, «.», «-» )
        /// 3)	аффинная система подстановок Цезаря
        /// A (сомножитель)
        /// </summary>
        static void Main()
        {
            string plaintext = File.ReadAllText("text.txt").ToUpper().Trim().Replace("\n","").Replace("\t", "");
            Console.WriteLine("Длинна словаря: "+M);
            Console.WriteLine("Введите значение A для ключа шифрования (целое число):");
            int A = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите значение K для ключа шифрования (целое число):");
            int K = int.Parse(Console.ReadLine());

            string ciphertext = Encrypt(plaintext, A, K);

            Console.WriteLine($"Шифрованный текст: {ciphertext}");

            // Сохранение результатов шифрования в файл
            File.WriteAllText("ciphertext.txt", ciphertext);

            Console.WriteLine("Начинаем криптоанализ...");

            // Чтение шифртекста из файла
            string encryptedTextFromFile = File.ReadAllText("ciphertext.txt");

            // Криптоанализ
            int decryptedKeyA, decryptedKeyK;
            Cryptoanalysis(encryptedTextFromFile, out decryptedKeyA, out decryptedKeyK);

            Console.WriteLine($"Определенные ключи: A={decryptedKeyA}, K={decryptedKeyK}");

            // Расшифрование с использованием определенного ключа
            string decryptedText = Decrypt(encryptedTextFromFile, decryptedKeyA, decryptedKeyK);

            Console.WriteLine($"Расшифрованный текст: {decryptedText}");

            // Сохранение результатов криптоанализа в файл
            File.WriteAllText("decryption_results.txt", $"Определенные ключи: A={decryptedKeyA}, K={decryptedKeyK}\nРасшифрованный текст: {decryptedText}");

            Console.ReadLine();
        }

        static void PrintRecommendedKeys()
        {

            Console.WriteLine("A, K:");

            // Перебор возможных ключей A и K, удовлетворяющих условиям
            for (int potentialA = 1; potentialA < M; potentialA++)
            {
                for (int potentialK = 0; potentialK < M; potentialK++)
                {
                    // Проверка условий
                    if (IsConditionSatisfied(potentialA, potentialK, M) )
                    {
                        Console.WriteLine($"{potentialA}, {potentialK}");
                    }
                }
            }
        }

        static bool IsConditionSatisfied(int A, int K, int M)
        {
            // Проверка условий: 0 ? (A, J)? (M-1), 0 ? K ? (M-1), НОД (A, M)=1
            return A >= 0 && A < M && K >= 0 && K < M && GreatestCommonDivisor(A, M) == 1;
        }

        static int GreatestCommonDivisor(int a, int b)
        {
            // Алгоритм Евклида для нахождения НОД
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        static string Encrypt(string text, int A, int K)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in text)
            {
                if (alphabet.Contains(symbol))
                {
                    int index = Array.IndexOf(alphabet, symbol);
                    int encryptedIndex = ((A * index) + K) % M;
                    result.Append(alphabet[encryptedIndex]);
                }
                }

            return result.ToString();
        }

        static string Decrypt(string text, int A, int K)
        {
            StringBuilder result = new StringBuilder();

            foreach (char symbol in text)
            {
                if (alphabet.Contains(symbol))
                {
                    int index = Array.IndexOf(alphabet, symbol);
                    // Формула расшифровки: I = A^(-1) * (J - K) mod M, где A^(-1) - обратное по модулю A
                    int decryptedIndex = (ModularMultiplicativeInverse(A, M) * (index - K + M)) % M;
                    result.Append(alphabet[decryptedIndex]);
                }
                }

            return result.ToString();
        }

        static void Cryptoanalysis(string ciphertext, out int decryptedKeyA, out int decryptedKeyK)
        {
            decryptedKeyA = 0;
            decryptedKeyK = 0;
            double minW = double.MaxValue;

            var progress = new ProgressBar();
            int i = 1;
            // Перебор возможных ключей A и K
            for (int potentialA = 1; potentialA < M; potentialA++)
            {
                if (GreatestCommonDivisor(potentialA, M) == 1)
                    for (int potentialK = 0; potentialK < M; potentialK++)
                {
                    // Расшифрование текста с текущими ключами
                    string decryptedText = Decrypt(ciphertext, potentialA, potentialK);

                    // Вычисление степени расхождения статистики
                    double W = CalculateW(decryptedText);

                        // Если текущее значение W меньше минимального, обновляем ключи и минимальное значение W
                    if (W < minW)
                    {
                        minW = W;
                        decryptedKeyA = potentialA;
                        decryptedKeyK = potentialK;
                    }
                    //Console.WriteLine(i++);
                    double progressValue = (double)(potentialA * M + potentialK) / (M * M);
                    progress.Report(progressValue);

                }

                // Отображение прогресса
            }
            Console.WriteLine();
            Console.WriteLine(minW);
            // Завершение отображения прогресса
        }

        static int ModularMultiplicativeInverse(int a, int m)
        {
            // Реализация вычисления обратного элемента по модулю (расширенный алгоритм Евклида)
            int m0 = m, t, q;
            int x0 = 0, x1 = 1;

            if (m == 1)
                return 0;
            try
            {
                while (a > 1)
                {
                    q = a / m;
                    t = m;

                    m = a % m;
                    a = t;

                    t = x0;
                    x0 = x1 - q * x0;
                    x1 = t;
                }
            }
            catch (Exception)
            {
            }
            

            if (x1 < 0)
                x1 += m0;

            return x1;
        }

        
        static Dictionary<char, double> letterFrequenciesInRussian = new Dictionary<char, double>
        {
            { 'А', 0.062 },
            { 'Б', 0.014 },
            { 'В', 0.038 },
            { 'Г', 0.013 },
            { 'Д', 0.025 },
            { 'Е', 0.072 },
            { 'Ё', 0.072 },
            { 'Ж', 0.007 },
            { 'З', 0.016 },
            { 'И', 0.062 },
            { 'Й', 0.010 },
            { 'К', 0.028 },
            { 'Л', 0.035 },
            { 'М', 0.026 },
            { 'Н', 0.053 },
            { 'О', 0.090 },
            { 'П', 0.023 },
            { 'Р', 0.040 },
            { 'С', 0.045 },
            { 'Т', 0.053 },
            { 'У', 0.021 },
            { 'Ф', 0.002 },
            { 'Х', 0.009 },
            { 'Ц', 0.004 },
            { 'Ч', 0.012 },
            { 'Ш', 0.006 },
            { 'Щ', 0.003 },
            { 'Ъ', 0.014 },
            { 'Ы', 0.016 },
            { 'Ь', 0.014 },
            { 'Э', 0.003 },
            { 'Ю', 0.006 },
            { 'Я', 0.018 },
            { ' ', 0.175 }
        };

        static double CalculateW(string text)
        {
            double W = 0;

            // Подсчет частот букв в расшифрованном тексте
            Dictionary<char, int> letterFrequencies = new Dictionary<char, int>();
            foreach (char symbol in text)
            {
                if (letterFrequenciesInRussian.ContainsKey(symbol))
                {
                    if (letterFrequencies.ContainsKey(symbol))
                    {
                        letterFrequencies[symbol]++;
                    }
                    else
                    {
                        letterFrequencies[symbol] = 1;
                    }
                }
            }

            // Вычисление степени расхождения статистики
            foreach (var kvp in letterFrequencies)
            {
                char symbol = kvp.Key;
                int count = kvp.Value;

                double expectedProbability = (double)count / text.Length;
                double actualProbability = letterFrequenciesInRussian[symbol];

                W += Math.Pow(actualProbability - expectedProbability, 2);
            }

            return W;
        }
    }

}
