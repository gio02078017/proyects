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

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    [Register ("ShiftInBoxView")]
    partial class ShiftInBoxView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel averagelabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel shiftNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView timeImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel timeTurnLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView turnImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel turnsLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (averagelabel != null) {
                averagelabel.Dispose ();
                averagelabel = null;
            }

            if (shiftNumberLabel != null) {
                shiftNumberLabel.Dispose ();
                shiftNumberLabel = null;
            }

            if (timeImageView != null) {
                timeImageView.Dispose ();
                timeImageView = null;
            }

            if (timeTurnLabel != null) {
                timeTurnLabel.Dispose ();
                timeTurnLabel = null;
            }

            if (turnImageView != null) {
                turnImageView.Dispose ();
                turnImageView = null;
            }

            if (turnsLabel != null) {
                turnsLabel.Dispose ();
                turnsLabel = null;
            }
        }
    }
}