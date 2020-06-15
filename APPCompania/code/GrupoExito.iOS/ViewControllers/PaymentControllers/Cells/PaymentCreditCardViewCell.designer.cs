// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    [Register ("PaymentCreditCardViewCell")]
    partial class PaymentCreditCardViewCell
    {
        [Outlet]
        UIKit.UIView cardImageView { get; set; }


        [Outlet]
        UIKit.UIImageView checkboxImageView { get; set; }


        [Outlet]
        UIKit.UIImageView imageCardImageView { get; set; }


        [Outlet]
        UIKit.UILabel numberCardLabel { get; set; }


        [Outlet]
        UIKit.UIView viewContent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cardImageView != null) {
                cardImageView.Dispose ();
                cardImageView = null;
            }

            if (checkboxImageView != null) {
                checkboxImageView.Dispose ();
                checkboxImageView = null;
            }

            if (imageCardImageView != null) {
                imageCardImageView.Dispose ();
                imageCardImageView = null;
            }

            if (numberCardLabel != null) {
                numberCardLabel.Dispose ();
                numberCardLabel = null;
            }

            if (viewContent != null) {
                viewContent.Dispose ();
                viewContent = null;
            }
        }
    }
}