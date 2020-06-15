// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    [Register ("GenericErrorView")]
    partial class GenericErrorView
    {
        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.UIImageView imageView { get; set; }


        [Outlet]
        UIKit.UILabel messageLabel { get; set; }


        [Outlet]
        UIKit.UIButton tryAgainButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (messageLabel != null) {
                messageLabel.Dispose ();
                messageLabel = null;
            }

            if (tryAgainButton != null) {
                tryAgainButton.Dispose ();
                tryAgainButton = null;
            }
        }
    }
}