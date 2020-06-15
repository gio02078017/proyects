using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.iOS.Views.RecipesViews.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.RecipesControllers.Sources
{
    public partial class DetailRecipeViewSource : UITableViewSource
    {

        #region Attributtes
        private IList<RecipeDetail> Recipes;
        private IList<string> Ingredientes;
        private IList<string> Preparacion;
        #endregion

        public DetailRecipeViewSource(IList<RecipeDetail> Recipes,IList<string> Ingredients, IList<string> Preparations )
        {
            this.Recipes = Recipes;
            this.Ingredientes = Ingredients;
            this.Preparacion = Preparations;
        }


        public override nint NumberOfSections(UITableView tableView)
        {
        
            return 3;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            { 
                case 0:
                    return Recipes.Count;

                case 1:
                    return Ingredientes.Count;

                default:
                    return Preparacion.Count;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

            switch (indexPath.Section)
            {
                case 0:
                    HeaderRecipeViewCell HeaderCell = (HeaderRecipeViewCell)tableView.DequeueReusableCell(HeaderRecipeViewCell.Key, indexPath);
                    RecipeDetail recipe = Recipes[indexPath.Row];
                    HeaderCell.LoadData(recipe);
                    return HeaderCell;
                case 1:
                    NotificationsViewCell IngredientsCell = (NotificationsViewCell)tableView.DequeueReusableCell(NotificationsViewCell.Key, indexPath);
                    string ingrediente = Ingredientes[indexPath.Row];
                    IngredientsCell.Ingredients(ingrediente);
                    return IngredientsCell;
                default:
                    NotificationsViewCell Preparationscell = (NotificationsViewCell)tableView.DequeueReusableCell(NotificationsViewCell.Key, indexPath);
                    string preparacion = Preparacion[indexPath.Row];
                    Preparationscell.Preparations(preparacion, indexPath);

                    return Preparationscell;
            }
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIView view = new UIView
            {
                BackgroundColor = UIColor.White
            };
            switch (section)
            {
                case 1:
                    UILabel label = new UILabel
                    {
                        Text = "Ingredientes",
                        Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size),
                        Frame = new CGRect(15, 5, tableView.Frame.Width, 35)
                    };
                    view.AddSubview(label);
                    break;
                case 2:
                    UILabel label1 = new UILabel
                    {
                        Text = "Preparación",
                        Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextSubtitle1Size),
                        Frame = new CGRect(15, 5, tableView.Frame.Width, 35)
                    };
                    view.AddSubview(label1);
                    break;
                default:
                    break;
            }
            return view;
        }
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case 1:
                case 2:
                    return 45;
                default:
                    return 1;
            }
        }
        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 1;
        }
        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            UIView view = new UIView
            {
                BackgroundColor = UIColor.White
            };
            return view;
        }

    }
}

