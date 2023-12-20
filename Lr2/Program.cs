namespace Lr2
{
    using System;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите сообщение для шифрования:");
            string originalMessage = Console.ReadLine().ToUpper(); 

            Console.WriteLine("Введите первое ключевое слово:");
            string firstKeyword = Console.ReadLine().ToUpper(); 

            Console.WriteLine("Введите второе ключевое слово:");
            string secondKeyword = Console.ReadLine().ToUpper(); 

            string encryptedMessage = Encrypt(originalMessage, firstKeyword);
            string decryptedMessage = Decrypt(encryptedMessage, secondKeyword);
            string reEncryptedMessage = Encrypt(decryptedMessage, firstKeyword);


            Console.WriteLine("Полное шифрование");

            Console.WriteLine($"Зашифрованное сообщение: {encryptedMessage}");
            Console.WriteLine($"Расшифрованное сообщение: {decryptedMessage}");
            Console.WriteLine($"Повторно зашифрованное сообщение: {reEncryptedMessage}");
            Console.WriteLine("Полная расшифровка");

            string decryptedWithFirstKeyword = Decrypt(reEncryptedMessage, firstKeyword);
            string reEncryptedWithSecondKeyword = Encrypt(decryptedWithFirstKeyword, secondKeyword);
            string fullyDecryptedMessage = Decrypt(reEncryptedWithSecondKeyword, firstKeyword);


            Console.WriteLine($"Расшифрованное сообщение: {decryptedWithFirstKeyword}");
            Console.WriteLine($"повторно зашифрованное сообщение: {reEncryptedWithSecondKeyword}");
            Console.WriteLine($"Полностью расшифрованное сообщение: {fullyDecryptedMessage}");
        }
        static char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ".ToCharArray();

        static string Encrypt(string message, string keyword)
        {
            int alphabetLength = alphabet.Length;

            int messageLength = message.Length;
            int keywordLength = keyword.Length;
            char[] encryptedMessage = new char[messageLength];

            for (int i = 0; i < messageLength; i++)
            {
                char originalChar = message[i];
                char keywordChar = keyword[i % keywordLength];

                int originalIndex = Array.IndexOf(alphabet, originalChar);
                int keywordIndex = Array.IndexOf(alphabet, keywordChar);

                int newIndex = (originalIndex + keywordIndex) % alphabetLength;

                encryptedMessage[i] = alphabet[newIndex];
            }

            return new string(encryptedMessage);
        }


        static string Decrypt(string message, string keyword)
        {
            int alphabetLength = alphabet.Length;

            int messageLength = message.Length;
            int keywordLength = keyword.Length;
            char[] decryptedMessage = new char[messageLength];

            for (int i = 0; i < messageLength; i++)
            {
                char encryptedChar = message[i];
                char keywordChar = keyword[i % keywordLength];

                int encryptedIndex = Array.IndexOf(alphabet, encryptedChar);
                int keywordIndex = Array.IndexOf(alphabet, keywordChar);

                int newIndex = (encryptedIndex - keywordIndex + alphabetLength) % alphabetLength;

                decryptedMessage[i] = alphabet[newIndex];
            }

            return new string(decryptedMessage);
        }
    }

}
