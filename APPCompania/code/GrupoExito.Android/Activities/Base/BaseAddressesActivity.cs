using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Base
{
    [Activity(Label = "Direcciones", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BaseAddressesActivity : BaseActivity
    {
        #region Controls

        private RelativeLayout rlWelcome;
        public RelativeLayout RlWelcome
        {
            get { return rlWelcome; }
            set { rlWelcome = value; }
        }

        private AutoCompleteTextView actvAddress;
        public new AutoCompleteTextView ActvAddress
        {
            get { return actvAddress; }
            set { actvAddress = value; }
        }

        private Spinner spCityDelivery;
        public Spinner SpCityDelivery
        {
            get { return spCityDelivery; }
            set { spCityDelivery = value; }
        }

        #endregion

        #region Properties       

        private AddressModel addressModel;
        public AddressModel AddressModel
        {
            get { return addressModel; }
            set { addressModel = value; }
        }

        private int galleryPosition = 0;
        public int GalleryPosition
        {
            get { return galleryPosition; }
            set { galleryPosition = value; }
        }

        private bool enableGpsScreen = false;
        public new bool EnableGpsScreen
        {
            get { return enableGpsScreen; }
            set { enableGpsScreen = value; }
        }

        private readonly string[] permissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        private const int RequestLocationId = 0;
        private bool LocationPermission { get; set; }

        private bool SentLocation { get; set; }

        #endregion

        #region Public Methods      

        public async Task<AddressResponse> GetAddresses()
        {
            return await AddressModel.GetAddress();
        }

        public async Task<ResponseBase> BaseAddAddress(UserAddress Address)
        {
            ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
            ResponseBase response = await AddressModel.AddAddress(Address);
            return response;
        }

        public async Task<ResponseBase> UpdateDispatchRegion(UpdateDispatchRegionParameters parameters)
        {
            ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
            ResponseBase response = await AddressModel.UpdateDispatchRegion(parameters);
            return response;
        }

        public async Task<bool> DeleteAddress(UserAddress address)
        {
            bool result = false;

            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await AddressModel.DeleteAddress(address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        result = true;
                        HideProgressDialog();
                    });
                }

                return result;
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseAddressesActivity, ConstantMethodName.DeleteAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
                return result;
            }
            finally
            {
                HideProgressDialog();
            }
        }

        public async Task<bool> SetDefaultAddress(UserAddress address, bool showProgressDialog = true)
        {
            bool result = false;

            try
            {
                if (showProgressDialog)
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                }

                var response = await AddressModel.SetDefaultAddress(address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();

                        if (showProgressDialog)
                        {
                            DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                        }
                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        result = true;
                        HideProgressDialog();
                    });
                }

                return result;
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.GetCities } };
                ShowAndRegisterMessageExceptions(exception, properties);
                return result;
            }
            finally
            {
                HideProgressDialog();
            }
        }

        public async Task<IList<City>> LoadCitiesStore(bool showProgressDialog = true)
        {
            IList<City> cities = new List<City>();

            try
            {
                if (showProgressDialog)
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                }

                CitiesFilter parameters = new CitiesFilter() { HomeDelivery = "true", Pickup = "true" };
                var response = await AddressModel.GetCities(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    ShowBodyLayout();
                    cities = response.Cities;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.GetCities } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return cities;
        }

        public async Task<IList<Store>> LoadStores(StoreParameters parameters)
        {
            IList<Store> stores = new List<Store>();

            try
            {
                var response = await AddressModel.GetStores(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    stores = response.Stores;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.GetCities } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return stores;
        }

        public void SaveAddress(UserAddress address)
        {
            var user = ParametersManager.UserContext;
            user.Address = address;
            user.Store = null;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));

            var order = ParametersManager.Order;

            if (order != null)
            {
                order.DependencyId = address.DependencyId;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }
        }

        public void SaveStore(Store store)
        {
            var user = ParametersManager.UserContext;
            user.Address = null;
            user.Store = store;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
            ParametersManager.ChangeProductQuantity = true;

            var order = ParametersManager.Order;

            if (order != null)
            {
                order.DependencyId = store.Id.ToString();
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }
        }

        #endregion

        #region Geolocation Methods

        public async Task TryGetLocationAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                await ValidateGpsAvailabilityAndGetLocation();
                return;
            }

            await GetLocationPermissionAsync();
        }

        public async Task GetLocationPermissionAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                await ValidateGpsAvailabilityAndGetLocation();
                return;
            }

            RequestPermissions(permissionsLocation, RequestLocationId);
        }

        public async Task GetLocationAndAddressAsync()
        {
            Position positionSend = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                CancellationTokenSource ctsrc = new CancellationTokenSource(2000);
                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(20000));
                positionSend = position;
                this.SendLocation(positionSend);
                var geo = new Geocoder(this);

                var addresses = await geo.GetFromLocationAsync(position.Latitude, position.Longitude, 1);

                if (addresses.Any())
                {
                    var actualAddress = addresses[0];
                    var addressLine = actualAddress.GetAddressLine(0).Split(',');

                    if (addressLine.Any())
                    {
                        ActvAddress.Text = addressLine[0];
                    }
                }
            }
            catch (Exception e)
            {
                if (positionSend != null && !SentLocation)
                {
                    this.SendLocation(positionSend);
                }
            }
        }

        public async Task ValidateGpsAvailabilityAndGetLocation()
        {
            LocationManager manager = (LocationManager)GetSystemService(LocationService);

            if (manager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                await GetLocationAndAddressAsync();
            }
            else
            {
                ShowGpsDialog();
            }
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults.Any() && grantResults[0] == Permission.Granted)
                        {
                            await GetLocationAndAddressAsync();
                        }
                    }
                    break;
            }
        }

        #endregion

        #region Protected Methods

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            base.HandlerOkButton(sender, e);

            if (LocationPermission)
            {
                RequestPermissions(permissionsLocation, RequestLocationId);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.SetSoftInputMode(SoftInput.AdjustResize);
            AddressModel = new AddressModel(new AddressService(DeviceManager.Instance));
        }

        #endregion
    }
}