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
    [Register ("MyOrdersViewController")]
    partial class MyOrdersViewController
    {
        [Outlet]
        UIKit.UILabel currentOrdersQuantityLabel { get; set; }


        [Outlet]
        UIKit.UITableView currentOrdersTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint currentOrdersTableViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel currentOrdersTitleLabel { get; set; }


        [Outlet]
        UIKit.UIView customSpinnerView { get; set; }


        [Outlet]
        UIKit.UITableView historicalOrdersTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint historicalOrdersTableViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel historicalOrdersTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel historicalOrdersValueLabel { get; set; }


        [Outlet]
        UIKit.UIView historicalTitleView { get; set; }


        [Outlet]
        UIKit.UIView lineView { get; set; }


        [Outlet]
        UIKit.UILabel myOrdersSubtitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel myOrdersTitleLabel { get; set; }


        [Outlet]
        UIKit.UIScrollView scrollView { get; set; }


        [Outlet]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (currentOrdersQuantityLabel != null) {
                currentOrdersQuantityLabel.Dispose ();
                currentOrdersQuantityLabel = null;
            }

            if (currentOrdersTableView != null) {
                currentOrdersTableView.Dispose ();
                currentOrdersTableView = null;
            }

            if (currentOrdersTableViewHeightConstraint != null) {
                currentOrdersTableViewHeightConstraint.Dispose ();
                currentOrdersTableViewHeightConstraint = null;
            }

            if (currentOrdersTitleLabel != null) {
                currentOrdersTitleLabel.Dispose ();
                currentOrdersTitleLabel = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (historicalOrdersTableView != null) {
                historicalOrdersTableView.Dispose ();
                historicalOrdersTableView = null;
            }

            if (historicalOrdersTableViewHeightConstraint != null) {
                historicalOrdersTableViewHeightConstraint.Dispose ();
                historicalOrdersTableViewHeightConstraint = null;
            }

            if (historicalOrdersTitleLabel != null) {
                historicalOrdersTitleLabel.Dispose ();
                historicalOrdersTitleLabel = null;
            }

            if (historicalOrdersValueLabel != null) {
                historicalOrdersValueLabel.Dispose ();
                historicalOrdersValueLabel = null;
            }

            if (historicalTitleView != null) {
                historicalTitleView.Dispose ();
                historicalTitleView = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (myOrdersSubtitleLabel != null) {
                myOrdersSubtitleLabel.Dispose ();
                myOrdersSubtitleLabel = null;
            }

            if (myOrdersTitleLabel != null) {
                myOrdersTitleLabel.Dispose ();
                myOrdersTitleLabel = null;
            }

            if (scrollView != null) {
                scrollView.Dispose ();
                scrollView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}