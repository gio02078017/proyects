using Foundation;
using GrupoExito.Models.ViewModels.Payments;
using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class SummaryPriceInfoCell : GenericCell
    {
        public static readonly NSString Key = new NSString("SummaryPriceInfoCell");
        public static readonly UINib Nib;

        private bool isDisplayed = false;
        public bool IsDisplayed { get { return isDisplayed; } private set { isDisplayed = value; } }

        protected SummaryPriceInfoCell(IntPtr handle) : base(handle) { }

        static SummaryPriceInfoCell()
        {
            Nib = UINib.FromName("SummaryPriceInfoCell", NSBundle.MainBundle);
        }

        public override void AwakeFromNib()
        {
            backgroundView.Layer.CornerRadius = 10;
        }

        public override void Setup(object v)
        {
            if (v is TotalViewModel model)
            {
                UpdateDisplayState(isDisplayed);

                totalPriceLabel.Text = model.Total;
                subtotalLabel.Text = model.Subtotal;
                bagTaxLabel.Text = model.BagTax;
                discountLabel.Text = model.Discounts;
            }
        }

        public void UpdateDisplayState(bool display)
        {
            IsDisplayed = display;
            totalDetailedStackView.Hidden = !IsDisplayed;
        }
    }
}
