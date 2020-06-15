using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GlobalToast;
using GrupoExito.DataAgent.Services.Products;
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
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers
{
    public partial class MyCustomListViewController : UIViewControllerBase
    {
        #region Atributtes
        private int isCustomerList { get; set; }
        private ShoppingListModel ShoppingListModel { get; set; }
        private IList<ShoppingList> shoppingLists;
        private ProductList product;
        #endregion

        #region Properties 
        public IList<ShoppingList> ShoppingLists { get => shoppingLists; set => shoppingLists = value; }
        public ProductList Product { get => product; set => product = value; }
        public int IsCustomerList { get => isCustomerList; set => isCustomerList = value; }
        #endregion

        #region Constructors
        public MyCustomListViewController(IntPtr handle) : base(handle)
        {
            ShoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.CreateNewList, nameof(MyCustomListViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {              
                this.LoadExternalViews();
                this.LoadHandlers();
                this.LoadCorners();
                this.LoadLabels(isCustomerList);
                this.ValidateListName();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.ShowCarData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.ViewWillAppear);
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
            try
            {
                listTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.NameListViewCell, NSBundle.MainBundle), ConstantIdentifier.NameListViewCellIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
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


        private void ValidateListName()
        {
            nameListTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 20;
            };
        }

        private void LoadHandlers()
        {
            createListButton.TouchUpInside += CreateListButtonTouchUpInside;
            addListButton.TouchUpInside += AddListButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void LoadCorners()
        {
            try
            {
                createListButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                addListButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private async Task CreateList()
        {
            try
            {
                StartActivityIndicatorCustom();
                ShoppingList data = new ShoppingList()
                {
                    Name = nameListTextField.Text,
                    Description = nameListTextField.Text
                };

                ShoppingListsResponse response = await ShoppingListModel.AddShoppingList(data);
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
                    StopActivityIndicatorCustom();

                    if (isCustomerList == 1)
                    {
                        ToastAppearance appearance = new ToastAppearance
                        {
                            MessageColor = UIColor.FromRGB(255, 255, 255),
                            Color = ConstantColor.OrangeColor,
                            MessageTextAlignment = UITextAlignment.Center,
                            TitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric),
                            CornerRadius = ConstantStyle.CornerRadius
                        };

                        Toast.MakeToast(string.Format(AppMessages.CreatedListMessage, nameListTextField.Text))
                            .SetPosition(ToastPosition.Center)
                            .SetLayout(new ToastLayout())
                            .SetAppearance(appearance)
                            .SetDuration(2000)
                            .Show();
                        TutorialMyListViewController tutorialMyListViewController = (TutorialMyListViewController)Storyboard.InstantiateViewController(ConstantControllersName.TutorialMyListViewController);
                        NavigationController.PushViewController(tutorialMyListViewController, true);
                        await this.GetList();
                        NewListScreenView();
                    }
                    else
                    {
                        if (Product != null)
                        {
                            ParametersManager.ShoppingListSelectedId = response.Id;
                            await AddProductsToList();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }

        private async Task GetList()
        {
            try
            {
                ShoppingListsResponse response = await ShoppingListModel.GetShoppingLists();

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
                    this.shoppingLists = response.ShpoppingLists;
                    if (this.shoppingLists.Any())
                    {
                        listTableView.Source = new ListViewSource(shoppingLists);
                        listTableView.ReloadData();
                        int tableHeight = shoppingLists.Count * ConstantViewSize.NameCustomerList;
                        Util.SetConstraint(listTableView, listTableView.Frame.Height, tableHeight);
                        addProductsLabel.Hidden = false;
                        textLabel.Hidden = false;
                        youListLabel.Hidden = false;
                        listTableView.Hidden = false;
                    }
                    else
                    {
                        addProductsLabel.Hidden = true;
                        textLabel.Hidden = true;
                        youListLabel.Hidden = true;
                        listTableView.Hidden = true;
                        addListButton.Hidden = true;
                    }

                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }


        public async Task AddProductsToList()
        {
            try
            {
                StartActivityIndicatorCustom();
                Product.ShoppingListId = ParametersManager.ShoppingListSelectedId;
                if (Product.Quantity <= 0)
                {
                    Product.Quantity = 1;
                }

                ResponseBase response = await ShoppingListModel.AddProductShoppingList(Product);

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
                    ToastAppearance appearance = new ToastAppearance
                    {
                        MessageColor = UIColor.FromRGB(255, 255, 255),
                        Color = ConstantColor.UiBackgroundToastMake,
                        MessageTextAlignment = UITextAlignment.Center,
                        TitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric),
                        CornerRadius = ConstantStyle.CornerRadius
                    };

                    Toast.MakeToast("Se ha creado la lista")
                        .SetPosition(ToastPosition.Center)
                        .SetLayout(new ToastLayout())
                        .SetAppearance(appearance)
                        .SetDuration(2000)
                        .Show();
                    this.NavigationController.PopViewController(true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyCustomListViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }

        protected async Task LoadLabels(int number)
        {
            if (number == 1)
            {
                StopActivityIndicatorCustom();
                addProductsLabel.Hidden = true;
                textLabel.Hidden = true;
                youListLabel.Hidden = true;
                listTableView.Hidden = true;
                addListButton.Hidden = true;
            }
            else
            {
                StartActivityIndicatorCustom();
                await this.GetList();
            }
        }

        private void NewListScreenView()
        {
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.NewList, nameof(MyCustomListViewController));
        }
        #endregion

        #region Events
        private void CreateListButtonTouchUpInside(object sender, EventArgs e)
        {
            if (nameListTextField.Text == "")
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.ValidateNameList, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
            else
            {
                nameListTextField.ResignFirstResponder();
                InvokeOnMainThread(async () =>
                {
                    await this.CreateList();
                });
            }
        }

        private void AddListButtonTouchUpInside(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ParametersManager.ShoppingListSelectedId))
            {
                RegisterEventAddedToWishList();

                InvokeOnMainThread(async () =>
                {
                    await this.AddProductsToList();
                });
            }
            else
            {
                //Mostrar mensaje donde se indique que debe seleccionar una lista
            }
        }

        private void RegisterEventAddedToWishList()
        {
            FacebookEventRegistrationService.Instance.AddToWishlist(Product);
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            //Retry Events in error view 
        }
        #endregion
    }
}

