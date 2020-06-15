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
    [Register ("CustomActivityIndicatorView")]
    partial class CustomActivityIndicatorView
    {
        [Outlet]
        UIKit.UIImageView productAddingImageView { get; set; }


        [Outlet]
        UIKit.UIImageView spinnerImageImageView { get; set; }


        [Outlet]
        UIKit.UILabel spinnerMessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView productAddingView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton retryButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (productAddingImageView != null) {
                productAddingImageView.Dispose ();
                productAddingImageView = null;
            }

            if (productAddingView != null) {
                productAddingView.Dispose ();
                productAddingView = null;
            }

            if (retryButton != null) {
                retryButton.Dispose ();
                retryButton = null;
            }

            if (spinnerImageImageView != null) {
                spinnerImageImageView.Dispose ();
                spinnerImageImageView = null;
            }

            if (spinnerMessageLabel != null) {
                spinnerMessageLabel.Dispose ();
                spinnerMessageLabel = null;
            }
        }
    }
}