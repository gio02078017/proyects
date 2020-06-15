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
    [Register ("SkipLoginView")]
    partial class SkipLoginView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView contentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton continueButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageOneLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageThreeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageTwoLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton returnLoginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        [Action ("continueButton_UpInside:")]
        partial void continueButton_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (continueButton != null) {
                continueButton.Dispose ();
                continueButton = null;
            }

            if (messageOneLabel != null) {
                messageOneLabel.Dispose ();
                messageOneLabel = null;
            }

            if (messageThreeLabel != null) {
                messageThreeLabel.Dispose ();
                messageThreeLabel = null;
            }

            if (messageTitleLabel != null) {
                messageTitleLabel.Dispose ();
                messageTitleLabel = null;
            }

            if (messageTwoLabel != null) {
                messageTwoLabel.Dispose ();
                messageTwoLabel = null;
            }

            if (returnLoginButton != null) {
                returnLoginButton.Dispose ();
                returnLoginButton = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}