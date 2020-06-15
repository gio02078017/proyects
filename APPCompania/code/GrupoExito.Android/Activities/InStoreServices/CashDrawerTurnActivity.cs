using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using droid = Android;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Turno en Caja", ScreenOrientation = ScreenOrientation.Portrait)]
    [IntentFilter(new[] { "cash_drawer" },
    Categories = new[] { droid.Content.Intent.CategoryDefault })]
    public class CashDrawerTurnActivity : BaseActivity
    {
        #region Controls

        private LinearLayout LyAskYouTurn;
        private TextView TvTurnInBox;
        private TextView TvMessageTurnInBox;
        private TextView TvStore;
        private TextView TvMessageAvailable;
        private Spinner SpStore;
        private Button BtnAskCashDrawerTurn;
        private LinearLayout LyStateYouTurn;
        private LinearLayout LyJoinWait;
        private LinearLayout LyTurnInWait;
        private LinearLayout LyAverageOfWait;
        private TextView TvTitleItemTurnWait;        
        private TextView TvItemTurnWait;
        private TextView TvTitleItemTurnAverage;
        private TextView TvItemTurnAverage;
        private ImageView IvItemTurnWait;
        private ImageView IvItemTurnAverage;
        private View viewDivide;
        private AlertDialog AcceptTurnDialog;
        private View viewAcceptTurn;
        private TextView TvNumberTurn;
        private TextView TvCashDrawerNumber;
        private TextView TvMessageTurn;
        private TextView TvCancelTurn, TvAskOtherTurn;
        private LinearLayout LyTurnState;
        private LinearLayout LyTurnFinished;
        private LinearLayout LyJoinWaitDialog;
        private LinearLayout lyTurnInWaitDialog;
        private LinearLayout LyAverageOfWaitDialog;
        private TextView TvTitleItemTurnWaitDialog;
        private TextView TvItemTurnWaitDialog;
        private TextView TvTitleItemTurnAverageDialog;
        private TextView TvItemTurnAverageDialog;
        private TextView TvTurnReady;
        private ImageView IvItemTurnWaitDialog;
        private ImageView IvItemTurnAverageDialog;
        private LinearLayout LyReject;
        private LinearLayout LyAccept;
        private LinearLayout LyTurnReady;
        private LinearLayout LyWaitingTurn;
        private TextView TvReject;
        private TextView TvAccept;
        #endregion

        #region Properties

        private IList<City> CitiesStore { get; set; }
        private IList<StoreCashDrawerTurn> Stores { get; set; }
        private CashDrawerTurnModel CashDrawerTurnModel;
        private StatusCashDrawerTurnResponse StatusCashDrawerTurnResponse;
        private TicketResponse TicketResponse;
        private Timer TicketTimer;
        private int IntRefreshTimer;
        private bool ControlRefreshTimer;
        private bool ControlStartTimer;

        #endregion

        private void ItemSelectedStore(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (SpStore.SelectedItemPosition > 0)
            {
                BtnAskCashDrawerTurn.Enabled = true;
                BtnAskCashDrawerTurn.SetBackgroundResource(Resource.Drawable.button_yellow);
            }
            else
            {
                BtnAskCashDrawerTurn.Enabled = false;
                BtnAskCashDrawerTurn.SetBackgroundResource(Resource.Drawable.button_gray);
            }
        }

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityCashDrawerTurn);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            CashDrawerTurnModel = new CashDrawerTurnModel(new CashDrawerTurnService(DeviceManager.Instance));
            HideItemsCarToolbar(this);
            Stores = new List<StoreCashDrawerTurn>();

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            await this.ValidateTickets();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.CashDrawerRequestTurn, typeof(CashDrawerTurnActivity).Name);
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            StopTicketTimer();
            Finish();
        }

        public void RefreshData()
        {
            if (!string.IsNullOrEmpty(DeviceManager.Instance.GetAccessPreference(ConstantPreference.TicketId)))
            {
                Refresh();
            }
        }

        protected override void Refresh()
        {
            base.Refresh();

            RunOnUiThread(async () =>
            {
                await GetCurrentTicketStatus();
            });
        }

        protected async override void EventError()
        {
            base.EventError();
            await this.ValidateTickets();
        }

        private async Task ValidateTickets()
        {
            string value = DeviceManager.Instance.GetAccessPreference(ConstantPreference.TicketId);

            if (!string.IsNullOrEmpty(value))
            {
                await GetCurrentTicketStatus();
            }
            else
            {
                if (!DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.TokenCreated, false) && await GetMobileId())
                {
                    if(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.TokenRefreshed, false))
                    {
                        await UpdateDevice();
                    }
                    else
                    {
                        await GetStores();
                    }
                }
                else
                {
                    await RegisterDevice();
                }
            }
        }

        private async Task RegisterDevice()
        {
            try
            {
                string firebaseToken = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken);
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                AppDevice appDevice = new AppDevice
                {
                    DeviceId = DeviceManager.Instance.GetDeviceId(),
                    TokenNotificationPush = !string.IsNullOrEmpty(firebaseToken) ? firebaseToken : Guid.NewGuid().ToString("D")
                };

                var response = await CashDrawerTurnModel.Device(appDevice);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    var errorCode = (EnumErrorCode)Enum.Parse(typeof(EnumErrorCode), response.Result.Messages.First().Code);

                    if(errorCode == EnumErrorCode.InvalidExternalResponse)
                    {
                        await UpdateDevice();
                    }
                    else
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    }
                }
                else
                {
                    DeviceManager.Instance.SaveAccessPreference(ConstantPreference.MobileId, response.MobileId);
                    await GetStores();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.RegisterDevice } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task UpdateDeviceTicket()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                string firebaseToken = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken);

                AppDevice appDevice = new AppDevice
                {
                    DeviceId = DeviceManager.Instance.GetDeviceId(),
                    TokenNotificationPush = !string.IsNullOrEmpty(firebaseToken) ? firebaseToken : Guid.NewGuid().ToString("D")
                };

                await CashDrawerTurnModel.UpdateDevice(appDevice);
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.RegisterDevice } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private async Task UpdateDevice()
        {
            try
            {
                string firebaseToken = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken);
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                AppDevice appDevice = new AppDevice
                {
                    DeviceId = DeviceManager.Instance.GetDeviceId(),
                    TokenNotificationPush = !string.IsNullOrEmpty(firebaseToken) ? firebaseToken : Guid.NewGuid().ToString("D")
                };

                var response = await CashDrawerTurnModel.UpdateDevice(appDevice);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    if (response.Success)
                    {
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.TokenRefreshed , false);
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.TokenCreated , false);
                        await GetMobileId();
                        await GetStores();
                    }
                    else
                    {
                        await RegisterDevice();
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.RegisterDevice } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        protected override void EventYesGenericDialog()
        {
            GenericDialog.Hide();

            this.RunOnUiThread(async () =>
            {
                await CancelTicket();
            });
        }

        protected override void EventNotGenericDialog()
        {
            base.EventNotGenericDialog();
            GenericDialog.Hide();
        }

        private async Task GetStores()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await CashDrawerTurnModel.GetStores();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    Stores = response.Stores;
                    List<string> storeNames = new List<string>();

                    foreach (var store in response.Stores)
                    {
                        storeNames.Add(store.Name);
                    }

                    ArrayAdapter adapter = new ArrayAdapter(this, Resource.Layout.spinner_layout, storeNames);
                    SpStore.Adapter = adapter;
                    ShowBodyLayout();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.GetStores } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task<bool> GetMobileId()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                string firebaseToken = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken);

                AppDevice appDevice = new AppDevice
                {
                    DeviceId = DeviceManager.Instance.GetDeviceId(),
                    TokenNotificationPush = !string.IsNullOrEmpty(firebaseToken) ? firebaseToken : Guid.NewGuid().ToString("D")
                };

                var response = await CashDrawerTurnModel.GetMobileId(appDevice);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    return false;
                }
                else
                {
                    DeviceManager.Instance.SaveAccessPreference(ConstantPreference.MobileId, response.MobileId);
                    return true;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.GetMobileId } };
                ShowAndRegisterMessageExceptions(exception, properties);
                return false;
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task GetTurnStatus()
        {
            try
            {
                DeviceManager.Instance.SaveAccessPreference(ConstantPreference.SlotDisplayName, "");
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                StatusCashDrawerTurn parameters = new StatusCashDrawerTurn
                {
                    StoreId = Stores[SpStore.SelectedItemPosition].Id
                };

                StatusCashDrawerTurnResponse = await CashDrawerTurnModel.StatusCashDrawerTurn(parameters);

                if (StatusCashDrawerTurnResponse.Result != null && StatusCashDrawerTurnResponse.Result.HasErrors && StatusCashDrawerTurnResponse.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(StatusCashDrawerTurnResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    ShowDialog();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.GetTurnStatus } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task RequestTicket()
        {
            try
            {
                Ticket parameters = new Ticket
                {
                    MobileId = DeviceManager.Instance.GetAccessPreference(ConstantPreference.MobileId),
                    StoreId = Stores[SpStore.SelectedItemPosition].Id
                };

                TicketResponse = await CashDrawerTurnModel.Ticket(parameters);

                if (TicketResponse.Result != null && TicketResponse.Result.HasErrors && TicketResponse.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(TicketResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    DeviceManager.Instance.SaveAccessPreference(ConstantPreference.TicketId, TicketResponse.Id);
                    await GetCurrentTicketStatus();
                    RegisterCashDrawerTurn();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.RequestTicket } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task GetCurrentTicketStatus()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                Ticket parameters = new Ticket
                {
                    Id = DeviceManager.Instance.GetAccessPreference(ConstantPreference.TicketId)
                };

                TicketResponse = await CashDrawerTurnModel.StatusTicket(parameters);

                if (TicketResponse.Result != null && TicketResponse.Result.HasErrors && TicketResponse.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(TicketResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    TvCashDrawerNumber.Text = string.IsNullOrEmpty(TicketResponse.SlotDisplayName) ? 
                        DeviceManager.Instance.GetAccessPreference(ConstantPreference.SlotDisplayName) : TicketResponse.SlotDisplayName;

                    if (string.IsNullOrEmpty(TvCashDrawerNumber.Text))
                    {
                        FindViewById<TextView>(Resource.Id.tvTitleCashDrawer).Visibility = ViewStates.Gone;
                        TvCashDrawerNumber.Visibility = ViewStates.Gone;
                    }
                    else
                    {
                        FindViewById<TextView>(Resource.Id.tvTitleCashDrawer).Visibility = ViewStates.Visible;
                        TvCashDrawerNumber.Visibility = ViewStates.Visible;
                    }

                    if (!ControlStartTimer)
                    {
                        StartTicketTimer();
                    }
                    
                    LyAskYouTurn.Visibility = ViewStates.Gone;
                    LyStateYouTurn.Visibility = ViewStates.Visible;

                    if ( TicketResponse.Status == ConstStatusTicket.AutoDumped || TicketResponse.Status == ConstStatusTicket.Dumped || TicketResponse.Status == ConstStatusTicket.Finished)
                    {
                        this.TurnFinished();
                    }
                    else if (TicketResponse.Status == ConstStatusTicket.Waiting)
                    {
                        this.TurnStatus();
                    }
                    else
                    {
                        this.TurnReady();
                    }

                    await GetStores();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.RequestTicket } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private async Task CancelTicket()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                Ticket parameters = new Ticket
                {
                    Id = DeviceManager.Instance.GetAccessPreference(ConstantPreference.TicketId)
                };

                TicketResponse = await CashDrawerTurnModel.CancelTicket(parameters);

                if (TicketResponse.Result != null && TicketResponse.Result.HasErrors && TicketResponse.Result.Messages != null)
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(TicketResponse.Result), AppMessages.AcceptButtonText);
                }
                else
                {
                    this.RequestOtherTurn();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.CashDrawerTurnActivity, ConstantMethodName.CancelTicket } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTurnInBox).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageTurnInBox).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvStore).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvMessageAvailable.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.btnAskTurnInBox).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            FindViewById<TextView>(Resource.Id.tvTitleYouTurn).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageYouTurn).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            FindViewById<TextView>(Resource.Id.tvTitleYou).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleTurn).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvNumberTurn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvTitleCashDrawer).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvCashDrawerNumber).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            FindViewById<TextView>(Resource.Id.tvTitleTurnFinishedA).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleTurnFinishedB).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitleTurnFinishedC).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAttended).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);

            FindViewById<TextView>(Resource.Id.tvTitleNote).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageNote).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvMessageTurn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvTurnReady.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            TvTitleItemTurnWait.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvItemTurnWait.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvTitleItemTurnAverage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvItemTurnAverage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            TvCancelTurn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvAskOtherTurn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                 FindViewById<RelativeLayout>(Resource.Id.rlCashDrawer));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
            };

            LyAskYouTurn = FindViewById<LinearLayout>(Resource.Id.lyAskYouTurn);
            LyWaitingTurn = FindViewById<LinearLayout>(Resource.Id.lyWaitingTurn);
            LyTurnReady = FindViewById<LinearLayout>(Resource.Id.lyTurnReady);
            TvTurnInBox = FindViewById<TextView>(Resource.Id.tvTurnInBox);
            TvTurnReady = FindViewById<TextView>(Resource.Id.tvTurnReady);

            TvMessageTurnInBox = FindViewById<TextView>(Resource.Id.tvMessageTurnInBox);
            TvStore = FindViewById<TextView>(Resource.Id.tvStore);
            SpStore = FindViewById<Spinner>(Resource.Id.spStore);
            TvMessageAvailable = FindViewById<TextView>(Resource.Id.tvMessageAvailable);
            BtnAskCashDrawerTurn = FindViewById<Button>(Resource.Id.btnAskTurnInBox);
            BtnAskCashDrawerTurn.Enabled = false;
            BtnAskCashDrawerTurn.SetBackgroundResource(Resource.Drawable.button_transparent);
            BtnAskCashDrawerTurn.Click += async delegate { await this.GetTurnStatus(); };

            LyStateYouTurn = FindViewById<LinearLayout>(Resource.Id.lyStateYouTurn);
            TvNumberTurn = FindViewById<TextView>(Resource.Id.tvNumberTurn);
            TvCashDrawerNumber = FindViewById<TextView>(Resource.Id.tvCashDrawerNumber);
            TvMessageTurn = FindViewById<TextView>(Resource.Id.tvMessageTurn);

            LyJoinWait = FindViewById<LinearLayout>(Resource.Id.lyJoinWait);
            viewDivide = LyJoinWait.FindViewById<View>(Resource.Id.viewDivide);
            LyTurnInWait = LyJoinWait.FindViewById<LinearLayout>(Resource.Id.lyTurnInWait);
            TvTitleItemTurnWait = LyTurnInWait.FindViewById<TextView>(Resource.Id.tvTitleItemTurn);
            IvItemTurnWait = LyTurnInWait.FindViewById<ImageView>(Resource.Id.ivItemTurn);
            TvItemTurnWait = LyTurnInWait.FindViewById<TextView>(Resource.Id.tvItemTurn);
            LyAverageOfWait = LyJoinWait.FindViewById<LinearLayout>(Resource.Id.lyAverageOfWait);
            TvTitleItemTurnAverage = LyAverageOfWait.FindViewById<TextView>(Resource.Id.tvTitleItemTurn);
            IvItemTurnAverage = LyAverageOfWait.FindViewById<ImageView>(Resource.Id.ivItemTurn);
            TvItemTurnAverage = LyAverageOfWait.FindViewById<TextView>(Resource.Id.tvItemTurn);

            LyTurnState = FindViewById<LinearLayout>(Resource.Id.lyTurnState);
            LyTurnFinished = FindViewById<LinearLayout>(Resource.Id.lyTurnFinished);

            TvCancelTurn = FindViewById<TextView>(Resource.Id.tvCancelTurn);
            TvAskOtherTurn = FindViewById<TextView>(Resource.Id.tvAskOtherTurn);
            TvAskOtherTurn.Click += delegate { this.RequestOtherTurn(); };
            TvCancelTurn.Click += delegate { this.EventCancelTicket(); };
            SpStore.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ItemSelectedStore);
        }

        private void EventCancelTicket()
        {
            DataDialog dataDialog = new DataDialog(AppMessages.TitleGenericDialog, AppMessages.CancelTicket);
            ShowGenericDialogDialog(dataDialog);
        }

        private void TurnStatus()
        {
            RunOnUiThread(() =>
            {
                LyTurnState.Visibility = ViewStates.Visible;
                LyTurnFinished.Visibility = ViewStates.Gone;
                TvCancelTurn.Visibility = ViewStates.Visible;
                TvAskOtherTurn.Visibility = ViewStates.Gone;
                LyTurnReady.Visibility = ViewStates.Gone;
                LyWaitingTurn.Visibility = ViewStates.Visible;

                viewDivide.SetBackgroundResource(Resource.Color.colorDescription);
                TvTitleItemTurnWait.Text = Resources.GetString(Resource.String.str_turn_in_wait);
                TvTitleItemTurnWait.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                IvItemTurnWait.SetImageResource(Resource.Drawable.personas_turno);
                TvItemTurnWait.Text = TicketResponse.TicketInFront.ToString();
                TvNumberTurn.Text = TicketResponse.Name;
                FindViewById<TextView>(Resource.Id.tvMessageNote).Text = AppMessages.TurnStatusNote;

                TvItemTurnWait.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvTitleItemTurnWait.SetTextSize(ComplexUnitType.Sp, 12);
                TvItemTurnWait.SetTextSize(ComplexUnitType.Sp, 13);
                TvTitleItemTurnAverage.Text = Resources.GetString(Resource.String.str_average_of_wait);
                TvTitleItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                IvItemTurnAverage.SetImageResource(Resource.Drawable.tiempo_espera);
                TvItemTurnAverage.Text = TicketResponse.WaitEstimate.ToString() + " " + AppMessages.Minutes;
                TvItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvTitleItemTurnAverage.SetTextSize(ComplexUnitType.Sp, 12);
                TvItemTurnAverage.SetTextSize(ComplexUnitType.Sp, 12);
                SetRefreshVisible(true);
            });
        }

        private void TurnFinished()
        {
            RunOnUiThread(() =>
            {
                LyTurnState.Visibility = ViewStates.Gone;
                LyTurnFinished.Visibility = ViewStates.Visible;
                TvCancelTurn.Visibility = ViewStates.Gone;
                TvAskOtherTurn.Visibility = ViewStates.Visible;
                LyTurnReady.Visibility = ViewStates.Gone;
                LyWaitingTurn.Visibility = ViewStates.Gone;

                viewDivide.SetBackgroundResource(Resource.Color.colorDescription);

                TvTitleItemTurnWait.Text = Resources.GetString(Resource.String.str_turn_in_wait);
                TvTitleItemTurnWait.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorDescription)));
                IvItemTurnWait.SetImageResource(Resource.Drawable.personas_turno_primario);
                TvItemTurnWait.Text = TicketResponse.WaitingTickets.ToString();
                TvItemTurnWait.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorDescription)));


                TvTitleItemTurnWait.SetTextSize(ComplexUnitType.Sp, 12);
                TvItemTurnWait.SetTextSize(ComplexUnitType.Sp, 14);

                TvTitleItemTurnAverage.Text = Resources.GetString(Resource.String.str_average_of_wait);
                TvTitleItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorDescription)));
                IvItemTurnAverage.SetImageResource(Resource.Drawable.tiempo_espera_primario);
                TvItemTurnAverage.Text = AproxTime(TicketResponse.AvgWaitServ, TicketResponse.AvgWaitTime);
                TvItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorDescription)));

                TvTitleItemTurnAverage.SetTextSize(ComplexUnitType.Sp, 12);
                TvItemTurnAverage.SetTextSize(ComplexUnitType.Sp, 12);
            });
        }

        private void TurnReady()
        {
            RunOnUiThread(() =>
            {
                LyTurnState.Visibility = ViewStates.Visible;
                LyTurnFinished.Visibility = ViewStates.Gone;
                TvCancelTurn.Visibility = ViewStates.Gone;
                TvAskOtherTurn.Visibility = ViewStates.Gone;
                LyWaitingTurn.Visibility = ViewStates.Gone;
                LyTurnReady.Visibility = ViewStates.Visible;

                FindViewById<TextView>(Resource.Id.tvMessageNote).Text = AppMessages.TurnWaitingTime;
                viewDivide.SetBackgroundResource(Resource.Color.colorDescription);
                TvTitleItemTurnWait.Text = Resources.GetString(Resource.String.str_turn_in_wait);
                TvTitleItemTurnWait.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                IvItemTurnWait.SetImageResource(Resource.Drawable.personas_turno);
                TvItemTurnWait.Text = TicketResponse.TicketInFront.ToString();
                TvNumberTurn.Text = TicketResponse.Name;
                TvItemTurnWait.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvTitleItemTurnWait.SetTextSize(ComplexUnitType.Sp, 12);
                TvItemTurnWait.SetTextSize(ComplexUnitType.Sp, 13);
                TvTitleItemTurnAverage.Text = Resources.GetString(Resource.String.str_average_of_wait);
                TvTitleItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                IvItemTurnAverage.SetImageResource(Resource.Drawable.tiempo_espera);
                TvItemTurnAverage.Text = TicketResponse.WaitEstimate.ToString() + " " + AppMessages.Minutes;
                TvItemTurnAverage.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));
                TvTitleItemTurnAverage.SetTextSize(ComplexUnitType.Sp, 12);
                TvItemTurnAverage.SetTextSize(ComplexUnitType.Sp, 12);
                SetRefreshVisible(true);
            });
        }

        private void RequestOtherTurn()
        {
            RunOnUiThread(() =>
            {
                StopTicketTimer();
                DeviceManager.Instance.SaveAccessPreference(ConstantPreference.TicketId, "");
                LyAskYouTurn.Visibility = ViewStates.Visible;
                LyStateYouTurn.Visibility = ViewStates.Gone;
                SetRefreshVisible(false);
            });
        }

        private void ShowDialog()
        {
            RunOnUiThread(() =>
            {
                AcceptTurnDialog = new AlertDialog.Builder(this).Create();
                viewAcceptTurn = LayoutInflater.Inflate(Resource.Layout.DialogAcceptTurn, null);
                AcceptTurnDialog.SetView(viewAcceptTurn);
                AcceptTurnDialog.SetCanceledOnTouchOutside(false);

                LyJoinWaitDialog = viewAcceptTurn.FindViewById<LinearLayout>(Resource.Id.lyJoinWait);
                LyJoinWaitDialog.SetBackgroundResource(Resource.Drawable.propuesta_turno);

                lyTurnInWaitDialog = LyJoinWaitDialog.FindViewById<LinearLayout>(Resource.Id.lyTurnInWait);
                TvTitleItemTurnWaitDialog = lyTurnInWaitDialog.FindViewById<TextView>(Resource.Id.tvTitleItemTurn);
                IvItemTurnWaitDialog = lyTurnInWaitDialog.FindViewById<ImageView>(Resource.Id.ivItemTurn);
                TvItemTurnWaitDialog = lyTurnInWaitDialog.FindViewById<TextView>(Resource.Id.tvItemTurn);

                LyAverageOfWaitDialog = LyJoinWaitDialog.FindViewById<LinearLayout>(Resource.Id.lyAverageOfWait);
                TvTitleItemTurnAverageDialog = LyAverageOfWaitDialog.FindViewById<TextView>(Resource.Id.tvTitleItemTurn);
                IvItemTurnAverageDialog = LyAverageOfWaitDialog.FindViewById<ImageView>(Resource.Id.ivItemTurn);
                TvItemTurnAverageDialog = LyAverageOfWaitDialog.FindViewById<TextView>(Resource.Id.tvItemTurn);

                TvTitleItemTurnWaitDialog.Text = Resources.GetString(Resource.String.str_turn_in_wait);
                IvItemTurnWaitDialog.SetImageResource(Resource.Drawable.personas_turno);
                TvItemTurnWaitDialog.Text = StatusCashDrawerTurnResponse.WaitingTickets.ToString();
                TvItemTurnWaitDialog.SetTextSize(ComplexUnitType.Sp, 13);

                TvTitleItemTurnAverageDialog.Text = Resources.GetString(Resource.String.str_average_of_wait);
                IvItemTurnAverageDialog.SetImageResource(Resource.Drawable.tiempo_espera);
                TvItemTurnAverageDialog.Text = AproxTime(StatusCashDrawerTurnResponse.AvgWaitServ, StatusCashDrawerTurnResponse.AvgWaitTime);

                TvTitleItemTurnWaitDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvItemTurnWaitDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

                TvTitleItemTurnAverageDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
                TvItemTurnAverageDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

                LyReject = viewAcceptTurn.FindViewById<LinearLayout>(Resource.Id.lyReject);
                TvReject = viewAcceptTurn.FindViewById<TextView>(Resource.Id.tvReject);
                TvReject.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

                LyAccept = viewAcceptTurn.FindViewById<LinearLayout>(Resource.Id.lyAccept);
                TvAccept = viewAcceptTurn.FindViewById<TextView>(Resource.Id.tvAccept);
                TvAccept.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

                LyAccept.Click += async delegate
                {
                    AcceptTurnDialog.Hide();
                    await UpdateDeviceTicket();
                    await RequestTicket();
                };

                LyReject.Click += delegate { AcceptTurnDialog.Hide(); };

                AcceptTurnDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
                AcceptTurnDialog.Show();

                RegisterModalCashDrawerTurn();
            });
        }

        private void StartTicketTimer()
        {
            if (TicketTimer != null && TicketTimer.Enabled)
            {
                 TicketTimer.Stop();
                 TicketTimer.Elapsed -= new ElapsedEventHandler(EventTicketTimer);
            }
            else
            {
                TicketTimer = new Timer();
            }

            if(IntRefreshTimer == 0)
            {
                if (!ControlStartTimer)
                {
                    ControlRefreshTimer = false;
                    ControlStartTimer = true;
                }
                else
                {
                    ControlRefreshTimer = true;
                    //TvMessageTurn.Text = string.Format(AppMessages.UpdatingTimerMessage, IntRefreshTimer);
                    MessageTimer(IntRefreshTimer);
                }
                
                IntRefreshTimer = int.Parse(AppConfigurations.TicketTimerTime);
            }
            else
            {
                //TvMessageTurn.Text = string.Format(AppMessages.UpdatingTimerMessage, IntRefreshTimer);
                MessageTimer(IntRefreshTimer);
                IntRefreshTimer -= 1;
            }

            TicketTimer.Elapsed += new ElapsedEventHandler(EventTicketTimer);
            TicketTimer.Interval = 1000;
            TicketTimer.Enabled = true;
        }

        private void EventTicketTimer(object source, ElapsedEventArgs e)
        {
            this.RunOnUiThread(async () =>
            {
                if (ControlRefreshTimer)
                {
                    ControlRefreshTimer = false;
                    await GetCurrentTicketStatus();                    
                }
                else
                {
                    StartTicketTimer();
                }
                
            });
        }

        private void StopTicketTimer()
        {
            if (TicketTimer != null)
            {
                TicketTimer.Enabled = false;
                TicketTimer.Stop();
            }
        }

        private string AproxTime(decimal avgWaitServ, decimal avgWaitTime)
        {
            return AppMessages.Between + " " + avgWaitServ.ToString()
                  + " " + AppMessages.And + " " + avgWaitTime.ToString() + " " +
                  AppMessages.Minutes;
        }

        public void MessageTimer(int time)
        {
            string message = string.Format(AppMessages.UpdatingTimerMessage, time);
            CustomFonts(TvMessageTurn, 35, message.Length, message);
        }

        private void CustomFonts(TextView textView, int textStart, int TextEnd, string text)
        {
            SpannableStringBuilder strfont = new SpannableStringBuilder(text);
            strfont.SetSpan(new StyleSpan(TypefaceStyle.Bold), textStart, TextEnd, SpanTypes.ExclusiveExclusive);
            textView.TextFormatted = strfont;
        }

        private void RegisterModalCashDrawerTurn()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ModalRequestTurn, typeof(CashDrawerTurnActivity).Name);
        }

        private void RegisterCashDrawerTurn()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.CashDrawerTurn, typeof(CashDrawerTurnActivity).Name);
        }
    }
}