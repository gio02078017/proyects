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
    [Register ("ProductDetailViewController")]
    partial class ProductDetailViewController
    {
        [Outlet]
        UIKit.UIView borderBottomPriceView { get; set; }


        [Outlet]
        UIKit.UIView navigationView { get; set; }


        [Outlet]
        UIKit.UILabel productPumLabel { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint viewContainerImageConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AddCountProductButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addListButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addProductToCarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView borderBottomAddToListView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView borderBottomTableContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView discountImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton howLikeLabelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView howLikeTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView noteVariablePriceWeightStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel priceOtherMeansLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView productAddCarView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productContainerTableImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productCountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView productCountView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productDescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productDiscountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productDisountImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productFullViewImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productImageImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint productImageNutritionHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIPageControl ProductImagePageControl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productInfoDetailTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productPreviousPriceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productPriceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView searchProductView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton showFullImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel similarProductsTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SubstractCountProductButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton unitOptionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView unitOptionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton weightOptionButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView weigthOptionView { get; set; }

        [Action ("changeImageProductDetail:")]
        partial void ChangeImageProductDetail (UIKit.UIPageControl sender);


        [Action ("productAddCountButton_UpInside:")]
        partial void productAddCountButton_UpInside (UIKit.UIButton sender);


        [Action ("productAddToCarButton_UpInside:")]
        partial void productAddToCarButton_UpInside (UIKit.UIButton sender);


        [Action ("productSubtractCountButton_UpInside:")]
        partial void productSubtractCountButton_UpInside (UIKit.UIButton sender);


        [Action ("showViewFullImage_UpInside:")]
        partial void showViewFullImage_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (AddCountProductButton != null) {
                AddCountProductButton.Dispose ();
                AddCountProductButton = null;
            }

            if (addListButton != null) {
                addListButton.Dispose ();
                addListButton = null;
            }

            if (addProductToCarButton != null) {
                addProductToCarButton.Dispose ();
                addProductToCarButton = null;
            }

            if (borderBottomAddToListView != null) {
                borderBottomAddToListView.Dispose ();
                borderBottomAddToListView = null;
            }

            if (borderBottomPriceView != null) {
                borderBottomPriceView.Dispose ();
                borderBottomPriceView = null;
            }

            if (borderBottomTableContainerView != null) {
                borderBottomTableContainerView.Dispose ();
                borderBottomTableContainerView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (discountImageView != null) {
                discountImageView.Dispose ();
                discountImageView = null;
            }

            if (howLikeLabelButton != null) {
                howLikeLabelButton.Dispose ();
                howLikeLabelButton = null;
            }

            if (howLikeTextView != null) {
                howLikeTextView.Dispose ();
                howLikeTextView = null;
            }

            if (navigationView != null) {
                navigationView.Dispose ();
                navigationView = null;
            }

            if (noteVariablePriceWeightStackView != null) {
                noteVariablePriceWeightStackView.Dispose ();
                noteVariablePriceWeightStackView = null;
            }

            if (priceOtherMeansLabel != null) {
                priceOtherMeansLabel.Dispose ();
                priceOtherMeansLabel = null;
            }

            if (productAddCarView != null) {
                productAddCarView.Dispose ();
                productAddCarView = null;
            }

            if (productContainerTableImageView != null) {
                productContainerTableImageView.Dispose ();
                productContainerTableImageView = null;
            }

            if (productCountLabel != null) {
                productCountLabel.Dispose ();
                productCountLabel = null;
            }

            if (productCountView != null) {
                productCountView.Dispose ();
                productCountView = null;
            }

            if (productDescriptionLabel != null) {
                productDescriptionLabel.Dispose ();
                productDescriptionLabel = null;
            }

            if (productDiscountLabel != null) {
                productDiscountLabel.Dispose ();
                productDiscountLabel = null;
            }

            if (productDisountImageView != null) {
                productDisountImageView.Dispose ();
                productDisountImageView = null;
            }

            if (productFullViewImageView != null) {
                productFullViewImageView.Dispose ();
                productFullViewImageView = null;
            }

            if (productImageImageView != null) {
                productImageImageView.Dispose ();
                productImageImageView = null;
            }

            if (productImageNutritionHeightConstraint != null) {
                productImageNutritionHeightConstraint.Dispose ();
                productImageNutritionHeightConstraint = null;
            }

            if (ProductImagePageControl != null) {
                ProductImagePageControl.Dispose ();
                ProductImagePageControl = null;
            }

            if (productInfoDetailTextView != null) {
                productInfoDetailTextView.Dispose ();
                productInfoDetailTextView = null;
            }

            if (productPreviousPriceLabel != null) {
                productPreviousPriceLabel.Dispose ();
                productPreviousPriceLabel = null;
            }

            if (productPriceLabel != null) {
                productPriceLabel.Dispose ();
                productPriceLabel = null;
            }

            if (productPumLabel != null) {
                productPumLabel.Dispose ();
                productPumLabel = null;
            }

            if (searchProductView != null) {
                searchProductView.Dispose ();
                searchProductView = null;
            }

            if (showFullImageButton != null) {
                showFullImageButton.Dispose ();
                showFullImageButton = null;
            }

            if (similarProductsTitleLabel != null) {
                similarProductsTitleLabel.Dispose ();
                similarProductsTitleLabel = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (SubstractCountProductButton != null) {
                SubstractCountProductButton.Dispose ();
                SubstractCountProductButton = null;
            }

            if (unitOptionButton != null) {
                unitOptionButton.Dispose ();
                unitOptionButton = null;
            }

            if (unitOptionView != null) {
                unitOptionView.Dispose ();
                unitOptionView = null;
            }

            if (weightOptionButton != null) {
                weightOptionButton.Dispose ();
                weightOptionButton = null;
            }

            if (weigthOptionView != null) {
                weigthOptionView.Dispose ();
                weigthOptionView = null;
            }
        }
    }
}