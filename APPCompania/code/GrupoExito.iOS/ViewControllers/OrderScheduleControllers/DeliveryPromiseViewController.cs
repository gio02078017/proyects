using System;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers
{
    public partial class DeliveryPromiseViewController : UIViewControllerBase
    {
        #region Attributes
        private PriceDisplayView priceDisplayView;
        private string pricePromise;
        private string day;
        private string hoursRange;
        private SubtotalViewModel subtotalViewModel;
        #endregion

        #region Properties
        public string PricePromise { get => pricePromise; set => pricePromise = value; }
        public string Day { get => day; set => day = value; }
        public string HoursRange { get => hoursRange; set => hoursRange = value; }
        public string Subtotal { get; set; }
        public string BagTax { get; set; }
        #endregion

        #region Constructors
        public DeliveryPromiseViewController (IntPtr handle) : base (handle)
        {
            //Default Constructors this class
        }
        #endregion

        #region Life Cycle
        public override void ViewDidLoad()
        {
            try
            {
                deliveryPriceValueLabel.Text = StringFormat.ToPrice(Convert.ToDecimal(PricePromise));

                subtotalViewModel = new SubtotalViewModel("0", "0");
                subtotalViewModel.BagTaxInfoHandler += BagTaxButton_TouchUpInside;

                SubtotalView subtotalView = SubtotalView.Create();
                subtotalView.Frame = subtotalParent.Bounds;
                subtotalView.Setup(subtotalViewModel);
                subtotalParent.AddSubview(subtotalView);
                subtotalView.LayoutIfNeeded();
                subtotalViewModel.UpdateContentCommand.Execute(null);
                continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius;

                deliveryTitleLabel.Text = string.Format(AppMessages.ContingencyScheduleMessage, Day, HoursRange);
                this.LoadExternalViews();
                this.LoadHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DeliveryPromiseViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
            base.ViewDidLoad();
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            try
            {
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                deliveryImageView.Image = UIImage.FromFile(ConstantImages.CarroEntregaPrimario);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DeliveryPromiseViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            deliveryPriceTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextBodySize);
            deliveryPriceValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);

            deliveryTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextTitleSize);
            deliverySubtitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);
            continueButton.TitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);
        }

        private void LoadHandlers()
        {
            continueButton.TouchUpInside += (o, e) =>
            {
                bool responseNetWorking = DeviceManager.Instance.IsNetworkAvailable().Result;
                if (!responseNetWorking)
                {
                    StartActivityIndicatorCustom();
                    StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
                }
                else
                {
                    PaymentContainerController paymentContainerController = (PaymentContainerController)this.Storyboard.InstantiateViewController(nameof(PaymentContainerController));
                    this.NavigationController.PushViewController(paymentContainerController, true);
                }
            };
        }
        #endregion

        void BagTaxButton_TouchUpInside(object sender, EventArgs e)
        {
            PopUpInformationView bagTaxView = PopUpInformationView.Create(AppMessages.BagTax, AppMessages.BagTaxDisclaimer);
            this.NavigationController.SetNavigationBarHidden(true, true);
            bagTaxView.Frame = View.Bounds;
            bagTaxView.LayoutIfNeeded();
            View.AddSubview(bagTaxView);

            bagTaxView.AcceptButtonHandler += () =>
            {
                bagTaxView.RemoveFromSuperview();
            };
            bagTaxView.CloseButtonHandler += () =>
            {
                bagTaxView.RemoveFromSuperview();
            };
        }
    }
}