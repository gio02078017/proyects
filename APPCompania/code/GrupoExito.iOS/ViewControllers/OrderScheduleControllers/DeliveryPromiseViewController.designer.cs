// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers
{
    [Register ("DeliveryPromiseViewController")]
    partial class DeliveryPromiseViewController
    {
        [Outlet]
        UIKit.UIView subtotalParent { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton continueButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView deliveryImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deliveryPriceTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deliveryPriceValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deliverySubtitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deliveryTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (continueButton != null) {
                continueButton.Dispose ();
                continueButton = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (deliveryImageView != null) {
                deliveryImageView.Dispose ();
                deliveryImageView = null;
            }

            if (deliveryPriceTitleLabel != null) {
                deliveryPriceTitleLabel.Dispose ();
                deliveryPriceTitleLabel = null;
            }

            if (deliveryPriceValueLabel != null) {
                deliveryPriceValueLabel.Dispose ();
                deliveryPriceValueLabel = null;
            }

            if (deliverySubtitleLabel != null) {
                deliverySubtitleLabel.Dispose ();
                deliverySubtitleLabel = null;
            }

            if (deliveryTitleLabel != null) {
                deliveryTitleLabel.Dispose ();
                deliveryTitleLabel = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (subtotalParent != null) {
                subtotalParent.Dispose ();
                subtotalParent = null;
            }
        }
    }
}