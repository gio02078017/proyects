using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.CategoryControllers.Cells;
using GrupoExito.iOS.ViewControllers.CategoryControllers.Sources;
using GrupoExito.iOS.ViewControllers.ProductControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.CategoryControllers
{
    public partial class CategoryViewController : UIViewControllerBase
    {
        #region Attributes
        private ProductCarModel dataBase;
        private IList<Product> DiscountProducts;
        private IList<Product> CustomerProducts;
        private CategoriesModel _categoriesModel;
        private IList<Category> Categories;
        private CategoryCollectionViewSource categoriesSource;
        #endregion

        #region Constructors 
        public CategoryViewController(IntPtr handle) : base(handle)
        {
            _categoriesModel = new CategoriesModel(new CategoriesService(DeviceManager.Instance));
            dataBase = new ProductCarModel(ProductCarDataBase.Instance);
        }
        #endregion

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {                
                this.LoadExternalViews();
                this.LoadHandlers();
                base.StopActivityIndicatorCustom();
                this.GetCategories();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public void LoadTemplateCategoryData()
        {
            Categories = new List<Category>();
            for (int i = 0; i < 12; i++)
            {
                Categories.Add(new Category() { SiteId = AppMessages.Template });
            }
            categoriesSource = new CategoryCollectionViewSource(Categories);
            categoryCollectionView.Source = categoriesSource;
            categoryCollectionView.ReloadData();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                if (Categories == null || (Categories != null && !Categories.Any()))
                {
                    this.GetCategories();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Categories, nameof(CategoryViewController));
                NavigationView.LoadControllers(true, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.IsSummaryDisabled = false;
                NavigationView.IsAccountEnabled = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            GC.Collect();
        }

        #endregion

        #region Methods

        private void UpdateDiscountProducts()
        {
            foreach (var item in DiscountProducts)
            {
                Product exists = dataBase.GetProduct(item.Id);
                if (exists == null)
                {
                    item.Quantity = 0;
                }
            }
        }

        private void UpdateCustomerProducts()
        {
            foreach (var item in CustomerProducts)
            {
                Product exists = dataBase.GetProduct(item.Id);
                if (exists == null)
                {
                    item.Quantity = 0;
                }
            }
        }
        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void GetCategories()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetCategoriesAsync();
                });
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private async Task GetCategoriesAsync()
        {
            try
            {

                LoadTemplateCategoryData();
                CategoriesResponse response = await _categoriesModel.GetCategories();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        Categories = null;
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    Categories = new List<Category>();
                    Categories = response.Categories;
                    this.DrawCategoryList();
                }
            }
            catch (Exception exception)
            {
                Categories = null;
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantMethodName.GetCategories);
                ShowMessageException(exception);
            }
        }

        private void DrawCategoryList()
        {
            try
            {
                categoriesSource = new CategoryCollectionViewSource(Categories);
                categoryCollectionView.Source = categoriesSource;
                categoriesSource.SelectAction += CategoriesSourceSelectAction;
                categoryCollectionView.ReloadData();
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantMethodName.DrawCategoryList);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                categoryCollectionView.RegisterNibForCell(CategoryViewCell.Nib, CategoryViewCell.Key);
                categoryCollectionView.RegisterNibForCell(HeaderCategoryViewCell.Nib, HeaderCategoryViewCell.Key);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void RegisterCategorySelected(string category)
        {
            //FirebaseEventRegistrationService.Instance.CategorySelected(category);
        }
        #endregion

        #region Events 
        private void CategoriesSourceSelectAction(object sender, EventArgs e)
        {
            Category category = (Category)sender;

            if (this.Storyboard.InstantiateViewController(ConstantControllersName.ProductByCategoryViewController) is ProductByCategoryViewController productByCategoryViewController)
            {
                ParametersManager.ContainChanges = false;
                ParametersManager.CategoriesName = null;
                ParametersManager.BrandsName = null;
                ParametersManager.UserQuery = null;
                ParametersManager.OrderBy = ConstOrder.Name;
                ParametersManager.OrderType = ConstOrderType.Desc;
                ParametersManager.OrderByFilter = ConstOrder.Relevance;
                ParametersManager.From_ProductsByCategory = "0";

                RegisterCategorySelected(category.Id);

                productByCategoryViewController._category = category;
                productByCategoryViewController.HidesBottomBarWhenPushed = true;
                productByCategoryViewController.ProvidesPresentationContextTransitionStyle = true;
                productByCategoryViewController.DefinesPresentationContext = true;
                productByCategoryViewController.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                this.NavigationController.PushViewController(productByCategoryViewController, true);
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                StopActivityIndicatorCustom();
                this.GetCategories();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CategoryViewController, ConstantEventName.RetryTouchUpInside);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}

