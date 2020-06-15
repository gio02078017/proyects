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
    [Register ("UserProfileViewController")]
    partial class UserProfileViewController
    {
        [Outlet]
        UIKit.UILabel SuperclientLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton closeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton editButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView profileCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (closeButton != null) {
                closeButton.Dispose ();
                closeButton = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (editButton != null) {
                editButton.Dispose ();
                editButton = null;
            }

            if (nameLabel != null) {
                nameLabel.Dispose ();
                nameLabel = null;
            }

            if (profileCollectionView != null) {
                profileCollectionView.Dispose ();
                profileCollectionView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (SuperclientLabel != null) {
                SuperclientLabel.Dispose ();
                SuperclientLabel = null;
            }
        }
    }
}