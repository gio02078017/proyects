using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.Payments;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Products;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Activities.Orders
{
    [Activity(Label = "Informacion y Sugerencias del pago", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OrderStatusInfoActivity : BaseActivity
    {
        #region Controls

        private RecyclerView RvDeletedProducts;
        private LinearLayoutManager linerLayoutManager;
        private DeletedProductsAdapter _DeletedProductsAdapter;
        private RecyclerView RvModificatedUnits;
        private ModificatedUnitsAdapter _ModificatedUnitsAdapter;
        private LinearLayout LyExcuseUsPayment;
        private LinearLayout LyDeletedProducts;
        private LinearLayout LyModificatedUnits;
        private LinearLayout LyUpdatedTotal;
        private TextView TvPriceUpdatedTotal;
        private LinearLayout LySuggestPrime;
        private TextView TvMessageEnjoyPrime;
        private TextView TvTotalPaymentStep1;
        private TextView TvRememberPrime;
        private LinearLayout LyReturnCar;
        private LinearLayout LyContinuePayment;

        #endregion

        #region Properties

        private IList<SoldOut> ListSoldOut { get; set; }
        private IList<SoldOut> ListStockOut { get; set; }
        private PaymentResponse Response { get; set; }
        private bool ModifiedProducts { get; set; }
        private bool OrderWithoutProducts { get; set; }
        private ProductCarModel _productCarModel { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            ValidateReturn();
        }

        protected override void OnResume()
        {
            base.OnResume();
            ModifiedProducts = false;
            ListSoldOut = new List<SoldOut>();
            ListStockOut = new List<SoldOut>();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityOrderStatusInfo);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            HideItemsToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.PaymentResponse)))
            {
                Response = JsonService.Deserialize<PaymentResponse>(Intent.Extras.GetString(ConstantPreference.PaymentResponse));
            }

            this.SetControlsProperties();
            this.EditFonts();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvExcuseUsPayment).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageExcuseUsPayment).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleDeletedProducts).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvModificatedUnits).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleUpdatedTotal).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvContinuePayment).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvReturnCar).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPriceUpdatedTotal.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvMessageEnjoyPrime.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            
            TvRememberPrime.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTotalPaymentStep1.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTotalPaymentStep2).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);            
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            RvDeletedProducts = FindViewById<RecyclerView>(Resource.Id.rvDeletedProducts);
            RvModificatedUnits = FindViewById<RecyclerView>(Resource.Id.rvModificatedUnits);

            LyExcuseUsPayment = FindViewById<LinearLayout>(Resource.Id.lyExcuseUsPayment);
            LyDeletedProducts = FindViewById<LinearLayout>(Resource.Id.lyDeletedProducts);
            LyModificatedUnits = FindViewById<LinearLayout>(Resource.Id.lyModificatedUnits);
            LyUpdatedTotal = FindViewById<LinearLayout>(Resource.Id.lyUpdatedTotal);
            TvPriceUpdatedTotal = FindViewById<TextView>(Resource.Id.tvPriceUpdatedTotal);

            LySuggestPrime = FindViewById<LinearLayout>(Resource.Id.lySuggestPrime);
            TvMessageEnjoyPrime = FindViewById<TextView>(Resource.Id.tvMessageEnjoyPrime);
            TvTotalPaymentStep1 = FindViewById<TextView>(Resource.Id.tvTotalPaymentStep1);
            TvRememberPrime = FindViewById<TextView>(Resource.Id.tvRememberPrime);
            LyContinuePayment = FindViewById<LinearLayout>(Resource.Id.lyContinuePayment);
            LyReturnCar = FindViewById<LinearLayout>(Resource.Id.lyReturnCar);

            LyReturnCar.Click += delegate { this.ReturnCar(); };
            LyContinuePayment.Click += delegate { this.ContinuePayment(); };

            IvToolbarBack.Click += delegate
            {
                ValidateReturn();
            };

            this.ValidateContinuePayment();
            this.ProcessPaymentResponse();
        }

        private void ValidateContinuePayment()
        {
            if (Response.IsPrime && Response.CostRemaining > 0)
            {
                LyContinuePayment.Visibility = ViewStates.Visible;
                OrderWithoutProducts = false;
            }
            else
            {
                if (Response.Total == 0)
                {
                    LyContinuePayment.Visibility = ViewStates.Gone;
                    OrderWithoutProducts = true;
                }
                else
                {
                    LyContinuePayment.Visibility = ViewStates.Visible;
                    OrderWithoutProducts = false;
                }
            }
        }

        private void ValidateReturn()
        {
            if (OrderWithoutProducts)
            {
                OrderWithoutProducts = false;
                ReturnCar();
            }
            else
            {
                if (ModifiedProducts)
                {
                    ReturnCar();
                }
                else
                {
                    Finish();
                }
            }
        }

        private void ReturnCar()
        {
            Intent intent = new Intent(this, typeof(SummaryActivity));
            intent.PutExtra(ConstantPreference.KeepLobby, true);
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
        }

        private void ContinuePayment()
        {
            Intent intent = new Intent(this, typeof(SummaryOfBuyActivity));
            intent.PutExtra(ConstantPreference.PaymentResponse, JsonService.Serialize(Response));
            HideProgressDialog();
            StartActivity(intent);
            Finish();
        }

        private SpannableStringBuilder ConvertMessageEnjoyPrime(decimal price)
        {
            string strMessageEnjoyPrime = GetString(Resource.String.str_message_enjoy_prime);
            string strYouAreMissingStart = GetString(Resource.String.str_you_are_missing_start);
            string strEnjoyPrime = " " + StringFormat.ToPrice(price) + " ";
            string strYouAreMissingEnd = GetString(Resource.String.str_you_are_missing_end);
            int lengthMessageEnjoyPrime = strMessageEnjoyPrime.Length;
            int lengthMessageEnjoyPrimeWithStart = strMessageEnjoyPrime.Length + strYouAreMissingStart.Length;
            int lengthPrice = strEnjoyPrime.Length;
            int lengthPriceWithStart = lengthMessageEnjoyPrimeWithStart + strEnjoyPrime.Length;
            string strTvMessageEnjoyPrime = strMessageEnjoyPrime + strYouAreMissingStart + strEnjoyPrime + strYouAreMissingEnd;
            SpannableStringBuilder strSpannableMessageEnjoyPrime = new SpannableStringBuilder(strTvMessageEnjoyPrime);
            strSpannableMessageEnjoyPrime.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, strMessageEnjoyPrime.Length, SpanTypes.ExclusiveExclusive);
            strSpannableMessageEnjoyPrime.SetSpan(new StyleSpan(TypefaceStyle.Bold), lengthMessageEnjoyPrimeWithStart, lengthPriceWithStart, SpanTypes.ExclusiveExclusive);
            return strSpannableMessageEnjoyPrime;
        }

        private SpannableStringBuilder ConvertRememberPrime(decimal price)
        {
            string strPriceRememberPrime = " " + StringFormat.ToPrice(price) + " ";
            string strRememberPrimeStart = GetString(Resource.String.str_remember_prime_start);
            string strRememberPrimeEnd = GetString(Resource.String.str_remember_prime_end);
            string strTvRememberPrime = strRememberPrimeStart + strPriceRememberPrime + strRememberPrimeEnd;
            SpannableStringBuilder strSpannableRememberPrime = new SpannableStringBuilder(strTvRememberPrime);
            strSpannableRememberPrime.SetSpan(new StyleSpan(TypefaceStyle.Bold), (strTvRememberPrime.Length - strRememberPrimeEnd.Length), strTvRememberPrime.Length, SpanTypes.ExclusiveExclusive);
            return strSpannableRememberPrime;
        }

        private void DrawRvDeletedProducts()
        {
            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvDeletedProducts.NestedScrollingEnabled = false;
            RvDeletedProducts.HasFixedSize = true;
            RvDeletedProducts.SetLayoutManager(linerLayoutManager);
            _DeletedProductsAdapter = new DeletedProductsAdapter(ListSoldOut, this);
            RvDeletedProducts.SetAdapter(_DeletedProductsAdapter);
        }

        private void DrawRvModificatedUnits()
        {
            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvModificatedUnits.NestedScrollingEnabled = false;
            RvModificatedUnits.HasFixedSize = true;
            RvModificatedUnits.SetLayoutManager(linerLayoutManager);
            _ModificatedUnitsAdapter = new ModificatedUnitsAdapter(ListStockOut, this);
            RvModificatedUnits.SetAdapter(_ModificatedUnitsAdapter);
        }

        private void ProcessPaymentResponse()
        {
            this.ValidatePrimeResponse();
            this.ValidateListStockOut();
            this.ValidateListSoldOut();
            this.ValidateTotalProducts();
        }

        private void ValidatePrimeResponse()
        {
            if (Response.IsPrime && Response.CostRemaining > 0)
            {
                LySuggestPrime.Visibility = ViewStates.Visible;
                TvMessageEnjoyPrime.TextFormatted = this.ConvertMessageEnjoyPrime(Response.CostRemaining);
                TvTotalPaymentStep1.Text = GetString(Resource.String.str_total_payment_step_1) + " "+ StringFormat.ToPrice(Response.Total);
                TvRememberPrime.TextFormatted = this.ConvertRememberPrime(50000);
            }
            else
            {
                LySuggestPrime.Visibility = ViewStates.Gone;
            }
        }

        private void ValidateListSoldOut()
        {
            if (Response.ProductsRemoved != null && Response.ProductsRemoved.Any())
            {
                LyDeletedProducts.Visibility = ViewStates.Visible;
                ListSoldOut = Response.ProductsRemoved;
                ModifiedProducts = true;

                if (ListSoldOut != null && ListSoldOut.Any())
                {
                    _productCarModel.DeleteProductSouldOut(ListSoldOut);                   
                }
                
                this.DrawRvDeletedProducts();
            }
            else
            {
                LyDeletedProducts.Visibility = ViewStates.Gone;
            }
        }

        private void ValidateListStockOut()
        {
            if (Response.ProductsChanged != null && Response.ProductsChanged.Any())
            {
                LyModificatedUnits.Visibility = ViewStates.Visible;
                ModifiedProducts = true;
                ListStockOut = Response.ProductsChanged;

                if (ListStockOut != null && ListStockOut.Any())
                {
                    _productCarModel.UpdateProductStockOut(ListStockOut);
                }

                this.DrawRvModificatedUnits();
            }
            else
            {
                LyModificatedUnits.Visibility = ViewStates.Gone;
            }
        }

        private void UpdateOrder()
        {
            _productCarModel.RecalculateSummary();
            var products = _productCarModel.GetProducts();
            var summary = _productCarModel.GetSummary();

            if (products != null)
            {
                Order order = ParametersManager.Order;
                order.Products = products;
                order.TotalProducts = products.Count;
                order.SubTotal = decimal.Parse(summary[ConstDataBase.TotalPrice].ToString());
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }
        }

        private void ValidateTotalProducts()
        {
            if (ModifiedProducts)
            {
                UpdateOrder();
                LyExcuseUsPayment.Visibility = ViewStates.Visible;
                LyUpdatedTotal.Visibility = ViewStates.Visible;
                TvPriceUpdatedTotal.Text = StringFormat.ToPrice(Response.Total);
            }
            else
            {
                LyExcuseUsPayment.Visibility = ViewStates.Gone;
                LyUpdatedTotal.Visibility = ViewStates.Gone;
            }
        }
    }
}