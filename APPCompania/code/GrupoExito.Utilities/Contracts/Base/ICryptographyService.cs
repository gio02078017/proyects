namespace GrupoExito.Utilities.Contracts.Base
{
    public interface ICryptographyService
    {
        string CreateSalt(int lengthInBytes = 16);
        string CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000);
        string Encrypt(string value, string derivedKey);
        string Decrypt(string value, string derivedKey);
        byte[] GetBytes(string value);
    }
}
