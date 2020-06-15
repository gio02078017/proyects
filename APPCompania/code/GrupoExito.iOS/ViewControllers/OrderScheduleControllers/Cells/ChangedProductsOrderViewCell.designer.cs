// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Cells
{
    [Register ("ChangedProductsOrderViewCell")]
    partial class ChangedProductsOrderViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PriceProductLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel pumLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (nameProductLabel != null) {
                nameProductLabel.Dispose ();
                nameProductLabel = null;
            }

            if (PriceProductLabel != null) {
                PriceProductLabel.Dispose ();
                PriceProductLabel = null;
            }

            if (productImageView != null) {
                productImageView.Dispose ();
                productImageView = null;
            }

            if (pumLabel != null) {
                pumLabel.Dispose ();
                pumLabel = null;
            }
        }
    }
}