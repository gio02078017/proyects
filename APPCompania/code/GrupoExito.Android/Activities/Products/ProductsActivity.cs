using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Android.Interfaces;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Android.Services;
using GrupoExito.Entities.Constants.Analytic;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Lista de productos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductsActivity : BaseProductActivity, IProducts, View.IOnScrollChangeListener
    {
        #region Controls

        private RecyclerView RvProducts;
        private View viewRvProducts;
        private TextView TvTotalProducts;
        private TextView TvProductCategory;
        private TextView TvProductFilters;
        private TextView TvProductOrder;
        private TextView TvSearcher;
        private TextView TvLoadingMore;
        private ImageView IvProductCategory;
        private ImageView IvClose;
        private ImageView IvProductFilter;
        private ImageView IvProductOrder;
        private GridLayoutManager ProductsLayoutManager;
        private ProductAdapter ProductsAdapter;
        private Category Category;
        private NestedScrollView NsvShimmer;
        private LinearLayout LyProductFilter;
        private LinearLayout LyProductOrder;
        private LinearLayout LyLoader;
        private LinearLayout LyTitleCategory;
        private ShimmerFrameLayout ShimmerFrameLayoutLeft;
        private ShimmerFrameLayout ShimmerFrameLayoutRight;
        private FabToolbar FabToolbar;
        private FloatingActionButton FabProducts;
        private int ActualItemPosition;
        private LinearLayout LyErrorInformation;
        private ImageView IvErrorInformation;
        private TextView TvMessageErrorInformation;
        private LinearLayout LyLookForOtherOnce;
        private TextView TvLookForOtherOnce;

        #endregion

        #region properties

        private int VisibleItemCount, PastVisibleItems, TotalItemCount, totalProducts;
        private bool IsLoadingProducts;
        private ProductsModel _productsModel;
        private ProductCarModel _productCarModel;

        #endregion

        public void OnProductClicked(Product product)
        {
            RegisterEvent(product);
            var productView = new Intent(this, typeof(ProductActivity));
            productView.PutExtra(ConstantPreference.Product, JsonService.Serialize(product));
            StartActivity(productView);
        }

        public void OnAddToListClicked(Product product)
        {
            var productView = new Intent(this, typeof(CreateNewListActivity));
            if (product.Quantity == 0)
            {
                product.Quantity = 1;
            }
            productView.PutExtra(ConstantPreference.Product, JsonService.Serialize(product));
            StartActivity(productView);
        }

        public override void OnBackPressed()
        {
            this.ClearFiltersResult();
            ParametersManager.FromProductsActivity = true;
            base.OnBackPressed();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        public void OnAddPressed(Product product)
        {          
            var summary = _productCarModel.UpSertProduct(product, true);
            SetToolbarCarItems();
            RegisterAddProductEvent(product);
        }

        public void OnSubstractPressed(Product product)
        {          
            var summary = _productCarModel.UpSertProduct(product, false);
            SetToolbarCarItems();
            RegisterDeleteProduct(product);
        }

        public void OnAddProduct(Product product)
        {
            OnAddPressed(product);
        }

        protected override void RefreshProductList(Product ActualProductId)
        {
            Product product = ParametersManager.Products.Where(x => x.Id.Equals(ActualProductId)).FirstOrDefault();

            if (product != null)
            {
                ProductsAdapter.NotifyItemChanged(ParametersManager.Products.IndexOf(product));
            }

            try
            {
                ProductsLayoutManager.ScrollToPositionWithOffset(ActualItemPosition, 30);
            }
            catch (System.Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductsActivity, ConstantMethodName.RefreshProductList } };
                ShowAndRegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }

            base.RefreshProductList(ActualProductId);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Runtime.GetRuntime().Gc();
        }

        protected override void EventError()
        {
            base.EventError();
            ProductsAdapter = null;
            LoadProductsFromCategories();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _productsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            SetContentView(Resource.Layout.ActivityProducts);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();

            var root = FindViewById<LinearLayout>(Resource.Id.rootLayout);
            root.RequestFocus();            
        }

        protected override void OnResume()
        {
            base.OnResume();

            SetToolbarCarItems();

            if (ParametersManager.ChangeAddress)
            {
                RunOnUiThread(async () =>
                {
                    await UpdateProductsPrice();
                    ParametersManager.ChangeAddress = false;
                });
            }

            if (ParametersManager.FromProductsActivity)
            {
                ParametersManager.FromProductsActivity = false;
                OnBackPressed();
            }
            else
            {
                ShimmerFrameLayoutLeft.StartShimmer();
                ShimmerFrameLayoutRight.StartShimmer();
                NsvShimmer.Visibility = ViewStates.Visible;
                FabProducts.Visibility = ViewStates.Gone;

                if (ParametersManager.ChangeProductQuantityFromDetail)
                {
                    LoadProductsFromCategories();
                }
                else
                {
                    if (ParametersManager.ChangeProductQuantity ||
                       (ParametersManager.Products != null && ParametersManager.Products.Any()))
                    {
                        SetToolbarCarItems();
                        totalProducts = ParametersManager.Products.Count;
                        this.DrawProducts(true, ParametersManager.Products);
                    }
                    else
                    {
                        this.LoadProductsFromCategories();
                    }
                }
            }
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ProdutsxCategory, typeof(ProductsActivity).Name);
        }

        private void LoadProductsFromCategories()
        {
            if (Intent.Extras != null && !string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Category)))
            {
                Category = JsonService.Deserialize<Category>(Intent.Extras.GetString(ConstantPreference.Category));
            }

            if (ParametersManager.ParameterActionBanner != null && !string.IsNullOrEmpty(ParametersManager.ParameterActionBanner.Id))
            {
                Category = new Category
                {
                    Name = ParametersManager.ParameterActionBanner.Name,
                    IconCategoryGray = ParametersManager.ParameterActionBanner.URLImage,
                    Id = ParametersManager.ParameterActionBanner.Id
                };

                ParametersManager.ParameterActionBanner = null;
            }

            if (Category != null)
            {
                TvProductCategory.Text = Category.Name;
                Glide.With(ApplicationContext).Load(Category.IconCategoryGray).Into(IvProductCategory);
                ParametersManager.Products = new List<Product>();

                RunOnUiThread(async () =>
                {
                    await GetProductsCategories();
                });
            }
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetSearcher(FindViewById<LinearLayout>(Resource.Id.searcher), true);
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
             FindViewById<LinearLayout>(Resource.Id.lyBodyOutside), AppMessages.NotFoundProductAvailable, AppMessages.GenericBackAction);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<LinearLayout>(Resource.Id.lyBodyOutside));
            IvProductCategory = FindViewById<ImageView>(Resource.Id.ivProductCategory);
            IvProductFilter = FindViewById<ImageView>(Resource.Id.ivProductFilter);
            IvProductOrder = FindViewById<ImageView>(Resource.Id.ivProductOrder);
            TvProductCategory = FindViewById<TextView>(Resource.Id.tvProductCategory);
            TvProductFilters = FindViewById<TextView>(Resource.Id.tvProductFilters);
            TvProductOrder = FindViewById<TextView>(Resource.Id.tvProductOrder);
            TvSearcher = FindViewById<TextView>(Resource.Id.tvSearcher);
            TvToolbarPrice = FindViewById<TextView>(Resource.Id.tvToolbarPrice);
            TvLoadingMore = FindViewById<TextView>(Resource.Id.tvLoadingMore);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            IvClose = FindViewById<ImageView>(Resource.Id.ivClose);
            LyProductFilter = FindViewById<LinearLayout>(Resource.Id.lyProductFilter);
            LyTitleCategory = FindViewById<LinearLayout>(Resource.Id.lyTitleCategory);
            ShimmerFrameLayoutLeft = FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container_left);
            ShimmerFrameLayoutRight = FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container_right);
            LyLoader = FindViewById<LinearLayout>(Resource.Id.lyLoader);
            LyProductOrder = FindViewById<LinearLayout>(Resource.Id.lyProductOrder);
            TvTotalProducts = FindViewById<TextView>(Resource.Id.tvTotalProducts);
            NsvShimmer = FindViewById<NestedScrollView>(Resource.Id.nsvShimmer);
            FabToolbar = FindViewById<FabToolbar>(Resource.Id.fabtoolbar);
            FabProducts = FindViewById<FloatingActionButton>(Resource.Id.fabtoolbar_fab);
            RvProducts = FindViewById<RecyclerView>(Resource.Id.rvProducts);
            viewRvProducts = FindViewById<View>(Resource.Id.viewRvProducts);
            LyErrorInformation = FindViewById<LinearLayout>(Resource.Id.lyErrorInformation);
            IvErrorInformation = FindViewById<ImageView>(Resource.Id.ivErrorInformatiuon);
            TvMessageErrorInformation = FindViewById<TextView>(Resource.Id.tvMessageErrorInformation);
            LyLookForOtherOnce = FindViewById<LinearLayout>(Resource.Id.lyLookForOtherOnce);
            TvLookForOtherOnce = FindViewById<TextView>(Resource.Id.tvLookForOtherOnce);
            ProductsLayoutManager = new GridLayoutManager(this, 2, GridLayoutManager.Vertical, false);

            RvProducts.HasFixedSize = true;

            var onScrollListener = new CustomScrollListener(ProductsLayoutManager);
            onScrollListener.LoadMoreEvent += (object sender, EventArgs e) =>
            {
                if (!IsLoadingProducts)
                {
                    IsLoadingProducts = true;
                    LoadMoreItems();
                }
            };

            if (Intent.Extras != null && Intent.Extras.GetBoolean(ConstantPreference.FromSearch))
            {
                LyTitleCategory.Visibility = ViewStates.Gone;
            }

            RvProducts.AddOnScrollListener(onScrollListener);

            RvProducts.SetLayoutManager(ProductsLayoutManager);
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            TvProductFilters.Click += delegate { CallFilterActivity(); };
            IvProductFilter.Click += delegate { CallFilterActivity(); };
            TvProductOrder.Click += delegate { CallFilterActivity(false); };
            IvProductOrder.Click += delegate { CallFilterActivity(false); };

            FabProducts.Visibility = !string.IsNullOrEmpty(ParametersManager.UserQuery) ? ViewStates.Gone : FabProducts.Visibility = ViewStates.Visible;
            FabProducts.Click += delegate { DefineFabVisibility(); };
            IvClose.Click += delegate { DefineFabVisibility(); };
            LyLookForOtherOnce.Click += delegate { ItLookForOtherOnce(); };
        }

        private void DefineFabVisibility()
        {
            if (FabToolbar.IsFloatingActionButton())
            {
                FabToolbar.Show();
            }
            else
            {
                FabToolbar.Hide();
            }
        }

        private void EditFonts()
        {
            TvProductCategory.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductFilters.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductOrder.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvSearcher.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTotalProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvLoadingMore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvLoadingMore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvMessageErrorInformation.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvLookForOtherOnce.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private async Task GetProductsCategories()
        {
            try
            {
                if (int.Parse(ParametersManager.From) == 0)
                {
                    ParametersManager.CategoryNames = new List<string>();
                    ParametersManager.BrandNames = new List<string>();
                }

                SearchProductsParameters parameters = GetSearchProductsParameters();
                var response = await _productsModel.GetProducts(parameters);
                this.ValidateResponseProducts(response, parameters);
                SetToolbarCarItems();
            }
            catch (System.Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductsActivity, ConstantMethodName.GetProductsCategories } };
                ShowAndRegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private SearchProductsParameters GetSearchProductsParameters()
        {
            SearchProductsParameters parameters = parameters = new SearchProductsParameters
            {
                DependencyId = ParametersManager.UserContext.DependencyId,
                CategoriesNames = ParametersManager.CategoryNames,
                Brands = ParametersManager.BrandNames,
                Size = ParametersManager.Size,
                From = ParametersManager.From,
                OrderBy = ParametersManager.OrderBy,
                OrderType = ParametersManager.OrderType,
                UserQuery = ParametersManager.UserQuery,
                CategoryId = Category != null ? Category.Id : string.Empty
            };

            return parameters;
        }

        private void ValidateResponseProducts(ProductsResponse response, SearchProductsParameters parameters)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                var errorCode = (EnumErrorCode)System.Enum.Parse(typeof(EnumErrorCode), response.Result.Messages.First().Code);

                if (errorCode != EnumErrorCode.SearchNotProductsFound)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    if (totalProducts <= 0)
                    {
                        ShowNoInfoLayout();
                    }
                }

                LyLoader.Visibility = ViewStates.Gone;
            }
            else
            {
                totalProducts = response.TotalProductsSearch;

                if (int.Parse(ParametersManager.From) == 0)
                {
                    ParametersManager.Categories = response.Categories;
                    ParametersManager.Brands = response.Brands;
                }

                DrawProducts(false, response.Products);
                ShowBodyLayout();
            }

            HideProgressDialog();
        }

        private void CallFilterActivity(bool? filterView = true)
        {
            var productsFilterView = new Intent(this, typeof(ProductsFilterActivity));
            productsFilterView.PutExtra(ConstantPreference.Category, JsonService.Serialize(Category));
            productsFilterView.PutExtra(ConstantPreference.FilterView, JsonService.Serialize(filterView));
            StartActivity(productsFilterView);

            if (!FabToolbar.IsFloatingActionButton())
            {
                FabToolbar.Hide();
            }
        }

        private void DrawProducts(bool isFilter, List<Product> products)
        {
            if (isFilter || ProductsAdapter == null || ParametersManager.ChangeProductQuantityFromDetail)
            {
                if (!isFilter)
                {
                    ParametersManager.Products = new List<Product>();
                    ParametersManager.Products = products;
                }

                if (ParametersManager.Products.Any())
                {
                    ParametersManager.ChangeProductQuantityFromDetail = false;
                    ProductsAdapter = new ProductAdapter(ParametersManager.Products, this, this, null);

                    this.RunOnUiThread(() =>
                    {
                        TvTotalProducts.Text = GetString(Resource.String.str_we_find);
                        RvProducts.SetAdapter(ProductsAdapter);
                    });
                }
                else
                {
                    LyErrorInformation.Visibility = ViewStates.Visible;
                    RvProducts.Visibility = ViewStates.Gone;
                    viewRvProducts.Visibility = ViewStates.Gone;
                    RegisterSearchNotExits();
                }

                IsLoadingProducts = false;
            }
            else
            {
                ProductsAdapter.Update(products);
                IsLoadingProducts = false;

                try
                {
                    ProductsLayoutManager.ScrollToPositionWithOffset(ActualItemPosition - 2, 30);
                }
                catch (System.Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductsActivity, ConstantMethodName.DrawProducts } };
                    ShowAndRegisterMessageExceptions(exception, properties);
                    DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
                }

                ActualItemPosition = 0;
                LyLoader.Visibility = ViewStates.Gone;
            }

            RegisterProductImpressionEvent(products);

            ShimmerFrameLayoutLeft.StopShimmer();
            ShimmerFrameLayoutRight.StopShimmer();
            NsvShimmer.Visibility = ViewStates.Gone;
            FabProducts.Visibility = ViewStates.Visible;
        }

        private void ClearFiltersResult()
        {
            ParametersManager.From = "0";
            ParametersManager.UserQuery = string.Empty;
            ParametersManager.OrderBy = ConstOrder.Relevance;

            if (ParametersManager.Products != null)
            {
                ParametersManager.Products = new List<Product>();
                ParametersManager.Categories = new List<ProductFilter>();
                ParametersManager.Brands = new List<ProductFilter>();
            }
        }

        public void OnScrollChange(View v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
            var nestedView = (NestedScrollView)v;

            if (nestedView.GetChildAt(nestedView.ChildCount - 1) != null)
            {
                if ((scrollY >= (nestedView.GetChildAt(nestedView.ChildCount - 1).MeasuredHeight - v.MeasuredHeight)) &&
                    scrollY > oldScrollY)
                {
                    VisibleItemCount = ProductsLayoutManager.ChildCount;
                    TotalItemCount = ProductsLayoutManager.ItemCount;
                    PastVisibleItems = ProductsLayoutManager.FindFirstVisibleItemPosition();

                    if (!IsLoadingProducts && (VisibleItemCount + PastVisibleItems) >= TotalItemCount)
                    {
                        IsLoadingProducts = true;
                        LoadMoreItems();
                    }
                }
            }
        }

        private void LoadMoreItems()
        {
            LyLoader.Visibility = ViewStates.Visible;
            ParametersManager.From = (int.Parse(ParametersManager.From) + int.Parse(ParametersManager.Size)).ToString();
            ActualItemPosition = int.Parse(ParametersManager.From);

            RunOnUiThread(async () =>
            {
                await GetProductsCategories();
            });
        }

        private void ItLookForOtherOnce()
        {
            LyErrorInformation.Visibility = ViewStates.Gone;
            RvProducts.Visibility = ViewStates.Visible;
            viewRvProducts.Visibility = ViewStates.Visible;

            CallSearcherActivity(true);
        }

        private void RegisterEvent(Product product)
        {
            FirebaseRegistrationEventsService.Instance.ProductClic(product, product.CategoryName);
            FirebaseRegistrationEventsService.Instance.ProductDetail(product, product.CategoryName);
            FacebookRegistrationEventsService.Instance.ViewedContent(product);
        }

        private void RegisterAddProductEvent(Product product)
        {
            FirebaseRegistrationEventsService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookRegistrationEventsService.Instance.AddProductToCart(product);
        }

        private void RegisterProductImpressionEvent(List<Product> products)
        {
            string categoryName = Category != null? Category.Name : string.Empty;
            FirebaseRegistrationEventsService.Instance.ProductImpression(products, categoryName);
        }

        public void RegisterDeleteProduct(Product product)
        {
            FirebaseRegistrationEventsService.Instance.DeleteProductFromCart(product, product.CategoryName);
        }

        private void RegisterSearchNotExits()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.SearchNotExits, typeof(ProductsActivity).Name);
        }
    }
}