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

namespace GrupoExito.iOS
{
    [Register ("AccessInitialViewCell")]
    partial class AccessInitialViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView menuIconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel menuNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (menuIconImageView != null) {
                menuIconImageView.Dispose ();
                menuIconImageView = null;
            }

            if (menuNameLabel != null) {
                menuNameLabel.Dispose ();
                menuNameLabel = null;
            }
        }
    }
}