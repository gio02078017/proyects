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
    [Register ("PaymentCashViewCell")]
    partial class PaymentCashViewCell
    {
        [Outlet]
        UIKit.UILabel cardLabel { get; set; }


        [Outlet]
        GrupoExito.iOS.ViewControllers.ReusableControllers.GenericButton cashButton { get; set; }


        [Outlet]
        UIKit.UIStackView cashStackView { get; set; }


        [Outlet]
        UIKit.UIImageView checkboxImageView { get; set; }


        [Outlet]
        GrupoExito.iOS.ViewControllers.ReusableControllers.GenericButton dataphoneButton { get; set; }


        [Outlet]
        UIKit.UIView viewContent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cardLabel != null) {
                cardLabel.Dispose ();
                cardLabel = null;
            }

            if (cashButton != null) {
                cashButton.Dispose ();
                cashButton = null;
            }

            if (cashStackView != null) {
                cashStackView.Dispose ();
                cashStackView = null;
            }

            if (checkboxImageView != null) {
                checkboxImageView.Dispose ();
                checkboxImageView = null;
            }

            if (dataphoneButton != null) {
                dataphoneButton.Dispose ();
                dataphoneButton = null;
            }

            if (viewContent != null) {
                viewContent.Dispose ();
                viewContent = null;
            }
        }
    }
}