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
    [Register ("EmptyListView")]
    partial class EmptyListView
    {
        [Outlet]
        UIKit.UIButton addProductsButton { get; set; }


        [Outlet]
        UIKit.UILabel infoLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addProductsButton != null) {
                addProductsButton.Dispose ();
                addProductsButton = null;
            }

            if (infoLabel != null) {
                infoLabel.Dispose ();
                infoLabel = null;
            }
        }
    }
}