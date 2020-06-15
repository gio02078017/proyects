using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Orders;
using GrupoExito.Android.Adapters;
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
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Logic.Models.Payments;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Pagar", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PaymentActivity : BaseCreditCardActivity, IPayment
    {
        #region Controls

        private TextView TvSendTo;
        private TextView TvSendToAddress;
        private TextView TvExitoCard;
        private TextView TvOtherCards;
        private LinearLayoutManager linerLayoutManager;
        private Spinner SpCreditCardQuotes;
        private LinearLayout LyCardQuotes;
        private LinearLayout LyDetails;
        private TextView TvSubTotalSummary;
        private TextView TvCostBagTitleSummary;
        private ImageView IvBoxTaxes;

        private LinearLayout LyButtonPay;

        private RecyclerView RvPayment;
        private PaymentAdapter _PaymentAdapter;
        #endregion

        #region Properties

        private int PaymentType = -1;
        private PaymentModel PaymentModel;
        private OrderScheduleModel OrderScheduleModel;
        private CreditCard Card;
        private IList<CreditCard> PaymentItems { get; set; }
        private List<string> CreditCardQuotes { get; set; }
        private string CreditCardQuote { get; set; }
        private bool ScheduleReservationFailed { get; set; }
        private PaymentSummaryModel _paymentSummaryModel { get; set; }

        private PaymentResponse Response { get; set; }
        private CreditCardResponse _CreditCardResponse { get; set; }

        #endregion

        #region Public Methods

        public void OnDeleteMyCardsClicked(CreditCard myCard)
        {
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        #endregion

        #region Protected Methods

        protected override async void OnResume()
        {
            base.OnResume();
            SetSummaryCarItems();

            if (ParametersManager.AddCreditCard)
            {
                ParametersManager.AddCreditCard = false;
                await DrawPaymentItems();
            }
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Payment, typeof(PaymentActivity).Name);
        }

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            base.HandlerOkButton(sender, e);

            if (ScheduleReservationFailed)
            {
                Finish();
            }
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ActivityPayment);
            PaymentModel = new PaymentModel(new PaymentService(DeviceManager.Instance));
            CreditCardModel = new CreditCardModel(new CreditCardService(DeviceManager.Instance));
            OrderScheduleModel = new OrderScheduleModel(new OrderScheduleService(DeviceManager.Instance));
            _paymentSummaryModel = new PaymentSummaryModel(new PaymentSummaryService(DeviceManager.Instance));
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            await DrawPaymentItems();
            this.PutQuotesData();
            if (LyCardQuotes != null)
            {
                LyCardQuotes.Visibility = ViewStates.Gone;
            }            
        }

        protected override void EventError()
        {
            base.EventError();
            ShowBodyLayout();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        #endregion

        #region Private Methods

        #region Draw Information

        private void PutQuotesData()
        {
            CreditCardQuotes = ModelHelper.GetCreditCardQuotes();

            this.RunOnUiThread(() =>
            {
                ArrayAdapter<string> adapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, CreditCardQuotes);
                SpCreditCardQuotes.Adapter = adapter;
            });
        }

        private void SetControlsProperties()
        {
            LyDetails = FindViewById<LinearLayout>(Resource.Id.lyDetail);
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetSetSummaryControls(LyDetails.FindViewById<TextView>(Resource.Id.tvSubTotalPriceSummary),
                LyDetails.FindViewById<TextView>(Resource.Id.tvCostBagPriceSummary));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
             FindViewById<RelativeLayout>(Resource.Id.rlBody), AppMessages.NotSearcherResultsTitle, AppMessages.NotSearcherResultsMessage);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<RelativeLayout>(Resource.Id.rlBody));
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            TvSendTo = FindViewById<TextView>(Resource.Id.tvSendTo);
            TvSendToAddress = FindViewById<TextView>(Resource.Id.tvSendToAddress);

            RvPayment = FindViewById<RecyclerView>(Resource.Id.rvPayment);

            TvSubTotalSummary = LyDetails.FindViewById<TextView>(Resource.Id.tvSubTotalSummary);
            TvCostBagTitleSummary = LyDetails.FindViewById<TextView>(Resource.Id.tvCostBagTitleSummary);
            IvBoxTaxes = LyDetails.FindViewById<ImageView>(Resource.Id.ivInformation);

            LyCardQuotes = FindViewById<LinearLayout>(Resource.Id.lyCardQuotes);
            LyCardQuotes.Visibility = ViewStates.Gone;

            SpCreditCardQuotes = FindViewById<Spinner>(Resource.Id.spCreditCardQuotes);

            TvExitoCard = FindViewById<TextView>(Resource.Id.tvExitoCard);
            TvOtherCards = FindViewById<TextView>(Resource.Id.tvOtherCards);

            LyButtonPay = FindViewById<LinearLayout>(Resource.Id.lyButtonPay);

            this.SetControlsEvents();
        }

        private void SetControlsEvents()
        {
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            TvOtherCards.Click += delegate { AddCreditCard(); ; };
            TvExitoCard.Click += delegate { AddSiteCreditCard(); ; };
            LyButtonPay.Click += async delegate { await ConfirmPaymentMethod(); };
            IvBoxTaxes.Click += delegate { OnBoxTaxesTouched(); };
            FindViewById<LinearLayout>(Resource.Id.lyCostBag).Click += delegate { OnBoxTaxesTouched(); };
        }

        private void EditFonts()
        {
            
            FindViewById<TextView>(Resource.Id.tvQuotesNumber).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium),
                TypefaceStyle.Normal);

            TvExitoCard.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvOtherCards.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            FindViewById<TextView>(Resource.Id.tvTitlePayment).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvDescriptionPayment).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvButtonPay).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            TvSubTotalSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCostBagTitleSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            
        }

        private async Task DrawPaymentItems()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                _CreditCardResponse = await CreditCardModel.GetAllPaymentMethods();

                if (_CreditCardResponse.Result != null && _CreditCardResponse.Result.HasErrors && _CreditCardResponse.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_CreditCardResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    linerLayoutManager = new LinearLayoutManager(this)
                    {
                        AutoMeasureEnabled = true
                    };

                    RvPayment.NestedScrollingEnabled = false;
                    RvPayment.HasFixedSize = true;
                    RvPayment.SetLayoutManager(linerLayoutManager);
                    PaymentItems = _CreditCardResponse.CreditCards;
                    _PaymentAdapter = new PaymentAdapter(PaymentItems, this, this);
                    RvPayment.SetAdapter(_PaymentAdapter);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.PaymentActivity, ConstantMethodName.DrawPaymentItems } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        #endregion

        #region Events credit cards

        private void AddCreditCard()
        {
            Intent intent = new Intent(this, typeof(AddCreditCardActivity));
            intent.PutExtra(ConstantPreference.AddCreditCardFromPay, true);
            StartActivity(intent);
        }

        private void AddSiteCreditCard()
        {
            Intent intent = new Intent(this, typeof(AddCreditCardPaymentezActivity));
            intent.PutExtra(ConstantPreference.AddCreditCardFromPay, true);
            StartActivity(intent);
        }

        private async Task ConfirmPaymentMethod()
        {
            await ValidatePay();
        }

        private async Task ValidatePay()
        {
            ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
            string responseMessage = this.ValidateSelectedPaymentMethod();

            if (string.IsNullOrEmpty(responseMessage))
            {
                await SaveProducts();
            }
            else
            {
                HideProgressDialog();
                OnBoxMessageTouched(responseMessage, AppMessages.AcceptButtonText);               
            }
        }

        private string ValidateSelectedPaymentMethod()
        {
            string responseMessage = string.Empty;

            if (Card == null)
            {
                return AppMessages.PaymentMethodMessage;
            }

            if (Card.Bin.Equals(ConstTypePayment.OnDeliver))
            {
                return ValidateSelectedDeliveryPayment();
            }

            if (!Card.Paymentez)
            {
                return ValidateSelectedPaymentCreditCard();
            }

            if (Card.Paymentez)
            {
                return ValidateSelectedPaymentSiteCreditCard();
            }

            return responseMessage;
        }

        private string ValidateSelectedDeliveryPayment()
        {
            string responseMessage = string.Empty;

            if (PaymentType < 0)
            {
                return AppMessages.DeliveryMethodPayMessage;
            }

            return responseMessage;
        }

        private string ValidateSelectedPaymentCreditCard()
        {
            string responseMessage = string.Empty;

            if (Card == null)
            {
                return AppMessages.CreditCardMethodPayMessage;
            }
            else
            {
                if (SpCreditCardQuotes.SelectedItemPosition > 0)
                {
                    CreditCardQuote = CreditCardQuotes[SpCreditCardQuotes.SelectedItemPosition];
                }
                else
                {
                    return AppMessages.CreditCardQuoteMessage;
                }
            }

            return responseMessage;
        }

        private string ValidateSelectedPaymentSiteCreditCard()
        {
            string responseMessage = string.Empty;

            if (Card == null)
            {
                return AppMessages.CreditCardMethodPayMessage;
            }
            else
            {
                if (SpCreditCardQuotes.SelectedItemPosition > 0)
                {
                    CreditCardQuote = CreditCardQuotes[SpCreditCardQuotes.SelectedItemPosition];
                }
                else
                {
                    return AppMessages.CreditCardQuoteMessage;
                }
            }

            return responseMessage;
        }

        public void OnCardSelected(CreditCard selectedCard)
        {
            if (selectedCard != null)
            {
                Card = PaymentItems.Where(x => x.Selected).FirstOrDefault();

                if (Card != null)
                {
                    Card.Selected = false;
                }

                selectedCard.Selected = true;


                _PaymentAdapter.NotifyDataSetChanged();

                Card = selectedCard;

                LyCardQuotes.Visibility = selectedCard.Selected && !selectedCard.Bin.Equals("Pago contra entrega") ? ViewStates.Visible : ViewStates.Gone;

            }
        }

        public void OnCardSelectedTypeOnDeliver(int PaymentType)
        {
            this.PaymentType = PaymentType;
        }

        #endregion

        #region Summary

        private Order SetOrder()
        {
            var order = ParametersManager.Order;
            bool selectedStore = ParametersManager.UserContext.Store != null ? true : false;
            string dependencyId = ParametersManager.UserContext.DependencyId;

            if (order != null)
            {

                if (Card.Bin.Equals(ConstTypePayment.OnDeliver))
                {
                    order.TypePayment = ConstTypePayment.Delivery;
                    order.PaymentType = PaymentType;
                }
                else
                {
                    if (Card.Paymentez)
                    {
                        order.Bin = Card.Bin;
                        order.NumberDues = CreditCardQuote;
                        order.TypePayment = ConstTypePayment.CreditCardExito;
                        order.PaymentType = (int)EnumPaymentType.Dataphone;
                        order.Number = Card.Number;
                    }
                    else
                    {
                        order.Bin = Card.Bin;
                        order.NumberDues = CreditCardQuote;
                        order.TypePayment = ConstTypePayment.CreditCard;
                        order.PaymentType = (int)EnumPaymentType.Dataphone;
                    }
                }

                order.TypeOfDispatch = order.Contingency ? order.TypeOfDispatch
                    : selectedStore ? ConstTypeOfDispatch.Store : ConstTypeOfDispatch.Delivery;
                order.DependencyAddress = selectedStore ? ParametersManager.UserContext.Store.Address : string.Empty;
                order.DependencyName = selectedStore ? ParametersManager.UserContext.Store.Name : string.Empty;
                order.DependencyId = int.Parse(dependencyId).ToString();
                order.TypeDispatch = order.Contingency ? order.TypeDispatch :
                    order.TypeModality == ConstTypeModality.ScheduledPickup ? ConstTypeModality.Scheduled : ConstTypeModality.Express;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }

            return order;
        }

        private async Task PaymentSummary()
        {
            try
            {
                var order = this.SetOrder();
                PaymentSummaryResponse response = await _paymentSummaryModel.Summary(order);
                ValidateSummaryResponse(response);
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.PaymentActivity, ConstantMethodName.PaymentSummary } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void ValidateSummaryResponse(PaymentSummaryResponse response)
        {
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
                if ((response.ProductsRemoved != null && response.ProductsRemoved.Any()) ||
                    (response.ProductsChanged != null && response.ProductsChanged.Any()) ||
                    (response.IsPrime && response.CostRemaining > 0))
                {
                    Intent intent = new Intent(this, typeof(OrderStatusInfoActivity));
                    ParametersManager.CallFromSummaryPayment = true;
                    HideProgressDialog();
                    intent.PutExtra(ConstantPreference.PaymentResponse, JsonService.Serialize(response));
                    StartActivity(intent);
                }
                else
                {
                    RegisterContinueEvent();
                    Intent intent = new Intent(this, typeof(SummaryOfBuyActivity));
                    intent.PutExtra(ConstantPreference.PaymentResponse, JsonService.Serialize(response));
                    HideProgressDialog();
                    StartActivity(intent);
                }
            }
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
                await PaymentSummary();
            }
        }

        #endregion

        public void RegisterContinueEvent()
        {
            FirebaseRegistrationEventsService.Instance.Payment();
        }

        #endregion
    }
}