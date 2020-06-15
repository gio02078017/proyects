using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Android.Support.V7.Widget.RecyclerView;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Mis Descuentos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyDiscountsActivity : BaseVerifyUser, IDiscount, IOnItemTouchListener, GestureDetector.IOnGestureListener, ISmsListener
    {
        #region Controls

        private LinearLayout LyKillers;
        private LinearLayout LyAlreadyPurchased;
        private LinearLayout LyCouldLike;
        private LinearLayout LyCouponMania;
        private RecyclerView RvKillers;
        private RecyclerView RvAlreadyPurchased;
        private RecyclerView RvCouldLike;
        private RecyclerView RvCouponMania;
        private MyDiscountsAdapter KillersAdapter;
        private MyDiscountsAdapter AlreadyPurchasedAdapter;
        private MyDiscountsAdapter CouldLikeAdapter;
        private MyDiscountsAdapter CouponManiaAdapter;
        private ImageView IvKillersEndArrow;
        private ImageView IvAlreadyPurchasedEndArrow;
        private ImageView IvCouldLikeEndArrow;
        private ImageView IvCouponManiaEndArrow;
        private ImageView IvKillersStartArrow;
        private ImageView IvAlreadyPurchasedStartArrow;
        private ImageView IvCouldLikeStartArrow;
        private ImageView IvCouponManiaStartArrow;
        private ImageView ivCouponManiaTitle;
        private TextView TvMaxDiscounts;
        private GestureDetector GestureDetector;
        private AlertDialog TermsDialog;
        private LinearLayout LyErrorDiscount;
        private LinearLayout LySuccessfulDiscount;
        private LinearLayout LySuccessfulDiscountItems;
        private LinearLayout LyActivate;
        private LinearLayout LyActivated;
        private LinearLayout LyRedeemed;
        private LinearLayout LyInsideActivate;
        private LinearLayout LyInsideActivated;
        private LinearLayout LyInsideRedeemed;
        private TextView TvRangeDateDiscount;
        private TextView TvMessagePreView;
        private TextView TvRangeDateDiscountPreview;
        private TextView TvNumberActivate;
        private TextView TvForActive;
        private TextView TvNumberActivated;
        private TextView TvActived;
        private View viewActivated;
        private TextView TvNumberRedeemed;
        private TextView TvRedeemed;
        private LinearLayout LyInformationActivated;
        private TextView TvTitleDiscountActivated;
        private RecyclerView RvMyDiscountActivated;
        private MyDiscountsDetailAdapter MyDiscountActivatedAdapter;
        private LinearLayout LyInformationRedeemed;
        private TextView TvTitleDiscountRedeemed;
        private RecyclerView RvMyDiscountRedeemed;
        private MyDiscountsDetailAdapter MyDiscountRedeemedAdapter;
        private LinearLayout LyPreViewDiscount;
        private LinearLayout LyViewDiscount;
        private TextView TvMessageError;
        private ImageView IvErrorDiscount;
        private LinearLayout LyTypeDiscounts;
        private LinearLayout LyKillersTitle;
        private TextView TvKillersTitle;
        private LinearLayout LyAlreadyPurchasedTitle;
        private TextView TvAlreadyPurchasedTitle;
        private LinearLayout LyCouldLikeTitle;
        private TextView TvCouldLikeTitle;
        private TextView TvHowDoItUses;
        #endregion

        #region Properties

        private int KillersCurrentPosition { get; set; }
        private int AlreadyPurchasedCurrentPosition { get; set; }
        private int CouldLikeCurrentPosition { get; set; }
        private int CouponManiaCurrentPosition { get; set; }
        private DiscountsModel discountsModel;
        private DiscountsResponse discountsResponse;
        private bool BackLobbyActivity { get; set; }
        private string CellPhone { get; set; }
        private bool SelectedItem { get; set; }
        private Discount SelectedDiscount { get; set; }
        private bool DrawBeforeTypeDiscounts { get; set; }

        private EnumDiscounts currentTap = EnumDiscounts.None;
        private EnumDiscounts currentEvent = EnumDiscounts.None;

        #endregion

        #region Public

        public bool OnInterceptTouchEvent(RecyclerView rv, MotionEvent e)
        {
            return GestureDetector.OnTouchEvent(e);
        }

        public void OnRequestDisallowInterceptTouchEvent(bool disallowIntercept)
        {
        }

        public void OnTouchEvent(RecyclerView rv, MotionEvent e)
        {
        }

        public async void OnSelectItemClicked(Discount discount)
        {
            SelectedDiscount = discount;
            SelectedItem = true;

            if (ValidateUserVerifyCode())
            {
                await ValidateRedeemableDiscount();
            }
        }

        public async void OnInactivatedClicked(Discount discount)
        {
            SelectedDiscount = discount;
            SelectedItem = true;

            if (ValidateUserVerifyCode())
            {
                await ValidateInactiveDiscount();
            }
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            return false;
        }

        public void OnLongPress(MotionEvent e)
        {
        }

        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return true;
        }

        public void OnShowPress(MotionEvent e)
        {
        }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }

        public void OnTermsClicked(Discount discount)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogTaxesInfo, null);
            TermsDialog = new AlertDialog.Builder(this).Create();
            TermsDialog.SetView(view);
            TermsDialog.SetCanceledOnTouchOutside(true);
            Button btnOk = view.FindViewById<Button>(Resource.Id.btnOk);
            TextView tvTitle = view.FindViewById<TextView>(Resource.Id.tvBoxTaxes);
            TextView tvMessage = view.FindViewById<TextView>(Resource.Id.tvBoxTaxesMessage);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                tvMessage.JustificationMode = JustificationMode.InterWord;
            }

            ImageView ivClose = view.FindViewById<ImageView>(Resource.Id.ivClose);
            btnOk.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            tvTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            btnOk.Click += delegate { TermsDialog.Cancel(); };
            ivClose.Click += delegate { TermsDialog.Cancel(); };
            tvTitle.Text = AppMessages.TermsAndConditions;
            tvMessage.Text = discount.Legal;
            TermsDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            TermsDialog.Show();
            RegisterModalTermsConditionsDiscount();
        }

        public void MessageReceived(string message)
        {
            if (EtVerifyCode != null)
            {
                EtVerifyCode.Text = message;
            }
        }

        public override void ResendAsync()
        {
            CloseModal();

            RunOnUiThread(async () =>
            {
                await SendSMSVerifyCode();
            });
        }

        public override void VerifyCodeAsync()
        {
            string code = EtVerifyCode.Text;
            ActiveCodeDialog.Dismiss();
            RunOnUiThread(async () =>
            {
                await VerifyUser(code);
            });
        }

        public override void SendSmsCodeAsync()
        {
            RunOnUiThread(async () =>
            {
                FirstTimeSendCode = true;
                await SendSMSVerifyCode();
            });
        }

        public override void EditCellPhoneAsync()
        {
            ShowVerifyCodeDialog(EnumTypesVerifyCode.EditCellPhone);
        }

        public override void SaveCellPhone(string cellPhone)
        {
            CellPhone = cellPhone;
            UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
            string messageValidation = userModel.ValidateCellPhoneField(cellPhone);

            if (string.IsNullOrEmpty(messageValidation))
            {
                RunOnUiThread(async () =>
                {
                    await SendSMSVerifyCode();
                });
            }
            else
            {
                MessageVerifyCode = messageValidation;
                ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            if (ActiveCodeDialog != null && ActiveCodeDialog.IsShowing)
            {
                CloseModal();
            }
            else
            {
                HideProgressDialog();
                Finish();
            }
        }

        #endregion

        #region Protected

        protected override async void EventError()
        {
            base.EventError();
            await this.GetDiscounts();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            Shop();
        }

        protected override void OnPause()
        {
            if (BroadcastService != null)
            {
                LocalBroadcastManager.GetInstance(this).UnregisterReceiver(BroadcastService);
                BroadcastService = null;
            }

            base.OnPause();
        }

        protected override void OnDestroy()
        {
            if (BroadcastService != null)
            {
                LocalBroadcastManager.GetInstance(this).UnregisterReceiver(BroadcastService);
                BroadcastService = null;
            }

            base.OnDestroy();
        }

        protected override void OnResume()
        {
            base.OnResume();
            SelectedItem = false;

            if (BroadcastService == null)
            {
                BroadcastService = new SmsMessageService();
            }

            BroadcastService.BindListener(this);
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(BroadcastService, new IntentFilter(Telephony.Sms.Intents.SmsReceivedAction));

            RunOnUiThread(async () =>
            {
                await GetUser();
            });
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.MyDiscounts, typeof(MyDiscountsActivity).Name);
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityMyDiscounts);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);
            discountsModel = new DiscountsModel(new DiscountsService(DeviceManager.Instance));
            _UserModel = new UserModel(new UserService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                BackLobbyActivity = Intent.Extras.GetBoolean(ConstantPreference.IsLobbyInMyDiscount);
            }

            GestureDetector = new GestureDetector(this, this);
            this.SetControlsProperties();
            this.EditFonts();
            await this.GetDiscounts();
            ValidateReadSmsAvailability();            
        }

        #endregion

        #region Privates     

        #region Discounts

        private void OnDiscountRedeemed(bool activated)
        {
            View view = LayoutInflater.Inflate(Resource.Layout.DialogTaxesInfo, null);
            TermsDialog = new AlertDialog.Builder(this).Create();
            TermsDialog.SetView(view);
            TermsDialog.SetCanceledOnTouchOutside(true);
            Button btnOk = view.FindViewById<Button>(Resource.Id.btnOk);
            TextView tvTitle = view.FindViewById<TextView>(Resource.Id.tvBoxTaxes);
            TextView tvMessage = view.FindViewById<TextView>(Resource.Id.tvBoxTaxesMessage);
            ImageView ivClose = view.FindViewById<ImageView>(Resource.Id.ivClose);
            btnOk.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            tvTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            btnOk.Click += delegate { TermsDialog.Cancel(); };
            ivClose.Click += delegate { TermsDialog.Cancel(); };
            
            tvTitle.Text = AppMessages.DiscountRedeemed;
            tvMessage.Text = AppMessages.GoToStore;
            TermsDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            TermsDialog.Show();
        }

        private async Task GetDiscounts()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                discountsResponse = await discountsModel.GetDiscounts();

                this.EventErrorIsFalse();

                if (discountsResponse.Result != null && discountsResponse.Result.HasErrors && discountsResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        ValidateErrorCode();
                    });
                }
                else
                {
                    if (HaveDiscounts())
                    {
                        ValidateUserVerifyCode();
                        DrawDiscounts();
                    }
                    else
                    {
                        HideProgressDialog();
                        ShowNoInfoLayout();
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.GetDiscounts } };

                RegisterMessageExceptions(exception, properties);
            }
        }

        private void DrawDiscounts()
        {
            discountsModel.ValidateRedeemedDiscounts(discountsResponse);
            DrawBeforeTypeDiscounts = false;
            SetKillers();
            SetAlreadyPurchased();
            SetCouldLike();
            SetCouponMania();
            SetDiscountActivated();
            SetDiscountRedeemed();
            ShowBodyLayout();
            DrawCounters();

            if(currentEvent == EnumDiscounts.None || currentEvent == EnumDiscounts.ForActivate) {
                EventForActivate();
              }
            else if (currentEvent == EnumDiscounts.Activated)
            {
                EventActivated();
            }
            else if (currentEvent == EnumDiscounts.Redeemed)
            {
                EventRedeemed();
            }

            if (discountsResponse.PreviewCampaign)
            {
                LyPreViewDiscount.Visibility = ViewStates.Visible;
                LyViewDiscount.Visibility = ViewStates.Gone;
            }
            else
            {
                LyPreViewDiscount.Visibility = ViewStates.Gone;
                LyViewDiscount.Visibility = ViewStates.Visible;
            }

            LyRedeemed.Visibility = discountsResponse.CorpEvent ? ViewStates.Visible : ViewStates.Gone;
            viewActivated.Visibility = discountsResponse.CorpEvent ? ViewStates.Visible : ViewStates.Gone;

            TvRangeDateDiscount.Text = discountsResponse.HeaderCampaign;
            TvRangeDateDiscountPreview.Text = discountsResponse.HeaderCampaign;

            TvMaxDiscounts.Text = GetString(Resource.String.str_active_discounts_message).Replace("{numero}", discountsResponse.ActivateCoupons.ToString());
            HideProgressDialog();
        }

        private void ValidateErrorCode()
        {
            string message = string.Empty;
            var errorServiceUnavailable = discountsResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ErrorServiceUnavailable)).Any();

            if (errorServiceUnavailable)
            {
                message = discountsResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).
                Equals(EnumErrorCode.ErrorServiceUnavailable)).FirstOrDefault().Description;

                this.EventErrorIsTrue(message, false);
            }
            else
            {
                var errorCouponsNotFound = discountsResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.CouponsNotFound)).Any();

                if (errorCouponsNotFound)
                {
                    message = discountsResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).
                    Equals(EnumErrorCode.CouponsNotFound)).FirstOrDefault().Description;

                    this.EventErrorIsTrue(message, true);
                }
                else
                {
                    ShowErrorLayout(MessagesHelper.GetMessage(discountsResponse.Result));
                }
            }

            HideProgressDialog();
        }

        private bool HaveDiscounts()
        {
            int total = 0;

            if (discountsResponse != null && discountsResponse.ActiveDiscounts != null)
            {
                total = discountsResponse.ActiveDiscounts.Killers != null ? discountsResponse.ActiveDiscounts.Killers.Count : 0;
                total += discountsResponse.ActiveDiscounts.AlreadyPurchased != null ? discountsResponse.ActiveDiscounts.AlreadyPurchased.Count : 0;
                total += discountsResponse.ActiveDiscounts.CouldLike != null ? discountsResponse.ActiveDiscounts.CouldLike.Count : 0;
                total += discountsResponse.Coupons != null ? discountsResponse.Coupons.Count : 0;
            }

            return total > 0;
        }

        private void EditFonts()
        {
            TvMaxDiscounts.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvExclusiveCoupons).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvMessageError.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvRangeDateDiscount.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvMessagePreView.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvRangeDateDiscountPreview.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvNumberActivate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvForActive.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvNumberActivated.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvActived.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvNumberRedeemed.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvRedeemed.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamBlack), TypefaceStyle.Normal);
            TvKillersTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvAlreadyPurchasedTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvCouldLikeTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvHowDoItUses.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvTitleDiscountActivated.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);
            TvTitleDiscountRedeemed.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.GothamMedium), TypefaceStyle.Normal);

        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            DefineNoInfoLayout();
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<RelativeLayout>(Resource.Id.rlMyDiscounts));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            LyKillers = FindViewById<LinearLayout>(Resource.Id.lyDiscountA);
            LyAlreadyPurchased = FindViewById<LinearLayout>(Resource.Id.lyDiscountB);
            LyCouldLike = FindViewById<LinearLayout>(Resource.Id.lyDiscountC);
            LyCouponMania = FindViewById<LinearLayout>(Resource.Id.lyDiscountCouponMania);
            RvKillers = LyKillers.FindViewById<RecyclerView>(Resource.Id.rvMyDiscount);
            RvAlreadyPurchased = LyAlreadyPurchased.FindViewById<RecyclerView>(Resource.Id.rvMyDiscount);
            RvCouldLike = LyCouldLike.FindViewById<RecyclerView>(Resource.Id.rvMyDiscount);
            RvCouponMania = LyCouponMania.FindViewById<RecyclerView>(Resource.Id.rvMyDiscount);
            LyTypeDiscounts = FindViewById<LinearLayout>(Resource.Id.lyTypeDiscounts);
            LyKillersTitle = FindViewById<LinearLayout>(Resource.Id.lyKillersTitle);
            TvKillersTitle = FindViewById<TextView>(Resource.Id.tvKillersTitle);
            LyAlreadyPurchasedTitle = FindViewById<LinearLayout>(Resource.Id.lyAlreadyPurchasedTitle);
            TvAlreadyPurchasedTitle = FindViewById<TextView>(Resource.Id.tvAlreadyPurchasedTitle);
            LyAlreadyPurchasedTitle = FindViewById<LinearLayout>(Resource.Id.lyAlreadyPurchasedTitle);
            TvAlreadyPurchasedTitle = FindViewById<TextView>(Resource.Id.tvAlreadyPurchasedTitle);
            LyCouldLikeTitle = FindViewById<LinearLayout>(Resource.Id.lyCouldLikeTitle);
            TvCouldLikeTitle = FindViewById<TextView>(Resource.Id.tvCouldLikeTitle);
            TvHowDoItUses = FindViewById<TextView>(Resource.Id.tvHowDoItUses);
            ivCouponManiaTitle = LyCouponMania.FindViewById<ImageView>(Resource.Id.ivTitleDiscount);
            IvKillersEndArrow = LyKillers.FindViewById<ImageView>(Resource.Id.ivArrowDiscount);
            IvAlreadyPurchasedEndArrow = LyAlreadyPurchased.FindViewById<ImageView>(Resource.Id.ivArrowDiscount);
            IvCouldLikeEndArrow = LyCouldLike.FindViewById<ImageView>(Resource.Id.ivArrowDiscount);
            IvCouponManiaEndArrow = LyCouponMania.FindViewById<ImageView>(Resource.Id.ivArrowDiscount);
            IvKillersStartArrow = LyKillers.FindViewById<ImageView>(Resource.Id.ivStartArrowDiscount);
            IvAlreadyPurchasedStartArrow = LyAlreadyPurchased.FindViewById<ImageView>(Resource.Id.ivStartArrowDiscount);
            IvCouldLikeStartArrow = LyCouldLike.FindViewById<ImageView>(Resource.Id.ivStartArrowDiscount);
            IvCouponManiaStartArrow = LyCouponMania.FindViewById<ImageView>(Resource.Id.ivStartArrowDiscount);
            TvMaxDiscounts = FindViewById<TextView>(Resource.Id.tvMaximumQuantityDiscounts);
            LyErrorDiscount = FindViewById<LinearLayout>(Resource.Id.lyErrorDiscount);
            LySuccessfulDiscount = FindViewById<LinearLayout>(Resource.Id.lySuccessfulDiscount);
            LySuccessfulDiscountItems = FindViewById<LinearLayout>(Resource.Id.lySuccessfulDiscountItems);
            LyActivate = FindViewById<LinearLayout>(Resource.Id.lyActivate);
            LyActivated = FindViewById<LinearLayout>(Resource.Id.lyActivated);
            LyRedeemed = FindViewById<LinearLayout>(Resource.Id.lyRedeemed);
            LyInsideActivate = FindViewById<LinearLayout>(Resource.Id.lyInsideActivate);
            LyInsideActivated = FindViewById<LinearLayout>(Resource.Id.lyInsideActivated);
            LyInsideRedeemed = FindViewById<LinearLayout>(Resource.Id.lyInsideRedeemed);
            TvRangeDateDiscount = FindViewById<TextView>(Resource.Id.tvRangeDateDiscount);
            TvMessagePreView = FindViewById<TextView>(Resource.Id.tvMessagePreView);
            TvRangeDateDiscountPreview = FindViewById<TextView>(Resource.Id.tvRangeDateDiscountPreview);
            TvNumberActivate = FindViewById<TextView>(Resource.Id.tvNumberActivate);
            TvForActive = FindViewById<TextView>(Resource.Id.tvForActive);
            TvNumberActivated = FindViewById<TextView>(Resource.Id.tvNumberActivated);
            TvActived = FindViewById<TextView>(Resource.Id.tvActived);
            viewActivated = FindViewById<View>(Resource.Id.viewActivated);
            TvNumberRedeemed = FindViewById<TextView>(Resource.Id.tvNumberRedeemed);
            TvRedeemed = FindViewById<TextView>(Resource.Id.tvRedeemed);

            LyInformationActivated = FindViewById<LinearLayout>(Resource.Id.lyInformationActivated);
            TvTitleDiscountActivated = FindViewById<TextView>(Resource.Id.tvTitleDiscountActivated);
            RvMyDiscountActivated = FindViewById<RecyclerView>(Resource.Id.rvMyDiscountActivated);
            LyInformationRedeemed = FindViewById<LinearLayout>(Resource.Id.lyInformationRedeemed);
            TvTitleDiscountRedeemed = FindViewById<TextView>(Resource.Id.tvTitleDiscountRedeemed);
            RvMyDiscountRedeemed = FindViewById<RecyclerView>(Resource.Id.rvMyDiscountRedeemed);
            LyPreViewDiscount = FindViewById<LinearLayout>(Resource.Id.LyPreViewDiscount);
            LyViewDiscount = FindViewById<LinearLayout>(Resource.Id.lyViewDiscount);

            TvMessageError = FindViewById<TextView>(Resource.Id.tvMessageError);
            IvErrorDiscount = FindViewById<ImageView>(Resource.Id.ivErrorDiscount);

            this.DrawStyleLayout();
            this.SetAction();
            RvKillers.AddOnItemTouchListener(this);
            RvAlreadyPurchased.AddOnItemTouchListener(this);
            RvCouldLike.AddOnItemTouchListener(this);
            RvCouponMania.AddOnItemTouchListener(this);
        }

        private void EventTypeDiscount(int idType)
        {
            switch (idType)
            {
                case 1:
                    EventType(true, TvKillersTitle); EventType(false, TvAlreadyPurchasedTitle); EventType(false, TvCouldLikeTitle);
                    LyKillers.Visibility = ViewStates.Visible;
                    LyAlreadyPurchased.Visibility = ViewStates.Gone;
                    LyCouldLike.Visibility = ViewStates.Gone;
                    currentTap = EnumDiscounts.Killer;
                    break;
                case 2:
                    EventType(false, TvKillersTitle); EventType(true, TvAlreadyPurchasedTitle); EventType(false, TvCouldLikeTitle);
                    LyKillers.Visibility = ViewStates.Gone;
                    LyAlreadyPurchased.Visibility = ViewStates.Visible;
                    LyCouldLike.Visibility = ViewStates.Gone;
                    currentTap = EnumDiscounts.AlreadyPurchased;
                    break;
                case 3:
                    EventType(false, TvKillersTitle); EventType(false, TvAlreadyPurchasedTitle); EventType(true, TvCouldLikeTitle);
                    LyKillers.Visibility = ViewStates.Gone;
                    LyAlreadyPurchased.Visibility = ViewStates.Gone;
                    LyCouldLike.Visibility = ViewStates.Visible;
                    currentTap = EnumDiscounts.CouldLike;
                    break;
            }
        }

        public void EventType(bool Selected, TextView texview)
        {
            if (Selected)
            {
                texview.SetBackgroundResource(Resource.Drawable.addresses_item_selected_discount);
                texview.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));                
            }
            else
            {
                texview.SetBackgroundColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorBackgroundDiscounts)));
                texview.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.black)));                
            }
        }

        private void SetAction()
        {

            LyActivate.Click += delegate
            {
                if (!String.IsNullOrEmpty(TvNumberActivate.Text) && !TvNumberActivate.Text.Equals("0"))
                {
                    EventForActivate();
                }
            };

            LyActivated.Click += delegate
            {
                if (!String.IsNullOrEmpty(TvNumberActivated.Text) && !TvNumberActivated.Text.Equals("0"))
                {
                    EventActivated();
                }
            };

            LyRedeemed.Click += delegate
            {
                if (!String.IsNullOrEmpty(TvNumberRedeemed.Text) && !TvNumberRedeemed.Text.Equals("0"))
                {
                    EventRedeemed();
                }
            };

            LyKillersTitle.Click += delegate { EventTypeDiscount(1); };
            LyAlreadyPurchasedTitle.Click += delegate { EventTypeDiscount(2); };
            LyCouldLikeTitle.Click += delegate { EventTypeDiscount(3); };
            TvHowDoItUses.Click += delegate { Tutorial(); };
        }

        private void DrawCounters()
        {
            TvNumberActivate.Text = discountsResponse.TotalActiveDiscounts.ToString();
            TvNumberActivated.Text = discountsResponse.TotalActivatedDiscounts.ToString();
            TvNumberRedeemed.Text = discountsResponse.TotalRedeemedDiscounts.ToString();
        }

        private void DrawStyleLayout()
        {
            this.PutUnderLineTextView(TvForActive);
            this.DeleteUnderLineTextView(TvActived);
            this.DeleteUnderLineTextView(TvRedeemed);
        }

        private void EventForActivate()
        {
            this.Selected(true, LyInsideActivate, LySuccessfulDiscountItems, TvNumberActivate, TvForActive);
            this.Selected(false, LyInsideActivated, LyInformationActivated, TvNumberActivated, TvActived);
            this.Selected(false, LyInsideRedeemed, LyInformationRedeemed, TvNumberRedeemed, TvRedeemed);
            currentEvent = EnumDiscounts.ForActivate;
        }

        private void EventActivated()
        {
            this.Selected(false, LyInsideActivate, LySuccessfulDiscountItems, TvNumberActivate, TvForActive);
            this.Selected(true, LyInsideActivated, LyInformationActivated, TvNumberActivated, TvActived);
            this.Selected(false, LyInsideRedeemed, LyInformationRedeemed, TvNumberRedeemed, TvRedeemed);
            currentEvent = EnumDiscounts.Activated;
            RegisterActivatedDiscounts();
        }

        private void EventRedeemed()
        {
            this.Selected(false, LyInsideActivate, LySuccessfulDiscountItems, TvNumberActivate, TvForActive);
            this.Selected(false, LyInsideActivated, LyInformationActivated, TvNumberActivated, TvActived);
            this.Selected(true, LyInsideRedeemed, LyInformationRedeemed, TvNumberRedeemed, TvRedeemed);
            currentEvent = EnumDiscounts.Redeemed;
        }

        public void Selected(bool select, LinearLayout linearLayout, LinearLayout lyVisible, TextView textView, TextView textViewName)
        {
            if (select)
            {
                lyVisible.Visibility = ViewStates.Visible;
                this.PutUnderLineTextView(textViewName);
            }
            else
            {
                lyVisible.Visibility = ViewStates.Gone;
                this.DeleteUnderLineTextView(textViewName);
            }
        }

        private void ResizeLayout(LinearLayout linearLayout, int size)
        {
            int dp = ConvertUtilities.ConvertDpToPixels(size);
            LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(dp, dp)
            {
                Gravity = GravityFlags.Center
            };
            linearLayout.LayoutParameters = layoutParams;
        }

        private void PutUnderLineTextView(TextView textView)
        {
            SpannableString content = new SpannableString(textView.Text);
            content.SetSpan(new UnderlineSpan(), 0, content.Length(), 0);
            textView.TextFormatted = content;
        }

        private void DeleteUnderLineTextView(TextView textView)
        {
            SpannableString content = new SpannableString(textView.Text);
            content.SetSpan(new StyleSpan(TypefaceStyle.Normal), 0, content.Length(), 0);
            textView.TextFormatted = content;
        }


        private void Fonts(TextView textView, int textStart, int TextEnd, string text)
        {
            SpannableStringBuilder strfont = new SpannableStringBuilder(text);
            strfont.SetSpan(new StyleSpan(TypefaceStyle.Bold), textStart, TextEnd, SpanTypes.ExclusiveExclusive);
            textView.TextFormatted = strfont;
        }

        private void DefineNoInfoLayout()
        {
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                     FindViewById<RelativeLayout>(Resource.Id.rlMyDiscounts),
                     AppMessages.NoDiscountsMessage, AppMessages.DiscountsAction);
        }

        private void MoveListToNext(RecyclerView recyclerView, ImageView ivArrowStart, ImageView ivArrowEnd, IList<Discount> discounts, int currentPosition)
        {
            try
            {
                LayoutManager layoutManager = recyclerView.GetLayoutManager();
                recyclerView.SmoothScrollToPosition(((LinearLayoutManager)layoutManager).FindLastVisibleItemPosition() + 1);

                ivArrowStart.Visibility = ViewStates.Visible;

                if (discounts != null && currentPosition >= discounts.Count - 1)
                {
                    ivArrowEnd.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.MoveListToBack } };

                RegisterMessageExceptions(exception, properties);
            }
        }

        private void MoveListToBack(RecyclerView recyclerView, ImageView ivArrowStart, ImageView ivArrowEnd, IList<Discount> discounts, int currentPosition)
        {
            try
            {
                LayoutManager layoutManager = recyclerView.GetLayoutManager();
                recyclerView.SmoothScrollToPosition(((LinearLayoutManager)layoutManager).FindLastVisibleItemPosition() - 1);

                ivArrowEnd.Visibility = ViewStates.Visible;

                if (currentPosition <= 0)
                {
                    ivArrowStart.Visibility = ViewStates.Gone;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.MoveListToBack } };

                RegisterMessageExceptions(exception, properties);
            }
        }

        private void SetKillers()
        {
            if (discountsResponse.ActiveDiscounts.Killers != null && discountsResponse.ActiveDiscounts.Killers.Any())
            {
                LyKillersTitle.Visibility = ViewStates.Visible;
                int itemForLine = discountsResponse.ActiveDiscounts.Killers.Count == 1 ? 1 : 2;
                GridLayoutManager manager = new GridLayoutManager(this, itemForLine, GridLayoutManager.Vertical, false)
                {
                    AutoMeasureEnabled = true
                };

                if (itemForLine > 1)
                {
                    IvKillersEndArrow.Visibility = ViewStates.Gone;
                    IvKillersStartArrow.Visibility = ViewStates.Gone;
                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    int dpTopBottom = ConvertUtilities.ConvertDpToPixels(15);
                    layoutParams.SetMargins(0, dpTopBottom, 0, dpTopBottom);
                    RvKillers.LayoutParameters = layoutParams;
                }

                RvKillers.HasFixedSize = false;
                RvKillers.SetLayoutManager(manager);
                KillersAdapter = new MyDiscountsAdapter(discountsResponse.ActiveDiscounts.Killers, this, this, false);
                RvKillers.SetAdapter(KillersAdapter);
                LyKillers.Visibility = ViewStates.Gone;

                if(currentTap == EnumDiscounts.None || currentTap == EnumDiscounts.Killer)
                {
                    DrawBeforeTypeDiscounts = true;
                    EventTypeDiscount(1);
                }
                
            }
            else
            {
                LyKillers.Visibility = ViewStates.Gone;
                LyKillersTitle.Visibility = ViewStates.Gone;
            }
        }

        private void SetAlreadyPurchased()
        {
            if (discountsResponse.ActiveDiscounts.AlreadyPurchased != null && discountsResponse.ActiveDiscounts.AlreadyPurchased.Any())
            {
                LyAlreadyPurchasedTitle.Visibility = ViewStates.Visible;
                int itemForLine = discountsResponse.ActiveDiscounts.AlreadyPurchased.Count == 1 ? 1 : 2;
                GridLayoutManager manager = new GridLayoutManager(this, itemForLine, GridLayoutManager.Vertical, false)
                {
                    AutoMeasureEnabled = true
                };

                if (itemForLine > 1)
                {
                    IvAlreadyPurchasedEndArrow.Visibility = ViewStates.Gone;
                    IvAlreadyPurchasedStartArrow.Visibility = ViewStates.Gone;
                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    int dpTopBottom = ConvertUtilities.ConvertDpToPixels(15);
                    layoutParams.SetMargins(0, dpTopBottom, 0, dpTopBottom);
                    RvAlreadyPurchased.LayoutParameters = layoutParams;
                }

                RvAlreadyPurchased.HasFixedSize = false;
                RvAlreadyPurchased.SetLayoutManager(manager);
                AlreadyPurchasedAdapter = new MyDiscountsAdapter(discountsResponse.ActiveDiscounts.AlreadyPurchased, this, this, false);
                RvAlreadyPurchased.SetAdapter(AlreadyPurchasedAdapter);
                LyAlreadyPurchased.Visibility = ViewStates.Gone;

                if (!DrawBeforeTypeDiscounts && currentTap == EnumDiscounts.AlreadyPurchased)
                {
                    DrawBeforeTypeDiscounts = true;
                    EventTypeDiscount(2);
                }
            }
            else
            {
                LyAlreadyPurchased.Visibility = ViewStates.Gone;
                LyAlreadyPurchasedTitle.Visibility = ViewStates.Gone;                
            }
        }

        private void SetCouldLike()
        {
            if (discountsResponse.ActiveDiscounts.CouldLike != null && discountsResponse.ActiveDiscounts.CouldLike.Any())
            {
                LyCouldLikeTitle.Visibility = ViewStates.Visible;
                int itemForLine = discountsResponse.ActiveDiscounts.CouldLike.Count == 1 ? 1 : 2;
                GridLayoutManager manager = new GridLayoutManager(this, itemForLine, GridLayoutManager.Vertical, false)
                {
                    AutoMeasureEnabled = true
                };

                if (itemForLine > 1)
                {
                    IvCouldLikeEndArrow.Visibility = ViewStates.Gone;
                    IvCouldLikeStartArrow.Visibility = ViewStates.Gone;
                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    int dpTopBottom = ConvertUtilities.ConvertDpToPixels(15);
                    layoutParams.SetMargins(0, dpTopBottom, 0, dpTopBottom);
                    RvCouldLike.LayoutParameters = layoutParams;
                }

                RvCouldLike.HasFixedSize = false;
                RvCouldLike.SetLayoutManager(manager);
                CouldLikeAdapter = new MyDiscountsAdapter(discountsResponse.ActiveDiscounts.CouldLike, this, this, false);
                RvCouldLike.SetAdapter(CouldLikeAdapter);
                LyCouldLike.Visibility = ViewStates.Gone;
                if (!DrawBeforeTypeDiscounts && currentTap == EnumDiscounts.CouldLike)
                {
                    DrawBeforeTypeDiscounts = true;
                    EventTypeDiscount(3);
                }
            }
            else
            {
                LyCouldLike.Visibility = ViewStates.Gone;
                LyCouldLikeTitle.Visibility = ViewStates.Gone;
            }
        }

        private void SetCouponMania()
        {
            if (discountsResponse.Coupons != null && discountsResponse.Coupons.Any())
            {
                LinearLayoutManager linerLayoutManager = new LinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
                RvCouponMania.HasFixedSize = false;
                RvCouponMania.SetLayoutManager(linerLayoutManager);
                CouponManiaAdapter = new MyDiscountsAdapter(discountsResponse.Coupons, this, this, true);
                RvCouponMania.SetAdapter(CouponManiaAdapter);
                LyCouponMania.Visibility = ViewStates.Visible;
            }
            else
            {
                LyCouponMania.Visibility = ViewStates.Gone;
            }
        }

        private void SetDiscountActivated()
        {
            if (discountsResponse.ActivatedDiscounts != null && discountsResponse.ActivatedDiscounts.Any())
            {
                Fonts(TvTitleDiscountActivated, 15, 24, AppMessages.ActivatedTitle);
                int itemForLine = discountsResponse.ActivatedDiscounts.Count == 1 ? 1 : 2;
                GridLayoutManager manager = new GridLayoutManager(this, itemForLine, GridLayoutManager.Vertical, false)
                {
                    AutoMeasureEnabled = true
                };

                if (itemForLine > 1)
                {
                    View ViewBeforeActivated = FindViewById<View>(Resource.Id.viewBeforeActivated);
                    ViewBeforeActivated.Visibility = ViewStates.Gone;
                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    int dpTopBottom = ConvertUtilities.ConvertDpToPixels(15);
                    layoutParams.SetMargins(0, dpTopBottom, 0, dpTopBottom);
                    RvMyDiscountActivated.LayoutParameters = layoutParams;
                    View ViewAfterActivated = FindViewById<View>(Resource.Id.viewAfterActivated);
                    ViewAfterActivated.Visibility = ViewStates.Gone;
                }

                RvMyDiscountActivated.HasFixedSize = false;
                RvMyDiscountActivated.SetLayoutManager(manager);
                MyDiscountActivatedAdapter = new MyDiscountsDetailAdapter(discountsResponse.ActivatedDiscounts, this, this, false);
                RvMyDiscountActivated.SetAdapter(MyDiscountActivatedAdapter);
            }
        }

        private void SetDiscountRedeemed()
        {
            if (discountsResponse.RedeemedDiscounts != null && discountsResponse.RedeemedDiscounts.Any())
            {
                Fonts(TvTitleDiscountRedeemed, 15, 24, AppMessages.RedeemedTitle);

                int itemForLine = discountsResponse.RedeemedDiscounts.Count == 1 ? 1 : 2;

                GridLayoutManager manager = new GridLayoutManager(this, itemForLine, GridLayoutManager.Vertical, false)
                {
                    AutoMeasureEnabled = true
                };

                if (itemForLine > 1)
                {
                    View ViewBeforeRedeemed = FindViewById<View>(Resource.Id.viewBeforeRedeemed);
                    ViewBeforeRedeemed.Visibility = ViewStates.Gone;

                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                    int dpTopBottom = ConvertUtilities.ConvertDpToPixels(15);
                    layoutParams.SetMargins(0, dpTopBottom, 0, dpTopBottom);
                    RvMyDiscountRedeemed.LayoutParameters = layoutParams;

                    View ViewAfterRedeemed = FindViewById<View>(Resource.Id.viewAfterRedeemed);
                    ViewAfterRedeemed.Visibility = ViewStates.Gone;
                }

                RvMyDiscountRedeemed.HasFixedSize = false;
                RvMyDiscountRedeemed.SetLayoutManager(manager);
                MyDiscountRedeemedAdapter = new MyDiscountsDetailAdapter(discountsResponse.RedeemedDiscounts, this, this, true);
                RvMyDiscountRedeemed.SetAdapter(MyDiscountRedeemedAdapter);
            }
        }

        private async Task RedeemDiscount(Discount discount)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                DiscountParameters discountParameters = GetParameters(discount);

                DisccountResponse activeDisccountResponse = await discountsModel.ActiveDisccount(discountParameters);

                if (activeDisccountResponse.Result != null && activeDisccountResponse.Result.HasErrors && activeDisccountResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        ShowErrorLayout(MessagesHelper.GetMessage(activeDisccountResponse.Result));
                    });
                }
                else
                {
                    ValidateResponseActivateDiscount(activeDisccountResponse, discount);
                }
            }
            catch (Exception exception)
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.RedeemDiscount } };

                RegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ValidateResponseActivateDiscount(DisccountResponse activeDisccountResponse, Discount discount)
        {
            if (!string.IsNullOrEmpty(activeDisccountResponse.Message) && activeDisccountResponse.Message.Equals("success"))
            {
                AnalyticsActivateDiscount(discount);
                ConvertUtilities.CustomColorMessageToast(AppMessages.DiscountRedeemed, this, new Color(ContextCompat.GetColor(this, Resource.Color.colorDiscounts)), new Color(ContextCompat.GetColor(this, Resource.Color.white)));

                discountsModel.ValidateRedeemedDiscounts(discountsResponse);

                if (KillersAdapter != null)
                {
                    KillersAdapter.NotifyDataSetChanged();
                }

                if (AlreadyPurchasedAdapter != null)
                {
                    AlreadyPurchasedAdapter.NotifyDataSetChanged();
                }

                if (CouldLikeAdapter != null)
                {
                    CouldLikeAdapter.NotifyDataSetChanged();
                }

                if (CouponManiaAdapter != null)
                {
                    CouponManiaAdapter.NotifyDataSetChanged();
                }

                discountsResponse = discountsModel.UpdatActivatedDiscounts(discountsResponse, discount);
                this.DrawDiscounts();
            }
            else
            {
                ShowErrorLayout(AppMessages.DiscountErrorMessage);
            }
        }

        private async Task InactivateDiscount(Discount discount)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                DiscountParameters discountParameters = GetParameters(discount);

                DisccountResponse inactiveDisccountResponse = await discountsModel.InactiveDisccount(discountParameters);

                if (inactiveDisccountResponse.Result != null && inactiveDisccountResponse.Result.HasErrors && inactiveDisccountResponse.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        ShowErrorLayout(MessagesHelper.GetMessage(inactiveDisccountResponse.Result));
                    });
                }
                else
                {
                    ValidateResponseInactivateDiscount(inactiveDisccountResponse, discount);
                }
            }
            catch (Exception exception)
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.RedeemDiscount } };

                RegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private DiscountParameters GetParameters(Discount discount)
        {
            DiscountParameters discountParameters = new DiscountParameters
            {
                PosCode = discount.PosCode,
                StartDate = discount.StartDate
            };

            return discountParameters;
        }

        private void ValidateResponseInactivateDiscount(DisccountResponse inactiveDisccountResponse, Discount discount)
        {
            if (!string.IsNullOrEmpty(inactiveDisccountResponse.Message) && inactiveDisccountResponse.Message.Equals("success"))
            {
                AnalyticsInactivateDiscount(discount);
                ConvertUtilities.CustomColorMessageToast(AppMessages.DiscountDesactivated, this, new Color(ContextCompat.GetColor(this, Resource.Color.colorDiscounts)), new Color(ContextCompat.GetColor(this, Resource.Color.white)));

                discountsModel.ValidateRedeemedDiscounts(discountsResponse);

                if (KillersAdapter != null)
                {
                    KillersAdapter.NotifyDataSetChanged();
                }

                if (AlreadyPurchasedAdapter != null)
                {
                    AlreadyPurchasedAdapter.NotifyDataSetChanged();
                }

                if (CouldLikeAdapter != null)
                {
                    CouldLikeAdapter.NotifyDataSetChanged();
                }

                if (CouponManiaAdapter != null)
                {
                    CouponManiaAdapter.NotifyDataSetChanged();
                }

                if (MyDiscountActivatedAdapter != null)
                {
                    MyDiscountActivatedAdapter.NotifyDataSetChanged();
                }

                discountsResponse = discountsModel.UpdatActivatedDiscounts(discountsResponse, discount);

                this.DrawDiscounts();
            }
            else
            {
                ShowErrorLayout(AppMessages.DiscountErrorMessage);
            }
        }

        private void Shop()
        {
            Intent intent = null;
            intent = BackLobbyActivity ? new Intent(this, typeof(LobbyActivity)) : new Intent(this, typeof(MainActivity));
            intent.PutExtra(ConstantPreference.KeepLobby, true);
            intent.AddFlags(ActivityFlags.ClearTask | ActivityFlags.NewTask);
            StartActivity(intent);
            Finish();
        }

        private void EventErrorIsFalse()
        {
            LyErrorDiscount.Visibility = ViewStates.Gone;
            LySuccessfulDiscount.Visibility = ViewStates.Visible;
            LySuccessfulDiscountItems.Visibility = ViewStates.Visible;
        }

        private void EventErrorIsTrue(String message, bool NoFoundInfo)
        {
            LyErrorDiscount.Visibility = ViewStates.Visible;
            LySuccessfulDiscount.Visibility = ViewStates.Gone;
            LySuccessfulDiscountItems.Visibility = ViewStates.Gone;

            IvErrorDiscount.SetImageResource(NoFoundInfo ? Resource.Drawable.sin_informacion : Resource.Drawable.mantenimiento_descuentos);
            TvMessageError.Text = message;
        }

        #endregion

        #region Active code

        private async Task GetUser()
        {
            try
            {
                MyAccountModel _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
                var response = await _myAccountModel.GetUser();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                }
                else
                {
                    SavetUser(response.User);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.GetUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }
        private void SavetUser(User user)
        {
            UserContext userContext = ModelHelper.UpdateUserContext(ParametersManager.UserContext, user);
            userContext.Address = ParametersManager.UserContext.Address;
            userContext.Store = ParametersManager.UserContext.Store;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
        }

        private async Task ValidateRedeemableDiscount()
        {
            if (SelectedDiscount != null && SelectedDiscount.Redeemable && !SelectedDiscount.Active)
            {
                await this.RedeemDiscount(SelectedDiscount);
            }
        }

        private async Task ValidateInactiveDiscount()
        {
            if (SelectedDiscount != null && SelectedDiscount.Active)
            {
                await this.InactivateDiscount(SelectedDiscount);
            }
        }

        private bool ValidateUserVerifyCode()
        {
            bool activate = false;

            if (!ParametersManager.UserContext.UserActivate)
            {
                if (string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone))
                {
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.NewCellPhone);
                }
                else
                {
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.SendCod);
                    SetCellPhoneVerifyCode(!string.IsNullOrEmpty(CellPhone) ? CellPhone : ParametersManager.UserContext.CellPhone);
                }
            }
            else
            {
                activate = true;
            }

            SaveValidatedUser(activate);
            return activate;
        }

        public async Task SendSMSVerifyCode()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                string messageValidation = userModel.ValidateCellPhoneField(!string.IsNullOrEmpty(CellPhone) ? CellPhone : ParametersManager.UserContext.CellPhone);

                if (string.IsNullOrEmpty(messageValidation))
                {
                    VerifyUserParameters parameters = SetParametersSendSMS();
                    SendMessageVerifyUserResponse response = await SendSMSVerifyCode(parameters);
                    this.ValidateResponseSendSms(response);
                }
                else
                {
                    FirstTimeSendCode = true;
                    HideProgressDialog();
                    MessageVerifyCode = messageValidation;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.SendSMSVerifyCode } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        public VerifyUserParameters SetParametersSendSMS()
        {
            VerifyUserParameters parameters = new VerifyUserParameters
            {
                CellPhone = !string.IsNullOrEmpty(CellPhone) ? CellPhone : ParametersManager.UserContext.CellPhone,
                DocumentNumber = ParametersManager.UserContext.DocumentNumber,
                SiteId = AppServiceConfiguration.SiteId,
            };

            return parameters;
        }

        private void ValidateResponseSendSms(SendMessageVerifyUserResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    MessageVerifyCode = MessagesHelper.GetMessage(response.Result);
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                });
            }
            else if (response.MessageSent)
            {
                HideProgressDialog();
                ShowVerifyCodeDialog(EnumTypesVerifyCode.VerifyCode);
                SetCellPhone(!string.IsNullOrEmpty(CellPhone) ? CellPhone : ParametersManager.UserContext.CellPhone);
            }
            else
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    MessageVerifyCode = AppMessages.SedSMSError;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                });
            }
        }

        private async Task VerifyUser(string code)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                string messageValidation = userModel.ValidateCodeField(code.TrimStart().TrimEnd());

                if (string.IsNullOrEmpty(messageValidation))
                {
                    VerifyUserParameters parameters = SetParametersVerifyUser(code);
                    VerifyUserResponse response = await VerifyUser(parameters);
                    await this.ValidateResponseVerifyUser(response);
                }
                else
                {
                    HideProgressDialog();
                    MessageVerifyCode = messageValidation;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.VerifyUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private async Task ValidateResponseVerifyUser(VerifyUserResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();

                    var errorCellPhoneRegistered = response.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ErrorCellPhoneRegistered)).Any();

                    if (errorCellPhoneRegistered)
                    {
                        string messageCellPhoneRegistered = response.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).
                        Equals(EnumErrorCode.ErrorCellPhoneRegistered)).First().Description;

                        MessageVerifyCode = messageCellPhoneRegistered;
                        ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                    }
                    else
                    {
                        MessageVerifyCode = MessagesHelper.GetMessage(response.Result);
                        ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                    }
                });
            }
            else if (response.Verified)
            {
                await SaveAndContinueUserVerify(response);
            }
            else
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.VerifyUserError, AppMessages.AcceptButtonText);
                });
            }
        }

        private async Task SaveAndContinueUserVerify(VerifyUserResponse response)
        {
            SaveValidatedUser(response.Verified);

            if (!string.IsNullOrEmpty(CellPhone))
            {
                SaveValidatedCellPhone(CellPhone);
                await UpdateCellPhone();
            }

            if (SelectedItem)
            {
                await ValidateRedeemableDiscount();
            }
            else
            {
                CloseModal();
            }

            HideProgressDialog();
            SelectedItem = false;
        }

        private VerifyUserParameters SetParametersVerifyUser(string code)
        {
            VerifyUserParameters parameters = new VerifyUserParameters
            {
                CellPhone = !string.IsNullOrEmpty(CellPhone) ? CellPhone : ParametersManager.UserContext.CellPhone,
                DocumentNumber = ParametersManager.UserContext.DocumentNumber,
                SiteId = AppServiceConfiguration.SiteId,
                Code = code.TrimStart().TrimEnd()
            };

            return parameters;
        }

        private async Task UpdateCellPhone()
        {
            try
            {
                UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                UpdateCellPhoneParameters parameters = new UpdateCellPhoneParameters { CellPhone = ParametersManager.UserContext.CellPhone };
                await userModel.UpdateCellPhone(parameters);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.MyDiscountsActivity, ConstantMethodName.EditProfile } };
                RegisterMessageExceptions(exception, properties);
            }
        }

        public void Tutorial()
        {
            string preferenceName = ConstNameViewTutorial.Discount;

            Intent intent = new Intent(this, typeof(TutorialsActivity));
            intent.PutExtra(ConstantPreference.ActivityTutorial, preferenceName);
            StartActivity(intent);
        }

        #endregion

        #region Analytic

        private void AnalyticsActivateDiscount(Discount discount)
        {
            FirebaseRegistrationEventsService.Instance.ActivatedDiscount(discount);
            FacebookRegistrationEventsService.Instance.ActivatedDiscount(discount);
        }

        private void AnalyticsInactivateDiscount(Discount discount)
        {
            FirebaseRegistrationEventsService.Instance.InactivateDiscount(discount);
            FacebookRegistrationEventsService.Instance.InactivateDiscount(discount);
        }

        private void RegisterActivatedDiscounts()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ActivatedDiscounts, typeof(MyDiscountsActivity).Name);
        }

        private void RegisterModalTermsConditionsDiscount()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ModalTermsConditionsDiscount, typeof(MyDiscountsActivity).Name);
        }

        #endregion

        #endregion
    }
}