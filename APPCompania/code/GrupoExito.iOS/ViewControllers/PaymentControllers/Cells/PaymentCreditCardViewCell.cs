using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Models.ViewModels.Payments;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    public partial class PaymentCreditCardViewCell : BasePaymentCell
    {
        #region Properties
        private EventHandler caduceCreditCard;
        private EventHandler activeCaduceWarningAction;
        private CreditCardViewModel viewModel;
        #endregion

        public static readonly NSString Key = new NSString("PaymentCreditCardViewCell");
        public static readonly UINib Nib;

        static PaymentCreditCardViewCell()
        {
            Nib = UINib.FromName("PaymentCreditCardViewCell", NSBundle.MainBundle);
        }

        #region Constructors 
        #endregion

        #region Overrides Methods
        public override void AwakeFromNib()
        {
            this.CreateShadowLayer();

            viewContent.Layer.CornerRadius = ConstantStyle.CornerRadius;
            viewContent.Layer.BorderWidth = 1.0f;
            viewContent.Layer.BorderColor = UIColor.Clear.CGColor;
        }

        protected PaymentCreditCardViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Methods

        private void CreateShadowLayer()
        {
            viewContent.Layer.ShadowOffset = new CGSize(5, 5);
            viewContent.Layer.ShadowRadius = 2.0f;
            viewContent.Layer.ShadowOpacity = 0.7f;
            viewContent.Layer.MasksToBounds = false;
        }

        private void ShowShadow()
        {
            viewContent.Layer.ShadowColor = UIColor.LightGray.CGColor;
        }

        private void HideShadow()
        {
            viewContent.Layer.ShadowColor = UIColor.Clear.CGColor;
        }
        #endregion

        #region Events

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            if (selected)
            {
                viewContent.BackgroundColor = ConstantColor.UiPrimary;
                numberCardLabel.TextColor = ConstantColor.DefaultSelectedText;

                checkboxImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                checkboxImageView.BackgroundColor = UIColor.Clear;

                ShowShadow();
            }
            else
            {
                viewContent.BackgroundColor = ConstantColor.UiGrayBackground;
                numberCardLabel.TextColor = ConstantColor.DefaultDeselectedText;

                checkboxImageView.Image = null;
                checkboxImageView.BackgroundColor = ConstantColor.UiGrayBackground;

                HideShadow();
            }
        }

        public override void SetData(CreditCardViewModel viewModel)
        {
            try
            {
                this.viewModel = viewModel;
                imageCardImageView.Hidden = false;
                imageCardImageView.Image = UIImage.FromFile(viewModel.CreditCard.Image);
                numberCardLabel.Text = viewModel.CreditCard.NumberCard.Substring(viewModel.CreditCard.NumberCard.Length - 4);

                SetSelected(viewModel.IsSelected, true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.CreditCardViewCell, ConstantMethodName.LoadData);
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            checkboxImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            checkboxImageView.Layer.CornerRadius = checkboxImageView.Layer.Frame.Size.Width / 2;
            checkboxImageView.ClipsToBounds = true;
            checkboxImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;
        }
        #endregion
    }
}
