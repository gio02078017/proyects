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
    [Register ("AddressTypeView")]
    partial class AddressTypeView
    {
        [Outlet]
        UIKit.UIImageView coupleImageView { get; set; }


        [Outlet]
        UIKit.UILabel coupleLabel { get; set; }


        [Outlet]
        UIKit.UIView coupleView { get; set; }


        [Outlet]
        UIKit.UIImageView homeImageView { get; set; }


        [Outlet]
        UIKit.UILabel homeLabel { get; set; }


        [Outlet]
        UIKit.UIView homeView { get; set; }


        [Outlet]
        UIKit.UIImageView officeImageView { get; set; }


        [Outlet]
        UIKit.UILabel officeLabel { get; set; }


        [Outlet]
        UIKit.UIView officeView { get; set; }


        [Outlet]
        UIKit.UIImageView otherImageView { get; set; }


        [Outlet]
        UIKit.UILabel otherLabel { get; set; }


        [Outlet]
        UIKit.UIView otherView { get; set; }


        [Action ("coupleButton_UpInside:")]
        partial void coupleButton_UpInside (Foundation.NSObject sender);


        [Action ("homeButton_UpInside:")]
        partial void homeButton_UpInside (Foundation.NSObject sender);


        [Action ("officeButton_UpInside:")]
        partial void officeButton_UpInside (Foundation.NSObject sender);


        [Action ("otherButton_UpInside:")]
        partial void otherButton_UpInside (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (coupleImageView != null) {
                coupleImageView.Dispose ();
                coupleImageView = null;
            }

            if (coupleLabel != null) {
                coupleLabel.Dispose ();
                coupleLabel = null;
            }

            if (coupleView != null) {
                coupleView.Dispose ();
                coupleView = null;
            }

            if (homeImageView != null) {
                homeImageView.Dispose ();
                homeImageView = null;
            }

            if (homeLabel != null) {
                homeLabel.Dispose ();
                homeLabel = null;
            }

            if (homeView != null) {
                homeView.Dispose ();
                homeView = null;
            }

            if (officeImageView != null) {
                officeImageView.Dispose ();
                officeImageView = null;
            }

            if (officeLabel != null) {
                officeLabel.Dispose ();
                officeLabel = null;
            }

            if (officeView != null) {
                officeView.Dispose ();
                officeView = null;
            }

            if (otherImageView != null) {
                otherImageView.Dispose ();
                otherImageView = null;
            }

            if (otherLabel != null) {
                otherLabel.Dispose ();
                otherLabel = null;
            }

            if (otherView != null) {
                otherView.Dispose ();
                otherView = null;
            }
        }
    }
}