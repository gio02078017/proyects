// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    [Register ("CurrentOrderTableViewCell")]
    partial class CurrentOrderTableViewCell
    {
        [Outlet]
        UIKit.UIView orderInfoView { get; set; }


        [Outlet]
        UIKit.UIImageView recipientCheckImageView { get; set; }


        [Outlet]
        UIKit.UILabel recipientInfoLabel { get; set; }


        [Outlet]
        UIKit.UIView recipientInfoView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel dateValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deliveryOnLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel deliveryOnValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView lineView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderValueLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton seeOrderButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel stateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel stateValueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (dateValueLabel != null) {
                dateValueLabel.Dispose ();
                dateValueLabel = null;
            }

            if (deliveryOnLabel != null) {
                deliveryOnLabel.Dispose ();
                deliveryOnLabel = null;
            }

            if (deliveryOnValueLabel != null) {
                deliveryOnValueLabel.Dispose ();
                deliveryOnValueLabel = null;
            }

            if (lineView != null) {
                lineView.Dispose ();
                lineView = null;
            }

            if (orderInfoView != null) {
                orderInfoView.Dispose ();
                orderInfoView = null;
            }

            if (orderLabel != null) {
                orderLabel.Dispose ();
                orderLabel = null;
            }

            if (orderValueLabel != null) {
                orderValueLabel.Dispose ();
                orderValueLabel = null;
            }

            if (recipientCheckImageView != null) {
                recipientCheckImageView.Dispose ();
                recipientCheckImageView = null;
            }

            if (recipientInfoLabel != null) {
                recipientInfoLabel.Dispose ();
                recipientInfoLabel = null;
            }

            if (recipientInfoView != null) {
                recipientInfoView.Dispose ();
                recipientInfoView = null;
            }

            if (seeOrderButton != null) {
                seeOrderButton.Dispose ();
                seeOrderButton = null;
            }

            if (stateLabel != null) {
                stateLabel.Dispose ();
                stateLabel = null;
            }

            if (stateValueLabel != null) {
                stateValueLabel.Dispose ();
                stateValueLabel = null;
            }
        }
    }
}