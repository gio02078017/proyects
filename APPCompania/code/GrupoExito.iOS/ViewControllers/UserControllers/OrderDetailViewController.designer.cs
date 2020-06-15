// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("OrderDetailViewController")]
    partial class OrderDetailViewController
    {
        [Outlet]
        UIKit.UIButton addToCarButton { get; set; }


        [Outlet]
        UIKit.UILabel addToCarLabel { get; set; }


        [Outlet]
        UIKit.UIView addToCarView { get; set; }


        [Outlet]
        UIKit.UIButton addToListButton { get; set; }


        [Outlet]
        UIKit.UILabel addToListLabel { get; set; }


        [Outlet]
        UIKit.UIImageView carImageView { get; set; }


        [Outlet]
        UIKit.UIImageView checkBoxImageView { get; set; }


        [Outlet]
        UIKit.UIView customSpinnerView { get; set; }


        [Outlet]
        UIKit.UILabel dateTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel dateValueLabel { get; set; }


        [Outlet]
        UIKit.UIImageView listImageView { get; set; }


        [Outlet]
        UIKit.UILabel myOrdersTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel orderIdLabel { get; set; }


        [Outlet]
        UIKit.UITableView orderTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint orderTableViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel productsNumberTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel productsNumberValueLabel { get; set; }


        [Outlet]
        UIKit.UIButton selectAllButton { get; set; }


        [Outlet]
        UIKit.UILabel selectAllTitleLabel { get; set; }


        [Outlet]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView addToListView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView listView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addToCarButton != null) {
                addToCarButton.Dispose ();
                addToCarButton = null;
            }

            if (addToCarLabel != null) {
                addToCarLabel.Dispose ();
                addToCarLabel = null;
            }

            if (addToCarView != null) {
                addToCarView.Dispose ();
                addToCarView = null;
            }

            if (addToListButton != null) {
                addToListButton.Dispose ();
                addToListButton = null;
            }

            if (addToListLabel != null) {
                addToListLabel.Dispose ();
                addToListLabel = null;
            }

            if (addToListView != null) {
                addToListView.Dispose ();
                addToListView = null;
            }

            if (carImageView != null) {
                carImageView.Dispose ();
                carImageView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (listImageView != null) {
                listImageView.Dispose ();
                listImageView = null;
            }

            if (listView != null) {
                listView.Dispose ();
                listView = null;
            }

            if (orderTableView != null) {
                orderTableView.Dispose ();
                orderTableView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}