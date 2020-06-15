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
    [Register ("CashOnDeliveryView")]
    partial class CashOnDeliveryView
    {
        [Outlet]
        UIKit.UITextField cashAmountTextField { get; set; }


        [Outlet]
        UIKit.UIView cashOnPaymentAdviceView { get; set; }


        [Outlet]
        UIKit.UILabel dateLabel { get; set; }


        [Outlet]
        UIKit.UILabel scheduleLabel { get; set; }


        [Outlet]
        UIKit.UIView typeOfPaymentView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView howMuchCashView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cashAmountTextField != null) {
                cashAmountTextField.Dispose ();
                cashAmountTextField = null;
            }

            if (cashOnPaymentAdviceView != null) {
                cashOnPaymentAdviceView.Dispose ();
                cashOnPaymentAdviceView = null;
            }

            if (dateLabel != null) {
                dateLabel.Dispose ();
                dateLabel = null;
            }

            if (howMuchCashView != null) {
                howMuchCashView.Dispose ();
                howMuchCashView = null;
            }

            if (scheduleLabel != null) {
                scheduleLabel.Dispose ();
                scheduleLabel = null;
            }

            if (typeOfPaymentView != null) {
                typeOfPaymentView.Dispose ();
                typeOfPaymentView = null;
            }
        }
    }
}