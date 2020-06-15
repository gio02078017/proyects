using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Logic.Models;
using GrupoExito.iOS.ViewControllers.RecipesControllers.Cells;
using UIKit;
using GrupoExito.iOS.Referentials;

namespace GrupoExito.iOS.ViewControllers.RecipesControllers.Sources
{
    public partial class MyRecipeTableViewSource : UITableViewSource
    {

        #region Attributes
        private IList<RecipeCategory> GetCategories;
        private UIViewControllerBase controllerBase;
        #endregion

        #region Constructors
        public MyRecipeTableViewSource(IList<RecipeCategory> recipeCategories, InitialRecipeViewController initial)
        {
            this.GetCategories = recipeCategories;
            this.controllerBase = initial;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }


        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return GetCategories.Count;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            MyRecipesCategoryTableViewCell cell = tableView.DequeueReusableCell(ConstantIdentifier.MyRecipesCategoryIdentifier, indexPath) as MyRecipesCategoryTableViewCell;
            RecipeCategory row = GetCategories[indexPath.Row];
            cell.LoadCategorysViewCell(row);
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            MyRecipesViewController myRecipesViewController_ = (MyRecipesViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.MyRecipesViewController);
            myRecipesViewController_.HidesBottomBarWhenPushed = true;
            myRecipesViewController_.Category = GetCategories[indexPath.Row];
            controllerBase.NavigationController.PushViewController(myRecipesViewController_, true);
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.RecipesHeightCell;
        }
        #endregion
    }
}

