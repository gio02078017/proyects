using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Resources;
using Plugin.Connectivity;
using Plugin.Permissions;
using System;
using System.Threading.Tasks;
using static Android.Support.V4.App.ActivityCompat;
using android = Android;

namespace GrupoExito.Android.Utilities
{
    public class DeviceManager : IDeviceManager, IOnRequestPermissionsResultCallback
    {
        private static DeviceManager instance;
        private ISharedPreferences _SharedPreferences;
        private ISharedPreferencesEditor _SharedPreferencesEditor;
        private Context _Context;

        public static DeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceManager(AndroidApplication.Current);
                }

                return instance;
            }
        }

        public IntPtr Handle => throw new NotImplementedException();

        public DeviceManager(Context context)
        {
            this._Context = context;
            _SharedPreferences = global::Android.Preferences.PreferenceManager.GetDefaultSharedPreferences(_Context);
            _SharedPreferencesEditor = _SharedPreferences.Edit();
        }

        public void SaveAccessPreference(string key, string value)
        {
            _SharedPreferencesEditor.PutString(key, value);
            _SharedPreferencesEditor.Commit();
        }

        public void SaveAccessPreference(string key, bool value)
        {
            _SharedPreferencesEditor.PutBoolean(key, value);
            _SharedPreferencesEditor.Commit();
        }

        public string GetAccessPreference(string key)
        {
            return _SharedPreferences.GetString(key, "");
        }

        public bool GetAccessPreference(string key, bool value = false)
        {
            return _SharedPreferences.GetBoolean(key, value);
        }

        public bool ValidateAccessPreference(string key)
        {
            return !string.IsNullOrWhiteSpace(_SharedPreferences.GetString(key, ""));
        }

        public bool ValidateAccessPreference(string key, bool value = false)
        {
            return _SharedPreferences.GetBoolean(key, value);
        }

        public string GetDeviceId()
        {
            return android.Provider.Settings.Secure.GetString(_Context.ContentResolver,
                     android.Provider.Settings.Secure.AndroidId);
        }

        public bool DeleteAccessPreference(string key)
        {
            return _SharedPreferences.Edit().Remove(key).Commit();
        }

        public bool DeleteAccessPreference()
        {
            return _SharedPreferences.Edit().Clear().Commit();
        }

        public async Task<bool> IsNetworkAvailable()
        {
            bool isConnected = false;

            if (CrossConnectivity.IsSupported)
            {
                var connectivity = CrossConnectivity.Current;

                try
                {
                    isConnected = connectivity.IsConnected;
                    isConnected = isConnected ? await connectivity.IsRemoteReachable(AppConfigurations.ReachableUrl) : isConnected;
                }
                catch
                {
                }
                finally
                {
                    CrossConnectivity.Dispose();
                }
            }

            return isConnected;
        }

        public void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public static void HideKeyboard(Activity activity)
        {
            InputMethodManager imm = (InputMethodManager)activity.GetSystemService(Activity.InputMethodService);
            View view = activity.CurrentFocus;

            if (view == null)
            {
                view = new View(activity);

            }

            imm.HideSoftInputFromWindow(view.WindowToken, 0);
        }
    }
}