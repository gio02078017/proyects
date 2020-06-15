// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("RegistryUserSuccessViewController")]
    partial class RegistryUserSuccessViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView navigationView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton startButton { get; set; }

        [Action ("startButton_UpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void startButton_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (navigationView != null) {
                navigationView.Dispose ();
                navigationView = null;
            }

            if (startButton != null) {
                startButton.Dispose ();
                startButton = null;
            }
        }
    }
}