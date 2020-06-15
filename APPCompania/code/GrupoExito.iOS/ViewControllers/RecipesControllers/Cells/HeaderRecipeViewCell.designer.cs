// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.Views.RecipesViews.Cells
{
    [Register ("HeaderRecipeViewCell")]
    partial class HeaderRecipeViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel difficultyLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel nameRecipe { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel subTitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel timeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (difficultyLabel != null) {
                difficultyLabel.Dispose ();
                difficultyLabel = null;
            }

            if (nameRecipe != null) {
                nameRecipe.Dispose ();
                nameRecipe = null;
            }

            if (subTitleLabel != null) {
                subTitleLabel.Dispose ();
                subTitleLabel = null;
            }

            if (timeLabel != null) {
                timeLabel.Dispose ();
                timeLabel = null;
            }
        }
    }
}