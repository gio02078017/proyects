// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    [Register ("ProductModifiedTableViewCell")]
    partial class ProductModifiedTableViewCell
    {
        [Outlet]
        UIKit.UILabel cantLabel { get; set; }


        [Outlet]
        UIKit.UILabel cellTitleLabel { get; set; }


        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.UILabel descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIImageView iconImageView { get; set; }


        [Outlet]
        UIKit.UIImageView imageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cantLabel != null) {
                cantLabel.Dispose ();
                cantLabel = null;
            }

            if (cellTitleLabel != null) {
                cellTitleLabel.Dispose ();
                cellTitleLabel = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }
        }
    }
}