using System;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class PaymentErrorViewController : UIViewControllerBase
    {
        #region Attributes
        private PaymentResponse _response;
        #endregion

        #region Properties
        public PaymentResponse Response { get => _response; set => _response = value; }
        #endregion

        #region Constructors
        public PaymentErrorViewController(IntPtr handle) : base(handle) 
        {
            //Default constructor this class
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ConfigureView();
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                //NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentErrorViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
            base.ViewWillAppear(animated);
        }
        #endregion

        #region Methods
        private void ConfigureView()
        {
            try
            {
                retryButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                retryButton.TouchUpInside += RetryButtonTouchUpInside;

                if (Response != null)
                {
                    switch (Response.StatusCode)
                    {
                        case ConstStatusPay.Pending:
                            imageView.Image = UIImage.FromFile(ConstantImages.PagoPendiente);
                            titleLabel.TextColor = UIColor.LightGray;
                            titleLabel.Text = AppMessages.PendingPaymentTitle;

                            NSMutableAttributedString attributedOriginalText = new NSMutableAttributedString(String.Format(AppMessages.PendingPaymentMessage, Response.OrderId));
                            NSRange range1 = attributedOriginalText.MutableString.LocalizedStandardRangeOfString(new NSString(Response.OrderId));
                            attributedOriginalText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterTitle, 16), range1);
                            reasonLabel.AttributedText = attributedOriginalText;
                            //reasonLabel.Text = AppMessages.PendingPaymentMessage;
                            retryButton.SetAttributedTitle(new NSAttributedString(AppMessages.MessageButtonPaymentPending), UIControlState.Normal);
                            retryButton.TouchUpInside -= RetryButtonTouchUpInside;
                            retryButton.TouchUpInside += ContinueShoping;
                            ConfigureNavigationAction(false);
                            ParametersManager.ProductUpdated = true;
                            break;
                        case ConstStatusPay.Rejected:
                            imageView.Image = UIImage.FromFile(ConstantImages.Error);
                            titleLabel.TextColor = UIColor.Red;
                            titleLabel.Text = AppMessages.RejectedPaymentTitle;
                            reasonLabel.Text = AppMessages.RejectedPaymentMessage;

                            ConfigureNavigationAction(true);
                            break;
                        case ConstStatusPay.OtherError:
                            imageView.Image = UIImage.FromFile(ConstantImages.Error);
                            titleLabel.TextColor = UIColor.Red;
                            titleLabel.Text = AppMessages.ApologyPaymentErrorTitle;
                            reasonLabel.Text = AppMessages.ApologyPaymentErrorMessage;

                            ConfigureNavigationAction(true);
                            break;
                        default:
                            imageView.Image = UIImage.FromFile(ConstantImages.Error);
                            titleLabel.TextColor = UIColor.LightGray;
                            titleLabel.Text = AppMessages.ApologyPaymentErrorTitle;
                            reasonLabel.Text = AppMessages.ApologyPaymentErrorMessage;

                            ConfigureNavigationAction(true);
                            break;
                    }
                }
                else if (Response.Result != null && Response.Result.HasErrors && Response.Result.Messages != null)
                {
                    imageView.Image = UIImage.FromFile(ConstantImages.Error);
                    titleLabel.TextColor = UIColor.LightGray;
                    titleLabel.Text = AppMessages.ApologyPaymentErrorTitle;
                    reasonLabel.Text = AppMessages.ApologyPaymentDeliveryErrorMessage;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentErrorViewController, ConstantMethodName.ConfigureView);
                ShowMessageException(exception);
            }
        }

        private void ConfigureNavigationAction(bool backAllowed)
        {
            NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            NavigationView.EnableBackButton(backAllowed);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = backAllowed;

            this.NavigationController.NavigationBar.Hidden = false;
            NavigationView.HiddenCarData();
            NavigationView.IsSummaryDisabled = true;
            NavigationView.HiddenAccountProfile();
            NavigationView.IsAccountEnabled = false;
        }
        #endregion

        #region Events
        private void RetryButtonTouchUpInside(object sender, EventArgs e)
        {
            PaymentContainerController paymentViewController = null;
                UIViewController[] vcs = this.NavigationController.ViewControllers;

                foreach (var vc in vcs)
                {
                    if (vc is PaymentContainerController)
                    {
                        paymentViewController = (PaymentContainerController)vc;
                        break;
                    }
                }

                if (paymentViewController != null)
                {
                    this.NavigationController.PopToViewController(paymentViewController, true);
                }
                else
                {
                this.NavigationController.PopViewController(true);
            }
        }

        private void ContinueShoping(object sender, EventArgs e)
        {
            this.NavigationController.PopToRootViewController(true);
        }
        #endregion
    }
}