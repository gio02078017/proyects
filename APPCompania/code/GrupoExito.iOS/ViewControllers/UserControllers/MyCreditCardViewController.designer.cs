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

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("MyCreditCardViewController")]
    partial class MyCreditCardViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addCreditCardButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addCreditCardExito { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView myCreditCardTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerAcitivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addCreditCardButton != null) {
                addCreditCardButton.Dispose ();
                addCreditCardButton = null;
            }

            if (addCreditCardExito != null) {
                addCreditCardExito.Dispose ();
                addCreditCardExito = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (myCreditCardTableView != null) {
                myCreditCardTableView.Dispose ();
                myCreditCardTableView = null;
            }

            if (spinnerAcitivityIndicatorView != null) {
                spinnerAcitivityIndicatorView.Dispose ();
                spinnerAcitivityIndicatorView = null;
            }
        }
    }
}