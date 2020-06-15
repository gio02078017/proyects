// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers
{
    [Register ("MySoatViewController")]
    partial class MySoatViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addSoatButton { get; set; }

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
        UIKit.UITableView soatTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addSoatButton != null) {
                addSoatButton.Dispose ();
                addSoatButton = null;
            }

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

            if (soatTableView != null) {
                soatTableView.Dispose ();
                soatTableView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}