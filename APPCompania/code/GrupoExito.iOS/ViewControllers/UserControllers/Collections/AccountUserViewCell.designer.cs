// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Collections
{
    [Register ("AccountUserViewCell")]
    partial class AccountUserViewCell
    {
        [Outlet]
        UIKit.UIStackView containerStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView horizontalView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconProfileImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView iconView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameIconLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerStackView != null) {
                containerStackView.Dispose ();
                containerStackView = null;
            }

            if (horizontalView != null) {
                horizontalView.Dispose ();
                horizontalView = null;
            }

            if (iconProfileImageView != null) {
                iconProfileImageView.Dispose ();
                iconProfileImageView = null;
            }

            if (iconView != null) {
                iconView.Dispose ();
                iconView = null;
            }

            if (nameIconLabel != null) {
                nameIconLabel.Dispose ();
                nameIconLabel = null;
            }
        }
    }
}