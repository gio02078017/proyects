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
    [Register ("ProductFullImageCollectionViewCell")]
    partial class ProductFullImageCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView productFullImageImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (productFullImageImageView != null) {
                productFullImageImageView.Dispose ();
                productFullImageImageView = null;
            }
        }
    }
}