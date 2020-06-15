using System;
using Foundation;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using UIKit;
using WebKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class NotPrimeViewController : UIViewControllerBase
    {
        public NotPrimeViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ClientPrime, nameof(PrimeCustomerViewController));
        }


        public override void ViewDidLoad()
        {           
            base.ViewDidLoad();
            this.LoadCorners();
            this.LoadHandlers();
            this.LoadBold();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        #region Methods 

        private void LoadCorners()

        {
            viewMes.Layer.CornerRadius = ConstantStyle.CornerRadius;
            viewAnual.Layer.CornerRadius = ConstantStyle.CornerRadius;
            viewMes.Layer.BorderColor = ConstantColor.UiBorderColorButton.CGColor;
            viewAnual.Layer.BorderColor = ConstantColor.UiBorderColorButton.CGColor;
            viewAnual.Layer.BorderWidth = ConstantStyle.BorderWidth;
            viewMes.Layer.BorderWidth = ConstantStyle.BorderWidth;
            exitoButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadHandlers()
        {
            closeButton.TouchUpInside += CloseButtonTouchUpInside;
            exitoButton.TouchUpInside += ExitoButtonTouchUpInside;
        }


        private void LoadBold()
        {

 
            var attributedText = new NSMutableAttributedString("Envío gratis ", UIFont.FromName(ConstantFontSize.LetterTitle, 17f));
            var infoText = new NSMutableAttributedString("en productos de tecnología,decoración,hogar y moda.", UIFont.FromName(ConstantFontSize.LetterSubtitle, 17f));

            var attributedText2 = new NSMutableAttributedString("Envío gratis ", UIFont.FromName(ConstantFontSize.LetterTitle, 17f));
            var infoText2 = new NSMutableAttributedString("en productos de mercado por compras superiores a $50,000.", UIFont.FromName(ConstantFontSize.LetterSubtitle, 17f));

            var attributedText3 = new NSMutableAttributedString("Entrega express ", UIFont.FromName(ConstantFontSize.LetterTitle, 17f));
            var infoText3 = new NSMutableAttributedString("en productos de tecnologia, decoración,hogar y moda 48 horas hábiles.", UIFont.FromName(ConstantFontSize.LetterSubtitle, 17f));

            var attributedText4 = new NSMutableAttributedString("Hasta 10 días ", UIFont.FromName(ConstantFontSize.LetterTitle, 17f));
            var infoText4 = new NSMutableAttributedString("hábiles de devoluciones de tus productos.", UIFont.FromName(ConstantFontSize.LetterSubtitle, 17f));

            attributedText.Append(infoText);
            attributedText2.Append(infoText2);
            attributedText3.Append(infoText3);
            attributedText4.Append(infoText4);
            labelBold1.AttributedText = attributedText;
            labelBold2.AttributedText = attributedText2;
            labelBold3.AttributedText = attributedText3;
            labelBold4.AttributedText = attributedText4;

        }

        #endregion

        private void CloseButtonTouchUpInside(object sender, EventArgs e)
        {
            this.NavigationController.PopToRootViewController(true);
        }

        private void ExitoButtonTouchUpInside(object sender, EventArgs e)
        {

            var habeasDataUrl = new NSUrl(AppConfigurations.Exitoprime);
            var req = new NSUrlRequest(habeasDataUrl);
            UIViewController habeasDataViewController = new UIViewController();
            WKWebView webView = new WKWebView(View.Bounds, new WKWebViewConfiguration());
            habeasDataViewController.View.AddSubview(webView);
            var request = new NSUrlRequest(habeasDataUrl);
            webView.LoadRequest(request);
            NavigationController.PushViewController(habeasDataViewController, true);
        }
    }
}
