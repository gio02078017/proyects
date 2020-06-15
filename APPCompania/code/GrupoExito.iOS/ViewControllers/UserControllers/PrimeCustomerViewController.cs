using System;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class PrimeCustomerViewController : UIViewControllerBase
    {
        #region Attributes 
        private MyAccount MyAccount { get; set; }
        private MyAccountModel _myAccountModel { get; set; }
        #endregion

        #region Constructors 
        public PrimeCustomerViewController(IntPtr handle) : base(handle)
        {
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
            MyAccount = new MyAccount();
        }
        #endregion

        #region Life Cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ClientPrime, nameof(PrimeCustomerViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                this.LoadExternalViews();
                this.GetUser();
            }
            catch(Exception exception){
                Util.LogException(exception, ConstantControllersName.PrimeCustomerViewController, ConstantMethodName.ViewDidLoad);
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
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PrimeCustomerViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion


        #region Methods Async 
        private async Task GetUser()
        {
            try
            {
                StartActivityIndicatorCustom();
                var response = await _myAccountModel.GetUser();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    string message = MessagesHelper.GetMessage(response.Result);
                    StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                }
                else
                {
                    SetUser(response.User);
                    this.PutData();
                }

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UserProfileViewController, ConstantMethodName.GetMyAccount);
                ShowMessageException(exception);
            }
        }
        #endregion


        #region Methods 

        private void PutData()
        {
            UserContext user = ParametersManager.UserContext;
            startDateLabel.Text = ParametersManager.UserContext.StartDatePrime;
            endDateLabel.Text = ParametersManager.UserContext.EndDatePrime;
            paymentTypeLabel.Text = ParametersManager.UserContext.PaymentMethodPrime;
            typeOfMembershipLabel.Text = ParametersManager.UserContext.PeriodicityPrime;
            StopActivityIndicatorCustom();
            
           
        }

        private void SetUser(User user)
        {
            if (user != null)
            {
                UserContext userContext = ModelHelper.UpdateUserContext(ParametersManager.UserContext, user);
                userContext.Address = ParametersManager.UserContext.Address;
                userContext.Store = ParametersManager.UserContext.Store;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
            }
        }


        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                _spinnerActivityIndicatorView.Retry.TouchUpInside += Retry_TouchUpInside;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PrimeCustomerViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        void Retry_TouchUpInside(object sender, EventArgs e)
        {
            try
            {
                this.GetUser();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.PrimeCustomerViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            StartActivityIndicatorCustom();
            if (string.IsNullOrEmpty(ParametersManager.UserContext.StartDatePrime))
            {
                StartActivityErrorMessage(EnumErrorCode.UnexpectedErrorMessage.ToString(), "");
            }
            else
            {
                startDateLabel.Text = ParametersManager.UserContext.StartDatePrime;
                endDateLabel.Text = ParametersManager.UserContext.EndDatePrime;
                paymentTypeLabel.Text = ParametersManager.UserContext.PaymentMethodPrime;
                typeOfMembershipLabel.Text = ParametersManager.UserContext.PeriodicityPrime;
                StopActivityIndicatorCustom();
            }
        }
        #endregion
    }
}
