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
    [Register ("MyDiscountViewController")]
    partial class MyDiscountViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ContingencyImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ContingencyLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Contingencylabeltwo { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView contingencyStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView myDiscountCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContingencyImageView != null) {
                ContingencyImageView.Dispose ();
                ContingencyImageView = null;
            }

            if (ContingencyLabel != null) {
                ContingencyLabel.Dispose ();
                ContingencyLabel = null;
            }

            if (Contingencylabeltwo != null) {
                Contingencylabeltwo.Dispose ();
                Contingencylabeltwo = null;
            }

            if (contingencyStackView != null) {
                contingencyStackView.Dispose ();
                contingencyStackView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (myDiscountCollectionView != null) {
                myDiscountCollectionView.Dispose ();
                myDiscountCollectionView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}