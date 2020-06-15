using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    public partial class TermAndConditionViewController : UIViewControllerBase
    {
        #region Constructors
        public TermAndConditionViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.LoadExternalViews();
            this.LoadData();
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
                NavigationView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.NavigationHeaderView, Self, null).GetItem<NavigationHeaderView>(0);
                CGRect navigationViewFrame = new CGRect(0, 0, this.NavigationController.NavigationBar.Frame.Size.Width, this.NavigationController.NavigationBar.Frame.Size.Height);
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
                this.NavigationController.NavigationBar.AddSubview(NavigationView);
                NavigationView.LoadWidthSuperView();
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.TermAndConditionViewController, ConstantMethodName.LoadExternalViews } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void LoadData()
        {
            var url = new NSUrl(AppServiceConfiguration.TermsAndConditionApp);
            var req = new NSUrlRequest(url);
            termAndConditionsWebView.LoadRequest(req);
        }
        #endregion
    }
}

