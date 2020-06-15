using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Widget.AdapterView;

namespace GrupoExito.Android.Activities.Addresses
{
    [Activity(Label = "", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddressesDialogActivity : BaseAddressesActivity, IMyAddress, IOnItemSelectedListener
    {
        #region Controls

        private RecyclerView RvAddresses;
        private LinearLayoutManager AddressLayoutManager;
        private LinearLayout LyAtHome;
        private LinearLayout LyInStore;
        private LinearLayout LyStoreItem;
        private MyAddressAdapter AddressesAdapter;
        private TextView TvHelloUser;
        private TextView TvAddAddress;
        private TextView TvWhereDoYouWant;
        private TextView TvAtHome;
        private TextView TvInStore;
        private TextView TvItemTypeAddress;
        private TextView TvAddStore;
        private ImageView IvEditStore;
        private Spinner SpCity, SpStore;
        private AlertDialog storesDialog;
        private LinearLayout LyChooseStore;

        #endregion

        #region Properties
        private List<UserAddress> Addresses { get; set; }
        private UserAddress DefaultAddress { get; set; }
        private UserAddress DataActionAddress { get; set; }
        private string TypeAction { get; set; }
        private IList<City> CitiesStore { get; set; }
        private IList<Store> Stores { get; set; }
        private Store UserStore { get; set; }
        private string CurrentCity { get; set; }

        #endregion

        protected async override void OnResume()
        {
            base.OnResume();
            await DrawAddresses();
            DrawSelectedStore();
            this.UpdateCurrentCity();
        }

        protected async override void EventYesGenericDialog()
        {
            GenericDialog.Hide();

            if (TypeAction.Equals(ConstantEventName.DeleteItemClicked))
            {
                var result = await this.DeleteAddress(DataActionAddress);

                if (result)
                {
                    Addresses.Remove(Addresses[Addresses.IndexOf(DataActionAddress)]);
                    AddressesAdapter.NotifyDataSetChanged();
                }
            }
        }

        protected override void EventNotGenericDialog()
        {
            base.EventNotGenericDialog();
            TypeAction = null;
            GenericDialog.Hide();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DialogAddresses);
            this.SetFinishOnTouchOutside(false);
            Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlProperties();
            SetUserName();
            EditFonts();
        }

        protected override async void EventNoInfo()
        {
            base.EventNoInfo();

            if (LyAtHome.Visibility == ViewStates.Visible)
            {
                EventAddAddress();
            }
            else
            {
                await ShowStoresDialog();
            }
        }

        protected override void EventError()
        {
            base.EventError();
            OnResume();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private void SetControlProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                            FindViewById<NestedScrollView>(Resource.Id.nsvAddressType),
                            AppMessages.NotAddressMessage, AppMessages.AddAddressText);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<NestedScrollView>(Resource.Id.nsvAddressType));

            TvHelloUser = FindViewById<TextView>(Resource.Id.tvHelloUserName);
            TvWhereDoYouWant = FindViewById<TextView>(Resource.Id.tvWhereDoYouWant);
            TvAddAddress = FindViewById<TextView>(Resource.Id.tvAddAddress);
            TvAtHome = FindViewById<TextView>(Resource.Id.tvAtHome);
            TvInStore = FindViewById<TextView>(Resource.Id.tvPickUp);
            TvItemTypeAddress = FindViewById<TextView>(Resource.Id.tvItemTypeAddress);
            TvAddStore = FindViewById<TextView>(Resource.Id.tvAddStore);
            IvEditStore = FindViewById<ImageView>(Resource.Id.ivEditStore);
            RvAddresses = FindViewById<RecyclerView>(Resource.Id.rvAddressesAtHome);
            LyAtHome = FindViewById<LinearLayout>(Resource.Id.lyAddressesAtHome);
            LyInStore = FindViewById<LinearLayout>(Resource.Id.lyAddressesInStore);
            LyStoreItem = FindViewById<LinearLayout>(Resource.Id.lyStoreItem);

            AddressLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvAddresses.NestedScrollingEnabled = false;
            RvAddresses.HasFixedSize = true;
            RvAddresses.SetLayoutManager(AddressLayoutManager);

            TvAddAddress.Click += delegate { EventAddAddress(); };
            TvAtHome.Click += delegate { DefineAtHomeVisibility(); };
            TvInStore.Click += delegate { DefineInStoreVisibility(); };
            TvAddStore.Click += async delegate { await ShowStoresDialog(); };
            IvEditStore.Click += async delegate { await ShowStoresDialog(); };
            LyStoreItem.Click += async delegate { await SaveStoreDefault(); };
        }

        private async Task SaveStoreDefault()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserStore = ParametersManager.UserContext.Store;
                UpdateDispatchRegionParameters parameters = new UpdateDispatchRegionParameters
                {
                    CityId = UserStore.CityId,
                    Region = UserStore.Region,
                    DependencyId = UserStore.Id
                };

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
                    HideProgressDialog();
                    Finish();
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.AddressesDialogActivity, ConstantMethodName.SaveStoreDefault } };
                RegisterMessageExceptions(exception, properties);
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private void EditFonts()
        {
            TvHelloUser.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvWhereDoYouWant.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvAtHome.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvInStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvItemTypeAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
        }

        private void DefineAtHomeVisibility()
        {
            AddressesDialogDeliverScreenView();
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                     FindViewById<NestedScrollView>(Resource.Id.nsvAddressType), AppMessages.NotAddressMessage, AppMessages.AddAddressText);

            TvAtHome.SetTextColor(Color.Black);
            TvAtHome.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvAtHome.SetBackgroundResource(Resource.Drawable.addresses_item_selected);

            TvInStore.SetTextColor(Color.ParseColor("#EF7C32"));
            TvInStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvInStore.SetBackgroundColor(Color.White);

            LyAtHome.Visibility = ViewStates.Visible;
            LyInStore.Visibility = ViewStates.Gone;

            if (Addresses != null)
            {
                if (Addresses.Any())
                {
                    ShowBodyLayout();
                }
                else
                {
                    ShowNoInfoLayout();
                }
            }
            else
            {
                ShowErrorLayout();
            }
        }

        private void DefineInStoreVisibility()
        {
            AddressesDialogStoreScreenView();
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                     FindViewById<NestedScrollView>(Resource.Id.nsvAddressType), AppMessages.NotStoreMessage, AppMessages.AddStoreText);

            TvInStore.SetTextColor(Color.Black);
            TvInStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvInStore.SetBackgroundResource(Resource.Drawable.addresses_item_selected);

            TvAtHome.SetTextColor(Color.ParseColor("#EF7C32"));
            TvAtHome.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAtHome.SetBackgroundColor(Color.White);

            LyInStore.Visibility = ViewStates.Visible;
            LyAtHome.Visibility = ViewStates.Gone;

            if (ParametersManager.UserContext != null && ParametersManager.UserContext.Store != null)
            {
                ShowBodyLayout();
            }
            else
            {
                ShowNoInfoLayout();
            }
        }

        private async Task DrawAddresses()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await GetAddresses();

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
                    if (response.Addresses != null && response.Addresses.Any())
                    {
                        Addresses = response.Addresses.ToList();
                        ShowBodyLayout();
                        AddressesAdapter = new MyAddressAdapter(Addresses, this, this, true);
                        RvAddresses.SetAdapter(AddressesAdapter);
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
                    { ConstantActivityName.AddressesDialogActivity, ConstantMethodName.DrawAddresses } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawSelectedStore()
        {
            if (ParametersManager.UserContext != null && ParametersManager.UserContext.Store != null)
            {
                TvItemTypeAddress.Text = ParametersManager.UserContext.Store.Name;
                LyStoreItem.Visibility = ViewStates.Visible;
            }
            else
            {
                LyStoreItem.Visibility = ViewStates.Gone;
            }
        }

        private void SetUserName()
        {
            TvHelloUser.Text = AppMessages.Hello + " " + StringFormat.Capitalize(ParametersManager.UserContext.FirstName) + "!";
        }

        private void AddressDefault(UserAddress userAddress)
        {
            DefaultAddress = userAddress;
        }

        public void OnEditItemClicked(UserAddress userAddress)
        {
            this.EventEditAddress(userAddress);
        }

        public void OnDelateItemClicked(UserAddress userAddress)
        {
            this.EventDeleteAddress(userAddress);
        }

        public async void OnSelectDefaultItemClicked(UserAddress userAddress)
        {
            DataActionAddress = userAddress;
            TypeAction = ConstantEventName.DefaultItemClicked;

            if (Addresses != null)
            {
                var adress = Addresses.Where(x => x.IsDefaultAddress = true)?.FirstOrDefault();
                DataActionAddress.SelectedAddress = adress != null ? adress.AddressName : string.Empty;
                var result = await this.SetDefaultAddress(DataActionAddress);

                if (result)
                {
                    ParametersManager.ChangeAddress = true;
                    ParametersManager.ChangeProductQuantity = true;
                    SaveAddress(DataActionAddress);
                    Addresses.ForEach(x => x.IsDefaultAddress = false);
                    Addresses[Addresses.IndexOf(DataActionAddress)].IsDefaultAddress = true;
                    Addresses = Addresses.OrderBy(x => x.IsDefaultAddress ? 0 : 1).ToList();
                    RegisterNotificationTags();
                    if (!CurrentCity.Equals(userAddress.CityId))
                    {
                        DeletePreferencesLastDateUpdatedPromotion();
                    }
                    Finish();
                }
            }
        }

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
            });
        }

        private void EventEditAddress(UserAddress userAddress)
        {
            Intent intent = new Intent(this, typeof(AddressActivity));

            if (userAddress != null)
            {
                intent.PutExtra(ConstantPreference.Address, JsonService.Serialize<UserAddress>(userAddress));
                intent.PutExtra(ConstantPreference.PreviousActivity, ConstantActivityName.MyAddressActivity);
            }

            StartActivity(intent);
        }

        private void EventDeleteAddress(UserAddress userAddress)
        {
            DataActionAddress = userAddress;
            TypeAction = ConstantEventName.DeleteItemClicked;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.DeleteAddressChangeMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        private void EventAddAddress()
        {
            Intent intent = new Intent(this, typeof(AddressActivity));
            intent.PutExtra(ConstantPreference.PreviousActivity, ConstantActivityName.MyAddressActivity);
            StartActivity(intent);
        }

        private async Task ShowStoresDialog()
        {
            CitiesStore = await LoadCitiesStore();
            Stores = new List<Store>();
            Stores.Insert(0, new Store { Id = string.Empty, Name = AppMessages.Choose });
            storesDialog = new AlertDialog.Builder(this).Create();
            View storesView = LayoutInflater.Inflate(Resource.Layout.DialogChooseStore, null);
            storesDialog.SetView(storesView);
            storesDialog.SetCanceledOnTouchOutside(false);
            TextView tvAddStore = storesView.FindViewById<TextView>(Resource.Id.tvAddStore);
            TextView tvInStoreCity = storesView.FindViewById<TextView>(Resource.Id.tvInStoreCity);
            TextView tvStore = storesView.FindViewById<TextView>(Resource.Id.tvStore);
            Button btnAddStore = storesView.FindViewById<Button>(Resource.Id.btnAddStore);
            Button btnCancel = storesView.FindViewById<Button>(Resource.Id.btnCancel);
            LyChooseStore = storesView.FindViewById<LinearLayout>(Resource.Id.lyChooseStore);
            SpCity = storesView.FindViewById<Spinner>(Resource.Id.spCity);
            SpStore = storesView.FindViewById<Spinner>(Resource.Id.spStore);
            SpCity.OnItemSelectedListener = this;
            tvAddStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvInStoreCity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            btnAddStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            btnCancel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);

            btnAddStore.Click += async delegate { await SaveInStore(); };
            btnCancel.Click += delegate { storesDialog.Cancel(); };
            storesDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            storesDialog.Show();
            this.DrawCitiesStore();
        }

        public async void OnItemSelected(AdapterView parent, View view, int position, long id)
        {
            City city = CitiesStore[position];

            if (!city.Id.Equals("0"))
            {
                StoreParameters parameters = new StoreParameters { CityId = city.Id, HomeDelivery = false, PickUp = true, IdPickup = city.IdPickup };
                Stores = await LoadStores(parameters);
            }

            if (!Stores.Any())
            {
                Stores = new List<Store>();
                Stores.Insert(0, new Store { Id = string.Empty, Name = AppMessages.Choose });
            }

            DrawStores();
        }

        public void OnNothingSelected(AdapterView parent)
        {
            // Is required because of IOnItemSelectedListener implementation
        }

        private async Task SaveInStore()
        {
            try
            {
                storesDialog.Dismiss();
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserStore = SetStore();
                string message = AddressModel.ValidateFieldsInStore(UserStore);

                if (string.IsNullOrEmpty(message))
                {
                    UpdateDispatchRegionParameters parameters = new UpdateDispatchRegionParameters
                    {
                        CityId = UserStore.CityId,
                        Region = UserStore.Region,
                        DependencyId = UserStore.Id
                    };

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
                        ParametersManager.ChangeAddress = true;
                        ParametersManager.ChangeProductQuantity = true;
                        City city = CitiesStore[SpCity.SelectedItemPosition];
                        HideProgressDialog();
                        DrawSelectedStore();
                        ShowBodyLayout();
                        RegisterNotificationTags();
                        AddStoreScreenView();

                        if (!CurrentCity.Equals(city.Id))
                        {
                            DeletePreferencesLastDateUpdatedPromotion();
                        }

                        HideProgressDialog();
                        storesDialog.Cancel();
                    }
                }
                else
                {
                    HideProgressDialog();
                    ShowErrorLayout(message);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.AddressesDialogActivity, ConstantMethodName.SaveInStore } };
                RegisterMessageExceptions(exception, properties);
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        public void OnStoreTouched(bool error, string message)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogHowDoYouLike, null);
            storesDialog = new AlertDialog.Builder(this).Create();
            storesDialog.SetView(view);
            storesDialog.SetCanceledOnTouchOutside(false);
            LinearLayout LyHowDoYouLike = view.FindViewById<LinearLayout>(Resource.Id.lyHowDoYouLike);
            LinearLayout LyCustomError = view.FindViewById<LinearLayout>(Resource.Id.lyCustomError);
            LinearLayout LyCustomSuccess = view.FindViewById<LinearLayout>(Resource.Id.lyCustomSuccess);
            LyHowDoYouLike.Visibility = ViewStates.Gone;
            LyCustomError.Visibility = ViewStates.Gone;
            LyCustomSuccess.Visibility = ViewStates.Gone;

            LyHowDoYouLike.Visibility = ViewStates.Gone;

            if (error)
            {
                LyCustomError.Visibility = ViewStates.Visible;
                Button BtnReturnError = LyCustomError.FindViewById<Button>(Resource.Id.btnReturn);
                ImageView ImgCloseError = LyCustomError.FindViewById<ImageView>(Resource.Id.imgCloseTwo);
                TextView TvCannotFinishSuccess = LyCustomError.FindViewById<TextView>(Resource.Id.tvCannotFinishSuccess);
                TextView TvApology = LyCustomError.FindViewById<TextView>(Resource.Id.tvApology);
                TvCannotFinishSuccess.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvApology.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
                TvCannotFinishSuccess.Text = message;

                ImgCloseError.Click += delegate { storesDialog.Dismiss(); };
                BtnReturnError.Click += delegate { storesDialog.Dismiss(); };

            }
            else
            {
                LyCustomSuccess.Visibility = ViewStates.Visible;
                Button BtnReturnSuccess = LyCustomSuccess.FindViewById<Button>(Resource.Id.btnAccept);
                ImageView ImgCloseSuccess = LyCustomSuccess.FindViewById<ImageView>(Resource.Id.imgCloseTwo);
                TextView TvSuccessMessage = LyCustomSuccess.FindViewById<TextView>(Resource.Id.tvSuccessMessage);
                TextView TvSuccess = LyCustomSuccess.FindViewById<TextView>(Resource.Id.tvSuccess);
                TvSuccessMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvSuccess.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
                TvSuccessMessage.Text = message;
                ImgCloseSuccess.Click += delegate { storesDialog.Dismiss(); };
                BtnReturnSuccess.Click += delegate { storesDialog.Dismiss(); };
            }

            storesDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            storesDialog.Show();
        }

        private Store SetStore()
        {
            City city = CitiesStore[SpCity.SelectedItemPosition];
            Store store = Stores[SpStore.SelectedItemPosition];
            store = store ?? new Store();

            if (city != null && store != null)
            {
                store.CityId = city.Id;
                store.IdPickup = city.IdPickup;
                store.Region = city.Region;
                store.City = city.Name;
            }

            return store;
        }

        private void UpdateCurrentCity()
        {
            if (ParametersManager.UserContext != null)
            {
                if (ParametersManager.UserContext.Address != null)
                {
                    CurrentCity = ParametersManager.UserContext.Address.CityId;
                }
                else
                {
                    CurrentCity = ParametersManager.UserContext.Store.CityId;
                }
            }
        }

        private void AddStoreScreenView()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.AddStore, typeof(AddressesDialogActivity).Name);
        }

        private void AddressesDialogDeliverScreenView()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.AddressesDialogDeliver, typeof(AddressesDialogActivity).Name);
        }

        private void AddressesDialogStoreScreenView()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.AddressesDialogStore, typeof(AddressesDialogActivity).Name);
        }
    }
}