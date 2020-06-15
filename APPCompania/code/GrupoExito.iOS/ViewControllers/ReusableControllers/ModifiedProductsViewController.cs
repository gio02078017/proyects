using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Sources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class ModifiedProductsViewController : UIViewController
    {
        #region Attributes
        private IList<SoldOut> ProductsRemoved { get; set; }
        private IList<SoldOut> ProductsChanged { get; set; }
        #endregion

        #region Constructors
        public ModifiedProductsViewController() : base("ModifiedProductsViewController", null)
        {
            //Default constructor this class
        }

        public ModifiedProductsViewController(IList<SoldOut> listSoldOut, IList<SoldOut> listSotckOut)
        {
            ProductsRemoved = listSoldOut;
            ProductsChanged = listSotckOut;
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                SetListView();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ModifiedProductsViewController, ConstantMethodName.ViewDidLoad);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        #endregion

        #region Methods
        private void SetListView()
        {
            productsTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.ProductModifiedTableViewCell, NSBundle.MainBundle), ConstantIdentifier.ProductModifiedTableViewCell);
            productsTableView.Source = new ModifiedProductsDataSource(ProductsRemoved, ProductsChanged);
            productsTableView.ReloadData();
            productsTableViewHeightConstraint.Constant = ProductsRemoved.Count * 88 + ProductsChanged.Count * 88 + 56;
        }
        #endregion
    }
}

