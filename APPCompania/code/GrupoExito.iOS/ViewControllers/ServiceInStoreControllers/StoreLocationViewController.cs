using System;
using System.Collections.Generic;
using CoreGraphics;
using CoreLocation;
using Foundation;
using Google.Maps;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class StoreLocationViewController : BaseAddressController
    {
        #region Attributes
        private MapView mapView;
        private CLLocationManager LocationManager;
        public List<Store> Stores { get; set; }
        private bool firstLocationUpdate;
        private UISegmentedControl switcher;
        private MarkerInfoView markerInfoView;
        #endregion

        #region Constructors
        public StoreLocationViewController(IntPtr handle) : base(handle)
        {
            _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Stores, nameof(StoreLocationViewController));
        }

        public override void LoadView()
        {
            base.LoadView();

            try
            {
                LocationManager = new CLLocationManager
                {
                    ActivityType = CLActivityType.OtherNavigation
                };

                this.LoadExternalViews();

                if (DeviceManager.Instance.IsNetworkAvailable().Result)
                {
                    if (Stores != null)
                    {
                        DrawStoreLocation();
                    }
                }
                else
                {
                    this.StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {
                this.LoadHandlers();
                switcher = new UISegmentedControl(new[] { "Normal", "Satellite", "Hybrid", "Terrain" })
                {
                    AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleBottomMargin,
                    SelectedSegment = 0,
                    ControlStyle = UISegmentedControlStyle.Bar
                };
                View.AddSubview(switcher);
                switcher.ValueChanged += HandleValueChanged;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.ViewDidLoad);
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
                if (ParametersManager.UserContext == null || ParametersManager.UserContext.IsAnonymous)
                {
                    NavigationView.HiddenAccountProfile();
                }
                else
                {
                    NavigationView.ShowAccountProfile();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            try
            {
                base.ViewDidDisappear(animated);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.ViewDidDisappear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods 

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
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
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void DrawStoreLocation()
        {
            try
            {
                CameraPosition camera = CameraPosition.FromCamera(LocationManager.Location.Coordinate.Latitude, LocationManager.Location.Coordinate.Longitude, ConstantViewSize.DefaultStoresMapZoom);
                mapView = MapView.FromCamera(CGRect.Empty, camera);
                mapView.MyLocationEnabled = true;
                mapView.TrafficEnabled = true;
                mapView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                int position = 0;
                foreach (Store store in Stores)
                {
                    Marker storeMarker = new Marker()
                    {
                        Title = store.Name,
                        Icon = UIImage.FromBundle(ConstantImages.PinAlmacenes),
                        Snippet = "Dirección: " + store.Address
                                   + "\n" + "Horarios: " + store.Schedules,
                        Position = new CLLocationCoordinate2D(store.Latitude, store.Longitude),
                        Map = mapView,
                    };
                    storeMarker.ZIndex = position;
                    position++;
                }
                mapView.Settings.CompassButton = true;
                mapView.Settings.MyLocationButton = true;
                mapView.TappedMarker = (_mapView, marker) =>
                {
                    mapView.MarkerInfoWindow = new GMSInfoFor(HandleGMSInfoFor);
                    return false;
                };

                mapView.InfoTapped += (object sender, GMSMarkerEventEventArgs e) =>
                {
                    MapView mapViewSender = (MapView)sender;
                    if (markerInfoView != null)
                    {
                        CallWazeApp(mapViewSender.MyLocation.Coordinate.Latitude, mapViewSender.MyLocation.Coordinate.Longitude);
                    }
                };

                //mapView.AddObserver(this, new NSString(AppMessages.MyLocationText), NSKeyValueObservingOptions.New, IntPtr.Zero);
                View = mapView;
                InvokeOnMainThread(() => mapView.MyLocationEnabled = true);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.DrawStoreLocation);
                ShowMessageException(exception);
            }
        }

        public void CallWazeApp(double latitude, double longitude)
        {
            try
            {
                String url = "https://waze.com/ul?ll=" + StringFormat.DoubleToString(latitude) + "%2c" + StringFormat.DoubleToString(longitude) + "&navigate=yes&zoom=17";
                var sharedApp = UIApplication.SharedApplication;
                if (sharedApp.CanOpenUrl(new NSUrl(url)))
                {
                    sharedApp.OpenUrl(new NSUrl(url));
                }

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StoreLocationViewController, ConstantMethodName.CallWazeApp);
            }
        }

        #endregion

        #region Observers 

        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            if (!firstLocationUpdate)
            {
                firstLocationUpdate = true;
                var location = change.ObjectForKey(NSValue.ChangeNewKey) as CLLocation;
                mapView.Camera = CameraPosition.FromCamera(location.Coordinate, 20);
            }
        }

        #endregion

        #region Events
        void HandleValueChanged(object sender, EventArgs e)
        {
            var sw = sender as UISegmentedControl;

            switch (sw.SelectedSegment)
            {
                case 0:
                    mapView.MapType = MapViewType.Normal;
                    break;
                case 1:
                    mapView.MapType = MapViewType.Satellite;
                    break;
                case 2:
                    mapView.MapType = MapViewType.Hybrid;
                    break;
                case 3:
                    mapView.MapType = MapViewType.Terrain;
                    break;
                default:
                    break;
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            LoadView();
        }

        UIView HandleGMSInfoFor(MapView _mapView, Marker marker)
        {
            markerInfoView = MarkerInfoView.Create();
            CGRect markerInfoFrame = new CGRect(0, 0, customSpinnerView.Frame.Width, ConstantViewSize.MarkerInfoViewHeight);
            markerInfoView.Frame = markerInfoFrame;
            markerInfoView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            Store store = Stores[marker.ZIndex];
            markerInfoView.LoadData(store);
            return markerInfoView;
        }
        #endregion
    }
}

