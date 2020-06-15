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

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers
{
    [Register ("OtherServiceViewController")]
    partial class OtherServiceViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton beforeRowButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton closeGenericPickerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerGenericPickerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView genericPickerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton nextRowButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView servicesTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (beforeRowButton != null) {
                beforeRowButton.Dispose ();
                beforeRowButton = null;
            }

            if (closeGenericPickerButton != null) {
                closeGenericPickerButton.Dispose ();
                closeGenericPickerButton = null;
            }

            if (containerGenericPickerView != null) {
                containerGenericPickerView.Dispose ();
                containerGenericPickerView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (genericPickerView != null) {
                genericPickerView.Dispose ();
                genericPickerView = null;
            }

            if (nextRowButton != null) {
                nextRowButton.Dispose ();
                nextRowButton = null;
            }

            if (servicesTableView != null) {
                servicesTableView.Dispose ();
                servicesTableView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}