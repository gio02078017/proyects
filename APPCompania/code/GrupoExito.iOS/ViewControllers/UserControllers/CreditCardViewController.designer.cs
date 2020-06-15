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
    [Register ("CreditCardViewController")]
    partial class CreditCardViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView addEditCreditCardWebView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerAcitivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addEditCreditCardWebView != null) {
                addEditCreditCardWebView.Dispose ();
                addEditCreditCardWebView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (spinnerAcitivityIndicatorView != null) {
                spinnerAcitivityIndicatorView.Dispose ();
                spinnerAcitivityIndicatorView = null;
            }
        }
    }
}