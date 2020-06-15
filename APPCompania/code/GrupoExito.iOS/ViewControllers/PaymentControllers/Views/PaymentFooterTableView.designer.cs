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
    [Register ("PaymentFooterTableView")]
    partial class PaymentFooterTableView
    {
        [Outlet]
        GrupoExito.iOS.ViewControllers.ReusableControllers.GenericButton cashButton { get; set; }


        [Outlet]
        GrupoExito.iOS.ViewControllers.ReusableControllers.GenericButton dataphoneButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (cashButton != null) {
                cashButton.Dispose ();
                cashButton = null;
            }

            if (dataphoneButton != null) {
                dataphoneButton.Dispose ();
                dataphoneButton = null;
            }
        }
    }
}