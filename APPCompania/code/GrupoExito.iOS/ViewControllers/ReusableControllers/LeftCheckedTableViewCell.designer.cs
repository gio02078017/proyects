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
    [Register ("LeftCheckedTableViewCell")]
    partial class LeftCheckedTableViewCell
    {
        [Outlet]
        UIKit.UIImageView checkIconImageView { get; set; }


        [Outlet]
        UIKit.UILabel descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIView innerContentView { get; set; }


        [Outlet]
        UIKit.UILabel priceLabel { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkIconImageView != null) {
                checkIconImageView.Dispose ();
                checkIconImageView = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (innerContentView != null) {
                innerContentView.Dispose ();
                innerContentView = null;
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