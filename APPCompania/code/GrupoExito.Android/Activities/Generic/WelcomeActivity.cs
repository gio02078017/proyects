using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static Android.Widget.AdapterView;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Bienvenida", ScreenOrientation = ScreenOrientation.Portrait)]
    public class WelcomeActivity : BaseAddressesActivity, ViewPager.IOnPageChangeListener, IOnItemSelectedListener
    {
        #region Controls

        private TextGalleryAdapter TextGalleryAdapter;
        private ViewPager VpTextGallery;
        private View[] DotsGalleryViews;
        private LinearLayout LyImagecount;
        private LinearLayout LyAtHome;
        private LinearLayout LyInStore;
        private LinearLayout LyError;
        private LinearLayout LyActions;
        private LinearLayout LyChangeAddress;
        private LinearLayout LyPickup;
        private TextView TvAtHome;
        private TextView TvWelcomeReceive;
        private TextView TvCity;
        private TextView TvAddress;
        private TextView TvCanType;
        private TextView TvInStore;
        private TextView TvExcuseUs;
        private TextView TvWeCantSend;
        private TextView TvWeSuggestYou;
        private TextView TvChangeAddress;
        private TextView TvAddOtherPlace;
        private TextView TvPickUp;
        private TextView TvChooseStore;
        private IList<TextGallery> GalleryTexts { get; set; }
        private Button BtnContinue, BtnReturn;
        private Spinner SpCity, SpStore;
        private NestedScrollView NsvBody;

        #endregion

        #region Properties    

        private IList<City> CitiesAddresses { get; set; }
        private IList<City> CitiesStore { get; set; }
        private IList<Store> Stores { get; set; }
        private TypefaceStyle TypefaceStyle { get; set; }
        private UserAddress UserAddress { get; set; }
        private Store UserStore { get; set; }
        private bool IsActivityRunning { get; set; }
        private LocationAddress _location { get; set; }
        private CoverageAddressResponse CoverageResponse { get; set; }

        #endregion

        #region Public Methods

        public override void OnBackPressed()
        {
            FinishAndRemoveTask();
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageSelected(int position)
        {
            for (int i = 0; i < GalleryTexts.Count; i++)
            {
                DotsGalleryViews[i].SetBackgroundResource(Resource.Drawable.circle_stroke);
            }

            GalleryPosition = position;
            DotsGalleryViews[position].SetBackgroundResource(Resource.Drawable.circle_solid);
        }

        #endregion

        #region Protected Methods

        protected async override void OnResume()
        {
            base.OnResume();
            HideProgressDialog();

            if (EnableGpsScreen)
            {
                EnableGpsScreen = false;
                await TryGetLocationAsync();
            }
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Welcome, typeof(WelcomeActivity).Name);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityWelcome);
            
            Window.SetSoftInputMode(SoftInput.AdjustPan);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.PutFonts();
            this.InitAddresses();

            string preferenceName = ConstNameViewTutorial.Welcome;
            bool response = DeviceManager.Instance.GetAccessPreference(preferenceName, false);

            if (!response)
            {
                Intent intent = new Intent(this, typeof(TutorialsActivity));
                intent.PutExtra(ConstantPreference.ActivityTutorial, preferenceName);
                StartActivity(intent);
            }            
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

        protected override void EventError()
        {
            base.EventError();
            InitAddresses();
        }

        #endregion

        #region At Home Methods

        private async void ActvAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (LyAtHome.Visibility == ViewStates.Visible)
                {
                    this.DefineButtonAvailability();

                    if (ActvAddress.Text.Length > 3)
                    {
                        var addressPredictionResponse = await AddressModel.AutoCompleteAddress(ActvAddress.Text);

                        if (addressPredictionResponse != null && addressPredictionResponse.Predictions != null)
                        {
                            List<string> predictions = addressPredictionResponse.Predictions.ToList()
                                .Where(x => x.Description.ToLower().Contains(AppConfigurations.CountryGeolocation))
                                .Select(x => x.Description).ToList();

                            if (IsActivityRunning)
                            {
                                this.RunOnUiThread(() =>
                                {
                                    ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, predictions);
                                    ActvAddress.Adapter = adapter;
                                    ActvAddress.ShowDropDown();
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.AutoCompleteAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void InitAddresses()
        {
            this.RunOnUiThread(async () =>
            {
                CitiesAddresses = await LoadCitiesAddresses();
                CitiesStore = await LoadCitiesStore(false);
                this.DrawCitiesAddresses();
                this.DrawCitiesStore();
                Stores = new List<Store>();
                Stores.Insert(0, new Store { Id = string.Empty, Name = AppMessages.Choose });
                await TryGetLocationAsync();
            });
        }

        private void ActvCity_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            if (LyAtHome.Visibility == ViewStates.Visible)
            {
                this.DefineButtonAvailability();
            }
        }

        private void DrawCitiesAddresses()
        {
            List<string> citiesName = CitiesAddresses.ToList().Select(x => x.Name).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, citiesName);
                SpCityDelivery.Adapter = adapter;
            });
        }

        private UserAddress SetAddress()
        {
            City city = AddressModel.GetCity(CitiesAddresses[SpCityDelivery.SelectedItemPosition].Name, this.CitiesAddresses);
            UserContext user = ParametersManager.UserContext;

            if (city != null && user != null)
            {
                return new UserAddress
                {
                    CityId = city.Id,
                    IdPickup = city.IdPickup,
                    Region = city.Region,
                    AddressComplete = ActvAddress.Text,
                    AditionalInformationAddress = string.Empty,
                    Description = ConstAddressType.Other,
                    Name = user.FirstName,
                    LastName = user.LastName,
                    CellPhone = !string.IsNullOrEmpty(user.CellPhone) ? user.CellPhone : AppConfigurations.DefaultCellphone,
                    StateId = city.State,
                    City = city.Name
                };
            }
            else
            {
                return new UserAddress { };
            }
        }

        private async Task ValidateCoverageResponse(CoverageAddressResponse response)
        {
            if (response != null && response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    ShowErrorLayout();
                });
            }
            else
            {
                await AddAddress();
            }
        }

        private void DefineAtHomeVisibility()
        {
            if (LyAtHome.Visibility == ViewStates.Gone)
            {
                LyAtHome.Visibility = ViewStates.Visible;
                LyInStore.Visibility = ViewStates.Gone;
                this.DefineButtonAvailability();
            }
            else
            {
                LyAtHome.Visibility = ViewStates.Gone;
            }
        }

        private void ChangeAddress()
        {
            LyError.Visibility = ViewStates.Gone;
            LyActions.Visibility = ViewStates.Visible;
            DefineAtHomeVisibility();
        }

        private async Task SaveAtHome()
        {
            UserAddress = this.SetAddress();
            string message = AddressModel.ValidateFieldsAtHome(UserAddress);

            if (string.IsNullOrEmpty(message))
            {
                _location = new LocationAddress
                {
                    Address = UserAddress.AddressComplete,
                    City = AddressModel.GetShortCityId(UserAddress.City, this.CitiesAddresses)
                };

                CoverageResponse = await AddressModel.CoverageAddress(_location);
                await this.ValidateCoverageResponse(CoverageResponse);
            }
            else
            {
                ShowMessageError(message);
            }
        }

        private async Task AddAddress()
        {
            try
            {
                string adress = JsonService.Serialize<UserAddress>(UserAddress);
                var response = await BaseAddAddress(UserAddress);

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
                    HideProgressDialog();
                    UserAddress = ModelHelper.SetCoverageInformation(CoverageResponse, UserAddress);
                    UserContext user = ParametersManager.UserContext;
                    user.Address = UserAddress;
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
                    RegisterNotificationTags();
                    DeletePreferencesLastDateUpdatedPromotion();
                    var mainActivity = new Intent(this, typeof(MainActivity));
                    StartActivity(mainActivity);
                    Finish();
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

        #endregion

        #region In Store Methods

        private void DrawCitiesStore()
        {
            List<string> citiesName = CitiesStore.ToList().Select(x => x.Name).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, citiesName);
                SpCity.Adapter = adapter;
            });
        }

        private void DrawStores()
        {
            List<string> storeNames = Stores.ToList().Select(x => x.Name).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, storeNames);
                SpStore.Adapter = adapter;
                DefineButtonAvailability();
            });
        }

        public async void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            if (LyInStore.Visibility == ViewStates.Visible)
            {
                City city = CitiesStore[position];

                if (!city.Id.Equals("0"))
                {
                    StoreParameters parameters = new StoreParameters
                    {
                        CityId = city.Id,
                        HomeDelivery = false,
                        PickUp = true,
                        IdPickup = city.IdPickup
                    };

                    Stores = await LoadStores(parameters);
                }

                if (!Stores.Any())
                {
                    Stores = new List<Store>();
                    Stores.Insert(0, new Store { Id = string.Empty, Name = AppMessages.Choose });
                }

                DrawStores();
            }
            else if (LyAtHome.Visibility == ViewStates.Visible)
            {
                if (!string.IsNullOrEmpty(ActvAddress.Text))
                {
                    BtnContinue.Enabled = true;
                    BtnContinue.SetBackgroundResource(Resource.Drawable.button_black);
                }
                else
                {
                    BtnContinue.Enabled = false;
                    BtnContinue.SetBackgroundResource(Resource.Drawable.button_transparent);
                }
            }
        }

        public void OnNothingSelected(AdapterView parent)
        {
            // Is required because of IOnItemSelectedListener implementation
        }

        private void DefineInStoreVisibility()
        {
            if (LyInStore.Visibility == ViewStates.Gone)
            {
                LyInStore.Visibility = ViewStates.Visible;
                LyAtHome.Visibility = ViewStates.Gone;
                LyInStore.SetBackgroundResource(Resource.Drawable.welcome_in_store);
                TvInStore.SetBackgroundColor(Color.White);
                this.DefineButtonAvailability();
            }
            else
            {
                LyInStore.Visibility = ViewStates.Gone;
                TvInStore.SetBackgroundResource(Resource.Drawable.welcome_in_store);
                LyInStore.SetBackgroundColor(Color.White);
            }
        }

        private void PickUpStore()
        {
            LyError.Visibility = ViewStates.Gone;
            LyActions.Visibility = ViewStates.Visible;
        }

        private async Task SaveInStore()
        {
            try
            {
                UserStore = SetStore();

                UpdateDispatchRegionParameters parameters = new UpdateDispatchRegionParameters
                {
                    CityId = UserStore.CityId,
                    Region = UserStore.Region,
                    DependencyId = UserStore.Id
                };

                string message = AddressModel.ValidateFieldsInStore(UserStore);

                if (string.IsNullOrEmpty(message))
                {
                    var response = await UpdateDispatchRegion(parameters);

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
                        SaveStore(UserStore);
                        City city = CitiesStore[SpCity.SelectedItemPosition];
                        RegisterNotificationTags();
                        DeletePreferencesLastDateUpdatedPromotion();
                        var loginView = new Intent(this, typeof(MainActivity));
                        StartActivity(loginView);
                        Finish();
                    }
                }
                else
                {
                    ShowErrorLayout(message);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.SaveInStore } };
                RegisterMessageExceptions(exception, properties);
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private Store SetStore()
        {
            City city = CitiesStore[SpCity.SelectedItemPosition];
            Store store = Stores[SpStore.SelectedItemPosition];
            store = store ?? new Store();

            if (city != null)
            {
                store.CityId = city.Id.ToString();
                store.IdPickup = city.IdPickup;
                store.Region = city.Region;
                store.City = city.Name;
            }

            return store;
        }

        #endregion

        #region Gallery Methods

        private void StartGalleryCounter()
        {
            Timer galleryTimer = new Timer();
            galleryTimer.Elapsed += new ElapsedEventHandler(GalleryNextPageEvent);
            galleryTimer.Interval = 5000;
            galleryTimer.Enabled = true;
        }

        private void GalleryNextPageEvent(object source, ElapsedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                if (GalleryPosition < GalleryTexts.Count - 1)
                {
                    VpTextGallery.SetCurrentItem(GalleryPosition + 1, true);
                }
                else
                {
                    VpTextGallery.SetCurrentItem(0, true);
                }
            });
        }

        private void DrawCircleViews()
        {
            DotsGalleryViews = new View[GalleryTexts.Count];

            for (int i = 0; i < GalleryTexts.Count; i++)
            {
                View view = new TextView(this);
                LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(new LinearLayout.LayoutParams(40, 40));
                parameters.SetMargins(0, 0, 20, 0);
                view.LayoutParameters = parameters;

                if (i == 0)
                {
                    view.SetBackgroundResource(Resource.Drawable.circle_solid);
                }
                else
                {
                    view.SetBackgroundResource(Resource.Drawable.circle_stroke);
                }

                DotsGalleryViews[i] = view;
                LyImagecount.AddView(DotsGalleryViews[i]);
            }
        }

        #endregion

        #region Generic Methods

        private void SetControlsProperties()
        {
            RlWelcome = FindViewById<RelativeLayout>(Resource.Id.rlWelcome);
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError), RlWelcome);
            VpTextGallery = FindViewById<ViewPager>(Resource.Id.vpTextGallery);
            LyImagecount = FindViewById<LinearLayout>(Resource.Id.lyImagecount);
            LyAtHome = FindViewById<LinearLayout>(Resource.Id.lyAtHome);
            LyInStore = FindViewById<LinearLayout>(Resource.Id.lyInStore);
            LyError = FindViewById<LinearLayout>(Resource.Id.lyError);
            LyActions = FindViewById<LinearLayout>(Resource.Id.lyActions);
            LyChangeAddress = FindViewById<LinearLayout>(Resource.Id.lyChangeAddress);
            LyPickup = FindViewById<LinearLayout>(Resource.Id.lyPickUp);
            TvAtHome = FindViewById<TextView>(Resource.Id.tvAtHome);
            TvWelcomeReceive = FindViewById<TextView>(Resource.Id.tvWelcomeReceive);
            TvCity = FindViewById<TextView>(Resource.Id.tvCity);
            TvAddress = FindViewById<TextView>(Resource.Id.tvAddress);
            TvCanType = FindViewById<TextView>(Resource.Id.tvCanType);
            TvInStore = FindViewById<TextView>(Resource.Id.tvInStore);
            TvExcuseUs = FindViewById<TextView>(Resource.Id.tvExcuseUs);
            TvWeCantSend = FindViewById<TextView>(Resource.Id.tvWeCantSend);
            TvWeSuggestYou = FindViewById<TextView>(Resource.Id.TvWeSuggestYou);
            TvChangeAddress = FindViewById<TextView>(Resource.Id.tvChangeAddress);
            TvAddOtherPlace = FindViewById<TextView>(Resource.Id.tvAddOtherPlace);
            TvPickUp = FindViewById<TextView>(Resource.Id.tvPickUp);
            TvChooseStore = FindViewById<TextView>(Resource.Id.tvChooseStore);
            BtnContinue = FindViewById<Button>(Resource.Id.btnContinue);
            BtnReturn = FindViewById<Button>(Resource.Id.btnReturn);
            SpCity = FindViewById<Spinner>(Resource.Id.spCity);
            SpStore = FindViewById<Spinner>(Resource.Id.spStore);
            SpCityDelivery = FindViewById<Spinner>(Resource.Id.spCityDelivery);
            ActvAddress = FindViewById<AutoCompleteTextView>(Resource.Id.actvAddress);
            NsvBody = FindViewById<NestedScrollView>(Resource.Id.nsvBody);

            GalleryTexts = new List<TextGallery>();
            GalleryTexts = JsonService.Deserialize<List<TextGallery>>(AppConfigurations.TextGalleryWelcomeSource);
            TextGalleryAdapter = new TextGalleryAdapter(this, GalleryTexts);
            VpTextGallery.Adapter = TextGalleryAdapter;
            VpTextGallery.AddOnPageChangeListener(this);
            BtnContinue.Enabled = false;
            BtnContinue.SetBackgroundResource(Resource.Drawable.button_transparent);
            this.DrawCircleViews();
            this.StartGalleryCounter();

            TvAtHome.Click += delegate { DefineAtHomeVisibility(); };
            TvInStore.Click += delegate { DefineInStoreVisibility(); };
            BtnContinue.Click += async delegate { await SaveInformation(); };
            BtnReturn.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
            SpCityDelivery.OnItemSelectedListener = this;
            ActvAddress.TextChanged += ActvAddress_TextChanged;
            LyChangeAddress.Click += delegate { ChangeAddress(); };
            LyPickup.Click += delegate { PickUpStore(); };
            SpCity.OnItemSelectedListener = this;
        }

        private void PutFonts()
        {
            TvAtHome.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvInStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnContinue.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnReturn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvWelcomeReceive.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            ActvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCanType.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvExcuseUs.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvWeCantSend.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvWeSuggestYou.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvChangeAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvPickUp.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvAddOtherPlace.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvChooseStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void ModifyFieldsStyle()
        {
            if (LyAtHome.Visibility == ViewStates.Visible)
            {
                if (string.IsNullOrEmpty(UserAddress.CityId))
                {
                    SpCityDelivery.SetBackgroundResource(Resource.Drawable.field_error);
                }
                else
                {
                    SpCityDelivery.SetBackgroundResource(Resource.Drawable.button_products);
                }

                if (string.IsNullOrEmpty(UserAddress.AddressComplete))
                {
                    ActvAddress.SetBackgroundResource(Resource.Drawable.field_error);
                }
                else
                {
                    ActvAddress.SetBackgroundResource(Resource.Drawable.button_products);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(UserStore.CityId))
                {
                    SpCity.SetBackgroundResource(Resource.Drawable.field_error);
                }
                else
                {
                    SpCity.SetBackgroundResource(Resource.Drawable.button_products);
                }

                if (string.IsNullOrEmpty(UserStore.Id))
                {
                    SpStore.SetBackgroundResource(Resource.Drawable.field_error);
                }
                else
                {
                    SpStore.SetBackgroundResource(Resource.Drawable.button_products);
                }
            }
        }

        private void DefineButtonAvailability()
        {
            if (LyAtHome.Visibility == ViewStates.Visible)
            {
                if (SpCityDelivery.SelectedItemPosition <= 0 || SpCityDelivery.SelectedItemPosition <= -1)
                {
                    BtnContinue.Enabled = false;
                    BtnContinue.SetBackgroundResource(Resource.Drawable.button_transparent);
                }
                else
                {
                    BtnContinue.Enabled = true;
                    BtnContinue.SetBackgroundResource(Resource.Drawable.button_yellow);
                }
            }
            else
            {
                if (SpCity.SelectedItemPosition <= 0 || SpStore.SelectedItemPosition <= -1)
                {
                    BtnContinue.Enabled = false;
                    BtnContinue.SetBackgroundResource(Resource.Drawable.button_transparent);
                }
                else
                {
                    BtnContinue.Enabled = true;
                    BtnContinue.SetBackgroundResource(Resource.Drawable.button_black);
                }
            }

            Handler handler = new Handler();

            void productAction()
            {
                NsvBody.SmoothScrollingEnabled = true;
                int scrollTo = ((View)BtnContinue.Parent.Parent).Top + BtnContinue.Top + 20;
                NsvBody.SmoothScrollTo(0, scrollTo);
            }

            handler.PostDelayed(productAction, 200);
        }

        private void ShowErrorLayout()
        {
            LyActions.Visibility = ViewStates.Gone;
            LyAtHome.Visibility = ViewStates.Gone;
            LyError.Visibility = ViewStates.Visible;
            BtnContinue.Enabled = false;
            BtnContinue.SetBackgroundResource(Resource.Drawable.button_transparent);
        }

        private void ShowMessageError(string message)
        {
            HideProgressDialog();

            if (message.Equals(AppMessages.CoverageAddressMessage))
            {
                this.ShowErrorLayout();
            }
            else
            {
                if (message.Equals(AppMessages.RequiredFieldsText))
                {
                    ModifyFieldsStyle();
                }

                DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText);
            }
        }

        private async Task SaveInformation()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                if (LyInStore.Visibility == ViewStates.Gone)
                {
                    await this.SaveAtHome();
                }
                else
                {
                    await this.SaveInStore();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.SaveInformation } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        #endregion
    }
}