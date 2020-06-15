using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.Payments;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Orders
{
    [Activity(Label = "Detalle Pedido Historico", ScreenOrientation = ScreenOrientation.Portrait)]
    public class HistoricalOrderDetailActivity : BaseOrderActivity, IHistoricalProducts
    {
        #region Controls

        private RecyclerView RvHistoricalDetailOrder;
        private HistoricalProductAdapter historicalProductAdapter;
        private LinearLayoutManager linerLayoutManager;
        private TextView TvIdOrder;
        private TextView TvDate;
        private TextView TvQuantityProducts;
        private LinearLayout LyAddList;
        private LinearLayout LyAddCar;
        private ConstraintLayout LyInsideAddCar;
        private CheckBox CkSelectAll;

        #endregion

        #region Properties

        private OrderDetailResponse OrderDetailResponse;
        private ProductCarModel _productCarModel;

        private bool IsAddProducts { get; set; }

        #endregion

        #region Public Methods

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        public void OnCheck(Product product, bool action)
        {
            product.Selected = action;
            historicalProductAdapter.NotifyDataSetChanged();
            this.ValidateCheck();
        }

        public void ValidateCheck()
        {
            var productList = OrderDetailResponse.Products.Where(x => x.Selected);
            bool selected = false;

            foreach (var selectedProduct in productList)
            {
                if (selectedProduct.Selected)
                {
                    selected = true;
                    break;
                }
            }

            IsAddProducts = selected;
            ValidateSelectedAll();
            EnableCar();
        }

        private void ValidateSelectedAll()
        {
            var productList = OrderDetailResponse.Products.Where(x => x.Selected);

            if (productList.Count() < OrderDetailResponse.Products.Count())
            {
                CkSelectAll.Checked = false;
            }
            else
            {
                CkSelectAll.Checked = true;
            }
        }

        public void EnableCar()
        {
            LyAddCar.Enabled = IsAddProducts;

            if (LyAddCar.Enabled)
            {
                LyInsideAddCar.SetBackgroundResource(Resource.Drawable.button_yellow);
            }
            else
            {
                LyInsideAddCar.SetBackgroundResource(Resource.Drawable.button_gray);

                if (CkSelectAll.Checked)
                {
                    CkSelectAll.Checked = false;
                }
            }
        }

        protected override void EventError()
        {
            base.EventError();

            RunOnUiThread(async () =>
            {
                await DrawProducts();
            });
        }

        protected override void GoToSummary()
        {
            Intent intent = new Intent(this, typeof(SummaryActivity));
            StartActivity(intent);
            Finish();
        }

        #endregion

        #region Protected Methods

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            this.OnBackPressed();
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityHistoricalOrderDetail);
            OrderModel = new OrderModel(new OrderService(DeviceManager.Instance));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            this.HideItemsCarToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            await this.DrawProducts();
        }

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            base.HandlerOkButton(sender, e);

            if (IsAddProducts)
            {
                IsAddProducts = false;
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            }
        }

        #endregion

        #region Private Methods

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
             FindViewById<RelativeLayout>(Resource.Id.rlHistoricalOrderDetail), AppMessages.NotSearcherResultsTitle, AppMessages.NotSearcherResultsMessage);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<RelativeLayout>(Resource.Id.rlHistoricalOrderDetail));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            RvHistoricalDetailOrder = FindViewById<RecyclerView>(Resource.Id.rvHistoricalDetailOrder);
            TvIdOrder = FindViewById<TextView>(Resource.Id.tvIdOrder);
            TvDate = FindViewById<TextView>(Resource.Id.tvDate);
            TvQuantityProducts = FindViewById<TextView>(Resource.Id.tvQuantityProducts);
            CkSelectAll = FindViewById<CheckBox>(Resource.Id.ckSelectAll);
            LyAddList = FindViewById<LinearLayout>(Resource.Id.lyAddList);
            LyAddCar = FindViewById<LinearLayout>(Resource.Id.lyAddCar);
            LyInsideAddCar = FindViewById<ConstraintLayout>(Resource.Id.lyInsideAddCar);

            LyAddCar.Enabled = false;

            LyAddList.Click += delegate { OnSendList(); };
            LyAddCar.Click += delegate { OnSendCar(); };
            CkSelectAll.Click += delegate { OnSelectAll(); };
            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvHistoricalDetailOrder.NestedScrollingEnabled = false;
            RvHistoricalDetailOrder.HasFixedSize = true;
            RvHistoricalDetailOrder.SetLayoutManager(linerLayoutManager);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvNameOrder).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameDate).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameProducts).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameSelectAll).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAddList).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAddCar).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvIdOrder.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvQuantityProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private async Task DrawProducts()
        {
            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                if (!string.IsNullOrEmpty(bundle.GetString(ConstantPreference.OrderId)))
                {
                    OrderDetailResponse = await this.GetHistoricalOrderDetail(bundle.GetString(ConstantPreference.OrderId));

                    if (OrderDetailResponse != null && OrderDetailResponse.Products != null && OrderDetailResponse.Products.Count > 0)
                    {
                        this.RunOnUiThread(() =>
                        {
                            historicalProductAdapter = new HistoricalProductAdapter(OrderDetailResponse.Products, this, this);
                            RvHistoricalDetailOrder.SetAdapter(historicalProductAdapter);
                            TvIdOrder.Text = OrderDetailResponse.OrderId;
                            TvDate.Text = OrderDetailResponse.OrderDate;
                            TvQuantityProducts.Text = OrderDetailResponse.Products != null ? Convert.ToString(OrderDetailResponse.Products.Count) : string.Empty;
                        });

                        ShowBodyLayout();
                    }
                }
            }
        }

        private void OnSendList()
        {
            OnBackPressed();
        }

        private void OnSendCar()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var productList = OrderDetailResponse.Products.Where(x => x.Selected);

                if (productList != null && productList.Any())
                {
                    _productCarModel.UpsertProducts(productList.ToList());
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
                    { ConstantActivityName.BaseProductActivity, ConstantMethodName.OnSendCar } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void OnSelectAll()
        {
            foreach (var item in OrderDetailResponse.Products)
            {
                item.Selected = CkSelectAll.Checked;
                historicalProductAdapter.NotifyDataSetChanged();
            }

            if (CkSelectAll.Checked)
            {
                IsAddProducts = true;
            }
            else
            {
                IsAddProducts = false;
            }

            this.EnableCar();
        }

        private void RegisterAddProductEvent(Product product)
        {
            FirebaseRegistrationEventsService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookRegistrationEventsService.Instance.AddProductToCart(product);
        }

        #endregion        
    }
}