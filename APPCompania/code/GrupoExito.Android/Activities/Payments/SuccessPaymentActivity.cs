using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Pago Exitoso", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SuccessPaymentActivity : BaseProductActivity
    {
        #region Controls

        private NestedScrollView NsvProducts;
        private LinearLayout LyRootLayout;
        private LinearLayout LyMainSuccessPayment;
        private LinearLayout LyAddressOfDeliver;
        private LinearLayout LyMainSubTotalPayment;
        private LinearLayout LySubTotalPayment;
        private LinearLayout LyMethodPaid;
        private LinearLayout LyTotalPayment;
        private LinearLayout LyAddNewProduct;
        private LinearLayout LyTitleMethodPaid;
        private LinearLayout LyCardPayment;
        private LinearLayout LyDiscountPayment;
        private LinearLayout LyPoints;
        private LinearLayout LyMainPoints;
        private LinearLayout LyNotAvailable;
        private LinearLayout LyFollowIsBuying;
        private TextView TvSuccessPayment;
        private TextView TvMessagePayment;
        private TextView TvOrderPayment;
        private TextView TvTitleAddressOfDelivery;
        private TextView TvAddressOfDelivery;
        private TextView TvDateDelivery;
        private TextView TvTitleProductPayment;
        private TextView TvQuantityProduct;
        private TextView TvTitleBoughtSummary;
        private TextView TvAddNewProduct;
        private TextView TvSubTotalPayment;
        private TextView TvSubTotalPricePayment;
        private TextView TvCostSendTitlePayment;
        private TextView TvCostSendPricePayment;
        private TextView TvCostBagTitlePayment;
        private TextView TvCostBagPricePayment;
        private TextView TvDiscountsAppliedTitlePayment;
        private TextView TvDiscountsAppliedPricePayment;
        private TextView TvTotalTitlePayment;
        private TextView TvTotalPricePayment;
        private TextView TvTitleMethodPaid;
        private TextView TvCardPayment;
        private TextView TvTitleDiscountPayment;
        private TextView TvPriceDiscountPayment;
        private TextView TvTitlePoints;
        private TextView TvTitleAccumulatedPoints;
        private TextView TvAccumulatedPoints;
        private TextView TvTitleRedeemedPoints;
        private TextView TvRedeemedPoints; private TextView TvTitleTotalPoints;
        private TextView TvTotalPoints;
        private TextView TvMessageNotAvailable;
        private TextView TvTitleChooseSuccessPayment;
        private TextView TvChooseSuccessPayment;
        private ImageView IvBoxTaxes;
        private View ViewDivideSubTotal;
        private View ViewDivideTotal;
        private View ViewDivideMethodPaid;
        private View ViewDivideMainPoints;
        private TableRow RowCostBag;
        private TableRow RowCostSend;
        private TableRow RowSubTotal;
        private TableRow RowDiscounts;
        private TableRow RowTitlePoints;
        private TableRow RowAccumulatedPoints;
        private TableRow RowTitleRedeemedPoints;
        private TableRow RowTitleTotalPoints;
        private Button BtnFollowIsBuying;

        #endregion

        #region Properties

        private string OrderId { get; set; }
        private PaymentResponse PaymentResponse;
        private ProductCarModel _productCarModel;

        #endregion

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            GoToNextBuying();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySuccessPayment);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            SetActionBar(toolbar);

            FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Visible;

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.PaymentResponse)))
            {
                PaymentResponse = JsonService.Deserialize<PaymentResponse>(Intent.Extras.GetString(ConstantPreference.PaymentResponse));
            }

            HideItemsToolbar(this);
            this.SetControlsProperties();
            this.EditFonts();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.SuccessPayment, typeof(SuccessPaymentActivity).Name);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            NsvProducts = FindViewById<NestedScrollView>(Resource.Id.nsvProducts);
            LyRootLayout = FindViewById<LinearLayout>(Resource.Id.lyRootLayout);
            LyMainSuccessPayment = FindViewById<LinearLayout>(Resource.Id.lyMainSuccessPayment);
            TvSuccessPayment = FindViewById<TextView>(Resource.Id.tvSuccessPayment);
            TvMessagePayment = FindViewById<TextView>(Resource.Id.tvMessagePayment);
            TvOrderPayment = FindViewById<TextView>(Resource.Id.tvOrderPayment);

            LyAddressOfDeliver = FindViewById<LinearLayout>(Resource.Id.lyAddressOfDeliver);
            TvTitleAddressOfDelivery = FindViewById<TextView>(Resource.Id.tvTitleAddressOfDelivery);
            TvAddressOfDelivery = FindViewById<TextView>(Resource.Id.tvAddressOfDelivery);
            TvDateDelivery = FindViewById<TextView>(Resource.Id.tvDateDelivery);

            ViewDivideSubTotal = FindViewById<View>(Resource.Id.viewDivideSubTotal);
            LyMainSubTotalPayment = FindViewById<LinearLayout>(Resource.Id.lyMainSubTotalPayment);
            TvTitleBoughtSummary = FindViewById<TextView>(Resource.Id.tvTitleBoughtSummary);
            LyMainSubTotalPayment = FindViewById<LinearLayout>(Resource.Id.lyMainSubTotalPayment);
            LySubTotalPayment = FindViewById<LinearLayout>(Resource.Id.lySubTotalPayment);

            TvTitleProductPayment = FindViewById<TextView>(Resource.Id.tvTitleProductPayment);
            TvTitleProductPayment.Text = StringFormat.Capitalize(Resources.GetString(Resource.String.str_products));
            TvQuantityProduct = FindViewById<TextView>(Resource.Id.tvQuantityProduct);
            TvAddNewProduct = FindViewById<TextView>(Resource.Id.tvAddNewProduct);
            LyAddNewProduct = FindViewById<LinearLayout>(Resource.Id.lyAddNewProduct);

            RowSubTotal = FindViewById<TableRow>(Resource.Id.rowSubTotal);
            TvSubTotalPayment = FindViewById<TextView>(Resource.Id.tvSubTotalPayment);
            TvSubTotalPricePayment = FindViewById<TextView>(Resource.Id.tvSubTotalPricePayment);

            RowCostSend = FindViewById<TableRow>(Resource.Id.rowCostSend);
            TvCostSendTitlePayment = FindViewById<TextView>(Resource.Id.tvCostSendTitlePayment);
            TvCostSendPricePayment = FindViewById<TextView>(Resource.Id.tvCostSendPricePayment);

            RowCostBag = FindViewById<TableRow>(Resource.Id.rowCostBag);
            TvCostBagTitlePayment = FindViewById<TextView>(Resource.Id.tvCostBagTitlePayment);
            TvCostBagPricePayment = FindViewById<TextView>(Resource.Id.tvCostBagPricePayment);
            IvBoxTaxes = FindViewById<ImageView>(Resource.Id.ivBoxTaxes);

            RowDiscounts = FindViewById<TableRow>(Resource.Id.rowDiscounts);
            TvDiscountsAppliedTitlePayment = FindViewById<TextView>(Resource.Id.tvDiscountsAppliedTitlePayment);
            TvDiscountsAppliedPricePayment = FindViewById<TextView>(Resource.Id.tvDiscountsAppliedPricePayment);

            ViewDivideTotal = FindViewById<View>(Resource.Id.viewDivideTotal);
            LyTotalPayment = FindViewById<LinearLayout>(Resource.Id.lyTotalPayment);
            TvTotalTitlePayment = FindViewById<TextView>(Resource.Id.tvTotalTitlePayment);
            TvTotalPricePayment = FindViewById<TextView>(Resource.Id.tvTotalPricePayment);

            ViewDivideMethodPaid = FindViewById<View>(Resource.Id.viewDivideMethodPaid);
            LyMethodPaid = FindViewById<LinearLayout>(Resource.Id.lyMethodPaid);
            LyTitleMethodPaid = FindViewById<LinearLayout>(Resource.Id.lyTitleMethodPaid);
            TvTitleMethodPaid = FindViewById<TextView>(Resource.Id.tvTitleMethodPaid);

            LyCardPayment = FindViewById<LinearLayout>(Resource.Id.lyCardPayment);
            TvCardPayment = FindViewById<TextView>(Resource.Id.tvCardPayment);

            LyDiscountPayment = FindViewById<LinearLayout>(Resource.Id.lyDiscountPayment);
            TvTitleDiscountPayment = FindViewById<TextView>(Resource.Id.tvTitleDiscountPayment);
            TvPriceDiscountPayment = FindViewById<TextView>(Resource.Id.tvPriceDiscountPayment);

            ViewDivideMainPoints = FindViewById<View>(Resource.Id.viewDivideMainPoints);
            LyMainPoints = FindViewById<LinearLayout>(Resource.Id.lyMainPoints);
            LyPoints = FindViewById<LinearLayout>(Resource.Id.lyPoints);

            RowTitlePoints = FindViewById<TableRow>(Resource.Id.rowTitlePoints);
            TvTitlePoints = FindViewById<TextView>(Resource.Id.tvTitlePoints);

            RowAccumulatedPoints = FindViewById<TableRow>(Resource.Id.rowAccumulatedPoints);
            TvTitleAccumulatedPoints = FindViewById<TextView>(Resource.Id.tvTitleAccumulatedPoints);
            TvAccumulatedPoints = FindViewById<TextView>(Resource.Id.tvAccumulatedPoints);

            RowTitleRedeemedPoints = FindViewById<TableRow>(Resource.Id.rowTitleRedeemedPoints);
            TvTitleRedeemedPoints = FindViewById<TextView>(Resource.Id.tvTitleRedeemedPoints);
            TvRedeemedPoints = FindViewById<TextView>(Resource.Id.tvRedeemedPoints);

            RowTitleTotalPoints = FindViewById<TableRow>(Resource.Id.rowTitleTotalPoints);
            TvTitleTotalPoints = FindViewById<TextView>(Resource.Id.tvTitleTotalPoints);
            TvTotalPoints = FindViewById<TextView>(Resource.Id.tvTotalPoints);

            LyNotAvailable = FindViewById<LinearLayout>(Resource.Id.lyNotAvailable);
            TvMessageNotAvailable = FindViewById<TextView>(Resource.Id.tvMessageNotAvailable);
            TvTitleChooseSuccessPayment = FindViewById<TextView>(Resource.Id.tvTitleChooseSuccessPayment);
            TvChooseSuccessPayment = FindViewById<TextView>(Resource.Id.tvChooseSuccessPayment);

            LyFollowIsBuying = FindViewById<LinearLayout>(Resource.Id.lyFollowIsBuying);
            BtnFollowIsBuying = FindViewById<Button>(Resource.Id.btnFollowIsBuying);

            IvBoxTaxes.Click += delegate { OnBoxTaxesTouched(); };

            this.SetControlsEvents();
            this.SetTexControls();
            this.SetTextPriceSumarry();
            this.SetMethodPaid();
            this.SetPoints();
            this.SetNotAvailable();
            this.FlushCar();
            this.SetToolbarCarItems();
        }

        private void SetTexControls()
        {
            if (PaymentResponse != null)
            {
                TvOrderPayment.Text = PaymentResponse.OrderId;
            }

            if (ParametersManager.UserContext != null)
            {
                TvAddressOfDelivery.Text = ParametersManager.UserContext.Address != null ?
                          ParametersManager.UserContext.Address.AddressComplete : ParametersManager.UserContext.Store.Name;
            }

            if (ParametersManager.Order != null && !string.IsNullOrEmpty(ParametersManager.Order.DateSelected) && !string.IsNullOrEmpty(ParametersManager.Order.Schedule))
            {
                TvDateDelivery.Visibility = ViewStates.Visible;
                TvDateDelivery.Text = this.DateSchedule(ParametersManager.Order.DateSelected, ParametersManager.Order.Schedule);
            }
            else
            {
                TvDateDelivery.Visibility = ViewStates.Gone;
            }
        }

        private void FlushCar()
        {
            _productCarModel.FlushCar();
            var order = ParametersManager.Order;
            order = new Entities.Order();
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
        }

        private void SetTextPriceSumarry()
        {

            TvQuantityProduct.Text = Convert.ToString(PaymentResponse.ProductsQuantity);
            TvSubTotalPricePayment.Text = StringFormat.ToPrice(PaymentResponse.SubTotal);
            TvCostBagPricePayment.Text = StringFormat.ToPrice(PaymentResponse.CountryTax);
            TvDiscountsAppliedPricePayment.Text = StringFormat.ToPrice(PaymentResponse.Discounts);
            TvTotalPricePayment.Text = StringFormat.ToPrice(PaymentResponse.Total);
            this.CostToSend();
        }

        private void CostToSend()
        {
            var order = ParametersManager.Order;

            TvCostSendPricePayment.Text = CostSend(order);

            if (PaymentResponse.IsPrime)
            {
                RowCostSend.Visibility = PaymentResponse.CostRemaining == decimal.Parse("0.0") ? ViewStates.Gone : ViewStates.Visible;
            }
            else
            {
               RowCostSend.Visibility = order.TypeOfDispatch == ConstTypeOfDispatch.Delivery ? ViewStates.Visible : ViewStates.Gone;
            }

        }

        private void SetMethodPaid()
        {
            TvCardPayment.Text = PaymentResponse.PaymentMethodName;
            LyDiscountPayment.Visibility = ViewStates.Gone;
        }

        private void SetPoints()
        {
            ViewDivideMainPoints.Visibility = ViewStates.Gone;
            LyMainPoints.Visibility = ViewStates.Gone;
        }

        private void SetNotAvailable()
        {
            if (!PaymentResponse.ProductsSubstitution)
            {
                TvChooseSuccessPayment.Text = Resources.GetString(Resource.String.str_replace_not_replace_payment);
            }
        }

        private void SetControlsEvents()
        {
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            BtnFollowIsBuying.Click += delegate { GoToNextBuying(); };
            TvAddNewProduct.Click += delegate { AddNewProducts(); };
            LyAddNewProduct.Click += delegate { AddNewProducts(); };
        }

        private void EditFonts()
        {
            TvSuccessPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvMessagePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvOrderPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvTitleAddressOfDelivery.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddressOfDelivery.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDateDelivery.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvTitleBoughtSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvTitleProductPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvQuantityProduct.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvAddNewProduct.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvSubTotalPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSubTotalPricePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            TvCostSendTitlePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCostSendPricePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvCostBagTitlePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCostBagPricePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvDiscountsAppliedTitlePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDiscountsAppliedPricePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvTotalTitlePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvTotalPricePayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvTitleMethodPaid.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvCardPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTitleDiscountPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPriceDiscountPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvTitlePoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvTitleAccumulatedPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAccumulatedPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTitleRedeemedPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvRedeemedPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTitleTotalPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTotalPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvMessageNotAvailable.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvTitleChooseSuccessPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvChooseSuccessPayment.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnFollowIsBuying.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        private void GoToNextBuying()
        {            
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.PutExtra(ConstantPreference.KeepLobby, true);
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
        }

        private void AddNewProducts()
        {
        }

        private string DateSchedule(string date, string rank)
        {
            string day = DateTime.Now.ToString("dd");
            day = day.TrimStart('0');
            string[] splitday = date.Split(' ');
            if ((splitday != null) && splitday.Length > 0 && (day.Equals(splitday[1])))
            {
                return "Hoy, " + date + " entre " + rank;
            }
            else
            {
                return date + " entre " + rank;
            }
        }
    }
}