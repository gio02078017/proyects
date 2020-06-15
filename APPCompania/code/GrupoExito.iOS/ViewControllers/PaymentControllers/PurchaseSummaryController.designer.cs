// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    [Register ("PurchaseSummaryController")]
    partial class PurchaseSummaryController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton buyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView purchaseSummaryTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (buyButton != null) {
                buyButton.Dispose ();
                buyButton = null;
            }

            if (purchaseSummaryTableView != null) {
                purchaseSummaryTableView.Dispose ();
                purchaseSummaryTableView = null;
            }
        }
    }
}