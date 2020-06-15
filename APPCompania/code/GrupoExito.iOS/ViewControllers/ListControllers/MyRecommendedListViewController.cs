using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers.Sources;
using GrupoExito.iOS.ViewControllers.PaymentControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Newtonsoft.Json;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers
{
    public partial class MyRecommendedListViewController : BaseListController
    {
        #region Attributes
        private int viewType;
        private ShoppingList shoppingList;
        private bool _isSelectedAll = false;
        private ProductListViewSource productListDataSource;
        private bool select;
        private bool validate;
        #endregion

        #region Properties
        public ShoppingList ShoppingList { get => shoppingList; set => shoppingList = value; }
        public int ViewType { get => viewType; set => viewType = value; }
        #endregion

        #region Constructors
        public MyRecommendedListViewController(IntPtr handle) : base(handle) { }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.RecommendProducst, nameof(MyRecommendedListViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {              
                this.nameListTextField.Hidden = true;
                this.LoadExternalViews();
                this.LoadHandlers();
                this.LoadCorners();
                this.LoadData();
                this.EnableButton(select);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.ShowAccountProfile();
                NavigationView.HiddenCarData();
                if (ParametersManager.ContainChanges)
                {
                    LoadData();
                    ParametersManager.ContainChanges = false;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.ViewWillAppear);
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
            selectAllButton.TouchUpInside += SelectAllButtonTouchUpInside;
            addToCarButton.TouchUpInside += AddToCarButtonTouchUpInside;
            editButtonList.TouchUpInside += EditButtonListTouchUpInside;
            updateButton.TouchUpInside += UpdateButtonTouchUpInside;
            cancelButton.TouchUpInside += CancelButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            nameListTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 20;
            };

            nameListTextField.ShouldReturn = (textField) =>
            {
                nameListTextField.ResignFirstResponder();
                return true;
            };
        }

        private string TotalProductsText(int quantity)
        {
            return (quantity > 1 || quantity == 0) ?
                    quantity + " " + AppMessages.Products :
                    quantity + " " + AppMessages.Product;
        }

        private void LoadCorners()
        {
            try
            {
                addToCarView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                updatelistView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                updateButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                addToCarButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                enableAddButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                enableAddView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                productsInListTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.ProductListViewCell, NSBundle.MainBundle), ConstantIdentifier.ProductListIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void SetShoppingListProducts()
        {
            if (shoppingList.Products != null && shoppingList.Products.Any())
            {
                countProductsLabel.Text = TotalProductsText(shoppingList.Products.Count);
                nameListTextField.Text = shoppingList.Name;
                Util.SetConstraint(productsInListTableView, productsInListTableView.Frame.Height, (ShoppingList.Products.Count * ConstantViewSize.ProductListViewCellHeight));
                productListDataSource = new ProductListViewSource(shoppingList.Products, this, true );
                productsInListTableView.Source = productListDataSource;
                productListDataSource.SelectItemAction += DataSourceSelectItemAction;
                productListDataSource.DeleteRowEvent += MyListViewSourceDeleteRowEvent;          
                productsInListTableView.ReloadData();
                productsInListTableView.RowHeight = UITableView.AutomaticDimension;
                productsInListTableView.EstimatedRowHeight = 120;
                StopActivityIndicatorCustom();
                countProductsSelectedLabel.Text = TotalProductsText(0);
                priceProductsSelectedLabel.Text = StringFormat.ToPrice(0);
            }
            else
            {
                _spinnerActivityIndicatorView.Image.StopAnimating();
                _spinnerActivityIndicatorView.Image.Image = UIImage.FromFile(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Hidden = false;
                _spinnerActivityIndicatorView.Message.TextColor = ConstantColor.UiMessageError;
                _spinnerActivityIndicatorView.Message.Text = AppMessages.NofoundProductsonList;
            }
        }

        private void SetSuggestedList()
        {
            if (shoppingList.Products != null && shoppingList.Products.Count >= 0)
            {
                countProductsLabel.Text = TotalProductsText(shoppingList.Products.Count);
                Util.SetConstraint(productsInListTableView, productsInListTableView.Frame.Height, ((shoppingList.Products.Count + 1) * ConstantViewSize.ProductListViewCellHeight));
                productListDataSource = new ProductListViewSource(shoppingList.Products, this, false);
                productsInListTableView.Source = productListDataSource;
                productListDataSource.SelectItemAction += DataSourceSelectItemAction;
                productsInListTableView.ReloadData();
                StopActivityIndicatorCustom();
            }
            else
            {
                countProductsLabel.Text = AppMessages.ProductsNotFound;
            }
        }

        private void CalculateProductsSelected(IList<ProductList> productList)
        {
            try
            {
                //this.shoppingList = (IList<Product>)productList;
                int productsCount = 0;
                decimal productsCost = 0;
                if (productList != null && productList.Any())
                {
                    foreach (Product product in productList)
                    {
                        if (product.Selected)
                        {
                            if (product.Quantity <= 0)
                            {
                                product.Quantity = 0;
                                product.Selected = false;
                            }

                            productsCount++;

                            productsCost += (product.Price.ActualPrice * product.Quantity);
                        }
                    }
                }

                string labelProduct = string.Empty;
                countProductsSelectedLabel.Text = TotalProductsText(productsCount);
                priceProductsSelectedLabel.Text = StringFormat.ToPrice(productsCost);

                if (validate == false)
                {
                    if (countProductsSelectedLabel.Text == TotalProductsText(0))
                    {
                        select = false;
                        this.EnableButton(select);
                    }
                    else
                    {
                        select = true;
                        this.EnableButton(select);
                    }

                }
                this.shoppingList.Products = productList;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.AddProduct);
            }
        }

        private void ValidateButton(string products)
        {
            if (products == null)
            {
                addToCarButton.Enabled = false;

            }
            else
            {
                addToCarButton.Enabled = true;
            }

        }


        private void EnableButton(bool selected)
        {
            if (select == false)
            {

                addToCarButton.Enabled = false;
                addToCarView.Hidden = true;
                enableAddView.Hidden = false;

            }
            else
            {

                addToCarButton.Enabled = true;
                addToCarView.Hidden = false;
                enableAddView.Hidden = true;
            }

        }

        private ShoppingList CloneList(ShoppingList source)
        {
            if (source != null)
                return JsonConvert.DeserializeObject<ShoppingList>(JsonConvert.SerializeObject(source));
            else return null;
        }
        #endregion

        #region Methods Async 
        private async Task LoadData()
        {
            if (ViewType == 1 && shoppingList != null)
            {
                await GetProductsShoppingListAsync();
            }
            else
            {
                await this.GetRecommendProductsAsync();
            }
        }

        private async Task GetRecommendProductsAsync()
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
                    editLabel.Hidden = true;
                    editButtonList.Hidden = true;
                    masImageView.Hidden = true;

                    shoppingList = new ShoppingList
                    {
                        Products = new List<ProductList>()
                    };
                    shoppingList.Products = SuggestedProductsResponse.ProductsClient;
                    SetSuggestedList();
                }
                this.CalculateProductsSelected(shoppingList.Products);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.GetRecommendProducs);
            }
        }

        private async Task GetProductsShoppingListAsync()
        {
            try
            {
                titleLabel.Text = shoppingList.Name;
                StartActivityIndicatorCustom();
                SuggestedProductsResponse productsShoppingListResponse = await ShoppingListModel.GetProductsShoppingList(shoppingList.Id.ToString());

                if (productsShoppingListResponse.Result != null && productsShoppingListResponse.Result.HasErrors && productsShoppingListResponse.Result.Messages != null)
                {
                    if (productsShoppingListResponse.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(productsShoppingListResponse.Result);
                        StartActivityErrorMessage(productsShoppingListResponse.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    shoppingList.Products = new List<ProductList>();
                    shoppingList.Products = productsShoppingListResponse.ProductsClient;
                    SetShoppingListProducts();
                }

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.GetRecommendProducs);
            }
        }

        private async Task UpdateList(ShoppingList list)
        {
            try
            {
                StartActivityIndicatorCustom();
                ResponseBase response = await ShoppingListModel.UpdateShoppingList(list);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    titleLabel.Text = list.Name;
                    titleLabel.Hidden = false;
                    nameListTextField.Hidden = true;
                    addToCarView.Hidden = false;
                    updatelistView.Hidden = true;
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.GetRecommendProducs);
            }
        }

        private async Task UpdateQuantityItemList(ShoppingList List)
        {
            try
            {
                StartActivityIndicatorCustom();
                ResponseBase response = await ShoppingListModel.UpdateQuantityItemList(List);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    Util.LoadCenterToast(AppMessages.UpdateList).Show();
                    titleLabel.Hidden = false;
                    nameListTextField.Hidden = true;
                    addToCarView.Hidden = false;
                    updatelistView.Hidden = true;

                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.GetRecommendProducs);
            }
        }

        public async Task DeleteListAsync(ProductList product, UITableView tableView)
        {
            await DeleteList(product, tableView);
        }

        public async Task<bool> DeleteList(ProductList product, UITableView tableView)
        {
            bool result = false;
            try
            {
                ResponseBase response = await ShoppingListModel.DeleteProductShoppingList(product.ItemId, shoppingList.Id);
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
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeletedListProductCustomMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action =>
                    {
                        if (shoppingList.Products.Count == 0)
                        {
                            StartActivityIndicatorCustom();
                            this.NavigationController.PopViewController(true);
                        }
                    }));
                    PresentViewController(alertController, true, null);
                    shoppingList.Products.Remove(product);
                    Util.SetConstraint(productsInListTableView, productsInListTableView.Frame.Height, ((shoppingList.Products.Count + 1) * ConstantViewSize.ProductListViewCellHeight));
                    tableView.ReloadData();
                    this.SetShoppingListProducts();

                }
                return result;
            }
            catch (Exception)
            {
                spinnerActivityIndicatorView.StopAnimating();
                _spinnerActivityIndicatorView.Image.StopAnimating();
                customSpinnerView.Hidden = true;
                return result;
            }
        }
        #endregion

        #region Events
        private void MyListViewSourceDeleteRowEvent(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteProductList, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Cancel, action => { }));
            alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action =>
            {
                DeleteListAsync(shoppingList.Products[indexPath.Row], productsInListTableView);
            }));
            PresentViewController(alertController, true, null);
        }

        private void CancelButtonTouchUpInside(object sender, EventArgs e)
        {
            titleLabel.Hidden = false;
            nameListTextField.Hidden = true;
            addToCarView.Hidden = false;
            updatelistView.Hidden = true;
            masImageView.Hidden = false;
            editButtonList.Hidden = false;
            cancelButton.Hidden = true;
            editLabel.Hidden = false;
            enableAddView.Hidden = false;
            validate = false;

        }

        private void EditButtonListTouchUpInside(object sender, EventArgs e)
        {
            titleLabel.Hidden = true;
            nameListTextField.Hidden = false;
            addToCarView.Hidden = true;
            updatelistView.Hidden = false;
            masImageView.Hidden = true;
            editButtonList.Hidden = true;
            cancelButton.Hidden = false;
            editLabel.Hidden = true;
            enableAddView.Hidden = true;
            validate = true;
        }

        private void UpdateButtonTouchUpInside(object sender, EventArgs e)
        {
            CancelButtonTouchUpInside(sender, e);
            ShoppingList shoppingListTmp = CloneList(this.shoppingList);
            foreach (ProductList product in shoppingListTmp.Products)
            {
                product.Price = null;
            }
            InvokeOnMainThread(async () =>
            {
                await UpdateQuantityItemList(shoppingListTmp);
            });
            if (!nameListTextField.Text.Equals(this.shoppingList.Name))
            {
                this.shoppingList.Name = nameListTextField.Text;
                InvokeOnMainThread(async () =>
                {
                    await UpdateList(shoppingList);
                });
            }
        }


        private void SelectAllButtonTouchUpInside(object sender, EventArgs e)
        {
            _isSelectedAll = !_isSelectedAll;
            if (_isSelectedAll)
            {
                checkerImageView.Image = UIImage.FromFile(ConstantImages.CheckBoxSelected);
                productListDataSource.Products.ToList().ForEach(k => k.Quantity = k.Quantity == 0 ? 1 : k.Quantity);
            }
            else
            {
                checkerImageView.Image = UIImage.FromFile(ConstantImages.CheckboxUnselected);
            }
            productListDataSource.ChangeTableViewSelection(_isSelectedAll, productsInListTableView);
        }

        private void AddToCarButtonTouchUpInside(object sender, EventArgs e)
        {
            IList<ProductList> productList = productListDataSource.Products.Where(x => x.Selected).ToList();
            if (productList != null && productList.Any())
            {
                foreach (ProductList product in productList)
                {
                    DataBase.UpSertProduct(product);
                    RegisterAddProductEvent(product);
                }
            }
            UpdateCar(true);
            Util.LoadCenterToast(AppMessages.ProductAddedToCar).Show();
            //Aqui se debe mostrar un mensaje indicando que se ha agregado.
            ParametersManager.Products = null;
            SummaryContainerController summaryContainer = (SummaryContainerController)this.Storyboard.InstantiateViewController(nameof(SummaryContainerController));
            summaryContainer.HidesBottomBarWhenPushed = true;
            this.NavigationController.PushViewController(summaryContainer, true);
        }

        private void RegisterAddProductEvent(Product product)
        {
            FirebaseEventRegistrationService.Instance.AddProductToCart(product, product.CategoryName);
            FacebookEventRegistrationService.Instance.AddProductToCart(product);
        }

        private void DataSourceSelectItemAction(object sender, EventArgs e)
        {
            CalculateProductsSelected((List<ProductList>)sender);
            productsInListTableView.ReloadData();
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion
    }
}

