using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class StepThreeView : UIView
    {
        #region Constructors
        public StepThreeView(IntPtr handle) : base(handle)
        {
            //Default Constructor this class
        }
        #endregion

        #region Overrides Method
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadFonts();
        }
        #endregion

        #region Methods
        public void SetTotalPrice(string price)
        {
            this.totalPriceDescriptionStatusLabel.Text = price;
        }

        public void SetStatus(bool status)
        {
            if (status)
            {
                this.checkStatusView.Layer.BorderWidth = 0;
                this.checkStatusImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
            }
            else
            {
                this.checkStatusImageView.Hidden = true;
                this.checkStatusView.BackgroundColor = ConstantColor.UiBackgroundCheckStatusNotSelected;
            }
        }

        public void SetTitle(string text)
        {
            this.titleLabel.Text = text ?? string.Empty;
        }

        public void SetDescription(string text)
        {
            this.messageTitleLabel.Text = text ?? string.Empty;
        }

        public void OrderCancel()
        {
            this.checkStatusView.Layer.BorderColor = ConstantColor.UiBackgroundCheckStatusCanceled.CGColor;
            this.verticalLineView.Layer.BackgroundColor = ConstantColor.UiBackgroundCheckStatusCanceled.CGColor;
            this.checkStatusImageView.Hidden = true;
            this.checkStatusView.BackgroundColor = ConstantColor.UiBackgroundCheckStatusNotSelected;
            this.statusImageView.Image = UIImage.FromFile(ConstantImages.PuertaPrimario);
        }

        private void LoadCorners()
        {
            this.checkStatusView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            this.checkStatusView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            this.checkStatusView.Layer.CornerRadius = (this.checkStatusView.Frame.Width) / 2;

            this.containerDescriptionStatusView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            this.containerDescriptionStatusView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            this.containerDescriptionStatusView.Layer.BorderWidth = ConstantStyle.BorderWidth;
        }

        private void LoadFonts()
        {
            this.titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepThreeTitle);
            this.messageTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepThreeSubtitle);
            this.messageDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepThreeMessageDescription);
            this.totalPriceDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepThreeTotalPriceDescription);
        }
        #endregion
    }
}

