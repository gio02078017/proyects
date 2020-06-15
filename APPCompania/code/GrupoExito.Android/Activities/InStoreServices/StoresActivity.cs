using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Gms.Maps.GoogleMap;
using Asv7 = Android.Support.V7.App;
using net = Android.Net.Uri;
using ProviderSettings = Android.Provider;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Almacenes", ScreenOrientation = ScreenOrientation.Portrait)]
    public class StoresActivity : BaseActivity, IOnMapReadyCallback, IOnInfoWindowClickListener
    {
        #region Controls

        private MapView MapView;

        #endregion

        #region Properties

        private AddressModel _addressModel;
        private readonly string[] permissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        private const int RequestLocationId = 0;
        private bool locationPermission;
        private double latitude;
        private double longitude;
        private List<Store> Stores { get; set; }

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityStores);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
            this.SetControlsProperties(savedInstanceState);            
        }

        protected async override void OnResume()
        {
            base.OnResume();
            await TryGetLocationAsync();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Stores, typeof(StoresActivity).Name);
        }

        public void OnMapReady(GoogleMap map)
        {
            if (map != null)
            {
                MarkerOptions markerOpt = null;
                map.SetOnInfoWindowClickListener(this);

                LatLng position = new LatLng(latitude, longitude);
                map.AddMarker(new MarkerOptions().SetPosition(position));

                CameraPosition cameraPosition = new CameraPosition.Builder().Target(position).Zoom(15).Build();
                map.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));

                foreach (var store in Stores)
                {
                    markerOpt = new MarkerOptions();
                    markerOpt.SetPosition(new LatLng(store.Latitude, store.Longitude));
                    markerOpt.SetTitle(store.DependencyName);
                    markerOpt.SetSnippet(store.Address);
                    markerOpt.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin_almacenes));
                    map.AddMarker(markerOpt);
                }
            }
        }

        public void OnInfoWindowClick(Marker marker)
        {
            Store store = Stores.Where(x => x.DependencyName.Equals(marker.Title)).FirstOrDefault();

            if (store != null)
            {
                ShowStoreInfoDialog(store);
            }
        }

        protected void HandlerOkButton(DialogClickEventArgs e, object sender)
        {
            if (locationPermission)
            {
                RequestPermissions(permissionsLocation, RequestLocationId);
            }
        }

        private async Task GetStores()
        {
            try
            {
                SearchStoresParameters parameters = new SearchStoresParameters { Latitude = latitude, Longitude = longitude };
                Stores = new List<Store>();
                var response = await _addressModel.GetStores(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                    });
                }
                else
                {
                    this.ShowBodyLayout();
                    Stores = response.Stores.ToList();
                    MapView.GetMapAsync(this);
                    ShowBodyLayout();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.StoresFragment, ConstantMethodName.GetStores } };
                RegisterMessageExceptions(exception, properties);
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
            finally
            {
                HideProgressDialog();
            }

        }

        private void SetControlsProperties(Bundle savedInstanceState)
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            MapView = (MapView)FindViewById(Resource.Id.map);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                MapView);

            MapView.OnCreate(savedInstanceState);
            MapView.OnResume();
            MapsInitializer.Initialize(this);
        }

        private void ShowStoreInfoDialog(Store store)
        {
            Asv7.AlertDialog storeInfoDialog = new Asv7.AlertDialog.Builder(this).Create();
            View storeInfoView = LayoutInflater.Inflate(Resource.Layout.DialogStoresMap, null);
            storeInfoDialog.SetView(storeInfoView);
            storeInfoDialog.SetCanceledOnTouchOutside(false);

            TextView tvStoreName = storeInfoView.FindViewById<TextView>(Resource.Id.tvStoreName);
            TextView tvStoreAddress = storeInfoView.FindViewById<TextView>(Resource.Id.tvStoreAddress);
            TextView tvStorePhone = storeInfoView.FindViewById<TextView>(Resource.Id.tvStorePhone);
            TextView tvStoreScheduleTitle = storeInfoView.FindViewById<TextView>(Resource.Id.tvStoreScheduleTitle);
            TextView tvStoreSchedule = storeInfoView.FindViewById<TextView>(Resource.Id.tvStoreSchedule);
            TextView tvStoreServicesTitle = storeInfoView.FindViewById<TextView>(Resource.Id.tvStoreServicesTitle);
            TextView tvStoreServices = storeInfoView.FindViewById<TextView>(Resource.Id.tvStoreServices);
            ImageView ivClose = storeInfoView.FindViewById<ImageView>(Resource.Id.ivClose);
            ImageView ivWaze = storeInfoView.FindViewById<ImageView>(Resource.Id.ivWaze);

            tvStoreName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvStoreScheduleTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvStoreServicesTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvStoreAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvStorePhone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvStoreServices.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvStoreSchedule.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            tvStoreName.Text = store.DependencyName;
            tvStoreAddress.Text = store.Address;
            tvStorePhone.Text = AppMessages.PhoneNumberAbreviation + " " + store.PhoneNumber;
            tvStoreSchedule.Text = store.Schedules;

            if (store.Services != null)
            {
                foreach (var service in store.Services)
                {
                    tvStoreServices.Text = tvStoreServices.Text + service.ServiceName + "\r\n";
                }
            }

            ivWaze.Click += delegate { CallWazeApp(store.Latitude, store.Longitude); };
            ivClose.Click += delegate { storeInfoDialog.Cancel(); };

            storeInfoDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            storeInfoDialog.Show();
        }

        private void CallWazeApp(double latitude, double longitude)
        {
            try
            {
                String url = "https://waze.com/ul?ll=" + StringFormat.DoubleToString(latitude) + "%2c" + StringFormat.DoubleToString(longitude) + "&navigate=yes&zoom=17";
                Intent intent = new Intent(Intent.ActionView, net.Parse(url));
                StartActivity(intent);
            }
            catch (ActivityNotFoundException)
            {
                Intent intent = new Intent(Intent.ActionView, net.Parse(AppConfigurations.WazeStoreAndroidEndPoint));
                StartActivity(intent);
            }
        }

        #region Geolocation

        private async Task TryGetLocationAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                await ValidateGpsAvailability();
                return;
            }

            await GetLocationPermissionAsync();
        }

        private async Task GetLocationPermissionAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                await ValidateGpsAvailability();
                return;
            }

            if (ShouldShowRequestPermissionRationale(permission))
            {
                locationPermission = true;
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.LocationRequiredMessage, AppMessages.AcceptButtonText);
                return;
            }

            RequestPermissions(permissionsLocation, RequestLocationId);
        }

        private new async Task GetLocationAsync()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(2));
                latitude = position.Latitude;
                longitude = position.Longitude;
                await this.GetStores();
            }
            catch (Exception)
            {
                HideProgressDialog();
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private new async Task ValidateGpsAvailability()
        {
            LocationManager manager = (LocationManager)GetSystemService(LocationService);

            if (manager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                await GetLocationAsync();
            }
            else
            {
                ShowGpsDialog();
            }
        }

        private new void ShowGpsDialog()
        {
            Asv7.AlertDialog.Builder builder = new Asv7.AlertDialog.Builder(this);
            builder.SetMessage(AppMessages.GpsDisable)
                    .SetCancelable(false)
                    .SetPositiveButton(AppMessages.AcceptButtonText, delegate
                    {
                        StartActivity(new Intent(ProviderSettings.Settings.ActionLocationSourceSettings));
                    })
                    .SetNegativeButton(AppMessages.CancelButtonText, (EventHandler<DialogClickEventArgs>)null);
            Asv7.AlertDialog alert = builder.Create();
            Button cancelButton = alert.GetButton((int)DialogButtonType.Negative);
            alert.Show();
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            await GetLocationAsync();
                        }
                        else
                        {
                            DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.PermissionDeniedLocationMessage,
                            AppMessages.AcceptButtonText);
                        }
                    }
                    break;
            }
        }

        #endregion
    }
}