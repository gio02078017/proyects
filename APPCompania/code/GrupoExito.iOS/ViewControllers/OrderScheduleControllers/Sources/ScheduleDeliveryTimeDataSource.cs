using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Sources
{
    public class ScheduleDeliveryTimeDataSource : UITableViewSource
    {
        #region Attributes
        public Action<NSIndexPath> CellSelectedHandler { get; set; }
        private ScheduleDays scheduleDays;
        #endregion

        #region Properties
        public ScheduleDays ScheduleDays
        {
            get
            {
                return scheduleDays;
            }
            set
            {
                scheduleDays = value;
            }
        }
        #endregion

        #region Constructors
        public ScheduleDeliveryTimeDataSource(ScheduleDays _scheduleDays)
        {
            ScheduleDays = _scheduleDays;
        }
        #endregion

        #region Override Methods
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            LeftCheckedTableViewCell cell = (LeftCheckedTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.LeftCheckedTableViewCell, indexPath);
            cell.Configure(ScheduleDays.Hours[indexPath.Row], ParametersManager.UserContext.Prime);
            cell.LayoutIfNeeded();
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ScheduleDays.Hours.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            CellSelectedHandler?.Invoke(indexPath);
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.ScheduleDeliveryTimeCellHeight;
        }
        #endregion
    }
}
