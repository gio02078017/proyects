using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Sources
{
    public class ServiceInStoreViewSource : UITableViewSource
    {
        #region Attributes
        private EventHandler selectedAction;
        private IList<MenuItem> MenuItems;
        #endregion

        #region Properties
        public EventHandler SelectedAction { get => selectedAction; set => selectedAction = value; }
        #endregion

        #region Constructors
        public ServiceInStoreViewSource(IList<MenuItem> menuItems)
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
                    HeaderServiceInStoreViewCell  headerServiceInStoreViewCell = (HeaderServiceInStoreViewCell)tableView.DequeueReusableCell(HeaderServiceInStoreViewCell.Key, indexPath);
                    return headerServiceInStoreViewCell;
                default:
                    MenuServicesTableViewCell cell = (MenuServicesTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.MenuServicesItemIdentifier, indexPath);
                    MenuItem currentCell = MenuItems[indexPath.Row];
                    cell.Image.Image = UIImage.FromFile(currentCell.Icon);
                    cell.Title.Text = currentCell.Title;
                    if (currentCell.Subtitle.Trim().Equals(string.Empty))
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
            if (indexPath.Section == 1)
            {
                selectedAction(MenuItems[indexPath.Row].ActionName, null);
            }
        }
        #endregion
    }
}
