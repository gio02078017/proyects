using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Cells;
using GrupoExito.iOS.ViewControllers.OrderScheduleControllers.Sources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OrderScheduleControllers
{
    public partial class ChangedOrderViewController : UIViewControllerBase
    {
        #region Attributes
        private List<Product> products;
        #endregion

        #region Properties
        public List<Product> Products { get => products; set => products = value; }
        #endregion

        #region Constructors
        public ChangedOrderViewController(IntPtr handle) : base(handle) { }
        #endregion

        #region Overrides Method
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.LoadExternalViews();
            this.LoadData();
            this.LoadHandlers();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            try
            {
                productsTableView.RegisterNibForCellReuse(HeaderChangedOrderViewCell.Nib, HeaderChangedOrderViewCell.Key);
                productsTableView.RegisterNibForCellReuse(ChangedProductsOrderViewCell.Nib, ChangedProductsOrderViewCell.Key);
                LoadNavigationView(this.NavigationController.NavigationBar);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ChangedOrderViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            ChangedOrderViewSource source = new ChangedOrderViewSource(Products);
            productsTableView.Source = source;
            productsTableView.RowHeight = UITableView.AutomaticDimension;
            productsTableView.EstimatedRowHeight = 140;
            productsTableView.ReloadData();
        }

        private void LoadHandlers()
        {
            acceptButton.TouchUpInside += AcceptButtonTouchUpInside;
            acceptButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }
        #endregion

        #region Events
        private void AcceptButtonTouchUpInside(object sender, EventArgs e)
        {
            this.DismissModalViewController(true);
        }
        #endregion
    }
}

