using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimatedButtons;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Sources;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers
{
    public partial class ProductByCategoryViewController : BaseProductController
    {
        #region Attributes
        private bool isSearcherProduct = false;
        private List<Product> Products { get; set; }
        private Category categoryLoad;
        private bool IsLoadingProducts = false;
        private bool HaventThereAreMoreProducts = false;
        private ProductsCollectionViewSource productsCollectionSource;
        private ProductCarModel dataBase;
        #endregion

        #region properties
        public Category _category { get => categoryLoad; set => categoryLoad = value; }
        public bool IsSearcherProduct { get => isSearcherProduct; set => isSearcherProduct = value; }
        #endregion

        #region constructors
        public ProductByCategoryViewController(IntPtr handle) : base(handle){ }
        #endregion

        #region lifeCycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                dataBase = new ProductCarModel(ProductCarDataBase.Instance);
                this.IsLoadingProducts = false;
                this.LoadExternalViews();
                this.LoadHandlers();
                this.LoadTitleCategory();
                base.StopActivityIndicatorCustom();
                this.productActivityIndicatorView.StopAnimating();
                this.GetProductsCategories();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                if (ParametersManager.ChangeAddress)
                {
                    base.GetProductsPrice();
                    ParametersManager.ChangeAddress = false;
                }
                if (ParametersManager.ContainChanges)
                {
                    this.GetProductsCategories();
                    ParametersManager.ContainChanges = false;
                }
                if (Products != null)
                {
                    UpdateProducts();
                    productByCategoryCollectionView.ReloadData();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ProdutsxCategory, nameof(ProductByCategoryViewController));
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.IsSummaryDisabled = false;
                NavigationView.IsAccountEnabled = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            try
            {
                spinnerActivityIndicatorView.StopAnimating();
                _spinnerActivityIndicatorView.Image.StopAnimating();
                ParametersManager.Products = Products;
                ParametersManager.IsAddProducts = false;
                ParametersManager.UserQuery = null;
                ParametersManager.From_ProductsByCategory = "0";
                ParametersManager.Size_ProductsByCategory = ParametersManager.Count_ProductsByCategory.ToString();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.ViewWillDisappear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            GC.Collect();
        }
        #endregion

        #region methods

        private void UpdateProducts()
        {
            foreach (var item in Products)
            {
                if (item.Id != null)
                {
                    Product exists = dataBase.GetProduct(item.Id);
                    if (exists == null)
                    {
                        item.Quantity = 0;
                    }
                }
            }
        }

        private void GetProductsCategories()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetProductsCategoriesAsync();
                });
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void LoadTitleCategory()
        {
            string categoryName = string.Empty;
            if (this.categoryLoad != null)
            {
                categoryName = this.categoryLoad.Name;
                categoryIconImageView.SetImage(new NSUrl(this.categoryLoad.IconCategoryGray), UIImage.FromFile(ConstantImages.SinImagen));
            }
            else if (ParametersManager.UserQuery != null && ParametersManager.UserQuery != string.Empty)
            {
                categoryName = ParametersManager.UserQuery;
                categoryIconImageView.Image = UIImage.FromFile(ConstantImages.Buscar);
            }
            categoryNameLabel.Text = categoryName;
        }

        private void LoadTemplateShimmer()
        {
            this.Products = new List<Product>();

            for (int i = 0; i < 10; i++)
            {
                this.Products.Add(new Product() { SiteId = "Template" });
            }
            productsCollectionSource = new ProductsCollectionViewSource(this.Products, ConstantIdentifier.Products_by_category, this, IsSearcherProduct);
            productByCategoryCollectionView.Source = productsCollectionSource;
            productByCategoryCollectionView.ReloadData();
        }

        private async Task GetProductsCategoriesAsync()
        {
            try
            {
                if (!IsLoadingProducts)
                {
                    LoadTemplateShimmer();
                }
                else
                {
                    categoryIconImageView.Image = null;
                    UIImage[] images = Util.LoadAnimationImage(ConstantImages.FolderSpinnerLoad, ConstantViewSize.FolderSpinnerLoadCount);
                    categoryIconImageView.Image = images[0];
                    categoryIconImageView.AnimationImages = images;
                    categoryIconImageView.AnimationDuration = ConstantDuration.AnimationImageLoading;
                    categoryIconImageView.StartAnimating();
                }
                SearchProductsParameters parameters = new SearchProductsParameters()
                {
                    DependencyId = ValidateDependecy(),
                    CategoryId = categoryLoad != null ? categoryLoad.Id : string.Empty,
                    CategoriesNames = ParametersManager.CategoriesName,
                    Brands = ParametersManager.BrandsName,
                    Size = ParametersManager.Size_ProductsByCategory,
                    From = ParametersManager.From_ProductsByCategory,
                    OrderBy = ParametersManager.OrderByFilter,
                    OrderType = ParametersManager.OrderType,
                    UserQuery = ParametersManager.UserQuery
                };
                var response = await _productsModel.GetProducts(parameters);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    ParametersManager.IsAddProducts = false;
                    if (IsLoadingProducts)
                    {
                        categoryIconImageView.StopAnimating();
                        categoryIconImageView.Image = null;
                        if (this.categoryLoad != null)
                        {
                            categoryIconImageView.SetImage(new NSUrl(this.categoryLoad.IconCategoryGray), UIImage.FromFile(ConstantImages.SinImagen));
                        }
                        else if (ParametersManager.UserQuery != null && ParametersManager.UserQuery != string.Empty)
                        {
                            categoryIconImageView.Image = UIImage.FromFile(ConstantImages.Buscar);
                        }
                    }
                    IsLoadingProducts = false;
                    if (ParametersManager.From_ProductsByCategory.Equals("0"))
                    {
                        if (response.Result.Messages.Any())
                        {
                            string message = MessagesHelper.GetMessage(response.Result);
                            StartActivityIndicatorCustom();
                            StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                            SearchNotExitsScreenView();
                        }
                    }
                    else
                    {
                        HaventThereAreMoreProducts = true;
                    }
                }
                else
                {
                    if (IsSearcherProduct && !string.IsNullOrEmpty(ParametersManager.UserQuery))
                    {
                        RegisterEventSearch(ParametersManager.UserQuery, true, response.Products);
                    }

                    ShowProducts(response);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.GetProductsCategories);
            }
        }

        private void SearchNotExitsScreenView()
        {
            Utilities.Analytic.FirebaseEventRegistrationService.Instance.RegisterScreen(Entities.Constants.Analytic.AnalyticsScreenView.SearchNotExits, nameof(ProductByCategoryViewController));
        }

        private void RegisterEventSearch(string query, bool success, IList<Product> products)
        {
            FacebookEventRegistrationService.Instance.Searched(query, success, products);
        }

        private void ShowProducts(ProductsResponse response)
        {
            HaventThereAreMoreProducts = false;
            if (!ParametersManager.IsAddProducts)
            {
                Products = new List<Product>();
            }
            else
            {
                ParametersManager.IsAddProducts = false;
            }
            Products.AddRange(response.Products);
            ParametersManager.Categories = new List<ProductFilter>();
            ParametersManager.Brands = new List<ProductFilter>();
            ParametersManager.Categories = response.Categories;
            ParametersManager.Brands = response.Brands;
            DrawProducts();
        }

        private void DrawProducts()
        {
            try
            {
                if (this.Products != null && this.Products.Any())
                {
                    productCountLabel.Text = AppMessages.FoundThisProducts;
                    if (productsCollectionSource == null)
                    {
                        productsCollectionSource = new ProductsCollectionViewSource(Products, ConstantIdentifier.Products_by_category, this, IsSearcherProduct);
                        productByCategoryCollectionView.Source = productsCollectionSource;
                    }
                    else
                    {
                        productsCollectionSource.Products = this.Products;
                        RegisterProductImpressionEvent();
                    }
                    productsCollectionSource.ActionScrolled += ProductsScrollViewScrolled;
                    productsCollectionSource.ActionAddingRemoveProducts += ProductsCollectionSourceActionAddingRemoveProducts;
                    productsCollectionSource.ActionSummaryProducts += SummaryActionProducts;
                    productByCategoryCollectionView.ReloadData();
                    if (IsLoadingProducts)
                    {
                        categoryIconImageView.StopAnimating();
                        categoryIconImageView.Image = null;
                        if (this.categoryLoad != null)
                        {
                            categoryIconImageView.SetImage(new NSUrl(this.categoryLoad.IconCategoryGray), UIImage.FromFile(ConstantImages.SinImagen));
                        }
                        else if (ParametersManager.UserQuery != null && ParametersManager.UserQuery != string.Empty)
                        {
                            categoryIconImageView.Image = UIImage.FromFile(ConstantImages.Buscar);
                        }
                    }
                    IsLoadingProducts = false;
                }
                else
                {
                    StartActivityIndicatorCustom();
                    if (categoryLoad != null)
                    {
                        StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.NotFoundProductInSelectedCategory);
                    }
                    else
                    {
                        StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.NotFoundProductInSearch);
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.DrawProducts);
                ShowMessageException(exception);
            }
        }

        private void RegisterProductImpressionEvent()
        {
            string name = categoryLoad != null ? categoryLoad.Name : string.Empty;
            FirebaseEventRegistrationService.Instance.ProductImpression(this.Products, name);
        }

        private void LoadFonts()
        {
            try
            {
                if (!IsSearcherProduct)
                {
                    categoryNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.CategoryTitle);
                }
                else
                {
                    categoryNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.CategorySearcher);
                }
                productCountLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.ProductByCategoryCountFound);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                productByCategoryCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.ProductViewCell, NSBundle.MainBundle), ConstantIdentifier.ProductsIdentifier);
                //LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                if (IsSearcherProduct)
                {
                    HiddenSearchProduct(searchProductView);
                }
                LiquidFloatingCell FilterliquidFloatingCell = new LiquidFloatingCell(UIImage.FromBundle(ConstantImages.Filtro))
                {
                    TintColor = ConstantColor.UiLaterIncomeTitleButtonsSelected
                };
                LiquidFloatingCell orderByLiquidFloatingCell = new LiquidFloatingCell(UIImage.FromBundle(ConstantImages.Order))
                {
                    TintColor = ConstantColor.UiOrderFilterButton
                };
                List<LiquidFloatingCell> cells = new List<LiquidFloatingCell>
                {
                    orderByLiquidFloatingCell,
                    FilterliquidFloatingCell,
                };
                FilterLiquidFloatingView.Color = ConstantColor.UiBackgroundFloatingFilter;
                FilterLiquidFloatingView.ContentMode = UIViewContentMode.ScaleAspectFit;
                FilterLiquidFloatingView.Cells = cells;
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            FilterLiquidFloatingView.CellSelected += FilterLiquidFloatingView_CellSelected;
            productFilterHeaderButton.TouchUpInside += ProductFilterHeaderButtonTouchUpInside;
        }

        private void FilterLiquidFloatingView_CellSelected(object sender, CellSelectedEventArgs e)
        {
            switch (e.Index)
            {
                case 1:
                    ShowFilterUpInside(sender, e);
                    break;
                case 0:
                    ShowOrderUpInside(sender, e);
                    break;
            }
            FilterLiquidFloatingView.Close();
        }
        #endregion

        #region Events
        private void ProductsScrollViewScrolled(object sender, EventArgs e)
        {
            try
            {
                var scrollView = sender as UICollectionView;
                nfloat scrollViewHeight = scrollView.Frame.Size.Height;
                nfloat scrollContentSizeHeight = scrollView.ContentSize.Height - (ConstantViewSize.ProductHeightCell);
                nfloat scrollOffset = scrollView.ContentOffset.Y;
                if (scrollOffset + scrollViewHeight >= scrollContentSizeHeight - (ConstantViewSize.ProductHeightCell * 5))
                {
                    if (!IsLoadingProducts && !HaventThereAreMoreProducts)
                    {
                        ParametersManager.From_ProductsByCategory = (int.Parse(ParametersManager.From_ProductsByCategory) + ParametersManager.Count_ProductsByCategory).ToString();
                        ParametersManager.IsAddProducts = true;
                        IsLoadingProducts = true;
                        GetProductsCategories();
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantEventName.SelectedPageItem);
                ShowMessageException(exception);
            }
        }

        private void ProductFilterHeaderButtonTouchUpInside(object sender, EventArgs e)
        {
            InvokeOnMainThread(() => { this.NavigationController.PopViewController(true); });
        }

        private void ShowFilterUpInside(object sender, EventArgs e)
        {
            try
            {
                ProductFilterAndOrderByViewController productFilterAndOrderByViewController = (ProductFilterAndOrderByViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ProductFilterAndOrderByViewController);
                productFilterAndOrderByViewController.Category = categoryLoad;
                productFilterAndOrderByViewController.IsFilterAction = false;
                productFilterAndOrderByViewController.HidesBottomBarWhenPushed = true;
                productFilterAndOrderByViewController.IsSearcherProduct = IsSearcherProduct;
                this.NavigationController.PushViewController(productFilterAndOrderByViewController, true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantEventName.ShowFilterUpInside);
                ShowMessageException(exception);
            }
        }

        private void ShowOrderUpInside(object sender, EventArgs e)
        {
            try
            {
                ProductFilterAndOrderByViewController productFilterAndOrderByViewController = (ProductFilterAndOrderByViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ProductFilterAndOrderByViewController);
                productFilterAndOrderByViewController.Category = categoryLoad;
                productFilterAndOrderByViewController.IsFilterAction = true;
                productFilterAndOrderByViewController.HidesBottomBarWhenPushed = true;
                productFilterAndOrderByViewController.IsSearcherProduct = IsSearcherProduct;
                this.NavigationController.PushViewController(productFilterAndOrderByViewController, true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantEventName.ShowOrderUpInside);
                ShowMessageException(exception);
            }
        }

        private void ProductsCollectionSourceActionAddingRemoveProducts(object sender, EventArgs e)
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                productByCategoryCollectionView.ReloadData();
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void SummaryActionProducts(object sender, EventArgs e)
        {
            UpdateCar(true);
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (_spinnerActivityIndicatorView.Retry.Title(UIControlState.Normal).Equals(AppMessages.SearchOtherProduct))
                {
                    this.NavigationController.PopViewController(true);
                }
                else
                {
                    StopActivityIndicatorCustom();
                    this.GetProductsCategories();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantEventName.RetryTouchUpInside);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}
