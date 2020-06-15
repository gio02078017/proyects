using System;
using Foundation;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class ConditionsStickersViewController : UIViewControllerBase, IDisposable
    {
        #region Attributes
        private NSHttpCookie cookieValue;
        private NSString termsAndConditions = new NSString("TermsAndConditionStickers");
        #endregion

        #region Life Cycle 
        public ConditionsStickersViewController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.LoadData();
            this.LoadExternalViews();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public new void Dispose()
        {
            cookieValue.Dispose();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            try
            {

                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
                NavigationView.EnableBackButton(true);
                this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StickersViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ConditionsStickersViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            var habeasDataUrl = new NSUrl(AppConfigurations.TermsAndConditionStickers);
            var req = new NSUrlRequest(habeasDataUrl);
            UIViewController habeasDataViewController = new UIViewController();
            var request = new NSUrlRequest(habeasDataUrl);
            ConditionsStickersWebView.LoadRequest(request);
        }
        #endregion
    }
}

