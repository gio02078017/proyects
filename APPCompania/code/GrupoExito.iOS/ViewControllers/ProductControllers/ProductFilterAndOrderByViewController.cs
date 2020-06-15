using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Cells;
using GrupoExito.iOS.ViewControllers.ProductControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    public partial class ProductFilterAndOrderByViewController : BaseProductController
    {
        #region Attributes
        private Category category;
        private bool isSearcherProduct = false;
        private bool isFilterAction;
        #endregion

        #region Properties
        public Category Category { get => category; set => category = value; }
        public bool IsSearcherProduct { get => isSearcherProduct; set => isSearcherProduct = value; }
        public bool IsFilterAction { get => isFilterAction; set => isFilterAction = value; }
        #endregion

        #region Constructors
        public ProductFilterAndOrderByViewController(IntPtr handle) : base(handle)
        {
            _productsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
        }
        #endregion

        #region life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadData();
                this.LoadHandlers();
                base.StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.ViewWillAppear } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.IsSummaryDisabled = false;
                NavigationView.IsAccountEnabled = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            filterOrderTableView.RegisterNibForHeaderFooterViewReuse(FilterHeaderView.Nib, FilterHeaderView.Key);
            filterOrderTableView.RegisterNibForCellReuse(HeaderProductViewCell.Nib, HeaderProductViewCell.Key);
            filterOrderTableView.RegisterNibForCellReuse(FilterByViewCell.Nib, FilterByViewCell.Key);
            filterOrderTableView.RegisterNibForCellReuse(OrderByViewCell.Nib, OrderByViewCell.Key);

            LoadNavigationView(this.NavigationController.NavigationBar);
            LoadSearchProductsView(searchProductView);
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            if (IsSearcherProduct)
            {
                NavigationView.IsSummaryDisabled = true;
                NavigationView.IsAccountEnabled = false;
            }
            SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            CustomSpinnerViewFromBase = customSpinnerView;
        }


        private void LoadCorners()
        {
            ApplyChangeButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            ClearButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            ClearButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
            ClearButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
        }

        private void LoadData()
        {
            if (isFilterAction) 
            {
                FilterByUpInside(null, null);
            }
            else
            {
                OrderByUpInside(null, null);
            }
        }

        private void LoadHandlers()
        {
            ClearButton.TouchUpInside += ClearFilterAndOrderUpInside;
            ApplyChangeButton.TouchUpInside += ApplyChangeUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private async Task GetProductsCategories()
        {
            try
            {
                StartActivityIndicatorCustom();
                SearchProductsParameters parameters = new SearchProductsParameters()
                {
                    DependencyId = ValidateDependecy(),
                    CategoryId = Category != null ? Category.Id : string.Empty,
                    CategoriesNames = ParametersManager.CategoriesName,
                    Brands = ParametersManager.BrandsName,
                    Size = ParametersManager.Size,
                    From = ParametersManager.From,
                    OrderBy = ParametersManager.OrderByFilter,
                    OrderType = ParametersManager.OrderType
                };

                var response = await _productsModel.GetProducts(parameters);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        String message = MessagesHelper.GetMessage(response.Result);

                        if (!string.IsNullOrEmpty(message))
                        {
                            StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                        }
                    }
                }
                else
                {
                    ParametersManager.ContainChanges = true;
                    this.NavigationController.PopViewController(true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.GetProductsCategories);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private void ClearFilterAndOrderUpInside(object sender, EventArgs e)
        {
            try
            {
                if (isFilterAction)
                {
                    FilterProductTableViewSource sourceFilter = (filterOrderTableView.Source as FilterProductTableViewSource);
                    sourceFilter.ClearAll(filterOrderTableView);
                }
                else
                {
                    OrderProductTableViewSource sourceOrderBy = (filterOrderTableView.Source as OrderProductTableViewSource);
                    sourceOrderBy.ClearAll(filterOrderTableView);
                }
                ParametersManager.CategoriesName = null;
                ParametersManager.BrandsName = null;
                ParametersManager.OrderByFilter = ConstOrder.Relevance;
                ParametersManager.OrderBy = ConstOrder.Name;
                ParametersManager.OrderType = ConstOrderType.Desc;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantEventName.ClearFilterAndOrderUpInside);
                ShowMessageException(exception);
            }
        }

        private async void ApplyChangeUpInside(object sender, EventArgs e)
        {
            try
            {
                if (isFilterAction)
                {
                    FilterProductTableViewSource sourceFilter = (filterOrderTableView.Source as FilterProductTableViewSource);
                    sourceFilter.SetParameterFilters();
                }
                else
                {
                    OrderProductTableViewSource sourceOrderBy = (filterOrderTableView.Source as OrderProductTableViewSource);
                }
                await GetProductsCategories();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantEventName.ApplyChangeUpInside);
                ShowMessageException(exception);
            }
        }

        private void FilterByUpInside(object sender, EventArgs e)
        {
            isFilterAction = true;
            FilterProductTableViewSource filterProductTableViewSource = new FilterProductTableViewSource(ParametersManager.Categories as List<ProductFilter>,
                                                                                   ParametersManager.Brands as List<ProductFilter>, filterOrderTableView, Category);
            filterProductTableViewSource.FilterAction = FilterByUpInside;
            filterProductTableViewSource.OrderAction = OrderByUpInside;
            filterOrderTableView.Source = filterProductTableViewSource;
            filterOrderTableView.EstimatedRowHeight = UITableView.AutomaticDimension;
            filterOrderTableView.ReloadData();
        }

        private void OrderByUpInside(object sender, EventArgs e)
        {
            isFilterAction = false;
            OrderProductTableViewSource orderProductTableViewSource = new OrderProductTableViewSource(category);
            orderProductTableViewSource.FilterAction = FilterByUpInside;
            orderProductTableViewSource.OrderAction = OrderByUpInside;
            filterOrderTableView.Source = orderProductTableViewSource;
            filterOrderTableView.EstimatedRowHeight = UITableView.AutomaticDimension;
            filterOrderTableView.ReloadData();
        }

        private async void RetryTouchUpInside(object sender, EventArgs e)
        {
            _spinnerActivityIndicatorView.Retry.Hidden = true;
            await this.GetProductsCategories();
        }
        #endregion
    }
}