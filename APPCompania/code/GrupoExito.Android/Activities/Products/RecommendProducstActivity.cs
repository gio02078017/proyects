using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Payments;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Mis Listas Recomendadas", ScreenOrientation = ScreenOrientation.Portrait)]
    public class RecommendProducstActivity : BaseActivity, IRecommendedAdapter
    {
        #region Controls

        private RecyclerView RvMyRecommendList;
        private LinearLayoutManager linerLayoutManager;
        private RecommendProductsAdapter recommendProductsAdapter;
        private ImageView IvAddlist;
        private TextView TvListName;
        private TextView TvListQuantity;
        private TextView TvSelectedPrice;
        private TextView TvSelectedQuantity;
        private LinearLayout LyEditMyList;
        private LinearLayout LyAddCar;
        private ConstraintLayout LyInsideAddCar;
        private CheckBox ChkSelectAll;
        private TextView TvAddCar;
        private ImageView IvAddCar;
        private EditText EtNameList;
        private LinearLayout LySelectAll;
        private LinearLayout LyCancelEditMyList;
        private TextView TvCancelEditMyList;

        #endregion

        #region Properties

        private ShoppingListModel ShoppingListModel { get; set; }
        private SuggestedProductsResponse SuggestedProductsResponse;
        private bool ControlCheck;
        private string ListId;
        private bool ViewDelete;
        private bool Edit;
        private int PositionDelete;
        private bool EventEdit;
        private bool updateNow;
        private ProductCarModel _productCarModel;
        private ProductList ProductDelete { get; set; }
        private int _currentPosition { get; set; }

        #endregion

        #region Public Methods

        public void OnItemSelected(ProductList products, int position)
        {
            _currentPosition = position;
            GetTotalValues();
            this.ValidateCheck();
        }

        public void OnAddPressed(ProductList product, int position)
        {
            _currentPosition = position;
            GetTotalValues();
            this.ValidateCheck();
        }

        public void OnSubstractPressed(ProductList product, int position)
        {
            _currentPosition = position;
            GetTotalValues();
            this.ValidateCheck();
        }

        public void OnItemDeleted(ProductList product, int position)
        {
            ProductDelete = product;
            PositionDelete = position;
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.DeleteProductList);
            ShowGenericDialogDialog(dataDialog);
        }

        #endregion

        #region Protected Methods

        protected override void GoToSummary()
        {
            Intent intent = new Intent(this, typeof(SummaryActivity));
            StartActivity(intent);
            Finish();
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityRecommendedProducts);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            ShoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);

            HideItemsCarToolbar(this);
            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.ListId)))
                {
                    ListId = Intent.Extras.GetString(ConstantPreference.ListId);
                    ViewDelete = true;
                    await this.GetList();
                    LyEditMyList.Visibility = ViewStates.Visible;
                }
                else
                {
                    await this.GetRecommendProducts();
                }
            }
            else
            {
                await this.GetRecommendProducts();
            }            
        }

        protected async override void OnResume()
        {
            base.OnResume();

            if (TvToolbarPrice != null)
            {
                SetToolbarCarItems(true);
            }

            if (!string.IsNullOrEmpty(ListId))
            {
                ViewDelete = true;
                await this.GetList();
                LyEditMyList.Visibility = ViewStates.Visible;
                LyCancelEditMyList.Visibility = ViewStates.Gone;
                TvListName.Visibility = ViewStates.Visible;
                EtNameList.Visibility = ViewStates.Gone;
            }
            else
            {
                LyEditMyList.Visibility = ViewStates.Gone;
                await this.GetRecommendProducts();
            }
            GetTotalValues();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, ViewDelete ? AnalyticsScreenView.EditList : AnalyticsScreenView.RecommendProducst, typeof(RecommendProducstActivity).Name);
        }

        protected async override void EventError()
        {
            base.EventError();
            if (ViewDelete)
            {
                await this.GetList();
            }
            else
            {
                await this.GetRecommendProducts();
            }

        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        protected async override void EventYesGenericDialog()
        {
            GenericDialog.Hide();
            await this.DeleteProductList(ProductDelete, PositionDelete);
        }

        protected override void EventNotGenericDialog()
        {
            base.EventNotGenericDialog();
            GenericDialog.Hide();

        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        #endregion

        #region Private Methods

        private void EditFonts()
        {
            TvListName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvListQuantity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvSelectedQuantity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvSelectedPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageRecommendList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleProducts).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameSelectAll).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvEditMyList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCancelEditMyList.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleProductsSelect).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAddCar).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            EtNameList.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
               FindViewById<RelativeLayout>(Resource.Id.rlRecommendedLists));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                 FindViewById<RelativeLayout>(Resource.Id.rlRecommendedLists),
                 AppMessages.NotRecommendList, AppMessages.GenericBackAction);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            TvListName = FindViewById<TextView>(Resource.Id.tvNameList);
            TvListQuantity = FindViewById<TextView>(Resource.Id.tvQuantityList);
            TvSelectedPrice = FindViewById<TextView>(Resource.Id.tvPriceSelect);
            TvSelectedQuantity = FindViewById<TextView>(Resource.Id.tvQuantitySelect);
            LyEditMyList = FindViewById<LinearLayout>(Resource.Id.lyEditMyList);
            LyCancelEditMyList = FindViewById<LinearLayout>(Resource.Id.lyCancelEditMyList);
            TvCancelEditMyList = FindViewById<TextView>(Resource.Id.tvCancelEditMyList);
            LyAddCar = FindViewById<LinearLayout>(Resource.Id.lyAddCar);
            LyInsideAddCar = FindViewById<ConstraintLayout>(Resource.Id.lyInsideAddCar);
            TvAddCar = FindViewById<TextView>(Resource.Id.tvAddCar);
            IvAddCar = FindViewById<ImageView>(Resource.Id.ivAddCar);
            EtNameList = FindViewById<EditText>(Resource.Id.etNameList);
            LySelectAll = FindViewById<LinearLayout>(Resource.Id.lySelectAll);
            LyAddCar.Enabled = false;
            IvAddlist = FindViewById<ImageView>(Resource.Id.ivAddlist);
            ChkSelectAll = FindViewById<CheckBox>(Resource.Id.ckSelectAll);
            LyEditMyList.Click += delegate { EventEditListTrue(); };
            LyCancelEditMyList.Click += delegate { EventEditListFalse(); };
            TvCancelEditMyList.Click += delegate { EventEditListFalse(); };
            LyAddCar.Click += async delegate { await this.SelectOpcion(); };
            ChkSelectAll.Click += delegate { OnSelectAll(true); };

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            RvMyRecommendList = FindViewById<RecyclerView>(Resource.Id.rvMyRecommendList);
            EtNameList.AddTextChangedListener(new MyTextWatcher(this));
        }

        private void OnSelectAll(bool updateSelectAll)
        {
            if (updateSelectAll)
            {
                ShoppingListModel.SelectAll(SuggestedProductsResponse.ProductsClient, ChkSelectAll.Checked);
                RvMyRecommendList.GetAdapter().NotifyDataSetChanged();
            }
            else
            {
                RvMyRecommendList.GetAdapter().NotifyItemChanged(_currentPosition);
            }

            GetTotalValues();

            if (updateSelectAll)
            {
                if (ChkSelectAll.Checked)
                {
                    ControlCheck = true;
                }
                else
                {
                    ControlCheck = false;
                }
            }

            this.EnableCar();
        }

        private void EventListRecommend()
        {
        }

        private async Task GetRecommendProducts()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                ProductParameters parameters = new ProductParameters
                {
                    DependencyId = ParametersManager.UserContext.DependencyId,
                    From = ParametersManager.FromRecommendProducts,
                    Size = ParametersManager.SizeRecommendProducts
                };

                SuggestedProductsResponse = await ShoppingListModel.GetSuggestedProducts(parameters);

                if (SuggestedProductsResponse.Result != null && SuggestedProductsResponse.Result.HasErrors &&
                    SuggestedProductsResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        ShowErrorLayout(MessagesHelper.GetMessage(SuggestedProductsResponse.Result));
                    });
                }
                else
                {
                    ShowBodyLayout();
                    SetSuggestedList();
                    SetSuggestedInfo();
                    TvListName.Text = GetString(Resource.String.str_recommend_for_you);
                    HideProgressDialog();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.GetRecommendProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task GetList()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                SuggestedProductsResponse = await ShoppingListModel.GetProductsShoppingList(ListId);

                if (SuggestedProductsResponse.Result != null && SuggestedProductsResponse.Result.HasErrors &&
                    SuggestedProductsResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        ShowErrorLayout(MessagesHelper.GetMessage(SuggestedProductsResponse.Result));
                    });
                }
                else
                {
                    if (SuggestedProductsResponse.ProductsClient.Count > 0)
                    {
                        ShowBodyLayout();
                        SetSuggestedList();
                        SetSuggestedInfo();

                    }
                    else
                    {
                        Intent intent = new Intent(this, typeof(TutorialListActivity));
                        StartActivity(intent);
                    }
                    TvListName.Text = SuggestedProductsResponse.Name;
                    HideProgressDialog();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.GetList } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task DeleteProductList(ProductList product, int position)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                ResponseBase _ResponseBase = await ShoppingListModel.DeleteProductShoppingList(product.ItemId, SuggestedProductsResponse.ListId);

                if (_ResponseBase.Result != null && _ResponseBase.Result.HasErrors &&
                    _ResponseBase.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        ShowErrorLayout(MessagesHelper.GetMessage(_ResponseBase.Result));
                    });
                }
                else
                {
                    SuggestedProductsResponse.ProductsClient.RemoveAt(position); 
                    SetSuggestedInfo();
                    if (SuggestedProductsResponse.ProductsClient.Count > 0)
                    {
                        RvMyRecommendList.GetAdapter().NotifyDataSetChanged();
                    }
                    else
                    {
                        Intent intent = new Intent(this, typeof(TutorialListActivity));
                        StartActivity(intent);
                        Finish();
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.DeleteProductList } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void SetSuggestedInfo()
        {
            TvListQuantity.Text = SuggestedProductsResponse.ProductsClient.Count.ToString();
            FindViewById<TextView>(Resource.Id.tvTitleProducts).Text = SuggestedProductsResponse.ProductsClient.Count == 1 ? AppMessages.Product : AppMessages.Products;

        }

        private void SetSuggestedList()
        {
            RunOnUiThread(() =>
            {

                if (SuggestedProductsResponse.ProductsClient != null && SuggestedProductsResponse.ProductsClient.Count > 0)
                {
                    linerLayoutManager = new LinearLayoutManager(this)
                    {
                        AutoMeasureEnabled = true
                    };
                    RvMyRecommendList.NestedScrollingEnabled = false;
                    RvMyRecommendList.HasFixedSize = true;
                    RvMyRecommendList.SetLayoutManager(linerLayoutManager);
                    recommendProductsAdapter = new RecommendProductsAdapter(SuggestedProductsResponse.ProductsClient, this, this, ViewDelete);
                    RvMyRecommendList.SetAdapter(recommendProductsAdapter);
                }
                else
                {
                    ShowNoInfoLayout();
                }
            });
        }

        private void GetTotalValues()
        {
            TvSelectedQuantity.Text = ShoppingListModel.GetSelectedProductsQuantity(SuggestedProductsResponse.ProductsClient);

            if (!string.IsNullOrEmpty(TvSelectedQuantity.Text) && TvSelectedQuantity.Text.Equals("1"))
            {
                FindViewById<TextView>(Resource.Id.tvTitleProductsSelect).Text = AppMessages.Product;
            }
            else
            {
                FindViewById<TextView>(Resource.Id.tvTitleProductsSelect).Text = AppMessages.Products;
            }

            TvSelectedPrice.Text = ShoppingListModel.GetProductTotal(SuggestedProductsResponse.ProductsClient);
            TvSelectedPrice.Visibility = (string.IsNullOrEmpty(TvSelectedPrice.Text) || TvSelectedPrice.Text.Equals("0")) ? ViewStates.Gone : ViewStates.Visible;
        }

        private void AddProductsToCar()
        {
            try
            {
                var productList = SuggestedProductsResponse.ProductsClient.Where(x => x.Selected).ToList();

                if (productList != null && productList.Any())
                {
                    _productCarModel.UpsertProducts(productList);
                    _productCarModel.RecalculateSummary();
                    SetToolbarCarItems(true);
                    GoToSummary();

                    foreach (var item in productList)
                    {
                        RegisterAddProductEvent(item);
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.AddProductsToCar } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void ValidateCheck()
        {
            var productList = SuggestedProductsResponse.ProductsClient.Where(x => x.Selected);
            bool selected = false;

            foreach (var selectedProduct in productList)
            {
                if (selectedProduct.Selected)
                {
                    selected = true;
                    break;
                }
            }

            ControlCheck = selected;
            OnSelectAll(false);
            ValidateSelectedAll();
            EnableCar();
        }

        private void ValidateSelectedAll()
        {
            var productList = SuggestedProductsResponse.ProductsClient.Where(x => x.Selected);

            if (productList.Count() < SuggestedProductsResponse.ProductsClient.Count())
            {
                ChkSelectAll.Checked = false;
            }
            else
            {
                ChkSelectAll.Checked = true;
            }
        }

        public void EnableCar(bool edit = false)
        {
            if (!updateNow)
            {
                LyAddCar.Enabled = ControlCheck;

                if (LyAddCar.Enabled && ValidateUpdateList())
                {
                    LyInsideAddCar.SetBackgroundResource(Resource.Drawable.button_yellow);
                }
                else
                {
                    LyInsideAddCar.SetBackgroundResource(Resource.Drawable.button_gray);
                }
            }
        }

        private async Task SelectOpcion()
        {
            if (Edit)
            {
                await JoinUpdateList(null, 0);
            }
            else
            {
                AddProductsToCar();
            }
        }

        private void EventEditListTrue()
        {
            TvAddCar.Text = GetString(Resource.String.str_save);
            IvAddCar.Visibility = ViewStates.Gone;
            TvListName.Visibility = ViewStates.Gone;
            EtNameList.Visibility = ViewStates.Visible;
            EtNameList.Text = SuggestedProductsResponse.Name;
            Edit = true;
            ControlCheck = true;
            LyEditMyList.Visibility = ViewStates.Gone;
            LyCancelEditMyList.Visibility = ViewStates.Visible;

            EventEdit = !EventEdit;
            EnableCar();
        }

        private void EventEditListFalse()
        {
            TvAddCar.Text = GetString(Resource.String.str_add);
            IvAddCar.Visibility = ViewStates.Visible;
            TvListName.Visibility = ViewStates.Visible;
            EtNameList.Visibility = ViewStates.Gone;
            Edit = false;
            ControlCheck = false;
            LyEditMyList.Visibility = ViewStates.Visible;
            LyCancelEditMyList.Visibility = ViewStates.Gone;

            EventEdit = !EventEdit;
            EnableCar();
        }

        private async Task JoinUpdateList(Product product, int position)
        {
            IList<ProductList> listProducts = SuggestedProductsResponse.ProductsClient.Where(x => x.Selected).ToArray();

            ShoppingList shoppingList = new ShoppingList
            {
                Id = SuggestedProductsResponse.ListId
            };

            UpdateList updateList = new UpdateList
            {
                ControlUpdateName = false,
                ControlUpdateQuantity = false,
                ResultUpdateName = true,
                ResultUpdateQuantity = true
            };

            if (listProducts != null && listProducts.Any())
            {
                shoppingList.Products = listProducts;
            }

            shoppingList.QuantityProducts = SuggestedProductsResponse.ProductsClient.Count().ToString();

            if (!SuggestedProductsResponse.Name.Equals(EtNameList.Text.ToString()))
            {
                shoppingList.Name = EtNameList.Text;
                updateList.ControlUpdateName = true;
                updateList.ResultUpdateName = await this.UpdateNameList(shoppingList);
                updateNow = true;
            }

            if (shoppingList.Products.Any())
            {
                updateList.ControlUpdateQuantity = true;
                var prices = shoppingList.Products.Select(x => x.Price = null).ToList();
                updateList.ResultUpdateQuantity = await this.UpdateQuantityItemList(shoppingList);
                updateNow = true;
            }

            await this.MessagesUpdateList(updateList);
        }

        private async Task MessagesUpdateList(UpdateList updateList)
        {
            if (updateList.ControlUpdateName || updateList.ControlUpdateQuantity)
            {
                if (updateList.ResultUpdateName && updateList.ResultUpdateQuantity)
                {
                    ConvertUtilities.MessageToast(AppMessages.UpdateList, this, true);
                    updateNow = false;
                    EventEditListFalse();
                    await this.GetList();
                }
                else
                {
                    if (!updateList.ResultUpdateName && !updateList.ResultUpdateQuantity)
                    {
                        ConvertUtilities.MessageToast(AppMessages.ApologyUpdateListErrorMessage, this, true);
                    }
                    else
                    {
                        string MessageError = null;

                        if (updateList.ControlUpdateName)
                        {
                            if (!updateList.ResultUpdateName)
                            {
                                MessageError = AppMessages.ApologyUpdateListNameErrorMessage;
                            }
                        }

                        if (updateList.ControlUpdateQuantity)
                        {
                            if (!updateList.ResultUpdateQuantity)
                            {
                                MessageError = AppMessages.ApologyUpdateListProductsErrorMessage;
                            }
                        }

                        ConvertUtilities.MessageToast(MessageError, this, true);
                    }
                }
            }
        }

        private async Task<bool> UpdateNameList(ShoppingList shoppingList)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                ResponseBase _ResponseBase = await ShoppingListModel.UpdateShoppingList(shoppingList);

                if (_ResponseBase.Result != null && _ResponseBase.Result.HasErrors &&
                    _ResponseBase.Result.Messages != null)
                {
                    HideProgressDialog();
                    return false;
                }
                else
                {
                    HideProgressDialog();
                    return true;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.UpdateList } };
                RegisterMessageExceptions(exception, properties);
                return false;
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task<bool> UpdateQuantityItemList(ShoppingList _ShoppingList)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                ResponseBase _ResponseBase = await ShoppingListModel.UpdateQuantityItemList(_ShoppingList);

                if (_ResponseBase.Result != null && _ResponseBase.Result.HasErrors && _ResponseBase.Result.Messages != null)
                {
                    HideProgressDialog();
                    return false;
                }
                else
                {
                    HideProgressDialog();
                    if (_ResponseBase.Result.Messages != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.RecommendProducstActivity, ConstantMethodName.AddToList } };
                RegisterMessageExceptions(exception, properties);
                return false;
            }
            finally
            {
                HideProgressDialog();
            }
        }

        protected struct UpdateList
        {
            public bool ControlUpdateName { get; set; }
            public bool ControlUpdateQuantity { get; set; }
            public bool ResultUpdateName { get; set; }
            public bool ResultUpdateQuantity { get; set; }
        }

        private void RegisterAddProductEvent(Product product)
        {
            FirebaseRegistrationEventsService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookRegistrationEventsService.Instance.AddProductToCart(product);
        }
        
        private bool ValidateUpdateList()
        {
            bool Validated = true;

            if (EventEdit)
            {
                IList<ProductList> listProducts = SuggestedProductsResponse.ProductsClient.Where(x => x.Selected).ToArray();

                ShoppingList shoppingList = new ShoppingList
                {
                    Id = SuggestedProductsResponse.ListId
                };

                if (listProducts != null && listProducts.Any())
                {
                    shoppingList.Products = listProducts;
                }

                if (SuggestedProductsResponse.Name.Equals(EtNameList.Text.ToString()) && !shoppingList.Products.Any())
                {
                    Validated = false;
                }
            }

            return Validated;
        }

        public class MyTextWatcher : Java.Lang.Object, ITextWatcher
        {

            RecommendProducstActivity _RecommendProducstActivity;

            public MyTextWatcher(RecommendProducstActivity _RecommendProducstActivity)
            {
                this._RecommendProducstActivity = _RecommendProducstActivity;
            }
            public void AfterTextChanged(IEditable s) { }
            public void BeforeTextChanged(Java.Lang.ICharSequence arg0, int start, int count, int after) { }
            public void OnTextChanged(Java.Lang.ICharSequence arg0, int start, int before, int count)
            {
                _RecommendProducstActivity.EnableCar();
            }
        }

        #endregion
    }
}