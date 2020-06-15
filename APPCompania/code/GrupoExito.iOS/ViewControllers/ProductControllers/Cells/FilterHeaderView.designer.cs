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
    [Register ("FilterHeaderView")]
    partial class FilterHeaderView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView backgroundImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton headerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconArrowImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineBottomView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (backgroundImageView != null) {
                backgroundImageView.Dispose ();
                backgroundImageView = null;
            }

            if (headerButton != null) {
                headerButton.Dispose ();
                headerButton = null;
            }

            if (iconArrowImageView != null) {
                iconArrowImageView.Dispose ();
                iconArrowImageView = null;
            }

            if (lineBottomView != null) {
                lineBottomView.Dispose ();
                lineBottomView = null;
            }
        }
    }
}