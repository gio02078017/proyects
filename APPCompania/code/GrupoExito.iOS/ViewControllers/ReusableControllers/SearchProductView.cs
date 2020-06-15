using System;
using CoreLocation;
using Foundation;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers;
using GrupoExito.iOS.ViewControllers.RecipesControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers;
using GrupoExito.Utilities.Resources;
using UIKit;
using static GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.ServiceInStoreViewController;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class SearchProductView : UIView
    {
        #region Attributes
         
        private UIViewControllerBase ControllerBase;
        #endregion

        #region Properties 
        public UITextField ProductSearch{
            get { return productSearchTextField; }
        }

        public UIButton ProductSearchButton{
            get { return openSearchButton; }
        }

        public UIButton searchData{
            get { return searchTextButton; }
        }

        private CLLocationManager LocationManager;
        #endregion 

        #region Constructors
        static SearchProductView() 
        {
            //Static default Constructor this class 
        }

        protected SearchProductView(IntPtr handle) : base(handle) 
        {
            //Default Constructor this class 
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadHandlers();
            LocationManager = new CLLocationManager();
        }
        #endregion

        #region Methods
        public void LoadSizeControl(UIViewControllerBase controllerBase){
            this.ControllerBase = controllerBase;
            searchProductWidthConstraint.Constant = this.Frame.Width;
        }

        private void LoadHandlers()
        {
            
            productSearchTextField.ReturnKeyType = UIReturnKeyType.Search;
            productSearchTextField.EditingChanged += (object sender, EventArgs e) =>
            {
            };

            productSearchTextField.ShouldReturn = (textField) =>
            {
                textField.ResignFirstResponder();
                return true;
            };
            recipeButton.TouchUpInside += RecipeButtonTouchUpInside;
            openSearchButton.TouchUpInside += SearchProductUpInside;
        }
        #endregion

        #region Events 
        private void SearchProductUpInside(object sender, EventArgs e)
        {
            RegisterEventSearch();
            SearchProductViewController searchProductViewController_ = (SearchProductViewController) ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.SearchProductViewController);
            searchProductViewController_.HidesBottomBarWhenPushed = true;
            ControllerBase.NavigationController.PushViewController(searchProductViewController_, true);
        }

        void RecipeButtonTouchUpInside(object sender, EventArgs e)
        {
            RegisterEventChecker();
            InitialRecipeViewController initialRecipeViewController = (InitialRecipeViewController)ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.InitialRecipeViewController);
            initialRecipeViewController.HidesBottomBarWhenPushed = true;
            ControllerBase.NavigationController.PushViewController(initialRecipeViewController, true);
        }

        private void RegisterEventSearch()
        {
            RegisterEventChecker();
        }
        
        private void RegisterEventChecker()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEvent(AnalyticsEvent.HomeChecker);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEvent(AnalyticsEvent.HomeChecker);
        }

        private void CheckerFeature()
        {
            if (IsLocationEnabled())
            {
                SelectStorePriceCheckerViewController selectStorePriceCheckerView = (SelectStorePriceCheckerViewController)ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.SelectStorePriceCheckerViewController);
                selectStorePriceCheckerView.HidesBottomBarWhenPushed = true;
                ControllerBase.NavigationController.PushViewController(selectStorePriceCheckerView, true);
            }
            else if (CLLocationManager.Status.Equals(CLAuthorizationStatus.NotDetermined))
            {
                LocationManager.RequestWhenInUseAuthorization();
            }
            else
            {
                ShowStoresRelatedAlert(AppMessages.AppLocationMessage, (result) => { },
                (result) =>
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                });
            }
        }

        private void ShowStoresRelatedAlert(string message, AlertOKCancelDelegate acceptAction, AlertOKCancelDelegate configurationAction)
        {
            var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, action =>
            {
                acceptAction?.Invoke(true);
            }));

            if (configurationAction != null)
            {
                alertController.AddAction(UIAlertAction.Create(AppMessages.Configuration, UIAlertActionStyle.Default, action =>
                {
                    configurationAction(false);
                }));
            }
            ControllerBase.PresentViewController(alertController, true, null);
        }

        private bool IsLocationEnabled()
        {
            return (CLLocationManager.Status.Equals(CLAuthorizationStatus.AuthorizedAlways) || CLLocationManager.Status.Equals(CLAuthorizationStatus.AuthorizedWhenInUse));
        }
        #endregion
    }
}
