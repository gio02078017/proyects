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
    [Register ("OrderDetailCollectionViewCell")]
    partial class OrderDetailCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView arrowImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton displayButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel paymentMethodLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel paymentMethodTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel statusOrderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel totalPriceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel totalPriceTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel totalProductsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel totalProductsTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton viewProductsButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (arrowImageView != null) {
                arrowImageView.Dispose ();
                arrowImageView = null;
            }

            if (displayButton != null) {
                displayButton.Dispose ();
                displayButton = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (orderNumberLabel != null) {
                orderNumberLabel.Dispose ();
                orderNumberLabel = null;
            }

            if (orderTitleLabel != null) {
                orderTitleLabel.Dispose ();
                orderTitleLabel = null;
            }

            if (paymentMethodLabel != null) {
                paymentMethodLabel.Dispose ();
                paymentMethodLabel = null;
            }

            if (paymentMethodTitleLabel != null) {
                paymentMethodTitleLabel.Dispose ();
                paymentMethodTitleLabel = null;
            }

            if (statusOrderLabel != null) {
                statusOrderLabel.Dispose ();
                statusOrderLabel = null;
            }

            if (totalPriceLabel != null) {
                totalPriceLabel.Dispose ();
                totalPriceLabel = null;
            }

            if (totalPriceTitleLabel != null) {
                totalPriceTitleLabel.Dispose ();
                totalPriceTitleLabel = null;
            }

            if (totalProductsLabel != null) {
                totalProductsLabel.Dispose ();
                totalProductsLabel = null;
            }

            if (totalProductsTitleLabel != null) {
                totalProductsTitleLabel.Dispose ();
                totalProductsTitleLabel = null;
            }

            if (viewProductsButton != null) {
                viewProductsButton.Dispose ();
                viewProductsButton = null;
            }
        }
    }
}