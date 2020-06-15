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

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    [Register ("MarkerInfoView")]
    partial class MarkerInfoView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel addressLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton closeButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel scheduleDescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel scheduleTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel servicesDescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel servicesTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel storeNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel telephoneLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton wazeButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addressLabel != null) {
                addressLabel.Dispose ();
                addressLabel = null;
            }

            if (closeButton != null) {
                closeButton.Dispose ();
                closeButton = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (scheduleDescriptionLabel != null) {
                scheduleDescriptionLabel.Dispose ();
                scheduleDescriptionLabel = null;
            }

            if (scheduleTitleLabel != null) {
                scheduleTitleLabel.Dispose ();
                scheduleTitleLabel = null;
            }

            if (servicesDescriptionLabel != null) {
                servicesDescriptionLabel.Dispose ();
                servicesDescriptionLabel = null;
            }

            if (servicesTitleLabel != null) {
                servicesTitleLabel.Dispose ();
                servicesTitleLabel = null;
            }

            if (storeNameLabel != null) {
                storeNameLabel.Dispose ();
                storeNameLabel = null;
            }

            if (telephoneLabel != null) {
                telephoneLabel.Dispose ();
                telephoneLabel = null;
            }

            if (wazeButton != null) {
                wazeButton.Dispose ();
                wazeButton = null;
            }
        }
    }
}