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
    [Register ("ProductListViewCell")]
    partial class ProductListViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerCheckedView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconCheckedImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel priceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productInfoLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel pumLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SelectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView selectImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView stackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView unitWeightView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerCheckedView != null) {
                containerCheckedView.Dispose ();
                containerCheckedView = null;
            }

            if (iconCheckedImageView != null) {
                iconCheckedImageView.Dispose ();
                iconCheckedImageView = null;
            }

            if (priceLabel != null) {
                priceLabel.Dispose ();
                priceLabel = null;
            }

            if (productImageView != null) {
                productImageView.Dispose ();
                productImageView = null;
            }

            if (productInfoLabel != null) {
                productInfoLabel.Dispose ();
                productInfoLabel = null;
            }

            if (productNameLabel != null) {
                productNameLabel.Dispose ();
                productNameLabel = null;
            }

            if (pumLabel != null) {
                pumLabel.Dispose ();
                pumLabel = null;
            }

            if (SelectButton != null) {
                SelectButton.Dispose ();
                SelectButton = null;
            }

            if (selectImageView != null) {
                selectImageView.Dispose ();
                selectImageView = null;
            }

            if (stackView != null) {
                stackView.Dispose ();
                stackView = null;
            }

            if (unitWeightView != null) {
                unitWeightView.Dispose ();
                unitWeightView = null;
            }
        }
    }
}