using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.Users;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites.Users;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Plugin.LatestVersion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using droid = Android;

namespace GrupoExito.Android
{
    [Activity(Label = "Carulla", MainLauncher = true, Immersive = true, Icon = "@mipmap/icono", Theme = "@style/FullScreenAppTheme", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : BaseAddressesActivity
    {
        private Timer SplashTimer;
        private AppVersionModel _appVersionModel;
        private TaxesModel _taxesModel;
        private ProductCarModel _productCarModel;
        private MyAccountModel _myAccountModel;

        private Dialog errorDialog;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySplash);

            if (CheckPlayServices())
            {
                FindViewById<TextView>(Resource.Id.tvVersionLabel).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                FindViewById<TextView>(Resource.Id.tvVersionValue).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                _appVersionModel = new AppVersionModel(new AppVersionService(DeviceManager.Instance));
                AppCenter.Start(AppConfigurations.AndroidAppCenterId, typeof(Crashes));
                await Crashes.IsEnabledAsync();

                if (SupportActionBar != null)
                {
                    SupportActionBar.Hide();
                }

                await GetAppVersion();

                RunOnUiThread(async () =>
                {
                    await RegisterCostumer();
                });

                RunOnUiThread(async () =>
                {
                    await GetUserAddress().ConfigureAwait(false);
                    await GetTaxesBag().ConfigureAwait(false);

                    if (AppServiceConfiguration.SiteId.Equals("carulla") && (ParametersManager.UserContext != null && !ParametersManager.UserContext.IsAnonymous))
                    {
                        await GetUserType().ConfigureAwait(false);
                    }

                    RegisterNotificationTags();
                });
            }

            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Splash, typeof(SplashActivity).Name);
        }

        public bool CheckPlayServices()
        {
            GoogleApiAvailability googleApiAvailability = GoogleApiAvailability.Instance;

            int resultCode = googleApiAvailability.IsGooglePlayServicesAvailable(this);

            if (resultCode != ConnectionResult.Success)
            {
                if (googleApiAvailability.IsUserResolvableError(resultCode))
                {
                    if (errorDialog == null)
                    {
                        errorDialog = googleApiAvailability.GetErrorDialog(this, resultCode, 2404);
                        errorDialog.SetCancelable(false);
                    }

                    if (!errorDialog.IsShowing)
                    {
                        errorDialog.Show();
                    }
                }
            }

            return resultCode == ConnectionResult.Success;
        }

        private void StartSplashCounter()
        {
            SplashTimer = new Timer();
            SplashTimer.Elapsed += new ElapsedEventHandler(GoToLobby);
            SplashTimer.Interval = 1000;
            SplashTimer.Enabled = true;
        }

        private void GoToLobby(object source, ElapsedEventArgs e)
        {

            RunOnUiThread(() =>
            {
                if (SplashTimer != null)
                {
                    SplashTimer.Enabled = false;
                    SplashTimer.Stop();
                }

                if (ParametersManager.UserContext == null || ParametersManager.UserContext.IsAnonymous)
                {
                    Intent intent = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    Intent intent = new Intent(this, typeof(LobbyActivity));
                    StartActivity(intent);
                    Finish();
                }
            });
        }

        private void DialogUpdateApp(bool TypeUpdate)
        {
            RunOnUiThread(() =>
            {
                View view = LayoutInflater.Inflate(Resource.Layout.DialogUpdateApp, null);
                AlertDialog updateAppDialog = new AlertDialog.Builder(this).Create();
                updateAppDialog.SetView(view);
                updateAppDialog.SetCanceledOnTouchOutside(false);

                ImageView imgCloseTwo = view.FindViewById<ImageView>(Resource.Id.imgCloseTwo);
                TextView tvTitleUpdateApp = view.FindViewById<TextView>(Resource.Id.tvTitleUpdateApp);
                TextView tvMessageUpdateApp = view.FindViewById<TextView>(Resource.Id.tvMessageUpdateApp);
                LinearLayout lyUpdateNow = view.FindViewById<LinearLayout>(Resource.Id.lyUpdateNow);
                Button btnUpdateNow = view.FindViewById<Button>(Resource.Id.btnUpdateNow);
                LinearLayout lyOptionalUpdate = view.FindViewById<LinearLayout>(Resource.Id.lyOptionalUpdate);
                LinearLayout btnReject = view.FindViewById<LinearLayout>(Resource.Id.btnReject);
                LinearLayout btnUpdate = view.FindViewById<LinearLayout>(Resource.Id.btnUpdate);
                TextView tvReject = view.FindViewById<TextView>(Resource.Id.tvBtnReject);
                TextView tvUpdate = view.FindViewById<TextView>(Resource.Id.tvBtnUpdate);
                tvTitleUpdateApp.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                tvMessageUpdateApp.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                btnUpdateNow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                tvReject.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                tvUpdate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

                btnReject.Click += delegate
                {
                    updateAppDialog.Dismiss();
                    StartSplashCounter();
                };

                btnUpdate.Click += delegate { GoToPlayStore(); };
                btnUpdateNow.Click += delegate { GoToPlayStore(); };

                if (!TypeUpdate)
                {
                    lyOptionalUpdate.Visibility = ViewStates.Visible;
                    tvMessageUpdateApp.Text = GetString(Resource.String.str_message_update_app);
                    lyUpdateNow.Visibility = ViewStates.Gone;
                }
                else
                {
                    lyOptionalUpdate.Visibility = ViewStates.Gone;
                    tvMessageUpdateApp.Text = GetString(Resource.String.str_message_update_now_app);
                    lyUpdateNow.Visibility = ViewStates.Visible;
                }

                updateAppDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

                if (!IsFinishing)
                {
                    updateAppDialog.Show();
                }
            });
        }

        private void GoToPlayStore()
        {
            try
            {
                StartActivity(new Intent(Intent.ActionView, droid.Net.Uri.Parse(AppConfigurations.MarketUrl + PackageName)));
            }
            catch
            {
                StartActivity(new Intent(Intent.ActionView, droid.Net.Uri.Parse(AppConfigurations.PlayStoreUrl + PackageName)));
            }
        }

        private async Task GetAppVersion()
        {
            try
            {
                string latestVersionAppStore = await CrossLatestVersion.Current.GetLatestVersionNumber(Application.Context.ApplicationContext.PackageName);
                string localVersion = Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;

                if (!latestVersionAppStore.Equals(localVersion))
                {
                    var response = await _appVersionModel.GetAppVersion(ConstOSApp.Android);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        StartSplashCounter();
                    }
                    else
                    {
                        if (string.Compare(localVersion, response.MinimumVersion, StringComparison.CurrentCulture) < 0)
                        {
                            DialogUpdateApp(true);
                        }
                        else
                        {
                            StartSplashCounter();
                        }
                    }
                }
                else
                {
                    StartSplashCounter();
                }
            }
            catch
            {
                StartSplashCounter();
            }
        }

        private async Task GetUserAddress()
        {
            try
            {
                if (ParametersManager.UserContext != null && ParametersManager.UserContext.Store == null)
                {
                    var response = await GetAddresses();

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        //Mostrar mensaje
                    }
                    else
                    {
                        RunOnUiThread(async () =>
                        {
                            if (response != null && response.Addresses != null && response.Addresses.Any())
                            {
                                var addresses = response.Addresses.ToList();
                                bool hasAnAddress = addresses != null && addresses.Any() ? true : false;
                                bool hasAnAddressDefault = hasAnAddress && addresses.Where(x => x.IsDefaultAddress == true) != null
                                 && addresses.Where(x => x.IsDefaultAddress == true).Any() ? true : false;

                                if (hasAnAddress)
                                {
                                    if (hasAnAddressDefault)
                                    {
                                        var address = addresses.Where(x => x.IsDefaultAddress == true).FirstOrDefault();
                                        SavePreferences(address);
                                    }
                                    else
                                    {
                                        var address = addresses.FirstOrDefault();
                                        var responseSetDefaultAddress = await SetDefaultAddress(address, false);

                                        if (responseSetDefaultAddress)
                                        {
                                            SavePreferences(addresses[0]);
                                        }
                                    }
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SplashActivity, ConstantMethodName.GetUserAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private async Task GetTaxesBag()
        {
            try
            {
                _taxesModel = new TaxesModel(new TaxesService(DeviceManager.Instance));
                var response = await _taxesModel.GetTaxBag().ConfigureAwait(false);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    //Mostrar mensaje
                }
                else
                {
                    if (response.TaxBags != null && response.TaxBags.Any())
                    {
                        _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
                        _productCarModel.UpSertTaxBag(response.TaxBags);
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SplashActivity, ConstantMethodName.GetTaxesBag } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private async Task GetUserType()
        {
            try
            {
                _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
                var response = await _myAccountModel.GetUserType().ConfigureAwait(false);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    //Mostrar mensaje
                }
                else
                {
                    if (!string.IsNullOrEmpty(response.Name))
                    {
                        Segment segment = new Segment { Name = response.Name, Code = response.Code };
                        SavePreferences(segment);
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SplashActivity, ConstantMethodName.GetTaxesBag } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void SavePreferences(Segment segment)
        {
            UserContext user = ParametersManager.UserContext;
            user.UserType = segment;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
        }

        private void SavePreferences(UserAddress address)
        {
            UserContext user = ParametersManager.UserContext;

            if (address != null && user != null)
            {
                ParametersManager.ChangeAddress = true;
                user.Address = address;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
            }
        }
    }
}