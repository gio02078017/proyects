using System;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Models;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class TotalPriceCollectionViewCell : UICollectionViewCell
    {
        #region Attributes
        public event EventHandler<bool> DisplayStateHandler;
        private bool IsTotalPriceDisplayed = true;
        #endregion

        #region Constructors
        static TotalPriceCollectionViewCell() { }
        protected TotalPriceCollectionViewCell(IntPtr handle) : base(handle) { }
        #endregion

        #region Methods
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }

        public void Configure()
        {
            UpdateArrow();
            displayButton.TouchUpInside += (o, s) =>
            {
                IsTotalPriceDisplayed = !IsTotalPriceDisplayed;
                UpdateArrow();
                DisplayStateHandler?.Invoke(this, IsTotalPriceDisplayed);
            };
        }

        public void UpdateCell(TotalPriceModel totalPriceModel)
        {
            totalPriceLabel.Text = StringFormat.ToPrice(totalPriceModel.TotalPrice);
            subtotalPriceLabel.Text = StringFormat.ToPrice(totalPriceModel.SubtotalPrice);
            shipCostPriceLabel.Text = StringFormat.ToPrice(totalPriceModel.ShipCost);
            bagTaxPriceLabel.Text = StringFormat.ToPrice(totalPriceModel.BagTax);
            discountAppliedPriceLabel.Text = StringFormat.ToPrice(totalPriceModel.Discounts);
        }

        private void UpdateArrow()
        {
            if (IsTotalPriceDisplayed)
            {
                arrowImageView.Image = UIImage.FromBundle(ConstantImages.FlechaArribaPrimaria);
                lineView.Hidden = false;
            }
            else
            {
                arrowImageView.Image = UIImage.FromBundle(ConstantImages.FlechaAbajoPrimaria);
                lineView.Hidden = true;
            }
        }
        #endregion
    }
}
