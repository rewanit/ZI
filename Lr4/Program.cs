namespace Lr4
{
    using System;

    using System;
    using System.Text;

    using System;

    class LFSR
    {
        private int register;

        public LFSR()
        {
            Random random = new Random();
            register = random.Next(128); // 2^7 for 7-bit register
        }

        public int Shift()
        {
            int newBit = (register >> 6) ^ ((register >> 5) & 1);
            register = (newBit << 6) | (register >> 1);
            return newBit;
        }

        public Tuple<int, Tuple<string, int>[]> Generate16Bits()
        {
            int output = 0;
            Tuple<string, int>[] stepsOutput = new Tuple<string, int>[16];

            for (int i = 0; i < 16; i++)
            {
                int newBit = Shift();
                output = (output << 1) | newBit;
                stepsOutput[i] = Tuple.Create(Convert.ToString(register, 2).PadLeft(7, '0'), newBit);
            }

            return Tuple.Create(output, stepsOutput);
        }
    }

    class LinearCongruentialGenerator
    {
        private int state;
        private int a;
        private int b;

        public LinearCongruentialGenerator(int seed, int a = 1664525, int b = 1013904223)
        {
            state = seed;
            this.a = a;
            this.b = b;
        }

        public int Next()
        {
            state = (int)((state * (long)a + b) % Math.Pow(2, 16));
            return state;
        }

        public string GammaGenerate(int length)
        {
            string gamma = "";
            while (gamma.Length < length)
            {
                int nextValue = Next();
                Console.WriteLine($"Текущее состояние LCG: десятичное: {nextValue}, двоичный: {Convert.ToString(nextValue, 2).PadLeft(16, '0')}");
                gamma += Convert.ToString(nextValue, 2).PadLeft(16, '0');
            }
            return gamma.Substring(0, length);
        }
    }

    class Program
    {
        static string TextToBinary(string text)
        {
            char[] charArray = text.ToCharArray();
            string binary = "";
            foreach (char c in charArray)
            {
                binary += Convert.ToString(c, 2).PadLeft(8, '0');
            }
            return binary;
        }

        static string BinaryToText(string binary)
        {
            int numOfBytes = binary.Length / 8;
            byte[] byteArray = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                byteArray[i] = Convert.ToByte(binary.Substring(i * 8, 8), 2);
            }
            return System.Text.Encoding.ASCII.GetString(byteArray);
        }

        static string Encrypt(string messageBinary, string gamma)
        {
            char[] encryptedChars = new char[messageBinary.Length];
            for (int i = 0; i < messageBinary.Length; i++)
            {
                encryptedChars[i] = (messageBinary[i] != gamma[i]) ? '1' : '0';
            }
            return new string(encryptedChars);
        }

        static string Decrypt(string encryptedBinary, string gamma)
        {
            return Encrypt(encryptedBinary, gamma);
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Write("Введите исходное сообщение: ");
            string message = Console.ReadLine();
            string messageBinary = TextToBinary(message);

            Console.WriteLine("Исходное сообщение в бинарном виде: " + messageBinary);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            LFSR lfsr = new LFSR();
            string initialLFSRState = Convert.ToString(lfsr.Generate16Bits().Item1, 2).PadLeft(16, '0');
            Console.WriteLine("Начальное состояние LSFR: " + initialLFSRState);

            Tuple<int, Tuple<string, int>[]> lfsrOutput = lfsr.Generate16Bits();
            Console.WriteLine("Первая ступень:");
            foreach (var step in lfsrOutput.Item2)
            {
                Console.WriteLine($"Регистр: {step.Item1}, Новый бит: {step.Item2}");
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            LinearCongruentialGenerator lcg = new LinearCongruentialGenerator(0);
            lcg.Next();
            lcg.GammaGenerate(messageBinary.Length);

            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            string gamma = lcg.GammaGenerate(messageBinary.Length);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            string encryptedMessageBinary = Encrypt(messageBinary, gamma);
            string encryptedMessage = BinaryToText(encryptedMessageBinary);

            for (int i = 0; i < messageBinary.Length; i += 8)
            {
                string messageBlock = messageBinary.Substring(i, 8);
                string gammaBlock = gamma.Substring(i, 8);
                string encryptedBlock = Encrypt(messageBlock, gammaBlock);
                char encryptedChar = BinaryToText(encryptedBlock)[0];
                Console.WriteLine($"Блок сообщения: {messageBlock}, Гамма блок: {gammaBlock}, Зашифрованный блок: {encryptedBlock}, Зашифрованный символ: {encryptedChar}");
            }
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            Console.WriteLine("Гамма: " + gamma);
            Console.WriteLine("Исходное сообщение в бинарном виде: " + messageBinary);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            encryptedMessageBinary = Encrypt(messageBinary, gamma);
            encryptedMessage = BinaryToText(encryptedMessageBinary);
            Console.WriteLine("Зашифрованное сообщение (символьный): " + encryptedMessage);
            Console.WriteLine("Зашифрованное сообщение (двоичный): " + encryptedMessageBinary);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");

            string decryptedMessageBinary = Decrypt(encryptedMessageBinary, gamma);
            string decryptedMessage = BinaryToText(decryptedMessageBinary);
            Console.WriteLine("Расшифрованное сообщение (символьный): " + decryptedMessage);
            Console.WriteLine("Расшифрованное сообщение (двоичный): " + decryptedMessageBinary);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------");
        }
    }



}
