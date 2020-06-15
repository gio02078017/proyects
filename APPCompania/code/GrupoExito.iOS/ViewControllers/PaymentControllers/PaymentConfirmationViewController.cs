using System;
using Foundation;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class PaymentConfirmationViewController : UIViewControllerBase
    {
        #region Attributes
        private PaymentResponse _paymentSummary;
        private string deliveryDate = null;
        #endregion

        #region Properties
        public PaymentResponse PaymentSummary { get => _paymentSummary; set => _paymentSummary = value; }
        public string DeliveryDate { get => deliveryDate; set => deliveryDate = value; }
        #endregion

        #region Constructors
        public PaymentConfirmationViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                SetFonts();
                SetSubviews();
                SetOrderInfo();
                SetDeliveryInfo();
                SetSummaryInfo();
                SetPaymentMethod();
                SetPoints();
                SetUnavailableInfo();
                this.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentConfirmationViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            try
            {
                SetHandlers();
                //NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                //NavigationView.LoadControllers(true, false, true, this);
                //NavigationView.HiddenCarData();
                //NavigationView.HiddenAccountProfile();
                ConfigureNavigationBar();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentConfirmationViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            Utilities.Analytic.FirebaseEventRegistrationService.Instance.RegisterScreen(Entities.Constants.Analytic.AnalyticsScreenView.SuccessPayment, nameof(PaymentConfirmationViewController));
        }

        public override void ViewWillDisappear(bool animated)
        {
            try
            {
                RemoveHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PaymentConfirmationViewController, ConstantMethodName.ViewWillDisappear);
                ShowMessageException(exception);
            }
            base.ViewWillDisappear(animated);
        }

        #endregion

        #region Methods
        private void ConfigureNavigationBar()
        {
            this.NavigationController.NavigationBar.Hidden = false;
            NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            NavigationView.HiddenCarData();
            NavigationView.IsSummaryDisabled = true;
            NavigationView.HiddenAccountProfile();
            NavigationView.IsAccountEnabled = false;
            NavigationView.EnableBackButton(false);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
        }

        private void SetFonts()
        {
            titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentTitleFontSize);
            confirmationCodeLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            confirmationCodeValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            deliveryOnLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentSubtitleFontSize);
            deliveryAddressLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            deliveryDateLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);

            purchaseTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentTitleFontSize);

            productsTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            productsNumberLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            imageViewAddButton.Image = UIImage.FromFile(ConstantImages.Lista);
            addButton.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            subtotalLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            subtotalValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            shipmentCostLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            shipmentCostValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            bagTaxLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            bagTaxValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            discountsLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            discountsValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            bagTaxInfoButton.ImageView.Image = UIImage.FromFile(ConstantImages.Informacion);

            totalLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentTitleFontSize);
            totalPriceLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentTitleFontSize);

            paymentMethodTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            paymentMethodLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentBodyFontSize);
            paymentMethodValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            discountInfoLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            discountInfoValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);

            yourPointsTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            accumulatedPointsLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            accumulatedPointsValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            redeemedPointsLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            redeemedPointsValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            totalPointsLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);
            totalPointsValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);

            ifUnavailableTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.PaymentSubtitleFontSize);
            youChoseLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentBodyFontSize);

            keepBuyingButton.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.PaymentSubtitleFontSize);
        }

        private void SetSubviews()
        {
            deliveryAddressLabel.Text = ParametersManager.UserContext.Address != null ?
                ParametersManager.UserContext.Address.AddressComplete :
                ParametersManager.UserContext.Store.Name;
            deliveryOnLabel.Text = ParametersManager.UserContext.Address != null ? deliveryOnLabel.Text : AppMessages.GetStoreText;

            deliveryView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            keepBuyingButton.BackgroundColor = ConstantColor.UiBackgroundKeepBuying;
            keepBuyingButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void SetHandlers()
        {
            keepBuyingButton.TouchUpInside += KeepBuyingHandler;
            bagTaxInfoButton.TouchUpInside += BagTaxHandler;
        }

        private void RemoveHandlers()
        {
            keepBuyingButton.TouchUpInside -= KeepBuyingHandler;
        }

        private void KeepBuyingHandler(Object sender, EventArgs e)
        {
            GoToHome();
        }

        private void BagTaxHandler(Object sender, EventArgs e)
        {
            BagTaxView bagTaxView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.BagTaxView, Self, null).GetItem<BagTaxView>(0);
            bagTaxView.Frame = this.View.Superview.Frame;
            this.View.AddSubview(bagTaxView);
        }

        private void GoToHome()
        {
            this.TabBarController.SelectedIndex = 0;
            this.NavigationController.PopToRootViewController(true);
        }

        private void SetOrderInfo()
        {
            imageView.Image = UIImage.FromFile(ConstantImages.Verificado);
            confirmationCodeValueLabel.Text = PaymentSummary.OrderId;
        }

        private void SetDeliveryInfo()
        {
            deliveryOnLabel.Text = ParametersManager.UserContext.Address != null ? deliveryOnLabel.Text : AppMessages.GetStoreText;
            deliveryAddressLabel.Text = ParametersManager.UserContext.Address != null ?
                      ParametersManager.UserContext.Address.AddressComplete : ParametersManager.UserContext.Store.Name;
            //deliveryDateLabel.Text = ParametersManager.UserContext.Address != null ? deliveryDate : ParametersManager.UserContext.Store.Name;
            deliveryDateLabel.Text = DeliveryDate ?? "";
        }

        private void SetSummaryInfo()
        {
            productsNumberLabel.Text = Convert.ToString(PaymentSummary.ProductsQuantity);
            subtotalValueLabel.Text = StringFormat.ToPrice(PaymentSummary.SubTotal);
            shipmentCostValueLabel.Text = StringFormat.ToPrice(PaymentSummary.Shipping);
            bagTaxValueLabel.Text = StringFormat.ToPrice(PaymentSummary.CountryTax);
            discountsValueLabel.Text = StringFormat.ToPrice(PaymentSummary.Discounts);
            totalPriceLabel.Text = StringFormat.ToPrice(PaymentSummary.Total);
        }

        private void SetPaymentMethod()
        {
            //paymentMethodValueLabel.Text = PaymentSummary.PaymentMethodName;
            paymentMethodLabel.Text = PaymentSummary.PaymentMethodName;
            paymentMethodValueLabel.Text = string.Empty;
            discountInfoValueLabel.Hidden = true;
        }

        private void SetPoints()
        {
            pointsView.RemoveFromSuperview();
        }

        private void SetUnavailableInfo()
        {
            if(!PaymentSummary.ProductsSubstitution)
            {
                youChoseValueLabel.Text = AppMessages.WeWillNotSendItAWellReturnYourMoney;
            }
            else
            {
                youChoseValueLabel.Text = AppMessages.ReplaceUnderTheCriterionOf + AppMessages.ApplicationName;
            }
        }
        #endregion
    }
}