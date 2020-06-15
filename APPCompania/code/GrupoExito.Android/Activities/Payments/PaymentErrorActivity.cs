using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.Content;
using Android.Text;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities.Constants;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Android.Activities.Payments
{
    [Activity(Label = "Pago Erroneo", ScreenOrientation = ScreenOrientation.Portrait)]
    public class PaymentErrorActivity : BaseProductActivity
    {
        #region Controls

        private Button BtnReturnPayMent;
        private ImageView ImgError;
        private TextView TvApologyPayMent, TvCannotPayMent;

        #endregion

        #region Properties

        public string Error { get; set; }
        private ProductCarModel _productCarModel;
        private string CurrentOrder { get; set; }
        #endregion

        public override void OnBackPressed()
        {           
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityPaymentError);
            _productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            SetActionBar(GetMainToolbar(Resource.Id.mainToolbar, this));

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            HideItemsToolbar(this);
            this.EditFonts();            
            ValidError();
        }

        private void ValidError()
        {
            Error = Intent.GetStringExtra(ConstantPreference.PaymentErrorResponse);
            ValidateButtonBack();
            Glide.With(ApplicationContext).Load(ConvertUtilities.ResourceId("error")).Thumbnail(0.1f).Into(ImgError);
            TvApologyPayMent.Text = AppMessages.ApologyPaymentErrorTitle;
            TvApologyPayMent.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.red)));

            switch (Error)
            {
                case ConstStatusPay.Pending:
                    Glide.With(ApplicationContext).Load(ConvertUtilities.ResourceId("pago_pendiente")).Thumbnail(0.1f).Into(ImgError);
                    TvApologyPayMent.Text = AppMessages.PendingPaymentTitle;
                    TvApologyPayMent.SetTextColor(new Color(ContextCompat.GetColor(this, Resource.Color.colorDescription)));
                    this.DrawOrderIsPending(AppMessages.PendingPaymentMessage, CurrentOrder);
                    break;
                case ConstStatusPay.Rejected:
                    TvApologyPayMent.Text = AppMessages.RejectedPaymentTitle;
                    TvCannotPayMent.Text = AppMessages.RejectedPaymentMessage;
                    break;
                case ConstStatusPay.OtherError:
                    TvCannotPayMent.Text = AppMessages.ApologyPaymentErrorMessage;
                    break;
                case ConstStatusPay.DeliveryError:                    
                    TvCannotPayMent.Text = AppMessages.ApologyPaymentDeliveryErrorMessage;
                    break;
                default:
                    TvCannotPayMent.Text = AppMessages.ApologyPaymentErrorMessage;
                    break;
            }
        }

        private void SetControlsProperties()
        {
            SetToolbarControls(FindViewById<TextView>(Resource.Id.tvToolbarPrice), FindViewById<TextView>(Resource.Id.tvToolbarQuantity));
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            ImgError = FindViewById<ImageView>(Resource.Id.imgError);
            TvApologyPayMent = FindViewById<TextView>(Resource.Id.tvApologyPayMent);
            TvCannotPayMent = FindViewById<TextView>(Resource.Id.tvCannotPayMent);
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            BtnReturnPayMent = FindViewById<Button>(Resource.Id.btnReturnPayMent);
        }

        private void ValidateButtonBack()
        {
            if(Error.Equals(ConstStatusPay.Pending))
            {
                ClearOrder();
                BtnReturnPayMent.Click += delegate { GoToNextBuying(); };
                IvToolbarBack.Click += delegate { GoToNextBuying(); };
                BtnReturnPayMent.Text = AppMessages.MessageButtonPaymentPending;
            }
            else
            {
                BtnReturnPayMent.Text = AppMessages.MessageButtonPaymentError;
                BtnReturnPayMent.Click += delegate { OnBackPressed(); };
                IvToolbarBack.Click += delegate { OnBackPressed(); };
            }
        }

        private void GoToNextBuying()
        {
            Intent intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void ClearOrder()
        {
            _productCarModel.FlushCar();
            var order = ParametersManager.Order;
            CurrentOrder = order.Id;
            order = new Entities.Order();
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
        }

        private void EditFonts()
        {
            FindViewById<TextView>(Resource.Id.tvApologyPayMent).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvCannotPayMent).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnReturnPayMent.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private void DrawOrderIsPending(string message, string idOrder)
        {
            string strItem = string.Format(message, idOrder);
            int startItem = 18;
            int endItem = 18 + idOrder.Length;
            SpannableStringBuilder strSpannableMessageCustom = new SpannableStringBuilder(strItem);
            strSpannableMessageCustom.SetSpan(new StyleSpan(TypefaceStyle.Bold), startItem, endItem, SpanTypes.ExclusiveExclusive);
            TvCannotPayMent.TextFormatted = strSpannableMessageCustom;
        }
    }
}