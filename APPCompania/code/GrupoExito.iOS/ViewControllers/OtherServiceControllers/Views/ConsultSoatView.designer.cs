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

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers.Views
{
    [Register ("ConsultSoatView")]
    partial class ConsultSoatView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton cancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton consultButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel documentNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField documentNumberTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton documentTypeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel documentTypeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPickerView documentTypePickerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel plateTruckLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField plateTruckTextField { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cancelButton != null) {
                cancelButton.Dispose ();
                cancelButton = null;
            }

            if (consultButton != null) {
                consultButton.Dispose ();
                consultButton = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (documentNumberLabel != null) {
                documentNumberLabel.Dispose ();
                documentNumberLabel = null;
            }

            if (documentNumberTextField != null) {
                documentNumberTextField.Dispose ();
                documentNumberTextField = null;
            }

            if (documentTypeButton != null) {
                documentTypeButton.Dispose ();
                documentTypeButton = null;
            }

            if (documentTypeLabel != null) {
                documentTypeLabel.Dispose ();
                documentTypeLabel = null;
            }

            if (documentTypePickerView != null) {
                documentTypePickerView.Dispose ();
                documentTypePickerView = null;
            }

            if (plateTruckLabel != null) {
                plateTruckLabel.Dispose ();
                plateTruckLabel = null;
            }

            if (plateTruckTextField != null) {
                plateTruckTextField.Dispose ();
                plateTruckTextField = null;
            }
        }
    }
}