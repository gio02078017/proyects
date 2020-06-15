using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS.Views.RecipesViews.Cells
{
    public partial class HeaderRecipeViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("HeaderRecipeViewCell");
        public static readonly UINib Nib;

        static HeaderRecipeViewCell()
        {
            Nib = UINib.FromName("HeaderRecipeViewCell", NSBundle.MainBundle);
        }

        protected HeaderRecipeViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

            public void LoadData(RecipeDetail recipe) {
            nameRecipe.Text = recipe.Title;
            subTitleLabel.Text = recipe.Subtitle;
            difficultyLabel.Text = recipe.Difficulty;
            timeLabel.Text = recipe.PreparationTime;
         
            }
    }
}
