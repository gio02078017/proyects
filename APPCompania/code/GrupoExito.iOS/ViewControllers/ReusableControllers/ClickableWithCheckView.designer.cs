// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("ClickableWithCheckView")]
    partial class ClickableWithCheckView
    {
        [Outlet]
        UIKit.UIImageView checkImageView { get; set; }


        [Outlet]
        UIKit.UILabel descriptionLabel { get; set; }


        [Outlet]
        UIKit.UILabel priceLabel { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkImageView != null) {
                checkImageView.Dispose ();
                checkImageView = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (priceLabel != null) {
                priceLabel.Dispose ();
                priceLabel = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}