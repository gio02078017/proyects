using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Widgets;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Paymentez", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddCreditCardPaymentezActivity : BaseActivity, ICustomWebClient
    {
        #region Properties

        private readonly string cookieName = "cardResponse";
        private bool isAddCreditCard;

        private bool GetIsAddCreditCard()
        {
            return isAddCreditCard;
        }

        private void SetIsAddCreditCard(bool value)
        {
            isAddCreditCard = value;
        }

        private CreditCardModel creditCardModel;

        #endregion

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SetIsAddCreditCard(false);
            Finish();
        }

        public void ShouldOverrideUrlLoading(WebView view, string request)
        {
            this.GetCookieCreditCard(view);
        }

        public void ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            this.GetCookieCreditCard(view);
        }

        private void GetCookieCreditCard(WebView view)
        {
            CookieManager cookieManager = CookieManager.Instance;
            Handler handler = new Handler(Looper.MainLooper);
            AddCardResponse response = null;

            string cookies = cookieManager.GetCookie(view.Url);

            if (!string.IsNullOrEmpty(cookies) && cookies.Contains(cookieName))
            {
                response = creditCardModel.GetResponseAddCardPaymentez(cookies);

                if (response != null)
                {
                    this.ResolveResponseWebView(response);
                }
            }
        }

        private void ResolveResponseWebView(AddCardResponse response)
        {
            if (response.Response != null && !string.IsNullOrEmpty(response.Response.Status)
                && response.Response.Status.Equals(ConstStatusAddCard.Valid))
            {
                ParametersManager.AddCreditCard = true;
                OnBackPressed();
            }
            else
            {
                if (!GetIsAddCreditCard())
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.AddCreditCardFailedMessage, AppMessages.AcceptButtonText);
                }
                else
                {
                    ParametersManager.AddCreditCard = true;
                }

                SetIsAddCreditCard(true);
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityAddCreditCardPaymentez);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            creditCardModel = new CreditCardModel(new CreditCardService(DeviceManager.Instance));
            SetActionBar(toolbar);
            SetIsAddCreditCard(false);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
            HideItemsToolbar(this);            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.AddCreditCardPaymentez, typeof(AddCreditCardPaymentezActivity).Name);
        }

        private void SetControlsProperties()
        {
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            WebView webView = FindViewById<WebView>(Resource.Id.webviewAddCreditCardPaymentez);
            WebSettings webSettings = webView.Settings;
            webSettings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new CustomWebViewClient(this));
            string url = string.Format(AppServiceConfiguration.AddCreditCardPaymentezEndPoint,
                ParametersManager.UserContext.Id, ParametersManager.UserContext.Email, AppServiceConfiguration.SiteId);
            webView.LoadUrl(url);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }
    }
}