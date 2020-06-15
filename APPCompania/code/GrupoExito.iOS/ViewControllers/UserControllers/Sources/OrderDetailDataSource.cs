using System;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public class OrderDetailDataSource: UITableViewSource
    {
        #region Attributes
        private OrderDetailResponse orderDetail;
        private EventHandler selectAllAction;
        #endregion

        #region Properties
        public EventHandler SelectAllAction { get => selectAllAction; set => selectAllAction = value; }
        #endregion

        #region Constructors
        public OrderDetailDataSource(OrderDetailResponse orderDetail)
        {
            this.orderDetail = orderDetail;
        }
        #endregion

        #region Override Methods
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
                    return orderDetail.Products.Count;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    HeaderOrderDetailViewCell headerCell = (HeaderOrderDetailViewCell)tableView.DequeueReusableCell(HeaderOrderDetailViewCell.Key, indexPath);
                    headerCell.LoadOrder(orderDetail);
                    headerCell.SelectAllAction = HeaderCellSelectAllAction;
                    return headerCell;
                default:
                    OrderDetailTableViewCell cell = (OrderDetailTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.OrderDetailCellIdentifier, indexPath);
                    cell.Configure(orderDetail.Products[indexPath.Row]);
                    return cell;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 1:
                    orderDetail.Products[indexPath.Row].Selected = !orderDetail.Products[indexPath.Row].Selected;
                    tableView.ReloadData();
                    break;
            }
        }
        #endregion


        #region Events
        private void HeaderCellSelectAllAction(object sender, EventArgs e)
        {
            selectAllAction?.Invoke(sender, e);
        }

        public void ChangeTableViewSelection(bool selectAll, UITableView tableView)
        {
            foreach (Product product in orderDetail.Products)
            {
                product.Selected = selectAll;
            }
            tableView.ReloadData();
        }
        #endregion
    }
}
