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
    [Register ("MyAddressViewController")]
    partial class MyAddressViewController
    {
        [Outlet]
        UIKit.UIView detailInfoView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addAddressButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView addressDefaultView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView infoAddressView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageMyAddressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint myAddressHeightTableViewConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView myAddressTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel otherAddressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerAcitivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleMyAddressLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addAddressButton != null) {
                addAddressButton.Dispose ();
                addAddressButton = null;
            }

            if (addressDefaultView != null) {
                addressDefaultView.Dispose ();
                addressDefaultView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (detailInfoView != null) {
                detailInfoView.Dispose ();
                detailInfoView = null;
            }

            if (infoAddressView != null) {
                infoAddressView.Dispose ();
                infoAddressView = null;
            }

            if (messageMyAddressLabel != null) {
                messageMyAddressLabel.Dispose ();
                messageMyAddressLabel = null;
            }

            if (myAddressHeightTableViewConstraint != null) {
                myAddressHeightTableViewConstraint.Dispose ();
                myAddressHeightTableViewConstraint = null;
            }

            if (myAddressTableView != null) {
                myAddressTableView.Dispose ();
                myAddressTableView = null;
            }

            if (otherAddressLabel != null) {
                otherAddressLabel.Dispose ();
                otherAddressLabel = null;
            }

            if (spinnerAcitivityIndicatorView != null) {
                spinnerAcitivityIndicatorView.Dispose ();
                spinnerAcitivityIndicatorView = null;
            }

            if (titleMyAddressLabel != null) {
                titleMyAddressLabel.Dispose ();
                titleMyAddressLabel = null;
            }
        }
    }
}