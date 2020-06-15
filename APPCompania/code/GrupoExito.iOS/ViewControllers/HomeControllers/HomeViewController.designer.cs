// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    [Register ("HomeViewController")]
    partial class HomeViewController
    {
        [Outlet]
        UIKit.UIImageView productNotFoundImageView { get; set; }

        [Outlet]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addressStoreSaveLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addressStoreTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView bannerCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView bannerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton changeAddressStoreButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView changeAddressStoreDisplayImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView changeAddressStoreImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView changeAddressStoreStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView changeAddressStoreView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView customerProductsImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel customerProductsLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView discountProductsCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView favoriteProductsCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint favoriteProductsHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel infoTextUserNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton moveBeforeProductButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView moveBeforeProductImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton moveNextProductButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView moveNextProductmageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MyCouponsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView myDiscountsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint productDiscountHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView searchProductView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel userNameLabel { get; set; }

        [Action ("moveProductsDiscount:")]
        partial void MoveProductsDiscount (UIKit.UIButton sender);

        [Action ("seeMyCupons_btnUpInside:")]
        partial void seeMyCupons_btnUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (addressStoreSaveLabel != null) {
                addressStoreSaveLabel.Dispose ();
                addressStoreSaveLabel = null;
            }

            if (addressStoreTitleLabel != null) {
                addressStoreTitleLabel.Dispose ();
                addressStoreTitleLabel = null;
            }

            if (bannerCollectionView != null) {
                bannerCollectionView.Dispose ();
                bannerCollectionView = null;
            }

            if (bannerView != null) {
                bannerView.Dispose ();
                bannerView = null;
            }

            if (changeAddressStoreButton != null) {
                changeAddressStoreButton.Dispose ();
                changeAddressStoreButton = null;
            }

            if (changeAddressStoreDisplayImageView != null) {
                changeAddressStoreDisplayImageView.Dispose ();
                changeAddressStoreDisplayImageView = null;
            }

            if (changeAddressStoreImageView != null) {
                changeAddressStoreImageView.Dispose ();
                changeAddressStoreImageView = null;
            }

            if (changeAddressStoreStackView != null) {
                changeAddressStoreStackView.Dispose ();
                changeAddressStoreStackView = null;
            }

            if (changeAddressStoreView != null) {
                changeAddressStoreView.Dispose ();
                changeAddressStoreView = null;
            }

            if (customerProductsImageView != null) {
                customerProductsImageView.Dispose ();
                customerProductsImageView = null;
            }

            if (customerProductsLabel != null) {
                customerProductsLabel.Dispose ();
                customerProductsLabel = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (discountProductsCollectionView != null) {
                discountProductsCollectionView.Dispose ();
                discountProductsCollectionView = null;
            }

            if (favoriteProductsCollectionView != null) {
                favoriteProductsCollectionView.Dispose ();
                favoriteProductsCollectionView = null;
            }

            if (favoriteProductsHeightConstraint != null) {
                favoriteProductsHeightConstraint.Dispose ();
                favoriteProductsHeightConstraint = null;
            }

            if (infoTextUserNameLabel != null) {
                infoTextUserNameLabel.Dispose ();
                infoTextUserNameLabel = null;
            }

            if (moveBeforeProductButton != null) {
                moveBeforeProductButton.Dispose ();
                moveBeforeProductButton = null;
            }

            if (moveBeforeProductImageView != null) {
                moveBeforeProductImageView.Dispose ();
                moveBeforeProductImageView = null;
            }

            if (moveNextProductButton != null) {
                moveNextProductButton.Dispose ();
                moveNextProductButton = null;
            }

            if (moveNextProductmageView != null) {
                moveNextProductmageView.Dispose ();
                moveNextProductmageView = null;
            }

            if (MyCouponsButton != null) {
                MyCouponsButton.Dispose ();
                MyCouponsButton = null;
            }

            if (myDiscountsView != null) {
                myDiscountsView.Dispose ();
                myDiscountsView = null;
            }

            if (productDiscountHeightConstraint != null) {
                productDiscountHeightConstraint.Dispose ();
                productDiscountHeightConstraint = null;
            }

            if (productNotFoundImageView != null) {
                productNotFoundImageView.Dispose ();
                productNotFoundImageView = null;
            }

            if (searchProductView != null) {
                searchProductView.Dispose ();
                searchProductView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (userNameLabel != null) {
                userNameLabel.Dispose ();
                userNameLabel = null;
            }
        }
    }
}