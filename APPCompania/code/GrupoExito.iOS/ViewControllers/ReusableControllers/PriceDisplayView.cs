using System;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Models;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class PriceDisplayView : UIView
    {
        #region Attributes
        private bool IsDisplayed;
        private Action<bool> priceCellDisplayed;
        private Action bagTaxAction;
        #endregion

        #region Properties
        public Action<bool> PriceCellDisplayed { get => priceCellDisplayed; set => priceCellDisplayed = value; }
        public Action BagTaxAction { get => bagTaxAction; set => bagTaxAction = value; }
        #endregion

        #region Constructors
        public PriceDisplayView(IntPtr handle) : base(handle)
        {
            //Default Constructor this class 
        }
        #endregion

        #region Overrides Method
        public override void AwakeFromNib()
        {
            this.Initialize();
            //this.LoadFonts();
        }
        #endregion

        #region Methods
        public void Configure(bool display)
        {
            IsDisplayed = display;
            Update();
        }

        public void UpdateContent(TotalPriceModel totalPriceModel)
        {
            totalValueLabel.Text = StringFormat.ToPrice(totalPriceModel.TotalPrice);
            subTotalValueLabel.Text = StringFormat.ToPrice(totalPriceModel.SubtotalPrice);
            averageShippingPriceValueLabel.Text = StringFormat.ToPrice(totalPriceModel.ShipCost);
            bagTaxValueLabel.Text = StringFormat.ToPrice(totalPriceModel.BagTax);
            discountAppliedValueLabel.Text = StringFormat.ToPrice(totalPriceModel.Discounts);
        }

        private void Initialize()
        {
            totalPriceView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;
            subTotalPriceView.BackgroundColor = ConstantColor.UiBackgroundSummarySelector;

            Layer.CornerRadius = ConstantStyle.CornerRadius;
            ClipsToBounds = true;

            displayArrowImageView.Image = UIImage.FromFile(ConstantImages.FlechaArribaPrimaria);

            bagTaxInfoButton.TouchUpInside += (o, e) =>
            {
                BagTaxAction?.Invoke();
            };

            displayButton.TouchUpInside += (o, e) =>
            {
                IsDisplayed = !IsDisplayed;
                Update();
            };
        }

        private void Update()
        {
            if (IsDisplayed)
            {
                subTotalPriceView.Hidden = false;
                stackViewHeightConstraint.Constant = ConstantViewSize.TotalDisplayViewHeight;
                displayArrowImageView.Image = UIImage.FromBundle(ConstantImages.FlechaArribaPrimaria);
            }
            else
            {
                subTotalPriceView.Hidden = true;
                stackViewHeightConstraint.Constant = ConstantViewSize.TotalDisplayHiddenViewHeight;
                displayArrowImageView.Image = UIImage.FromBundle(ConstantImages.FlechaAbajoPrimaria);
            }
            PriceCellDisplayed?.Invoke(IsDisplayed);
        }

        private void LoadFonts()
        {
            totalTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);
            totalValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size);

            subTotalTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextBodySize);
            subTotalValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextBodySize);

            averageShippingPriceTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);
            averageShippingPriceValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);

            bagTaxtTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);
            bagTaxValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);

            discountAppliedTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);
            discountAppliedValueLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextBodySize);
        }
        #endregion
    }
}