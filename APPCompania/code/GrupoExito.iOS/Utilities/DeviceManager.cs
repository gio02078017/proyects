using System;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Utilities.Contracts.Generic;
using Plugin.Connectivity;
using Security;

namespace GrupoExito.iOS.Utilities
{
    public class DeviceManager : IDeviceManager
    {
        public static DeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceManager();
                }

                return instance;
            }
        }

        private static DeviceManager instance;
        private NSUserDefaults _SharedPreferences;

        public DeviceManager()
        {
            _SharedPreferences = NSUserDefaults.StandardUserDefaults;
        }

        public void SaveAccessPreference(string key, string value)
        {
            _SharedPreferences.SetString(value, key);
            _SharedPreferences.Synchronize();
            if (key.Equals(ConstPreferenceKeys.Order)) { NSNotificationCenter.DefaultCenter.PostNotificationName(key, null); }
        }

        public void SaveAccessPreference(string key, bool value)
        {
            _SharedPreferences.SetBool(value, key);
            _SharedPreferences.Synchronize();
        }

        public string GetAccessPreference(string key)
        {
            try
            {
                var value = _SharedPreferences.ValueForKey((NSString)key).ToString();
                return value;
            }
            catch (Exception e)
            {
                var exc = e.Message;
                return exc;
            }
        }

        public bool GetAccessPreference(string key, bool value)
        {
            return bool.TryParse(_SharedPreferences.ValueForKey((NSString)key).ToString(), out value);
        }

        public bool ValidateAccessPreference(string key)
        {
            try
            {
                var data = (NSString)_SharedPreferences.ValueForKey((NSString)key).ToString();
                var value = !string.IsNullOrWhiteSpace(data);
                return value;
            }
            catch (Exception e)
            {
                var exc = e.Message;
                return false;
            }
        }

        public bool ValidateAccessPreference(string key, bool value = false)
        {
            return bool.TryParse(_SharedPreferences.ValueForKey((NSString)key).ToString(), out value);
        }

        public string GetDeviceId()
        {
            #if SIM
               return "111";
            #else
                var response = new SecRecord(SecKind.GenericPassword)
                {
                    Service = NSBundle.MainBundle.BundleIdentifier,
                    Account = "UniqueID"
                };

                NSData uniqueId = SecKeyChain.QueryAsData(response);

                if (uniqueId == null)
                {
                    response.ValueData = NSData.FromString(System.Guid.NewGuid().ToString());
                    var err = SecKeyChain.Add(response);

                    if (err != SecStatusCode.Success && err != SecStatusCode.DuplicateItem)
                        return string.Empty;

                    return response.ValueData.ToString();
                }
                else
                {
                    return uniqueId.ToString();
                }
            #endif
        }

        public bool DeleteAccessPreference(string key)
        {
            _SharedPreferences.RemoveObject(key);
            return true;
        }

        public bool DeleteAccessPreference()
        {
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.Token);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.ExpirationTime);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.User);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.Order);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.MobileId);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.Ticket);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.NextDateCache);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.Plus);
            _SharedPreferences.RemoveObject(ConstPreferenceKeys.UserActivateClifre);
            SaveAccessPreference(ConstPreferenceKeys.LastDateUpdated, string.Empty);
            return true;
        }

        public Task<bool> IsNetworkAvailable()
        {
            NetworkStatus remoteHostStatus = Reachability.RemoteHostStatus();
            var isConnected = CrossConnectivity.Current.IsConnected;
            isConnected = isConnected ? (remoteHostStatus.Equals(NetworkStatus.NotReachable) ? false : true) : false;

            return Task.FromResult(isConnected);
        }
    }
}