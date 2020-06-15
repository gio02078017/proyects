using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Android.Net;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Uri = Android.Net.Uri;
using Java.Net;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Android.Services;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Cliente prime", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ClientPrimeActivity : BaseActivity
    {
        #region Controls

        private TextView TvTitle;
        private TextView TvInfoTitle;
        private TextView TvInfoContent;
        private TextView TvInitDateTitle;
        private TextView TvInitDate;
        private TextView TvEndDateTitle;
        private TextView TvEndDate;
        private TextView TvSuscriptionTypeTitle;
        private TextView TvSuscriptionType;
        private TextView TvPaymentMethodTitle;
        private TextView TvPaymentMethod;
        private TextView TvDescription;
        private TextView TvPrimeBenefit;
        private TextView TvPrimePlans;
        private TextView TvItemNoPrime1;
        private TextView TvItemNoPrime2;
        private TextView TvItemNoPrime3;
        private TextView TvItemNoPrime4;
        private TextView TvItemNoPrime5;
        private LinearLayout LyButtonGotoExito;
        private LinearLayout RootLayout;
        private LinearLayout ClientNoPrimeLayout;

        #endregion

        #region Properties

        private MyAccountModel _myAccountModel { get; set; }

        #endregion


        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityClientPrime);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();
            this.customFont();
            await this.GetUser();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ClientPrime, typeof(ClientPrimeActivity).Name);
        }

        private void customFont()
        {
            Fonts(TvDescription, 45, 55, GetString(Resource.String.str_description));
            Fonts(TvItemNoPrime1, 0, 12, GetString(Resource.String.str_item_prime_1));
            Fonts(TvItemNoPrime2, 0, 12, GetString(Resource.String.str_item_prime_2));
            Fonts(TvItemNoPrime4, 0, 22, GetString(Resource.String.str_item_prime_4));
            string strItem2 = GetString(Resource.String.str_item_prime_2);
            int countTextItem2 = strItem2.Length;
            int startItem2 = countTextItem2 - 7;
            SpannableStringBuilder strSpannableMessageEnjoyPrime = new SpannableStringBuilder(strItem2);
            strSpannableMessageEnjoyPrime.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, 12, SpanTypes.ExclusiveExclusive);
            strSpannableMessageEnjoyPrime.SetSpan(new StyleSpan(TypefaceStyle.Bold), startItem2, countTextItem2, SpanTypes.ExclusiveExclusive);
            TvItemNoPrime2.TextFormatted = strSpannableMessageEnjoyPrime;
            Fonts(TvItemNoPrime3, 0, 16, GetString(Resource.String.str_item_prime_3));
            Fonts(TvItemNoPrime5, 0, 12, GetString(Resource.String.str_item_prime_5));
        }

        private void EditFonts()
        {
            TvTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvInfoTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvInfoContent.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvInitDateTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvInitDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvEndDateTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvEndDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvSuscriptionTypeTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvSuscriptionType.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvPaymentMethodTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvPaymentMethod.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TvDescription.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPrimeBenefit.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvItemNoPrime1.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvItemNoPrime2.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvItemNoPrime3.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvItemNoPrime4.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            TvItemNoPrime5.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPrimePlans.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvButtonGotoExito).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvPrimeMonth).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvPrimeMonthPrice).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvPrimeYear).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvPrimeYearPrice).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private void PutData()
        {
            UserContext user = ParametersManager.UserContext;

            if (user.Prime)
            {
                if (string.IsNullOrEmpty(user.StartDatePrime))
                {
                    ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                }
                else
                {
                    RootLayout.Visibility = ViewStates.Visible;
                    ClientNoPrimeLayout.Visibility = ViewStates.Gone;
                    LyButtonGotoExito.Visibility = ViewStates.Gone;
                    TvInitDate.Text = user.StartDatePrime;
                    TvEndDate.Text = user.EndDatePrime;
                    TvSuscriptionType.Text = user.PeriodicityPrime;
                    TvPaymentMethod.Text = user.PaymentMethodPrime;
                }
            }
            else
            {
                RootLayout.Visibility = ViewStates.Gone;
                ClientNoPrimeLayout.Visibility = ViewStates.Visible;
                LyButtonGotoExito.Visibility = ViewStates.Visible;
            }
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError), FindViewById<NestedScrollView>(Resource.Id.nsvBody));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            TvTitle = FindViewById<TextView>(Resource.Id.tvPrimeTitle);
            TvInfoTitle = FindViewById<TextView>(Resource.Id.tvPrimeInfoTitle);
            TvInfoContent = FindViewById<TextView>(Resource.Id.tvPrimeInfoContent);
            TvInitDateTitle = FindViewById<TextView>(Resource.Id.tvPrimeInitDateTitle);
            TvInitDate = FindViewById<TextView>(Resource.Id.tvPrimeInitDate);
            TvEndDateTitle = FindViewById<TextView>(Resource.Id.tvPrimeEndDateTitle);
            TvEndDate = FindViewById<TextView>(Resource.Id.tvPrimeEndDate);
            TvSuscriptionTypeTitle = FindViewById<TextView>(Resource.Id.tvPrimeTypeTitle);
            TvSuscriptionType = FindViewById<TextView>(Resource.Id.tvPrimeType);
            TvPaymentMethodTitle = FindViewById<TextView>(Resource.Id.tvPrimePaymentMethodTitle);
            TvPaymentMethod = FindViewById<TextView>(Resource.Id.tvPrimePaymentMethod);
            RootLayout = FindViewById<LinearLayout>(Resource.Id.rootLayout);
            ClientNoPrimeLayout = FindViewById<LinearLayout>(Resource.Id.layoutClientNoPrime);
            TvDescription = FindViewById<TextView>(Resource.Id.tvDescription);
            TvPrimeBenefit = FindViewById<TextView>(Resource.Id.tvPrimeBenefit);
            TvPrimePlans = FindViewById<TextView>(Resource.Id.tvPrimePlans);
            TvItemNoPrime1 = FindViewById<TextView>(Resource.Id.tvPrimeItem1);
            TvItemNoPrime2 = FindViewById<TextView>(Resource.Id.tvPrimeItem2);
            TvItemNoPrime3 = FindViewById<TextView>(Resource.Id.tvPrimeItem3);
            TvItemNoPrime4 = FindViewById<TextView>(Resource.Id.tvPrimeItem4);
            TvItemNoPrime5 = FindViewById<TextView>(Resource.Id.tvPrimeItem5);
            LyButtonGotoExito = FindViewById<LinearLayout>(Resource.Id.lyButtonGotoExito);
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            LyButtonGotoExito.Click += delegate { GotoExito(); };
        }

        private void GotoExito()
        {
            try
            {
                Uri uri = Uri.Parse(AppConfigurations.PurchasePrime);
                Intent intent = new Intent(Intent.ActionView, uri);
                StartActivity(intent);
            }
            catch (UriFormatException) { }
        }

        private void Fonts(TextView textView, int textStart, int TextEnd, string text)
        {
            SpannableStringBuilder strfont = new SpannableStringBuilder(text);
            strfont.SetSpan(new StyleSpan(TypefaceStyle.Bold), textStart, TextEnd, SpanTypes.ExclusiveExclusive);
            textView.TextFormatted = strfont;
        }

        private async Task GetUser()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await _myAccountModel.GetUser();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                }
                else
                {
                    SetUser(response.User);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ClientPrimeActivity, ConstantMethodName.GetUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void SetUser(User user)
        {
            UserContext userContext = ModelHelper.UpdateUserContext(ParametersManager.UserContext, user);
            userContext.Address = ParametersManager.UserContext.Address;
            userContext.Store = ParametersManager.UserContext.Store;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
            this.PutData();
        }
    }
}