// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.CategoryControllers.Cells
{
    [Register ("CategoryViewCell")]
    partial class CategoryViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconCategoryGrayImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconCategoryImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameCategoryLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (iconCategoryGrayImageView != null) {
                iconCategoryGrayImageView.Dispose ();
                iconCategoryGrayImageView = null;
            }

            if (iconCategoryImageView != null) {
                iconCategoryImageView.Dispose ();
                iconCategoryImageView = null;
            }

            if (nameCategoryLabel != null) {
                nameCategoryLabel.Dispose ();
                nameCategoryLabel = null;
            }
        }
    }
}