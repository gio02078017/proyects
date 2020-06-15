using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    public partial class PaymentCashViewCell : BasePaymentCell
    {
        public static readonly NSString Key = new NSString("PaymentCashViewCell");
        public static readonly UINib Nib;

        private CreditCardViewModel viewModel;

        static PaymentCashViewCell()
        {
            Nib = UINib.FromName("PaymentCashViewCell", NSBundle.MainBundle);
        }

        protected PaymentCashViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            CreateShadowLayer();

            viewContent.Layer.CornerRadius = ConstantStyle.CornerRadius;
            viewContent.Layer.BorderWidth = 1.0f;
            viewContent.Layer.BorderColor = UIColor.Clear.CGColor;
        }

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

        public override void SetData(CreditCardViewModel viewModel)
        {
            this.viewModel = viewModel;
            cardLabel.Text = AppMessages.PaymentDelivery;

            cashButton.TouchUpInside += CashButton_TouchUpInside;
            dataphoneButton.TouchUpInside += DataphoneButton_TouchUpInside;

            cashButton.Enabled = true;
            dataphoneButton.Enabled = true;

            SetSelected(viewModel.IsSelected, true);
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            if (selected)
            {
                cashStackView.Hidden = false;
                viewContent.BackgroundColor = ConstantColor.UiPrimary;
                cardLabel.TextColor = ConstantColor.DefaultSelectedText;

                checkboxImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                checkboxImageView.BackgroundColor = UIColor.Clear;

                ShowShadow();
            }
            else
            {
                cashStackView.Hidden = true;

                cashButton.Enabled = true;
                dataphoneButton.Enabled = true;

                viewContent.BackgroundColor = ConstantColor.UiGrayBackground;
                cardLabel.TextColor = ConstantColor.DefaultDeselectedText;

                checkboxImageView.Image = null;
                checkboxImageView.BackgroundColor = ConstantColor.UiGrayBackground;

                HideShadow();
            }
        }

        void CashButton_TouchUpInside(object sender, EventArgs e)
        {
            cashButton.Enabled = false;
            dataphoneButton.Enabled = true;
            ((CashViewModel)viewModel).CashTypeSelected.Execute(EnumPaymentType.Cash);
        }

        void DataphoneButton_TouchUpInside(object sender, EventArgs e)
        {
            cashButton.Enabled = true;
            dataphoneButton.Enabled = false;
            ((CashViewModel)viewModel).CashTypeSelected.Execute(EnumPaymentType.Dataphone);
        }

        public override void PrepareForReuse()
        {
            cashButton.Enabled = true;
            dataphoneButton.Enabled = true;

            cashButton.TouchUpInside -= CashButton_TouchUpInside;
            dataphoneButton.TouchUpInside -= DataphoneButton_TouchUpInside;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            checkboxImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            checkboxImageView.Layer.CornerRadius = checkboxImageView.Layer.Frame.Size.Width / 2;
            checkboxImageView.ClipsToBounds = true;
            checkboxImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;
        }
    }
}
