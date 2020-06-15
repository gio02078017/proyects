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

namespace GrupoExito.iOS.ViewControllers.ListControllers
{
    [Register ("MyCustomListViewController")]
    partial class MyCustomListViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addListButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addProductsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton createListButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView listTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField nameListTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView navigationHeaderView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView searchProductView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel textLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel youListLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addListButton != null) {
                addListButton.Dispose ();
                addListButton = null;
            }

            if (addProductsLabel != null) {
                addProductsLabel.Dispose ();
                addProductsLabel = null;
            }

            if (createListButton != null) {
                createListButton.Dispose ();
                createListButton = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (listTableView != null) {
                listTableView.Dispose ();
                listTableView = null;
            }

            if (nameListTextField != null) {
                nameListTextField.Dispose ();
                nameListTextField = null;
            }

            if (navigationHeaderView != null) {
                navigationHeaderView.Dispose ();
                navigationHeaderView = null;
            }

            if (searchProductView != null) {
                searchProductView.Dispose ();
                searchProductView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (textLabel != null) {
                textLabel.Dispose ();
                textLabel = null;
            }

            if (youListLabel != null) {
                youListLabel.Dispose ();
                youListLabel = null;
            }
        }
    }
}