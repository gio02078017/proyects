using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Utilities.Constants;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Términos y condiciones", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TermsActivity : BaseActivity
    {
        private bool TermsStickers { get; set; }

        public override void OnBackPressed()
        {
            Finish();
        }

        public void ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
        }

        public void ShouldOverrideUrlLoading(WebView view, string request)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityTerms);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            SetActionBar(toolbar);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
        }

        private void SetControlsProperties()
        {
            string referentia = string.Empty;

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Terms)))
            {
                referentia = Intent.Extras.GetString(ConstantPreference.Terms);
            }

            TermsStickers = Intent.GetBooleanExtra(ConstantPreference.TermsStickers, false);

            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            WebView webviewTerms = FindViewById<WebView>(Resource.Id.webviewTerms);
            WebSettings webSettings = webviewTerms.Settings;
            webSettings.JavaScriptEnabled = true;
            webSettings.JavaScriptCanOpenWindowsAutomatically = true;
            webSettings.DomStorageEnabled = true;

            if (TermsStickers)
            {
                webSettings.BuiltInZoomControls = true;
                webSettings.SetSupportZoom(true);
            }

            webviewTerms.SetWebViewClient(new WebViewClient());
            webviewTerms.LoadUrl(referentia);
            IvToolbarBack.Click += delegate { OnBackPressed(); };
        }
    }
}