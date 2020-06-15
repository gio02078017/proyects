// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Cells
{
    [Register ("BannerViewCell")]
    partial class BannerViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView bannerImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (bannerImageView != null) {
                bannerImageView.Dispose ();
                bannerImageView = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }
        }
    }
}