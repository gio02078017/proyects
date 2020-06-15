// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    [Register ("MyListViewCell")]
    partial class MyListViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView listNameStackeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameListLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel productsLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (listNameStackeView != null) {
                listNameStackeView.Dispose ();
                listNameStackeView = null;
            }

            if (nameListLabel != null) {
                nameListLabel.Dispose ();
                nameListLabel = null;
            }

            if (productsLabel != null) {
                productsLabel.Dispose ();
                productsLabel = null;
            }
        }
    }
}