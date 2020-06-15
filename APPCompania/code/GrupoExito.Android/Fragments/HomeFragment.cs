using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.InStoreServices;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using static Android.Support.V4.Widget.NestedScrollView;

namespace GrupoExito.Android.Fragments
{
    public class HomeFragment : Fragment, IProducts, IHome, IOnScrollChangeListener, IBanner
    {
        #region Controls

        private RecyclerView RvDiscountProdcuts, rvCustomerProducts, RvBanner;
        private LinearLayoutManager DiscountsLayoutManager;
        private GridLayoutManager CustomerLayoutManager;
        private ProductAdapter DiscountProductAdapter, CustomerProductAdapter;
        private RelativeLayout RlMyCoupons, RlHomeProductsBar;
        private View ViewCarouselOppacty;
        private ImageView IvScrollRecyclerView, IvError;
        private TextView TvHomeUserName, TvTypeClientHome, TvError, TvSendTo, TvSendToAddress, TvHomeDiscountsLabel, TvSendToShimmer, TvSendToAddressShimmer;
        private NestedScrollView NsvHome;
        private Button BtnError;
        private RelativeLayout RlBody, RlError, RlContainer;
        private LinearLayout LyToastSendTo, LyShimmer, LyShimmerCustomer;
        private Button BtnMyCoupons;
        private ShimmerFrameLayout ShimmerFrameLayoutLeft;
        private ShimmerFrameLayout ShimmerFrameLayoutRight;
        private ShimmerFrameLayout ShimmerFrameLayoutCustomerLeft;
        private ShimmerFrameLayout ShimmerFrameLayoutCustomerRight;
        private ShimmerFrameLayout ShimmerToast;

        #endregion

        #region Properties

        private HomeModel _homeModel;
        private ProductCarModel _productCarModel;
        private ContentsModel _contentModel;
        private List<Product> discountProducts;
        private List<Product> customerProducts;
        private bool DiscountError, CustomerError;
        private string ServiceError;

        private Timer shimmerTimer;
        public Timer ShimmerTimer
        {
            get { return shimmerTimer; }
            set { shimmerTimer = value; }
        }

        #endregion

        public static HomeFragment NewInstance(String question, String answer)
        {
            HomeFragment homeFragment = new HomeFragment();
            return homeFragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.FragmentHome, container, false);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _homeModel = new HomeModel(new HomeService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            _contentModel = new ContentsModel(new ContentsService(DeviceManager.Instance));

            this.SetControlsProperties(view);
            this.SetFonts(view);            

            Activity.RunOnUiThread(async () =>
            {
                await GetBanner();
            });

            Activity.RunOnUiThread(() =>
            {
                ((MainActivity)Activity).CustomizeTabs();
                Animation animation = AnimationUtils.LoadAnimation(Activity, Resource.Animation.slide_from_left);
                animation.AnimationEnd += Animation_AnimationEnd;
                LyToastSendTo.StartAnimation(animation);
            });

            StartShimmerTimer();
        }

        private void Animation_AnimationEnd(object sender, Animation.AnimationEndEventArgs e)
        {
            LyToastSendTo.StartAnimation(AnimationUtils.LoadAnimation(Activity, Resource.Animation.shake));
        }

        public void OnAddPressed(Product product)
        {           
            var summary = _productCarModel.UpSertProduct(product, true);

            if (Activity != null)
            {
                ((BaseActivity)Activity).SetToolbarCarItems();
            }

            RegisterAddProductEvent(product);
        }

        public void OnSubstractPressed(Product product)
        {           
            _productCarModel.UpSertProduct(product, false);

            if (Activity != null)
            {
                ((BaseActivity)Activity).SetToolbarCarItems();
            }

            RegisterDeleteProduct(product);
        }

        public void OnAddProduct(Product product)
        {
            OnAddPressed(product);
        }

        public void OnProductClicked(Product product)
        {
            RegisterEvent(product);
            var productView = new Intent(Context, typeof(ProductActivity));
            productView.PutExtra(ConstantPreference.Product, JsonService.Serialize(product));
            StartActivity(productView);
        }

        public void OnAddToListClicked(Product product)
        {
            var productView = new Intent(Context, typeof(CreateNewListActivity));

            if (product.Quantity == 0)
            {
                product.Quantity = 1;
            }

            productView.PutExtra(ConstantPreference.Product, JsonService.Serialize(product));
            StartActivity(productView);
        }

        public async override void OnResume()
        {
            base.OnResume();
            this.SetUserName();
            this.SetTextAddress();
            ((BaseActivity)Activity).SetToolbarCarItems();
            ((BaseActivity)Activity).UpdateToolbarName();
            await ValidateLoadInformation();

            if (Activity != null)
            {
                ((MainActivity)Activity).SetHomeInterface(this);
                ((MainActivity)Activity).RegisterScreen(AnalyticsScreenView.Home);
            }
        }

        private async Task ValidateLoadInformation()
        {
            if (ParametersManager.ChangeProductQuantity ||
            ((discountProducts == null || !discountProducts.Any())
             && (customerProducts == null || !customerProducts.Any())))
            {
                ShimmerFrameLayoutLeft.StartShimmer();
                ShimmerFrameLayoutRight.StartShimmer();
                ShimmerFrameLayoutCustomerRight.StartShimmer();
                ShimmerFrameLayoutCustomerLeft.StartShimmer();
                LyShimmer.Visibility = ViewStates.Visible;
                LyShimmerCustomer.Visibility = ViewStates.Visible;
                RlContainer.Visibility = ViewStates.Gone;
                rvCustomerProducts.Visibility = ViewStates.Gone;
                ParametersManager.ChangeProductQuantity = false;
                await LoadProducts();
            }

            if (ParametersManager.ChangeAddress)
            {
                if (Activity != null)
                {
                    Activity.RunOnUiThread(async () =>
                    {                   
                        await GetBanner();
                        await ((MainActivity)Activity).UpdateProductsPrice();
                        ((MainActivity)Activity).SetToolbarCarItems();
                        ParametersManager.ChangeAddress = false;
                    });
                }
            }

            if (DiscountProductAdapter != null && CustomerProductAdapter != null)
            {
                DiscountProductAdapter.NotifyDataSetChanged();
                CustomerProductAdapter.NotifyDataSetChanged();
            }
        }

        private async Task LoadProducts()
        {
            await this.ProductsAppDiscounts();

            if (Activity != null)
            {
                Activity.RunOnUiThread(async () =>
                {
                    await CustomerProducts();
                });
            }
        }

        public void RefreshProductList(Product ActualProduct)
        {
            if (CustomerProductAdapter != null && customerProducts != null && customerProducts.Any())
            {
                var product = customerProducts.Where(x => x.Id.Equals(ActualProduct.Id)).FirstOrDefault();

                if (product != null)
                {
                    product.Quantity = ActualProduct.Quantity;
                }

                CustomerProductAdapter.NotifyDataSetChanged();
            }

            if (DiscountProductAdapter != null && discountProducts != null && discountProducts.Any())
            {
                var product = discountProducts.Where(x => x.Id.Equals(ActualProduct.Id)).FirstOrDefault();

                if (product != null)
                {
                    product.Quantity = ActualProduct.Quantity;
                }

                DiscountProductAdapter.NotifyDataSetChanged();
            }
        }

        public void OnScrollChange(NestedScrollView v, int scrollX, int scrollY, int oldScrollX, int oldScrollY)
        {
        }

        public async Task ProductsAppDiscounts()
        {
            if (Activity != null)
            {
                try
                {
                    SearchProductsParameters parameters = SetParameters();

                    var response = await _homeModel.ProductsAppDiscounts(parameters);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            DiscountError = true;
                            ServiceError = MessagesHelper.GetMessage(response.Result);
                            TvHomeDiscountsLabel.Visibility = ViewStates.Gone;
                            RvDiscountProdcuts.Visibility = ViewStates.Gone;
                            ShimmerFrameLayoutLeft.StopShimmer();
                            ShimmerFrameLayoutRight.StopShimmer();
                            LyShimmer.Visibility = ViewStates.Gone;

                            if (DiscountError && CustomerError)
                            {
                                ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                            }
                        });
                    }
                    else
                    {
                        DiscountError = false;
                        this.discountProducts = response.DiscountProducts.ToList();
                        this.DrawDiscountList();
                        ParametersManager.ChangeProductQuantity = false;

                        if (Activity != null)
                        {
                            ((BaseActivity)Activity).SetToolbarCarItems();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.HomeFragment, ConstantMethodName.ProductsAppDiscounts } };

                    if (Activity != null)
                    {
                        ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                    }

                    ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                }
                finally
                {
                    if (Activity != null)
                    {
                        ((BaseProductActivity)Activity).HideProgressDialog();
                    }
                }
            }
        }

        public async Task CustomerProducts()
        {
            if (Activity != null)
            {
                try
                {
                    SearchProductsParameters parameters = SetParameters();
                    var response = await _homeModel.CustomerProducts(parameters);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            CustomerError = true;
                            RlHomeProductsBar.Visibility = ViewStates.Gone;
                            rvCustomerProducts.Visibility = ViewStates.Gone;
                            ShimmerFrameLayoutCustomerLeft.StopShimmer();
                            ShimmerFrameLayoutCustomerRight.StopShimmer();
                            LyShimmerCustomer.Visibility = ViewStates.Gone;

                            if (CustomerError && DiscountError)
                            {
                                ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                            }
                        });
                    }
                    else
                    {
                        CustomerError = false;
                        ShowBodyLayout();
                        this.customerProducts = response.CustomerProducts.ToList();
                        this.DrawCustomerList();
                        ParametersManager.ChangeProductQuantity = false;

                        if (Activity != null)
                        {
                            ((BaseActivity)Activity).SetToolbarCarItems();
                        }
                    }
                }
                catch (Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.HomeFragment, ConstantMethodName.CustomerProducts } };

                    if (Activity != null)
                    {
                        ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                    }

                    ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                }
            }
        }

        private async Task GetBanner()
        {
            if (Activity != null)
            {
                try
                {
                    var response = await _contentModel.GetContentHome();


                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        Activity.RunOnUiThread(() =>
                        {
                            RvBanner.Visibility = ViewStates.Gone;
                        });

                    }
                    else
                    {
                        DrawBanner(response.Images);
                    }

                }
                catch (Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.HomeFragment, ConstantMethodName.CustomerProducts } };

                    if (Activity != null)
                    {
                        ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                    }

                    ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                }
            }
        }

        private SearchProductsParameters SetParameters()
        {
            SearchProductsParameters parameters = new SearchProductsParameters()
            {
                DependencyId = ParametersManager.UserContext.DependencyId,
                Size = ParametersManager.HomeSize,
                From = ParametersManager.HomeFrom,
                OrderType = ParametersManager.OrderType,
                OrderBy = ParametersManager.OrderBy
            };

            return parameters;
        }


        private void SetFonts(View view)
        {
            view.FindViewById<TextView>(Resource.Id.tvHomeDiscountsLabel).SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            view.FindViewById<TextView>(Resource.Id.tvHomeBar).SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
            view.FindViewById<Button>(Resource.Id.btnMyCoupons).SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvHomeUserName.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSendTo.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSendToAddress.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            BtnError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }
        
        private void StartShimmerTimer()
        {
            if (ShimmerTimer != null && ShimmerTimer.Enabled)
            {
                ShimmerTimer.Stop();
                ShimmerTimer.Elapsed -= new ElapsedEventHandler(AddShimmerEvent);
            }
            else
            {
                ShimmerTimer = new Timer();
            }

            ShimmerTimer.Elapsed += new ElapsedEventHandler(AddShimmerEvent);
            ShimmerTimer.Interval = 3000;
            ShimmerTimer.Enabled = true;
        }

        private void AddShimmerEvent(object source, ElapsedEventArgs e)
        {
            if (Activity != null)
            {
                Activity.RunOnUiThread(() =>
                {
                    ShimmerToast.StopShimmer();
                    ShimmerToast.Visibility = ViewStates.Gone;
                    TvSendTo.Visibility = ViewStates.Visible;
                    TvSendToAddress.Visibility = ViewStates.Visible;
                });
            }
        }

        private void DrawDiscountList()
        {
            if (Activity != null)
            {
                if (discountProducts.Any())
                {
                    DiscountProductAdapter = new ProductAdapter(this.discountProducts, Activity, this, Activity.WindowManager);
                    Activity.RunOnUiThread(() =>
                    {
                        TvHomeDiscountsLabel.Visibility = ViewStates.Gone;
                        RvDiscountProdcuts.Visibility = ViewStates.Visible;
                        RvDiscountProdcuts.SetAdapter(DiscountProductAdapter);
                        RvDiscountProdcuts.Measure(View.MeasureSpec.MakeMeasureSpec(RvDiscountProdcuts.Width, MeasureSpecMode.Exactly),
                        View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
                        int width = RvDiscountProdcuts.MeasuredWidth;
                        int height = RvDiscountProdcuts.MeasuredHeight;
                        RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(ViewCarouselOppacty.Width, height);
                        layoutParams.AddRule(LayoutRules.AlignParentEnd);
                        IvScrollRecyclerView.Click += delegate
                        {
                            RvDiscountProdcuts.SmoothScrollToPosition(DiscountsLayoutManager.FindLastVisibleItemPosition() + 1);
                        };

                        IvScrollRecyclerView.Visibility = this.discountProducts.Count() > 2 ? ViewStates.Visible : ViewStates.Gone;

                        ShimmerFrameLayoutLeft.StopShimmer();
                        ShimmerFrameLayoutRight.StopShimmer();
                        LyShimmer.Visibility = ViewStates.Gone;
                        RlContainer.Visibility = ViewStates.Visible;
                        RlContainer.StartAnimation(AnimationUtils.LoadAnimation(Activity, Resource.Animation.slide_from_left));
                    });
                }
                else
                {
                    IvScrollRecyclerView.Visibility = ViewStates.Gone;
                    RvDiscountProdcuts.Visibility = ViewStates.Gone;
                    TvHomeDiscountsLabel.Visibility = ViewStates.Gone;
                    ShimmerFrameLayoutLeft.StopShimmer();
                    ShimmerFrameLayoutRight.StopShimmer();
                    LyShimmer.Visibility = ViewStates.Gone;
                }
            }
        }

        private void DrawCustomerList()
        {
            RegisterCustomerProductImpressionEvent();

            if (Activity != null)
            {
                CustomerProductAdapter = new ProductAdapter(this.customerProducts, Activity, this, null);
                Activity.RunOnUiThread(() =>
                {
                    rvCustomerProducts.SetAdapter(CustomerProductAdapter);
                    ShimmerFrameLayoutCustomerLeft.StopShimmer();
                    ShimmerFrameLayoutCustomerRight.StopShimmer();
                    LyShimmerCustomer.Visibility = ViewStates.Gone;
                    rvCustomerProducts.Visibility = ViewStates.Visible;
                    rvCustomerProducts.StartAnimation(AnimationUtils.LoadAnimation(Activity, Resource.Animation.slide_from_left));
                });
            }
        }

        private void DrawBanner(IList<BannerPromotion> images)
        {
            if (Activity != null)
            {
                var BannerAdapter = new BannerAdapter(images, Activity, this);
                Activity.RunOnUiThread(() =>
                {
                    RvBanner.SetAdapter(BannerAdapter);
                    RvBanner.StartAnimation(AnimationUtils.LoadAnimation(Activity, Resource.Animation.slide_from_left));
                });
            }
        }

        private void SetControlsProperties(View view)
        {
            RlError = view.FindViewById<RelativeLayout>(Resource.Id.layoutError);
            RlBody = view.FindViewById<RelativeLayout>(Resource.Id.rlBody);
            TvError = RlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = RlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = RlError.FindViewById<ImageView>(Resource.Id.ivError);

            RvDiscountProdcuts = view.FindViewById<RecyclerView>(Resource.Id.rvDiscountProducts);
            NsvHome = view.FindViewById<NestedScrollView>(Resource.Id.nsvHome);
            LyToastSendTo = view.FindViewById<LinearLayout>(Resource.Id.toastSendTo);
            LyShimmer = view.FindViewById<LinearLayout>(Resource.Id.lyShimmer);
            LyShimmerCustomer = view.FindViewById<LinearLayout>(Resource.Id.lyShimmerCustomer);
            rvCustomerProducts = view.FindViewById<RecyclerView>(Resource.Id.rvFavoriteProducts);
            RvBanner = view.FindViewById<RecyclerView>(Resource.Id.rvBanner);
            RlMyCoupons = view.FindViewById<RelativeLayout>(Resource.Id.rvMyCoupons);
            RlHomeProductsBar = view.FindViewById<RelativeLayout>(Resource.Id.rvHomeProductsBar);
            RlContainer = view.FindViewById<RelativeLayout>(Resource.Id.rlContainer);
            ViewCarouselOppacty = view.FindViewById(Resource.Id.viewCarouselOppacity);
            IvScrollRecyclerView = view.FindViewById<ImageView>(Resource.Id.ivScrollRv);
            TvHomeUserName = view.FindViewById<TextView>(Resource.Id.tvHomeUserName);
            TvTypeClientHome = view.FindViewById<TextView>(Resource.Id.tvTypeClientHome);
            TvSendTo = view.FindViewById<TextView>(Resource.Id.tvSendTo);
            TvSendToShimmer = view.FindViewById<TextView>(Resource.Id.tvSendToShimmer);
            TvSendToAddress = view.FindViewById<TextView>(Resource.Id.tvSendToAddress);
            TvSendToAddressShimmer = view.FindViewById<TextView>(Resource.Id.tvSendToAddressShimmer);
            TvHomeDiscountsLabel = view.FindViewById<TextView>(Resource.Id.tvHomeDiscountsLabel);
            ShimmerFrameLayoutLeft = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container_left);
            ShimmerFrameLayoutCustomerLeft = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container_customer_left);
            ShimmerFrameLayoutRight = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container_right);
            ShimmerFrameLayoutCustomerRight = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container_customer_right);
            ShimmerToast = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmerToast);

            TvHomeUserName.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvTypeClientHome.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvSendTo.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSendToShimmer.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            BtnError.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvSendToAddress.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSendToAddressShimmer.SetTypeface(FontManager.Instance.GetTypeFace(Activity, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            DiscountsLayoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            RvDiscountProdcuts.HasFixedSize = true;
            RvDiscountProdcuts.SetLayoutManager(DiscountsLayoutManager);
            CustomerLayoutManager = new GridLayoutManager(Activity, 2, GridLayoutManager.Vertical, false)
            {
                AutoMeasureEnabled = true
            };
            rvCustomerProducts.NestedScrollingEnabled = false;
            rvCustomerProducts.HasFixedSize = false;
            rvCustomerProducts.SetLayoutManager(CustomerLayoutManager);
            NsvHome.SetOnScrollChangeListener(this);
            LyToastSendTo.Click += delegate { DialogBeforeChangeAddress(); };

            LinearLayoutManager bannerManager = new LinearLayoutManager(Activity, LinearLayoutManager.Horizontal, false);
            RvBanner.HasFixedSize = true;
            RvBanner.SetLayoutManager(bannerManager);
            view.FindViewById<Button>(Resource.Id.btnMyCoupons).Click += delegate { GoToMyDiscounts(); };
        }

        private void GoToMyDiscounts()
        {
            Intent intent = new Intent(Activity, typeof(MyDiscountsActivity));
            StartActivity(intent);
        }

        public void CallShowAddressDialog()
        {
            Intent intent = new Intent(Activity, typeof(MainActivity));
            ((MainActivity)Activity).ShowAddressesDialog();
        }

        private void DialogBeforeChangeAddress()
        {
            ((MainActivity)Activity).OnBoxMessageTouched(AppMessages.MessageChangeAdress, AppMessages.MessagebtnChangeAdress);
        }

        private void SetTextAddress()
        {
            if (ParametersManager.UserContext != null && (ParametersManager.UserContext.Address != null || ParametersManager.UserContext.Store != null))
            {
                
                TvSendToAddress.Text = ParametersManager.UserContext.Address != null ?
                            ParametersManager.UserContext.Address.City + ", " + ParametersManager.UserContext.Address.AddressComplete :
                               ParametersManager.UserContext.Store.City + ", " + ParametersManager.UserContext.Store.Name;

                TvSendToAddressShimmer.Text = ParametersManager.UserContext.Address != null ?
                              ParametersManager.UserContext.Address.AddressComplete : ParametersManager.UserContext.Store.Name;

                TvSendTo.Text = ParametersManager.UserContext.Address != null ? AppMessages.GetAddressText : AppMessages.GetStoreText;
                TvSendToShimmer.Text = ParametersManager.UserContext.Address != null ? AppMessages.GetAddressText : AppMessages.GetStoreText;
                LyToastSendTo.Visibility = ViewStates.Visible;
            }
        }

        private void ShowBodyLayout()
        {
            RlBody.Visibility = ViewStates.Visible;
            RlError.Visibility = ViewStates.Gone;
        }

        private void ShowErrorLayout(string message, int resource = 0)
        {
            RlBody.Visibility = ViewStates.Gone;
            RlError.Visibility = ViewStates.Visible;
            TvError.Text = message;

            if (resource != 0)
            {
                IvError.SetImageResource(resource);
            }

            if (!BtnError.HasOnClickListeners)
            {
                BtnError.Click += async delegate { await LoadProducts(); };
            }
        }

        private void SetUserName()
        {
            if (ParametersManager.UserContext != null)
            {
                TvHomeUserName.Text = AppMessages.Hello + " " + StringFormat.Capitalize(StringFormat.SplitName(ParametersManager.UserContext.FirstName)) + "!";

                CustomText(TvHomeUserName.Text);

                if (AppServiceConfiguration.SiteId.Equals("carulla"))
                {
                    if (ParametersManager.UserContext.UserType != null)
                    {
                        TvTypeClientHome.Text = ParametersManager.UserContext.UserType.Name;
                        TvTypeClientHome.Visibility = ViewStates.Visible;
                    }
                    else
                    {
                        TvTypeClientHome.Text = string.Empty;
                        TvTypeClientHome.Visibility = ViewStates.Gone;
                    }
                }
            }
        }

        public void OnSelectBannerItem(BannerPromotion bannerItem)
        {
            try
            {
                if (!string.IsNullOrEmpty(bannerItem.ActionDroid) && !bannerItem.ActionDroid.Equals("NotAction"))
                {
                    ParametersManager.ParameterActionBanner = !string.IsNullOrEmpty(bannerItem.ParameterAction) ? JsonService.Deserialize<BannerParameter>(bannerItem.ParameterAction) : new BannerParameter();
                    Intent intent = new Intent(Activity, Java.Lang.Class.ForName(bannerItem.ActionDroid));
                    StartActivity(intent);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.HomeFragment, ConstantMethodName.OnSelectBannerItem },
                    { "Action", bannerItem.ActionDroid } };

                if (Activity != null)
                {
                    ((BaseProductActivity)Activity).RegisterMessageExceptions(exception, properties);
                }

                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
            }
        }

        private void CustomText(string strUserName)
        {
            if (!string.IsNullOrEmpty(strUserName))
            {
                SpannableStringBuilder strHomeUserName = new SpannableStringBuilder(strUserName);

                if (strHomeUserName != null)
                {
                    strHomeUserName.SetSpan(new StyleSpan(TypefaceStyle.Bold), 7, strUserName.Length, SpanTypes.ExclusiveExclusive);
                    TvHomeUserName.TextFormatted = strHomeUserName;
                }
            }
        }

        public void RetryPermmision(bool retry)
        {
        }

        public void GoToLobby()
        {
            ((MainActivity)Activity).OnBackPressed();
        }

        #region Analytic

        private void RegisterCustomerProductImpressionEvent()
        {
            FirebaseRegistrationEventsService.Instance.ProductImpression(customerProducts, AnalyticsParameter.CustomerProducts);
        }

        private void RegisterDiscountProductImpressionEvent()
        {
            FirebaseRegistrationEventsService.Instance.ProductImpression(discountProducts, AnalyticsParameter.DiscountProducts);
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

        public void RegisterDeleteProduct(Product product)
        {
            FirebaseRegistrationEventsService.Instance.DeleteProductFromCart(product, product.CategoryName);
        }


        #endregion
    }
}