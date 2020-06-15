// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ConfigurationControllers
{
    [Register ("VersioningViewController")]
    partial class VersioningViewController
    {
        [Outlet]
        UIKit.UIView containerView { get; set; }


        [Outlet]
        UIKit.UIButton refuseButton { get; set; }


        [Outlet]
        UIKit.UIButton updateButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (refuseButton != null) {
                refuseButton.Dispose ();
                refuseButton = null;
            }

            if (updateButton != null) {
                updateButton.Dispose ();
                updateButton = null;
            }
        }
    }
}