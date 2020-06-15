using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Request.Target;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Logic.Models.Generic;
using Java.Lang;
using android = Android;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Consulta de soat", ScreenOrientation = ScreenOrientation.Portrait)]
    public class SoatActivity : BaseActivity, View.IOnTouchListener
    {
        #region Controls

        private AppBarLayout AblGallery;
        private ImageView IvSOAT;
        private LinearLayout LySOAT;
        private LinearLayout LyErrorSoat;
        private TextView TvMessageError;
        private LinearLayout LyReturn;
        private TextView TvReturn;

        #endregion

        #region Porperties

        private string QR { get; set; }
        private DocumentsDataBaseModel _DocumentsDataBaseModel { get; set; }

        #endregion

        protected override void OnResume()
        {
            base.OnResume();
            ResumeSoatScreenView();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivitySoat);
            _DocumentsDataBaseModel = new DocumentsDataBaseModel(DocumentsDataBase.Instance);
            SetActionBar(FindViewById<Toolbar>(Resource.Id.mainToolbar));
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            Drawable drawable = FindViewById<Toolbar>(Resource.Id.mainToolbar).NavigationIcon;
            drawable.SetColorFilter(new Color(ContextCompat.GetColor(this, Resource.Color.colorWhite)), PorterDuff.Mode.SrcAtop);
            ActionBar.Title = "";

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            this.SetSOATImage();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void SetSOATImage()
        {
            byte[] imageByteArray = Base64.Decode(ParametersManager.QRCode, Base64Flags.Default);
            Glide.With(ApplicationContext).AsBitmap()
            .Load(imageByteArray)
            .Listener(new MyRequestListener(this))
            .Into(IvSOAT);
        }

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

        private void SetControlsProperties()
        {
            AblGallery = FindViewById<AppBarLayout>(Resource.Id.ablGallery);
            IvSOAT = FindViewById<ImageView>(Resource.Id.ivSOAT);
            LySOAT = FindViewById<LinearLayout>(Resource.Id.lySOAT);
            LyErrorSoat = FindViewById<LinearLayout>(Resource.Id.lyErrorSoat);
            TvMessageError = FindViewById<TextView>(Resource.Id.tvMessageError);
            LyReturn = FindViewById<LinearLayout>(Resource.Id.lyReturn);
            TvReturn = FindViewById<TextView>(Resource.Id.tvReturn);
            CustomFonts(TvMessageError, 24, 27, GetString(Resource.String.str_error_soat));

            IvSOAT.SetOnTouchListener(this);

            TvReturn.Click += delegate
            {
                OnBackPressed();
                Finish();
            };
        }

        private void EditFonts()
        {
            TvMessageError.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvReturn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        public class MyRequestListener : Java.Lang.Object, IRequestListener
        {
            SoatActivity _SoatActivity;
            public MyRequestListener(SoatActivity _SoatActivity)
            {
                this._SoatActivity = _SoatActivity;
            }

            public bool OnLoadFailed(GlideException p0, Object p1, ITarget p2, bool p3)
            {
                _SoatActivity.Error();
                return false;
            }

            public bool OnResourceReady(Object p0, Object p1, ITarget p2, DataSource p3, bool p4)
            {
                return false;
            }
        }

        public void Error()
        {
            _DocumentsDataBaseModel.DeleteSoat(ParametersManager.SoatPlate);
            AblGallery.Visibility = ViewStates.Visible;
            LySOAT.Visibility = ViewStates.Gone;
            LyErrorSoat.Visibility = ViewStates.Visible;
        }

        private void CustomFonts(TextView textView, int textStart, int TextEnd, string text)
        {
            SpannableStringBuilder strfont = new SpannableStringBuilder(text);
            strfont.SetSpan(new StyleSpan(TypefaceStyle.Bold), textStart, TextEnd, SpanTypes.ExclusiveExclusive);
            textView.TextFormatted = strfont;
        }

        private void ResumeSoatScreenView()
        {
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ResumeSoat, typeof(SoatActivity).Name);
        }
    }
}