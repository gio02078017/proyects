namespace GrupoExito.Utilities.Contracts.Generic
{
    using System.Threading.Tasks;

    public interface IDeviceManager
    {
        void SaveAccessPreference(string key, string value);
        void SaveAccessPreference(string key, bool value);
        string GetAccessPreference(string key);
        bool GetAccessPreference(string key, bool value);
        bool ValidateAccessPreference(string key);
        bool ValidateAccessPreference(string key, bool value);
        string GetDeviceId();
        Task<bool> IsNetworkAvailable();
        bool DeleteAccessPreference(string key);
        bool DeleteAccessPreference();
    }
}
