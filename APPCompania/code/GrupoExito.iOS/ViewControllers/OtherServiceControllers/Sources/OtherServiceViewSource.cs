using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.OtherServiceControllers.Cells;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers.Sources
{
    public class OtherServiceViewSource : UITableViewSource
    {
        #region Attributes
        private IList<MenuItem> MenuItems { get; set; }
        private EventHandler selectRowAction;
        #endregion

        #region Properties
        public EventHandler SelectRowAction { get => selectRowAction; set => selectRowAction = value; }
        #endregion

        #region Constructors
        public OtherServiceViewSource(IList<MenuItem> menuItems)
        {
            this.MenuItems = menuItems;
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
                    return MenuItems.Count;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    HeaderOtherServiceViewCell headerOtherServiceViewCell = (HeaderOtherServiceViewCell)tableView.DequeueReusableCell(HeaderOtherServiceViewCell.Key, indexPath);
                    return headerOtherServiceViewCell;
                default:
                    MenuServicesTableViewCell cell = (MenuServicesTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.MenuServicesItemIdentifier, indexPath);
                    MenuItem currentCell = MenuItems[indexPath.Row];
                    cell.Image.Image = UIImage.FromFile(currentCell.Icon);
                    cell.Title.Text = currentCell.Title;
                    if (string.IsNullOrEmpty(currentCell.Subtitle.Trim()))
                    {
                        cell.Descriptions.Hidden = true;
                        cell.Descriptions.Text = string.Empty;
                    }
                    else
                    {
                        cell.Descriptions.Hidden = false;
                        cell.Descriptions.Text = currentCell.Subtitle;
                    }
                    return cell;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            selectRowAction?.Invoke(indexPath, null);
        }
        #endregion
    }
}
