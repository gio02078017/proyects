using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.OS;
using Android.Provider;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Activities.Payments;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Activities.Recipes;
using GrupoExito.Android.Activities.Users;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Products;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Generic;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Microsoft.AppCenter.Crashes;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asv7 = Android.Support.V7.App;
using ProviderSettings = Android.Provider;

namespace GrupoExito.Android.Activities.Base
{
    public abstract class BaseActivity : AppCompatActivity
    {
        #region Properties   

        private View viewGenericDialog;
        public View ViewGenericDialog
        {
            get { return viewGenericDialog; }
            set { viewGenericDialog = value; }
        }

        private ImageView ivToolbarBack;
        public ImageView IvToolbarBack
        {
            get { return ivToolbarBack; }
            set { ivToolbarBack = value; }
        }

        private ImageView ivError;
        public ImageView IvError
        {
            get { return ivError; }
            set { ivError = value; }
        }

        private ImageView ivMyAccount;
        public ImageView IvMyAccount
        {
            get { return ivMyAccount; }
            set { ivMyAccount = value; }
        }

        private ImageView ivRefresh;
        public ImageView IvRefresh
        {
            get { return ivRefresh; }
            set { ivRefresh = value; }
        }

        private TextView tvToolbarPrice;
        public TextView TvToolbarPrice
        {
            get { return tvToolbarPrice; }
            set { tvToolbarPrice = value; }
        }

        private TextView tvSubTotalPriceSummary;
        public TextView TvSubTotalPriceSummary
        {
            get { return tvSubTotalPriceSummary; }
            set { tvSubTotalPriceSummary = value; }
        }

        private TextView tvCostBagPriceSummary;
        public TextView TvCostBagPriceSummary
        {
            get { return tvCostBagPriceSummary; }
            set { tvCostBagPriceSummary = value; }
        }

        private TextView tvToolbarQuantity;
        public TextView TvToolbarQuantity
        {
            get { return tvToolbarQuantity; }
            set { tvToolbarQuantity = value; }
        }

        private TextView tvToolbarName;
        public TextView TvToolbarName
        {
            get { return tvToolbarName; }
            set { tvToolbarName = value; }
        }

        private ImageView ivLoader;
        public ImageView IvLoader
        {
            get { return ivLoader; }
            set { ivLoader = value; }
        }

        private Asv7.AlertDialog genericDialog;
        public Asv7.AlertDialog GenericDialog
        {
            get { return genericDialog; }
            set { genericDialog = value; }
        }

        private Asv7.AlertDialog boxTaxesDialog;
        public Asv7.AlertDialog BoxTaxesDialog
        {
            get { return boxTaxesDialog; }
            set { boxTaxesDialog = value; }
        }

        private Asv7.AlertDialog messageInfoDialog;
        public Asv7.AlertDialog MessageInfoDialog
        {
            get { return messageInfoDialog; }
            set { messageInfoDialog = value; }
        }

        private Button btnYesDialog;
        public Button BtnYesDialog
        {
            get { return btnYesDialog; }
            set { btnYesDialog = value; }
        }

        private Button btnNotDialog;
        public Button BtnNotDialog
        {
            get { return btnNotDialog; }
            set { btnNotDialog = value; }
        }

        private Button btnNoInfo;
        public Button BtnNoInfo
        {
            get { return btnNoInfo; }
            set { btnNoInfo = value; }
        }

        private Button btnError;
        public Button BtnError
        {
            get { return btnError; }
            set { btnError = value; }
        }

        private ViewGroup viewBody;
        public ViewGroup ViewBody
        {
            get { return viewBody; }
            set { viewBody = value; }
        }

        private TextView tvTitleDialog;
        public TextView TvTitleDialog
        {
            get { return tvTitleDialog; }
            set { tvTitleDialog = value; }
        }

        private TextView tvMessageDialog;
        public TextView TvMessageDialog
        {
            get { return tvMessageDialog; }
            set { tvMessageDialog = value; }
        }

        private TextView tvNoInfo;
        public TextView TvNoInfo
        {
            get { return tvNoInfo; }
            set { tvNoInfo = value; }
        }

        private TextView tvError;
        public TextView TvError
        {
            get { return tvError; }
            set { tvError = value; }
        }

        private RelativeLayout rlCar;
        public RelativeLayout RlCar
        {
            get { return rlCar; }
            set { rlCar = value; }
        }

        private RelativeLayout rlLoader;
        public RelativeLayout RlLoader
        {
            get { return rlLoader; }
            set { rlLoader = value; }
        }

        private RelativeLayout rlNoInfo;
        public RelativeLayout RlNoInfo
        {
            get { return rlNoInfo; }
            set { rlNoInfo = value; }
        }

        private RelativeLayout rlError;
        public RelativeLayout RlError
        {
            get { return rlError; }
            set { rlError = value; }
        }

        private LinearLayout lySearcher;
        public LinearLayout LySearcher
        {
            get { return lySearcher; }
            set { lySearcher = value; }
        }

        private LinearLayout lyRefresh;
        public LinearLayout LyRefresh
        {
            get { return lyRefresh; }
            set { lyRefresh = value; }
        }

        private bool enableGpsScreen = false;
        public bool EnableGpsScreen
        {
            get { return enableGpsScreen; }
            set { enableGpsScreen = value; }
        }

        private AutoCompleteTextView actvAddress;
        public AutoCompleteTextView ActvAddress
        {
            get { return actvAddress; }
            set { actvAddress = value; }
        }

        private SmsMessageService broadcastService;
        public SmsMessageService BroadcastService
        {
            get { return broadcastService; }
            set { broadcastService = value; }
        }

        #endregion

        #region Private Properties 

        private OrderModel OrderModel { get; set; }
        private AddressModel AddressModel { get; set; }
        private readonly string[] permissionsLocation = { Manifest.Permission.AccessCoarseLocation, Manifest.Permission.AccessFineLocation };
        private const int RequestLocationId = 0;
        private bool LocationPermission { get; set; }
        private bool SentLocation { get; set; }
        private bool MediumPermissionBaseActivity { get; set; }
        private const int RequestSmsnId = 1;

        #endregion

        #region Cities

        public async Task<IList<City>> LoadCitiesAddressesAndProgressDialog()
        {
            ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
            return await GetCitiesAddresses();
        }

        public async Task<IList<City>> LoadCitiesAddresses()
        {
            return await GetCitiesAddresses();
        }

        private async Task<IList<City>> GetCitiesAddresses()
        {
            IList<City> cities = new List<City>();

            try
            {
                CitiesFilter parameters = new CitiesFilter() { HomeDelivery = "true", Pickup = "false" };
                var response = await AddressModel.GetCities(parameters);

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
                    ShowBodyLayout();
                    cities = response.Cities;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.WelcomeActivity, ConstantMethodName.GetCities } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return cities;
        }

        #endregion

        #region Document Types

        public async Task<DocumentTypeResponse> LoadDocumentsType()
        {
            DocumentTypeResponse response = null;

            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                DocumentTypesModel _model = new DocumentTypesModel(new DocumentTypesService(DeviceManager.Instance));
                response = await _model.GetDocumentTypes();
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseActivity, ConstantMethodName.GetDocumentTypes } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return response;
        }

        #endregion

        #region Tags Notifications

        public void RegisterNotificationTags()
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    NotificationHubService notificationHubService = new NotificationHubService(this);
                    notificationHubService.RegisterNotification();
                }
                catch (Exception exception)
                {
                    Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseActivity, ConstantMethodName.RegisterNotificationTags } };
                    RegisterMessageExceptions(exception, properties);
                }
            })

            {
                IsBackground = true
            };
            thread.Start();
        }

        #endregion

        #region Public Methods          

        public void DeletePreferencesLastDateUpdatedPromotion()
        {
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.LastDateUpdated, "");
        }

        public override void Finish()
        {
            base.Finish();
            OverridePendingTransitionExit();
        }

        public override void StartActivity(Intent intent)
        {
            base.StartActivity(intent);
            OverridePendingTransitionEnter();
        }

        public Toolbar GetMainToolbar(int id, Activity activity)
        {
            RlCar = activity.FindViewById<RelativeLayout>(Resource.Id.rlCar);
            TvToolbarPrice = activity.FindViewById<TextView>(Resource.Id.tvToolbarPrice);
            IvMyAccount = activity.FindViewById<ImageView>(Resource.Id.ivMyAccount);
            IvRefresh = activity.FindViewById<ImageView>(Resource.Id.ivRefresh);
            RlCar.Click += delegate { GoToSummary(activity); };
            TvToolbarPrice.Click += delegate { GoToSummary(activity); };
            IvMyAccount.Click += delegate { GoToMyAccount(activity); };
            IvRefresh.Click += delegate { Refresh(); };
            LyRefresh = activity.FindViewById<LinearLayout>(Resource.Id.lyRefresh);
            TvToolbarName = activity.FindViewById<TextView>(Resource.Id.tvToolbarName);
            TvToolbarName.Text = ParametersManager.UserContext != null ? StringFormat.Capitalize(StringFormat.SplitName(ParametersManager.UserContext.FirstName)) : string.Empty;
            TvToolbarName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvToolbarName.Visibility = ParametersManager.UserContext != null ? ViewStates.Visible : ViewStates.Gone;
            return activity.FindViewById<Toolbar>(id);
        }

        public void UpdateToolbarName()
        {
            TvToolbarName.Text = ParametersManager.UserContext != null ? StringFormat.Capitalize(StringFormat.SplitName(ParametersManager.UserContext.FirstName)) : string.Empty;
        }

        public void HideItemsCarToolbar(Activity activity)
        {
            RlCar = activity.FindViewById<RelativeLayout>(Resource.Id.rlCar);
            RlCar.Visibility = ViewStates.Invisible;
        }

        public void HideItemsToolbar(Activity activity)
        {
            RlCar = activity.FindViewById<RelativeLayout>(Resource.Id.rlCar);
            RlCar.Visibility = ViewStates.Invisible;
            IvMyAccount = activity.FindViewById<ImageView>(Resource.Id.ivMyAccount);
            activity.FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Gone;
            IvMyAccount.Visibility = ViewStates.Invisible;
        }

        public void ShowProgressDialog(string title, string message)
        {
            if (IvLoader != null && IvLoader.Visibility == ViewStates.Gone)
            {
                IvLoader.Visibility = ViewStates.Visible;
                RlLoader.Visibility = ViewStates.Visible;
                ((AnimationDrawable)IvLoader.Background).Start();
            }
        }

        public void HideProgressDialog()
        {
            RunOnUiThread(() =>
            {
                if (IvLoader != null)
                {
                    IvLoader.Visibility = ViewStates.Gone;
                    RlLoader.Visibility = ViewStates.Gone;

                    if (IvLoader.Background != null)
                    {
                        ((AnimationDrawable)IvLoader.Background).Stop();
                    }
                }
            });
        }

        public void DefineShowErrorWay(string title, string message, string buttonText, bool alertWay = false)
        {
            try
            {
                RunOnUiThread(() =>
                {
                    if (RlError != null && !alertWay)
                    {
                        ShowErrorLayout(message);
                    }
                    else
                    {
                        if (!IsFinishing)
                        {
                            var dlgAlert = (new Asv7.AlertDialog.Builder(this));
                            dlgAlert.SetTitle(title);
                            dlgAlert.SetMessage(message);
                            dlgAlert.SetPositiveButton(buttonText, HandlerOkButton);
                            dlgAlert.Show();
                        }
                    }

                    RegisterError();
                });
            }
            catch
            {
            }
        }

        public void ShowAndRegisterMessageExceptions(Exception exception, Dictionary<string, string> properties)
        {
            var st = new StackTrace(exception, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            properties.Add(ConstantActivityName.LineError, line.ToString());
            Crashes.TrackError(exception, properties);
            DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
        }

        public void RegisterMessageExceptions(Exception exception, Dictionary<string, string> properties)
        {
            var st = new StackTrace(exception, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            properties.Add(ConstantActivityName.LineError, line.ToString());
            Crashes.TrackError(exception, properties);
        }

        public void SetToolbarCarItems(bool hiddeItemsToolbar = false)
        {
            RunOnUiThread(() =>
            {
                var productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
                var summary = productCarModel.GetSummary();

                if (summary != null)
                {
                    TvToolbarPrice.Visibility = ViewStates.Visible;
                    TvToolbarPrice.Text = StringFormat.ToPrice(decimal.Parse(summary[ConstDataBase.TotalPrice].ToString()));
                    TvToolbarQuantity.Text = (summary[ConstDataBase.ProductQuantity]).ToString();

                    if (TvSubTotalPriceSummary != null && TvCostBagPriceSummary != null)
                    {
                        TvSubTotalPriceSummary.Text = StringFormat.ToPrice(decimal.Parse(summary[ConstDataBase.TotalPrice].ToString()));
                        TvCostBagPriceSummary.Text = StringFormat.ToPrice(decimal.Parse(summary[ConstDataBase.TotalTaxBag].ToString()));
                    }
                }
                else
                {
                    TvToolbarPrice.Text = "$0";
                    TvToolbarQuantity.Text = "0";

                    if (TvSubTotalPriceSummary != null && TvCostBagPriceSummary != null)
                    {
                        TvSubTotalPriceSummary.Text = string.Empty;
                        TvCostBagPriceSummary.Text = string.Empty;
                    }
                }

                TvToolbarPrice.Visibility = hiddeItemsToolbar ? ViewStates.Invisible : ViewStates.Visible;
                TvToolbarQuantity.Visibility = hiddeItemsToolbar ? ViewStates.Invisible : ViewStates.Visible;

            });
        }

        public void SetSummaryCarItems()
        {
            RunOnUiThread(() =>
            {
                var productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
                var summary = productCarModel.GetSummary();

                if (summary != null)
                {
                    if (TvSubTotalPriceSummary != null && TvCostBagPriceSummary != null)
                    {
                        TvSubTotalPriceSummary.Text = StringFormat.ToPrice(decimal.Parse(summary[ConstDataBase.TotalPrice].ToString()));
                        TvCostBagPriceSummary.Text = StringFormat.ToPrice(decimal.Parse(summary[ConstDataBase.TotalTaxBag].ToString()));
                    }
                }
                else
                {
                    if (TvSubTotalPriceSummary != null && TvCostBagPriceSummary != null)
                    {
                        TvSubTotalPriceSummary.Text = string.Empty;
                        TvCostBagPriceSummary.Text = string.Empty;
                    }
                }
            });
        }

        public void SetSetSummaryControls(TextView tvSubTotalPriceSummary, TextView tvCostBagPriceSummary)
        {
            TvSubTotalPriceSummary = tvSubTotalPriceSummary;
            TvCostBagPriceSummary = tvCostBagPriceSummary;
            TvSubTotalPriceSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvCostBagPriceSummary.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        public void SetToolbarControls(TextView tvToobarPrice, TextView tvToolbarQuantity)
        {
            TvToolbarPrice = tvToobarPrice;
            TvToolbarQuantity = tvToolbarQuantity;
            TvToolbarPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvToolbarQuantity.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        public void SetSearcher(LinearLayout lySearcher, bool CallProductsActivity = false)
        {
            this.lySearcher = lySearcher;
            this.lySearcher.FindViewById<LinearLayout>(Resource.Id.lyScan).Click += delegate { CallMyRecipes(); };
            this.lySearcher.Click += delegate { CallSearcherActivity(CallProductsActivity); };
            this.lySearcher.FindViewById<TextView>(Resource.Id.tvSearcher).Click += delegate { CallSearcherActivity(CallProductsActivity); };
            
        }

        public void DefineSearcherVisibility(bool visible)
        {
            if (lySearcher != null)
            {
                lySearcher.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
            }
        }

        public void SetRefreshVisible(bool visible)
        {
            LyRefresh.Visibility = visible ? ViewStates.Visible : ViewStates.Gone;
        }

        public void SetLoader(RelativeLayout rlLoader)
        {
            if (rlLoader != null)
            {
                IvLoader = rlLoader.FindViewById<ImageView>(Resource.Id.ivLoader);
                RlLoader = rlLoader;
            }
        }

        public void SetNoInfoLayout(RelativeLayout rlNoInfo, ViewGroup lyBody, string message, string buttonText)
        {
            TvNoInfo = rlNoInfo.FindViewById<TextView>(Resource.Id.tvNoInfo);
            BtnNoInfo = rlNoInfo.FindViewById<Button>(Resource.Id.btnNoInfo);
            RlNoInfo = rlNoInfo;
            ViewBody = lyBody;
            TvNoInfo.Text = message;
            BtnNoInfo.Text = buttonText;
            BtnNoInfo.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvNoInfo.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            if (!BtnNoInfo.HasOnClickListeners)
            {
                BtnNoInfo.Click += delegate { EventNoInfo(); };
            }
        }

        public void SetErrorLayout(RelativeLayout rlError, ViewGroup lyBody)
        {
            TvError = rlError.FindViewById<TextView>(Resource.Id.tvError);
            BtnError = rlError.FindViewById<Button>(Resource.Id.btnError);
            IvError = rlError.FindViewById<ImageView>(Resource.Id.ivError);
            RlError = rlError;
            BtnError.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvError.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnError.Click += delegate { EventError(); };
            ViewBody = lyBody;
        }

        public void ShowBodyLayout()
        {
            RunOnUiThread(() =>
            {
                ViewBody.Visibility = ViewStates.Visible;

                if (RlNoInfo != null)
                {
                    RlNoInfo.Visibility = ViewStates.Gone;
                }

                if (RlError != null)
                {
                    RlError.Visibility = ViewStates.Gone;
                }
            });
        }

        public void ShowNoInfoLayout(bool WithOutAction = false)
        {
            RunOnUiThread(() =>
            {
                ViewBody.Visibility = ViewStates.Gone;
                RlNoInfo.Visibility = ViewStates.Visible;

                if (WithOutAction)
                {
                    ViewBody.Visibility = ViewStates.Visible;
                    BtnNoInfo.Visibility = ViewStates.Gone;
                }
            });
        }

        public void ShowNoInfoLayout(string message, bool WithOutAction = false)
        {
            RunOnUiThread(() =>
            {
                ViewBody.Visibility = ViewStates.Gone;
                RlNoInfo.Visibility = ViewStates.Visible;
                TvNoInfo.Text = message;

                if (WithOutAction)
                {
                    ViewBody.Visibility = ViewStates.Visible;
                    BtnNoInfo.Visibility = ViewStates.Gone;
                }
            });
        }

        public void ShowErrorLayout(string message = "", int resource = 0)
        {
            ViewBody.Visibility = ViewStates.Gone;

            if (RlNoInfo != null)
            {
                RlNoInfo.Visibility = ViewStates.Gone;
            }

            RlError.Visibility = ViewStates.Visible;

            TvError.Text = string.IsNullOrEmpty(message) ? TvError.Text : message;

            if (resource != 0)
            {
                IvError.SetImageResource(resource);
            }
        }

        public void ShowGenericDialogDialog(DataDialog dataDialog)
        {
            GenericDialog = new Asv7.AlertDialog.Builder(this).Create();
            viewGenericDialog = LayoutInflater.Inflate(Resource.Layout.DialogMessageGeneric, null);
            GenericDialog.SetView(viewGenericDialog);
            GenericDialog.SetCanceledOnTouchOutside(false);
            TvTitleDialog = viewGenericDialog.FindViewById<TextView>(Resource.Id.tvTitleDialog);
            TvMessageDialog = viewGenericDialog.FindViewById<TextView>(Resource.Id.tvMessageDialog);
            TvTitleDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvMessageDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvTitleDialog.Text = dataDialog.TitleDialog;
            TvMessageDialog.Text = dataDialog.MensajeDialog;
            BtnYesDialog = viewGenericDialog.FindViewById<Button>(Resource.Id.btnYesDialog);
            BtnNotDialog = viewGenericDialog.FindViewById<Button>(Resource.Id.btnNotDialog);

            if (dataDialog.ButtonYesName != null)
            {
                BtnYesDialog.Text = dataDialog.ButtonYesName;
            }

            if (dataDialog.ButtonNotName != null)
            {
                BtnNotDialog.Text = dataDialog.ButtonNotName;
            }

            BtnYesDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnNotDialog.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            BtnYesDialog.Click += delegate { EventYesGenericDialog(); };
            BtnNotDialog.Click += delegate { EventNotGenericDialog(); };
            GenericDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            GenericDialog.Show();
        }

        public void OnBoxTaxesTouched()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogTaxesInfo, null);
            BoxTaxesDialog = new Asv7.AlertDialog.Builder(this).Create();
            BoxTaxesDialog.SetView(view);
            BoxTaxesDialog.SetCanceledOnTouchOutside(true);
            Button btnOk = view.FindViewById<Button>(Resource.Id.btnOk);
            TextView tvBoxTaxes = view.FindViewById<TextView>(Resource.Id.tvBoxTaxes);
            TextView tvBoxTaxesMessage = view.FindViewById<TextView>(Resource.Id.tvBoxTaxesMessage);
            ImageView ivClose = view.FindViewById<ImageView>(Resource.Id.ivClose);
            btnOk.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            tvBoxTaxes.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            tvBoxTaxesMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            btnOk.Click += delegate { BoxTaxesDialog.Cancel(); };
            ivClose.Click += delegate { BoxTaxesDialog.Cancel(); };
            BoxTaxesDialog.Window.SetBackgroundDrawableResource(Resource.Color.colorTransparent);
            BoxTaxesDialog.Show();
        }

        public void OnBoxMessageTouched(string message, string btnMessage)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogMessageInfo, null);
            MessageInfoDialog = new Asv7.AlertDialog.Builder(this).Create();
            MessageInfoDialog.SetView(view);
            MessageInfoDialog.SetCanceledOnTouchOutside(true);
            Button btnOk = view.FindViewById<Button>(Resource.Id.btnOk);
            TextView TvBoxMessageInfo = view.FindViewById<TextView>(Resource.Id.tvBoxMessageInfo);
            TvBoxMessageInfo.Text = message;
            btnOk.Text = btnMessage;
            ImageView ivClose = view.FindViewById<ImageView>(Resource.Id.ivClose);
            btnOk.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvBoxMessageInfo.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            btnOk.Click += delegate { EventCloseModalMessageInfo(); };
            ivClose.Click += delegate { MessageInfoDialog.Cancel(); };
            MessageInfoDialog.Window.SetBackgroundDrawableResource(Resource.Color.colorTransparent);
            MessageInfoDialog.Show();
        }

        public string CostSend(Order order)
        {
            if (order.shippingCost.Equals(AppMessages.FreeMessage) ||
                order.shippingCost.Equals("0") || string.IsNullOrEmpty(order.shippingCost))
            {
                return AppMessages.FreeMessage;
            }

            return StringFormat.ToPrice(decimal.Parse(order.shippingCost));
        }

        #endregion

        #region Register costumer clifre

        public async Task RegisterCostumer()
        {
            try
            {
                if (ParametersManager.UserContext != null && !ParametersManager.UserContext.IsAnonymous)
                {
                    bool userActivateClifre = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.UserActivateClifre, false);
                    if (!userActivateClifre)
                    {
                        UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                        RegisterCostumerParameters parameters = ModelHelper.RegisterCostumerParameters(ParametersManager.UserContext);
                        var response = await userModel.RegisterCostumer(parameters);

                        if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                        {
                            //Show message
                        }
                        else
                        {
                            if (response.Activate)
                            {
                                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.UserActivateClifre, response.Activate);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SplashActivity, ConstantMethodName.RegisterCostumer } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        #endregion

        #region Geolocation Methods

        public bool GetLocationPermission()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                return ValidateGpsAvailability();
            }
            else
            {
                RequestPermissions(permissionsLocation, RequestLocationId);
                return false;
            }
        }

        public bool ValidateGpsAvailability()
        {
            LocationManager manager = (LocationManager)GetSystemService(LocationService);

            if (manager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                return true;
            }
            else
            {
                ShowGpsDialog();
                return false;
            }
        }

        public async Task GetLocationAsync()
        {
            Position positionSend = null;

            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                CancellationTokenSource ctsrc = new CancellationTokenSource(2000);
                var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(20000));
                positionSend = position;
                this.SendLocation(positionSend);

            }
            catch (Exception e)
            {
                if (positionSend != null && !SentLocation)
                {
                    this.SendLocation(positionSend);
                }
            }
        }

        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults.Any() && grantResults[0] == Permission.Granted)
                        {
                            await GetLocationAsync();

                            if (MediumPermissionBaseActivity)
                            {
                                CallPriceCheckerActivity();
                            }
                            else
                            {
                                RetryRequestPermissions(true);
                            }
                        }
                    }
                    break;
                case RequestSmsnId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {
                            LocalBroadcastManager.GetInstance(this).RegisterReceiver(BroadcastService, new IntentFilter(Telephony.Sms.Intents.SmsReceivedAction));
                        }
                    }
                    break;
            }
        }

        public virtual void RetryRequestPermissions(bool Retried)
        {
        }

        protected virtual void SendLocation(Position position)
        {
            if (position != null)
            {
                SentLocation = true;
            }
        }

        public void ShowGpsDialog()
        {
            try
            {
                if (!IsFinishing)
                {
                    Asv7.AlertDialog.Builder builder = new Asv7.AlertDialog.Builder(this);
                    builder.SetMessage(AppMessages.GpsDisable)
                            .SetCancelable(false)
                            .SetPositiveButton(AppMessages.AcceptButtonText, delegate
                            {
                                EnableGpsScreen = true;
                                StartActivity(new Intent(ProviderSettings.Settings.ActionLocationSourceSettings));
                            })
                            .SetNegativeButton(AppMessages.CancelButtonText, (EventHandler<DialogClickEventArgs>)null);
                    Asv7.AlertDialog alert = builder.Create();
                    Button cancelButton = alert.GetButton((int)DialogButtonType.Negative);
                    alert.Show();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseAddressesActivity, ConstantMethodName.ShowGpsDialog } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void GoToSummary()
        {
        }

        protected override void OnDestroy()
        {
            if (IvLoader != null)
            {
                IvLoader.SetImageBitmap(null);
                IvLoader.Background = null;
            }

            base.OnDestroy();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.SetSoftInputMode(SoftInput.AdjustResize);
            AddressModel = new AddressModel(new AddressService(DeviceManager.Instance));
            OrderModel = new OrderModel(new OrderService(DeviceManager.Instance));
        }

        protected override void OnResume()
        {
            base.OnResume();
            ParametersManager.IsInBackground = false;
            AndroidApplication.CurrentActivity = this;
        }

        protected override void OnPause()
        {
            base.OnPause();
            ParametersManager.IsInBackground = true;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected virtual void EventNotGenericDialog()
        {
            GenericDialog.Hide();
        }

        protected virtual void EventYesGenericDialog()
        {
            GenericDialog.Hide();
        }

        protected virtual void EventCloseModalMessageInfo()
        {
            MessageInfoDialog.Hide();
        }

        protected virtual void EventNoInfo()
        {
        }

        protected virtual void EventError()
        {
        }

        protected virtual void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
        }

        protected virtual void Refresh()
        {
        }

        protected virtual void RefreshHeaderSummary()
        {
        }

        protected void OverridePendingTransitionEnter()
        {
            OverridePendingTransition(Resource.Animation.slide_from_right, Resource.Animation.slide_to_left);
        }

        protected void OverridePendingTransitionExit()
        {
            OverridePendingTransition(Resource.Animation.slide_from_left, Resource.Animation.slide_to_right);
        }

        protected void HideKeyBoard()
        {
            View view = this.CurrentFocus;

            if (view != null)
            {
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }

        #endregion

        #region Private Methods

        public void CallSearcherActivity(bool CallProductsActivity)
        {
            var activityName = this.GetType().Name;
            Intent intent = new Intent(this, typeof(SearcherActivity));
            StartActivity(intent);

            if (CallProductsActivity)
            {
                Finish();
            }
        }

        private void CallMyRecipes()
        {
            Intent intent = new Intent(this, typeof(MyRecipesActivity));
            StartActivity(intent);
        }

        private void CallPriceCheckerActivity()
        {
            var activityName = this.GetType().Name;
            bool permission = GetLocationPermission();
            MediumPermissionBaseActivity = true;

            if (permission)
            {
                Intent intent = new Intent(this, typeof(PriceCheckerLocationActivity));
                StartActivity(intent);
            }
        }

        private void GoToSummary(Activity activity)
        {
            if (activity.GetType() != typeof(SummaryActivity))
            {
                var activityName = this.GetType().Name;
                Intent intent = new Intent(this, typeof(SummaryActivity));
                StartActivity(intent);
            }
        }

        private void GoToMyAccount(Activity activity)
        {
            if (activity.GetType() != typeof(MyAccountActivity))
            {
                var activityName = this.GetType().Name;
                Intent intent = new Intent(this, typeof(MyAccountActivity));
                StartActivity(intent);
            }
        }

        private void RegisterError()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Error, typeof(BaseActivity).Name);
        }

        #endregion

    }
}