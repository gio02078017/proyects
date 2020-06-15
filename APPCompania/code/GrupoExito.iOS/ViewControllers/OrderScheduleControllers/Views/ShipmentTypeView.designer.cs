// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Views
{
    [Register ("ShipmentTypeView")]
    partial class ShipmentTypeView
    {
        [Outlet]
        UIKit.UIButton button { get; set; }


        [Outlet]
        UIKit.UILabel descriptionLabel { get; set; }


        [Outlet]
        UIKit.UIImageView iconImageView { get; set; }


        [Outlet]
        UIKit.UIView priceBackgroundView { get; set; }


        [Outlet]
        UIKit.UILabel priceLabel { get; set; }


        [Outlet]
        UIKit.UILabel titleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (button != null) {
                button.Dispose ();
                button = null;
            }

            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (priceBackgroundView != null) {
                priceBackgroundView.Dispose ();
                priceBackgroundView = null;
            }

            if (priceLabel != null) {
                priceLabel.Dispose ();
                priceLabel = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }
        }
    }
}