using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Sources
{
    public class SearchProductTableViewSource : UITableViewSource
    {
        #region Attributes
        private List<Item> Suggestions;
        private UIViewControllerBase ControllerBase;
        #endregion

        #region Constructors 
        public SearchProductTableViewSource(UIViewControllerBase controllerBase, List<Item> Suggestions)
        {
            this.ControllerBase = controllerBase;
            this.Suggestions = Suggestions;
        }
        #endregion

        #region Methods Override
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Suggestions.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            Item Data = Suggestions[indexPath.Row];
            ProductFilteredViewCell cell = tableView.DequeueReusableCell(ConstantIdentifier.ProductFilteredIdentifier, indexPath) as ProductFilteredViewCell;
            cell.ProductName.Text = Data.Text;
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.ProductByCategoryViewController) is ProductByCategoryViewController ProductByCategoryViewController)
            {
                ParametersManager.ContainChanges = false;
                ParametersManager.CategoriesName = null;
                ParametersManager.BrandsName = null;
                ParametersManager.OrderBy = ConstOrder.Name;
                ParametersManager.OrderType = ConstOrderType.Desc;
                ParametersManager.UserQuery = Suggestions[indexPath.Row].Text;
                Category category = null;
                ProductByCategoryViewController._category = category;
                ProductByCategoryViewController.HidesBottomBarWhenPushed = true;
                ProductByCategoryViewController.IsSearcherProduct = true;
                ControllerBase.NavigationController.PushViewController(ProductByCategoryViewController, true);
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.ProductSearcherHeightCell;
        }

        #endregion
    }
}