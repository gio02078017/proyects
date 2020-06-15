using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class StepFourView : UIView
    {
        #region Constructors
        public StepFourView(IntPtr handle) : base(handle)
        {
            //default Constructor this class
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
            this.statusImageView.Image = UIImage.FromFile(ConstantImages.ListoPrimario);
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
            this.titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepFourTitle);
            this.messageTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepFourSubtitle);
            this.helpUsToQualifyLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepFourHelpUsDescription);
            this.rateYouExperienceButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepFourRateYouExperienceDescription);
        }

        private void LoadHandlers(){
            this.rateYouExperienceButton.TouchUpInside += RateYouExperienceButtonTouchUpInside;
        }
        #endregion

        #region Events
        private void RateYouExperienceButtonTouchUpInside(object sender, EventArgs e)
        {
            //Event to quality experience
        }
        #endregion
    }
}

