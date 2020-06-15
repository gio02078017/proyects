using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class NotificationsViewController : UIViewControllerBase
    {
        #region Atributtes
        private NotificationsModel NotificationsModel;
        private List<AppNotification> AppNotifications;
        #endregion

        #region Constructors
        public NotificationsViewController(IntPtr handle) : base(handle)
        {
            NotificationsModel = new NotificationsModel(new NotificationsService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadExternalViews();
                this.GetNotificationsAsync();
                this.LoadFonts();
                this.LoadHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.NotificationsViewController, ConstantMethodName.ViewDidLoad);
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
                Util.LogException(exception, ConstantControllersName.ContactUsViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods
        public async Task GetNotificationsAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                NotificationsResponse response = await NotificationsModel.GetNotifications();

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
                    if (response.Notifications != null && response.Notifications.Count > 0)
                    {

                        this.AppNotifications = new List<AppNotification>();
                        this.AppNotifications.AddRange(response.Notifications);
                        this.LoadData();
                    }
                    else
                    {
                        _spinnerActivityIndicatorView.Image.StopAnimating();
                        _spinnerActivityIndicatorView.Image.Image = UIImage.FromFile(ConstantImages.SinInformacion);
                        _spinnerActivityIndicatorView.Message.Hidden = false;
                        _spinnerActivityIndicatorView.Message.TextColor = ConstantColor.UiMessageError;
                        _spinnerActivityIndicatorView.Message.Text = AppMessages.NoFoudnNotifications;

                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.NotificationsViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            //Load event of controls
        }

        private void LoadData()
        {
            notificationsTableView.Source = new NotificationsTableViewSource(AppNotifications, this);
            notificationsTableView.ReloadData();
            StopActivityIndicatorCustom();
        }

        private void LoadFonts()
        {
            //Load font size and style
        }

        private void LoadExternalViews()
        {
            try
            {
                notificationsTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.NotificationsViewCell, NSBundle.MainBundle), ConstantIdentifier.NotificationsViewCellIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        #endregion
    }
}

