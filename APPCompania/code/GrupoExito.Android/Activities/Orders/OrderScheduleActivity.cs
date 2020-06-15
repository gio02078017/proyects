using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Payments;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Parameters.Orders;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Orders
{
    [Activity(Label = "Programación pedido", ScreenOrientation = ScreenOrientation.Portrait)]
    public class OrderScheduleActivity : BaseActivity, IScheduleHours
    {
        #region Controls

        private TextView TvTitleDeliver;
        private LinearLayout LySheduleSuccessful;
        private LinearLayout LyMessageDeliveryInStore;
        private TextView TvMessageDeliveryInStore;
        private LinearLayout LyExpressDelivery;
        private LinearLayout LyScheduleDelivery;
        private ImageView IvExpressDelivery;
        private ImageView IvScheduleDelivery;
        private TextView TvExpressDeliveryTitle;
        private TextView TvScheduleDeliveryTitle;
        private TextView TvExpressDeliveryMessagge;
        private TextView TvScheduleDeliveryMessagge;
        private LinearLayout LyExpressDeliveryPrice;
        private LinearLayout LyScheduleDeliveryPrice;
        private TextView TvExpressDeliveryPrice;
        private TextView TvScheduleDeliveryPrice;
        private LinearLayout LyMessageDeliverType;
        private LinearLayout LyDeliveryDays;
        private LinearLayout LyToday;
        private LinearLayout LyTomorrow;
        private LinearLayout LyAfterTomorrow;
        private TextView TvToday;
        private TextView TvDateToday;
        private TextView TvTomorrow;
        private TextView TvDateTomorrow;
        private TextView TvAfterTomorrow;
        private TextView TvDateAfterTomorrow;
        private LinearLayout LyMessagePrimeSchedule;
        private TextView TvMessagePrimeSchedule;
        private RecyclerView RvOrderSchedule;
        private ScheduleHoursAdapter ScheduleHoursAdapter;
        private LinearLayoutManager linerLayoutManager;
        private LinearLayout LyFailServices;
        private TextView TvCostToSend;
        private LinearLayout LyPickUp;
        private LinearLayout LyTimeStore;
        private LinearLayout LyDeliveryTypes;
        private TextView TvPickUpOn;
        private TextView TvTimeStore;
        private NestedScrollView NsvOrderSchedule;
        private LinearLayout LyDetails;
        private TextView TvMessage12Hours;
        private TextView TvSubTotalSummary;
        private TextView TvCostBagTitleSummary;
        private ImageView IvBoxTaxes;
        private LinearLayout LyButtonPay;
        private TextView TvDayProgramer;
        private TextView TvHourProgramer;

        #endregion

        #region Properties

        private OrderScheduleModel _orderScheduleModel;
        private ScheduleDays ScheduleDays;
        private bool ScheduledControl;
        private bool SelectedDayControl;
        private bool Store;
        private bool ExpressTypeSelected;
        private bool FirstTime = true;
        private OrderScheduleResponse _0rderScheduleResponse;
        private ScheduleContingencyResponse _scheduleContingencyResponse;

        #endregion

        #region Public Methods 

        public void OnSelectItemClicked(ScheduleHours scheduleHours, int position)
        {
            for (int i = 0; i < ScheduleDays.Hours.Count; i++)
            {
                if (i == position)
                {
                    ScheduleDays.Hours[i].Active = true;
                    SelectedDayControl = true;
                }
                else
                {
                    ScheduleDays.Hours[i].Active = false;
                }
            }

            ScheduleHoursAdapter.NotifyDataSetChanged();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        #endregion

        #region Protected Methods

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityOrderSchedule);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            _orderScheduleModel = new OrderScheduleModel(new OrderScheduleService(DeviceManager.Instance));
            ScheduledControl = false;
            SelectedDayControl = false;
            Store = ParametersManager.UserContext.Store != null;

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            this.SetSummaryCarItems();
            await this.GetOrderSchedule();            
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (FirstTime)
            {
                FirstTime = false;
            }
            else
            {
                HideProgressDialog();
            }
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.OrderSchedule, typeof(OrderScheduleActivity).Name);
        }

        protected override void EventError()
        {
            base.EventError();

            RunOnUiThread(async () =>
            {
                await this.GetOrderSchedule();
            });
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        #endregion

        #region Private Methods

        #region Paint View

        private void EditFonts()
        {
            TvMessageDeliveryInStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvToday.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDateToday.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDateTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAfterTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDateAfterTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvMessagePrimeSchedule.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleCostToSend).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessage12Hours).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvCostToSend.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitlePickUp).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPickUpOn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTimeStore.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvButtonPay).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvSubTotalSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvCostBagTitleSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            LyDetails = FindViewById<LinearLayout>(Resource.Id.lyDetail);
            SetSetSummaryControls(LyDetails.FindViewById<TextView>(Resource.Id.tvSubTotalPriceSummary),
                 LyDetails.FindViewById<TextView>(Resource.Id.tvCostBagPriceSummary));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
             FindViewById<RelativeLayout>(Resource.Id.rlBody), AppMessages.NotSearcherResultsTitle, AppMessages.NotSearcherResultsMessage);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
             FindViewById<RelativeLayout>(Resource.Id.rlBody));
            NsvOrderSchedule = FindViewById<NestedScrollView>(Resource.Id.nsvOrderSchedule);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            LySheduleSuccessful = FindViewById<LinearLayout>(Resource.Id.lySheduleSuccessful);
            TvTitleDeliver = FindViewById<TextView>(Resource.Id.tvTitleDeliver);
            LyMessageDeliveryInStore = FindViewById<LinearLayout>(Resource.Id.lyMessageDeliveryInStore);
            TvMessageDeliveryInStore = FindViewById<TextView>(Resource.Id.tvMessageDeliveryInStore);
            LyMessageDeliverType = FindViewById<LinearLayout>(Resource.Id.lyMessageDeliverType);
            LyDeliveryDays = FindViewById<LinearLayout>(Resource.Id.lyDeliveryDays);
            LyToday = FindViewById<LinearLayout>(Resource.Id.lyToday);
            LyTomorrow = FindViewById<LinearLayout>(Resource.Id.lyTomorrow);
            LyAfterTomorrow = FindViewById<LinearLayout>(Resource.Id.lyAfterTomorrow);
            LyDeliveryTypes = FindViewById<LinearLayout>(Resource.Id.lyDeliveryTypes);
            LyButtonPay = FindViewById<LinearLayout>(Resource.Id.lyButtonPay);
            IvBoxTaxes = LyDetails.FindViewById<ImageView>(Resource.Id.ivInformation);
            IvBoxTaxes.Click += delegate { OnBoxTaxesTouched(); };
            FindViewById<LinearLayout>(Resource.Id.lyCostBag).Click += delegate { OnBoxTaxesTouched(); };
            LyToday.Click += delegate { this.EventToday(); };
            LyTomorrow.Click += delegate { this.EventTomorrow(); };
            LyAfterTomorrow.Click += delegate { this.EventAfterTomorrow(); };
            LyButtonPay.Click += async delegate { await EventContinueWithYourPurchase(); };

            TvToday = FindViewById<TextView>(Resource.Id.tvToday);
            TvDateToday = FindViewById<TextView>(Resource.Id.tvDateToday);
            TvTomorrow = FindViewById<TextView>(Resource.Id.tvTomorrow);
            TvDateTomorrow = FindViewById<TextView>(Resource.Id.tvDateTomorrow);
            TvAfterTomorrow = FindViewById<TextView>(Resource.Id.tvAfterTomorrow);
            TvDateAfterTomorrow = FindViewById<TextView>(Resource.Id.tvDateAfterTomorrow);
            LyMessagePrimeSchedule = FindViewById<LinearLayout>(Resource.Id.lyMessagePrimeSchedule);
            TvMessagePrimeSchedule = FindViewById<TextView>(Resource.Id.tvMessagePrimeSchedule);
            TvMessagePrimeSchedule.Text = Resources.GetString(Resource.String.str_message_prime_schedule);
            RvOrderSchedule = FindViewById<RecyclerView>(Resource.Id.rvOrderSchedule);
            LyFailServices = FindViewById<LinearLayout>(Resource.Id.lyFailServices);
            TvCostToSend = FindViewById<TextView>(Resource.Id.tvCostToSend);
            LyPickUp = FindViewById<LinearLayout>(Resource.Id.lyPickUp);
            TvPickUpOn = FindViewById<TextView>(Resource.Id.tvPickUpOn);
            LyTimeStore = FindViewById<LinearLayout>(Resource.Id.lyTimeStore);
            TvTimeStore = FindViewById<TextView>(Resource.Id.tvTimeStore);
            TvMessage12Hours = FindViewById<TextView>(Resource.Id.tvMessage12Hours);
            TvSubTotalSummary = LyDetails.FindViewById<TextView>(Resource.Id.tvSubTotalSummary);
            TvCostBagTitleSummary = LyDetails.FindViewById<TextView>(Resource.Id.tvCostBagTitleSummary);
            this.PaintTypeDeliver();
            this.EventExpressDelivery();
        }

        private void PaintTypeDeliver()
        {
            LyExpressDelivery = FindViewById<LinearLayout>(Resource.Id.layoutExpressDelivery);
            IvExpressDelivery = LyExpressDelivery.FindViewById<ImageView>(Resource.Id.ivDeliverType);
            TvExpressDeliveryTitle = LyExpressDelivery.FindViewById<TextView>(Resource.Id.tvDeliverTitle);
            TvExpressDeliveryMessagge = LyExpressDelivery.FindViewById<TextView>(Resource.Id.tvDeliverMessagge);
            LyExpressDeliveryPrice = LyExpressDelivery.FindViewById<LinearLayout>(Resource.Id.lyDeliverPrice);
            TvExpressDeliveryPrice = LyExpressDelivery.FindViewById<TextView>(Resource.Id.tvDeliverPrice);
            LyExpressDelivery.Click += delegate { this.EventExpressDelivery(); };
            //IvExpressDelivery.SetImageResource(Resource.Drawable.express);

            TvDayProgramer = FindViewById<TextView>(Resource.Id.tvDayProgramer);
            TvHourProgramer = FindViewById<TextView>(Resource.Id.tvHourProgramer);
            LyScheduleDelivery = FindViewById<LinearLayout>(Resource.Id.layoutScheduleDelivery);
            IvScheduleDelivery = LyScheduleDelivery.FindViewById<ImageView>(Resource.Id.ivDeliverType);
            TvScheduleDeliveryTitle = LyScheduleDelivery.FindViewById<TextView>(Resource.Id.tvDeliverTitle);
            TvScheduleDeliveryMessagge = LyScheduleDelivery.FindViewById<TextView>(Resource.Id.tvDeliverMessagge);
            LyScheduleDeliveryPrice = LyScheduleDelivery.FindViewById<LinearLayout>(Resource.Id.lyDeliverPrice);
            TvScheduleDeliveryPrice = LyScheduleDelivery.FindViewById<TextView>(Resource.Id.tvDeliverPrice);
            LyScheduleDelivery.Click += delegate { this.EventScheduleDelivery(); };
            //IvScheduleDelivery.SetImageResource(Resource.Drawable.programada);
            TvExpressDeliveryTitle.Text = Resources.GetString(Resource.String.str_title_express_deliver);
            TvScheduleDeliveryTitle.Text = Resources.GetString(Resource.String.str_title_schedule_deliver);

            if (Store)
            {
                TvExpressDeliveryPrice.Text = AppMessages.FreeMessage;
                TvScheduleDeliveryPrice.Text = AppMessages.FreeMessage;
                TvExpressDeliveryMessagge.Text = Resources.GetString(Resource.String.str_message_express_deliver_store);
                TvScheduleDeliveryMessagge.Text = Resources.GetString(Resource.String.str_message_schedule_deliver_store);
                TvExpressDeliveryMessagge.SetLines(3);
                TvScheduleDeliveryMessagge.SetLines(3);
            }
            else
            {
                TvExpressDeliveryMessagge.Text = Resources.GetString(Resource.String.str_message_express_deliver);
                TvScheduleDeliveryMessagge.Text = Resources.GetString(Resource.String.str_message_schedule_deliver);
                TvScheduleDeliveryPrice.Text = Resources.GetString(Resource.String.str_variable_prime);
                TvExpressDeliveryMessagge.SetLines(3);
                TvScheduleDeliveryMessagge.SetLines(3);
            }

            this.EditFontsDelivery();
        }

        private void EditFontsDelivery()
        {
            TvTitleDeliver.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvExpressDeliveryTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvExpressDeliveryMessagge.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvExpressDeliveryPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvDayProgramer.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            TvHourProgramer.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            TvScheduleDeliveryTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvScheduleDeliveryMessagge.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvScheduleDeliveryPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void PaintToday(ScheduleDays days)
        {
            TvDateToday.Text = days.Description;
            string day = DateTime.Now.ToString("dd");
            day = day.TrimStart('0');
            string[] splitday = days.Description.Split(' ');

            if ((splitday != null) && splitday.Length > 0 && (day.Equals(splitday[1])))
            {
                TvToday.Visibility = ViewStates.Visible;
            }
            else
            {
                TvToday.Visibility = ViewStates.Gone;
            }
        }

        private void PaintTomorrow(ScheduleDays days)
        {
            TvDateTomorrow.Text = days.Description;
        }

        private void PaintAfterTomorrow(ScheduleDays days)
        {
            TvDateAfterTomorrow.Text = days.Description;
        }

        private void DrawScheduledResponse()
        {
            this.OnFailServices(false);

            int count = _0rderScheduleResponse.Schedules.Count;

            if (_0rderScheduleResponse.Schedules.Any())
            {
                if (count >= 1 && _0rderScheduleResponse.Schedules[0].Hours != null && _0rderScheduleResponse.Schedules[0].Hours.Any())
                {
                    this.PaintToday(_0rderScheduleResponse.Schedules[0]);
                }

                if (count >= 2 && _0rderScheduleResponse.Schedules[1].Hours != null && _0rderScheduleResponse.Schedules[1].Hours.Any())
                {
                    this.PaintTomorrow(_0rderScheduleResponse.Schedules[1]);
                }

                if (count >= 3 && _0rderScheduleResponse.Schedules[2].Hours != null && _0rderScheduleResponse.Schedules[2].Hours.Any())
                {
                    this.PaintAfterTomorrow(_0rderScheduleResponse.Schedules[2]);
                }

                this.EventToday();
                this.ScrollEnd(TvToday, 0);
            }
            else
            {
                this.OnFailServices(true);
            }
        }

        private void DrawExpressResponse()
        {
            if (!_0rderScheduleResponse.IsExpress)
            {
                LyDeliveryTypes.Visibility = ViewStates.Gone;
                this.EventScheduleDelivery();
            }

            HideProgressDialog();
        }

        private void PaintRvOrderSchedule(ScheduleDays scheduleDays)
        {
            ScheduleDays _scheduleDays = null;

            linerLayoutManager = new LinearLayoutManager(this)
            {
                AutoMeasureEnabled = true
            };

            RvOrderSchedule.NestedScrollingEnabled = false;
            RvOrderSchedule.HasFixedSize = true;
            RvOrderSchedule.SetLayoutManager(linerLayoutManager);
            _scheduleDays = CleanList(scheduleDays);
            _scheduleDays.Hours[0].Active = true;

            if (Store)
            {
                scheduleDays.Hours.ToList().ForEach(x => x.Store = true);
            }

            ScheduleHoursAdapter = new ScheduleHoursAdapter(_scheduleDays.Hours, this, this);
            RvOrderSchedule.SetAdapter(ScheduleHoursAdapter);

        }

        public ScheduleDays CleanList(ScheduleDays scheduleDays)
        {
            scheduleDays.Hours.ToList().ForEach(x => x.Active = false);
            return scheduleDays;
        }

        private void OnFailServices(bool failYes)
        {
            if (failYes)
            {
                LySheduleSuccessful.Visibility = ViewStates.Gone;
                LyFailServices.Visibility = ViewStates.Visible;
                TvMessage12Hours.Text = string.Format(AppMessages.ContingencyScheduleMessage,
                    _scheduleContingencyResponse.DateSelected,
                    _scheduleContingencyResponse.UserSchedule);
                TvCostToSend.Text = StringFormat.ToPrice(Convert.ToDecimal(_scheduleContingencyResponse.ShippingCost));
            }
            else
            {
                LyDetails.Visibility = ViewStates.Visible;
                LySheduleSuccessful.Visibility = ViewStates.Visible;
                LyFailServices.Visibility = ViewStates.Gone;
            }
        }

        private void ScrollEnd(View view, int start)
        {
            Handler handler = new Handler();

            void productAction()
            {
                NsvOrderSchedule.SmoothScrollingEnabled = true;
                int scrollTo = ((View)view.Parent.Parent).Top + view.Top + start;
                NsvOrderSchedule.SmoothScrollTo(0, scrollTo);
            }

            handler.PostDelayed(productAction, 200);
        }

        #endregion

        #region Events

        private void EventExpressDelivery()
        {
            this.SelectedExpressDelivery(true);
            this.SelectedScheduleDelivery(false);
            this.DrawItemsSchedule(false);
        }

        private void EventScheduleDelivery()
        {
            this.SelectedExpressDelivery(false);
            this.SelectedScheduleDelivery(true);
            this.DrawItemsSchedule(true);
        }

        public void SelectedExpressDelivery(bool select)
        {
            if (select)
            {
                IvExpressDelivery.SetImageResource(Resource.Drawable.express_secundario);
                LyExpressDelivery.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 10, new Color(ContextCompat.GetColor(this, Resource.Color.colorPrimary)));
                LyExpressDeliveryPrice.Background = ConvertUtilities.ChangeColorButtonDrawable(this,5, new Color(ContextCompat.GetColor(this, Resource.Color.colorGreenStrong)));
                TvExpressDeliveryTitle.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvExpressDeliveryMessagge.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvExpressDeliveryPrice.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
            }
            else
            {
                IvExpressDelivery.SetImageResource(Resource.Drawable.express);
                LyExpressDelivery.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 10, new Color(ContextCompat.GetColor(this, Resource.Color.colorGray)));
                LyExpressDeliveryPrice.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 5, new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvExpressDeliveryTitle.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvExpressDeliveryMessagge.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvExpressDeliveryPrice.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
            }
        }

        public void SelectedScheduleDelivery(bool select)
        {
            if (select)
            {
                IvScheduleDelivery.SetImageResource(Resource.Drawable.programada_secundario);
                LyScheduleDelivery.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 10, new Color(ContextCompat.GetColor(this, Resource.Color.colorPrimary)));
                LyScheduleDeliveryPrice.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 5, new Color(ContextCompat.GetColor(this, Resource.Color.colorGreenStrong)));
                TvScheduleDeliveryTitle.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvScheduleDeliveryMessagge.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvScheduleDeliveryPrice.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                LyMessageDeliverType.Visibility = ViewStates.Visible;
                LyDeliveryDays.Visibility = ViewStates.Visible;
                LyMessagePrimeSchedule.Visibility = ParametersManager.UserContext.Prime && !Store ? ViewStates.Visible : ViewStates.Gone;
                RvOrderSchedule.Visibility = ViewStates.Visible;
                ScheduledControl = true;
                TvMessageDeliveryInStore.Text = Store ? Resources.GetString(Resource.String.str_message_deliver_type_store)
                     : Resources.GetString(Resource.String.str_message_deliver_type);
            }
            else
            {
                IvScheduleDelivery.SetImageResource(Resource.Drawable.programada);
                LyMessageDeliverType.Visibility = ViewStates.Visible;
                LyMessagePrimeSchedule.Visibility = ParametersManager.UserContext.Prime && !Store ? ViewStates.Visible : ViewStates.Gone;
                LyScheduleDelivery.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 10, new Color(ContextCompat.GetColor(this, Resource.Color.colorGray)));
                LyScheduleDeliveryPrice.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 5, new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvScheduleDeliveryTitle.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvScheduleDeliveryMessagge.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvScheduleDeliveryPrice.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                LyDeliveryDays.Visibility = ViewStates.Gone;
                RvOrderSchedule.Visibility = ViewStates.Gone;
                ScheduledControl = false;
                LyTimeStore.Visibility = ViewStates.Gone;
                TvMessageDeliveryInStore.Text = Store ? Resources.GetString(Resource.String.str_message_deliver_type_store)
                    : Resources.GetString(Resource.String.str_message_deliver_type);
            }
        }

        private void DrawItemsSchedule(bool paint)
        {
            if (paint)
            {
                TvDayProgramer.Visibility = ViewStates.Visible;
                TvHourProgramer.Visibility = ViewStates.Visible;
            }
            else
            {
                TvDayProgramer.Visibility = ViewStates.Gone;
                TvHourProgramer.Visibility = ViewStates.Gone;
            }
        }

        private void EventToday()
        {
            this.SelectedToday(true);
            this.SelectedTomorrow(false);
            this.SelectedAfterTomorrow(false);
        }

        private void EventTomorrow()
        {
            this.SelectedToday(false);
            this.SelectedTomorrow(true);
            this.SelectedAfterTomorrow(false);
        }

        private void EventAfterTomorrow()
        {
            this.SelectedToday(false);
            this.SelectedTomorrow(false);
            this.SelectedAfterTomorrow(true);
        }

        public void SelectedToday(bool select)
        {
            if (select)
            {
                LyToday.SetBackgroundResource(Resource.Drawable.button_little_primary);
                TvToday.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvToday.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                TvDateToday.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvDateToday.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                this.SelectedDay(TvDateToday.Text);
            }
            else
            {
                LyToday.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 2, new Color(ContextCompat.GetColor(this, Resource.Color.colorGrayLight)));
                TvToday.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvToday.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvDateToday.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvDateToday.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            }
        }

        public void SelectedTomorrow(bool select)
        {
            if (select)
            {
                LyTomorrow.SetBackgroundResource(Resource.Drawable.button_little_primary);
                TvTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                TvDateTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvDateTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                this.SelectedDay(TvDateTomorrow.Text);
            }
            else
            {
                LyTomorrow.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 2, new Color(ContextCompat.GetColor(this, Resource.Color.colorGrayLight)));
                TvTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvDateTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvDateTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            }
        }

        public void SelectedAfterTomorrow(bool select)
        {
            if (select)
            {
                LyAfterTomorrow.SetBackgroundResource(Resource.Drawable.button_little_primary);
                TvAfterTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvAfterTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                TvDateAfterTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.white)));
                TvDateAfterTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                this.SelectedDay(TvDateAfterTomorrow.Text);
            }
            else
            {
                LyAfterTomorrow.Background = ConvertUtilities.ChangeColorButtonDrawable(this, 2, new Color(ContextCompat.GetColor(this, Resource.Color.colorGrayLight)));
                TvAfterTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvAfterTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvDateAfterTomorrow.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvDateAfterTomorrow.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            }
        }

        private void SelectedDay(string day)
        {
            SelectedDayControl = true;

            if (_0rderScheduleResponse.Schedules != null && _0rderScheduleResponse.Schedules.Any())
            {
                foreach (var item in _0rderScheduleResponse.Schedules)
                {
                    if (item.Description.Equals(day))
                    {
                        ScheduleDays = item;
                        PaintRvOrderSchedule(ScheduleDays);
                    }
                }
            }
        }


        #endregion

        #region Get information

        private async Task GetOrderSchedule()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                bool selectedStore = ParametersManager.UserContext.Store != null ? true : false;
                string dependencyId = ParametersManager.UserContext.DependencyId;

                OrderScheduleParameters parameters = new OrderScheduleParameters
                {
                    DependencyId = dependencyId.Length == 2 ? "0" + dependencyId : dependencyId,
                    IdCity = selectedStore ? ParametersManager.UserContext.Store.IdPickup
                    : ParametersManager.UserContext.Address.IdPickup,
                    DeliveryMode = selectedStore ? ConstDeliveryMode.Pe : ConstDeliveryMode.Do,
                    QuantityUnits = ParametersManager.Order.TotalProducts
                };

                _0rderScheduleResponse = await _orderScheduleModel.GetOrderSchedule(parameters);
                this.ResponseGetOrderSchedule();
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.OrderScheduleActivity, ConstantMethodName.GetOrderSchedule } };
                HideProgressDialog();
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void ResponseGetOrderSchedule()
        {
            this.RunOnUiThread(async () =>
            {
                if (_0rderScheduleResponse.Result != null && _0rderScheduleResponse.Result.HasErrors && _0rderScheduleResponse.Result.Messages != null)
                {
                    HideProgressDialog();

                    var errorCode = (EnumErrorCode)Enum.Parse(typeof(EnumErrorCode), _0rderScheduleResponse.Result.Messages.First().Code);

                    if (errorCode == EnumErrorCode.AwsServiceUnavailable)
                    {
                        await GetScheduleContingency();
                    }
                    else
                    {
                        this.DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_0rderScheduleResponse.Result), AppMessages.AcceptButtonText);
                    }
                }
                else
                {
                    if (Store)
                    {
                        TvExpressDeliveryMessagge.Text = string.Format(AppMessages.ExpressDeliverStoreMessage, _0rderScheduleResponse.MinutesPromiseDelivery.ToString());
                        TvScheduleDeliveryMessagge.Text = Resources.GetString(Resource.String.str_message_schedule_deliver_store);
                    }
                    else
                    {
                        TvExpressDeliveryMessagge.Text = string.Format(AppMessages.PromiseDeliveryText, _0rderScheduleResponse.MinutesPromiseDelivery.ToString());
                        TvExpressDeliveryPrice.Text = !string.IsNullOrEmpty(_0rderScheduleResponse.PricePromise) ? "$" + Convert.ToDecimal(_0rderScheduleResponse.PricePromise) : string.Empty;
                    }

                    this.DrawScheduledResponse();
                    this.DrawExpressResponse();
                    ShowBodyLayout();
                }
            });
        }

        public async Task EventContinueWithYourPurchase()
        {
            if (ParametersManager.Order.Contingency)
            {
                GoToPay();
            }
            else
            {
                if (ScheduledControl)
                {
                    ExpressTypeSelected = false;

                    if (SelectedDayControl)
                    {
                        if (!ParametersManager.Order.Contingency)
                        {
                            await this.ScheduleReservation();
                        }
                        else
                        {
                            GoToPay();
                        }
                    }
                    else
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.ScheduleMessage, AppMessages.AcceptButtonText);
                    }
                }
                else
                {
                    ExpressTypeSelected = true;
                    this.SetParameters();
                    GoToPay();
                }
            }
        }

        private void GoToPay()
        {
            Intent intent = new Intent(this, typeof(PaymentActivity));
            StartActivity(intent);
        }

        private async Task ScheduleReservation()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                ScheduleReservationParameters parameters = this.SetParameters();

                var response = await _orderScheduleModel.ScheduleReservation(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        SaveAndrContinueEvent();

                        if (response.Code == (int)EnumErrorCode.ScheduleSuccessReservation)
                        {
                            Intent intent = new Intent(this, typeof(PaymentActivity));
                            StartActivity(intent);
                        }
                        else
                        {
                            DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.ScheduleReservationErroMessage, AppMessages.AcceptButtonText);
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.OrderScheduleActivity, ConstantMethodName.ScheduleReservation } };
                HideProgressDialog();
                ShowAndRegisterMessageExceptions(exception, properties);
                HideProgressDialog();
            }
        }

        private ScheduleReservationParameters SetParameters()
        {
            var order = this.SetScheduleOrder();
            bool selectedStore = ParametersManager.UserContext.Store != null ? true : false;
            string dependencyId = ParametersManager.UserContext.DependencyId;

            ScheduleReservationParameters parameters = new ScheduleReservationParameters
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

            return parameters;
        }

        private Order SetScheduleOrder()
        {
            var order = ParametersManager.Order;

            if (order != null)
            {
                order.TypeModality = ExpressTypeSelected ? ConstTypeModality.Express : ConstTypeModality.ScheduledPickup;
                order.Contingency = false;

                if (!ExpressTypeSelected)
                {
                    ScheduleHours ScheduleHour = ScheduleDays.Hours.Where(x => x.Active).FirstOrDefault();
                    order.shippingCost = string.IsNullOrEmpty(ScheduleHour.ShippingCostValue) ? "0" : ScheduleHour.ShippingCostValue;
                    order.DateSelected = ScheduleDays.Description;
                    order.Schedule = ScheduleHour.Shedule;
                    order.PluDispatch = ScheduleHour.ShippingCostPlu;
                }
                else
                {
                    order.PluDispatch = _0rderScheduleResponse.ShippingCostPlu;
                    order.shippingCost = _0rderScheduleResponse.PricePromise;
                    order.MinutesPromiseDelivery = _0rderScheduleResponse.MinutesPromiseDelivery;

                    order.DateSelected = string.Empty;
                    order.Schedule = string.Empty;
                }
            }

            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            return order;
        }

        private async Task GetScheduleContingency()
        {
            try
            {
                ProductCarModel _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
                List<Product> products = _productCarModel.GetProducts();

                ScheduleContingencyParameters parameters = new ScheduleContingencyParameters
                {
                    Quantity = products != null && products.Any() ? products.Count() : 0
                };

                _scheduleContingencyResponse = await _orderScheduleModel.GetScheduleContingency(parameters);

                if (_scheduleContingencyResponse.Result != null && _scheduleContingencyResponse.Result.HasErrors
                    && _scheduleContingencyResponse.Result.Messages != null)
                {
                    this.DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(_scheduleContingencyResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    Order order = ParametersManager.Order;
                    order.shippingCost = _scheduleContingencyResponse.ShippingCost;
                    order.TypeOfDispatch = _scheduleContingencyResponse.TypeShippingGroup;
                    order.TypeDispatch = _scheduleContingencyResponse.TypeDispatch;
                    order.PluDispatch = _scheduleContingencyResponse.PluDispatch;
                    order.Schedule = _scheduleContingencyResponse.UserSchedule;
                    order.DateSelected = _scheduleContingencyResponse.DateSelected;
                    order.Contingency = true;
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));

                    this.OnFailServices(true);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                     { ConstantActivityName.OrderScheduleActivity, ConstantMethodName.GetScheduleContingency } };
                HideProgressDialog();
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        public void SaveAndrContinueEvent()
        {
            FirebaseRegistrationEventsService.Instance.Schedule();
        }

        #endregion

        #endregion
    }
}