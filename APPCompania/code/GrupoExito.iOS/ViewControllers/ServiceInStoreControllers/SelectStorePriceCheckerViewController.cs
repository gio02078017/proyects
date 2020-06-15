using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.InStoreServices;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Source;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class SelectStorePriceCheckerViewController : UIViewControllerBase
    {

        #region Attributes
        private CheckerPriceModel _checkerPriceModel;
        private int PosSelectCity = -1;
        private int PosSelectStore = -1;
        private IList<Store> Stores;
        private IList<City> Cities;
        private CLLocationManager LocationManager;
        #endregion

        #region Constructors
        public SelectStorePriceCheckerViewController(IntPtr handle) : base(handle)
        {
            _checkerPriceModel = new CheckerPriceModel(new CheckerPriceService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {           
            this.LoadExternalViews();
            base.ViewDidLoad();
            this.LoadExternalViews();
            this.LoadHandlers();
            this.LoadCorners();
            this.GetStoresList();
            this.ValidateEnabledContinueButton(null, null);
            LocationManager = new CLLocationManager();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.ShowAccountProfile();
                NavigationView.HiddenCarData();
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailPriceCheckerViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.PriceCheckerLocation, nameof(SelectStorePriceCheckerViewController));
        }


        #endregion

        #region Methods 
        public void GetStoresList()
        {
            InvokeOnMainThread(async () =>
            {
                await this.GetCitiesListAsync();
            });
        }



        public async Task GetStoresListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                StoreResponse response = await _checkerPriceModel.GetStores(Cities[PosSelectCity].Id);
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
                    Stores = response.Stores;
                    if (Stores.Count > 1)
                    {
                        PosSelectStore = 0;
                        CityStorePickerViewDelegate cityAddressStoreDelegate = new CityStorePickerViewDelegate(Stores, selectStoreTextField);
                        cityAddressStoreDelegate.Action += StoreSelectedDelegateAction;
                        genericPickerView.Delegate = cityAddressStoreDelegate;
                        genericPickerView.DataSource = new CityStorePickerViewDataSource(Stores);
                        genericPickerView.ReloadAllComponents();
                        genericPickerView.Select(0, 0, true);
                        containerGenericPickerView.Hidden = false;
                        StopActivityIndicatorCustom();
                    }
                    else
                    {
                        StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.NotCitiesOrStores);
                        _spinnerActivityIndicatorView.Retry.Hidden = false;
                    }
                }

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SelectStorePriceCheckerViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        public async Task GetCitiesListAsync()
        {
            try
            {
                StartActivityIndicatorCustom();
                CitiesResponse response = await _checkerPriceModel.GetCities();

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
                    Cities = response.Cities;

                    if (Cities != null)
                    {
                        CityStorePickerViewDelegate cityAddressStoreDelegate = new CityStorePickerViewDelegate(Cities, selectCityTextField);
                        cityAddressStoreDelegate.Action += CitiesSelectedDelegateAction;
                        genericPickerView.Delegate = cityAddressStoreDelegate;
                        genericPickerView.DataSource = new CityStorePickerViewDataSource(Cities);
                        genericPickerView.ReloadAllComponents();
                        cityAddressStoreDelegate.Selected(genericPickerView, 0, 0);
                    }
                    StopActivityIndicatorCustom();
                }

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SelectStorePriceCheckerViewController, ConstantMethodName.GetHomeProducts);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.DetailPriceCheckerViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            continueButton.TouchUpInside += ContinueButtonTouchUpInside;
            selectCityButton.TouchUpInside += SelectCityButtonTouchUpInside;
            selectStoreButton.TouchUpInside += SelectStoreButtonTouchUpInside;
            closeGenericPickerButton.TouchUpInside += CloseGenericPickerButtonTouchUpInside;
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
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private async void ContinueButtonTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                bool isPositionCoverage = await RequestPositionValidation();
                if (isPositionCoverage)
                {
                    CameraViewController priceCheckerViewController = (CameraViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.CameraViewController);
                    priceCheckerViewController.HidesBottomBarWhenPushed = true;
                    priceCheckerViewController.DependencyId = Stores[PosSelectStore].Id;
                    priceCheckerViewController.DependecyName = Stores[PosSelectStore].DependencyName;
                    this.NavigationController.PushViewController(priceCheckerViewController, true);
                }
                else
                {
                    ShowMessage("", AppMessages.PriceCheckNotCoverage);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SelectStorePriceCheckerViewController, ConstantMethodName.LoadControllers);
            }
        }

        private void SelectCityButtonTouchUpInside(object sender, EventArgs e)
        {
            if (Cities != null && Cities.Any())
            {
                PosSelectCity = -1;
                PosSelectStore = -1;
                selectStoreTextField.Text = string.Empty;
                CityStorePickerViewDelegate cityAddressStoreDelegate = new CityStorePickerViewDelegate(Cities, selectCityTextField);
                cityAddressStoreDelegate.Action += CitiesSelectedDelegateAction;
                genericPickerView.Delegate = cityAddressStoreDelegate;
                genericPickerView.DataSource = new CityStorePickerViewDataSource(Cities);
                genericPickerView.ReloadAllComponents();
                cityAddressStoreDelegate.Selected(genericPickerView, 0, 0);
                containerGenericPickerView.Hidden = false;
            }
            else
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, "No se encontraron ciudades disponibles en este momento, por favor intente mas tarde", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
        }

        private async void SelectStoreButtonTouchUpInside(object sender, EventArgs e)
        {
            if (PosSelectCity > 0)
            {
                await GetStoresListAsync();
            }
            else
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, "Por favor selecciona una ciudad", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
        }

        private void CloseGenericPickerButtonTouchUpInside(object sender, EventArgs e)
        {
            if (PosSelectCity > 0 && PosSelectStore == -1)
            {
                SelectStoreButtonTouchUpInside(sender, e);
            }
            containerGenericPickerView.Hidden = true;
        }

        private void CitiesSelectedDelegateAction(object sender, EventArgs e)
        {
            PosSelectCity = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(sender, e);
        }

        private void StoreSelectedDelegateAction(object sender, EventArgs e)
        {
            PosSelectStore = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(sender, e);
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            this.GetStoresList();
        }

        private void ValidateEnabledContinueButton(object sender, EventArgs e)
        {
            if (PosSelectCity > 0 && PosSelectStore > 0)
            {
                continueButton.Enabled = true;
                continueButton.Layer.BackgroundColor = ConstantColor.UiPrimary.CGColor;
            }
            else
            {
                continueButton.Enabled = false;
                continueButton.Layer.BackgroundColor = UIColor.Gray.ColorWithAlpha(0.3f).CGColor;
            }
        }

        private async Task<bool> RequestPositionValidation()
        {
            StartActivityIndicatorCustom();
            CheckerPriceCoverageParameters parameters = new CheckerPriceCoverageParameters()
            {
                DependencyId = Stores[PosSelectStore].Id,
                Latitude = LocationManager.Location.Coordinate.Latitude,
                Longitude = LocationManager.Location.Coordinate.Longitude
            };

            CheckerPriceCoverageResponse response = await _checkerPriceModel.CheckerPriceCoverage(parameters);

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
                StopActivityIndicatorCustom();
            }

            return response.HaveCoverage;
        }

        private void ShowMessage(string title, string message)
        {
            InvokeOnMainThread(() =>
            {
                PopUpInformationView messageView = PopUpInformationView.Create(title, message);
                this.NavigationController.SetNavigationBarHidden(true, true);
                messageView.Frame = View.Bounds;
                messageView.LayoutIfNeeded();
                View.AddSubview(messageView);

                messageView.AcceptButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    messageView.RemoveFromSuperview();
                };
                messageView.CloseButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    messageView.RemoveFromSuperview();
                };
            });
        }
        #endregion
    }
}

