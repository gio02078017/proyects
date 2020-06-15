using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class NotificationsTableViewSource : UITableViewSource
    {
        #region
        private UIViewControllerBase ControllerBase;
        private List<AppNotification> GetNotifications;
        #endregion

        #region Constructor
        public NotificationsTableViewSource(List<AppNotification> appNotifications, UIViewControllerBase viewController)
        {
            this.GetNotifications = appNotifications;
            this.ControllerBase = viewController;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return GetNotifications.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            try
            {
                NotificationsViewCell cell = tableView.DequeueReusableCell(ConstantIdentifier.NotificationsViewCellIdentifier, indexPath) as NotificationsViewCell;
                AppNotification row = GetNotifications[indexPath.Row];
                cell.LoadData(row);
                return cell;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 100; ////.ContactUsMenuHeightCell;
        }
        #endregion
    }
}


