// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.CategoryControllers
{
    [Register ("CategoryViewController")]
    partial class CategoryViewController
    {
        [Outlet]
        UIKit.NSLayoutConstraint categoryCollectionHeightConstraint { get; set; }

        [Outlet]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView categoryCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView searchProductView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (categoryCollectionView != null) {
                categoryCollectionView.Dispose ();
                categoryCollectionView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
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