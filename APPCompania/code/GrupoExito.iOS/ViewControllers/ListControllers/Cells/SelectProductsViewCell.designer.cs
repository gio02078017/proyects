// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    [Register ("SelectProductsViewCell")]
    partial class SelectProductsViewCell
    {
        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.UIView leftAccesoryView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton checkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView checkImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PriceProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel sizeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (checkButton != null) {
                checkButton.Dispose ();
                checkButton = null;
            }

            if (checkImageView != null) {
                checkImageView.Dispose ();
                checkImageView = null;
            }

            if (leftAccesoryView != null) {
                leftAccesoryView.Dispose ();
                leftAccesoryView = null;
            }

            if (nameProductLabel != null) {
                nameProductLabel.Dispose ();
                nameProductLabel = null;
            }

            if (PriceProductLabel != null) {
                PriceProductLabel.Dispose ();
                PriceProductLabel = null;
            }

            if (productImageView != null) {
                productImageView.Dispose ();
                productImageView = null;
            }

            if (sizeLabel != null) {
                sizeLabel.Dispose ();
                sizeLabel = null;
            }
        }
    }
}