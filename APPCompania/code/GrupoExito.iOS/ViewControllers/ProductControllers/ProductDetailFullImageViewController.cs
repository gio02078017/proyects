using System;
using System.Collections.Generic;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    public partial class ProductDetailFullImageViewController : UIViewControllerBase
    {
        #region Atributes
        private IList<String> productFullImages;
        #endregion

        #region constructors
        public ProductDetailFullImageViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region life cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.spinnerActivityIndicatorView.StartAnimating();
                //this.LoadExternalViews();
                this.LoadData();
                this.spinnerActivityIndicatorView.StopAnimating();
            }catch(Exception exception){
                Util.LogException(exception, ConstantControllersName.ProductDetailFullImageViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.IsSummaryDisabled = false;
                NavigationView.IsAccountEnabled = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailFullImageViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region methods
        public void loadImages(IList<String> productFullImages){
            this.productFullImages = productFullImages;
        }

        private void LoadExternalViews()
        {
            LoadNavigationView(this.NavigationController.NavigationBar);
        }

        private void LoadData(){
            try
            {
                productFullImageCollectionView.Delegate = new ProductFullImageCollectionSource(productFullImages);
                productFullImageCollectionView.DataSource = new ProductFullImageCollectionSource(productFullImages);
                productFullImageCollectionView.ReloadData();
            }catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductDetailFullImageViewController, ConstantMethodName.LoadData);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}

