// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("RecoverPasswordView")]
    partial class RecoverPasswordView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton closeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel emailLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField emailTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageContentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton sendButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (closeButton != null) {
                closeButton.Dispose ();
                closeButton = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (emailLabel != null) {
                emailLabel.Dispose ();
                emailLabel = null;
            }

            if (emailTextField != null) {
                emailTextField.Dispose ();
                emailTextField = null;
            }

            if (messageContentLabel != null) {
                messageContentLabel.Dispose ();
                messageContentLabel = null;
            }

            if (sendButton != null) {
                sendButton.Dispose ();
                sendButton = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}