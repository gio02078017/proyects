using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers.Cells
{
    public class MySoatViewSource : UITableViewSource
    {
        #region Attributes
        private EventHandler selectedAction;
        private IList<Soat> soatList;
        #endregion

        #region Properties
        public EventHandler SelectedAction { get => selectedAction; set => selectedAction = value; }
        #endregion

        #region Constructors
        public MySoatViewSource(IList<Soat> soatList)
        {
            this.soatList = soatList;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case 0:
                    return 1;
                default:
                    return soatList.Count;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    HeaderOtherServiceViewCell headerOtherServiceViewCell = (HeaderOtherServiceViewCell)tableView.DequeueReusableCell(HeaderOtherServiceViewCell.Key, indexPath);
                    headerOtherServiceViewCell.setTitle("Mi SOAT");
                    return headerOtherServiceViewCell;
                default:
                    Soat soatData = soatList[indexPath.Row];
                    MySoatViewCell cell = (MySoatViewCell)tableView.DequeueReusableCell(MySoatViewCell.Key, indexPath);
                    cell.setTitle(soatData.Plate);
                    return cell;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 1)
            {
                selectedAction(indexPath, null);
            }
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction[] actions = new UITableViewRowAction[1];
            UITableViewRowAction delete = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, "Eliminar", (action, indexpath) =>
            {
                Logic.Models.Generic.DocumentsDataBaseModel documentsDataBaseModel = new Logic.Models.Generic.DocumentsDataBaseModel(DataAgent.DataBase.DocumentsDataBase.Instance);
                documentsDataBaseModel.DeleteSoat(soatList[indexPath.Row].Plate);
                soatList.Remove(soatList[indexpath.Row]);
                tableView.ReloadData();
            });
            delete.BackgroundColor = UIColor.Red;
            actions[0] = delete;
            return actions;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }
        #endregion
    }
}

