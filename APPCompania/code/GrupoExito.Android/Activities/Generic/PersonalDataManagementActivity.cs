using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "Términos y condiciones", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PersonalDataManagementActivity : BaseActivity
    {
        public override void OnBackPressed()
        {
            Finish();
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
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            FindViewById<LinearLayout>(Resource.Id.lyUser).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            WebView webViewPersonalDataManagement = FindViewById<WebView>(Resource.Id.webviewTerms);
            WebSettings webSettings = webViewPersonalDataManagement.Settings;
            webSettings.JavaScriptEnabled = true;
            webViewPersonalDataManagement.SetWebViewClient(new WebViewClient());
            webViewPersonalDataManagement.LoadUrl(AppServiceConfiguration.PersonalDataManagementUrl);

            IvToolbarBack.Click += delegate { OnBackPressed(); };
        }
    }
}