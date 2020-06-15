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
    [Register ("SubtotalViewCell")]
    partial class SubtotalViewCell
    {
        [Outlet]
        UIKit.UILabel bagTaxLabel { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxTitleLabel { get; set; }


        [Outlet]
        UIKit.UIButton infoButton { get; set; }


        [Outlet]
        UIKit.UILabel subtotalLabel { get; set; }


        [Outlet]
        UIKit.UILabel subtotalTitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (bagTaxLabel != null) {
                bagTaxLabel.Dispose ();
                bagTaxLabel = null;
            }

            if (bagTaxTitleLabel != null) {
                bagTaxTitleLabel.Dispose ();
                bagTaxTitleLabel = null;
            }

            if (infoButton != null) {
                infoButton.Dispose ();
                infoButton = null;
            }

            if (subtotalLabel != null) {
                subtotalLabel.Dispose ();
                subtotalLabel = null;
            }

            if (subtotalTitleLabel != null) {
                subtotalTitleLabel.Dispose ();
                subtotalTitleLabel = null;
            }
        }
    }
}