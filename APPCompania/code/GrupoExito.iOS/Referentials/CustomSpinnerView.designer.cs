// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.Referentials
{
    [Register ("CustomSpinnerView")]
    partial class CustomSpinnerView
    {
        [Outlet]
        UIKit.UILabel messageLabel { get; set; }


        [Outlet]
        UIKit.UIImageView productImageView { get; set; }


        [Outlet]
        UIKit.UIImageView spinnerImageView { get; set; }


        [Outlet]
        UIKit.UIButton tryAgainButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (messageLabel != null) {
                messageLabel.Dispose ();
                messageLabel = null;
            }

            if (productImageView != null) {
                productImageView.Dispose ();
                productImageView = null;
            }

            if (spinnerImageView != null) {
                spinnerImageView.Dispose ();
                spinnerImageView = null;
            }

            if (tryAgainButton != null) {
                tryAgainButton.Dispose ();
                tryAgainButton = null;
            }
        }
    }
}