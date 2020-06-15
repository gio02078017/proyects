using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Sources
{
    public class ChangedOrderViewSource : UITableViewSource
    {
        #region Attributes  
        private IList<Product> _products;
        #endregion

        #region Properties
        public IList<Product> Products { get => _products; set => _products = value; }
        #endregion

        #region Constructors
        public ChangedOrderViewSource(IList<Product> products)
        {
            this.Products = products;
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
                    return Products.Count;
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    var cellHeader = tableView.DequeueReusableCell(HeaderChangedOrderViewCell.Key, indexPath);
                    return cellHeader;
               default:
                    ChangedProductsOrderViewCell cell = (ChangedProductsOrderViewCell)tableView.DequeueReusableCell(ChangedProductsOrderViewCell.Key, indexPath);
                    Product product = Products[indexPath.Row];
                    cell.LoadProductListViewCell(product);
                    return cell;
            }
        }
        #endregion
    }
}