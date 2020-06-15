// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Views
{
    [Register ("PrimeCommentaryView")]
    partial class PrimeCommentaryView
    {
        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.UIImageView iconImageView { get; set; }


        [Outlet]
        UIKit.UILabel infoLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (contentView != null) {
                contentView.Dispose ();
                contentView = null;
            }

            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (infoLabel != null) {
                infoLabel.Dispose ();
                infoLabel = null;
            }
        }
    }
}