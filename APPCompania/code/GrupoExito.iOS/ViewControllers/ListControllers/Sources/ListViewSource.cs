using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Sources
{
    public partial class ListViewSource : UITableViewSource
    {
        #region Attributes
        private IList<ShoppingList> shoppingLists;
        #endregion

        #region Constructor
        public ListViewSource(IList<ShoppingList> shoppingLists)
        {
            this.shoppingLists = shoppingLists;
        }
        #endregion

        #region Overrides Methods
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return shoppingLists.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            NameListViewCell cell = (NameListViewCell)tableView.DequeueReusableCell(ConstantIdentifier.NameListViewCellIdentifier, indexPath);
            ShoppingList item = shoppingLists[indexPath.Row];
            cell.LoadNameList(item);
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ParametersManager.ShoppingListSelectedId = shoppingLists[indexPath.Row].Id;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.NameCustomerList;
        }
        #endregion
    }
}

