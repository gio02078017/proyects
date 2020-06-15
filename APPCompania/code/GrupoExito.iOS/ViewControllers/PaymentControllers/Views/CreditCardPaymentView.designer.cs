// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    [Register ("CreditCardPaymentView")]
    partial class CreditCardPaymentView
    {
        [Outlet]
        UIKit.UIButton exitoCardButton { get; set; }


        [Outlet]
        UIKit.UILabel exitoCardLabel { get; set; }


        [Outlet]
        UIKit.UIView exitoCardView { get; set; }


        [Outlet]
        UIKit.UIStackView installmentStackView { get; set; }


        [Outlet]
        UIKit.UITextField installmentTextField { get; set; }


        [Outlet]
        UIKit.UILabel installmentTitleLabel { get; set; }


        [Outlet]
        UIKit.UIView installmentView { get; set; }


        [Outlet]
        UIKit.UIButton mastercardButton { get; set; }


        [Outlet]
        UIKit.UIImageView mastercardImageView { get; set; }


        [Outlet]
        UIKit.UILabel mastercardLabel { get; set; }


        [Outlet]
        UIKit.UIView mastercardView { get; set; }


        [Outlet]
        UIKit.UITableView tableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (exitoCardButton != null) {
                exitoCardButton.Dispose ();
                exitoCardButton = null;
            }

            if (exitoCardLabel != null) {
                exitoCardLabel.Dispose ();
                exitoCardLabel = null;
            }

            if (exitoCardView != null) {
                exitoCardView.Dispose ();
                exitoCardView = null;
            }

            if (installmentStackView != null) {
                installmentStackView.Dispose ();
                installmentStackView = null;
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

            if (mastercardButton != null) {
                mastercardButton.Dispose ();
                mastercardButton = null;
            }

            if (mastercardLabel != null) {
                mastercardLabel.Dispose ();
                mastercardLabel = null;
            }

            if (mastercardView != null) {
                mastercardView.Dispose ();
                mastercardView = null;
            }

            if (tableView != null) {
                tableView.Dispose ();
                tableView = null;
            }
        }
    }
}