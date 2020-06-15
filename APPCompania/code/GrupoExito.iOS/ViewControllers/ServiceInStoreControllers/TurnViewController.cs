using System;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class TurnViewController : BaseTicketController
    {
        #region Attributes  
        private ShiftInBoxView shiftInBoxView;
        private NSTimer Timer;
        private int CountSegTimer = int.Parse(AppConfigurations.TicketTimerTime);
        #endregion

        #region Properties
        public Ticket Ticket { get; set; }
        #endregion

        #region Constructors
        public TurnViewController(IntPtr handle) : base(handle)
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
                this.LoadHandlers();
                this.ValidateStatusTurnCurrent();
                this.CountSegTimer = int.Parse(AppConfigurations.TicketTimerTime);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.IsAccountEnabled = false;
                this.NavigationController.NavigationBarHidden = false;
                NavigationView.ChangeImageMyAccount(ConstantImages.CambioPrimario);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            if (NavigationView != null)
            {
                NavigationView.ChangeImageMyAccount(ConstantImages.Avatar);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.CashDrawerTurn, nameof(TurnViewController));
                Timer = NSTimer.CreateRepeatingScheduledTimer(1, UpdatingStatusTurn);
                Timer.Fire();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            try
            {
                if (Timer != null && Timer.IsValid)
                {
                    Timer.Invalidate();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.ViewDidDisappear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods

        private void LoadExternalViews()
        {
            LoadNavigationView(this.NavigationController.NavigationBar);
            this.NavigationController.NavigationBar.Hidden = false;
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            CustomSpinnerViewFromBase = customSpinnerView;
            SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            turnView.LayoutIfNeeded();

            shiftInBoxView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.ShiftInBoxView, Self, null).GetItem<ShiftInBoxView>(0);
            CGRect shiftInBoxFrame = new CGRect(0, 0, turnView.Frame.Size.Width, turnView.Frame.Size.Height);
            shiftInBoxView.Frame = shiftInBoxFrame;
            turnView.AddSubview(shiftInBoxView);
        }

        public void ValidateStatusTurnCurrent()
        {
            if (Ticket.Id != null)
            {
                TurnStatusAsync();
            }
            else
            {
                GetTurnAsync();
            }
        }

        public async Task GetTurnAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                TicketResponse response = await cashDrawerTurnModel.Ticket(Ticket);

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
                    Ticket.Name = response.Name;
                    Ticket.Id = response.Id;
                    response.AvgWaitTime = 0;
                    shiftInBoxView.Loadticket(response);
                    turnLabel.Text = response.Name;
                    //boxLabel.Text = String.Format(boxLabel.Text, DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber));
                    string boxNumber = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber);
                    if (string.IsNullOrEmpty(boxNumber))
                    {
                        boxLabel.Hidden = true;
                    }
                    else
                    {
                        boxLabel.Text = String.Format($"Caja {boxNumber}");

                    }
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Ticket, JsonService.Serialize(Ticket));
                }

                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.GetHomeProducts);
            }
        }

        public async Task TurnStatusAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                TicketResponse response = await cashDrawerTurnModel.StatusTicket(Ticket);

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
                    if (response.Status == ConstStatusTicket.Finished || response.Status == ConstStatusTicket.Dumped || response.Status == ConstStatusTicket.AutoDumped)
                    {
                        turnView.Hidden = true;
                        shiftInBoxView.TurnFinished();
                        imageTicket.Image = UIImage.FromFile(ConstantImages.Turnoatendido);
                        youTurnLabel.Text = AppMessages.TurnText;
                        turnLabel.Text = AppMessages.AttendedText;
                        turnButton.SetTitle(AppMessages.TurnButton, UIControlState.Normal);
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.BoxNumber, "");
                        turnLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TextTurnSize);
                        turnButton.Hidden = false;
                        noteLabel.Hidden = true;
                        noteMessageLabel.Hidden = true;
                        boxLabel.Hidden = true;
                        updateLabel.Hidden = true;

                        if (Timer != null && Timer.IsValid)
                        {
                            Timer.Invalidate();
                        }

                        DeviceManager.Instance.DeleteAccessPreference(ConstPreferenceKeys.Ticket);
                    }
                    else if (response.Status == ConstStatusTicket.Serving)
                    {
                        turnView.Hidden = false;
                        shiftInBoxView.RemoveFromSuperview();
                        statusCurrentTurnLabel.Hidden = false;
                        turnButton.Hidden = true;
                        noteLabel.Hidden = true;
                        noteMessageLabel.Hidden = true;
                        turnLabel.Text = response.Name;
                        //boxLabel.Text = String.Format(boxLabel.Text, DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber));
                        string boxNumber = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber);
                        if (string.IsNullOrEmpty(boxNumber))
                        {
                            if (!string.IsNullOrEmpty(response.SlotDisplayName))
                            {
                                boxNumber = response.SlotDisplayName;
                            }
                        }
                        if (string.IsNullOrEmpty(boxNumber))
                        {
                            boxLabel.Hidden = true;
                        }
                        else
                        {
                            boxLabel.Text = String.Format($"Caja {boxNumber}");

                        }

                        turnView.Layer.BorderColor = ConstantColor.UiBorderTurnStatus.CGColor;
                        turnView.Layer.BorderWidth = ConstantStyle.BorderWidth;
                        turnView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    }
                    else
                    {
                        turnView.Hidden = false;
                        Ticket.Name = response.Name;
                        shiftInBoxView.Loadticket(response);
                        turnLabel.Text = response.Name;
                        //boxLabel.Text = String.Format(boxLabel.Text, DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber));
                        string boxNumber = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber);
                        if (string.IsNullOrEmpty(boxNumber))
                        {
                            if (!string.IsNullOrEmpty(response.SlotDisplayName))
                            {
                                boxNumber = response.SlotDisplayName;
                                boxLabel.Hidden = false;
                            }
                            else
                            {
                                boxLabel.Hidden = true;
                            }
                        }
                        else
                        {
                            boxLabel.Hidden = false;
                            boxLabel.Text = String.Format($"Caja {boxNumber}");
                        }

                        turnButton.Hidden = false;
                    }

                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        public void ExternalUpdateRequest()
        {
            CountSegTimer = int.Parse(AppConfigurations.TicketTimerTime);
            updateLabel.Text = AppMessages.UpdatingMessage;
            this.TurnStatusAsync();
        }

        public async Task CancelTurnAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                TicketResponse response = await cashDrawerTurnModel.CancelTicket(Ticket);

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
                    if (response.Success != false)
                    {
                        DeviceManager.Instance.DeleteAccessPreference(ConstPreferenceKeys.Ticket);
                        this.NavigationController.PopViewController(true);
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.GetHomeProducts);
            }
            finally
            {
                StopActivityIndicatorCustom();
            }
        }

        private void Alert()
        {
            var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.CancelTurn, UIAlertControllerStyle.Alert);
            var cancel = UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Cancel, null);
            var aceppt = UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action =>
            {
                CancelTurnAsync();
            });
            alertController.AddAction(aceppt);
            alertController.AddAction(cancel);
            this.PresentViewController(alertController, true, null);
        }


        private void LoadHandlers()
        {
            turnButton.TouchUpInside += TurnButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            NavigationView.Profile.TouchUpInside += UpdateTurnStatusUpInside;
        }

        private void UpdateTurnStatusUpInside(object sender, EventArgs e)
        {
            this.ValidateStatusTurnCurrent();
        }

        private void TurnButtonTouchUpInside(object sender, EventArgs e)
        {
            if (turnButton.Title(UIControlState.Normal).Equals(AppMessages.TurnButton))
            {
                DeviceManager.Instance.DeleteAccessPreference(ConstPreferenceKeys.Ticket);
                ShiftInBoxViewController shiftInBoxViewController = (ShiftInBoxViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ShiftInBoxViewController);
                shiftInBoxViewController.HidesBottomBarWhenPushed = true;
                this.NavigationController.PopViewController(true);
            }
            else
            {
                this.Alert();
            }
        }
        #endregion

        #region Events
        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.ValidateStatusTurnCurrent();
        }

        private void UpdatingStatusTurn(NSTimer obj)
        {
            if (CountSegTimer == 0)
            {
                CountSegTimer = int.Parse(AppConfigurations.TicketTimerTime);
                updateLabel.Text = AppMessages.UpdatingMessage;
                this.TurnStatusAsync();
            }
            else
            {
                CountSegTimer--;
                updateLabel.Text = string.Format(AppMessages.UpdatingTimerMessage, CountSegTimer);
            }
        }
        #endregion
    }
}

