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
    [Register ("SearchProductView")]
    partial class SearchProductView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton openSearchButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productSearchImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField productSearchTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton recipeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView scanCodeImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint searchProductWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton searchTextButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (openSearchButton != null) {
                openSearchButton.Dispose ();
                openSearchButton = null;
            }

            if (productSearchImageView != null) {
                productSearchImageView.Dispose ();
                productSearchImageView = null;
            }

            if (productSearchTextField != null) {
                productSearchTextField.Dispose ();
                productSearchTextField = null;
            }

            if (recipeButton != null) {
                recipeButton.Dispose ();
                recipeButton = null;
            }

            if (scanCodeImageView != null) {
                scanCodeImageView.Dispose ();
                scanCodeImageView = null;
            }

            if (searchProductWidthConstraint != null) {
                searchProductWidthConstraint.Dispose ();
                searchProductWidthConstraint = null;
            }

            if (searchTextButton != null) {
                searchTextButton.Dispose ();
                searchTextButton = null;
            }
        }
    }
}