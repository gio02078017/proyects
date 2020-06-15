// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.Referentials
{
    [Register ("CustomSpinnerViewController")]
    partial class CustomSpinnerViewController
    {
        [Outlet]
        UIKit.UIButton actionButton { get; set; }


        [Outlet]
        UIKit.UIImageView imageView { get; set; }


        [Outlet]
        UIKit.UILabel informationLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (actionButton != null) {
                actionButton.Dispose ();
                actionButton = null;
            }

            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (informationLabel != null) {
                informationLabel.Dispose ();
                informationLabel = null;
            }
        }
    }
}