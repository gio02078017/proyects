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

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("MessageConfirmView")]
    partial class MessageConfirmView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton closeViewButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton noButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleWaitLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton yesButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (closeViewButton != null) {
                closeViewButton.Dispose ();
                closeViewButton = null;
            }

            if (messageLabel != null) {
                messageLabel.Dispose ();
                messageLabel = null;
            }

            if (noButton != null) {
                noButton.Dispose ();
                noButton = null;
            }

            if (titleWaitLabel != null) {
                titleWaitLabel.Dispose ();
                titleWaitLabel = null;
            }

            if (yesButton != null) {
                yesButton.Dispose ();
                yesButton = null;
            }
        }
    }
}