using Foundation;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities.Constant;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class GenericErrorView : UIView
    {
        public GenericErrorView(IntPtr handle) : base(handle)
        {
        }

        public static GenericErrorView Create()
        {
            return NSBundle.MainBundle.LoadNib(nameof(GenericErrorView), null, null).GetItem<GenericErrorView>(0);
        }

        public override void AwakeFromNib()
        {
            tryAgainButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            tryAgainButton.BackgroundColor = ConstantColor.UiPrimary;
            messageLabel.TextColor = ConstantColor.UiMessageError;
        }

        public void Configure(string code, string message, EventHandler tryAgainHandler)
        {
            messageLabel.Text = message;
            switch (code)
            {
                case nameof(EnumErrorCode.InternetErrorMessage):
                    imageView.Image = UIImage.FromFile(ConstantImages.SinConexion);
                    break;
                case nameof(EnumErrorCode.InvalidExternalResponse):
                    imageView.Image = UIImage.FromFile(ConstantImages.SinConexion);
                    break;
                case nameof(EnumErrorCode.ErrorServiceUnavailable):
                    imageView.Image = UIImage.FromFile(ConstantImages.SinConexion);
                    break;
                case nameof(EnumErrorCode.ErrorDirectCommerce):
                    imageView.Image = UIImage.FromFile(ConstantImages.SinInformacion);
                    break;
                default:
                    imageView.Image = UIImage.FromFile(ConstantImages.SinInformacion);
                    break;
            }

            tryAgainButton.TouchUpInside += tryAgainHandler;
        }
    }
}