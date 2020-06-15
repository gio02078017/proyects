// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    [Register ("CheckoutCreditCardViewController")]
    partial class CheckoutCreditCardViewController
    {
        [Outlet]
        UIKit.UIButton addCardButton { get; set; }


        [Outlet]
        UIKit.UITableView creditCardsTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint creditCardsTableViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UITextField installmentTextField { get; set; }


        [Outlet]
        UIKit.UILabel installmentTitleLabel { get; set; }


        [Outlet]
        UIKit.UIView installmentView { get; set; }


        [Outlet]
        UIKit.UIView lineView { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addCardButton != null) {
                addCardButton.Dispose ();
                addCardButton = null;
            }

            if (creditCardsTableView != null) {
                creditCardsTableView.Dispose ();
                creditCardsTableView = null;
            }

            if (creditCardsTableViewHeightConstraint != null) {
                creditCardsTableViewHeightConstraint.Dispose ();
                creditCardsTableViewHeightConstraint = null;
            }

            if (installmentTextField != null) {
                installmentTextField.Dispose ();
                installmentTextField = null;
            }

            if (installmentTitleLabel != null) {
                installmentTitleLabel.Dispose ();
                installmentTitleLabel = null;
            }

            if (installmentView != null) {
                installmentView.Dispose ();
                installmentView = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}