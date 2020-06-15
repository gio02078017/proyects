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

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("MyDiscountView")]
    partial class MyDiscountView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton beforeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView beforeImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView myDiscountCollectionView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton nextButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView nextImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (beforeButton != null) {
                beforeButton.Dispose ();
                beforeButton = null;
            }

            if (beforeImageView != null) {
                beforeImageView.Dispose ();
                beforeImageView = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (myDiscountCollectionView != null) {
                myDiscountCollectionView.Dispose ();
                myDiscountCollectionView = null;
            }

            if (nextButton != null) {
                nextButton.Dispose ();
                nextButton = null;
            }

            if (nextImageView != null) {
                nextImageView.Dispose ();
                nextImageView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}