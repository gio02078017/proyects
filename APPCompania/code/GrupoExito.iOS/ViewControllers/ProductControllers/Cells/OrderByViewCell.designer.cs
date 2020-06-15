// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    [Register ("OrderByViewCell")]
    partial class OrderByViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel orderByLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch statusSwitch { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (orderByLabel != null) {
                orderByLabel.Dispose ();
                orderByLabel = null;
            }

            if (statusSwitch != null) {
                statusSwitch.Dispose ();
                statusSwitch = null;
            }
        }
    }
}