using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Source;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class ShiftInBoxViewController : UIViewControllerBase
    {
        #region Attributes
        private PopPupShiftlnBoxView popPupShiftlnBoxView;
        private CashDrawerTurnModel cashDrawerTurnModel;
        private IList<StoreCashDrawerTurn> GetStoreCashDrawerTurns;
        private string MobileId;
        private StatusCashDrawerTurn StatusCashDrawerTurn;
        #endregion

        #region Constructors
        public ShiftInBoxViewController(IntPtr handle) : base(handle)
        {
            cashDrawerTurnModel = new CashDrawerTurnModel(new CashDrawerTurnService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.CashDrawerRequestTurn, nameof(ShiftInBoxViewController));
                this.LoadExternalViews();
                this.LoadCorners();
                this.ValidateButton();
                this.LoadHandlers();
                this.AvailableDeviceAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.ViewDidLoad);
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
                NavigationView.ShowAccountProfile();
                NavigationView.IsAccountEnabled = true;
                NavigationView.ChangeImageMyAccount(ConstantImages.Avatar);
                if (ParametersManager.GetTicket != null)
                {
                    this.NavigationController.PopViewController(false);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            FilappController.Instance.FirebaseTokenRefreshed += Instance_FirebaseTokenRefreshed;

        }

        public override void ViewDidDisappear(bool animated)
        {
            FilappController.Instance.FirebaseTokenRefreshed -= Instance_FirebaseTokenRefreshed;
            base.ViewDidDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            GC.Collect();
        }
        #endregion


        public async Task AvailableDeviceAsync()
        {
            StartActivityIndicatorCustom();
            bool result = await FilappController.Instance.CheckFilappRegistration();
            if (result)
            {
                GetStoresListAsync();
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.ErrorServiceUnavailable.ToString(), AppMessages.ErrorServicesUnavailables);
            }
        }

        void Instance_FirebaseTokenRefreshed()
        {
            this.AvailableDeviceAsync();
        }

        public async Task GetStoresListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                StoreCashDrawerTurnResponse response = await cashDrawerTurnModel.GetStores();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        String message = MessagesHelper.GetMessage(response.Result);
                        if (!string.IsNullOrEmpty(message))
                        {
                            StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                        }
                    }
                }
                else
                {
                    if (response.Stores != null && response.Stores.Any())
                    {
                        this.GetStoreCashDrawerTurns = response.Stores;
                    }
                    this.DrawStoresList();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        private void DrawStoresList()
        {
            try
            {
                if (GetStoreCashDrawerTurns != null && GetStoreCashDrawerTurns.Any())
                {
                    CityStorePickerViewDelegate delegateCityStore = new CityStorePickerViewDelegate(GetStoreCashDrawerTurns, selectStoreTextField);
                    genericPickerView.Delegate = delegateCityStore;
                    genericPickerView.DataSource = new CityStorePickerViewDataSource(GetStoreCashDrawerTurns);
                    genericPickerView.ReloadAllComponents();
                    delegateCityStore.Selected(genericPickerView, 0, 0);
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.NotFoundStoreInShiftInBox, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        public async Task StatusCashDrawerTurnListAsync()
        {
            try
            {
                int pos = (int)genericPickerView.SelectedRowInComponent(0);
                StatusCashDrawerTurn = new StatusCashDrawerTurn()
                {
                    StoreId = GetStoreCashDrawerTurns[pos].Id,
                };
                StatusCashDrawerTurnResponse response = await cashDrawerTurnModel.StatusCashDrawerTurn(StatusCashDrawerTurn);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        String message = MessagesHelper.GetMessage(response.Result);
                        if (!string.IsNullOrEmpty(message))
                        {
                            StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                        }
                    }
                }
                else
                {
                    _spinnerActivityIndicatorView.Hidden = true;
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.PopPupShiftlnBoxViewHeight);
                    popPupShiftlnBoxView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.PopPupShiftlnBoxView, Self, null).GetItem<PopPupShiftlnBoxView>(0);
                    popPupShiftlnBoxView.Frame = new CGRect(0, 0, customSpinnerView.Frame.Width, customSpinnerView.Frame.Height);
                    popPupShiftlnBoxView.LayoutIfNeeded();
                    customSpinnerView.LayoutIfNeeded();
                    customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    customSpinnerView.AddSubview(popPupShiftlnBoxView);
                    popPupShiftlnBoxView.accept.TouchUpInside += AcceptTouchUpInside;
                    popPupShiftlnBoxView.refuse.TouchUpInside += RefuseTouchUpInside;
                    popPupShiftlnBoxView.LoadData(response);
                    customSpinnerView.Hidden = false;
                    spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.3f);
                    spinnerActivityIndicatorView.StartAnimating();
                    ModalRequestTurnScreenView();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        private void ModalRequestTurnScreenView()
        {
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ModalRequestTurn, nameof(ShiftInBoxViewController));
        }

        #region Methods 
        private void ValidateButton()
        {
            askTurnButton.Enabled = false;
            askTurnButton.BackgroundColor = UIColor.LightGray;
        }

        private void LoadHandlers()
        {
            selectStoreButton.TouchUpInside += SelectButtonTouchUpInside;
            askTurnButton.TouchUpInside += TurnButtonTouchUpInside;
            closeGenericPickerButton.TouchUpInside += CloseStorePickerButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void LoadCorners()
        {
            try
            {
               
                selectStoreView.Layer.BorderColor = UIColor.LightGray.ColorWithAlpha(0.3f).CGColor;
                selectStoreView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                labelCorner.ClipsToBounds = true;
                labelCorner.Layer.CornerRadius = ConstantStyle.CornerRadius;
                labelCorner.Layer.MaskedCorners = CoreAnimation.CACornerMask.MaxXMinYCorner | CoreAnimation.CACornerMask.MinXMinYCorner;
                labelCorner.ClipsToBounds = true;
                askTurnButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ShiftInBoxViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private void CloseStorePickerButtonTouchUpInside(object sender, EventArgs e)
        {
            containerGenericPickerView.Hidden = true;
            int pos = (int)genericPickerView.SelectedRowInComponent(0);
            if (GetStoreCashDrawerTurns[pos].Id != 0.ToString()) {
                askTurnButton.Enabled = true;
                askTurnButton.BackgroundColor = ConstantColor.UiPrimary;
            }
            else 
            {
                //askTurnButton.BackgroundColor = new UIColor(0.0f, 122.0f / 255f, 1.0f, 1.0f);
                askTurnButton.BackgroundColor = UIColor.LightGray;
                askTurnButton.Enabled = false;
            }

        }

        private void TurnButtonTouchUpInside(object sender, EventArgs e)
        {

            if (!selectStoreTextField.Text.TrimEnd().TrimStart().Equals(string.Empty))
            {
                StartActivityIndicatorCustom();
                this.StatusCashDrawerTurnListAsync();
            }
            else
            {
                askTurnButton.Enabled = true;
            }
        }

        private void SelectButtonTouchUpInside(object sender, EventArgs e)
        {
            containerGenericPickerView.Hidden = false;

        }

        private void AcceptTouchUpInside(object sender, EventArgs e)
        {
            TurnViewController turnViewController = this.Storyboard.InstantiateViewController(ConstantControllersName.TurnViewController) as TurnViewController;
            Ticket ticket = ParametersManager.GetTicket;
            if (ticket == null)
            {
                ticket = new Ticket()
                {
                    MobileId = DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.MobileId),
                    StoreId = StatusCashDrawerTurn.StoreId
                };
            }
            turnViewController.Ticket = ticket;
            this.NavigationController.PushViewController(turnViewController, true);
            popPupShiftlnBoxView.RemoveFromSuperview();
            customSpinnerView.Hidden = true;
            spinnerActivityIndicatorView.StopAnimating();
            spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            _spinnerActivityIndicatorView.Hidden = false;
        }

        private void RefuseTouchUpInside(object sender, EventArgs e)
        {
            popPupShiftlnBoxView.RemoveFromSuperview();
            customSpinnerView.Hidden = true;
            spinnerActivityIndicatorView.StopAnimating();
            spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            _spinnerActivityIndicatorView.Hidden = false;
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.AvailableDeviceAsync();
        }
        #endregion
    }
}