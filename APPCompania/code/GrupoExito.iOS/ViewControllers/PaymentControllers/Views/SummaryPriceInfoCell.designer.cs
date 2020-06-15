// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    [Register ("SummaryPriceInfoCell")]
    partial class SummaryPriceInfoCell
    {
        [Outlet]
        UIKit.UIImageView arrowImageView { get; set; }


        [Outlet]
        UIKit.UIView backgroundView { get; set; }


        [Outlet]
        UIKit.UIButton bagTaxButton { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxLabel { get; set; }


        [Outlet]
        UIKit.UILabel discountInformation { get; set; }


        [Outlet]
        UIKit.UILabel discountLabel { get; set; }


        [Outlet]
        UIKit.UILabel subtotalLabel { get; set; }


        [Outlet]
        UIKit.UIButton totalButton { get; set; }


        [Outlet]
        UIKit.UIStackView totalDetailedStackView { get; set; }


        [Outlet]
        UIKit.UILabel totalPriceLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (arrowImageView != null) {
                arrowImageView.Dispose ();
                arrowImageView = null;
            }

            if (backgroundView != null) {
                backgroundView.Dispose ();
                backgroundView = null;
            }

            if (bagTaxButton != null) {
                bagTaxButton.Dispose ();
                bagTaxButton = null;
            }

            if (bagTaxLabel != null) {
                bagTaxLabel.Dispose ();
                bagTaxLabel = null;
            }

            if (discountInformation != null) {
                discountInformation.Dispose ();
                discountInformation = null;
            }

            if (discountLabel != null) {
                discountLabel.Dispose ();
                discountLabel = null;
            }

            if (subtotalLabel != null) {
                subtotalLabel.Dispose ();
                subtotalLabel = null;
            }

            if (totalDetailedStackView != null) {
                totalDetailedStackView.Dispose ();
                totalDetailedStackView = null;
            }

            if (totalPriceLabel != null) {
                totalPriceLabel.Dispose ();
                totalPriceLabel = null;
            }
        }
    }
}