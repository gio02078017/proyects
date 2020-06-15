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
    [Register ("HeaderOrderDetailViewCell")]
    partial class HeaderOrderDetailViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView checkImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateOrderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateOrderTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel numberProductsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel numberProductsTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderIdLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderIdTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton selectAllButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel selectAllTitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkImageView != null) {
                checkImageView.Dispose ();
                checkImageView = null;
            }

            if (dateOrderLabel != null) {
                dateOrderLabel.Dispose ();
                dateOrderLabel = null;
            }

            if (dateOrderTitleLabel != null) {
                dateOrderTitleLabel.Dispose ();
                dateOrderTitleLabel = null;
            }

            if (numberProductsLabel != null) {
                numberProductsLabel.Dispose ();
                numberProductsLabel = null;
            }

            if (numberProductsTitleLabel != null) {
                numberProductsTitleLabel.Dispose ();
                numberProductsTitleLabel = null;
            }

            if (orderIdLabel != null) {
                orderIdLabel.Dispose ();
                orderIdLabel = null;
            }

            if (orderIdTitleLabel != null) {
                orderIdTitleLabel.Dispose ();
                orderIdTitleLabel = null;
            }

            if (selectAllButton != null) {
                selectAllButton.Dispose ();
                selectAllButton = null;
            }

            if (selectAllTitleLabel != null) {
                selectAllTitleLabel.Dispose ();
                selectAllTitleLabel = null;
            }
        }
    }
}