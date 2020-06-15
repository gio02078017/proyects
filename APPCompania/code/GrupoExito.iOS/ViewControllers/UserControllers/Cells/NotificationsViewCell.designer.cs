// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    [Register ("NotificationsViewCell")]
    partial class NotificationsViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel notificationsLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (notificationsLabel != null) {
                notificationsLabel.Dispose ();
                notificationsLabel = null;
            }
        }
    }
}