namespace GrupoExito.Utilities.Helpers
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class CryptoHelper
    {
        public static string HashSHA512(this string value)
        {
            using (var sha = SHA512.Create())
            {
                return Convert.ToBase64String(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value)));
            }
        }       

        public static string Encrypt(this string clearValue, string encryptionKey)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = CreateKey(encryptionKey);

                byte[] encrypted = AesEncryptStringToBytes(clearValue, aes.Key, aes.IV);
                return Convert.ToBase64String(encrypted) + ";" + Convert.ToBase64String(aes.IV);
            }
        }

        private static byte[] CreateKey(string password, int keyBytes = 32)
        {
            byte[] salt = new byte[] { 80, 70, 60, 50, 40, 30, 20, 10 };
            int iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, salt, iterations);
            return keyGenerator.GetBytes(keyBytes);
        }

        private static byte[] AesEncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException($"{nameof(plainText)}");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException($"{nameof(key)}");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException($"{nameof(iv)}");

            byte[] encrypted;

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))

                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    encrypted = memoryStream.ToArray();
                }
            }

            return encrypted;
        }
    }
}
