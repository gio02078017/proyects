using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Tutorial Listas", ScreenOrientation = ScreenOrientation.Portrait)]
    public class TutorialListActivity : BaseActivity
    {
        #region Controls
        private LinearLayout LyAddProduct;

        #endregion

        #region Protected Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityTutorialList);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();

        }

        #endregion

        #region Private Methods

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitle).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageStart).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessageEnd).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAddProduct).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            LyAddProduct = FindViewById<LinearLayout>(Resource.Id.lyAddProduct);
   
            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            LyAddProduct.Click += delegate
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
                Finish();
            };
                       
        }

        #endregion
    }
}