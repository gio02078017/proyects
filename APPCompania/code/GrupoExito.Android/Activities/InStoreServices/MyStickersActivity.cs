using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.InStoreServices
{
    [Activity(Label = "Mis Stickers", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MyStickersActivity : BaseActivity, ViewPager.IOnPageChangeListener
    {
        #region Controls

        private ViewPager VpGallery;
        private View[] DotsGalleryViews;
        private LinearLayout LyImagecount;
        private GalleryStickersAdapter _GalleryStickersAdapter;
        private TextView TvDescriptionStickers;
        private TextView TvPromotion;
        private TextView TvPriceOtherMedium;
        private TextView TvQuantityStickers;
        private TextView TvDate;
        private LinearLayout LyUpdateDate;

        #endregion

        #region Properties

        private int KillersCurrentPosition { get; set; }
        private int AlreadyPurchasedCurrentPosition { get; set; }
        private int CouldLikeCurrentPosition { get; set; }
        private int CouponManiaCurrentPosition { get; set; }
        private StickersModel stickersModel;
        StickersResponse response;
        private bool BackLobbyActivity { get; set; }
        private bool IsOpenTerms { get; set; }

        #endregion

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityStickers);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));
            HideItemsCarToolbar(this);
            stickersModel = new StickersModel(new StickersService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            await this.GetStickers();
        }

        protected override void OnResume()
        {
            base.OnResume();
            IsOpenTerms = false;
            DefineNoInfoLayout();
        }

        protected override void EventCloseModalMessageInfo()
        {
            base.EventCloseModalMessageInfo();
            MessageInfoDialog.Hide();
        }

        protected override async void EventError()
        {
            base.EventError();
            await this.GetStickers();
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
        }

        public override void OnTrimMemory([GeneratedEnum] TrimMemory level)
        {
            base.OnTrimMemory(level);
            Glide.Get(this).TrimMemory((int)level);
        }

        private void OpenTerms()
        {
            if (!IsOpenTerms)
            {
                IsOpenTerms = true;
                Intent intent = new Intent(this, typeof(TermsActivity));
                intent.PutExtra(ConstantPreference.Terms, AppConfigurations.TermsAndConditionStickers);
                intent.PutExtra(ConstantPreference.TermsStickers, true);
                StartActivity(intent);
            }
        }

        private async Task GetStickers()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                response = await stickersModel.GetSckers();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    ValidateResponseStickers();
                }
                else
                {
                    LyUpdateDate.Visibility = ViewStates.Visible;
                }

                if (response.StickersPage != null && response.StickersPage.Any())
                {
                    DrawImage();
                    DrawStickers();
                    DrawPromotion();
                }
            }
            catch (Exception exception)
            {
                ShowErrorLayout(AppMessages.UnexpectedErrorMessage);
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.StickersActivity, ConstantMethodName.GetStickers } };
                RegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ValidateResponseStickers()
        {
            RunOnUiThread(() =>
            {
                HideProgressDialog();

                var errorNotHaveStickers = response.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.NotHaveStickers)).Any();

                if (errorNotHaveStickers)
                {
                    LyUpdateDate.Visibility = ViewStates.Visible;
                }
                else
                {
                    LyUpdateDate.Visibility = ViewStates.Invisible;
                }

                OnBoxMessageTouched(MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);

            });
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvTitleStickers).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvDescriptionStickers.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            TvPromotion.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);           
            TvPriceOtherMedium.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvTitlePayOtherMedium).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);           
            TvQuantityStickers.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvTitleDate).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            TvDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvNameStickerEmpty).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            FindViewById<TextView>(Resource.Id.tvNameStickerFull).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);

        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));

            DefineNoInfoLayout();

            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                FindViewById<LinearLayout>(Resource.Id.lyBody));

            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);

            VpGallery = FindViewById<ViewPager>(Resource.Id.vpGallery);
            LyImagecount = FindViewById<LinearLayout>(Resource.Id.lyImagecount);
            TvDescriptionStickers = FindViewById<TextView>(Resource.Id.tvDescriptionStickers);
            TvPromotion = FindViewById<TextView>(Resource.Id.tvPromotion);
            TvPriceOtherMedium = FindViewById<TextView>(Resource.Id.tvPriceOtherMedium);
            TvQuantityStickers = FindViewById<TextView>(Resource.Id.tvQuantityStickers);
            TvDate = FindViewById<TextView>(Resource.Id.tvDate);
            LyUpdateDate = FindViewById<LinearLayout>(Resource.Id.lyUpdateDate);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

            TvDescriptionStickers.Click += delegate
            {
                OpenTerms();
            };

            this.CustomText();
        }

        private void CustomText()
        {
            SpannableStringBuilder strDescriptionStickers = new SpannableStringBuilder(GetString(Resource.String.str_description_stickers));
            int size = strDescriptionStickers.Length();
            int start = size - 7;
            ForegroundColorSpan fcs = new ForegroundColorSpan(this.Resources.GetColor(Resource.Color.colorLink));
            strDescriptionStickers.SetSpan(fcs, start, size, SpanTypes.ExclusiveExclusive);
            strDescriptionStickers.SetSpan(new StyleSpan(TypefaceStyle.Bold), start, size, SpanTypes.ExclusiveExclusive);
            TvDescriptionStickers.TextFormatted = strDescriptionStickers;
        }

        private void DefineNoInfoLayout()
        {
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                     FindViewById<LinearLayout>(Resource.Id.lyBody),
                     AppMessages.NoStickersMessage, AppMessages.GenericBackAction);
        }

        private void DrawImage()
        {

            DotsGalleryViews = new View[response.StickersPage.Count];

            if (response.StickersPage.Count < 2)
            {
                LyImagecount.Visibility = ViewStates.Gone;
            }

            for (int i = 0; i < response.StickersPage.Count; i++)
            {
                View view = new TextView(this);
                LinearLayout.LayoutParams parameters = new LinearLayout.LayoutParams(new LinearLayout.LayoutParams(20, 20));
                parameters.SetMargins(0, 0, 20, 0);
                view.LayoutParameters = parameters;

                if (i == 0)
                {
                    view.SetBackgroundResource(Resource.Drawable.circle_solid);
                }
                else
                {
                    view.SetBackgroundResource(Resource.Drawable.circle_stroke);
                }

                DotsGalleryViews[i] = view;
                LyImagecount.AddView(DotsGalleryViews[i]);
            }
        }

        private void DrawStickers()
        {
            RunOnUiThread(() =>
            {
                if (response.StickersPage != null && response.StickersPage.Count > 0)
                {
                    this.ChangeSize(150);
                    _GalleryStickersAdapter = new GalleryStickersAdapter(this, response.StickersPage);
                    VpGallery.Adapter = _GalleryStickersAdapter;
                    VpGallery.AddOnPageChangeListener(this);
                }
            });

            HideProgressDialog();
        }

        private void DrawPromotion()
        {
            TvPriceOtherMedium.Text = StringFormat.ToPrice(convertFromStringToDecimal(GetString(Resource.String.str_price_other_medium_of_paid)));
            TvQuantityStickers.Text = GetString(Resource.String.str_stickers_promotion);
            TvQuantityStickers.Text = GetString(Resource.String.str_stickers_promotion);
            TvDate.Text = response.LastUpdateStickers;
        }

        private decimal convertFromStringToDecimal(string texto)
        {
            decimal price = (decimal)ConvertUtilities.StringToDouble(texto);
            return price;
        }

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageSelected(int position)
        {
            for (int i = 0; i < response.StickersPage.Count; i++)
            {
                DotsGalleryViews[i].SetBackgroundResource(Resource.Drawable.circle_stroke);
            }

            DotsGalleryViews[position].SetBackgroundResource(Resource.Drawable.circle_solid);
        }

        private void ChangeSize(double size)
        {
            LinearLayout.LayoutParams linearLayoutParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent,
            ConvertDpToPixels(size));
            linearLayoutParams.SetMargins(0, 0, 0, 0);
            VpGallery.LayoutParameters = linearLayoutParams;
        }

        private int ConvertDpToPixels(double dp)
        {
            int pixelValue = Convert.ToInt32(dp * (double)this.Resources.DisplayMetrics.Density);
            return pixelValue;
        }
    }
}
