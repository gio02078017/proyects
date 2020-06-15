using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Utilities.Helpers;
using System.Collections.Generic;

namespace GrupoExito.Android.Activities.Generic
{
    [Activity(Label = "ImageDetailActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ImageDetailActivity : AppCompatActivity, View.IOnTouchListener, IGalleryAdapyer
    {
        #region Properties

        private GalleryAdapter GalleryAdapter;
        private ViewPager VpGallery;
        private IList<string> ImageList;
        private AppBarLayout AblGallery;

        #endregion

        #region Public Methods

        public override void OnBackPressed()
        {
            Finish();
        }

        public void OnImageTouched()
        {
            ValidateToolbarVisibility();
        }

        public bool OnTouch(View v, MotionEvent e)
        {
            if (e.Action == MotionEventActions.Down)
            {
                ValidateToolbarVisibility();
            }
            else
            {
                return false;
            }

            return true;
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            OnBackPressed();
            Finish();
            return true;
        }

        #endregion

        #region Protected Methods

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityGallery);

            SetActionBar(FindViewById<Toolbar>(Resource.Id.mainToolbar));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            Drawable drawable = FindViewById<Toolbar>(Resource.Id.mainToolbar).NavigationIcon;
            drawable.SetColorFilter(new Color(ContextCompat.GetColor(this, Resource.Color.colorWhite)), PorterDuff.Mode.SrcAtop);
            ActionBar.Title = "";

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            VpGallery = FindViewById<ViewPager>(Resource.Id.vpGallery);

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.Gallery)))
            {
                ImageList = JsonService.Deserialize<IList<string>>(Intent.Extras.GetString(ConstantPreference.Gallery));
                CreateGallery();
            }
            AblGallery = FindViewById<AppBarLayout>(Resource.Id.ablGallery);
            VpGallery.SetOnTouchListener(this);
        }

        #endregion

        #region Private Methods

        private void ValidateToolbarVisibility()
        {
            if (AblGallery.Visibility == ViewStates.Visible)
            {
                AblGallery.Visibility = ViewStates.Invisible;
            }
            else
            {
                AblGallery.Visibility = ViewStates.Visible;
            }
        }

        private void CreateGallery()
        {
            GalleryAdapter = new GalleryAdapter(this, ImageList, true, this);
            VpGallery.Adapter = GalleryAdapter;
            int galleryPosition = Intent.Extras.GetInt(ConstantPreference.GalleryPosition);
            VpGallery.Post(() =>
            {
                VpGallery.SetCurrentItem(galleryPosition, false);
            });
        }

        #endregion       
    }
}