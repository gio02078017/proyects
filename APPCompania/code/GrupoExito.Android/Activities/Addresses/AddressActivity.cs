using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Users;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Addresses
{
    [Activity(Label = "Direcciones", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddressActivity : BaseAddressesActivity
    {
        #region Controls

        private TextView TvAddAddress;
        private TextView TvAreAddingAddress;
        private TextView TvCity;
        private TextView TvAddress;
        private TextView TvAddressExample;
        private TextView TvSeeMap;
        private TextView TvAdditionalData;
        private TextView TvLocationData;
        private TextView TvWhereAddress;
        private TextView TvHome;
        private TextView TvOffice;
        private TextView TvCouple;
        private TextView TvOther;
        private TextView TvCellPhone;
        private EditText EtAdditionalData;
        private EditText EtCellphone;
        private Button BtnSaveAddress;
        private LinearLayout LyHome;
        private LinearLayout LyOffice;
        private LinearLayout LyCouple;
        private LinearLayout LyOther;
        private bool IsUpdateAddress = false;
        private bool IsDefaultAddress = false;

        #endregion

        #region Properties     

        private IList<City> cities;
        private UserAddress Address { get; set; }
        private string AddressType { get; set; }
        private bool IsActivityRunning { get; set; }
        private bool ISuccessMessage { get; set; }
        private string PreviousActivity { get; set; }

        #endregion

        #region Public Methods

        public override void OnBackPressed()
        {
            Finish();
        }

        #endregion

        #region Protected Methods

        protected override void EventError()
        {
            base.EventError();
            ShowBodyLayout();
        }

        protected override void OnStop()
        {
            base.OnStop();
            IsActivityRunning = false;
        }

        protected override void OnStart()
        {
            base.OnStart();
            IsActivityRunning = true;
        }

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            base.HandlerOkButton(sender, e);

            if (ISuccessMessage)
            {
                if (PreviousActivity != null && PreviousActivity.Equals(ConstantActivityName.MyAddressActivity))
                {
                    this.GoToPreviousActivity();
                }
                else
                {
                    this.GoToMyAddresses();
                }
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.cities = new List<City>();
            AddressModel = new AddressModel(new AddressService(DeviceManager.Instance));
            SetContentView(Resource.Layout.ActivityAddress);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            cities = await LoadCitiesAddressesAndProgressDialog();
            DrawCities();

            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Address)))
                {
                    Address = JsonService.Deserialize<UserAddress>(Intent.Extras.GetString(ConstantPreference.Address));
                    this.IsUpdateAddress = true;
                    this.IsDefaultAddress = Address.IsDefaultAddress;
                    this.FillControlsAddress();
                }

                if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.PreviousActivity)))
                {
                    PreviousActivity = Intent.Extras.GetString(ConstantPreference.PreviousActivity);
                }
            }

            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, this.IsUpdateAddress ? AnalyticsScreenView.EditAddress : AnalyticsScreenView.AddAddress, typeof(AddressActivity).Name);
        }


        #endregion

        #region Private Methods       

        private async void ActvAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ActvAddress.Text.Length > 3)
                {
                    var response = await AddressModel.AutoCompleteAddress(ActvAddress.Text);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        RunOnUiThread(() =>
                        {
                            DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                        });
                    }
                    else
                    {
                        this.DrawAddress(response);
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.AddressActivity, ConstantMethodName.AutoCompleteAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawCities()
        {
            List<string> citiesName = this.cities.ToList().Select(x => x.Name).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, citiesName);
                SpCityDelivery.Adapter = adapter;
            });
        }

        private void DrawAddress(AddressPredictionResponse response)
        {
            List<string> predictions = response.Predictions.ToList()
                 .Where(x => x.Description.ToLower().Contains(AppConfigurations.CountryGeolocation))
                 .Select(x => x.Description).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, predictions);
                ActvAddress.Adapter = adapter;

                if (IsActivityRunning)
                {
                    ActvAddress.ShowDropDown();
                }
            });
        }

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<NestedScrollView>(Resource.Id.nsvBody));

            FindViewById(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            LyHome = FindViewById<LinearLayout>(Resource.Id.lyHome);
            LyOffice = FindViewById<LinearLayout>(Resource.Id.lyOffice);
            LyCouple = FindViewById<LinearLayout>(Resource.Id.lyCouple);
            LyOther = FindViewById<LinearLayout>(Resource.Id.lyOther);
            TvHome = FindViewById<TextView>(Resource.Id.tvHome);
            TvOffice = FindViewById<TextView>(Resource.Id.tvOffice);
            TvCouple = FindViewById<TextView>(Resource.Id.tvCouple);
            TvOther = FindViewById<TextView>(Resource.Id.tvOther);
            TvAreAddingAddress = FindViewById<TextView>(Resource.Id.tvAreAddingAddress);
            TvAddress = FindViewById<TextView>(Resource.Id.tvAddress);
            TvAddressExample = FindViewById<TextView>(Resource.Id.tvAddressExample);
            TvSeeMap = FindViewById<TextView>(Resource.Id.tvSeeMap);
            TvCity = FindViewById<TextView>(Resource.Id.tvCity);
            TvAdditionalData = FindViewById<TextView>(Resource.Id.tvAdditionalData);
            TvWhereAddress = FindViewById<TextView>(Resource.Id.tvWhereAddress);
            TvLocationData = FindViewById<TextView>(Resource.Id.tvLocationData);
            TvAddAddress = FindViewById<TextView>(Resource.Id.tvAddAddress);
            BtnSaveAddress = FindViewById<Button>(Resource.Id.btnSaveAddress);
            EtAdditionalData = FindViewById<EditText>(Resource.Id.etAdditionalData);
            ActvAddress = FindViewById<AutoCompleteTextView>(Resource.Id.actvAddress);
            SpCityDelivery = FindViewById<Spinner>(Resource.Id.spCityDelivery);
            EtCellphone = FindViewById<EditText>(Resource.Id.etPhoneNumber);
            TvCellPhone = FindViewById<TextView>(Resource.Id.tvPhoneNumber);

            LyHome.Click += delegate { SelectAddressType(ConstAddressType.Home); };
            LyOffice.Click += delegate { SelectAddressType(ConstAddressType.Office); };
            LyCouple.Click += delegate { SelectAddressType(ConstAddressType.Couple); };
            LyOther.Click += delegate { SelectAddressType(ConstAddressType.Other); };
            BtnSaveAddress.Click += async delegate { HideKeyBoard(); await ValidateCoverageAddress(); };
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            ActvAddress.TextChanged += ActvAddress_TextChanged;
            TvSeeMap.Visibility = ViewStates.Gone;

            EtCellphone.Text = ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone) ?
                ParametersManager.UserContext.CellPhone : string.Empty;

            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Address)))
                {
                    BtnSaveAddress.SetText(Resource.String.str_update_address);
                    TvAddAddress.SetText(Resource.String.str_edit_address);
                    TvAreAddingAddress.Text = AppMessages.AreEditingAddress;
                }
            }
        }

        private void EditFonts()
        {
            TvAdditionalData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvLocationData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvWhereAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvAreAddingAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddressExample.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvHome.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvOffice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCouple.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvOther.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSeeMap.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnSaveAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            ActvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtAdditionalData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCellPhone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtCellphone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private async Task ValidateCoverageAddress()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                string message = string.Empty;
                Address = this.SetAddress();
                var city = AddressModel.GetCity(cities[SpCityDelivery.SelectedItemPosition].Name, cities);

                if (city != null && city.Id.Equals("0"))
                {
                    message = AppMessages.YouMustSelectFirtsCity;
                }
                else
                {
                    message = city == null ? AppMessages.WrongCity : AddressModel.ValidateFields(Address);
                }

                if (string.IsNullOrEmpty(message))
                {
                    if (city != null)
                    {
                        LocationAddress location = new LocationAddress { Address = ActvAddress.Text, City = AddressModel.GetShortCityId(city.Name, cities) };
                        var coverageResponse = await AddressModel.CoverageAddress(location);
                        await this.ValidateCoverageResponse(coverageResponse);
                    }
                }
                else
                {
                    HideProgressDialog();
                    this.ModifyFieldsStyle();
                    DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText, true);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.AddressActivity, ConstantMethodName.CoverageAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private UserAddress SetAddress()
        {
            UserContext user = ParametersManager.UserContext;
            City city = AddressModel.GetCity(cities[SpCityDelivery.SelectedItemPosition].Name, cities);

            if (city != null)
            {
                return new UserAddress
                {
                    AddressKey = this.IsUpdateAddress ? Address.AddressKey : string.Empty,
                    CityId = city.Id,
                    IdPickup = city.IdPickup,
                    Region = city.Region,
                    AddressComplete = ActvAddress.Text,
                    AditionalInformationAddress = EtAdditionalData.Text,
                    Description = AddressType,
                    Name = user.FirstName,
                    LastName = user.LastName,
                    CellPhone = EtCellphone.Text,
                    StateId = city.State,
                    City = city.Name
                };
            }
            else
            {
                return new UserAddress();
            }
        }

        private void SelectAddressType(string addressType)
        {
            AddressType = addressType;

            switch (addressType)
            {
                case ConstAddressType.Home:
                    LyHome.SetBackgroundResource(Resource.Drawable.button_yellow);
                    LyOffice.SetBackgroundColor(Color.Transparent);
                    LyCouple.SetBackgroundColor(Color.Transparent);
                    LyOther.SetBackgroundColor(Color.Transparent);
                    TvHome.SetTextColor(Color.White);
                    TvOffice.SetTextColor(Color.Black);
                    TvCouple.SetTextColor(Color.Black);
                    TvOther.SetTextColor(Color.Black);
                    TvHome.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.casa_primario, 0, 0);
                    TvOffice.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.oficina, 0, 0);
                    TvCouple.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.pareja, 0, 0);
                    TvOther.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.otro, 0, 0);
                    break;
                case ConstAddressType.Office:
                    LyOffice.SetBackgroundResource(Resource.Drawable.button_yellow);
                    LyHome.SetBackgroundColor(Color.Transparent);
                    LyCouple.SetBackgroundColor(Color.Transparent);
                    LyOther.SetBackgroundColor(Color.Transparent);
                    TvHome.SetTextColor(Color.Black);
                    TvOffice.SetTextColor(Color.White);
                    TvCouple.SetTextColor(Color.Black);
                    TvOther.SetTextColor(Color.Black);
                    TvHome.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.casa, 0, 0);
                    TvOffice.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.oficina_primario, 0, 0);
                    TvCouple.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.pareja, 0, 0);
                    TvOther.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.otro, 0, 0);
                    break;
                case ConstAddressType.Couple:
                    LyCouple.SetBackgroundResource(Resource.Drawable.button_yellow);
                    LyHome.SetBackgroundColor(Color.Transparent);
                    LyOffice.SetBackgroundColor(Color.Transparent);
                    LyOther.SetBackgroundColor(Color.Transparent);
                    TvHome.SetTextColor(Color.Black);
                    TvOffice.SetTextColor(Color.Black);
                    TvCouple.SetTextColor(Color.White);
                    TvOther.SetTextColor(Color.Black);
                    TvHome.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.casa, 0, 0);
                    TvOffice.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.oficina, 0, 0);
                    TvCouple.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.pareja_primario, 0, 0);
                    TvOther.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.otro, 0, 0);
                    break;
                case ConstAddressType.Other:
                    LyOther.SetBackgroundResource(Resource.Drawable.button_yellow);
                    LyHome.SetBackgroundColor(Color.Transparent);
                    LyCouple.SetBackgroundColor(Color.Transparent);
                    LyOffice.SetBackgroundColor(Color.Transparent);
                    TvHome.SetTextColor(Color.Black);
                    TvOffice.SetTextColor(Color.Black);
                    TvCouple.SetTextColor(Color.Black);
                    TvOther.SetTextColor(Color.White);
                    TvHome.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.casa, 0, 0);
                    TvOffice.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.oficina, 0, 0);
                    TvCouple.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.pareja, 0, 0);
                    TvOther.SetCompoundDrawablesWithIntrinsicBounds(0, Resource.Drawable.otro_primario, 0, 0);
                    break;
                default:
                    break;
            }
        }

        private void ModifyFieldsStyle()
        {
            if (string.IsNullOrEmpty(Address.CityId))
            {
                SpCityDelivery.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                SpCityDelivery.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(Address.AddressComplete))
            {
                ActvAddress.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                ActvAddress.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(Address.AditionalInformationAddress))
            {
                EtAdditionalData.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtAdditionalData.SetBackgroundResource(Resource.Drawable.button_products);
            }
        }

        private async Task ValidateCoverageResponse(CoverageAddressResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                });
            }
            else
            {
                Address = ModelHelper.SetCoverageInformation(response, Address);

                if (IsUpdateAddress)
                {
                    await UpdateAddress();
                }
                else
                {
                    await AddAddress();
                }
            }
        }

        private async Task AddAddress()
        {
            try
            {
                var response = await BaseAddAddress(Address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        ParametersManager.ChangeAddress = true;                        
                        ParametersManager.ChangeProductQuantity = true;
                        SaveAddress(Address);
                        HideProgressDialog();
                        ISuccessMessage = true;
                        RegisterNotificationTags();
                        DeletePreferencesLastDateUpdatedPromotion();
                        OnBackPressed();
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.AddressActivity, ConstantMethodName.AddAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task UpdateAddress()
        {
            try
            {
                ResponseBase response = await AddressModel.UpdateAddress(Address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        ISuccessMessage = true;
                        ParametersManager.ChangeAddress = true;
                        ParametersManager.ChangeProductQuantity = true;

                        if (ParametersManager.UserContext != null && ParametersManager.UserContext.Address != null)
                        {
                            if (ParametersManager.UserContext.Address.AddressComplete.Equals(Address.AddressComplete) && !ParametersManager.UserContext.Address.CityId.Equals(Address.CityId))
                            {
                                DeletePreferencesLastDateUpdatedPromotion();
                            }
                            
                        }

                        if (IsDefaultAddress)
                        {
                            SaveAddress(Address);
                        }

                        RegisterNotificationTags();

                        OnBackPressed();
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.AddressActivity, ConstantMethodName.UpdateAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void FillControlsAddress()
        {
            ActvAddress.Text = Address.AddressComplete;
            SpCityDelivery.SetSelection(AddressModel.GetCityPosition(Address.CityId, this.cities));
            EtAdditionalData.Text = Address.AditionalInformationAddress;

            if (!string.IsNullOrEmpty(Address.CellPhone) && !Address.CellPhone.Equals(AppConfigurations.DefaultCellphone))
            {
                EtCellphone.Text = Address.CellPhone;
            }
            else if (ParametersManager.UserContext != null &&
                !string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone) &&
                !ParametersManager.UserContext.CellPhone.Equals(AppConfigurations.DefaultCellphone))
            {
                EtCellphone.Text = ParametersManager.UserContext.CellPhone;
            }
            else
            {
                EtCellphone.Text = string.Empty;
            }

            AddressType = Address.Description;
            this.SelectAddressType(AddressType);
        }

        private void GoToMyAddresses()
        {
            Intent intent = new Intent(this, typeof(MyAccountActivity));
            StartActivity(intent);
        }

        private void GoToPreviousActivity()
        {
            OnBackPressed();
        }
        #endregion
    }
}