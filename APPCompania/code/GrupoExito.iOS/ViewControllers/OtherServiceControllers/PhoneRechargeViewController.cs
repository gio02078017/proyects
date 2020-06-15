using System;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers
{
    public partial class PhoneRechargeViewController : UIViewControllerBase
    {
        #region Constructors
        public PhoneRechargeViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadExternalViews();
                //this.LoadFonts();
                this.LoadHandlers();
                this.LoadData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PhoneRechargeViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.HiddenCarData();
                if (ParametersManager.UserContext == null || ParametersManager.UserContext.IsAnonymous)
                {
                    NavigationView.HiddenAccountProfile();
                }
                else
                {
                    NavigationView.ShowAccountProfile();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PhoneRechargeViewController, ConstantMethodName.ViewWillAppear);
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
            //Load events controls
        }

        private void LoadCorners()
        {
            try
            {
                //Load corners controls
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PhoneRechargeViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                //servicesTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.MenuServicesTableViewCell, NSBundle.MainBundle), ConstantIdentifier.MenuServicesItemIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                this.NavigationController.NavigationBarHidden = false;
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PhoneRechargeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            StartActivityIndicatorCustom();
            //menuItems = JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuServicesInStoreSource);
            //servicesTableViewHeightLayoutConstraint.Constant = menuItems.Count * ConstantViewSize.MenuServicesInStoreHeightCell;
            //servicesTableView.Source = new ServiceInStoreViewSource(menuItems, ConstantIdentifier.MenuServicesItemIdentifier, this);
            //servicesTableView.ReloadData();
            StopActivityIndicatorCustom();
        }
        #endregion
    }
}

