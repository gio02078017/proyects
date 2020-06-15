namespace GrupoExito.Utilities.Contracts.Base
{
    public interface IDiffieHellmanService
    {
        byte[] GetPublicKey(byte[] privateKey);
        byte[] GetSharedSecret(byte[] privateKey, byte[] peerPublicKey);
    }
}
