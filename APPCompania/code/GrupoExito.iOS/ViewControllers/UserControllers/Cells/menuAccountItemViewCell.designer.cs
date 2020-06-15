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
    [Register ("menuAccountItemViewCell")]
    partial class menuAccountItemViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageViewMenuItem { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelStatusNotifications { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel labelTitle { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (imageViewMenuItem != null) {
                imageViewMenuItem.Dispose ();
                imageViewMenuItem = null;
            }

            if (labelDescription != null) {
                labelDescription.Dispose ();
                labelDescription = null;
            }

            if (labelStatusNotifications != null) {
                labelStatusNotifications.Dispose ();
                labelStatusNotifications = null;
            }

            if (labelTitle != null) {
                labelTitle.Dispose ();
                labelTitle = null;
            }
        }
    }
}