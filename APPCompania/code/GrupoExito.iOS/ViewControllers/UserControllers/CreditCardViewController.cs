using System;
using Foundation;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class CreditCardViewController : BaseCreditCardController, IDisposable
    {
        #region Attributes
        private NSHttpCookie cookieValue;
        private string cookieNameError = "is_error";
        private bool IsSuccessfullAddCreditCard;
        #endregion

        #region Constructors
        public CreditCardViewController(IntPtr handle) : base(handle)
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
                this.LoadData();
                this.LoadHandlers();
            }
            catch(Exception exception){
                Util.LogException(exception, ConstantControllersName.CreditCardViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.HiddenAccountProfile();
                IsSuccessfullAddCreditCard = false;
                cookieValue = null;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CreditCardViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.AddCreditCard, nameof(CreditCardViewController));
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            Dispose();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public new void Dispose()
        {
            NSHttpCookieStorage.SharedStorage.Dispose();
            if(cookieValue != null)
            {
                cookieValue.Dispose();
            }
            cookieValue = null;
        }
        #endregion

        #region Methods 
        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                SpinnerActivityIndicatorViewFromBase = spinnerAcitivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CreditCardViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            var url = new NSUrl(string.Format(AppServiceConfiguration.AddCreditCardEndPoint, ParametersManager.UserContext.Id, 
                AppServiceConfiguration.SiteId.Equals("exito") ? EnumSiteId.Exito.GetHashCode().ToString() 
                : EnumSiteId.Carulla.GetHashCode().ToString()));
            var req = new NSUrlRequest(url);
            addEditCreditCardWebView.LoadRequest(req);
        }

        private void LoadHandlers(){
            addEditCreditCardWebView.LoadFinished += AddEditCreditCardWebViewLoadFinished;
        }

        private void ResolveResponseWebView()
        {
            if (cookieValue != null)
            {
                if (!string.IsNullOrEmpty(cookieValue.Value))
                {
                    {
                        if (cookieValue.Value.Equals("true"))
                        {
                            var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.AddCreditCardFailedMessage, UIAlertControllerStyle.Alert);
                            alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                            PresentViewController(alertController, true, null);
                        }
                        else
                        {
                            IsSuccessfullAddCreditCard = true;
                            ParametersManager.ContainChanges = true;
                            ParametersManager.CreditCardChanges = true;
                            NSHttpCookieStorage.SharedStorage.DeleteCookie(cookieValue);
                            this.Dispose();
                            this.NavigationController.PopViewController(true);
                        }
                    }
                }
            }
        }
        #endregion

        #region Events 
        private void AddEditCreditCardWebViewLoadFinished(object sender, EventArgs e)
        {
            try
            {
                NSHttpCookieStorage storage = NSHttpCookieStorage.SharedStorage;
                NSHttpCookie[] cookies = storage.CookiesForUrl(addEditCreditCardWebView.Request.MainDocumentURL);
                if (cookies != null && cookies.Length > 0)
                {
                    foreach (NSHttpCookie cookie in cookies)
                    {
                        if (cookie.Name.Contains(cookieNameError))
                        {
                            cookieValue = cookie;
                            break;
                        }
                    }
                    if (IsSuccessfullAddCreditCard == false)
                    {
                        this.ResolveResponseWebView();
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.CreditCardViewController, ConstantEventName.AddEditCreditCardWebViewLoadFinished);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}