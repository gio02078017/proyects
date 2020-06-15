// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    [Register ("ProductFilterAndOrderByViewController")]
    partial class ProductFilterAndOrderByViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint filterTableViewConstraint { get; set; }


        [Outlet]
        UIKit.UITableView orderByTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ApplyChangeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ClearButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView filterOrderTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView navigationView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView searchProductView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }


        [Action ("applyChange_UpInside:")]
        partial void applyChange_UpInside (UIKit.UIButton sender);


        [Action ("clearFilterAndOrder_UpInside:")]
        partial void clearFilterAndOrder_UpInside (UIKit.UIButton sender);


        [Action ("filterByButton_UpInside:")]
        partial void filterByButton_UpInside (UIKit.UIButton sender);


        [Action ("orderByButton_UpInside:")]
        partial void orderByButton_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ApplyChangeButton != null) {
                ApplyChangeButton.Dispose ();
                ApplyChangeButton = null;
            }

            if (ClearButton != null) {
                ClearButton.Dispose ();
                ClearButton = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (filterOrderTableView != null) {
                filterOrderTableView.Dispose ();
                filterOrderTableView = null;
            }

            if (navigationView != null) {
                navigationView.Dispose ();
                navigationView = null;
            }

            if (searchProductView != null) {
                searchProductView.Dispose ();
                searchProductView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}