using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public class HistoricalOrdersDataSource: UITableViewSource
    {
        #region Attributes
        private IList<Order> _orders;
        private EventHandler<Order> showOrderHandler;
        #endregion

        #region Properties
        public EventHandler<Order> ShowOrderHandler { get => showOrderHandler; set => showOrderHandler = value; }
        #endregion

        #region Constructors
        public HistoricalOrdersDataSource(IList<Order> orders)
        {
            _orders = orders;
        }
        #endregion

        #region Override Methods
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            HistoricalOrderTableViewCell cell = (HistoricalOrderTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.HistoricalCellIdentifier, indexPath);
            cell.LoadData(_orders[indexPath.Row]);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _orders.Count;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            ShowOrderHandler?.Invoke(this, _orders[indexPath.Row]);
        }
        #endregion
    }
}
