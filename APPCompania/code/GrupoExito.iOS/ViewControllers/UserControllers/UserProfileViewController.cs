using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class UserProfileViewController : UIViewControllerBase
    {
        #region Attributes 
        private MyAccount MyAccount { get; set; }
        private MyAccountModel _myAccountModel { get; set; }
        private IList<MenuItem> MenuItems { get; set; }
        #endregion

        #region Constructors
        public UserProfileViewController(IntPtr handle) : base(handle)
        {
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
            MyAccount = new MyAccount();
        }
        #endregion

        #region Life Cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.MyAccount, nameof(UserProfileViewController));
        }

        public override void ViewDidLoad()
        {
            try
            {               
                base.ViewDidLoad();
                this.LoadExternalViews();
                this.LoadHandlers();
                this.GetMyAccountAsync();
                this.LoadCorners();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UserProfileViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.IsAccountEnabled = false;
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
                this.SetUserName();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UserProfileViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Methods 

            private void LoadHandlers()
        {
            closeButton.TouchUpInside += LogoutButtonTouchUpInside;
            editButton.TouchUpInside += EditButtonTouchUpInside;
        }

        private void SetUserName()
        {
            try
            {
                string[] name = Util.Capitalize(ParametersManager.UserContext.FirstName).Split(' ');
                if (name.Any())
                {
                    nameLabel.Text = name[0];
                }

                if (!AppServiceConfiguration.SiteId.Equals("exito"))
                {
                    UserContext userContext = ParametersManager.UserContext;
                    if (userContext.UserType != null && !string.IsNullOrEmpty(userContext.UserType.Name))
                    {
                        if (SuperclientLabel != null)
                        {
                            SuperclientLabel.Hidden = false;
                            SuperclientLabel.Text = userContext.UserType.Name;
                        }
                    }
                    else
                    {
                        if (SuperclientLabel != null)
                        {
                            SuperclientLabel.Hidden = true;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UserProfileViewController, ConstantMethodName.SetUserName);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                profileCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.AccountUserViewCell, NSBundle.MainBundle), ConstantIdentifier.profileCollectionViewCellIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                this.NavigationController.NavigationBarHidden = false;
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AccountUserViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            profileCollectionView.Layer.BorderColor = UIColor.LightGray.ColorWithAlpha(0.2f).CGColor;
            profileCollectionView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            profileCollectionView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void SetUser()
        {
            if (MyAccount.User != null)
            {
                UserContext userContext = ModelHelper.SetUserEditContext(ParametersManager.UserContext, MyAccount.User);
                var userContextPreferences = ParametersManager.UserContext;
                userContext.Address = userContextPreferences.Address;
                userContext.Store = userContextPreferences.Store;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
            }
        }

        private void LoadMenuData()
        {
            MenuItems = new List<MenuItem>();
            MenuItems = MyAccount.Menu;
            profileCollectionView.Source = new MenuAccountTableViewSource(MenuItems, ConstantIdentifier.profileCollectionViewCellIdentifier, this);
            profileCollectionView.ReloadData();
        }

        private void LogOut()
        {
            var customAlert = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.AreYouSureWantDisconnect, UIAlertControllerStyle.Alert);
            customAlert.AddAction(UIAlertAction.Create(AppMessages.Confirm, UIAlertActionStyle.Default,
             action =>
             {
                 if (DeviceManager.Instance.DeleteAccessPreference())
                 {
                     DataBase.FlushCar();

                     ParametersManager.UserContext = null;
                     Plugin.FirebasePushNotification.CrossFirebasePushNotification.Current.RegisterForPushNotifications();

                     PresentLoginView();
                 }
             }));
            customAlert.AddAction(UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Default, action => { }));
            this.PresentViewController(customAlert, true, null);
        }
        #endregion

        #region Methods Async 
        private async Task GetMyAccountAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                MyAccount = _myAccountModel.GetMyAccount();
                this.SetUser();
                this.LoadMenuData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UserProfileViewController, ConstantMethodName.GetMyAccount);
                ShowMessageException(exception);
            }
            finally
            {
                StopActivityIndicatorCustom();
            }
        }

        #endregion

        #region Events
        private void LogoutButtonTouchUpInside(object sender, EventArgs e)
        {
            LogOut();
        }

        private async void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.SetUserName();
            await this.GetMyAccountAsync();
        }

        private void EditButtonTouchUpInside(object sender, EventArgs e)
        {
            EditProfileViewController editProfileViewController = (EditProfileViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.EditProfileViewController);
            this.NavigationController.PushViewController(editProfileViewController, true);
        }
        #endregion
    }
}

