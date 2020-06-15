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
    [Register ("CreditCardViewCell")]
    partial class CreditCardViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton alertCaduceCreditButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView alertCaduceCreditImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel cardDateCaducedLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel cardTitleMessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView detailCellImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconCaduceImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView imageCardImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView infoCaduceCreditStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameCardLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel numberCardLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewContent { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (alertCaduceCreditButton != null) {
                alertCaduceCreditButton.Dispose ();
                alertCaduceCreditButton = null;
            }

            if (alertCaduceCreditImageView != null) {
                alertCaduceCreditImageView.Dispose ();
                alertCaduceCreditImageView = null;
            }

            if (cardDateCaducedLabel != null) {
                cardDateCaducedLabel.Dispose ();
                cardDateCaducedLabel = null;
            }

            if (cardTitleMessageLabel != null) {
                cardTitleMessageLabel.Dispose ();
                cardTitleMessageLabel = null;
            }

            if (detailCellImageView != null) {
                detailCellImageView.Dispose ();
                detailCellImageView = null;
            }

            if (iconCaduceImageView != null) {
                iconCaduceImageView.Dispose ();
                iconCaduceImageView = null;
            }

            if (imageCardImageView != null) {
                imageCardImageView.Dispose ();
                imageCardImageView = null;
            }

            if (infoCaduceCreditStackView != null) {
                infoCaduceCreditStackView.Dispose ();
                infoCaduceCreditStackView = null;
            }

            if (nameCardLabel != null) {
                nameCardLabel.Dispose ();
                nameCardLabel = null;
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