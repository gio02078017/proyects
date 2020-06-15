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

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    [Register ("NameListViewCell")]
    partial class NameListViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameListLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (nameListLabel != null) {
                nameListLabel.Dispose ();
                nameListLabel = null;
            }
        }
    }
}