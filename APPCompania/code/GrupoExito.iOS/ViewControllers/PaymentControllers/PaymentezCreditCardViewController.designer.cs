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
    [Register ("PaymentezCreditCardViewController")]
    partial class PaymentezCreditCardViewController
    {
        [Outlet]
        UIKit.UIButton addPaymentezCreditCardButton { get; set; }


        [Outlet]
        UIKit.UIButton addPaymentezMastercardCreditCardButton { get; set; }


        [Outlet]
        UIKit.UILabel addPaymentezMastercardTCLabel { get; set; }


        [Outlet]
        UIKit.UILabel addPaymentezTCLabel { get; set; }


        [Outlet]
        UIKit.UIImageView headerIconImageView { get; set; }


        [Outlet]
        UIKit.UILabel headerLabel { get; set; }


        [Outlet]
        UIKit.UIView headerView { get; set; }


        [Outlet]
        UIKit.UILabel installmentLabel { get; set; }


        [Outlet]
        UIKit.UITextField installmentTextField { get; set; }


        [Outlet]
        UIKit.UIView installmentView { get; set; }


        [Outlet]
        UIKit.UIView lineView { get; set; }


        [Outlet]
        UIKit.UIImageView paymentezImageView { get; set; }


        [Outlet]
        UIKit.UIImageView paymentezMastercardImageView { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint tableViewHeightConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addPaymentezCreditCardButton != null) {
                addPaymentezCreditCardButton.Dispose ();
                addPaymentezCreditCardButton = null;
            }

            if (addPaymentezMastercardCreditCardButton != null) {
                addPaymentezMastercardCreditCardButton.Dispose ();
                addPaymentezMastercardCreditCardButton = null;
            }

            if (addPaymentezMastercardTCLabel != null) {
                addPaymentezMastercardTCLabel.Dispose ();
                addPaymentezMastercardTCLabel = null;
            }

            if (addPaymentezTCLabel != null) {
                addPaymentezTCLabel.Dispose ();
                addPaymentezTCLabel = null;
            }

            if (headerIconImageView != null) {
                headerIconImageView.Dispose ();
                headerIconImageView = null;
            }

            if (headerLabel != null) {
                headerLabel.Dispose ();
                headerLabel = null;
            }

            if (headerView != null) {
                headerView.Dispose ();
                headerView = null;
            }

            if (installmentLabel != null) {
                installmentLabel.Dispose ();
                installmentLabel = null;
            }

            if (installmentTextField != null) {
                installmentTextField.Dispose ();
                installmentTextField = null;
            }

            if (installmentView != null) {
                installmentView.Dispose ();
                installmentView = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (paymentezImageView != null) {
                paymentezImageView.Dispose ();
                paymentezImageView = null;
            }

            if (paymentezMastercardImageView != null) {
                paymentezMastercardImageView.Dispose ();
                paymentezMastercardImageView = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }

            if (tableViewHeightConstraint != null) {
                tableViewHeightConstraint.Dispose ();
                tableViewHeightConstraint = null;
            }
        }
    }
}