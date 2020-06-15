using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Products
{
    [Activity(Label = "Detalle del producto", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ProductDetailPriceCheckActivity : BaseActivity
    {
        #region Controls

        private TextView TvProductName;
        private TextView TvProductDetail;
        private TextView TvProductPrice;
        private TextView TvPriceForGrams;
        private ImageView IvOnlyImage;
        private LinearLayout LyImagecount;

        #endregion

        #region Properties


        private CheckerPriceModel checkerPriceModel;
        private String barcode;
        private String dependencyId;
        #endregion

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityDetailPriceCheck);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            checkerPriceModel = new CheckerPriceModel(new CheckerPriceService(DeviceManager.Instance));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            HideItemsToolbar(this);

            this.SetControlsProperties();
            this.EditFont();

            if (!string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.BarCode)) && !string.IsNullOrEmpty(Intent.Extras.GetString(ConstantPreference.DependencyId)))
            {
                barcode = Intent.Extras.GetString(ConstantPreference.BarCode);
                dependencyId = Intent.Extras.GetString(ConstantPreference.DependencyId);
                await GetProductDetailPriceCheck(barcode, dependencyId);

            }            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ProductDetailPriceCheck, typeof(ProductDetailPriceCheckActivity).Name);
        }

        protected override void EventError()
        {
            base.EventError();
            this.RunOnUiThread(async () =>
            {
                await this.GetProductDetailPriceCheck(barcode, dependencyId);
            });
        }

        protected override void EventNoInfo()
        {
            base.EventNoInfo();
            OnBackPressed();
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            SetNoInfoLayout(FindViewById<RelativeLayout>(Resource.Id.layoutNoInfo),
                             FindViewById<RelativeLayout>(Resource.Id.rlBody),
                             AppMessages.PriceCheckMessage, AppMessages.PriceCheckAction);
            SetErrorLayout(FindViewById<RelativeLayout>(Resource.Id.layoutError),
                           FindViewById<RelativeLayout>(Resource.Id.rlBody));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            LyImagecount = FindViewById<LinearLayout>(Resource.Id.lyImagecount);
            TvProductName = FindViewById<TextView>(Resource.Id.tvProductName);
            TvProductDetail = FindViewById<TextView>(Resource.Id.tvProductDetail);
            TvProductPrice = FindViewById<TextView>(Resource.Id.tvProductPrice);
            TvPriceForGrams = FindViewById<TextView>(Resource.Id.tvPriceForGrams);
            IvOnlyImage = FindViewById<ImageView>(Resource.Id.ivOnlyImage);

            IvToolbarBack.Click += delegate
            {
                OnBackPressed();
                Finish();
            };

        }

        private async Task GetProductDetailPriceCheck(String barcode, String dependencyId)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                CheckerPriceParameters parameters = new CheckerPriceParameters
                {
                    Barcode = barcode,
                    DependencyId = dependencyId,
                    Size = "XS"
                };

                CheckerPriceResponse response = await checkerPriceModel.CheckerPrice(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        var errorCode = (EnumErrorCode)Enum.Parse(typeof(EnumErrorCode), response.Result.Messages.First().Code);

                        if (errorCode == EnumErrorCode.ErrorSincoCheckPriceNotFound)
                        {
                            ShowNoInfoLayout();
                        }
                        else
                        {
                            ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                        }
                    });
                }
                else
                {
                    TvProductName.Text = response.Name;
                    TvProductDetail.Text = response.Presentation;
                    TvProductPrice.Text = StringFormat.ToPrice(Convert.ToDecimal(response.Price));
                    TvPriceForGrams.Text = response.Pum;

                    if (response.Image != null)
                    {
                        var requestOptions = new RequestOptions().Error(Resource.Drawable.sin_imagen);
                        Glide.With(ApplicationContext).AsBitmap().Apply(requestOptions).Load(response.Image).Thumbnail(0.1f).Into(IvOnlyImage);
                    }
                    else
                    {
                        IvOnlyImage.SetImageResource(Resource.Drawable.sin_imagen);
                    }

                    ShowBodyLayout();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ProductDetailPriceCheckActivity, ConstantMethodName.GetProductDetailPriceCheck } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void DrawImage(String url)
        {
            Glide.With(ApplicationContext).Load(url).Thumbnail(0.1f).Into(IvOnlyImage);
        }

        private void EditFont()
        {
            TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvProductDetail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvPriceForGrams = FindViewById<TextView>(Resource.Id.tvPriceForGrams);
            TvProductPrice.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPriceForGrams.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessagePriceReferenceShort).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvMessagePriceReferenceLong).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }
    }
}