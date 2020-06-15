// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells
{
    [Register ("MyDiscountCollectionViewCell")]
    partial class MyDiscountCollectionViewCell
    {
        [Outlet]
        UIKit.UIButton legalButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton activeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerAllView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel eventModeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel legalLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel pluDescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel pluNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint proportionalWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel regionLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (activeButton != null) {
                activeButton.Dispose ();
                activeButton = null;
            }

            if (containerAllView != null) {
                containerAllView.Dispose ();
                containerAllView = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (eventModeLabel != null) {
                eventModeLabel.Dispose ();
                eventModeLabel = null;
            }

            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (legalButton != null) {
                legalButton.Dispose ();
                legalButton = null;
            }

            if (legalLabel != null) {
                legalLabel.Dispose ();
                legalLabel = null;
            }

            if (pluDescriptionLabel != null) {
                pluDescriptionLabel.Dispose ();
                pluDescriptionLabel = null;
            }

            if (pluNumber != null) {
                pluNumber.Dispose ();
                pluNumber = null;
            }

            if (proportionalWidthConstraint != null) {
                proportionalWidthConstraint.Dispose ();
                proportionalWidthConstraint = null;
            }

            if (regionLabel != null) {
                regionLabel.Dispose ();
                regionLabel = null;
            }
        }
    }
}