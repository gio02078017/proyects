using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Sources
{
    public partial class AddProductsViewSource : UITableViewSource
    {
        #region Attributes  
        private IList<Product> _products;
        #endregion

        #region Properties
        public IList<Product> Products { get => _products; set => _products = value; }
        #endregion

        #region Constructors
        public AddProductsViewSource(IList<Product> products)
        {
            this.Products = products;
        }
        #endregion


        #region Overrides Methods 

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Products.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            SelectProductsViewCell cell = (SelectProductsViewCell)tableView.DequeueReusableCell(ConstantIdentifier.SelectProductsIdentifier, indexPath);
            Product product = Products[indexPath.Row];
            cell.LoadProductListViewCell(product);
            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.ProductListViewCellHeight;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.CellAt(indexPath);
            if (Products[indexPath.Row].Selected)
            {
                Products[indexPath.Row].Selected = false;
                tableView.DeselectRow(indexPath, true);
            }
            else
            {
                Products[indexPath.Row].Selected = true;
                tableView.SelectRow(indexPath, true, UITableViewScrollPosition.None);
            }
        }
        #endregion
    }
}

