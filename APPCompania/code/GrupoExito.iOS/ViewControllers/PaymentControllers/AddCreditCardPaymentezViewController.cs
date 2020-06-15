using System;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers
{
    public partial class AddCreditCardPaymentezViewController : BaseCreditCardController, IDisposable
    {
        #region Attributes
        private NSHttpCookie cookieValue;
        private readonly string cookieName = "cardResponse";
        private bool IsSuccessfullAddCreditCard;
        #endregion

        #region Constructors
        public AddCreditCardPaymentezViewController(IntPtr handle) : base(handle)
        {
            //Default Constructors this class
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                LoadExternalViews();
                LoadData();
                LoadHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddCreditCardPaymentezViewController, ConstantMethodName.ViewDidLoad);
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
                Util.LogException(exception, ConstantControllersName.AddCreditCardPaymentezViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.AddCreditCardPaymentez, nameof(AddCreditCardPaymentezViewController));
        }

        public new void Dispose()
        {
            cookieValue.Dispose();
        }
        #endregion

        #region Methods
        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddCreditCardPaymentezViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            var url = new NSUrl(string.Format(AppServiceConfiguration.AddCreditCardPaymentezEndPoint, ParametersManager.UserContext.Id, ParametersManager.UserContext.Email, AppServiceConfiguration.SiteId));
            var req = new NSUrlRequest(url);
            webView.LoadRequest(req);
        }

        private void LoadHandlers()
        {
            webView.LoadFinished += AddEditCreditCardWebViewLoadFinished;
        }

        private void ResolveResponseWebView()
        {
            if (cookieValue != null)
            {
                if (!string.IsNullOrEmpty(cookieValue.Value))
                {
                    {
                        string responseAddCard = System.Net.WebUtility.UrlDecode(cookieValue.Value);

                        AddCardResponse response = JsonService.Deserialize<AddCardResponse>(responseAddCard);

                        if (response.Response.Status != null && !string.IsNullOrEmpty(response.Response.Status))
                        {
                            if (response.Response.Status.Equals(ConstStatusAddCard.Valid))
                            {
                                IsSuccessfullAddCreditCard = true;
                                var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.AddCreditCardSuccessfullMessage, UIAlertControllerStyle.Alert);
                                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, (obj) =>
                                {
                                    this.NavigationController.PopViewController(true);
                                }));
                                PresentViewController(alertController, true, null);
                                ParametersManager.CreditCardChanges = true;
                            }
                            else
                            {
                                var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.AddCreditCardFailedMessage, UIAlertControllerStyle.Alert);
                                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, (obj) =>
                                {
                                    this.NavigationController.PopViewController(true);
                                }));
                                PresentViewController(alertController, true, null);
                            }
                        }

                        NSHttpCookieStorage.SharedStorage.DeleteCookie(cookieValue);
                        Dispose();
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
                NSHttpCookie[] cookies = storage.CookiesForUrl(webView.Request.MainDocumentURL);
                if (cookies != null && cookies.Length > 0)
                {
                    foreach (NSHttpCookie cookie in cookies)
                    {
                        if (cookie.Name.Contains(cookieName))
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
                Util.LogException(exception, ConstantControllersName.AddCreditCardPaymentezViewController, ConstantEventName.AddEditCreditCardWebViewLoadFinished);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}