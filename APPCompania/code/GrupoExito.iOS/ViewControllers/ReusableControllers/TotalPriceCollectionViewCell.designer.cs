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
    [Register ("TotalPriceCollectionViewCell")]
    partial class TotalPriceCollectionViewCell
    {
        [Outlet]
        UIKit.UIImageView arrowImageView { get; set; }


        [Outlet]
        UIKit.UIButton bagTaxInfoButton { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxLabel { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxPriceLabel { get; set; }


        [Outlet]
        UIKit.UILabel discountAppliedLabel { get; set; }


        [Outlet]
        UIKit.UILabel discountAppliedPriceLabel { get; set; }


        [Outlet]
        UIKit.UIButton displayButton { get; set; }


        [Outlet]
        UIKit.UIView lineView { get; set; }


        [Outlet]
        UIKit.UILabel shipCostLabel { get; set; }


        [Outlet]
        UIKit.UILabel shipCostPriceLabel { get; set; }


        [Outlet]
        UIKit.UILabel subtotalLabel { get; set; }


        [Outlet]
        UIKit.UILabel subtotalPriceLabel { get; set; }


        [Outlet]
        UIKit.UILabel totalLabel { get; set; }


        [Outlet]
        UIKit.UILabel totalPriceLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (arrowImageView != null) {
                arrowImageView.Dispose ();
                arrowImageView = null;
            }

            if (bagTaxInfoButton != null) {
                bagTaxInfoButton.Dispose ();
                bagTaxInfoButton = null;
            }

            if (bagTaxLabel != null) {
                bagTaxLabel.Dispose ();
                bagTaxLabel = null;
            }

            if (bagTaxPriceLabel != null) {
                bagTaxPriceLabel.Dispose ();
                bagTaxPriceLabel = null;
            }

            if (discountAppliedLabel != null) {
                discountAppliedLabel.Dispose ();
                discountAppliedLabel = null;
            }

            if (discountAppliedPriceLabel != null) {
                discountAppliedPriceLabel.Dispose ();
                discountAppliedPriceLabel = null;
            }

            if (displayButton != null) {
                displayButton.Dispose ();
                displayButton = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (shipCostLabel != null) {
                shipCostLabel.Dispose ();
                shipCostLabel = null;
            }

            if (shipCostPriceLabel != null) {
                shipCostPriceLabel.Dispose ();
                shipCostPriceLabel = null;
            }

            if (subtotalLabel != null) {
                subtotalLabel.Dispose ();
                subtotalLabel = null;
            }

            if (subtotalPriceLabel != null) {
                subtotalPriceLabel.Dispose ();
                subtotalPriceLabel = null;
            }

            if (totalLabel != null) {
                totalLabel.Dispose ();
                totalLabel = null;
            }

            if (totalPriceLabel != null) {
                totalPriceLabel.Dispose ();
                totalPriceLabel = null;
            }
        }
    }
}