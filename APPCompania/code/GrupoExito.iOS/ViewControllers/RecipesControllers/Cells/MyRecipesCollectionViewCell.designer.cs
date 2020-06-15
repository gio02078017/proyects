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
    [Register ("MyRecipesCollectionViewCell")]
    partial class MyRecipesCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView backgroundImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DifficultyLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel timeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleRecipeLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (backgroundImageView != null) {
                backgroundImageView.Dispose ();
                backgroundImageView = null;
            }

            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (DifficultyLabel != null) {
                DifficultyLabel.Dispose ();
                DifficultyLabel = null;
            }

            if (timeLabel != null) {
                timeLabel.Dispose ();
                timeLabel = null;
            }

            if (titleRecipeLabel != null) {
                titleRecipeLabel.Dispose ();
                titleRecipeLabel = null;
            }
        }
    }
}