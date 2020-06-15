// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    [Register ("PaymentNewsViewController")]
    partial class PaymentNewsViewController
    {
        [Outlet]
        UIKit.UILabel adviceLabel { get; set; }


        [Outlet]
        UIKit.UIButton backToSummaryButton { get; set; }


        [Outlet]
        UIKit.UIButton continuePaymentButton { get; set; }


        [Outlet]
        UIKit.UILabel enjoyBenefitLabel { get; set; }


        [Outlet]
        UIKit.UIView modifiedProductsView { get; set; }


        [Outlet]
        UIKit.UIImageView primeImageView { get; set; }


        [Outlet]
        UIKit.UIView primeView { get; set; }


        [Outlet]
        UIKit.UILabel remainingCostLabel { get; set; }


        [Outlet]
        UIKit.UILabel totalTitleValueLabel { get; set; }


        [Outlet]
        UIKit.UIView totalView { get; set; }


        [Outlet]
        UIKit.UILabel withDiscountsAppliedLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (adviceLabel != null) {
                adviceLabel.Dispose ();
                adviceLabel = null;
            }

            if (backToSummaryButton != null) {
                backToSummaryButton.Dispose ();
                backToSummaryButton = null;
            }

            if (continuePaymentButton != null) {
                continuePaymentButton.Dispose ();
                continuePaymentButton = null;
            }

            if (enjoyBenefitLabel != null) {
                enjoyBenefitLabel.Dispose ();
                enjoyBenefitLabel = null;
            }

            if (modifiedProductsView != null) {
                modifiedProductsView.Dispose ();
                modifiedProductsView = null;
            }

            if (primeImageView != null) {
                primeImageView.Dispose ();
                primeImageView = null;
            }

            if (primeView != null) {
                primeView.Dispose ();
                primeView = null;
            }

            if (remainingCostLabel != null) {
                remainingCostLabel.Dispose ();
                remainingCostLabel = null;
            }

            if (totalTitleValueLabel != null) {
                totalTitleValueLabel.Dispose ();
                totalTitleValueLabel = null;
            }

            if (totalView != null) {
                totalView.Dispose ();
                totalView = null;
            }

            if (withDiscountsAppliedLabel != null) {
                withDiscountsAppliedLabel.Dispose ();
                withDiscountsAppliedLabel = null;
            }
        }
    }
}