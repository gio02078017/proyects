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
    [Register ("LoginViewController")]
    partial class LoginViewController
    {
        [Outlet]
        GrupoExito.iOS.ViewControllers.ReusableControllers.SkipLoginView contentViewSkipeLogin { get; set; }


        [Outlet]
        UIKit.UILabel validateEmail { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint customSpinnerViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dontHaventAcountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField emailTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel emailTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView footerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton forgetPasswordButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton loginButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView loginContentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel loginTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView LoginView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView logoExitoImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel messageContentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel passwordRulesLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField passwordTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel passwordTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton registerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton showHidePasswordButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }


        [Action ("BtnLogin_TouchUpInside:")]
        partial void BtnLogin_TouchUpInside (UIKit.UIButton sender);


        [Action ("forgetPasswordbtn_TouchUpInside:")]
        partial void ForgetPasswordbtn_TouchUpInside (UIKit.UIButton sender);


        [Action ("loginButton_UpInside:")]
        partial void LoginButton_UpInside (UIKit.UIButton sender);


        [Action ("registerButton_UpInside:")]
        partial void registerButton_UpInside (UIKit.UIButton sender);


        [Action ("ReturnLogin_UpInside:")]
        partial void ReturnLogin_UpInside (UIKit.UIButton sender);


        [Action ("showHiddenPasswordButton_TouchUpInside:")]
        partial void ShowHiddenPasswordButton_TouchUpInside (UIKit.UIButton sender);


        [Action ("skipeLoginButton_UpInside:")]
        partial void skipeLoginButton_UpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (customSpinnerViewHeightConstraint != null) {
                customSpinnerViewHeightConstraint.Dispose ();
                customSpinnerViewHeightConstraint = null;
            }

            if (dontHaventAcountLabel != null) {
                dontHaventAcountLabel.Dispose ();
                dontHaventAcountLabel = null;
            }

            if (emailTextField != null) {
                emailTextField.Dispose ();
                emailTextField = null;
            }

            if (emailTitleLabel != null) {
                emailTitleLabel.Dispose ();
                emailTitleLabel = null;
            }

            if (footerView != null) {
                footerView.Dispose ();
                footerView = null;
            }

            if (forgetPasswordButton != null) {
                forgetPasswordButton.Dispose ();
                forgetPasswordButton = null;
            }

            if (loginButton != null) {
                loginButton.Dispose ();
                loginButton = null;
            }

            if (loginContentView != null) {
                loginContentView.Dispose ();
                loginContentView = null;
            }

            if (loginTitleLabel != null) {
                loginTitleLabel.Dispose ();
                loginTitleLabel = null;
            }

            if (LoginView != null) {
                LoginView.Dispose ();
                LoginView = null;
            }

            if (logoExitoImageView != null) {
                logoExitoImageView.Dispose ();
                logoExitoImageView = null;
            }

            if (messageContentLabel != null) {
                messageContentLabel.Dispose ();
                messageContentLabel = null;
            }

            if (passwordRulesLabel != null) {
                passwordRulesLabel.Dispose ();
                passwordRulesLabel = null;
            }

            if (passwordTextField != null) {
                passwordTextField.Dispose ();
                passwordTextField = null;
            }

            if (passwordTitleLabel != null) {
                passwordTitleLabel.Dispose ();
                passwordTitleLabel = null;
            }

            if (registerButton != null) {
                registerButton.Dispose ();
                registerButton = null;
            }

            if (showHidePasswordButton != null) {
                showHidePasswordButton.Dispose ();
                showHidePasswordButton = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (validateEmail != null) {
                validateEmail.Dispose ();
                validateEmail = null;
            }
        }
    }
}