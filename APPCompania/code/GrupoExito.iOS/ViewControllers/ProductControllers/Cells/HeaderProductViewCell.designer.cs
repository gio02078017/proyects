// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    [Register ("HeaderProductViewCell")]
    partial class HeaderProductViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView categoryIconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel categoryNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton filterButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView filterByView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView filterIconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel filterTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton orderByButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView orderByIconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderByTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView orderByView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (categoryIconImageView != null) {
                categoryIconImageView.Dispose ();
                categoryIconImageView = null;
            }

            if (categoryNameLabel != null) {
                categoryNameLabel.Dispose ();
                categoryNameLabel = null;
            }

            if (filterButton != null) {
                filterButton.Dispose ();
                filterButton = null;
            }

            if (filterByView != null) {
                filterByView.Dispose ();
                filterByView = null;
            }

            if (filterIconImageView != null) {
                filterIconImageView.Dispose ();
                filterIconImageView = null;
            }

            if (filterTitleLabel != null) {
                filterTitleLabel.Dispose ();
                filterTitleLabel = null;
            }

            if (orderByButton != null) {
                orderByButton.Dispose ();
                orderByButton = null;
            }

            if (orderByIconImageView != null) {
                orderByIconImageView.Dispose ();
                orderByIconImageView = null;
            }

            if (orderByTitleLabel != null) {
                orderByTitleLabel.Dispose ();
                orderByTitleLabel = null;
            }

            if (orderByView != null) {
                orderByView.Dispose ();
                orderByView = null;
            }
        }
    }
}