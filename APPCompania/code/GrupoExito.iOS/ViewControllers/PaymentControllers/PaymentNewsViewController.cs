using System;
using Foundation;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class PaymentNewsViewController : UIViewController
    {

        private enum PaymentEscenery
        {
            PrimeModifiedBelowPrice,
            PrimeModifiedAbovePrice,
        }

        #region Attributes
        private ModifiedProductsViewController modifiedProductsViewController;
        private PaymentModel _paymentModel;
        private Action<bool> _continuePaymentAction;
        private bool isSpinnerAdded = false;
        private CustomSpinnerView customSpinnerView;
        #endregion

        #region Properties
        public Action<bool> ContinuePaymentAction { get => _continuePaymentAction; set => _continuePaymentAction = value; }
        public PaymentSummaryResponse SummaryResponse { get; set; }
        #endregion

        #region Constructors
        public PaymentNewsViewController(IntPtr handle) : base(handle)
        {
            _paymentModel = new PaymentModel(new PaymentService(DeviceManager.Instance));
        }
        #endregion

        #region LifeCycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                SetSubviews();
                SetPrimeView();
                LoadHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentNewsViewController, ConstantMethodName.ViewDidLoad);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                ConfigureNavigationBar();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentNewsViewController, ConstantMethodName.ViewWillAppear);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            try
            {
                if (this.IsMovingFromParentViewController)
                {
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, ConstantControllersName.PaymentNewsViewController, ConstantMethodName.ViewWillDisappear);
                //ShowMessageException(ex);
            }
        }
        #endregion

        #region Methods
        private void ConfigureNavigationBar()
        {
            this.NavigationController.NavigationBar.Hidden = false;
            NavigationHeaderView navigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            navigationView.HiddenCarData();
            navigationView.IsSummaryDisabled = false;
            navigationView.HiddenAccountProfile();
            navigationView.IsAccountEnabled = false;
            navigationView.EnableBackButton(true);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        }

        private void SetSubviews()
        {
            try
            {
                totalTitleValueLabel.Text = String.Format(AppMessages.TotalPaymentPrime.Substring(0, AppMessages.TotalPaymentPrime.IndexOf(',')), StringFormat.ToPrice(SummaryResponse.Total));

                NSMutableAttributedString attributedOriginalText = new NSMutableAttributedString(adviceLabel.Text);

                NSRange range1 = attributedOriginalText.MutableString.LocalizedStandardRangeOfString(new NSString("tu envío es gratis"));
                attributedOriginalText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterSubtitle, 13), range1);

                adviceLabel.AttributedText = attributedOriginalText;

                continuePaymentButton.Layer.CornerRadius = ConstantStyle.CornerRadius;

                if (SummaryResponse.Total <= 0 && SummaryResponse.CostRemaining == 0)
                {
                    //continuePaymentButton.RemoveFromSuperview();
                    continuePaymentButton.Hidden = true;
                }

                if (SummaryResponse.ProductsChanged.Count != 0 || SummaryResponse.ProductsRemoved.Count != 0)
                {
                    AddModifiedProductsController();
                }
                else modifiedProductsView.RemoveFromSuperview();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentNewsViewController, ConstantMethodName.SetViews);
                //ShowMessageException(exception);
            }
        }

        private void SetPrimeView()
        {
            if (SummaryResponse.IsPrime && SummaryResponse.CostRemaining > 0)
            {
                remainingCostLabel.Text = AppMessages.youAreMissing + StringFormat.ToPrice(SummaryResponse.CostRemaining) + AppMessages.toGetYourFreeShipping;








                NSMutableAttributedString attributedOriginalPriceText = new NSMutableAttributedString(remainingCostLabel.Text);
                NSRange range2 = attributedOriginalPriceText.MutableString.LocalizedStandardRangeOfString(new NSString(StringFormat.ToPrice(SummaryResponse.CostRemaining)));
                attributedOriginalPriceText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterSubtitle, 20), range2);

                remainingCostLabel.AttributedText = attributedOriginalPriceText;






                //totalView.RemoveFromSuperview();
                totalView.Hidden = true;
            }
            else
            {
                //primeView.RemoveFromSuperview();
                primeView.Hidden = true;
            }
        }

        private void EvaluateEsceneries(bool isPrime, decimal total, decimal costRemaining)
        {

        }

        private void ShowPurchaseSummary(PaymentSummaryResponse summaryResponse)
        {
            InvokeOnMainThread(() =>
            {
                PurchaseSummaryController purchaseSummaryController = new PurchaseSummaryController(summaryResponse);
                this.NavigationController.PushViewController(purchaseSummaryController, true);
            });
        }

        private void LoadHandlers()
        {
            try
            {
                continuePaymentButton.TouchUpInside += ((o, e) =>
                {
                    ShowPurchaseSummary(SummaryResponse);
                });

                backToSummaryButton.TouchUpInside += ((o, e) =>
                {
                    SummaryContainerController summaryViewController = null;
                    UIViewController[] vcs = this.NavigationController.ViewControllers;

                    foreach (var vc in vcs)
                    {
                        if (vc is SummaryContainerController)
                        {
                            summaryViewController = (SummaryContainerController)vc;
                            break;
                        }
                    }

                    if (summaryViewController != null)
                    {
                        this.NavigationController.PopToViewController(summaryViewController, true);
                    }
                    else
                    {
                        SummaryContainerController summaryContainer = (SummaryContainerController)this.Storyboard.InstantiateViewController(nameof(SummaryContainerController));
                        summaryContainer.HidesBottomBarWhenPushed = true;
                        this.NavigationController.PushViewController(summaryContainer, true);
                    }
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentNewsViewController, ConstantMethodName.LoadHandler);
                //ShowMessageException(exception);
            }
        }

        private void AddModifiedProductsController()
        {
            try
            {
                modifiedProductsViewController = new ModifiedProductsViewController(SummaryResponse.ProductsRemoved, SummaryResponse.ProductsChanged);
                this.AddChildViewController(modifiedProductsViewController);
                modifiedProductsViewController.DidMoveToParentViewController(this);

                modifiedProductsView.AddSubview(modifiedProductsViewController.View);
                modifiedProductsViewController.View.TranslatesAutoresizingMaskIntoConstraints = false;

                var margins = modifiedProductsView.LayoutMarginsGuide;
                modifiedProductsViewController.View.LeadingAnchor.ConstraintEqualTo(margins.LeadingAnchor).Active = true;
                modifiedProductsViewController.View.TrailingAnchor.ConstraintEqualTo(margins.TrailingAnchor).Active = true;
                modifiedProductsViewController.View.TopAnchor.ConstraintEqualTo(margins.TopAnchor).Active = true;
                modifiedProductsViewController.View.BottomAnchor.ConstraintEqualTo(margins.BottomAnchor).Active = true;

                modifiedProductsViewController.View.LayoutIfNeeded();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentNewsViewController, ConstantMethodName.AddModifiedProductsController);
                //ShowMessageException(exception);
            }
        }

        #endregion

        private void RetryButtonTouchUpInside(object sender, EventArgs e)
        {
            this.TabBarController.SelectedIndex = 0;
            this.NavigationController.PopToRootViewController(true);
        }
    }
}