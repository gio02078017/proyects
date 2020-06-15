using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters.InStoreServices;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Widget.AdapterView;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Localización del Almacén", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PriceCheckerLocationActivity : BaseAddressesActivity, IOnItemSelectedListener
    {
        #region Controls

        private Button BtnAccept;
        private Spinner SpCityStore;
        private Spinner SpStore;

        #endregion

        #region Properties

        private CheckerPriceModel _CheckerPriceModel;
        private IList<City> CitiesStore { get; set; }
        private IList<Store> Stores { get; set; }

        private Position position { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        public async void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            switch (parent.Id)
            {
                case Resource.Id.spCityStore:
                    CleanStore();

                    City city = CitiesStore[position];

                    if (city != null && !city.Id.Equals("0"))
                    {
                        await this.LoadStore(city.Id);
                    }

                    break;
                case Resource.Id.spStore:
                    break;

            }
        }

        public void OnNothingSelected(AdapterView parent)
        {
            // Is required because of IOnItemSelectedListener implementation
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityPriceCheckerLocation);
            HideItemsCarToolbar(this);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            this.CitiesStore = new List<City>();
            this.Stores = new List<Store>();

            _CheckerPriceModel = new CheckerPriceModel(new CheckerPriceService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            await this.LoadCities();            
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            ShowBodyLayout();
            SpCityStore.SetSelection(0);
            SpStore.SetSelection(0);
        }

        protected async override void EventError()
        {
            base.EventError();
            await LoadCities();
        }

        protected override void SendLocation(Position position)
        {
            this.position = position;
        }

        protected async override void OnResume()
        {
            base.OnResume();

            if (position == null)
            {
                await GetLocationAsync();
            }

            await LoadCities();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.PriceCheckerLocation, typeof(PriceCheckerLocationActivity).Name);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo), FindViewById<LinearLayout>(Resource.Id.lyBody), AppMessages.NotCitiesOrStores, AppMessages.GenericBackAction);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<LinearLayout>(Resource.Id.lyBody));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            BtnAccept = FindViewById<Button>(Resource.Id.btnAccept);
            SpCityStore = FindViewById<Spinner>(Resource.Id.spCityStore);
            SpStore = FindViewById<Spinner>(Resource.Id.spStore);

            BtnAccept.Click += async delegate { await this.OnPriceChecker(); };
            BtnAccept.Enabled = false;

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            SpCityStore.ItemSelected += SpinnerItemSelected;
            SpStore.ItemSelected += SpinnerItemSelected;
            SpCityStore.OnItemSelectedListener = this;
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleCheckerLocation).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvMessagePriceCheckerLocation).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvPolititionAndConditionChecker).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvCity).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvStore).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnAccept.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private async Task LoadCities()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                CitiesResponse response = null;
                response = await _CheckerPriceModel.GetCities();
                CleanStore();

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
                    if (response.Cities != null && response.Cities.Any() && response.Cities.Count > 1)
                    {
                        ShowBodyLayout();
                        this.CitiesStore = response.Cities;
                        DrawCities();
                    }
                    else
                    {
                        ShowNoInfoLayout();
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyAddressesActivity, ConstantMethodName.LoadCities } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawCities()
        {
            List<string> citiesName = this.CitiesStore.ToList().Select(x => x.Name).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, citiesName);
                SpCityStore.Adapter = adapter;
            });
        }

        private async Task LoadStore(string idStore)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                StoreResponse response = null;
                response = await _CheckerPriceModel.GetStores(idStore);

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
                    if (response.Stores != null && response.Stores.Any() && response.Stores.Count > 1)
                    {
                        ShowBodyLayout();
                        this.Stores = response.Stores;
                        DrawStores();
                    }
                    else
                    {
                        ShowNoInfoLayout();
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.PriceCheckerLocationActivity, ConstantMethodName.LoadStore } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawStores()
        {
            List<string> storeNames = Stores.ToList().Select(x => x.DependencyName).ToList();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, storeNames);
                SpStore.Adapter = adapter;
            });
        }

        public void CleanStore()
        {
            if (Stores != null)
            {
                Stores.Clear();
                Stores.Insert(0, new Store { Id = string.Empty, DependencyName = AppMessages.Choose });
                DrawStores();
            }
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ValidateInformation();
        }

        private bool ValidateInformation()
        {
            bool pass = true;

            if (SpCityStore.SelectedItemPosition == -1 || SpCityStore.SelectedItemPosition == 0)
            {
                pass = false;
            }

            if (SpStore != null && Stores != null)
            {
                if (SpStore.SelectedItemPosition == -1 || SpStore.SelectedItemPosition == 0)
                {
                    pass = false;
                }
            }
            else
            {
                pass = false;
            }

            if (!pass)
            {
                BtnAccept.Enabled = false;
                BtnAccept.SetBackgroundResource(Resource.Drawable.button_transparent);
            }
            else
            {
                BtnAccept.Enabled = true;
                BtnAccept.SetBackgroundResource(Resource.Drawable.button_yellow);
            }

            return pass;
        }

        public async Task OnPriceChecker()
        {
            if(position != null)
            {
                Store store = Stores[SpStore.SelectedItemPosition];
               await ValidatedLocalizacion(store.Id);
            }

        }

        public async Task ValidatedLocalizacion(string storeId)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                CheckerPriceCoverageParameters parameters = new CheckerPriceCoverageParameters()
                {
                   DependencyId = storeId,
                   Latitude = position.Latitude,
                   Longitude = position.Longitude
                };

                CheckerPriceCoverageResponse CoverageResponse = await _CheckerPriceModel.CheckerPriceCoverage(parameters);

                if (CoverageResponse.Result != null && CoverageResponse.Result.HasErrors && CoverageResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(CoverageResponse.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    if (CoverageResponse.HaveCoverage) {
                        Intent intent = new Intent(this, typeof(PriceCheckActivity));
                        intent.PutExtra(ConstantPreference.DependencyId, storeId);
                        StartActivity(intent);
                    }
                    else
                    {
                        OnBoxMessageTouched(AppMessages.PriceCheckNotCoverage, AppMessages.AcceptButtonText);
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.PriceCheckerLocationActivity, ConstantMethodName.ValidatedLocalizacion } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }
    }
}