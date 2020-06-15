using Foundation;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using ObjCRuntime;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class CashPaymentView : UIView
    {
        public Action<EnumPaymentType> TypeSelected { get; set; }
        private nint optionSelected = -1;

        public CashPaymentView (IntPtr handle) : base (handle)
        {

        }

        public static CashPaymentView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(CashPaymentView), null, null);
            var v = Runtime.GetNSObject<CashPaymentView>(arr.ValueAt(0));
            return v;
        }

        public override void AwakeFromNib()
        {
            hintView.Layer.CornerRadius = 8;
            hintView.Layer.BorderColor = ConstantColor.CashPaymentHint.CGColor;
            hintView.Layer.BorderWidth = 2;

            cashButton.TouchUpInside += Button_TouchUpInside;
            dataphoneButton.TouchUpInside += Button_TouchUpInside;

            cashCheckboxImageView.Layer.CornerRadius = cashCheckboxImageView.Frame.Size.Width / 2;
            dataphoneCheckboxImageView.Layer.CornerRadius = dataphoneCheckboxImageView.Frame.Size.Width / 2;

            cashCheckboxImageView.ClipsToBounds = true;
            dataphoneCheckboxImageView.ClipsToBounds = true;

            cashCheckboxImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;
            dataphoneCheckboxImageView.Layer.BorderWidth = ConstantStyle.CircumferenceBorderWidth;

            cashCheckboxImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;
            dataphoneCheckboxImageView.Layer.BorderColor = ConstantColor.UiFilterOrderTextSelected.CGColor;

            cashBackgroundView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            dataphoneBackgroundView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            SetDateLabel();
        }

        private void SetOptionSelected()
        {
            switch (optionSelected)
            {
                case 0:
                    SelectCash(true);
                    SelectDataphone(false);
                    break;
                case 1:
                    SelectDataphone(true);
                    SelectCash(false);
                    break;
                default:
                    SelectCash(false);
                    SelectDataphone(false);
                    break;
            }
        }

        private void SelectDataphone(bool select)
        {
            if (select)
            {
                dataphoneBackgroundView.BackgroundColor = ConstantColor.UiPrimary;
                dataphoneCheckboxImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                dataphoneCheckboxImageView.BackgroundColor = UIColor.Clear;
                dataphoneLabel.TextColor = ConstantColor.DefaultSelectedText;
                dataphoneDescriptionLabel.TextColor = ConstantColor.DefaultSelectedText;
            }
            else
            {
                dataphoneBackgroundView.BackgroundColor = ConstantColor.UiGrayBackground;
                dataphoneCheckboxImageView.Image = null;
                dataphoneCheckboxImageView.BackgroundColor = ConstantColor.UiGrayBackground;
                dataphoneLabel.TextColor = ConstantColor.DefaultDeselectedText;
                dataphoneDescriptionLabel.TextColor = ConstantColor.DefaultDeselectedText;
            }
        }

        private void SelectCash(bool select)
        {
            if (select)
            {
                cashBackgroundView.BackgroundColor = ConstantColor.UiPrimary;
                cashCheckboxImageView.Image = UIImage.FromFile(ConstantImages.SeleccionarCirculo);
                cashCheckboxImageView.BackgroundColor = UIColor.Clear;
                cashLabel.TextColor = ConstantColor.DefaultSelectedText;
                cashDescriptionLabel.TextColor = ConstantColor.DefaultSelectedText;
            }
            else
            {
                cashBackgroundView.BackgroundColor = ConstantColor.UiGrayBackground;
                cashCheckboxImageView.Image = null;
                cashCheckboxImageView.BackgroundColor = ConstantColor.UiGrayBackground;
                cashLabel.TextColor = ConstantColor.DefaultDeselectedText;
                cashDescriptionLabel.TextColor = ConstantColor.DefaultDeselectedText;
            }
        }

        void Button_TouchUpInside(object sender, EventArgs e)
        {
            UIButton paymentTypeButton = (UIButton)sender;
            if (paymentTypeButton.Tag != optionSelected)
            {
                optionSelected = paymentTypeButton.Tag;
                SetOptionSelected();

                switch (optionSelected)
                {
                    case 0:
                        TypeSelected?.Invoke(EnumPaymentType.Cash);
                        break;
                    case 1:
                        TypeSelected?.Invoke(EnumPaymentType.Dataphone);
                        break;
                    default:
                        break;
                }
            }
        }

        internal void ClearSelection()
        {
            optionSelected = -1;
            SetOptionSelected();
        }

        internal void SetDateLabel()
        {

            string originalText = string.Format(AppMessages.CreditCardDateMessage, ParametersManager.Order.DateSelected) + " / " + string.Format(AppMessages.CashOnDeliveryHourMessage, ParametersManager.Order.Schedule);
            NSMutableAttributedString attributedOriginalText = new NSMutableAttributedString(originalText);

            NSRange range1 = attributedOriginalText.MutableString.LocalizedStandardRangeOfString(new NSString("Fecha:"));
            attributedOriginalText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterTitle, 15), range1);

            NSRange range2 = attributedOriginalText.MutableString.LocalizedStandardRangeOfString(new NSString("Hora:"));
            attributedOriginalText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterTitle, 15), range2);

            dateHourLabel.AttributedText = attributedOriginalText;
        }
}
}