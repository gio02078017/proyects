using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Orders;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Logic.Models.Payments;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Resumen de tu compra", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SummaryOfBuyActivity : BaseActivity
    {
        #region Controls
        private TextView TvPriceTotal;
        private LinearLayout LyQuantityProducts;
        private TextView TvNameQuantityProducts;
        private TextView TvQuantityProducts;
        private LinearLayout LyAddress;
        private TextView TvNameAddress;
        private TextView TvAddress;
        private LinearLayout LyPaymentMethod;
        private TextView TvNamePaymentMethod;
        private TextView TvPaymentMethod;
        private LinearLayout LyScheduled;
        private TextView TvNameScheduled;
        private TextView TvScheduled;
        private View ViewScheduled;

        private LinearLayout LySubTotal;
        private TextView TvNameSubTotal;
        private TextView TvPriceSubTotal;
        private LinearLayout LyPrimeSavings;
        private TextView TvNamePrimeSavings;
        private TextView TvPrimeSavings;
        private LinearLayout LyAccumulatedPoints;
        private TextView TvNameAccumulatedPoints;
        private TextView TvAccumulatedPoints;
        private LinearLayout LyCostToSend;
        private TextView TvNameCostToSend;
        private TextView TvCostToSend;
        private LinearLayout LyCostBag;
        private TextView TvNameCostBag;
        private TextView TvCostBag;
        private LinearLayout LyDiscounts;
        private TextView TvNameDiscounts;
        private TextView TvDiscounts;
        private LinearLayout LyButtonPay;
        #endregion

        #region Properties

        private PaymentSummaryModel _paymentSummaryModel { get; set; }
        private PaymentModel _paymentModel { get; set; }
        private OrderScheduleModel _orderScheduleModel { get; set; }
        private PaymentSummaryResponse Response { get; set; }

        #endregion

        #region Protected Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySummaryOfBuy);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            _paymentSummaryModel = new PaymentSummaryModel(new PaymentSummaryService(DeviceManager.Instance));
            _paymentModel = new PaymentModel(new PaymentService(DeviceManager.Instance));

            HideItemsCarToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.PaymentResponse)))
            {
                Response = JsonService.Deserialize<PaymentSummaryResponse>(Intent.Extras.GetString(ConstantPreference.PaymentResponse));
            }

            DrawInformation();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.SummaryOfBuy, typeof(SummaryOfBuyActivity).Name);
        }

        #endregion

        #region Private Methods

        #region Set Controls

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleSummaryOfBuy).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameTotal).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvButtonPay).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvPriceTotal.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvNameQuantityProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvQuantityProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvAddress.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNamePaymentMethod.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvPaymentMethod.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameScheduled.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvScheduled.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameSubTotal.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvPriceSubTotal.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNamePrimeSavings.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvPrimeSavings.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameAccumulatedPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvAccumulatedPoints.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameCostToSend.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCostToSend.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameCostBag.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCostBag.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvNameDiscounts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvDiscounts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            TvPriceTotal = FindViewById<TextView>(Resource.Id.tvPriceTotal);

            LyQuantityProducts = FindViewById<LinearLayout>(Resource.Id.layoutQuantity);
            TvNameQuantityProducts = LyQuantityProducts.FindViewById<TextView>(Resource.Id.tvName);
            TvQuantityProducts = LyQuantityProducts.FindViewById<TextView>(Resource.Id.tvValue);

            LyAddress = FindViewById<LinearLayout>(Resource.Id.layoutAddress);
            TvNameAddress = LyAddress.FindViewById<TextView>(Resource.Id.tvName);
            TvAddress = LyAddress.FindViewById<TextView>(Resource.Id.tvValue);

            LyPaymentMethod = FindViewById<LinearLayout>(Resource.Id.layoutPaymentMethod);
            TvNamePaymentMethod = LyPaymentMethod.FindViewById<TextView>(Resource.Id.tvName);
            TvPaymentMethod = LyPaymentMethod.FindViewById<TextView>(Resource.Id.tvValue);

            LyScheduled = FindViewById<LinearLayout>(Resource.Id.layoutScheduled);
            TvNameScheduled = LyScheduled.FindViewById<TextView>(Resource.Id.tvName);
            TvScheduled = LyScheduled.FindViewById<TextView>(Resource.Id.tvValue);
            ViewScheduled = LyScheduled.FindViewById<View>(Resource.Id.ViewLine);
            ViewScheduled.Visibility = ViewStates.Gone;

            LySubTotal = FindViewById<LinearLayout>(Resource.Id.layoutSubTotal);
            TvNameSubTotal = LySubTotal.FindViewById<TextView>(Resource.Id.tvName);
            TvPriceSubTotal = LySubTotal.FindViewById<TextView>(Resource.Id.tvValue);

            LyPrimeSavings = FindViewById<LinearLayout>(Resource.Id.layoutPrimeSavings);
            TvNamePrimeSavings = LyPrimeSavings.FindViewById<TextView>(Resource.Id.tvName);
            TvPrimeSavings = LyPrimeSavings.FindViewById<TextView>(Resource.Id.tvValue);

            LyAccumulatedPoints = FindViewById<LinearLayout>(Resource.Id.layoutAccumulatedPoints);
            TvNameAccumulatedPoints = LyAccumulatedPoints.FindViewById<TextView>(Resource.Id.tvName);
            TvAccumulatedPoints = LyAccumulatedPoints.FindViewById<TextView>(Resource.Id.tvValue);

            LyCostToSend = FindViewById<LinearLayout>(Resource.Id.layoutCostToSend);
            TvNameCostToSend = LyCostToSend.FindViewById<TextView>(Resource.Id.tvName);
            TvCostToSend = LyCostToSend.FindViewById<TextView>(Resource.Id.tvValue);

            LyCostBag = FindViewById<LinearLayout>(Resource.Id.layoutCostBag);
            TvNameCostBag = LyCostBag.FindViewById<TextView>(Resource.Id.tvName);
            TvCostBag = LyCostBag.FindViewById<TextView>(Resource.Id.tvValue);

            LyDiscounts = FindViewById<LinearLayout>(Resource.Id.layoutDiscounts);
            TvNameDiscounts = LyDiscounts.FindViewById<TextView>(Resource.Id.tvName);
            TvDiscounts = LyDiscounts.FindViewById<TextView>(Resource.Id.tvValue);

            LyButtonPay = FindViewById<LinearLayout>(Resource.Id.lyButtonPay);

            NameStart();
            NameEnd();

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            LyButtonPay.Click += async delegate
            {
                await SaveProducts();
            };
        }

        public void NameStart()
        {
            TvNameQuantityProducts.Text = GetString(Resource.String.str_quantity_of_products);
            TvNameAddress.Text = GetString(Resource.String.str_address_sends);
            TvNamePaymentMethod.Text = GetString(Resource.String.str_mid_pay);
            TvNameScheduled.Text = GetString(Resource.String.str_scheduled_schedule);
        }

        public void NameEnd()
        {
            TvNameSubTotal.Text = GetString(Resource.String.str_subtotal);
            TvNamePrimeSavings.Text = GetString(Resource.String.str_prime_savings);
            TvNameAccumulatedPoints.Text = GetString(Resource.String.str_accumulated_points_without_points);
            TvNameCostToSend.Text = GetString(Resource.String.str_cost_to_send_without_point);
            TvNameCostBag.Text = GetString(Resource.String.str_cost_bag_without_points);
            TvNameDiscounts.Text = GetString(Resource.String.str_discounts);
        }

        #endregion

        public void DrawInformation()
        {
            Order order = ParametersManager.Order;

            TvQuantityProducts.Text = order.TotalProducts.ToString();
            TvAddress.Text = ParametersManager.UserContext.Store != null ? ParametersManager.UserContext.Store.Address
                : ParametersManager.UserContext.Address.AddressComplete;
            TvPaymentMethod.Text = Response.PaymentMethodName;


            if (!string.IsNullOrEmpty(order.DateSelected) && !string.IsNullOrEmpty(order.Schedule))
            {
                LyScheduled.Visibility = ViewStates.Visible;
                TvScheduled.Text = order.DateSelected + " " + order.Schedule;
            }
            else
            {
                if (order.TypeModality.Equals("Express"))
                {
                    TvScheduled.Text = StringFormat.AddTimeToDateNow(order.MinutesPromiseDelivery);
                }
                else
                {
                    LyScheduled.Visibility = ViewStates.Gone;
                    TvScheduled.Text = string.Empty;
                }
            }

            TvPriceSubTotal.Text = StringFormat.ToPrice(Response.SubTotal);

            if (AppServiceConfiguration.SiteId.Equals("exito") && Response.IsPrime && order.TypeDispatch == ConstTypeOfDispatch.Delivery)
            {
                LyPrimeSavings.Visibility = ViewStates.Visible;
                TvPrimeSavings.Text = order.shippingCost;
            }
            else
            {
                LyPrimeSavings.Visibility = ViewStates.Gone;
                TvPrimeSavings.Text = string.Empty;
            }

            TvCostToSend.Text = CostSend(order);

            if (Response.IsPrime)
            {
                LyCostToSend.Visibility = Response.CostRemaining == decimal.Parse("0.0") ? ViewStates.Gone : ViewStates.Visible;
            }
            else
            {
                LyCostToSend.Visibility = order.TypeOfDispatch == ConstTypeOfDispatch.Delivery ? ViewStates.Visible : ViewStates.Gone;
            }

            TvAccumulatedPoints.Text = Response.Points.ToString();
            TvCostBag.Text = StringFormat.ToPrice(Response.CountryTax);
            TvDiscounts.Text = StringFormat.ToPrice(Response.Discounts);
            TvPriceTotal.Text = StringFormat.ToPrice(Response.Total);
        }

        #region Reservation

        private ScheduleReservationParameters SetParameters()
        {
            var order = ParametersManager.Order;
            ScheduleReservationParameters parameters = null;
            string dependencyId = ParametersManager.UserContext.DependencyId;

            if (order != null)
            {
                bool selectedStore = ParametersManager.UserContext.Store != null ? true : false;
                parameters = new ScheduleReservationParameters
                {
                    OrderId = order.Id,
                    ShippingCost = order.shippingCost,
                    DeliveryMode = selectedStore ? ConstDeliveryMode.Pe : ConstDeliveryMode.Do,
                    QuantityUnits = ParametersManager.Order.TotalProducts,
                    DependencyId = dependencyId.Length == 2 ? "0" + dependencyId : dependencyId,
                    Schedule = order.Schedule,
                    TypeModality = order.TypeModality,
                    DateSelected = order.DateSelected
                };
            }

            return parameters;
        }

        private async Task ValidateScheduleReservation()
        {
            try
            {
                ScheduleReservationParameters parameters = SetParameters();
                _orderScheduleModel = new OrderScheduleModel(new OrderScheduleService(DeviceManager.Instance));
                var response = await _orderScheduleModel.ValidateScheduleReservation(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.ScheduleNotAvailableMessage, AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    if (response.ActiveReservation)
                    {
                        await Pay();
                    }
                    else
                    {
                        RunOnUiThread(() =>
                        {
                            HideProgressDialog();
                            DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.ScheduleNotAvailableMessage, AppMessages.AcceptButtonText);
                        });
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.PaymentActivity, ConstantMethodName.ValidateScheduleReservation } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        #endregion

        #region Pay

        private async Task ValidatePay()
        {
            var order = ParametersManager.Order;

            if (order.Contingency)
            {
                await Pay();
            }
            else
            {
                if (order.TypeModality == ConstTypeModality.Express)
                {
                    await Pay();
                }
                else
                {
                    await ValidateScheduleReservation();
                }
            }
        }

        private async Task Pay()
        {
            try
            {
                RegisterPayEvent();
                var order = ParametersManager.Order;
                var response = await _paymentModel.Pay(order);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();

                        if (order.TypePayment == ConstTypePayment.Delivery)
                        {
                            RedirectPaymentError(ConstStatusPay.DeliveryError);
                        }
                        else
                        {
                            RedirectPaymentError(ConstStatusPay.OtherError);
                        }
                    });
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        this.ProcessPaymentResponse(response);
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryOfBuyActivity, ConstantMethodName.Pay } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void ValidateStatusCreditCard(PaymentResponse response)
        {
            if (response.StatusCode.Equals(ConstStatusPay.Approved))
            {
                RedirectSuccessPayment(response);
            }
            else
            {
                RedirectPaymentError(response.StatusCode);
            }
        }

        private void ProcessPaymentResponse(PaymentResponse response)
        {
            ParametersManager.ChangeProductQuantity = true;

            if (response.FinishProcess)
            {
                if (!string.IsNullOrEmpty(response.StatusCode))
                {
                    ValidateStatusCreditCard(response);
                }
                else
                {
                    RedirectSuccessPayment(response);
                }
            }
            else
            {
                RedirectOrderStatusInfo(response);
            }
        }

        private void RedirectPaymentError(string value)
        {
            Intent intent = new Intent(this, typeof(PaymentErrorActivity));
            intent.PutExtra(ConstantPreference.PaymentErrorResponse, value);
            HideProgressDialog();
            StartActivity(intent);
            Finish();
        }

        private void RedirectSuccessPayment(PaymentResponse response)
        {
            RegisterContinueEvent(ParametersManager.Order, response);
            Intent intent = new Intent(this, typeof(SuccessPaymentActivity));
            intent.PutExtra(ConstantPreference.PaymentResponse, JsonService.Serialize(response));
            HideProgressDialog();
            StartActivity(intent);
            Finish();
        }

        private void RedirectOrderStatusInfo(PaymentResponse response)
        {
            Intent intent = new Intent(this, typeof(OrderStatusInfoActivity));
            intent.PutExtra(ConstantPreference.PaymentResponse, JsonService.Serialize(response));
            HideProgressDialog();
            StartActivity(intent);
        }

        #endregion

        #region Validate Order

        private async Task SaveProducts()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                ProductCarModel _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
                List<Product> products = _productCarModel.GetProducts();

                OrderModel _orderModel = new OrderModel(new OrderService(DeviceManager.Instance));

                var responseOrder = await _orderModel.GetOrder();

                if (responseOrder.Result != null && responseOrder.Result.HasErrors && responseOrder.Result.Messages != null)
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(responseOrder.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    var order = ParametersManager.Order;

                    if (!order.Id.Equals(responseOrder.ActiveOrderId))
                    {
                        order.Id = responseOrder.ActiveOrderId;
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
                    }

                    await SaveProductsInTheOrder(order);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SummaryOfBuyActivity, ConstantMethodName.SaveProducts } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private async Task SaveProductsInTheOrder(Order order)
        {
            ProductsModel _productModel = new ProductsModel(new ProductsService(DeviceManager.Instance));

            var responseAddProductModel = await _productModel.AddProducts(order);

            if (responseAddProductModel.Result != null && responseAddProductModel.Result.HasErrors && responseAddProductModel.Result.Messages != null)
            {
                HideProgressDialog();
                DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(responseAddProductModel.Result), AppMessages.AcceptButtonText);
            }
            else
            {
                await ValidatePay();
            }
        }

        #endregion

        public void RegisterContinueEvent(Order order, PaymentResponse response)
        {
            order.Total = response.Total;
            order.SubTotal = response.SubTotal;
            order.CountryTax = response.CountryTax;
            FirebaseRegistrationEventsService.Instance.SuccessPayment(order);
            FacebookRegistrationEventsService.Instance.Purchased((double)response.Total, ParametersManager.Order?.Products);
        }

        public void RegisterPayEvent()
        {
            FirebaseRegistrationEventsService.Instance.SummaryPayment();
            FacebookRegistrationEventsService.Instance.AddPaymentInfo(true);
        }

        #endregion
    }
}

