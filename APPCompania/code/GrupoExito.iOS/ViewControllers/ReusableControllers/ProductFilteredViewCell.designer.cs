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
    [Register ("ProductFilteredViewCell")]
    partial class ProductFilteredViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (productNameLabel != null) {
                productNameLabel.Dispose ();
                productNameLabel = null;
            }
        }
    }
}