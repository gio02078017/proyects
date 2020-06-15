// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    [Register ("DetailPriceCheckerViewController")]
    partial class DetailPriceCheckerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel gramProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel priceProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView producImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel quantityProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel referenceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (gramProductLabel != null) {
                gramProductLabel.Dispose ();
                gramProductLabel = null;
            }

            if (priceProductLabel != null) {
                priceProductLabel.Dispose ();
                priceProductLabel = null;
            }

            if (producImageView != null) {
                producImageView.Dispose ();
                producImageView = null;
            }

            if (productNameLabel != null) {
                productNameLabel.Dispose ();
                productNameLabel = null;
            }

            if (quantityProductLabel != null) {
                quantityProductLabel.Dispose ();
                quantityProductLabel = null;
            }

            if (referenceLabel != null) {
                referenceLabel.Dispose ();
                referenceLabel = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}