using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Addresses;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.Orders;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Products;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Views.View;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Resumen de pedido", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SummaryActivity : BaseProductActivity, ISummaryProducts, IOnTouchListener
    {
        #region Controls

        private LinearLayout LyToastSendTo;
        private LinearLayout LyEmptyCar;
        private LinearLayout LyFlushCar;
        private LinearLayout LyBuy;
        private LinearLayout LySummary;
        private LinearLayout LyActions;
        private LinearLayout LyDetails;
        private RecyclerView RvProducts;
        private SummaryProductAdapter SummaryProductAdapter;
        private LinearLayoutManager ProductsLayoutManager;
        private Button BtnAddProducts, BtnAccept;
        private TextView TvFlushCar;
        private TextView TvBuy;
        private TextView TvMessageEmptyCar;
        private TextView TvSendTo;
        private TextView TvSendToAddress;
        private TextView TvMyPlace;
        private TextView TvSubTotalSummary;
        private TextView TvCostBagTitleSummary;
        private ImageView IvBoxTaxes;
        private EditText EtHowDoYouLike, EtPhoneNumber;
        private View viewDivider;
        private AlertDialog SkipDialog;
        private NestedScrollView NsvProducts;
        private Spinner SpCityDelivery;

        #endregion

        #region Properties

        private AddressModel _addressModel { get; set; }
        private ProductsModel _productModel { get; set; }
        private Summary Summary { get; set; }
        private ProductCarModel _productCarModel;
        private bool IsFlushCar = false;
        private Product SelectedProduct { get; set; }
        private bool EventDetails { get; set; }
        private IList<City> CitiesAddresses { get; set; }
        private bool keepLobby { get; set; }

        #endregion

        #region Public Metohod

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        public void OnProductClicked(Product product)
        {
            var productView = new Intent(this, typeof(ProductActivity));
            productView.PutExtra(ConstantPreference.Product, JsonService.Serialize(product));
            StartActivity(productView);
        }

        public void OnAddPressed(Product product)
        {
            var summary = _productCarModel.UpSertProduct(product, true);

            if (product != null)
            {
                SummaryProductAdapter.NotifyItemChanged(Summary.Products.IndexOf(product));
            }

            SetToolbarCarItems(true);
            RegisterAddProductEvent(product);
        }

        public void OnSubstractPressed(Product product)
        {
            if (product.Quantity == 1)
            {
                DeleteSummaryProduct(product);
            }
            else
            {
                var summary = _productCarModel.UpSertProduct(product, false);

                if (product != null)
                {
                    SummaryProductAdapter.NotifyItemChanged(Summary.Products.IndexOf(product));
                }

                SetToolbarCarItems(true);
            }

            RegisterDeleteProduct(product);
        }

        public void OnAddProduct(Product product)
        {
            OnAddPressed(product);
        }

        public void OnAddToListClicked(Product product)
        {
        }

        public void OnHoWDoYouLikeTouched(Product product = null, bool error = false, string message = null)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogHowDoYouLike, null);
            SkipDialog = new AlertDialog.Builder(this).Create();
            SkipDialog.SetView(view);
            SkipDialog.SetCanceledOnTouchOutside(false);
            LinearLayout LyHowDoYouLike = view.FindViewById<LinearLayout>(Resource.Id.lyHowDoYouLike);
            LinearLayout LyCustomError = view.FindViewById<LinearLayout>(Resource.Id.lyCustomError);
            LinearLayout LyCustomSuccess = view.FindViewById<LinearLayout>(Resource.Id.lyCustomSuccess);
            LyHowDoYouLike.Visibility = ViewStates.Gone;
            LyCustomError.Visibility = ViewStates.Gone;
            LyCustomSuccess.Visibility = ViewStates.Gone;

            if (product != null)
            {
                LyHowDoYouLike.Visibility = ViewStates.Visible;
                Button btnOk = view.FindViewById<Button>(Resource.Id.btnSave);
                TextView tvHowDoYouLike = view.FindViewById<TextView>(Resource.Id.tvHowDoYouLike);
                EtHowDoYouLike = view.FindViewById<EditText>(Resource.Id.etHowDoYouLike);
                ImageView ivClose = view.FindViewById<ImageView>(Resource.Id.ivClose);
                btnOk.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                tvHowDoYouLike.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
                EtHowDoYouLike.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                EtHowDoYouLike.Text = product.Note;
                btnOk.Click += delegate { SaveHowDoYouLike(product); };
                ivClose.Click += delegate { SkipDialog.Dismiss(); };
            }
            else
            {
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

                    ImgCloseError.Click += delegate { SkipDialog.Dismiss(); };
                    BtnReturnError.Click += delegate { SkipDialog.Dismiss(); };

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
                    ImgCloseSuccess.Click += delegate { SkipDialog.Dismiss(); };
                    BtnReturnSuccess.Click += delegate { SkipDialog.Dismiss(); };
                }
            }

            SkipDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            SkipDialog.Show();
        }

        public void SaveHowDoYouLike(Product product)
        {
            try
            {
                if (!string.IsNullOrEmpty(EtHowDoYouLike.Text.Trim()))
                {
                    SkipDialog.Dismiss();
                    product.Note = EtHowDoYouLike.Text.Trim();
                    _productCarModel.UpSertProduct(product);

                    int index = Summary.Products.IndexOf(product);

                    if (index != 1)
                    {
                        Summary.Products[index].Note = product.Note;
                        SummaryProductAdapter.NotifyDataSetChanged();
                    }

                    this.OnHoWDoYouLikeTouched(null, false, AppMessages.HowDoYouLikeMessageText);
                }
                else
                {
                    HideProgressDialog();
                    SkipDialog.Dismiss();
                    this.OnHoWDoYouLikeTouched(null, true, AppMessages.RequiredFieldsText);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryActivity, ConstantMethodName.SaveHowDoYouLike } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        public void OnRemoveTouched(Product product)
        {
            IsFlushCar = false;
            SelectedProduct = product;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.DeleteProductSummaryMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            ValidateInformationAddress();
            return false;
        }

        public void DialogBillingAddress()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogBillingAddress, null);
            AlertDialog billindAddressDialog = new AlertDialog.Builder(this).Create();
            billindAddressDialog.SetView(view);
            billindAddressDialog.SetCanceledOnTouchOutside(false);
            ImageView imgCloseTwo = view.FindViewById<ImageView>(Resource.Id.imgCloseTwo);
            TextView tvTitleBillingAddress = view.FindViewById<TextView>(Resource.Id.tvTitleBillingAddress);
            TextView tvMessageAddressPartOne = view.FindViewById<TextView>(Resource.Id.tvMessageAddressPartOne);
            TextView tvMessageAddressPartTwo = view.FindViewById<TextView>(Resource.Id.tvMessageAddressPartTwo);
            TextView tvCity = view.FindViewById<TextView>(Resource.Id.tvCity);
            SpCityDelivery = view.FindViewById<Spinner>(Resource.Id.spCityDelivery);
            TextView tvAddress = view.FindViewById<TextView>(Resource.Id.tvAddress);
            ActvAddress = view.FindViewById<AutoCompleteTextView>(Resource.Id.actvAddress);
            TextView tvExampleAddress = view.FindViewById<TextView>(Resource.Id.tvExampleAddress);
            TextView tvPhoneNumber = view.FindViewById<TextView>(Resource.Id.tvPhoneNumber);
            EtPhoneNumber = view.FindViewById<EditText>(Resource.Id.etPhoneNumber);
            BtnAccept = view.FindViewById<Button>(Resource.Id.btnAccept);
            BtnAccept.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            tvTitleBillingAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            tvMessageAddressPartOne.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvMessageAddressPartTwo.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvCity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            ActvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvExampleAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvPhoneNumber.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtPhoneNumber.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            imgCloseTwo.Click += delegate { billindAddressDialog.Dismiss(); };
            BtnAccept.Enabled = false;
            BtnAccept.Click += async delegate { await this.SaveCorrespondence(); billindAddressDialog.Dismiss(); };
            ActvAddress.TextChanged += ActvAddress_TextChanged;
            EtPhoneNumber.TextChanged += EditText_TextChanged;

            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone) && !ParametersManager.UserContext.CellPhone.Equals(AppConfigurations.DefaultCellphone))
            {
                EtPhoneNumber.Text = ParametersManager.UserContext.CellPhone;

                if (string.IsNullOrEmpty(AddressModel.ValidateFieldCellPhone(ParametersManager.UserContext.CellPhone)))
                {
                    tvPhoneNumber.Visibility = ViewStates.Gone;
                    EtPhoneNumber.Visibility = ViewStates.Gone;
                    ActvAddress.ImeOptions = ImeAction.Done;
                }
            }

            ActvAddress.TextChanged += EditText_TextChanged;
            SpCityDelivery.ItemSelected += Spinner_ItemSelected;
            billindAddressDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            billindAddressDialog.Show();
            ActvAddress.SetOnTouchListener(this);
            SpCityDelivery.SetOnTouchListener(this);
            EtPhoneNumber.SetOnTouchListener(this);
            DrawCities();
        }

        #endregion

        #region Protected Method       

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.Summary = new Summary();
            SetContentView(Resource.Layout.ActivitySummary);
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            EventDetails = false;
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            this.ScrollEnd(TvSendTo, 0);
            
            RunOnUiThread(async () =>
            {
                await UpdateProductsPrice();
                GetSummary();
                SetToolbarCarItems(true);
            });
        }

        protected override void OnResume()
        {
            base.OnResume();
            this.SetTextAddress();

            if (Intent.Extras != null && Intent.Extras.GetBoolean(ConstantPreference.KeepLobby))
            {
                keepLobby = true;
            }

            if (TvToolbarPrice != null)
            {
                SetToolbarCarItems(true);
            }

            if (ParametersManager.ChangeAddress)
            {
                RunOnUiThread(async () =>
                {
                    await UpdateProductsPrice();
                    GetSummary();
                    SetToolbarCarItems(true);
                });
            }

            if (ParametersManager.ChangeProductQuantity ||
                (this.Summary == null || this.Summary.Products == null || !this.Summary.Products.Any()))
            {
                GetSummary();
                SetToolbarCarItems(true);
            }
            else
            {
                InitAddresses();
            }
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Summary, typeof(SummaryActivity).Name);
        }

        protected override void EventYesGenericDialog()
        {
            GenericDialog.Hide();

            if (IsFlushCar)
            {
                FlushCar();
            }
            else
            {
                DeleteSummaryProduct(SelectedProduct);
            }
        }

        protected override void EventNotGenericDialog()
        {
            base.EventNotGenericDialog();
            IsFlushCar = false;
            GenericDialog.Hide();
        }

        protected override void EventError()
        {
            base.EventError();
            OnResume();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        public override void OnBackPressed()
        {
            ValidateBackMainActivity();
        }

        #endregion

        #region Private Method   

        private void DeleteSummaryProduct(Product product)
        {
            RegisterDeleteProduct(product);
            Summary.Products.Remove(Summary.Products.Where(x => x.Id.Equals(product.Id)).FirstOrDefault());
            _productCarModel.DeleteProduct(product.Id);
            _productCarModel.RecalculateSummary();
            SummaryProductAdapter.NotifyDataSetChanged();
            SetToolbarCarItems(true);
            this.ValidateProductsInSummary();
        }

        private void SetControlsProperties()
        {
            LyDetails = FindViewById<LinearLayout>(Resource.Id.lyDetail);
            HideItemsToolbar(this);
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice),
                FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetSetSummaryControls(LyDetails.FindViewById<TextView>(Resource.Id.tvSubTotalPriceSummary),
                LyDetails.FindViewById<TextView>(Resource.Id.tvCostBagPriceSummary));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
            FindViewById<LinearLayout>(Resource.Id.lySummaryRoot));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            LyEmptyCar = FindViewById<LinearLayout>(Resource.Id.lyEmptyCar);
            LySummary = FindViewById<LinearLayout>(Resource.Id.lySummary);
            LyActions = FindViewById<LinearLayout>(Resource.Id.lyActions);
            LyFlushCar = FindViewById<LinearLayout>(Resource.Id.lyFlushCar);
            LyBuy = FindViewById<LinearLayout>(Resource.Id.lyBuy);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            TvMessageEmptyCar = FindViewById<TextView>(Resource.Id.tvMessageEmptyCar);
            BtnAddProducts = FindViewById<Button>(Resource.Id.btnAddProducts);
            RvProducts = FindViewById<RecyclerView>(Resource.Id.rvProducts);
            TvFlushCar = FindViewById<TextView>(Resource.Id.tvFlushCar);
            TvBuy = FindViewById<TextView>(Resource.Id.tvBuy);
            viewDivider = FindViewById<View>(Resource.Id.viewDivider);
            NsvProducts = FindViewById<NestedScrollView>(Resource.Id.nsvProducts);
            LyToastSendTo = FindViewById<LinearLayout>(Resource.Id.toastSendTo);
            TvSendTo = FindViewById<TextView>(Resource.Id.tvSendTo);
            TvSendToAddress = FindViewById<TextView>(Resource.Id.tvSendToAddress);
            IvBoxTaxes = LyDetails.FindViewById<ImageView>(Resource.Id.ivInformation);
            TvMyPlace = FindViewById<TextView>(Resource.Id.tvMyPlace);
            TvSubTotalSummary = LyDetails.FindViewById<TextView>(Resource.Id.tvSubTotalSummary);
            TvCostBagTitleSummary = LyDetails.FindViewById<TextView>(Resource.Id.tvCostBagTitleSummary);

            IvToolbarBack.Click += delegate { ValidateBackMainActivity(); };
            ProductsLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvProducts.NestedScrollingEnabled = false;
            RvProducts.HasFixedSize = true;
            RvProducts.SetLayoutManager(ProductsLayoutManager);
            LyFlushCar.Click += delegate { FlushCarConfirmation(); };
            LyBuy.Click += async delegate { await GoToOrderScheduleActivity(); };
            BtnAddProducts.Click += delegate { AddMoreProducts(); };

            LyToastSendTo.Click += delegate
            {
                OnBoxMessageTouched(AppMessages.MessageChangeAdress, AppMessages.MessagebtnChangeAdress);
            };

            IvBoxTaxes.Click += delegate { OnBoxTaxesTouched(); };
            FindViewById<LinearLayout>(Resource.Id.lyCostBag).Click += delegate { OnBoxTaxesTouched(); };
        }

        private void ValidateBackMainActivity()
        {
            if (keepLobby)
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.PutExtra(ConstantPreference.KeepLobby, true);
                intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
                StartActivity(intent);
                Finish();
            }
            else
            {
                Finish();
            }
        }

        protected override void EventCloseModalMessageInfo()
        {
            base.EventCloseModalMessageInfo();
            MessageInfoDialog.Hide();
            Intent intent = new Intent(this, typeof(AddressesDialogActivity));
            StartActivity(intent);
        }

        private void SetTextAddress()
        {
            if (ParametersManager.UserContext != null && (ParametersManager.UserContext.Address != null || ParametersManager.UserContext.Store != null))
            {
                LyToastSendTo.Visibility = ViewStates.Visible;
                TvSendToAddress.Text = ParametersManager.UserContext.Address != null ?
                              ParametersManager.UserContext.Address.City + ", " + ParametersManager.UserContext.Address.AddressComplete :
                              ParametersManager.UserContext.Store.City + ", " + ParametersManager.UserContext.Store.Name;

                TvSendTo.Text = ParametersManager.UserContext.Address != null ? AppMessages.GetAddressText : AppMessages.GetStoreText;
                TvMyPlace.Text = ParametersManager.UserContext.Address != null ? ParametersManager.UserContext.Address.Description : string.Empty;
                TvMyPlace.Visibility = ParametersManager.UserContext.Address != null ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        private void AddMoreProducts()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra(ConstantPreference.KeepLobby, true);
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
        }

        private void FlushCarConfirmation()
        {
            IsFlushCar = true;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.FlushCarMessage);
            ShowGenericDialogDialog(dataDialog);
        }

        private async Task GoToOrderScheduleActivity()
        {
            await ValidateCorrespondence();
        }

        private async Task ValidateCorrespondence()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
                CorrespondenceRespondese response = await _addressModel.ValidateCorrespondence();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    this.RunOnUiThread(async () =>
                    {
                        if (!response.HaveCorreponseAddres)
                        {
                            HideProgressDialog();
                            this.DialogBillingAddress();
                        }
                        else
                        {
                            await SaveProducts();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryActivity, ConstantMethodName.ValidateCorrespondence } };
                ShowAndRegisterMessageExceptions(ex, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private void DrawCities()
        {
            List<string> citiesName = CitiesAddresses.ToList().Select(x => x.Name).ToList();

            if (citiesName.Count > 0)
            {
                this.RunOnUiThread(() =>
                {
                    ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, citiesName);
                    SpCityDelivery.Adapter = adapter;
                });
            }
        }

        private async Task SaveCorrespondence()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));

                if (ValidateInformationAddress())
                {
                    City city = AddressModel.GetCity(CitiesAddresses[SpCityDelivery.SelectedItemPosition].Name, this.CitiesAddresses);

                    UserAddress userAddress = new UserAddress
                    {
                        AddressComplete = ActvAddress.Text,
                        CellPhone = EtPhoneNumber.Text,
                        CityId = city.Id,
                        StateId = city.State,
                        Region = city.Region
                    };

                    CorrespondenceRespondese response = await _addressModel.SaveCorrespondence(userAddress);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    }
                    else
                    {
                        if (response.Error)
                        {
                            HideProgressDialog();
                            DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.SaveCorrespondenceMessageError, AppMessages.AcceptButtonText);
                        }
                        else
                        {
                            await SaveProducts();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryActivity, ConstantMethodName.SaveCorrespondence } };
                ShowAndRegisterMessageExceptions(ex, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private bool ValidateInformationAddress()
        {
            string cityId = null;
            string messageError = null;
            bool passed = true;

            if (SpCityDelivery.SelectedItemPosition != -1 && SpCityDelivery.SelectedItemPosition != 0)
            {
                City city = AddressModel.GetCity(CitiesAddresses[SpCityDelivery.SelectedItemPosition].Name, this.CitiesAddresses);
                cityId = city.Id;
            }

            UserAddress userAddress = new UserAddress
            {
                AddressComplete = ActvAddress.Text,
                CellPhone = EtPhoneNumber.Text,
                CityId = cityId
            };

            messageError = AddressModel.ValidateFieldsAddressCorrespondence(userAddress);

            if (!string.IsNullOrEmpty(messageError))
            {
                BtnAccept.Enabled = false;
                BtnAccept.SetBackgroundResource(Resource.Drawable.button_transparent);
                passed = false;
            }
            else
            {
                BtnAccept.Enabled = true;
                BtnAccept.SetBackgroundResource(Resource.Drawable.button_yellow);
                passed = true;
            }

            return passed;
        }

        private void EditFonts()
        {
            TvFlushCar.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvBuy.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            BtnAddProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvMessageEmptyCar.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvSendTo.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSendToAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvMyPlace.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSubTotalSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCostBagTitleSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void GetSummary()
        {
            try
            {
                this.Summary = new Summary
                {
                    Products = _productCarModel.GetProducts()
                };

                this.PutSummaryInfo();
                this.ValidateProductsInSummary();
                this.InitAddresses();
            }
            catch (Exception ex)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryActivity, ConstantMethodName.GetSummary } };
                ShowAndRegisterMessageExceptions(ex, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private void ValidateProductsInSummary()
        {
            if (Summary.Products == null || !Summary.Products.Any())
            {
                LyEmptyCar.Visibility = ViewStates.Visible;
                LySummary.Visibility = ViewStates.Gone;
                LyActions.Visibility = ViewStates.Gone;
                LyToastSendTo.Visibility = ViewStates.Gone;
                LyDetails.Visibility = ViewStates.Gone;
            }
            else
            {
                LyEmptyCar.Visibility = ViewStates.Gone;
                LySummary.Visibility = ViewStates.Visible;
                LyActions.Visibility = ViewStates.Visible;
                LyToastSendTo.Visibility = ViewStates.Visible;
                LyDetails.Visibility = ViewStates.Visible;
                ShowBodyLayout();
            }
        }

        private void PutSummaryInfo()
        {
            string actualId = "";

            if (this.Summary.Products != null && this.Summary.Products.Any())
            {
                foreach (Product product in this.Summary.Products)
                {
                    if (!string.IsNullOrEmpty(product.CategoryId))
                    {
                        if (product.CategoryId.Equals(actualId))
                        {
                            product.CategoryId = null;
                        }
                        else
                        {
                            actualId = product.CategoryId;
                        }
                    }
                }

                SummaryProductAdapter = new SummaryProductAdapter(this.Summary.Products, this, this);

                this.RunOnUiThread(() =>
                {
                    RvProducts.SetAdapter(SummaryProductAdapter);
                    RvProducts.ClearFocus();
                    this.ScrollEnd(TvSendTo, 0);
                });
            }
        }

        private void FlushCar()
        {
            RegisterDeleteProductsFromCart();
            _productCarModel.FlushCar();
            Summary.Products.Clear();
            SetToolbarCarItems(true);
            this.ValidateProductsInSummary();
        }

        private async void ActvAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (ActvAddress.Text.Length > 3)
                {
                    var addressPredictionResponse = await AddressModel.AutoCompleteAddress(ActvAddress.Text);

                    if (addressPredictionResponse != null && addressPredictionResponse.Predictions != null)
                    {
                        List<string> predictions = addressPredictionResponse.Predictions.ToList()
                            .Where(x => x.Description.ToLower().Contains(AppConfigurations.CountryGeolocation))
                            .Select(x => x.Description).ToList();

                        this.RunOnUiThread(() =>
                        {
                            ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, predictions);
                            ActvAddress.Adapter = adapter;
                            ActvAddress.ShowDropDown();
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.AutoCompleteAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void EditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
            ValidateInformationAddress();
        }

        private void InitAddresses()
        {
            this.RunOnUiThread(async () =>
            {
                CitiesAddresses = await LoadCitiesAddresses();
            });
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            ValidateInformationAddress();
        }

        private async Task SaveProducts()
        {
            try
            {
                List<Product> products = _productCarModel.GetProducts();
                var responseOrder = await OrderModel.GetOrder();

                if (responseOrder.Result != null && responseOrder.Result.HasErrors && responseOrder.Result.Messages != null)
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(responseOrder.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    Order order = new Order()
                    {
                        Id = responseOrder.ActiveOrderId,
                        DependencyId = ParametersManager.UserContext.DependencyId,
                        Products = products,
                        TotalProducts = products.Count,
                        SubTotal = GetSubTotalPrice()
                    };

                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
                    await SaveProductsInTheOrder(order);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryActivity, ConstantMethodName.SaveProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private decimal GetSubTotalPrice()
        {
            var productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            var summary = productCarModel.GetSummary();

            if (summary != null)
            {
                return decimal.Parse(summary[ConstDataBase.TotalPrice].ToString());
            }

            return 0;
        }

        private async Task SaveProductsInTheOrder(Order order)
        {
            _productModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
            var responseAddProductModel = await _productModel.AddProducts(order);

            if (responseAddProductModel.Result != null && responseAddProductModel.Result.HasErrors && responseAddProductModel.Result.Messages != null)
            {
                HideProgressDialog();
                DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(responseAddProductModel.Result), AppMessages.AcceptButtonText);
            }
            else
            {
                RegisterContinueEvent();
                HideProgressDialog();
                Intent intent = new Intent(this, typeof(OrderScheduleActivity));
                StartActivity(intent);
            }
        }

        private void ScrollEnd(View view, int start)
        {
            Handler handler = new Handler();

            void productAction()
            {
                NsvProducts.SmoothScrollingEnabled = true;
                int scrollTo = ((View)view.Parent.Parent).Top + view.Top + start;
                NsvProducts.SmoothScrollTo(0, scrollTo);
            }

            handler.PostDelayed(productAction, 200);
        }

        #endregion

        #region Analytic

        private void RegisterAddProductEvent(Product product)
        {
            FirebaseRegistrationEventsService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookRegistrationEventsService.Instance.AddProductToCart(product);
        }

        public void RegisterDeleteProduct(Product product)
        {
            FirebaseRegistrationEventsService.Instance.DeleteProductFromCart(product, product.CategoryName);
        }

        public void RegisterContinueEvent()
        {
            FirebaseRegistrationEventsService.Instance.Summary();
            FacebookRegistrationEventsService.Instance.InitiatedCheckout(true, ParametersManager.Order.Products);
        }

        public void RegisterDeleteProductsFromCart()
        {
            FirebaseRegistrationEventsService.Instance.DeleteProductsFromCart(Summary.Products);
        }

        #endregion
    }
}