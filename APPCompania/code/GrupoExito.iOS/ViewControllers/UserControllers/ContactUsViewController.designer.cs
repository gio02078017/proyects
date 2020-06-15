// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("ContactUsViewController")]
    partial class ContactUsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint contactStoreHeightTableViewConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView contactStoreTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel scheduleOfAttentionInfoLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel scheduleOfAttentionTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView scheduleOfAttentionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView titleImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (contactStoreHeightTableViewConstraint != null) {
                contactStoreHeightTableViewConstraint.Dispose ();
                contactStoreHeightTableViewConstraint = null;
            }

            if (contactStoreTableView != null) {
                contactStoreTableView.Dispose ();
                contactStoreTableView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (scheduleOfAttentionInfoLabel != null) {
                scheduleOfAttentionInfoLabel.Dispose ();
                scheduleOfAttentionInfoLabel = null;
            }

            if (scheduleOfAttentionTitleLabel != null) {
                scheduleOfAttentionTitleLabel.Dispose ();
                scheduleOfAttentionTitleLabel = null;
            }

            if (scheduleOfAttentionView != null) {
                scheduleOfAttentionView.Dispose ();
                scheduleOfAttentionView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (titleImageView != null) {
                titleImageView.Dispose ();
                titleImageView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}