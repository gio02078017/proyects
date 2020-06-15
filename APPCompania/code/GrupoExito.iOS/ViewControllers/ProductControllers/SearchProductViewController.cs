using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    public partial class SearchProductViewController : BaseProductController
    {
        #region Attributes 
        private List<Item> Suggestions;
        private SearchProductView _searchProduct;
        #endregion

        #region Constructors
        public SearchProductViewController(IntPtr handle) : base(handle){}
        #endregion

        #region Life Cycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                this.LoadExternalViews();
                this.LoadHandlers();
                base.StopActivityIndicatorCustom();
                emptySearchView.Hidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                emptySearchView.Hidden = false;
                _searchProduct.ProductSearch.Text = string.Empty;
                _searchProduct.ProductSearch.BecomeFirstResponder();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }


        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Lobby, nameof(SearchProductViewController));
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.HiddenCarData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            try
            {
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.ViewWillDisappear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            _searchProduct.searchData.TouchUpInside += SearchDataTouchUpInside;
            _searchProduct.ProductSearch.EditingChanged += ProductSearchEditingChanged;
            _searchProduct.ProductSearch.ShouldReturn = (textField) =>
            {
                SearchDataTouchUpInside(textField, null);
                textField.ResignFirstResponder();
                return true;
            };
            Xamarin.IQKeyboardManager.SharedManager.ToolbarDoneBarButtonItemText = "Cerrar";
            _searchProduct.ProductSearch.EditingDidEnd += ProductSearch_EditingDidEnd;
        }

        private void LoadExternalViews()
        {
            try
            {
                productFilteredTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.ProductFilteredViewCell, NSBundle.MainBundle), ConstantIdentifier.ProductFilteredIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                _searchProduct = searchProductView.Subviews[searchProductView.Subviews.Length - 1] as SearchProductView;
                _searchProduct.ProductSearchButton.Hidden = true;
                _searchProduct.LoadSizeControl(this);
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private async Task Search(UITextField productSearch)
        {
            StartActivityIndicatorCustom();
            UITextField productSearchTextField = ((SearchProductView)searchProductView.Subviews[searchProductView.Subviews.Length - 1]).ProductSearch;
            ProductSearcherParameters productSearcherParameters = new ProductSearcherParameters
            {
                Size = "10",
                From = "0",
                DependencyId = ParametersManager.UserContext.DependencyId,
                Prefix = productSearchTextField.Text,
            };

            var response = await ProductSearcher(productSearcherParameters);
            if (response.Result == null || !response.Result.HasErrors || response.Result.Messages == null)
            {
                Suggestions = new List<Item>();
                Suggestions.AddRange(response.NameSuggest);
                Suggestions.AddRange(response.CategorySuggest);
                Suggestions.AddRange(response.BrandSuggest);
                productFilteredTableView.Source = new SearchProductTableViewSource(this, Suggestions);
                productFilteredTableView.ReloadData();

                if (Suggestions.Any())
                {
                    emptySearchView.Hidden = true;
                }
                else
                {
                    emptySearchView.Hidden = false;
                }
            }
            else
            {
                StartActivityErrorMessage(response.Result.Messages[0].Code.ToString(), response.Result.Messages[0].Description);
            }
            StopActivityIndicatorCustom();
        }

        public async Task<ProductSearcherResponse> ProductSearcher(ProductSearcherParameters parameters)
        {
            ProductSearcherResponse response = null;
            try
            {
                response = await _productsModel.ProductSearcher(parameters);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.ProductSearcher);
                ShowMessageException(exception);
            }
            return response;
        }
        #endregion

        #region Events 
        public override UIView InputAccessoryView
        {
            get
            {
                UIView dismiss = new UIView(new RectangleF(0, 0, 320, 27))
                {
                    BackgroundColor = UIColor.GroupTableViewBackgroundColor
                };
                UIButton dismissBtn = new UIButton(new RectangleF(300, 2, 58, 23));
                dismissBtn.SetTitle("Buscar", UIControlState.Normal);
                dismissBtn.SetTitleColor(new UIColor(0.0f, 122.0f / 255f, 1.0f, 1.0f), UIControlState.Normal);
                dismissBtn.TouchDown += delegate {
                    try
                    {
                        if (!_searchProduct.ProductSearch.Text.Trim().Equals(string.Empty))
                        {
                            string textToSearch = _searchProduct.ProductSearch.Text;
                            emptySearchView.Hidden = false;
                            productFilteredTableView.Source = new SearchProductTableViewSource(this, new List<Item>());
                            _searchProduct.ProductSearch.Text = string.Empty;

                            if (Storyboard.InstantiateViewController(ConstantControllersName.ProductByCategoryViewController) is ProductByCategoryViewController ProductByCategoryViewController)
                            {
                                ParametersManager.ContainChanges = false;
                                ParametersManager.CategoriesName = null;
                                ParametersManager.BrandsName = null;
                                ParametersManager.OrderBy = ConstOrder.Name;
                                ParametersManager.OrderType = ConstOrderType.Desc;
                                ParametersManager.UserQuery = textToSearch;
                                Category category = null;
                                ProductByCategoryViewController._category = category;
                                ProductByCategoryViewController.HidesBottomBarWhenPushed = true;
                                ProductByCategoryViewController.IsSearcherProduct = true;
                                this.NavigationController.PushViewController(ProductByCategoryViewController, true);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.LogException(ex, ConstantControllersName.SearchProductViewController, ConstantMethodName.SearchDataTouchUpInside);
                    }
                };
                dismiss.AddSubview(dismissBtn);
                return dismiss;
            }
        }

        private void ProductSearch_EditingDidEnd(object sender, EventArgs e)
        {
            /*UITextField textField = (UITextField)sender;
            if (!string.IsNullOrEmpty(textField.Text))
            {
                SearchDataTouchUpInside(sender, e);
            }*/
        }

        private void ProductSearchEditingChanged(object sender, EventArgs e)
        {
            UITextField textField = (UITextField)sender;
            if (textField.Text.Length >= 3)
            {
                Search(textField);
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            Search(_searchProduct.ProductSearch);
        }

        private void SearchDataTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (!_searchProduct.ProductSearch.Text.Trim().Equals(string.Empty))
                {
                    string textToSearch = _searchProduct.ProductSearch.Text;
                    emptySearchView.Hidden = false;
                    productFilteredTableView.Source = new SearchProductTableViewSource(this, new List<Item>());
                    _searchProduct.ProductSearch.Text = string.Empty;

                    if (Storyboard.InstantiateViewController(ConstantControllersName.ProductByCategoryViewController) is ProductByCategoryViewController ProductByCategoryViewController)
                    {
                        ParametersManager.ContainChanges = false;
                        ParametersManager.CategoriesName = null;
                        ParametersManager.BrandsName = null;
                        ParametersManager.OrderBy = ConstOrder.Name;
                        ParametersManager.OrderType = ConstOrderType.Desc;
                        ParametersManager.UserQuery = textToSearch;
                        Category category = null;
                        ProductByCategoryViewController._category = category;
                        ProductByCategoryViewController.HidesBottomBarWhenPushed = true;
                        ProductByCategoryViewController.IsSearcherProduct = true;
                        this.NavigationController.PushViewController(ProductByCategoryViewController, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, ConstantControllersName.SearchProductViewController, ConstantMethodName.SearchDataTouchUpInside);
            }
        }
        #endregion
    }
}