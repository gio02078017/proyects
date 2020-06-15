using Android.App;
using Android.Content;
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
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Agregar tarjeta de credito", ScreenOrientation = ScreenOrientation.Portrait)]
    public class AddCreditCardActivity : BaseActivity, ICustomWebClient
    {
        #region Properties

        private string cookieValue = string.Empty;
        private readonly string cookieNameError = "is_error";

        #endregion

        public override void OnBackPressed()
        {
            base.OnBackPressed();
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
            string cookies = cookieManager.GetCookie(view.Url);

            if (!string.IsNullOrEmpty(cookies))
            {
                string[] cookiesSplit = cookies.Split(';');

                if (cookiesSplit != null)
                {
                    for (int i = 0; i < cookiesSplit.Length; i++)
                    {
                        string[] cookieSplit = cookiesSplit[i].Split('=');

                        if (cookieSplit != null && cookieSplit[0].Contains(cookieNameError))
                        {
                            cookieValue = cookieSplit[1];
                            break;
                        }
                    }

                    this.ResolveResponseWebView();
                }
            }
        }

        private void ResolveResponseWebView()
        {
            if (!string.IsNullOrEmpty(cookieValue))
            {
                if (cookieValue.Equals("true"))
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.AddCreditCardFailedMessage, AppMessages.AcceptButtonText);
                }
                else
                {
                    ParametersManager.AddCreditCard = true;
                    OnBackPressed();
                }
            }
        }

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            base.HandlerOkButton(sender, e);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityAddCreditCard);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            SetActionBar(toolbar);

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
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.AddCreditCard, typeof(AddCreditCardActivity).Name);
        }

        private void SetControlsProperties()
        {
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            WebView webView = FindViewById<WebView>(Resource.Id.webviewAddCreditCard);
            WebSettings webSettings = webView.Settings;
            webSettings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new CustomWebViewClient(this));
            string url = string.Format(AppServiceConfiguration.AddCreditCardEndPoint,
                ParametersManager.UserContext.Id, AppServiceConfiguration.SiteId.Equals("exito") ? EnumSiteId.Exito.GetHashCode().ToString()
                : EnumSiteId.Carulla.GetHashCode().ToString());
            webView.LoadUrl(url);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }
    }
}