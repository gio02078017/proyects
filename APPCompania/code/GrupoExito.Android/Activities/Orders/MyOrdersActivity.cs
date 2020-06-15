using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Orders
{
    [Activity(Label = "Mis pedidos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyOrdersActivity : BaseOrderActivity, IOrders
    {
        #region Controls

        private RecyclerView RvCurrentDeliveryOrder;
        private RecyclerView RvCurrentStoreOrder;
        private RecyclerView RvHistoricalOrder;
        private CurrentOrderAdapter currentDeliveryOrderAdapter;
        private CurrentOrderAdapter currentStoreOrderAdapter;
        private HistoricalOrderAdapter historicalOrderAdapter;
        private LinearLayoutManager CurrentDeliveryOrderLayoutManager;
        private LinearLayoutManager CurrentStoreOrderLayoutManager;
        private LinearLayoutManager HistoricalOrdersLayoutManager;
        private TextView TvQuantity;
        private TextView TvQuantityHistorical;
        private LinearLayout LyCurrentOrder;
        private LinearLayout LyStoreCurrentOrder;
        private LinearLayout LyViewLine;
        private LinearLayout LyViewLineHistory;
        private LinearLayout LyHistorical;
        private ConstraintLayout LyOutSideQuantity;

        #endregion

        #region Properties
        private OrdersResponse Response { get; set; }

        #endregion

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMyOrders);
            OrderModel = new OrderModel(new OrderService(DeviceManager.Instance));
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();            
        }

        protected async override void OnResume()
        {
            base.OnResume();
            await this.DrawOrders();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.MyOrders, typeof(MyOrdersActivity).Name);
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(intent);
        }

        protected override void EventError()
        {
            base.EventError();
            OnResume();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
               FindViewById<LinearLayout>(Resource.Id.lyBody), AppMessages.NotOrderMessage, AppMessages.AddProducts);

            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
              FindViewById<LinearLayout>(Resource.Id.lyBody));

            LyOutSideQuantity = FindViewById<ConstraintLayout>(Resource.Id.lyOutSideQuantity);
            LyViewLine = FindViewById<LinearLayout>(Resource.Id.lyViewLine);
            LyCurrentOrder = FindViewById<LinearLayout>(Resource.Id.lyCurrentOrder);
            LyStoreCurrentOrder = FindViewById<LinearLayout>(Resource.Id.lyStoreCurrentOrder);
            LyHistorical = FindViewById<LinearLayout>(Resource.Id.lyHistorical);
            LyViewLineHistory = FindViewById<LinearLayout>(Resource.Id.lyViewLineHistory);
            RvCurrentDeliveryOrder = FindViewById<RecyclerView>(Resource.Id.rvCurrentOrder);
            RvCurrentStoreOrder = FindViewById<RecyclerView>(Resource.Id.rvStoreCurrentOrder);
            RvHistoricalOrder = FindViewById<RecyclerView>(Resource.Id.rvHistoricalOrder);
            TvQuantity = FindViewById<TextView>(Resource.Id.tvQuantity);
            TvQuantityHistorical = FindViewById<TextView>(Resource.Id.tvQuantityHistorical);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            CurrentDeliveryOrderLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvCurrentDeliveryOrder.NestedScrollingEnabled = false;
            RvCurrentDeliveryOrder.HasFixedSize = true;
            RvCurrentDeliveryOrder.SetLayoutManager(CurrentDeliveryOrderLayoutManager);

            CurrentStoreOrderLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvCurrentStoreOrder.NestedScrollingEnabled = false;
            RvCurrentStoreOrder.HasFixedSize = true;
            RvCurrentStoreOrder.SetLayoutManager(CurrentStoreOrderLayoutManager);

            HistoricalOrdersLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };
            RvHistoricalOrder.NestedScrollingEnabled = false;
            RvHistoricalOrder.HasFixedSize = true;
            RvHistoricalOrder.SetLayoutManager(HistoricalOrdersLayoutManager);
            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleMyOrder).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageMyOrder).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleCurrents).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvQuantity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNameHistorical).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvQuantityHistorical.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageHistorical).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private async Task DrawOrders()
        {
            LyCurrentOrder.Visibility = ViewStates.Gone;
            LyStoreCurrentOrder.Visibility = ViewStates.Gone;
            LyOutSideQuantity.Visibility = ViewStates.Gone;
            LyViewLine.Visibility = ViewStates.Gone;

            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                OrderParameters orderParameters = new OrderParameters
                {
                    From = ParametersManager.From,
                    Size = ParametersManager.Size
                };

                Response = await GetOrders(orderParameters);

                if (Response != null && ((Response.Orders != null && Response.Orders.Any())
                         || (Response.HistoricalOrders != null && Response.HistoricalOrders.Any())))
                {
                    if (ValidateInformation())
                    {
                        ShowBodyLayout();

                        if (Response != null && Response.Orders != null && Response.Orders.Any())
                        {
                            ShowHistoricalOrders();
                        }
                    }
                    else
                    {
                        ShowNoInfoLayoutAddress();
                    }
                }
                else
                {
                    ShowNoInfoLayoutAddress();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyOrdersActivity, ConstantMethodName.DrawOrders } };
                RegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppServiceConfiguration.SiteId, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private void ShowHistoricalOrders()
        {
            bool ControlTypeOrder = false;

            this.RunOnUiThread(() =>
            {
                TvQuantity.Text = (Response.Orders[0].HomeDelivery.Count() + Response.Orders[0].PickUp.Count()).ToString();

                if (Response.Orders[0].HomeDelivery != null && Response.Orders[0].HomeDelivery.Count > 0)
                {
                    currentDeliveryOrderAdapter = new CurrentOrderAdapter(Response.Orders[0].HomeDelivery, this, this);
                    RvCurrentDeliveryOrder.SetAdapter(currentDeliveryOrderAdapter);

                    LyCurrentOrder.Visibility = ViewStates.Visible;
                    ControlTypeOrder = true;
                }

                if (Response.Orders[0].PickUp != null && Response.Orders[0].PickUp.Count > 0)
                {
                    currentStoreOrderAdapter = new CurrentOrderAdapter(Response.Orders[0].PickUp, this, this);
                    RvCurrentStoreOrder.SetAdapter(currentStoreOrderAdapter);

                    LyStoreCurrentOrder.Visibility = ViewStates.Visible;
                    ControlTypeOrder = true;
                }

                if (ControlTypeOrder)
                {
                    LyOutSideQuantity.Visibility = ViewStates.Visible;
                    LyViewLine.Visibility = ViewStates.Visible;
                }
            });

            this.DrawHistoricalOrders();
        }

        private void ShowNoInfoLayoutAddress()
        {
            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.DependencyId))
            {
                ShowNoInfoLayout();
            }
            else
            {
                ShowNoInfoLayout(true);
            }
        }

        private bool ValidateInformation()
        {
            bool validate = false;

            if (Response.Orders.Count > 0)
            {
                if (Response.Orders[0].HomeDelivery.Count > 0)
                {
                    validate = true;
                }

                if (Response.Orders[0].PickUp.Count > 0)
                {
                    validate = true;
                }
            }

            if (Response.HistoricalOrders.Count > 0)
            {
                validate = true;
            }

            return validate;
        }

        private void DrawHistoricalOrders()
        {

            LyHistorical.Visibility = ViewStates.Gone;
            LyViewLineHistory.Visibility = ViewStates.Gone;

            if (Response != null && Response.Orders != null && Response.HistoricalOrders.Any())
            {
                this.RunOnUiThread(() =>
                {

                    if (Response.HistoricalOrders != null && Response.HistoricalOrders.Count > 0)
                    {
                        TvQuantityHistorical.Text = Response.HistoricalOrders.Count().ToString();
                        historicalOrderAdapter = new HistoricalOrderAdapter(Response.HistoricalOrders, this, this);
                        RvHistoricalOrder.SetAdapter(historicalOrderAdapter);

                        LyHistorical.Visibility = ViewStates.Visible;
                        LyViewLineHistory.Visibility = ViewStates.Visible;
                    }
                });
            }
        }

        public void OnHistoricalOrderClicked(Order order)
        {
            Intent intent = new Intent(this, typeof(HistoricalOrderDetailActivity));
            intent.PutExtra(ConstantPreference.OrderId, order.Id);
            StartActivity(intent);
        }

        public void OnCurrentOrderClicked(Order order)
        {
            Intent intent = new Intent(this, typeof(OrderStatusActivity));
            string orderType = Response.Orders[0].HomeDelivery.IndexOf(order) > -1 ? AppMessages.HomeDelivery : AppMessages.Store;
            intent.PutExtra(ConstantPreference.OrderType, orderType);
            intent.PutExtra(ConstantPreference.Order, JsonService.Serialize(order));
            StartActivity(intent);
        }
    }
}