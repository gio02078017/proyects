// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("ChangePasswordUserViewController")]
    partial class ChangePasswordUserViewController
    {
        [Outlet]
        UIKit.UITextField textFieldConfirmPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ConfirmNewPasswordTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CurrentPasswordTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint customSpinnerViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelChangePasswordTitle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelConfirmPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelCurrentPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelDescriptionAction { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelNewPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelRulesNewPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView navigationView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewPasswordTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShowHiddenConfirmNewButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShowHiddenCurrentPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ShowHiddenNewPasswordButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UpdateButton { get; set; }


        [Action ("buttonUpdate_UpInside:")]
        partial void buttonUpdate_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (ConfirmNewPasswordTextField != null) {
                ConfirmNewPasswordTextField.Dispose ();
                ConfirmNewPasswordTextField = null;
            }

            if (CurrentPasswordTextField != null) {
                CurrentPasswordTextField.Dispose ();
                CurrentPasswordTextField = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (customSpinnerViewHeightConstraint != null) {
                customSpinnerViewHeightConstraint.Dispose ();
                customSpinnerViewHeightConstraint = null;
            }

            if (LabelChangePasswordTitle != null) {
                LabelChangePasswordTitle.Dispose ();
                LabelChangePasswordTitle = null;
            }

            if (LabelConfirmPassword != null) {
                LabelConfirmPassword.Dispose ();
                LabelConfirmPassword = null;
            }

            if (LabelCurrentPassword != null) {
                LabelCurrentPassword.Dispose ();
                LabelCurrentPassword = null;
            }

            if (LabelDescriptionAction != null) {
                LabelDescriptionAction.Dispose ();
                LabelDescriptionAction = null;
            }

            if (LabelNewPassword != null) {
                LabelNewPassword.Dispose ();
                LabelNewPassword = null;
            }

            if (LabelRulesNewPassword != null) {
                LabelRulesNewPassword.Dispose ();
                LabelRulesNewPassword = null;
            }

            if (navigationView != null) {
                navigationView.Dispose ();
                navigationView = null;
            }

            if (NewPasswordTextField != null) {
                NewPasswordTextField.Dispose ();
                NewPasswordTextField = null;
            }

            if (ShowHiddenConfirmNewButton != null) {
                ShowHiddenConfirmNewButton.Dispose ();
                ShowHiddenConfirmNewButton = null;
            }

            if (ShowHiddenCurrentPassword != null) {
                ShowHiddenCurrentPassword.Dispose ();
                ShowHiddenCurrentPassword = null;
            }

            if (ShowHiddenNewPasswordButton != null) {
                ShowHiddenNewPasswordButton.Dispose ();
                ShowHiddenNewPasswordButton = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (UpdateButton != null) {
                UpdateButton.Dispose ();
                UpdateButton = null;
            }
        }
    }
}