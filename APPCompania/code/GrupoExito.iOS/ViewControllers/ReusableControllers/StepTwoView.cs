using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class StepTwoView : UIView
    {
        #region Constructors
        public StepTwoView(IntPtr handle) : base(handle)
        {
            //Default constructor this class
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
            this.statusImageView.Image = UIImage.FromFile(ConstantImages.DomiciliosPrimario);
        }

        private void LoadCorners()
        {
            this.checkStatusView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            this.checkStatusView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            this.checkStatusView.Layer.CornerRadius = (this.checkStatusView.Frame.Width) / 2;
            this.checkStatusView.Layer.MasksToBounds = true;

            this.containerDescriptionStatusView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            this.containerDescriptionStatusView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            this.containerDescriptionStatusView.Layer.BorderWidth = ConstantStyle.BorderWidth;

            this.photoDomicileView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            this.photoDomicileView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            this.photoDomicileView.Layer.BorderWidth = ConstantStyle.BorderWidth;

            this.photoDomicileImageView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            this.photoDomicileImageView.Layer.MasksToBounds = true;
        }

        private void LoadFonts()
        {
            this.titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepTwoTitle);
            this.messageTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepTwoSubtitle);
            this.domicileTitleDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepTwoDomicileTitleDescription);
            this.domicileNameDescriptionStatusLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.StepTwoDomicileNameDescrption);
            this.whereIsDomicile.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.StepTwoWhereIsDomicile);
        }

        private void LoadHandlers()
        {
            this.starOneButton.TouchUpInside += StarOneButtonTouchUpInside;
            this.starTwoButton.TouchUpInside += StarTwoButtonTouchUpInside;
            this.starThreeButton.TouchUpInside += StarThreeButtonTouchUpInside;
            this.starFourButton.TouchUpInside += StarFourButtonTouchUpInside;
            this.starFiveButton.TouchUpInside += StarFiveButtonTouchUpInside;
        }
        #endregion

        #region Events
        private void StarOneButtonTouchUpInside(object sender, EventArgs e)
        {
            starOneButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starTwoButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
            starThreeButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
            starFourButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
            starFiveButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
        }

        private void StarTwoButtonTouchUpInside(object sender, EventArgs e)
        {
            starOneButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starTwoButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starThreeButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
            starFourButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
            starFiveButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
        }

        private void StarThreeButtonTouchUpInside(object sender, EventArgs e)
        {
            starOneButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starTwoButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starThreeButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starFourButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
            starFiveButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
        }

        private void StarFourButtonTouchUpInside(object sender, EventArgs e)
        {
            starOneButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starTwoButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starThreeButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starFourButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starFiveButton.SetImage(UIImage.FromFile(ConstantImages.Estrella), UIControlState.Normal);
        }

        private void StarFiveButtonTouchUpInside(object sender, EventArgs e)
        {
            starOneButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starTwoButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starThreeButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starFourButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
            starFiveButton.SetImage(UIImage.FromFile(ConstantImages.EstrellaPrimaria), UIControlState.Normal);
        }
        #endregion
    }
}

