using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Registro Exitoso", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SuccessSignUpActivity : BaseActivity
    {
        #region Controls

        private Button BtnBegin;

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySuccessSignUp);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Gone;
            SetActionBar(toolbar);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
            HideItemsToolbar(this);
        }

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);            
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            FindViewById<TextView>(Resource.Id.tvSuccessSignUp).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvSavedYourData).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvNowEnjoy).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvCustomLists).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvBenefits).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvEnjoyBenefits).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnBegin = FindViewById<Button>(Resource.Id.btnBegin);
            BtnBegin.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            BtnBegin.Click += delegate { ValidateNavigation(); };
        }

        private void ValidateNavigation()
        {
            Intent intent = new Intent(this, typeof(LobbyActivity));
            StartActivity(intent);
            Finish();
        }
    }
}