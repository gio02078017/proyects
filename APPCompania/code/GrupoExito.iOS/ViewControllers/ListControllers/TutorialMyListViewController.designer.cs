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

namespace GrupoExito.iOS.ViewControllers.ListControllers
{
    [Register ("TutorialMyListViewController")]
    partial class TutorialMyListViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton addProductsButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (addProductsButton != null) {
                addProductsButton.Dispose ();
                addProductsButton = null;
            }
        }
    }
}