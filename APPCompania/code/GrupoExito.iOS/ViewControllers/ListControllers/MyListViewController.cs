using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GlobalToast;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
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
    public partial class MyListViewController : UIViewControllerBase
    {
        #region Attributes
        private ShoppingListModel ShoppingListModel { get; set; }
        private IList<ShoppingList> shoppingLists;
        #endregion

        #region Constructors
        public MyListViewController(IntPtr handle) : base(handle)
        {
            ShoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {                
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadHandlers();
           
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyListViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                InvokeOnMainThread(async () =>
                {
                    await this.GetList();
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyListViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ShoppingLists, nameof(MyListViewController));
                NavigationView.LoadControllers(true, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.IsSummaryDisabled = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyListViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        private void LoadFonts()
        {
            //Load font size and style
        }

        private void LoadHandlers()
        {
            addNewListButton.TouchUpInside += AddNewListButtonTouchUpInside;
            weAlsoSuggestYouButton.TouchUpInside += WeAlsoSuggestYouButtonTouchUpInside;
            newListButton.TouchUpInside += NewListButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

     


        private void LoadCorners()
        {
            try
            {
                weAlsoSuggestYouView.BackgroundColor = ConstantColor.UiPrimary;
                weAlsoSuggestYouView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyListViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }
        private void LoadExternalViews()
        {
            try
            {
                myCustomListTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.MyCustomListViewCell, NSBundle.MainBundle), ConstantIdentifier.MyListIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadSearchProductsView(searchProductView);
                this.NavigationController.NavigationBarHidden = false;
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyListViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            StartActivityIndicatorCustom();
            if (this.shoppingLists.Any())
            {
                myCustomListTableHeightConstraint.Constant = this.shoppingLists.Count * ConstantViewSize.MyListViewCellHeight;
                MyListViewSource myListViewSource = new MyListViewSource(this.shoppingLists);
                myCustomListTableView.Source = myListViewSource;
                myListViewSource.SelectedRowEvent += MyListViewSourceSelectedRowEvent;
                myListViewSource.DeleteRowEvent += MyListViewSourceDeleteRowEvent;
                myListViewSource.EditRoWEvent +=  MyListViewSourceEditRoWEvent ;
                myCustomListTableView.ReloadData();
            }
            else
            {
                myCustomListTableHeightConstraint.Constant = 0;
            }
            StopActivityIndicatorCustom();
        }

         private void MyListViewSourceEditRoWEvent(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            UITextField field = null;
            var okCancelAlertController = UIAlertController.Create(AppMessages.ApplicationName,AppMessages.EditNameList, UIAlertControllerStyle.Alert);
            okCancelAlertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => {
                if (field.Text != "")
                {
                    shoppingLists[indexPath.Row].Name = field.Text;
                    this.UpdateList(shoppingLists[indexPath.Row]);
                    myCustomListTableView.ReloadData();
                }
                else 
                {
                    ToastAppearance appearance = new ToastAppearance
                    {
                        MessageColor = UIColor.FromRGB(255, 255, 255),
                        Color = ConstantColor.OrangeColor,
                        MessageTextAlignment = UITextAlignment.Center,
                        TitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric),
                        CornerRadius = ConstantStyle.CornerRadius
                    };

                    Toast.MakeToast(AppMessages.ValidateNameList)
                        .SetPosition(ToastPosition.Center)
                        .SetLayout(new ToastLayout())
                        .SetAppearance(appearance)
                        .SetDuration(2000)
                       .Show();
                }

            }));
            okCancelAlertController.AddAction(UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel was clicked")));
           
            okCancelAlertController.AddTextField((textField) => {
                field = textField;
                field.Text = shoppingLists[indexPath.Row].Name;
                myCustomListTableView.ReloadData();
            });

            field.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 20;
            };
            PresentViewController(okCancelAlertController, true, null);

        }


        private void MyListViewSourceDeleteRowEvent(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            MessageConfirmView messageConfirmView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageConfirmView, Self, null).GetItem<MessageConfirmView>(0);
            CGRect messageConfirmViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
            messageConfirmView_.Frame = messageConfirmViewFrame;
            customSpinnerView.AddSubview(messageConfirmView_);
            _spinnerActivityIndicatorView.Hidden = true;
            spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.3f);
            spinnerActivityIndicatorView.StartAnimating();
            customSpinnerView.Hidden = false;
            customSpinnerView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            messageConfirmView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
            messageConfirmView_.Message.Text = AppMessages.DeleteListCustomMessage; //Se debe cmabiar por texto listas
            Util.SetConstraint(customSpinnerView, ConstantViewSize.customSpinnerViewHeightDefault, ConstantViewSize.messageCustomViewHeightDefault);
            messageConfirmView_.Negation.TouchUpInside += (object sender2, EventArgs e2) =>
            {
                spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                spinnerActivityIndicatorView.StopAnimating();
                messageConfirmView_.RemoveFromSuperview();
                customSpinnerView.BackgroundColor = UIColor.Clear;
                customSpinnerView.Hidden = true;
                _spinnerActivityIndicatorView.Hidden = false;
                Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
            };

            messageConfirmView_.Afirmation.TouchUpInside += async (object sender2, EventArgs e2) =>
            {
                spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                spinnerActivityIndicatorView.StopAnimating();
                messageConfirmView_.RemoveFromSuperview();
                customSpinnerView.BackgroundColor = UIColor.Clear;
                _spinnerActivityIndicatorView.Hidden = false;
                await DeleteListAsync(shoppingLists[indexPath.Row], myCustomListTableView);
                Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
            };
        }

        private async Task UpdateList(ShoppingList shoppingList)
        {
            try
            {
                StartActivityIndicatorCustom();
                ResponseBase response = await ShoppingListModel.UpdateShoppingList(shoppingList);
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
                   await this.GetList();
                
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyRecommendedListViewController, ConstantMethodName.GetRecommendProducs);
            }
        }


        public async Task DeleteListAsync(ShoppingList shoppingList, UITableView tableView)
        {
            await DeleteList(shoppingList, tableView);
        }

        public async Task<bool> DeleteList(ShoppingList shoppingList, UITableView tableView)
        {
            bool result = false;
            try
            {
                spinnerActivityIndicatorView.StartAnimating();
                _spinnerActivityIndicatorView.Image.StartAnimating();
                _spinnerActivityIndicatorView.Message.Text = string.Empty;
                customSpinnerView.Hidden = false;
                var response = await ShoppingListModel.DeleteShoppingList(shoppingList.Id);
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
                    spinnerActivityIndicatorView.StopAnimating();
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    customSpinnerView.Hidden = true;
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeletedListCustomMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                    shoppingLists.Remove(shoppingList);
                    myCustomListTableHeightConstraint.Constant = (ConstantViewSize.MyListViewCellHeight * (shoppingLists.Count));
                   await this.GetList();
                    if (shoppingList.QuantityProducts == null) {
                        myCustomListTableView.Hidden = true;
                        CreateListLabel.Hidden = false;
                        addListImageView.Hidden = false;
                        newListButton.Hidden = false;
                    }else {
                        tableView.ReloadData();
                    }


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

        private async Task GetList()
        {
            try
            {
                StartActivityIndicatorCustom();
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

                    if (shoppingLists != null && shoppingLists.Count != 0) 
                    {
                        this.LoadData();
                        myCustomListTableView.Hidden = false;
                        CreateListLabel.Hidden = true;
                        addListImageView.Hidden = true;
                        newListButton.Hidden = true;
                        addNewListView.Hidden = false;
                    }
                    else
                    {
                        addNewListView.Hidden = true;
                        myCustomListTableView.Hidden = true;
                        CreateListLabel.Hidden = false;
                        addListImageView.Hidden = false;
                        newListButton.Hidden = false;
                    }
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyListViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }
        #endregion

        #region Events

        private void NewListButtonTouchUpInside(object sender, EventArgs e)
        {
            MyCustomListViewController myCustomListViewController = (MyCustomListViewController)Storyboard.InstantiateViewController(ConstantControllersName.MyCustomListViewController);
            myCustomListViewController.HidesBottomBarWhenPushed = true;
            myCustomListViewController.IsCustomerList = 1;
            NavigationController.PushViewController(myCustomListViewController, true);
        }

        private void MyListViewSourceSelectedRowEvent(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            ShoppingList shoppingList = this.shoppingLists[indexPath.Row];
            if (int.Parse(shoppingList.QuantityProducts) > 0)
            {
                MyRecommendedListViewController myRecommendedListViewController = (MyRecommendedListViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyRecommendedListViewController);
                myRecommendedListViewController.ShoppingList = shoppingList;
                myRecommendedListViewController.ViewType = 1;
                myRecommendedListViewController.HidesBottomBarWhenPushed = true;
                this.NavigationController.PushViewController(myRecommendedListViewController, true);
            }
            else
            {
                TutorialMyListViewController tutorialMyListViewController = (TutorialMyListViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.TutorialMyListViewController);
                tutorialMyListViewController.HidesBottomBarWhenPushed = true;
                tutorialMyListViewController.TypeView = 1;
                this.NavigationController.PushViewController(tutorialMyListViewController, true);
            }
        }

        private void AddNewListButtonTouchUpInside(object sender, EventArgs e)
        {
            MyCustomListViewController myCustomListViewController = (MyCustomListViewController) Storyboard.InstantiateViewController(ConstantControllersName.MyCustomListViewController);
            myCustomListViewController.HidesBottomBarWhenPushed = true;
            myCustomListViewController.IsCustomerList = 1;
            NavigationController.PushViewController(myCustomListViewController, true);
        }

        private void WeAlsoSuggestYouButtonTouchUpInside(object sender, EventArgs e)
        {
            MyRecommendedListViewController myRecommendedListViewController = (MyRecommendedListViewController)Storyboard.InstantiateViewController(ConstantControllersName.MyRecommendedListViewController);
            myRecommendedListViewController.HidesBottomBarWhenPushed = true;
            NavigationController.PushViewController(myRecommendedListViewController, true);
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            //retry event for error in this view
        }

        #endregion
    }
}

