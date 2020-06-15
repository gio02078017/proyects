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

namespace GrupoExito.iOS.ViewControllers.RecipesControllers
{
    [Register ("DetailRecipeViewController")]
    partial class DetailRecipeViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView detailRecipeImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView detailRecipeTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView recipeView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView viewGradient { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (detailRecipeImageView != null) {
                detailRecipeImageView.Dispose ();
                detailRecipeImageView = null;
            }

            if (detailRecipeTableView != null) {
                detailRecipeTableView.Dispose ();
                detailRecipeTableView = null;
            }

            if (recipeView != null) {
                recipeView.Dispose ();
                recipeView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }

            if (viewGradient != null) {
                viewGradient.Dispose ();
                viewGradient = null;
            }
        }
    }
}