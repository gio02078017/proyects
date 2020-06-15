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
    [Register ("OrderDetailTableViewCell")]
    partial class OrderDetailTableViewCell
    {
        [Outlet]
        UIKit.UIImageView checkImageView { get; set; }


        [Outlet]
        UIKit.UILabel howDoYouLikeItTitleLabel { get; set; }


        [Outlet]
        UIKit.UILabel howDoYouLikeItValueLabel { get; set; }


        [Outlet]
        UIKit.UIView howDoYouLikeItView { get; set; }


        [Outlet]
        UIKit.UILabel itemCantLabel { get; set; }


        [Outlet]
        UIKit.UIImageView itemImageView { get; set; }


        [Outlet]
        UIKit.UILabel itemPriceLabel { get; set; }


        [Outlet]
        UIKit.UILabel itemTitleLabel { get; set; }


        [Outlet]
        UIKit.UIView titleView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkImageView != null) {
                checkImageView.Dispose ();
                checkImageView = null;
            }

            if (howDoYouLikeItTitleLabel != null) {
                howDoYouLikeItTitleLabel.Dispose ();
                howDoYouLikeItTitleLabel = null;
            }

            if (howDoYouLikeItValueLabel != null) {
                howDoYouLikeItValueLabel.Dispose ();
                howDoYouLikeItValueLabel = null;
            }

            if (howDoYouLikeItView != null) {
                howDoYouLikeItView.Dispose ();
                howDoYouLikeItView = null;
            }

            if (itemCantLabel != null) {
                itemCantLabel.Dispose ();
                itemCantLabel = null;
            }

            if (itemImageView != null) {
                itemImageView.Dispose ();
                itemImageView = null;
            }

            if (itemPriceLabel != null) {
                itemPriceLabel.Dispose ();
                itemPriceLabel = null;
            }

            if (itemTitleLabel != null) {
                itemTitleLabel.Dispose ();
                itemTitleLabel = null;
            }

            if (titleView != null) {
                titleView.Dispose ();
                titleView = null;
            }
        }
    }
}