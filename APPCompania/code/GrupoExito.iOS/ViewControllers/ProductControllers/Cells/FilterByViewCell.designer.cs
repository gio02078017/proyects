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
    [Register ("FilterByViewCell")]
    partial class FilterByViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel quantityLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (nameLabel != null) {
                nameLabel.Dispose ();
                nameLabel = null;
            }

            if (quantityLabel != null) {
                quantityLabel.Dispose ();
                quantityLabel = null;
            }
        }
    }
}