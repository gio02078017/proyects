using System;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class ConsultPointsViewController : UIViewControllerBase
    {
        #region attributtes
        private PointsModel pointsModel;
        #endregion

        #region Constructors
        public ConsultPointsViewController(IntPtr handle) : base(handle)
        {
            pointsModel = new PointsModel(new PointsService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.MyPoints, nameof(ConsultPointsViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {               
                this.LoadExternalViews();
                this.LoadHandlers();
                this.GetpointsListAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ConsultPointsViewController, ConstantMethodName.ViewDidLoad);
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
                this.NavigationController.NavigationBarHidden = false;

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ConsultPointsViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        public async Task GetpointsListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                PointsResponse response = await pointsModel.GetUserPoints();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    if (response.AvailablePoints > 0)
                    {
                        accumulatedLabel.Text = response.AvailablePoints.ToString();
                        overcomeLabel.Text = response.ExpirationPoints.ToString();
                        accumulatedDateLabel.Text = response.AcumulatedDate != null ? StringFormat.DateWithPrefix(response.AcumulatedDate, "Al") : "";
                        overcomeDateLabel.Text = response.ExpirationDate != null ? StringFormat.DateWithPrefix(response.ExpirationDate, "El") : "";
                    }
                    else
                    {
                        overcomeStackView.Hidden = true;
                        accumulatedDateLabel.Hidden = true;
                        accumulatedLabel.Hidden = false;
                        accumulatedLabel.Text = response.AvailablePoints.ToString();
                    }
                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ConsultPointsViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            LoadNavigationView(this.NavigationController.NavigationBar);
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            CustomSpinnerViewFromBase = customSpinnerView;
            SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
        }

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }
        #endregion

        #region Events
        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.NavigationController.PopViewController(true);
        }
        #endregion        
    }
}
