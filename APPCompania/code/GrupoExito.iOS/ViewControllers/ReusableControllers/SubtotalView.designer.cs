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
    [Register ("SubtotalView")]
    partial class SubtotalView
    {
        [Outlet]
        UIKit.UIButton bagTaxButton { get; set; }


        [Outlet]
        UIKit.UILabel bagTaxLabel { get; set; }


        [Outlet]
        UIKit.UILabel subtotalLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (bagTaxButton != null) {
                bagTaxButton.Dispose ();
                bagTaxButton = null;
            }

            if (bagTaxLabel != null) {
                bagTaxLabel.Dispose ();
                bagTaxLabel = null;
            }

            if (subtotalLabel != null) {
                subtotalLabel.Dispose ();
                subtotalLabel = null;
            }
        }
    }
}