using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class RegistryUserSuccessViewController : UIViewControllerBase
    {

        #region Constructors
        public RegistryUserSuccessViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadExternalViews();
                this.LoadCorners();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserSuccessViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(true, false, false, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

        }
        #endregion

        #region Methods
        private void LoadCorners()
        {
            try
            {
                startButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
                startButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                startButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserSuccessViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                NavigationView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.NavigationHeaderView, Self, null).GetItem<NavigationHeaderView>(0);
                CGRect navigationViewFrame = new CGRect(0, 0, this.NavigationController.NavigationBar.Frame.Size.Width, this.NavigationController.NavigationBar.Frame.Size.Height);
                this.NavigationController.NavigationBar.AddSubview(NavigationView);
                NavigationView.LoadWidthSuperView();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserSuccessViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events 
        partial void startButton_UpInside(UIButton sender)
        {
            PresentLobbyView();
        }
        #endregion
    }
}

