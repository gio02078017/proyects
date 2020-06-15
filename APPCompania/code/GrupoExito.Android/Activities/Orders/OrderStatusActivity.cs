using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.Constraints;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Support.Constraints.ConstraintLayout;

namespace GrupoExito.Android.Activities.Orders
{
    [Activity(Label = "Estado del pedido", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OrderStatusActivity : BaseOrderActivity
    {
        #region Controls

        private ConstraintLayout LyCircleInside1;
        private ConstraintLayout LyCircleInside3;
        private ConstraintLayout LyCircleInside2;
        private View ViewLineStatus1;
        private View ViewLineStatus2;
        private View ViewLineStatus3;
        private View ViewDivider;
        private ImageView IvCircle1;
        private ImageView IvTypeOrder1;
        private ImageView IvCircle3;
        private ImageView IvTypeOrder3;
        private ImageView IvCircle2;
        private ImageView IvTypeOrder2;
        private ImageView IvArrowOrder;
        private LinearLayout LyOrderStatusStep2;
        private LinearLayout LyStep2;
        private LinearLayout LyCircle2;
        private LinearLayout LyIvTypeOrder2;
        private LinearLayout LyOrderStatusStep1;
        private LinearLayout LyStep1;
        private LinearLayout LyCircle1;
        private LinearLayout LyIvTypeOrder1;
        private LinearLayout LyDetailDelivery;
        private LinearLayout LyOrderHeader;
        private LinearLayout LyArrowOrder;
        private LinearLayout LyDetailOrder;
        private LinearLayout LyOrderStatusStep3;
        private LinearLayout LyStep3;
        private LinearLayout LyCircle3;
        private LinearLayout LyIvTypeOrder3;
        private TextView TvNameStep2;
        private TextView TvDescripcionStep2;
        private TextView TvHourStep2;
        private TextView TvNameStep3;
        private TextView TvDescripcionStep3;
        private TextView TvHourStep3;
        private TextView TvMessageStep3;
        private LinearLayout LyBoddyStatusStep3;
        private TextView TvNameStep1;
        private TextView TvDescripcionStep1;
        private TextView TvHourStep1;
        private TextView TvTitleStatusOrder;
        private TextView TvDelivery;
        private TextView TvAddressDelivery;
        private TextView TvDateDeliver;
        private TextView TvHourRangeDeliver;
        private TextView TvSupport;
        private TextView TvNameOrder;
        private TextView TvIdOrder;
        private TextView TvNameProducts;
        private TextView TvQuantityProducts;
        private TextView TvNameSee;
        private TextView TvNameTotal;
        private TextView TvTotal;
        private TextView TvNameMidPay;
        private TextView TvMidPay;
        private TextView TvCanceledTitle;
        private TextView TvCanceledMessage;
        private TextView TvViewProducts;
        private TableRow RowProducts;
        private TableRow RowTotal;
        private TableRow RowMidPay;
        private LinearLayout LyOrderCanceled;
        private ConstraintLayout ClOrderInfo;
        

        #endregion

        #region Properties

        private TrackingOrderResponse trackingOrderResponse;
        private int sizeLine;
        private bool eventDetails;
        private Context context;
        private Order Order;
        private string OrderType;

        #endregion

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityOrderStatus);
            OrderModel = new OrderModel(new OrderService(DeviceManager.Instance));

            eventDetails = false;
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            context = this;

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                Order = JsonService.Deserialize<Order>(Intent.Extras.GetString(ConstantPreference.Order));
                OrderType = Intent.Extras.GetString(ConstantPreference.OrderType);
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            this.DrawOrderInfo();
            await DrawTrackingData();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.OrderStatus, typeof(OrderStatusActivity).Name);
        }

        protected override void EventError()
        {
            base.EventError();
            this.RunOnUiThread(async () =>
            {
                await this.DrawTrackingData();
            });
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        private void DrawOrderInfo()
        {
            if (Order != null)
            {
                TvIdOrder.Text = Order.Id;
                TvHourRangeDeliver.Text = Order.Schedule ?? string.Empty;
                TvTotal.Text = StringFormat.ToPrice(Order.Total);
                TvQuantityProducts.Text = Convert.ToString(Order.TotalProducts);
                TvDelivery.Text = OrderType;
                TvAddressDelivery.Text = Order.Address ?? string.Empty;

                if (TvAddressDelivery.Text.Length == 0)
                {
                    TvAddressDelivery.Visibility = ViewStates.Gone;
                }

                TvDateDeliver.Text = ConvertUtilities.CustomDatewithDayWeek(Order.Date, true);
            }
        }

        private void DrawCanceledInfo()
        {
            LyOrderCanceled.Visibility = ViewStates.Visible;
            ClOrderInfo.Visibility = ViewStates.Gone;
        }

        private async Task DrawTrackingData()
        {
            try
            {
                if (Order != null && !string.IsNullOrEmpty(Order.Id))
                {
                    ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                    trackingOrderResponse = await OrderModel.GetTrackingOrder(new Order { Id = Order.Id });

                    if (trackingOrderResponse.Result != null && trackingOrderResponse.Result.HasErrors && trackingOrderResponse.Result.Messages != null)
                    {
                        RunOnUiThread(() =>
                        {
                            HideProgressDialog();
                            var errorCode = (EnumErrorCode)Enum.Parse(typeof(EnumErrorCode), trackingOrderResponse.Result.Messages.First().Code);

                            if (errorCode == EnumErrorCode.OrderTrackingNotFound)
                            {
                                ShowNoInfoLayout();
                            }
                            else
                            {
                                ShowErrorLayout(MessagesHelper.GetMessage(trackingOrderResponse.Result));
                            }
                        });
                    }
                    else
                    {
                        if (trackingOrderResponse.TrackingOrders != null)
                        {
                            if (trackingOrderResponse.TrackingOrders != null && 
                                trackingOrderResponse.TrackingOrders.Where(x => x.StatusName.Equals(ConstOrderStatus.Cancel)).Any())
                            {
                                this.DrawCanceledInfo();
                                DrawStep1Data(true);
                                DrawStep2Data(true);
                                DrawStep3Data(true);
                            }
                            else
                            {
                                DrawStep1Data();
                                DrawStep2Data();
                                DrawStep3Data();
                                ShowBodyLayout();
                            }
                        }
                        else
                        {
                            ShowNoInfoLayout();
                        }

                        HideProgressDialog();
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.OrderStatusActivity, ConstantMethodName.DrawTrackingData } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void DrawStep1Data(bool EventCancel = false)
        {
            if (!EventCancel)
            {
                if (trackingOrderResponse.TrackingOrders.Count <= 0 || !trackingOrderResponse.TrackingOrders[0].Status)
                {
                    IvCircle1.Visibility = ViewStates.Invisible;
                    TvHourStep1.Visibility = ViewStates.Gone;
                    LyCircleInside1.SetBackgroundResource(Resource.Drawable.circle_white);
                }
                else
                {
                    TvHourStep1.Text = trackingOrderResponse.TrackingOrders[0].Date;
                    LyCircleInside1.SetBackgroundResource(Resource.Drawable.circle_yellow);
                    IvCircle1.Visibility = ViewStates.Visible;
                }

                TvDescripcionStep1.Text = trackingOrderResponse.TrackingOrders.Any() ? trackingOrderResponse.TrackingOrders[0].StatusName : string.Empty;
            }
            else
            {
                
                IvTypeOrder1.SetImageResource(Resource.Drawable.productos_gris);
                TvDescripcionStep1.Text = GetString(Resource.String.str_description_step_one);
                DrawStepGray(LyStep1, LyCircle1, LyCircleInside1, ViewLineStatus1);
                DrawStepGrayText(TvNameStep1, TvHourStep1, TvDescripcionStep1);
                
            }            

            sizeLine = this.OnSizeLine(LyStep1, LyCircle1);
            this.OnHeightLine(sizeLine, ViewLineStatus1);
            this.OnCenterImage(sizeLine, LyIvTypeOrder1);

        }

        private void DrawStep2Data(bool EventCancel = false)
        {
            if (!EventCancel)
            {
                if (trackingOrderResponse.TrackingOrders.Count <= 1 || !trackingOrderResponse.TrackingOrders[1].Status)
                {
                    IvCircle2.Visibility = ViewStates.Invisible;
                    TvHourStep2.Visibility = ViewStates.Gone;
                    LyCircleInside2.SetBackgroundResource(Resource.Drawable.circle_white);
                }
                else
                {
                    TvHourStep2.Text = trackingOrderResponse.TrackingOrders[1].Date;
                    LyCircleInside2.SetBackgroundResource(Resource.Drawable.circle_yellow);
                    IvCircle2.Visibility = ViewStates.Visible;
                }

                TvDescripcionStep2.Text = trackingOrderResponse.TrackingOrders.Count > 1 ? trackingOrderResponse.TrackingOrders[1].StatusName : string.Empty;
            }
            else
            {
                IvTypeOrder2.SetImageResource(Resource.Drawable.domicilios_primario);
                TvDescripcionStep2.Text = GetString(Resource.String.str_description_step_two);
                DrawStepGray(LyStep2, LyCircle2, LyCircleInside2, ViewLineStatus2);
                DrawStepGrayText(TvNameStep2, TvHourStep2, TvDescripcionStep2);
            }

            
            sizeLine = this.OnSizeLine(LyStep2, LyCircle2);
            this.OnHeightLine(sizeLine, ViewLineStatus2);
            this.OnCenterImage(sizeLine, LyIvTypeOrder2);
        }

        private void DrawStep3Data(bool EventCancel = false)
        {
            if (!EventCancel)
            {
                if (trackingOrderResponse.TrackingOrders.Count <= 2 || !trackingOrderResponse.TrackingOrders[2].Status)
                {
                    IvCircle3.Visibility = ViewStates.Invisible;
                    TvHourStep3.Visibility = ViewStates.Gone;
                    LyCircleInside3.SetBackgroundResource(Resource.Drawable.circle_white);
                }
                else
                {
                    TvHourStep3.Text = trackingOrderResponse.TrackingOrders[2].Date;
                    LyCircleInside3.SetBackgroundResource(Resource.Drawable.circle_yellow);
                    IvCircle3.Visibility = ViewStates.Visible;
                }

                TvDescripcionStep3.Text = trackingOrderResponse.TrackingOrders.Count > 2 ? trackingOrderResponse.TrackingOrders[2].StatusName : string.Empty;
                LyBoddyStatusStep3.Visibility = ViewStates.Visible;
            }
            else
            {
               
               IvTypeOrder3.SetImageResource(Resource.Drawable.entregado);
               TvDescripcionStep3.Text = GetString(Resource.String.str_description_step_three);
               DrawStepGray(LyStep3, LyCircle3, LyCircleInside3, ViewLineStatus3);
               DrawStepGrayText(TvNameStep3, TvHourStep3, TvDescripcionStep3);
               LyBoddyStatusStep3.Visibility = ViewStates.Gone;
            }
            
            sizeLine = this.OnSizeLine(LyStep3, LyCircle3) + 60;
            this.OnHeightLine(sizeLine, ViewLineStatus3);
            this.OnCenterImage(sizeLine, LyIvTypeOrder3);


        }

        private void DrawStepGray(LinearLayout LyStep, LinearLayout LyCircle, ConstraintLayout LyCircleInside, View ViewLineStatus)
        {
            LyCircle.SetBackgroundResource(Resource.Drawable.circle_gray);
            LyCircleInside.SetBackgroundResource(Resource.Drawable.circle_white);
            ViewLineStatus.SetBackgroundColor(Resources.GetColor(Resource.Color.colorGray));
        }

        private void DrawStepGrayText(TextView TvNameStep, TextView TvHourStep, TextView TvDescripcionStep)
        {
            TvNameStep.SetTextColor(Resources.GetColor(Resource.Color.colorGray));
            TvDescripcionStep.SetTextColor(Resources.GetColor(Resource.Color.colorGray));
            TvHourStep.Visibility = ViewStates.Gone;
            
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice),
            FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                             FindViewById<RelativeLayout>(Resource.Id.rlBody),
                             AppMessages.NoOrderStatusMessage, AppMessages.OrderStatusAction);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                           FindViewById<RelativeLayout>(Resource.Id.rlBody));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            LyDetailDelivery = FindViewById<LinearLayout>(Resource.Id.lyDetailDelivery);
            TvTitleStatusOrder = FindViewById<TextView>(Resource.Id.tvTitleStatusOrder);
            TvDelivery = FindViewById<TextView>(Resource.Id.tvDelivery);
            TvAddressDelivery = FindViewById<TextView>(Resource.Id.tvAddressDelivery);
            TvSupport = FindViewById<TextView>(Resource.Id.tvSupport);
            TvDateDeliver = FindViewById<TextView>(Resource.Id.tvDateDeliver);
            TvHourRangeDeliver = FindViewById<TextView>(Resource.Id.tvHourRangeDeliver);
            LyOrderHeader = FindViewById<LinearLayout>(Resource.Id.lyOrderHeader);
            LyArrowOrder = FindViewById<LinearLayout>(Resource.Id.lyArrowOrder);
            LyDetailOrder = FindViewById<LinearLayout>(Resource.Id.lyDetailOrder);
            RowProducts = FindViewById<TableRow>(Resource.Id.rowProducts);
            RowTotal = FindViewById<TableRow>(Resource.Id.rowTotal);
            RowMidPay = FindViewById<TableRow>(Resource.Id.rowMidPay);
            TvNameOrder = FindViewById<TextView>(Resource.Id.tvNameOrder);
            TvIdOrder = FindViewById<TextView>(Resource.Id.tvIdOrder);
            TvNameProducts = FindViewById<TextView>(Resource.Id.tvNameProducts);
            TvQuantityProducts = FindViewById<TextView>(Resource.Id.tvQuantityProducts);
            TvNameSee = FindViewById<TextView>(Resource.Id.tvNameSee);
            TvNameTotal = FindViewById<TextView>(Resource.Id.tvNameTotal);
            TvTotal = FindViewById<TextView>(Resource.Id.tvTotal);
            TvNameMidPay = FindViewById<TextView>(Resource.Id.tvNameMidPay);
            TvMidPay = FindViewById<TextView>(Resource.Id.tvMidPay);
            ViewDivider = FindViewById<View>(Resource.Id.viewDivider);
            LyOrderCanceled = FindViewById<LinearLayout>(Resource.Id.lyOrderCanceled);
            TvCanceledTitle = LyOrderCanceled.FindViewById<TextView>(Resource.Id.tvCanceledTitle);
            TvCanceledMessage = LyOrderCanceled.FindViewById<TextView>(Resource.Id.tvCanceledMessage);
            TvViewProducts = LyOrderCanceled.FindViewById<TextView>(Resource.Id.tvViewProducts);
            ClOrderInfo = FindViewById<ConstraintLayout>(Resource.Id.clOrderInfo);
            IvArrowOrder = FindViewById<ImageView>(Resource.Id.ivArrowOrder);

            IvArrowOrder.Click += delegate { EventArrawSummary(); };
            LyArrowOrder.Click += delegate { EventArrawSummary(); };
            TvSupport.Click += delegate { EventSupport(); };
            TvNameSee.Click += delegate { EventSeeProducts(); };
            TvViewProducts.Click += delegate { EventSeeProducts(); };

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            HideItemsCarToolbar(this);

            this.SetControlsPropertiesStep1();
            this.EditFontsStep1();
            this.SetControlsPropertiesStep2();
            this.EditFontsStep2();
            this.SetControlsPropertiesStep3();
            this.EditFontsStep3();
            this.EventArrawSummary();
        }

        private void EventSeeProducts()
        {
            Intent intent = new Intent(this, typeof(HistoricalOrderDetailActivity));
            intent.PutExtra(ConstantPreference.OrderId, Order.Id);
            StartActivity(intent);
        }

        private void SetControlsPropertiesStep1()
        {
            LyOrderStatusStep1 = FindViewById<LinearLayout>(Resource.Id.lyOrderStatusStep1);
            LyStep1 = LyOrderStatusStep1.FindViewById<LinearLayout>(Resource.Id.lyStep);
            TvNameStep1 = LyOrderStatusStep1.FindViewById<TextView>(Resource.Id.tvNameStep);
            TvDescripcionStep1 = LyOrderStatusStep1.FindViewById<TextView>(Resource.Id.tvDescripcionStep);
            TvHourStep1 = LyOrderStatusStep1.FindViewById<TextView>(Resource.Id.tvHourStep);
            LyCircle1 = LyOrderStatusStep1.FindViewById<LinearLayout>(Resource.Id.lyCircle);
            LyIvTypeOrder1 = LyOrderStatusStep1.FindViewById<LinearLayout>(Resource.Id.lyIvTypeOrder);
            ViewLineStatus1 = LyOrderStatusStep1.FindViewById<View>(Resource.Id.viewLineStatus);
            LyCircleInside1 = LyOrderStatusStep1.FindViewById<ConstraintLayout>(Resource.Id.lyCircleInside);
            IvCircle1 = LyOrderStatusStep1.FindViewById<ImageView>(Resource.Id.ivCircle);
            IvTypeOrder1 = LyOrderStatusStep1.FindViewById<ImageView>(Resource.Id.ivTypeOrder);
            IvTypeOrder1.SetImageResource(Resource.Drawable.productos_primario);
            TvNameStep1.Text = "Paso 1";

            sizeLine = this.OnSizeLine(LyStep1, LyCircle1);
            this.OnHeightLine(sizeLine, ViewLineStatus1);
            this.OnCenterImage(sizeLine, LyIvTypeOrder1);
        }

        private void SetControlsPropertiesStep2()
        {
            LyOrderStatusStep2 = FindViewById<LinearLayout>(Resource.Id.lyOrderStatusStep2);
            LyStep2 = LyOrderStatusStep2.FindViewById<LinearLayout>(Resource.Id.lyStep);
            TvNameStep2 = LyOrderStatusStep2.FindViewById<TextView>(Resource.Id.tvNameStep);
            TvDescripcionStep2 = LyOrderStatusStep2.FindViewById<TextView>(Resource.Id.tvDescripcionStep);
            TvHourStep2 = LyOrderStatusStep2.FindViewById<TextView>(Resource.Id.tvHourStep);
            LyCircle2 = LyOrderStatusStep2.FindViewById<LinearLayout>(Resource.Id.lyCircle);
            LyIvTypeOrder2 = LyOrderStatusStep2.FindViewById<LinearLayout>(Resource.Id.lyIvTypeOrder);
            ViewLineStatus2 = LyOrderStatusStep2.FindViewById<View>(Resource.Id.viewLineStatus);
            LyCircleInside2 = LyOrderStatusStep2.FindViewById<ConstraintLayout>(Resource.Id.lyCircleInside);
            IvCircle2 = LyOrderStatusStep2.FindViewById<ImageView>(Resource.Id.ivCircle);
            IvTypeOrder2 = LyOrderStatusStep2.FindViewById<ImageView>(Resource.Id.ivTypeOrder);
            IvTypeOrder2.SetImageResource(Resource.Drawable.domicilios);
            TvNameStep2.Text = "Paso 2";

            sizeLine = this.OnSizeLine(LyStep2, LyCircle2);
            this.OnHeightLine(sizeLine, ViewLineStatus2);
            this.OnCenterImage(sizeLine, LyIvTypeOrder2);
        }

        private void SetControlsPropertiesStep3()
        {
            LyOrderStatusStep3 = FindViewById<LinearLayout>(Resource.Id.lyOrderStatusStep3);
            LyStep3 = LyOrderStatusStep3.FindViewById<LinearLayout>(Resource.Id.lyStep);
            TvNameStep3 = LyOrderStatusStep3.FindViewById<TextView>(Resource.Id.tvNameStep);
            TvDescripcionStep3 = LyOrderStatusStep3.FindViewById<TextView>(Resource.Id.tvDescripcionStep);
            TvDescripcionStep3 = LyOrderStatusStep3.FindViewById<TextView>(Resource.Id.tvDescripcionStep);
            TvHourStep3 = LyOrderStatusStep3.FindViewById<TextView>(Resource.Id.tvHourStep);
            TvMessageStep3 = FindViewById<TextView>(Resource.Id.tvMessageStep3);
            LyCircle3 = LyOrderStatusStep3.FindViewById<LinearLayout>(Resource.Id.lyCircle);
            LyIvTypeOrder3 = LyOrderStatusStep3.FindViewById<LinearLayout>(Resource.Id.lyIvTypeOrder);
            ViewLineStatus3 = LyOrderStatusStep3.FindViewById<View>(Resource.Id.viewLineStatus);
            LyCircleInside3 = LyOrderStatusStep3.FindViewById<ConstraintLayout>(Resource.Id.lyCircleInside);
            IvCircle3 = LyOrderStatusStep3.FindViewById<ImageView>(Resource.Id.ivCircle);
            IvTypeOrder3 = LyOrderStatusStep3.FindViewById<ImageView>(Resource.Id.ivTypeOrder);
            LyBoddyStatusStep3 = FindViewById<LinearLayout>(Resource.Id.lyBoddyStatusStep3);
            FindViewById<TextView>(Resource.Id.tvToServeYou).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            IvTypeOrder3.SetImageResource(Resource.Drawable.listo);
            TvNameStep3.Text = "Paso 3";

            sizeLine = this.OnSizeLine(LyStep3, LyCircle3);
            SpannableStringBuilder strMessageAlertAddress = new SpannableStringBuilder(GetString(Resource.String.str_delivered_message));
            int color = context.Resources.GetColor(Resource.Color.abc_background_cache_hint_selector_material_dark);
            ForegroundColorSpan fcs = new ForegroundColorSpan(Color.Rgb(37, 132, 168));
            strMessageAlertAddress.SetSpan(fcs, 11, 35, SpanTypes.ExclusiveExclusive);
            strMessageAlertAddress.SetSpan(new StyleSpan(TypefaceStyle.Bold), 11, 35, SpanTypes.ExclusiveExclusive);
            TvMessageStep3.TextFormatted = strMessageAlertAddress;
            this.OnHeightLine(sizeLine, ViewLineStatus3);
            this.OnCenterImage(sizeLine, LyIvTypeOrder3);
        }

        private void EditFonts()
        {
            TvTitleStatusOrder.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDelivery.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAddressDelivery.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvSupport.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvDateDeliver.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvHourRangeDeliver.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvNameOrder.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvIdOrder.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvNameProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvQuantityProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvNameSee.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvNameTotal.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvTotal.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvNameMidPay.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvMidPay.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCanceledMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCanceledTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvViewProducts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
        }

        private void EditFontsStep1()
        {
            TvNameStep1.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDescripcionStep1.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void EditFontsStep2()
        {
            TvNameStep2.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDescripcionStep2.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void EditFontsStep3()
        {
            TvNameStep3.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDescripcionStep3.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvMessageStep3.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private int OnSizeLine(LinearLayout LyStep, LinearLayout LyCircle)
        {
            LyStep.Measure(0, 0);
            LyCircle.Measure(0, 0);
            int circle = ConvertPixelsToDp(LyCircle.MeasuredHeight);
            int halfCicle = circle / 2;
            int restCircle = halfCicle - 5;
            int sizeLine = (ConvertPixelsToDp(LyStep.MeasuredHeight) - restCircle) * (int)Resources.System.DisplayMetrics.Density;
            return sizeLine;
        }

        private void OnHeightImage(int sizeImage, ImageView image)
        {
            LayoutParams layoutparams = (ConstraintLayout.LayoutParams)image.LayoutParameters;
            layoutparams.Height = ConvertDpToPixels(sizeImage);
            layoutparams.Width = ConvertDpToPixels(sizeImage);
            image.LayoutParameters = layoutparams;
        }

        private void OnHeightLine(int sizeLine, View ViewLineStatus)
        {
            LayoutParams layoutparams = (ConstraintLayout.LayoutParams)ViewLineStatus.LayoutParameters;
            layoutparams.Height = sizeLine;
            layoutparams.Width = ConvertDpToPixels(1.5);
            ViewLineStatus.LayoutParameters = layoutparams;
        }

        private void OnCenterImage(int sizeLine, LinearLayout LyIvTypeOrder)
        {
            LyIvTypeOrder.Measure(0, 0);
            LayoutParams layoutparams = (ConstraintLayout.LayoutParams)LyIvTypeOrder.LayoutParameters;
            int sizeLayout = LyIvTypeOrder.MeasuredHeight;
            int marginBottom = (sizeLine - sizeLayout) / 2;
            layoutparams.BottomMargin = marginBottom;
            LyIvTypeOrder.LayoutParameters = layoutparams;
        }

        private int ConvertPixelsToDp(float pixelValue)
        {
            var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
            return dp;
        }

        private int ConvertDpToPixels(double dp)
        {
            var pixelValue = Convert.ToInt32(dp * (double)Resources.System.DisplayMetrics.Density);
            return pixelValue;
        }

        private void EventArrawSummary()
        {
            eventDetails = !eventDetails;

            if (eventDetails)
            {
                LyDetailOrder.Visibility = ViewStates.Gone;
                ViewDivider.Visibility = ViewStates.Gone;
                IvArrowOrder.SetImageResource(Resource.Drawable.flecha_abajo_primaria);
            }
            else
            {
                LyDetailOrder.Visibility = ViewStates.Visible;
                ViewDivider.Visibility = ViewStates.Visible;
                IvArrowOrder.SetImageResource(Resource.Drawable.flecha_arriba_primaria);
            }
        }

        private void EventSupport()
        {
            Intent intent = new Intent(this, typeof(ContactUsActivity));
            StartActivity(intent);
        }
    }
}