// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.Views.UserViews.Cells
{
    [Register ("StickersViewCell")]
    partial class StickersViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel numberStickerLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView stickersImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (numberStickerLabel != null) {
                numberStickerLabel.Dispose ();
                numberStickerLabel = null;
            }

            if (stickersImageView != null) {
                stickersImageView.Dispose ();
                stickersImageView = null;
            }
        }
    }
}