using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Microsoft.AppCenter.Crashes;
using UIKit;

namespace GrupoExito.iOS.Referentials
{
    public partial class UIViewControllerBase : UIViewController
    {
        #region Attributes Protected
        protected OrderModel _orderModel;

        protected NavigationHeaderView navigationView;
        protected UIActivityIndicatorView SpinnerActivityIndicatorViewFromBase;
        protected CustomActivityIndicatorView _spinnerActivityIndicatorView;
        protected UIView CustomSpinnerViewFromBase;
        protected NetworkStatus RemoteHostStatus, InternetStatus, LocalWifiStatus;
        protected ProductCarModel DataBase;
        protected MyAccountModel _myAccountModel { get; set; }
        #endregion

        #region Attributes privates
        protected NavigationHeaderView NavigationView { get => navigationView; set => navigationView = value; }
        #endregion

        #region Constructor
        public UIViewControllerBase(IntPtr handle) : base(handle)
        {
            DataBase = new ProductCarModel(ProductCarDataBase.Instance);
            _orderModel = new OrderModel(new OrderService(DeviceManager.Instance));
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
        }
        #endregion

        #region lifeCycle
        public override void ViewDidLoad()
        {
            try
            {
                Xamarin.IQKeyboardManager.SharedManager.EnableAutoToolbar = true;
                Xamarin.IQKeyboardManager.SharedManager.ShouldResignOnTouchOutside = true;
                Xamarin.IQKeyboardManager.SharedManager.ShouldToolbarUsesTextFieldTintColor = true;
                Xamarin.IQKeyboardManager.SharedManager.KeyboardDistanceFromTextField = 150f;
                Xamarin.IQKeyboardManager.SharedManager.KeyboardAppearance = UIKeyboardAppearance.Light;
                Xamarin.IQKeyboardManager.SharedManager.ToolbarDoneBarButtonItemText = AppMessages.AddingSuccess;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Methods
        protected string ValidateDependecy()
        {
            string dependency = null;
            UserContext _userContext = ParametersManager.UserContext;
            if (_userContext != null)
            {
                if (_userContext.Address != null)
                {
                    if (_userContext.Address.DependencyId != null)
                    {
                        dependency = _userContext.Address.DependencyId;
                    }
                }
                else if (_userContext.Store != null)
                {
                    if (!string.IsNullOrEmpty(_userContext.Store.Id))
                    {
                        dependency = _userContext.Store.Id.ToString();
                    }
                }
            }
            return dependency;
        }

        protected void ShowAndRegisterMessageExceptions(Exception exception, Dictionary<string, string> properties)
        {
            StackTrace st = new StackTrace(exception, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            properties.Add(ConstantControllersName.LineError, line.ToString());
            Crashes.TrackError(exception, properties);
            InvokeOnMainThread(() =>
            {
                var defaultAlert = UIAlertController.Create(AppMessages.ApplicationName, exception.Message, UIAlertControllerStyle.Alert);
                defaultAlert.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                PresentViewController(defaultAlert, true, null);
            });
        }

        protected void LoadFooterView(UIView footerView)
        {
            footerView.LayoutIfNeeded();
            FooterView footerView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.FooterView, Self, null).GetItem<FooterView>(0);
            CGRect footerFrame = new CGRect(0, 0, footerView.Superview.Frame.Size.Width, footerView.Frame.Size.Height);
            footerView_.Frame = footerFrame;
            footerView.AddSubview(footerView_);
        }

        protected void LoadNavigationView(UINavigationBar navigationBar)
        {
            NavigationView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.NavigationHeaderView, Self, null).GetItem<NavigationHeaderView>(0);
            CGRect navigationViewFrame = new CGRect(0, 0, this.NavigationController.NavigationBar.Frame.Size.Width, this.NavigationController.NavigationBar.Frame.Size.Height);
            NavigationView.LoadControllers(true, false, true, this);
            this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
            NavigationView.Frame = navigationViewFrame;
            navigationBar.AddSubview(NavigationView);
            NavigationView.LoadWidthSuperView();
            UpdateCar();
        }

        public void UpdateCar(bool recalculate = false)
        {
            Dictionary<string, object> summary = recalculate ? DataBase.RecalculateSummary() : DataBase.GetSummary() ;
            int quantity = 0;
            decimal price = 0;
            if (summary != null)
            {
                quantity = int.Parse(summary["productQuantity"].ToString());
                price = decimal.Parse(summary["totalPrice"].ToString());
            }
            NavigationView.UpdateCar(StringFormat.ToPrice(price), StringFormat.ToQuantity(quantity));
        }

        protected void LoadSearchProductsView(UIView searchProductView)
        {
            searchProductView.LayoutIfNeeded();
            SearchProductView searchProduct_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.SearchProductView, Self, null).GetItem<SearchProductView>(0);
            CGRect searchProductFrame = new CGRect(0, 0, searchProductView.Superview.Frame.Size.Width, searchProductView.Frame.Size.Height);
            searchProduct_.Frame = searchProductFrame;
            searchProduct_.LoadSizeControl(this);
            searchProductView.AddSubview(searchProduct_);
        }

        protected void LoadCustomSpinnerView(UIView customSpinnerView, ref CustomActivityIndicatorView spinnerActivityIndicatorView_)
        {
            customSpinnerView.LayoutIfNeeded();
            customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            spinnerActivityIndicatorView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.CustomActivityIndicatorView, Self, null).GetItem<CustomActivityIndicatorView>(0);
            spinnerActivityIndicatorView_.LoadAnimationImages(ConstantImages.FolderSpinnerLoad, ConstantViewSize.FolderSpinnerLoadCount, ConstantDuration.AnimationImageLoading);
            CGRect spinnerActivityIndicatorViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
            spinnerActivityIndicatorView_.Frame = spinnerActivityIndicatorViewFrame;
            customSpinnerView.AddSubview(spinnerActivityIndicatorView_);
            spinnerActivityIndicatorView_.Image.Superview.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        protected void ShowMessageException(Exception exception)
        {
            InvokeOnMainThread(() =>
            {
                String message = exception.Message;
                if (!string.IsNullOrEmpty(message))
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, action => { }));
                    PresentViewController(alertController, true, null);
                }
            });
        }

        protected void AlertViewController(string applicationName, string message, string acceptButtonText)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, action => { }));
                PresentViewController(alertController, true, null);
            }
        }

        protected void ConfirmationViewController(string applicationName, string message, UIAlertAction acceptAction, UIAlertAction declineAction)
        {
            if (!string.IsNullOrEmpty(message))
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
                //alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, action => { }));
                alertController.AddAction(acceptAction);
                alertController.AddAction(declineAction);
                PresentViewController(alertController, true, null);
            }
        }

        protected void StartActivityErrorMessage(string code, string message)
        {
            StartActivityIndicatorCustom();
            CustomSpinnerViewFromBase.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            SpinnerActivityIndicatorViewFromBase.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            _spinnerActivityIndicatorView.Image.StopAnimating();
            _spinnerActivityIndicatorView.ProductAddingV.Hidden = true;
            _spinnerActivityIndicatorView.ProductAdding.Hidden = true;
            _spinnerActivityIndicatorView.Message.Hidden = false;
            _spinnerActivityIndicatorView.Message.TextColor = ConstantColor.UiMessageError;
            _spinnerActivityIndicatorView.Retry.Hidden = false;
            _spinnerActivityIndicatorView.CodeMesage = code;
            if (code.Equals(EnumErrorCode.InternetErrorMessage.ToString()) || code.Equals(EnumErrorCode.InvalidExternalResponse))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinConexion);
                _spinnerActivityIndicatorView.Message.Text = message;
            }
            else if (code.Equals(EnumErrorCode.UnexpectedErrorMessage.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = message;
            }
            else if (code.Equals(EnumErrorCode.NoDataFound.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = message;
            }
            else if (code.Equals(EnumErrorCode.OrderTrackingNotFound.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = message;
            }
            else if (code.Equals(EnumErrorCode.CouponsNotFound.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = AppMessages.NoDiscountsMessage;
                _spinnerActivityIndicatorView.Retry.SetTitle(AppMessages.DiscountsAction, UIControlState.Normal);
            }
            else if (code.Equals(EnumErrorCode.NotIsSuccessStatusCode.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = "Ups! Algo salió mal, por favor intenta más tarde";
            }
            else if (code.Equals(EnumErrorCode.ErrorSincoCheckPriceNotFound.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = message;
                _spinnerActivityIndicatorView.Retry.SetTitle(AppMessages.SearchOtherProduct, UIControlState.Normal);
            }
            else if (code.Equals(EnumErrorCode.SearchNotProductsFound.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinInformacion);
                _spinnerActivityIndicatorView.Message.Text = message;
                _spinnerActivityIndicatorView.Retry.SetTitle(AppMessages.SearchOtherProduct, UIControlState.Normal);
            }
            else if (code.Equals(EnumErrorCode.ErrorServiceUnavailable.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinConexion);
                _spinnerActivityIndicatorView.Message.Text = message;
                _spinnerActivityIndicatorView.Retry.SetTitle(AppMessages.RetryText, UIControlState.Normal);
            }
            else if (code.Equals(EnumErrorCode.UnknownError.ToString()))
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.Error);
                _spinnerActivityIndicatorView.Message.Text = message;
                _spinnerActivityIndicatorView.Retry.SetTitle(AppMessages.AcceptButtonText, UIControlState.Normal);
            }
            else
            {
                _spinnerActivityIndicatorView.LoadImage(ConstantImages.SinConexion);
                _spinnerActivityIndicatorView.Message.Text = message;
                _spinnerActivityIndicatorView.Retry.SetTitle(AppMessages.RetryText, UIControlState.Normal);
            }
        }

        protected void StartActivityIndicatorCustom()
        {
            try
            {
                SpinnerActivityIndicatorViewFromBase.StartAnimating();
                _spinnerActivityIndicatorView.Image.StartAnimating();
                _spinnerActivityIndicatorView.Retry.Hidden = true;
                _spinnerActivityIndicatorView.Message.Text = string.Empty;
                CustomSpinnerViewFromBase.Hidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.StartActivityIndicatorCustom);
            }
        }

        protected void StopActivityIndicatorCustom()
        {
            try
            {
                SpinnerActivityIndicatorViewFromBase.StopAnimating();
                SpinnerActivityIndicatorViewFromBase.HidesWhenStopped = true;
                SpinnerActivityIndicatorViewFromBase.Hidden = true;
                CustomSpinnerViewFromBase.Hidden = true;
                _spinnerActivityIndicatorView.Image.StopAnimating();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.StopActivityIndicatorCustom);
            }
        }

        protected void PresentWelcomeView()
        {
            try
            {
                if (this.Storyboard.InstantiateViewController(ConstantControllersName.WellcomeViewController) is WellcomeViewController welcomeViewController)
                {
                    welcomeViewController.ProvidesPresentationContextTransitionStyle = true;
                    welcomeViewController.DefinesPresentationContext = true;

                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        welcomeViewController.ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen;
                    }

                    PresentModalViewController(welcomeViewController, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentWelcomeView);
            }
        }

        protected void PresentLoginView()
        {
            try
            {
                if (this.Storyboard.InstantiateViewController(ConstantControllersName.LoginViewNavigationController) is UINavigationController loginViewNavigationController_)
                {
                    loginViewNavigationController_.ProvidesPresentationContextTransitionStyle = true;
                    loginViewNavigationController_.DefinesPresentationContext = true;
                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        loginViewNavigationController_.ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen;
                    }
                    PresentModalViewController(loginViewNavigationController_, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentLoginView);
            }
        }

        protected void PresentLaterIncomeView()
        {
            try
            {
                if (this.Storyboard.InstantiateViewController(ConstantControllersName.NavigationLaterIncomeController) is UINavigationController laterIncomeViewNavigationController_)
                {
                    laterIncomeViewNavigationController_.ProvidesPresentationContextTransitionStyle = true;
                    laterIncomeViewNavigationController_.DefinesPresentationContext = true;

                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        laterIncomeViewNavigationController_.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                    }

                    PresentModalViewController(laterIncomeViewNavigationController_, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentLaterIncome);
            }
        }

        protected void SetUser(User current)
        {
            if (current != null)
            {
                UserContext userContext = ModelHelper.UpdateUserContext(ParametersManager.UserContext, current);
                userContext.Address = ParametersManager.UserContext.Address;
                userContext.Store = ParametersManager.UserContext.Store;
                userContext.UserActivate = current.UserActivate;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
            }
        }

        public void PresentHomeView()
        {
            try
            {
                UITabBarController tabBarHomeController_ = (UITabBarController)this.Storyboard.InstantiateViewController(ConstantControllersName.tabBarHomeController);

                if (tabBarHomeController_ != null)
                {
                    tabBarHomeController_.ProvidesPresentationContextTransitionStyle = true;
                    tabBarHomeController_.DefinesPresentationContext = true;

                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        tabBarHomeController_.ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen;
                    }

                    PresentModalViewController(tabBarHomeController_, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentHomeView);
            }
        }


        public void PresentLobbyView()
        {
            try 
            { 
                UINavigationController initialAccessViewController = (UINavigationController)this.Storyboard.InstantiateViewController(ConstantControllersName.InitialAccessNavigationViewController);
                initialAccessViewController.ProvidesPresentationContextTransitionStyle = true;
                initialAccessViewController.DefinesPresentationContext = true;
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    initialAccessViewController.ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen;
                }
                this.PresentViewController(initialAccessViewController, true, null);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentLobbyView);
            }
        }

        protected void HiddenSearchProduct(UIView searchProductView)
        {
            try
            {
                NSLayoutConstraint[] constraints = searchProductView.Constraints;
                searchProductView.RemoveConstraints(constraints);
                foreach (NSLayoutConstraint item in constraints)
                {
                    if (item.Constant == ConstantViewSize.SearcherProductHeightView)
                    {
                        item.Constant = 0;
                    }
                }
                searchProductView.AddConstraints(constraints);
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.IsSummaryDisabled = true;
                NavigationView.IsAccountEnabled = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.HiddenSearchProduct);
            }
        }

        protected void CreateTutorialView(UIView superView, TutorialView tutorial, IList<ContentImage> imageName, int pos, bool repeat)
        {
            TutorialView tutorialView = tutorial;
            if (pos < imageName.Count)
            {
                if (!DeviceManager.Instance.ValidateAccessPreference(imageName[pos].Image) || repeat)
                {
                    this.NavigationController.NavigationBarHidden = true;
                    nfloat heightTabBar = 0;
                    if (this.TabBarController != null)
                    {
                        heightTabBar = this.TabBarController.TabBar.Frame.Height;
                        this.TabBarController.TabBar.Hidden = true;
                    }
                    if (tutorialView == null)
                    {
                        tutorialView = TutorialView.Create();
                    }
                    tutorialView.TutorialImageView.Image = UIImage.FromFile(imageName[pos].Image);
                    tutorialView.TutorialButton.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        DeviceManager.Instance.SaveAccessPreference(imageName[pos].Image, true);
                        pos++;
                        CreateTutorialView(superView, tutorialView, imageName, pos, repeat);
                    };
                    tutorialView.Frame = new CGRect(0, 0, superView.Frame.Width, superView.Frame.Height + heightTabBar);
                    superView.AddSubview(tutorialView);
                }
                else
                {
                    pos++;
                    CreateTutorialView(superView, tutorialView, imageName, pos, repeat);
                }
            }
            else
            {
                if (tutorialView != null)
                {
                    if (!imageName[0].Image.Equals(ConstantImages.Tutorial_entrega)){
                        this.NavigationController.NavigationBarHidden = false;
                    }

                    if (this.TabBarController != null)
                    {
                        this.TabBarController.TabBar.Hidden = false;
                    }
                    tutorialView.RemoveFromSuperview();
                }
            }
        }

        public void RegisterNotificationTags()
        {
            Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.RegisterForPushNotifications();
        }
        #endregion
    }
}
