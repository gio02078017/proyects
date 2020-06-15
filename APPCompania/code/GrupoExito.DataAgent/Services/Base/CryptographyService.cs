namespace GrupoExito.DataAgent.Services
{
    using GrupoExito.Utilities.Contracts.Base;
    using PCLCrypto;
    using System;
    using System.Text;

    public class CryptographyService : ICryptographyService
    {
        public string CreateSalt(int lengthInBytes = 16)
        {
            var bytes = WinRTCrypto.CryptographicBuffer.GenerateRandom(lengthInBytes);
            return BitConverter.ToString(bytes);
        }

        public string CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            return BitConverter.ToString(key);
        }

        public string Encrypt(string data, string derivedKey)
        {
            byte[] key = GetBytes(derivedKey);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));

            return Convert.ToBase64String(bytes);
        }

        public string Decrypt(string value, string derivedKey)
        {
            try
            {
                byte[] key = GetBytes(derivedKey);
                byte[] data = Convert.FromBase64String(value);

                ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
                ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
                var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data);

                return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public byte[] GetBytes(string value)
        {
            int length = (value.Length + 1) / 3;
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
                result[i] = Convert.ToByte(value.Substring(3 * i, 2), 16);

            return result;
        }
    }
}
