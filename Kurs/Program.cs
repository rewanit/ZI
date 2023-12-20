using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using System.Text;

class ElGamalSignature
{
    static void Main()
    {
        string message = "Hello, World!";
        int bits = 256;

        // Генерация ключей
        var keyPairGenerator = GeneratorUtilities.GetKeyPairGenerator("ELGAMAL");
        keyPairGenerator.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), new ElGamalParameters(GeneratePrime(bits),GeneratePrime(bits))));
        AsymmetricCipherKeyPair keyPair = keyPairGenerator.GenerateKeyPair();
        ElGamalPrivateKeyParameters privateKey = (ElGamalPrivateKeyParameters)keyPair.Private;
        ElGamalPublicKeyParameters publicKey = (ElGamalPublicKeyParameters)keyPair.Public;

        // Подписание сообщения
        ElGamalSigner signer = new ElGamalSigner();
        signer.Init(true, privateKey);

        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        byte[] signature = signer.GenerateSignature();

        // Проверка подписи
        signer.Init(false, publicKey);
        signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
        bool isValid = signer.VerifySignature(signature);

        Console.WriteLine("Подпись верна: " + isValid);
    }

    static BigInteger GeneratePrime(int bits)
    {
        var generator = GeneratorUtilities.GetKeyPairGenerator("DH");
        generator.Init(new DHKeyGenerationParameters(new SecureRandom(), bits, 2));
        AsymmetricCipherKeyPair keyPair = generator.GenerateKeyPair();
        return ((DHPublicKeyParameters)keyPair.Public).P;
    }
}
