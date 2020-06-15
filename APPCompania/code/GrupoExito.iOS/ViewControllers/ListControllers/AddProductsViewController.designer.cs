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

namespace GrupoExito.iOS.Views.ListViews
{
    [Register ("AddProductsViewController")]
    partial class AddProductsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addProductsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView addProductsTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView buttonsStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cleanButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton continueButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel resultsProductsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView resultsStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton searchProductsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField searchProductsTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel weAlsoSuggestLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addProductsLabel != null) {
                addProductsLabel.Dispose ();
                addProductsLabel = null;
            }

            if (addProductsTableView != null) {
                addProductsTableView.Dispose ();
                addProductsTableView = null;
            }

            if (buttonsStackView != null) {
                buttonsStackView.Dispose ();
                buttonsStackView = null;
            }

            if (cleanButton != null) {
                cleanButton.Dispose ();
                cleanButton = null;
            }

            if (continueButton != null) {
                continueButton.Dispose ();
                continueButton = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (resultsProductsLabel != null) {
                resultsProductsLabel.Dispose ();
                resultsProductsLabel = null;
            }

            if (resultsStackView != null) {
                resultsStackView.Dispose ();
                resultsStackView = null;
            }

            if (searchProductsButton != null) {
                searchProductsButton.Dispose ();
                searchProductsButton = null;
            }

            if (searchProductsTextField != null) {
                searchProductsTextField.Dispose ();
                searchProductsTextField = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (weAlsoSuggestLabel != null) {
                weAlsoSuggestLabel.Dispose ();
                weAlsoSuggestLabel = null;
            }
        }
    }
}