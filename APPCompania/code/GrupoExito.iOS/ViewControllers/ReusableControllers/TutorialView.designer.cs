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
    [Register ("TutorialView")]
    partial class TutorialView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton tutorialButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView tutorialImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewTutorial { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (tutorialButton != null) {
                tutorialButton.Dispose ();
                tutorialButton = null;
            }

            if (tutorialImageView != null) {
                tutorialImageView.Dispose ();
                tutorialImageView = null;
            }

            if (viewTutorial != null) {
                viewTutorial.Dispose ();
                viewTutorial = null;
            }
        }
    }
}