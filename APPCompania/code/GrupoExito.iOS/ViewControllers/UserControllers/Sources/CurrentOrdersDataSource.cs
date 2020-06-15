using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public class CurrentOrdersDataSource : UITableViewSource, ICurrentOrderCell
    {
        #region Attributes
        private EventHandler<Order> showOrderHandler;
        private IList<OrderInformation> _orders { get; set; }
        #endregion

        #region Properties
        public EventHandler<Order> ShowOrderHandler { get => showOrderHandler; set => showOrderHandler = value; }
        #endregion

        #region Constructors 
        public CurrentOrdersDataSource(IList<OrderInformation> orders)
        {
            _orders = orders;
        }
        #endregion

        #region Overrides Methods 
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            CurrentOrderTableViewCell cell = (CurrentOrderTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.CurrentOrderCellIdentifier, indexPath);
            cell.Configure(indexPath.Section == 0);
            return cell;
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    ((CurrentOrderTableViewCell)cell).LoadData(_orders[0].HomeDelivery[indexPath.Row], this);
                    break;
                case 1:
                    ((CurrentOrderTableViewCell)cell).LoadData(_orders[0].PickUp[indexPath.Row], this);
                    break;
            }
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case 0:
                    return _orders[0].HomeDelivery.Count;
                case 1:
                    return _orders[0].PickUp.Count;
                default:
                    return 0;
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    return ConstantViewSize.HomeDeliveryCurrentOrderCellHeight;
                case 1:
                    return ConstantViewSize.PickUpCurrentOrderCellHeight;
                default:
                    return 0;
            }
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIView headerView = new UIView
            {
                BackgroundColor = ConstantColor.UiHeaderOrderCurrent
            };
            UIImageView headerImageView = (section == 0) ? new UIImageView(UIImage.FromFile(ConstantImages.Programada)) :
                new UIImageView(UIImage.FromFile(ConstantImages.RecogerTienda));
            headerImageView.Frame = new CGRect(15, 10, 20, 20);
            headerImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            headerView.AddSubview(headerImageView);

            UILabel headerLabel = new UILabel
            {
                Text = (section == 0) ? AppMessages.HomeText : AppMessages.StoreText,
                Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.PaymentSubtitleFontSize),
                Frame = new CGRect(45, 10, 200, 21)
            };
            headerView.AddSubview(headerLabel);

            headerView.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;

            return headerView;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            return 40.0f;
        }
        #endregion

        #region ICurrentOrderCell implementation
        public void ShowOrderSelected(Order order)
        {
            ShowOrderHandler?.Invoke(this, order);
        }
        #endregion
    }
}
