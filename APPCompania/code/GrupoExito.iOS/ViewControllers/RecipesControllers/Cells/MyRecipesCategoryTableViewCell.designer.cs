// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.RecipesControllers.Cells
{
    [Register ("MyRecipesCategoryTableViewCell")]
    partial class MyRecipesCategoryTableViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel descriptionCategoryLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView iconImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameCategoryLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (descriptionCategoryLabel != null) {
                descriptionCategoryLabel.Dispose ();
                descriptionCategoryLabel = null;
            }

            if (iconImageView != null) {
                iconImageView.Dispose ();
                iconImageView = null;
            }

            if (nameCategoryLabel != null) {
                nameCategoryLabel.Dispose ();
                nameCategoryLabel = null;
            }
        }
    }
}