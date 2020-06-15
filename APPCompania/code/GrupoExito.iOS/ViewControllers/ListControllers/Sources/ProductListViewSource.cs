using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Sources
{
    public class ProductListViewSource : UITableViewSource
    {
        #region Attributes 
        private EventHandler _selectItemAction;
        private EventHandler deleteRowEvent;
        private IList<ProductList> _products;
        private UIViewControllerBase ControllerBase { get; set; }
        private bool IsEditable { get; set; }
        #endregion

        #region Properties
        public EventHandler SelectItemAction { get => _selectItemAction; set => _selectItemAction = value; }
        public EventHandler DeleteRowEvent { get => deleteRowEvent; set => deleteRowEvent = value; }
        public IList<ProductList> Products { get => _products; set => _products = value; }
        #endregion

        #region Constructors
        public ProductListViewSource(IList<ProductList> products, UIViewControllerBase controllerBase, bool isEditable)
        {
            this.Products = products;
            this.ControllerBase = controllerBase;
            this.IsEditable = isEditable;
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
            ProductListViewCell cell = (ProductListViewCell)tableView.DequeueReusableCell(ConstantIdentifier.ProductListIdentifier, indexPath);
            Product product = Products[indexPath.Row];
            cell.LoadProductListViewCell(product);
            cell.AddAction += AddTouchUpInside;
            cell.SubstractionAction += SubstractionTouchUpInside;
            cell.IndexPath = indexPath;
            if (!IsEditable)
            {
                cell.HiddenEditableImage();
            }
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            Product product = Products[indexPath.Row];
            product.Selected = !product.Selected;
            if (product.Selected)
            {
                product.Quantity = product.Quantity == 0 ? 1 : product.Quantity;
            }
            tableView.ReloadData();
            SelectItemAction(Products, null);
        }

        #endregion

        #region Methods
        public void ChangeTableViewSelection(bool selectAll, UITableView tableView)
        {
            foreach (Product product in Products)
            {
                product.Selected = selectAll;
            }
            tableView.ReloadData();
            SelectItemAction(Products, null);
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction[] actions = new UITableViewRowAction[1];
            UITableViewRowAction delete = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, string.Empty, (action, indexpath) =>
            {
                deleteRowEvent?.Invoke(indexpath, null);
            });
            delete.BackgroundColor = new UIColor(new UIImage(ConstantImages.Accion_eliminar_lista_productos));
            actions[0] = delete;
            return actions;
        }
        #endregion

        #region Events
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return IsEditable;
        }

        private void SubstractionTouchUpInside(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            ProductList product = Products[indexPath.Row];
            product.Selected = product.Quantity == 0 ? false : true;
            SelectItemAction(Products, null);
        }

        private void AddTouchUpInside(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            Products[indexPath.Row].Selected = true;
            SelectItemAction(Products, null);
        }
        #endregion
    }
}
