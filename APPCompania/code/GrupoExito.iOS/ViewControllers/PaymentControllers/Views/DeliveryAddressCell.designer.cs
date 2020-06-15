// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    [Register ("DeliveryAddressCell")]
    partial class DeliveryAddressCell
    {
        [Outlet]
        UIKit.UILabel addressLabel { get; set; }


        [Outlet]
        UIKit.UIView containerView { get; set; }


        [Outlet]
        UIKit.UIImageView imageView { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel descriptionLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addressLabel != null) {
                addressLabel.Dispose ();
                addressLabel = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}