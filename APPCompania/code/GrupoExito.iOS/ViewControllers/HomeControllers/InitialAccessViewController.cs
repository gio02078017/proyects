using System;
using System.Threading.Tasks;
using CoreAnimation;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.OtherServiceControllers;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.RecipesControllers;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers;
using GrupoExito.iOS.ViewControllers.UserControllers;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    public partial class InitialAccessViewController : UIViewControllerBase
    {
        #region Attributes
        private ContentsModel _contentsModel;
        #endregion

        #region Constructors 
        public InitialAccessViewController(IntPtr handle) : base(handle)
        {
            _contentsModel = new ContentsModel(new ContentsService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.NavigationController.NavigationBarHidden = true;
                this.LoadExternalViews();
                this.SetUserName();
                this.LoadHandlers();
                this.LoadCorners();
                this.RegisterNotificationTags();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Searcher, nameof(InitialAccessViewController));
                base.ViewDidAppear(animated);
                UIView.Animate(0, 0, UIViewAnimationOptions.TransitionFlipFromBottom,
                    () => { this.NavigationController.NavigationBarHidden = true; },
                    () => { });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        private async Task GetUserType()
        {
            UserTypeResponse response = await _myAccountModel.GetUserType();
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(response.Name))
                {
                    UserContext userContext = ParametersManager.UserContext;
                    userContext.UserType = new Entities.Entiites.Users.Segment
                    {
                        Name = response.Name,
                        Code = response.Code
                    };
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, GrupoExito.Utilities.Helpers.JsonService.Serialize(userContext));
                    if (SuperclientLabel != null)
                    {
                        SuperclientLabel.Hidden = false;
                        SuperclientLabel.Text = response.Name;
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

        private void SetUserName()
        {
            UserContext userContext = ParametersManager.UserContext;
            if (userContext == null || userContext.IsAnonymous)
            {
                userNameLabel.Text = AppMessages.Hello + "!";
            }
            else
            {
                userNameLabel.Text = AppMessages.Hello + " " + Util.Capitalize(userContext.FirstName).TrimEnd() + "!";
            }

            if (!AppServiceConfiguration.SiteId.Equals("exito"))
            {
                this.GetUserType();
            }
            else
            {
                if (SuperclientLabel != null)
                {
                    SuperclientLabel.Hidden = true;
                }
            }
        }

        private void LoadFonts()
        {
            userNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.UserNameHome);
        }

        private void LoadHandlers()
        {
            try
            {
                doMyShoppingButton.TouchUpInside += DoMyShoppingButtonTouchUpInside;
                knowMyDiscountsButton.TouchUpInside += KnowMyDiscountsButtonTouchUpInside;
                servicesInStoreButton.TouchUpInside += ServicesInStoreButtonTouchUpInside;
                otherServicesButton.TouchUpInside += OtherServicesButton_TouchUpInside;
                if (knowMyStickersButton != null)
                {
                    knowMyStickersButton.TouchUpInside += KnowMyStickersButtonTouchUpInside;
                }
                logoutButton.TouchUpInside += LogoutButtonTouchUpInside;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.LoadHandler);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                containerOptionsView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                //titleLabel.Layer.CornerRadius = ConstantStyle.CornerRadius;
                //titleLabel.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner ;
                //titleLabel.ClipsToBounds = true;
                //otherServicesView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                //otherServicesView.Layer.MaskedCorners = CACornerMask.MaxXMaxYCorner | CACornerMask.MinXMaxYCorner;
                //otherServicesView.ClipsToBounds = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.LoadCorners);
            } 
        }

        private void LoadExternalViews()
        {
            try
            {
                //LoadCustomSpinnerView(customSpinnerView, ref spinnerActivityIndicatorView_);
                //customSpinnerViewFromBase = customSpinnerView;
                //spinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }


        #endregion

        #region Events 
        private void DoMyShoppingButtonTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Entities.UserContext userContext = ParametersManager.UserContext;
                if (userContext != null && ValidateDependecy() != null)
                {
                    RegisterShoppingEvent();
                    PresentHomeView();
                }
                else
                {
                    WellcomeViewController welcomeViewController = (WellcomeViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.WellcomeViewController);
                    if (welcomeViewController != null)
                    {
                        this.NavigationController.PushViewController(welcomeViewController, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantEventName.DoMyShoppingButtonTouchUpInside);
                ShowMessageException(exception);
            }
        }

        private void RegisterShoppingEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserId(AnalyticsEvent.InitialAccessShopping);
        }

        private void RegisterDiscountsEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserIdAndLaunchedFrom(AnalyticsEvent.Discounts, ConstantControllersName.InitialAccessViewController);
        }

        private void RegisterServicesInStoreEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserId(AnalyticsEvent.InitialAccessStoreServices);
        }

        private void RegisterOtherServicesEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserId(AnalyticsEvent.Recipes);
        } 

        private void KnowMyDiscountsButtonTouchUpInside(object sender, EventArgs e)
        {
            if (false) //RegistrationValidationViewController.ValidationIsNeeded()
            {
                RegistrationValidationViewController validationViewController = new RegistrationValidationViewController();

                validationViewController.OperationDoneAction += (result) =>
                {
                    RemoveChild(validationViewController);

                    if (result)
                    {
                        MyDiscountViewController myDiscountViewController = (MyDiscountViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyDiscountViewController);
                        myDiscountViewController.HidesBottomBarWhenPushed = true;
                        this.NavigationController.PushViewController(myDiscountViewController, true);
                    }
                    else
                    {
                        ShowError(AppMessages.VerifyUserError);
                    }
                };

                validationViewController.OperationCanceledAction += () =>
                {
                    RemoveChild(validationViewController);
                    this.NavigationController.NavigationBarHidden = false;
                };

                AddChildViewController(validationViewController);
                validationViewController.View.Frame = View.Bounds;

                this.NavigationController.NavigationBarHidden = true;
                validationViewController.Cellphone = ParametersManager.UserContext.CellPhone;
                View.AddSubview(validationViewController.View);
                validationViewController.DidMoveToParentViewController(this);
            }
            else
            {
                MyDiscountViewController myDiscountViewController = (MyDiscountViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyDiscountViewController);
                myDiscountViewController.HidesBottomBarWhenPushed = true;
                this.NavigationController.PushViewController(myDiscountViewController, true);
            }
        }

        private void RemoveChild(UIViewController vc)
        {
            vc.WillMoveToParentViewController(null);
            vc.View.RemoveFromSuperview();
            vc.RemoveFromParentViewController();
        }

        private void ShowError(string message)
        {
            InvokeOnMainThread(() =>
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure("", message,
                    (errorSender, ea) => errorView.RemoveFromSuperview());
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            });
        }

        private void ServicesInStoreButtonTouchUpInside(object sender, EventArgs e)
        {
            RegisterServicesInStoreEvent();
            ServiceInStoreViewController serviceInStoreViewController = (ServiceInStoreViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ServiceInStoreViewController);
            serviceInStoreViewController.OpenFromInitialOption = true;
            serviceInStoreViewController.HidesBottomBarWhenPushed = true;
            this.NavigationController.PushViewController(serviceInStoreViewController, true);
        }

        private void OtherServicesButton_TouchUpInside(object sender, EventArgs e)
        {
            RegisterOtherServicesEvent();
            InitialRecipeViewController initialRecipeViewController = (InitialRecipeViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.InitialRecipeViewController);
            initialRecipeViewController.HidesBottomBarWhenPushed = true;
            this.NavigationController.PushViewController(initialRecipeViewController, true);
        }

        private void KnowMyStickersButtonTouchUpInside(object sender, EventArgs e)
        {
            StickersViewController stickersViewController = (StickersViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.StickersViewController);
            stickersViewController.OpenFromInitialOption = true;
            stickersViewController.HidesBottomBarWhenPushed = true;
            this.NavigationController.PushViewController(stickersViewController, true);
        }

        private void LogoutButtonTouchUpInside(object sender, EventArgs e)
        {
            LogOut();
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
                     RegisterNotificationTags();
                     PresentLoginView();
                 }
             }));
            customAlert.AddAction(UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Default, action => { }));
            this.PresentViewController(customAlert, true, null);
        }
        #endregion
    }
}

