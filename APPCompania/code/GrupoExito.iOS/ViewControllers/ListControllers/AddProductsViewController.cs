using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.Views.ListViews
{
    public partial class AddProductsViewController : BaseProductController
    {

        #region Attributes
        private List<Product> Products;
        private ShoppingListModel ShoppingListModel { get; set; }
        private OrderModel OrderModel { get; set; }
        private SuggestedProductsResponse SuggestedProductsResponse;

        #endregion


        public AddProductsViewController(IntPtr handle) : base(handle)
        {
            ShoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
        }

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadExternalViews();
                buttonsStackView.Hidden = true;
                InvokeOnMainThread(async () =>
                {
                    await this.GetRecommendProducsAsync();
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.IsSummaryDisabled = true;
                NavigationView.IsAccountEnabled = false;

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddProductsViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }

        }
        #endregion


        #region Methods Async 
        private async Task GetRecommendProducsAsync()
        {
            await this.GetRecommendProducs();
        }

        private async Task GetRecommendProducs()
        {
            try
            {
                StartActivityIndicatorCustom();
                ProductParameters parameters = new ProductParameters()
                {
                    DependencyId = ParametersManager.UserContext.DependencyId,
                    From = ParametersManager.FromRecommendProducts,
                    Size = ParametersManager.SizeRecommendProducts
                };

                SuggestedProductsResponse = await ShoppingListModel.GetSuggestedProducts(parameters);
                if (SuggestedProductsResponse.Result != null && SuggestedProductsResponse.Result.HasErrors && SuggestedProductsResponse.Result.Messages != null)
                {
                    if (SuggestedProductsResponse.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(SuggestedProductsResponse.Result);
                        StartActivityErrorMessage(SuggestedProductsResponse.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    Products = new List<Product>();
                    Products.AddRange(SuggestedProductsResponse.ProductsClient);
                    SetSuggestedList();
                }
                //this.CalculateProductsSelected(Products);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.GetRecommendProducs);
            }
        }
        #endregion


        #region Methods

        private void SetSuggestedList()
        {
            if (Products != null && Products.Count > 0)
            {
                //countProductsLabel.Text = Products.Count + " " + AppMessages.Products;
                Util.SetConstraint(addProductsTableView, addProductsTableView.Frame.Height, (Products.Count * ConstantViewSize.ProductListViewCellHeight));
                AddProductsViewSource addProductsViewSource = new AddProductsViewSource(Products);
                addProductsTableView.Source = addProductsViewSource;
                addProductsTableView.ReloadData();
                StopActivityIndicatorCustom();
            }
            else
            {
                //countProductsLabel.Text = "Sin productos";
            }
        }


        private void LoadExternalViews()
        {
            try
            {
                addProductsTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.SelectProductsViewCell, NSBundle.MainBundle), ConstantIdentifier.SelectProductsIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                this.NavigationController.NavigationBarHidden = false;
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events

        #endregion
    }
}

