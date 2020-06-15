using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLocation;
using Foundation;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Sources;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class ServiceInStoreViewController : BaseAddressController
    {
        #region Attributes
        private CashDrawerTurnModel cashDrawerTurnModel;
        private IList<MenuItem> menuItems;
        private CLLocationManager LocationManager;
        private int retryCoverageCount = 0;
        private IList<City> cities;
        private UserAddress _userAddress;
        private String DependencyId;
        private bool PushSend = false;
        private bool openFromInitialOption = false;
        #endregion

        #region Properties
        public bool OpenFromInitialOption { get => openFromInitialOption; set => openFromInitialOption = value; }
        #endregion

        #region Constructors
        public ServiceInStoreViewController(IntPtr handle) : base(handle)
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
                LocationManager = new CLLocationManager();
                this.LoadExternalViews();
                this.LoadHandlers();
                InvokeOnMainThread(async () =>
                {
                    await this.LoadData();
                });
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(!OpenFromInitialOption, false, true, this);
                NavigationView.HiddenCarData();

                if (ParametersManager.UserContext == null || ParametersManager.UserContext.IsAnonymous)
                {
                    NavigationView.HiddenAccountProfile();
                }
                else
                {
                    NavigationView.ShowAccountProfile();
                }

                retryCoverageCount = 0;
                PushSend = false;
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ServicesInStore, nameof(ServiceInStoreViewController));
                NavigationView.ChangeImageMyAccount(ConstantImages.Avatar);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SearchProductViewController, ConstantMethodName.ViewDidAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            try
            {
                if (LocationManager != null || (ValidateDependecy() == null && retryCoverageCount > 5))
                {
                    LocationManager.StopUpdatingLocation();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.ViewDidAppear);
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
            try
            {
                servicesTableView.RegisterNibForCellReuse(HeaderServiceInStoreViewCell.Nib, HeaderServiceInStoreViewCell.Key);
                servicesTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.MenuServicesTableViewCell, NSBundle.MainBundle), ConstantIdentifier.MenuServicesItemIdentifier);
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

        private async Task LoadData()
        {
            try
            {
                menuItems = JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuServicesInStoreSource);
                ServiceInStoreViewSource serviceInStoreViewSource = new ServiceInStoreViewSource(menuItems);
                servicesTableView.Source = serviceInStoreViewSource;
                serviceInStoreViewSource.SelectedAction += ServiceInStoreViewSourceSelectedAction;
                servicesTableView.RowHeight = UITableView.AutomaticDimension;
                servicesTableView.EstimatedRowHeight = 150;
                servicesTableView.ReloadData();
                if (DeviceManager.Instance.IsNetworkAvailable().Result)
                {
                    StartActivityIndicatorCustom();
                    cities = await LoadCitiesAddresses();
                    StopActivityIndicatorCustom();
                }
                else
                {
                    StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.LoadData);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            try
            {
                _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.LoadHandler);
                ShowMessageException(exception);
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            InvokeOnMainThread(async () =>
            {
                await this.LoadData();
            });
        }

        private void TryGetCurrentAddress()
        {
            StartActivityIndicatorCustom();
            LocationManager = new CLLocationManager
            {
                DesiredAccuracy = 0.1,
                DistanceFilter = 0.1,
                ActivityType = CLActivityType.OtherNavigation
            };
            LocationManager.StartUpdatingLocation();
        }

        private void ValidateCoverageResponse(CoverageAddressResponse response)
        {
            try
            {
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (retryCoverageCount > 5)
                    {
                        //Debe mostrar un alert indicando que no se encontro covertura 
                    }
                }
                else
                {
                    LocationManager.StopUpdatingLocation();
                    _userAddress = ModelHelper.SetCoverageInformation(response, _userAddress);
                    DependencyId = _userAddress.DependencyId;
                    PushSend = !PushSend ? true : false;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.ValidateCoverageResponse);
                ShowMessageException(exception);
            }
        }

        private UserAddress SetAddress(string address, string cityName)
        {
            String cityValue = cityName;
            string latitude = null;
            string longitud = null;
            City city = null;
            try
            {
                city = _addressModel.GetCity(Util.RemoveDiacritics(cityName.TrimStart().TrimEnd()), cities);
                if (city == null)
                {
                    city = Util.SearchCity(Util.RemoveDiacritics(cityName.TrimStart().TrimEnd()), cities);
                }
                cityValue = city != null ? city.Name : Util.RemoveDiacritics(cityName.TrimStart().TrimEnd());
                latitude = LocationManager.Location.Coordinate.Latitude.ToString();
                longitud = LocationManager.Location.Coordinate.Longitude.ToString();
            }
            catch
            {
                latitude = null;
                longitud = null;
            }
            return new UserAddress()
            {
                CityId = city != null ? city.Id : string.Empty,
                City = cityValue,
                AddressComplete = address,
                StateId = city != null ? city.State : string.Empty,
                Latitude = (latitude != null ? Decimal.Parse(latitude) : 0),
                Longitude = (longitud != null ? Decimal.Parse(longitud) : 0),
            };
        }

        #endregion

        #region Events
        private void ServiceInStoreViewSourceSelectedAction(object sender, EventArgs e)
        {
            string value = (string)sender;
            switch (value)
            {
                case ConstMenuServicesInStore.Discounts:
                    //if (RegistrationValidationViewController.ValidationIsNeeded())
                    //{
                    //    RegistrationValidationViewController validationViewController = new RegistrationValidationViewController();

                    //    validationViewController.OperationDoneAction += (result) =>
                    //    {
                    //        RemoveChild(validationViewController);

                    //        if (result)
                    //        {
                    //            MyDiscountViewController myDiscountViewController = (MyDiscountViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyDiscountViewController);
                    //            myDiscountViewController.HidesBottomBarWhenPushed = true;
                    //            this.NavigationController.PushViewController(myDiscountViewController, true);
                    //        }
                    //        else
                    //        {
                    //            ShowError(AppMessages.VerifyUserError);
                    //        }
                    //    };

                    //    validationViewController.OperationCanceledAction += () =>
                    //    {
                    //        RemoveChild(validationViewController);
                    //        this.NavigationController.NavigationBarHidden = false;
                    //    };

                    //    AddChildViewController(validationViewController);

                    //    this.NavigationController.NavigationBarHidden = true;
                    //    validationViewController.View.Frame = View.Bounds;

                    //    validationViewController.Cellphone = ParametersManager.UserContext.Cellphone;
                    //    View.AddSubview(validationViewController.View);
                    //    validationViewController.DidMoveToParentViewController(this);
                    //}
                    //else
                    //{
                        MyDiscountViewController myDiscountViewController = (MyDiscountViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MyDiscountViewController);
                        myDiscountViewController.HidesBottomBarWhenPushed = true;
                        this.NavigationController.PushViewController(myDiscountViewController, true);
                    //}
                    break;
                case ConstMenuServicesInStore.CashDrawerTurn:
                    Ticket ticket = ParametersManager.GetTicket;
                    if (ticket == null)
                    {
                        ShiftInBoxViewController shiftInBoxViewController = (ShiftInBoxViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ShiftInBoxViewController);
                        shiftInBoxViewController.HidesBottomBarWhenPushed = true;
                        this.NavigationController.PushViewController(shiftInBoxViewController, true);
                    }
                    else
                    {
                        TurnViewController turnViewController = this.Storyboard.InstantiateViewController(ConstantControllersName.TurnViewController) as TurnViewController;
                        turnViewController.Ticket = ticket;
                        turnViewController.HidesBottomBarWhenPushed = true;
                        ShiftInBoxViewController shiftInBoxViewController = (ShiftInBoxViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ShiftInBoxViewController);
                        this.NavigationController.PushViewController(turnViewController, true);
                    }
                    break;
                case ConstMenuServicesInStore.PriceChecker:
                    CheckerFeature();
                    break;
                case ConstMenuServicesInStore.Stores:
                    StoreFeature();
                    break;
            }
        }

        private void RemoveChild(UIViewController vc)
        {
            vc.WillMoveToParentViewController(null);
            vc.View.RemoveFromSuperview();
            vc.RemoveFromParentViewController();
        }

        private void ShowError(string message)
        {
            InvokeOnMainThread(() =>
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure("", message,
                    (errorSender, ea) => errorView.RemoveFromSuperview());
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            });
        }

        private void CheckerFeature()
        {
            if (IsLocationEnabled())
            {
                SelectStorePriceCheckerViewController selectStorePriceCheckerView = (SelectStorePriceCheckerViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.SelectStorePriceCheckerViewController);
                selectStorePriceCheckerView.HidesBottomBarWhenPushed = true;
                this.NavigationController.PushViewController(selectStorePriceCheckerView, true);
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

        private async Task StoreFeature()
        {
            StartActivityIndicatorCustom();
            if (IsLocationEnabled())
            {
                List<Store> stores = await GetStores();
                if (stores != null)
                {
                    if (stores.Any())
                    {
                        StoreLocationViewController storeLocationViewController = (StoreLocationViewController)Storyboard.InstantiateViewController(ConstantControllersName.StoreLocationViewController);
                        storeLocationViewController.HidesBottomBarWhenPushed = true;
                        storeLocationViewController.Stores = stores;
                        NavigationController.PushViewController(storeLocationViewController, true);
                        StopActivityIndicatorCustom();
                    }
                    else
                    {
                        ///TODO
                        /// Service failed
                    }
                }
                else
                {
                    StopActivityIndicatorCustom();
                    ShowStoresRelatedAlert("Hubo un problema al visualizar las sedes, por favor intente más tarde", (result) => { }, null);
                }
            }
            else if (CLLocationManager.Status.Equals(CLAuthorizationStatus.NotDetermined))
            {
                StopActivityIndicatorCustom();
                LocationManager.RequestWhenInUseAuthorization();
            }
            else
            {
                StopActivityIndicatorCustom();
                ShowStoresRelatedAlert(AppMessages.AppLocationMessage, (result) => { },
                (result) =>
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
                });
            }
        }

        #region Delegates
        public delegate void AlertOKCancelDelegate(bool OK);
        #endregion

        private bool IsLocationEnabled()
        {
            return (CLLocationManager.Status.Equals(CLAuthorizationStatus.AuthorizedAlways) || CLLocationManager.Status.Equals(CLAuthorizationStatus.AuthorizedWhenInUse));
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
            PresentViewController(alertController, true, null);
        }

        private async Task<List<Store>> GetStores()
        {
            try
            {
                SearchStoresParameters parameters = new SearchStoresParameters { Latitude = LocationManager.Location.Coordinate.Latitude, Longitude = LocationManager.Location.Coordinate.Longitude };
                List<Store> Stores = new List<Store>();
                var response = await _addressModel.GetStores(parameters);

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
                    Stores = new List<Store>();
                    Stores.AddRange(response.Stores);
                }

                return Stores;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ServiceInStoreViewController, ConstantMethodName.GetStores);
                return null;
            }
        }
        #endregion
    }
}

