using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class StepOneView : UIView
    {
        #region Constructors
        public StepOneView(IntPtr handle) : base(handle)
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
            this.LoadHandlers();
        }
        #endregion

        #region Methods
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
            this.statusImageView.Image = UIImage.FromFile(ConstantImages.Caja);
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
            this.titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepOneTitle);
            this.messageTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepOneSubtitle);
            this.titleDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepOneTitleDescription);
            this.messageDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepOneMessageDescrption);
            this.youHaveDoubtsDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepOneHaveYouDoubts);
            this.chatWithUsButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepOneChatUs);
        }

        private void LoadHandlers()
        {
            this.chatWithUsButton.TouchUpInside += ChatWithUsButtonTouchUpInside;
        }
        #endregion

        #region Events
        private void ChatWithUsButtonTouchUpInside(object sender, EventArgs e)
        {
            //Event to chat with 
        }
        #endregion
    }
}

