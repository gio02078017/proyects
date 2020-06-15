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

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    [Register ("SplashPromotionsView")]
    partial class SplashPromotionsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton doNotShowAgainButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton promotionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView promotionImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPageControl promotionPageControl { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (doNotShowAgainButton != null) {
                doNotShowAgainButton.Dispose ();
                doNotShowAgainButton = null;
            }

            if (promotionButton != null) {
                promotionButton.Dispose ();
                promotionButton = null;
            }

            if (promotionImageView != null) {
                promotionImageView.Dispose ();
                promotionImageView = null;
            }

            if (promotionPageControl != null) {
                promotionPageControl.Dispose ();
                promotionPageControl = null;
            }
        }
    }
}