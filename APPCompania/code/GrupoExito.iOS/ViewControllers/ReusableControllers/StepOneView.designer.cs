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
    [Register ("StepOneView")]
    partial class StepOneView
    {
        [Outlet]
        UIKit.UIButton chatWithUsButton { get; set; }


        [Outlet]
        UIKit.UIImageView checkStatusImageView { get; set; }


        [Outlet]
        UIKit.UIView checkStatusView { get; set; }


        [Outlet]
        UIKit.UIView containerDescriptionStatusView { get; set; }


        [Outlet]
        UIKit.UIView contentView { get; set; }


        [Outlet]
        UIKit.UILabel messageDescriptionStatusLabel { get; set; }


        [Outlet]
        UIKit.UILabel messageTitleLabel { get; set; }


        [Outlet]
        UIKit.UIImageView statusImageView { get; set; }


        [Outlet]
        UIKit.UILabel titleDescriptionStatusLabel { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        UIKit.UIView verticalLineView { get; set; }


        [Outlet]
        UIKit.UILabel youHaveDoubtsDescriptionStatusLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (chatWithUsButton != null) {
                chatWithUsButton.Dispose ();
                chatWithUsButton = null;
            }

            if (checkStatusImageView != null) {
                checkStatusImageView.Dispose ();
                checkStatusImageView = null;
            }

            if (checkStatusView != null) {
                checkStatusView.Dispose ();
                checkStatusView = null;
            }

            if (containerDescriptionStatusView != null) {
                containerDescriptionStatusView.Dispose ();
                containerDescriptionStatusView = null;
            }

            if (messageDescriptionStatusLabel != null) {
                messageDescriptionStatusLabel.Dispose ();
                messageDescriptionStatusLabel = null;
            }

            if (messageTitleLabel != null) {
                messageTitleLabel.Dispose ();
                messageTitleLabel = null;
            }

            if (statusImageView != null) {
                statusImageView.Dispose ();
                statusImageView = null;
            }

            if (titleDescriptionStatusLabel != null) {
                titleDescriptionStatusLabel.Dispose ();
                titleDescriptionStatusLabel = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (verticalLineView != null) {
                verticalLineView.Dispose ();
                verticalLineView = null;
            }

            if (youHaveDoubtsDescriptionStatusLabel != null) {
                youHaveDoubtsDescriptionStatusLabel.Dispose ();
                youHaveDoubtsDescriptionStatusLabel = null;
            }
        }
    }
}