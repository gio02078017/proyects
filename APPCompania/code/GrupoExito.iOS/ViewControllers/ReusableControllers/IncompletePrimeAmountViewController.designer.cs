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
    [Register ("IncompletePrimeAmountViewController")]
    partial class IncompletePrimeAmountViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel adviceLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel enjoyBenefitLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel missingAmountLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (adviceLabel != null) {
                adviceLabel.Dispose ();
                adviceLabel = null;
            }

            if (enjoyBenefitLabel != null) {
                enjoyBenefitLabel.Dispose ();
                enjoyBenefitLabel = null;
            }

            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (missingAmountLabel != null) {
                missingAmountLabel.Dispose ();
                missingAmountLabel = null;
            }
        }
    }
}