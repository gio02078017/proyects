using System;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;

namespace GrupoExito.iOS.ViewControllers.ListControllers
{
    public partial class TutorialMyListViewController : UIViewControllerBase
    {
        #region Attributes
        private int typeView;

        public int TypeView { get => typeView; set => typeView = value; }
        #endregion

        public TutorialMyListViewController(IntPtr handle) : base(handle){}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.LoadCorners();
            this.LoadHandlers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                if (TypeView != 1)
                {
                    this.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
                    NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                    NavigationView.LoadControllers(true, false, true, this);
                    NavigationView.HiddenCarData();
                    NavigationView.HiddenAccountProfile();
                }
                else
                {
                    NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                    NavigationView.LoadControllers(false, false, true, this);
                    NavigationView.ShowAccountProfile();
                    NavigationView.ShowCarData();
                    this.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TutorialMyListViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void LoadCorners() {
            addProductsButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadHandlers() { 
            addProductsButton.TouchUpInside += AddProductsButtonTouchUpInside;
        }

        private void AddProductsButtonTouchUpInside(object sender, EventArgs e)
        {
            this.TabBarController.SelectedIndex = 0;
            this.NavigationController.PopToRootViewController(true);
        }

    }

}


