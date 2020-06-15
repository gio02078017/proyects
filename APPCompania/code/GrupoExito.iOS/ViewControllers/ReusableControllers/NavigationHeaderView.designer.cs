// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("NavigationHeaderView")]
    partial class NavigationHeaderView
    {
        [Outlet]
        UIKit.UILabel clientNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineVerticalView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView logoExitoImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint navigationHeaderWidthViewConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint priceCurrentConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel priceCurrentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton profileButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView profileImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton returnImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton returnViewButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint spacingLeftLogoReturnWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView statusCarImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton statusCountCarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton summaryButton { get; set; }


        [Action ("menuAccountButton_UpInside:")]
        partial void menuAccountButton_UpInside (UIKit.UIButton sender);


        [Action ("ReturnButtonUpInside:")]
        partial void ReturnButtonUpInside (UIKit.UIButton sender);


        [Action ("statusCarButton_UpInside:")]
        partial void statusCarButton_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (clientNameLabel != null) {
                clientNameLabel.Dispose ();
                clientNameLabel = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (lineVerticalView != null) {
                lineVerticalView.Dispose ();
                lineVerticalView = null;
            }

            if (logoExitoImageView != null) {
                logoExitoImageView.Dispose ();
                logoExitoImageView = null;
            }

            if (navigationHeaderWidthViewConstraint != null) {
                navigationHeaderWidthViewConstraint.Dispose ();
                navigationHeaderWidthViewConstraint = null;
            }

            if (priceCurrentConstraint != null) {
                priceCurrentConstraint.Dispose ();
                priceCurrentConstraint = null;
            }

            if (priceCurrentLabel != null) {
                priceCurrentLabel.Dispose ();
                priceCurrentLabel = null;
            }

            if (profileButton != null) {
                profileButton.Dispose ();
                profileButton = null;
            }

            if (profileImageView != null) {
                profileImageView.Dispose ();
                profileImageView = null;
            }

            if (returnImageButton != null) {
                returnImageButton.Dispose ();
                returnImageButton = null;
            }

            if (returnViewButton != null) {
                returnViewButton.Dispose ();
                returnViewButton = null;
            }

            if (spacingLeftLogoReturnWidthConstraint != null) {
                spacingLeftLogoReturnWidthConstraint.Dispose ();
                spacingLeftLogoReturnWidthConstraint = null;
            }

            if (statusCarImageView != null) {
                statusCarImageView.Dispose ();
                statusCarImageView = null;
            }

            if (statusCountCarButton != null) {
                statusCountCarButton.Dispose ();
                statusCountCarButton = null;
            }

            if (summaryButton != null) {
                summaryButton.Dispose ();
                summaryButton = null;
            }
        }
    }
}