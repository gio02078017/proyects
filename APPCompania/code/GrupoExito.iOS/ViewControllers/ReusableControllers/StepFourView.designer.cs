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
    [Register ("StepFourView")]
    partial class StepFourView
    {
        [Outlet]
        UIKit.UIImageView checkStatusImageView { get; set; }


        [Outlet]
        UIKit.UIView checkStatusView { get; set; }


        [Outlet]
        UIKit.UIView containerDescriptionStatusView { get; set; }


        [Outlet]
        UIKit.UILabel helpUsToQualifyLabel { get; set; }


        [Outlet]
        UIKit.UILabel messageTitleLabel { get; set; }


        [Outlet]
        UIKit.UIButton rateYouExperienceButton { get; set; }


        [Outlet]
        UIKit.UIImageView statusImageView { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }


        [Outlet]
        UIKit.UIView verticalLineView { get; set; }

        void ReleaseDesignerOutlets ()
        {
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

            if (helpUsToQualifyLabel != null) {
                helpUsToQualifyLabel.Dispose ();
                helpUsToQualifyLabel = null;
            }

            if (messageTitleLabel != null) {
                messageTitleLabel.Dispose ();
                messageTitleLabel = null;
            }

            if (rateYouExperienceButton != null) {
                rateYouExperienceButton.Dispose ();
                rateYouExperienceButton = null;
            }

            if (statusImageView != null) {
                statusImageView.Dispose ();
                statusImageView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (verticalLineView != null) {
                verticalLineView.Dispose ();
                verticalLineView = null;
            }
        }
    }
}