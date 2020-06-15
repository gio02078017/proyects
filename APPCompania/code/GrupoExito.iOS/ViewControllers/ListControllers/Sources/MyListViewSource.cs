using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Sources
{
    public class MyListViewSource : UITableViewSource
    {
        #region Attributes
        private IList<ShoppingList> ShoppingLists { get; set; }
        private EventHandler selectedRowEvent;
        private EventHandler deleteRowEvent;
        private EventHandler editRoWEvent;
        #endregion

        #region Properties
        public EventHandler SelectedRowEvent { get => selectedRowEvent; set => selectedRowEvent = value; }
        public EventHandler DeleteRowEvent { get => deleteRowEvent; set => deleteRowEvent = value; }
        public EventHandler EditRoWEvent { get => editRoWEvent; set => editRoWEvent = value; }
        #endregion

        #region Constructors
        public MyListViewSource(IList<ShoppingList> shoppingLists)
        {
            this.ShoppingLists = shoppingLists;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ShoppingLists.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            MyListViewCell cell = (MyListViewCell)tableView.DequeueReusableCell(ConstantIdentifier.MyListIdentifier, indexPath);
            ShoppingList currentCell = ShoppingLists[indexPath.Row];
            cell.loadListViewCell(currentCell);
            return cell;
        }


        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction[] actions = new UITableViewRowAction[2];
            UITableViewRowAction delete = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, string.Empty, (action, indexpath) =>
            {
                deleteRowEvent.Invoke(indexpath, null);
            });
            delete.BackgroundColor = new UIColor(new UIImage(ConstantImages.AccionEliminar));
            actions[0] = delete;

            UITableViewRowAction edit = UITableViewRowAction.Create(UITableViewRowActionStyle.Normal, string.Empty, (action, indexpath) =>
            {
                EditListScreenView();
                MyListViewCell cell = (MyListViewCell)tableView.DequeueReusableCell(ConstantIdentifier.MyListIdentifier, indexPath);
                editRoWEvent.Invoke(indexpath, null);
            });
            edit.BackgroundColor = new UIColor(new UIImage(ConstantImages.AccionEditar));
            actions[1] = edit;
            return actions;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.MyListViewCellHeight;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            SelectedRowEvent.Invoke(indexPath, null);
        }
        #endregion

        private static void EditListScreenView()
        {
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.EditList, nameof(MyCustomListViewController));
        }
    }
}
