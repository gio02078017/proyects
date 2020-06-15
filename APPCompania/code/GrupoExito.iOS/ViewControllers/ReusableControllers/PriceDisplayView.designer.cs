// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("PriceDisplayView")]
    partial class PriceDisplayView
    {
        [Outlet]
        UIKit.UILabel averageShippingPriceTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel averageShippingPriceValueLabel { get; set; }


        [Outlet]
        UIKit.UIButton bagTaxInfoButton { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxtTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxValueLabel { get; set; }


        [Outlet]
        UIKit.UILabel discountAppliedTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel discountAppliedValueLabel { get; set; }


        [Outlet]
        UIKit.UIImageView displayArrowImageView { get; set; }


        [Outlet]
        UIKit.UIButton displayButton { get; set; }


        [Outlet]
        UIKit.UIStackView priceStackView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint stackViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UIView subTotalPriceView { get; set; }


        [Outlet]
        UIKit.UILabel subTotalTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel subTotalValueLabel { get; set; }


        [Outlet]
        UIKit.UIView totalPriceView { get; set; }


        [Outlet]
        UIKit.UILabel totalTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel totalValueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (averageShippingPriceTitleLabel != null) {
                averageShippingPriceTitleLabel.Dispose ();
                averageShippingPriceTitleLabel = null;
            }

            if (averageShippingPriceValueLabel != null) {
                averageShippingPriceValueLabel.Dispose ();
                averageShippingPriceValueLabel = null;
            }

            if (bagTaxInfoButton != null) {
                bagTaxInfoButton.Dispose ();
                bagTaxInfoButton = null;
            }

            if (bagTaxtTitleLabel != null) {
                bagTaxtTitleLabel.Dispose ();
                bagTaxtTitleLabel = null;
            }

            if (bagTaxValueLabel != null) {
                bagTaxValueLabel.Dispose ();
                bagTaxValueLabel = null;
            }

            if (discountAppliedTitleLabel != null) {
                discountAppliedTitleLabel.Dispose ();
                discountAppliedTitleLabel = null;
            }

            if (discountAppliedValueLabel != null) {
                discountAppliedValueLabel.Dispose ();
                discountAppliedValueLabel = null;
            }

            if (displayArrowImageView != null) {
                displayArrowImageView.Dispose ();
                displayArrowImageView = null;
            }

            if (displayButton != null) {
                displayButton.Dispose ();
                displayButton = null;
            }

            if (priceStackView != null) {
                priceStackView.Dispose ();
                priceStackView = null;
            }

            if (stackViewHeightConstraint != null) {
                stackViewHeightConstraint.Dispose ();
                stackViewHeightConstraint = null;
            }

            if (subTotalPriceView != null) {
                subTotalPriceView.Dispose ();
                subTotalPriceView = null;
            }

            if (subTotalTitleLabel != null) {
                subTotalTitleLabel.Dispose ();
                subTotalTitleLabel = null;
            }

            if (subTotalValueLabel != null) {
                subTotalValueLabel.Dispose ();
                subTotalValueLabel = null;
            }

            if (totalPriceView != null) {
                totalPriceView.Dispose ();
                totalPriceView = null;
            }

            if (totalTitleLabel != null) {
                totalTitleLabel.Dispose ();
                totalTitleLabel = null;
            }

            if (totalValueLabel != null) {
                totalValueLabel.Dispose ();
                totalValueLabel = null;
            }
        }
    }
}