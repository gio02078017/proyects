using System;
using System.Linq;
using Foundation;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers;
using GrupoExito.iOS.ViewControllers.UserControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class NavigationHeaderView : UINavigationBar
    {
        #region Atributes 
        private bool isAccountEnabled;
        private bool isSummaryDisabled;

        private bool isLobbyEnabled = false;

        private UIViewControllerBase ControllerBase;
        private UIViewController controller;
        private bool IsMaster, IsPopUp, IsNavigation = false;
        #endregion

        #region Properties 
        public UIButton Profile { get => profileButton; }
        public bool IsSummaryDisabled { get => isSummaryDisabled; set => isSummaryDisabled = value; }
        public bool IsAccountEnabled { get => isAccountEnabled; set => isAccountEnabled = value; }
        #endregion

        #region constructors
        static NavigationHeaderView() 
        {
            //Static default Constructor this class 
        }
        protected NavigationHeaderView(IntPtr handle) : base(handle) 
        {
            //Static default Constructor this class  
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadHandlers();
            this.LoadColors();


        }
        #endregion

        #region methods 
        public void LoadControllers(bool isMaster, bool isPopUp, bool isNavigation, UIViewControllerBase viewController)
        {
            try
            {
                this.ControllerBase = viewController;
                this.IsPopUp = isPopUp;
                this.IsNavigation = isNavigation;
                this.IsMaster = isMaster;
                this.IsAccountEnabled = true;
                if (isMaster)
                {
                    returnImageButton.Hidden = true;
                    spacingLeftLogoReturnWidthConstraint.Constant = ConstantStyle.SpacingLeftConstrolsNavHidden;
                }
                else
                {
                    returnImageButton.Hidden = false;
                    spacingLeftLogoReturnWidthConstraint.Constant = ConstantStyle.SpacingLeftConstrolsNavShow;
                }
                if (ParametersManager.UserContext != null)
                {
                    string[] name = Util.Capitalize(ParametersManager.UserContext.FirstName).Split(' ');
                    if (name.Any())
                    {
                        clientNameLabel.Text = name[0];
                    }
                }
                else
                {
                    clientNameLabel.RemoveFromSuperview();
                    this.summaryButton.Hidden = true;
                    this.statusCarImageView.Hidden = true;
                    this.statusCountCarButton.Hidden = true;
                    priceCurrentLabel.Hidden = true;
                }
                ControllerBase.UpdateCar();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.NavigationHeaderView, ConstantMethodName.LoadControllers);
            }
        }

        public void LoadControllerBase(UIViewController controller)
        {
            this.controller = controller;
            this.ControllerBase = null;
        }

        public void EnableLobbyButton(bool value)
        {
            isLobbyEnabled = value;
        }

        public void EnableBackButton(bool value)
        {
            IsMaster = false;
            IsNavigation = value;
            returnImageButton.Enabled = value;
            returnImageButton.Hidden = !value;
            if (value)
            {
                spacingLeftLogoReturnWidthConstraint.Constant = ConstantStyle.SpacingLeftConstrolsNavShow;
            }
            else
            {
                spacingLeftLogoReturnWidthConstraint.Constant = ConstantStyle.SpacingLeftConstrolsNavHidden;
            }
        }

        public void UpdateCar(String price, String quantity)
        {
            InvokeOnMainThread(() =>
            {
                statusCountCarButton.SetTitle(quantity, UIControlState.Normal);

                if (!price.Equals("$0") && !quantity.Equals("0"))
                {
                    this.priceCurrentLabel.Text = price;
                    this.priceCurrentLabel.Hidden = false;
                    this.summaryButton.Hidden = false;
                    this.statusCarImageView.Hidden = false;
                    this.statusCountCarButton.Hidden = false;
                }
                else
                {
                    if (!IsSummaryDisabled)
                    {
                        this.summaryButton.Hidden = false;
                        this.statusCarImageView.Hidden = false;
                        this.statusCountCarButton.Hidden = false;
                    }
                    this.priceCurrentLabel.Text = "";
                    this.priceCurrentLabel.Hidden = true;
                }
                int lengthPrice = ((this.priceCurrentLabel.Text.Length > 0 && this.priceCurrentLabel.Text.Length < 5) ? 8 : this.priceCurrentLabel.Text.Length);
                this.priceCurrentConstraint.Constant = ConstantViewSize.priceNavigationWidth * (lengthPrice + 2);

            });
        }

        public void LoadWidthSuperView()
        {
            navigationHeaderWidthViewConstraint.Constant = this.Superview.Frame.Width;
        }

        public void ShowCarData()
        {
            this.IsSummaryDisabled = false;
            this.summaryButton.Hidden = false;
            this.statusCarImageView.Hidden = false;
            this.statusCountCarButton.Hidden = false;
            this.priceCurrentLabel.Hidden = false;
        }

        public void HiddenCarData()
        {
            this.IsSummaryDisabled = true;
            this.summaryButton.Hidden = true;
            this.statusCarImageView.Hidden = true;
            this.statusCountCarButton.Hidden = true;
            this.priceCurrentLabel.Hidden = true;
        }

        public void ShowAccountProfile()
        {
            this.profileButton.Hidden = false;
            this.profileImageView.Hidden = false;
            this.lineVerticalView.Hidden = false;
        }

        public void HiddenAccountProfile()
        {
            this.profileButton.Hidden = true;
            this.profileImageView.Hidden = true;
            this.lineVerticalView.Hidden = true;
        }

        public void ChangeImageMyAccount(string image)
        {
            profileImageView.Image = UIImage.FromFile(image);
        }

        private void PresentSummaryView()
        {
            SummaryContainerController summaryContainer = (SummaryContainerController)ControllerBase.Storyboard.InstantiateViewController(nameof(SummaryContainerController));
            summaryContainer.HidesBottomBarWhenPushed = true;
            ControllerBase.NavigationController.PushViewController(summaryContainer, true);
        }

        private void PresentLobbyView()
        {
            try
            {
                UINavigationController initialAccessViewController = (UINavigationController)ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.InitialAccessNavigationViewController);
                initialAccessViewController.ProvidesPresentationContextTransitionStyle = true;
                initialAccessViewController.DefinesPresentationContext = true;
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    initialAccessViewController.ModalPresentationStyle = UIModalPresentationStyle.BlurOverFullScreen;
                }
                ControllerBase.PresentViewController(initialAccessViewController, true, null);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentLobbyView);
            }
        }

        private void LoadCorners()
        {
            statusCountCarButton.Layer.CornerRadius = statusCountCarButton.Frame.Width / 2;
            statusCountCarButton.Layer.BorderColor = ConstantColor.UiBorderCountStatus.CGColor;
        }

        private void LoadHandlers()
        {
            statusCountCarButton.TouchUpInside -= SummaryTouchUpInside;
            statusCountCarButton.TouchUpInside += SummaryTouchUpInside;
            profileButton.TouchUpInside -= MyProfileTouchUpInside;
            profileButton.TouchUpInside += MyProfileTouchUpInside;
            returnViewButton.TouchUpInside -= ReturnTouchUpInside;
            returnViewButton.TouchUpInside += ReturnTouchUpInside;
            summaryButton.TouchUpInside -= SummaryButtonTouchUpInside;
            summaryButton.TouchUpInside += SummaryButtonTouchUpInside;
        }

        private void LoadColors()
        {
            this.containerView.BackgroundColor = ConstantColor.UiPrimary;
            this.priceCurrentLabel.TextColor = ConstantColor.UiTextColorGeneric;
        }
        #endregion

        #region Events
        private void ReturnTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (IsPopUp)
                {
                    ControllerBase?.DismissViewController(true, null);
                }

                if (IsNavigation && !IsMaster)
                {
                    if (this.ControllerBase != null)
                    {
                        this.ControllerBase?.NavigationController.PopViewController(true);
                    }
                    else
                    {
                        this.controller?.NavigationController.PopViewController(true);
                    }
                }

                if (isLobbyEnabled && IsMaster)
                {
                    PresentLobbyView();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.NavigationHeaderView, ConstantEventName.ReturnButtonUpInside);
            }
        }

        private void MyProfileTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (IsAccountEnabled)
                {
                    RegisterEventProfile();
                    UserProfileViewController userProfileView = (UserProfileViewController)ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.UserProfileViewController);
                    userProfileView.HidesBottomBarWhenPushed = true;
                    ControllerBase.NavigationController.PushViewController(userProfileView, true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.NavigationHeaderView, ConstantEventName.ReturnButtonUpInside);
            }
        }

        private void SummaryButtonTouchUpInside(object sender, EventArgs e)
        {
            SummaryTouchUpInside(sender, e);
        }

        private void SummaryTouchUpInside(object sender, EventArgs e)
        {
            if (!IsSummaryDisabled)
            {
                PresentSummaryView();
            }
        }

        private void RegisterEventProfile()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEvent(AnalyticsEvent.HomeProfile);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEvent(AnalyticsEvent.HomeProfile);
        }
        #endregion
    }
}
