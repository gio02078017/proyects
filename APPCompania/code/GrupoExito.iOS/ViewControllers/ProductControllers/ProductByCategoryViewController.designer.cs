// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    [Register ("ProductByCategoryViewController")]
    partial class ProductByCategoryViewController
    {
        [Outlet]
        UIKit.UIActivityIndicatorView productActivityIndicatorView { get; set; }


        [Outlet]
        UIKit.UIButton productFilterHeaderButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView categoryIconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel categoryNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        AnimatedButtons.LiquidFloatingActionButton FilterLiquidFloatingView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView navigationView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView productByCategoryCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productCountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView productFilterHeaderView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView searchProductView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }


        [Action ("showFilterOrder_UpInside:")]
        partial void showFilterOrder_UpInside (UIKit.UIButton sender);


        [Action ("showOrder_UpInside:")]
        partial void showOrder_UpInside (UIKit.UIButton sender);

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

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (FilterLiquidFloatingView != null) {
                FilterLiquidFloatingView.Dispose ();
                FilterLiquidFloatingView = null;
            }

            if (navigationView != null) {
                navigationView.Dispose ();
                navigationView = null;
            }

            if (productActivityIndicatorView != null) {
                productActivityIndicatorView.Dispose ();
                productActivityIndicatorView = null;
            }

            if (productByCategoryCollectionView != null) {
                productByCategoryCollectionView.Dispose ();
                productByCategoryCollectionView = null;
            }

            if (productCountLabel != null) {
                productCountLabel.Dispose ();
                productCountLabel = null;
            }

            if (productFilterHeaderButton != null) {
                productFilterHeaderButton.Dispose ();
                productFilterHeaderButton = null;
            }

            if (productFilterHeaderView != null) {
                productFilterHeaderView.Dispose ();
                productFilterHeaderView = null;
            }

            if (searchProductView != null) {
                searchProductView.Dispose ();
                searchProductView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}