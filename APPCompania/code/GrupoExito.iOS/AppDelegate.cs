using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.CoreKit;
using Firebase.Core;
using Foundation;
using Google.Maps;
using Google.TagManager;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Parameters.Products;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Services;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ConfigurationControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Plugin.LatestVersion;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;

namespace GrupoExito.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        #region Attributes
        private AddressModel _addressModel;
        #endregion

        #region Properties
        public override UIWindow Window { get; set; }
        private SBNotificationHub Hub { get; set; }
        #endregion

        #region Override Methods
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            try
            {
                ShowInitalView();
                GetAppVersion();
                if (ParametersManager.UserContext != null && ParametersManager.UserContext.Store == null)
                {
                    GetUserAddress();
                }
                RegisterCostumer();
                GetTaxesBag();
                TagManager.Configure();
                App.Configure();
                AnalyticsConfiguration.SharedInstance.SetAnalyticsCollectionEnabled(true);

                AppCenter.Start(AppConfigurations.IOSAppCenterId, typeof(Crashes));
                Couchbase.Lite.Support.iOS.Activate();

                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.BoxNumber, "");

                MapServices.ProvideAPIKey(AppConfigurations.ApiKeyGoogleMapsIos);

                FirebasePushNotificationManager.Initialize(launchOptions, false);
                CrossFirebasePushNotification.Current.RegisterForPushNotifications();

                ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);

                FilappController.Instance.SetFirebaseToken(CrossFirebasePushNotification.Current.Token);

                CrossFirebasePushNotification.Current.OnTokenRefresh += (source, e) =>
                {
                    var newTokenFirebase = Firebase.InstanceID.InstanceId.SharedInstance.Token;

                    NotificationHubService notificationHubService = new NotificationHubService(newTokenFirebase);
                    notificationHubService.RegisterNotification();

                    if (newTokenFirebase != null)
                    {
                        FilappController.Instance.SetFirebaseToken(newTokenFirebase);
                    }
                };
                LoadFirebaseEvents();
                GetProductsPrice();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AppDelegate, ConstantMethodName.FinishedLaunching);
            }

            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            bool handled = ApplicationDelegate.SharedInstance.OpenUrl(app, url, options);
            return handled;
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            NotificationHubService notificationHubService = new NotificationHubService(deviceToken);
            notificationHubService.RegisterNotification();
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            //FirebasePushNotificationManager.DidReceiveMessage(userInfo);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            //ProcessNotification(userInfo, false);
        }

        private void ProcessNotification(Plugin.FirebasePushNotification.Abstractions.FirebasePushNotificationDataEventArgs data)
        {
            if (null != data)
            {
                string dataSerialized = JsonService.Serialize(data);
                CashDrawerNotification notification = JsonService.Deserialize<CashDrawerNotification>(dataSerialized);
                if (notification.Body != null)
                {
                    ///TODO
                    /// Notificación normal
                }
                else if (!string.IsNullOrEmpty(notification.Data.Status))
                {
                    string boxNumber = notification.Data.BoxNumber;
                    if (!string.IsNullOrEmpty(boxNumber))
                    {
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.BoxNumber, notification.Data.BoxNumber);
                    }

                    string message = notification.Data.Message ?? "Nueva notificación";

                    if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active)
                    {
                        if (CheckIfCurrentViewIsTurnView())
                        {
                            UIViewController vc = (TurnViewController)GetTopViewController();
                            TurnViewController turnViewController = (TurnViewController)vc;
                            turnViewController.ExternalUpdateRequest();
                        }
                        else
                        {
                            ShowAlertController(message);
                        }
                    }
                    else
                    {
                        LaunchAlert(message);
                    }
                }
                else
                {
                    data.Data.TryGetValue("aps.alert.body", out object title);
                    LaunchAlert((string)title);
                }
            }
        }

        private void ShowAlertController(string msg)
        {
            var defaultAlert = UIAlertController.Create("Turno en caja", msg, UIAlertControllerStyle.Alert);
            defaultAlert.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, alert =>
            {
                //if (ParametersManager.GetTicket != null)
                //{
                //    TurnViewController turnViewController = this.Window.RootViewController.Storyboard.InstantiateViewController(ConstantControllersName.TurnViewController) as TurnViewController;
                //    turnViewController.Ticket = ParametersManager.GetTicket;
                //    var navigationController = (UINavigationController)this.Window.RootViewController;
                //    if (navigationController != null)
                //    {
                //        navigationController.NavigationBarHidden = false;
                //        navigationController.PushViewController(turnViewController, true);
                //    }
                //}
            }));
            defaultAlert.AddAction(UIAlertAction.Create(AppMessages.CancelButtonText, UIAlertActionStyle.Default, alert =>
            {
            }));

            GetTopViewController().PresentViewController(defaultAlert, true, null);
        }

        private string AddNameUserToTextNotification(string messageBody)
        {
            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.FirstName))
            {
                return ParametersManager.UserContext.FirstName + ", " + messageBody;
            }
            else
            {
                return messageBody;
            }
        }


        private bool CheckIfCurrentViewIsTurnView()
        {
            return (GetTopViewController() is TurnViewController);
        }

        private static UIViewController GetTopViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;

            if (vc is UITabBarController)
            {
                vc = ((UITabBarController)vc).SelectedViewController;
            }

            if (vc is UINavigationController navController)
                vc = navController.ViewControllers.Last();

            return vc;
        }

        private void LaunchAlert(string body)
        {
                var content = new UNMutableNotificationContent
                {
                    Title = AppMessages.TitleNotification,
                    Body = body,
                    Sound = UNNotificationSound.Default
                };
                
            var requestID = NSBundle.MainBundle.BundleIdentifier;
            var request = UNNotificationRequest.FromIdentifier(requestID, content, null);

                UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
                {
                    if (err != null)
                    {
                        // Do something with error...
                    }
                });
           
                //var notification = new UILocalNotification
                //{
                //    FireDate = NSDate.FromTimeIntervalSinceNow(1),
                //    AlertTitle = AppMessages.TitleNotification,
                //    AlertBody = body,
                //    SoundName = UILocalNotification.DefaultSoundName
                //};
                //UIApplication.SharedApplication.PresentLocalNotificationNow(notification);
        }

        public override void OnResignActivation(UIApplication application)
        {
        }

        public override void DidEnterBackground(UIApplication application)
        {
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }

        public override void OnActivated(UIApplication application)
        {
            AppEvents.ActivateApp();
        }

        public override void WillTerminate(UIApplication application)
        {
        }

        #endregion

        #region Methods
        private void LoadFirebaseEvents()
        {
            try
            {
                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    if (p.Data.Count != 0)
                    {
                        ProcessNotification(p);
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    InvokeOnMainThread(() =>
                    {
                        if (ParametersManager.GetTicket != null)
                        {
                            string dataSerialized = JsonService.Serialize(p);
                            CashDrawerNotification notification = JsonService.Deserialize<CashDrawerNotification>(dataSerialized);

                            if (notification.Body != null)
                            {
                                ///TODO
                                /// Notificación normal
                            }
                            else if (!string.IsNullOrEmpty(notification.Data.Status))
                            {
                                string message = notification.Data.Message ?? "Nueva notificación";
                            }


                            if (CheckIfCurrentViewIsTurnView())
                            {
                                UIViewController vc = (TurnViewController)GetTopViewController();
                                TurnViewController tvc = (TurnViewController)vc;
                                tvc.ExternalUpdateRequest();
                            }
                            else
                            {
                                TurnViewController turnViewController = this.Window.RootViewController.Storyboard.InstantiateViewController(ConstantControllersName.TurnViewController) as TurnViewController;
                                turnViewController.Ticket = ParametersManager.GetTicket;
                                GetTopViewController().NavigationController?.PushViewController(turnViewController, true);
                            }
                        }
                    });
                };
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AppDelegate, ConstantMethodName.LoadFirebaseEvents);
            }
        }

        private void ShowInitalView()
        {
            if (ParametersManager.UserContext != null && !ParametersManager.UserContext.IsAnonymous)
            {
                try
                {
                    UINavigationController initialAccessViewController = (UINavigationController)this.Window.RootViewController.Storyboard.InstantiateViewController(ConstantControllersName.InitialAccessNavigationViewController);
                    this.Window.RootViewController = initialAccessViewController;
                }
                catch (Exception exception)
                {
                    Util.LogException(exception, ConstantControllersName.UIViewControllerBase, ConstantMethodName.PresentLobbyView);
                }
            }
        }

        private async System.Threading.Tasks.Task GetAppVersion()
        {
            try
            {
                string identifier = NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"].ToString();
                string latestVersionAppStore = await CrossLatestVersion.Current.GetLatestVersionNumber(identifier);
                //CFBundleVersion
                string localVersion = NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString();
                if (!localVersion.Equals(latestVersionAppStore))
                {
                    AppVersionModel _appVersionModel = new AppVersionModel(new AppVersionService(DeviceManager.Instance));
                    var response = await _appVersionModel.GetAppVersion(ConstOSApp.IOS);
                    if (response.Result == null || (response.Result != null && !response.Result.HasErrors))
                    {
                        if (string.Compare(localVersion, response.MinimumVersion, StringComparison.CurrentCulture) < 0)
                        {
                            VersioningViewController versioningViewController = (VersioningViewController)this.Window.RootViewController.Storyboard.InstantiateViewController(ConstantControllersName.VersioningViewController);
                            versioningViewController.AppVersionResponse = response;
                            this.Window.RootViewController = versioningViewController;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaunchScreenController, ConstantMethodName.GetAppVersion);
            }
        }

        private async System.Threading.Tasks.Task GetUserAddress()
        {
            try
            {
                _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
                List<UserAddress> Addresses = await GetAddresses() as List<UserAddress>;
                UserContext userContext = ParametersManager.UserContext;
                if (Addresses.Any())
                {
                    bool hasAnAddress = Addresses != null && Addresses.Any() ? true : false;
                    bool hasAnAddressDefault = hasAnAddress && Addresses.Where(x => x.IsDefaultAddress == true) != null
                     && Addresses.Where(x => x.IsDefaultAddress == true).Any() ? true : false;
                    if (!hasAnAddress)
                    {
                        userContext.Address = null;
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                    }
                    else if (hasAnAddressDefault)
                    {
                        var address = Addresses.Where(x => x.IsDefaultAddress == true).FirstOrDefault();
                        userContext.Address = address;
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                    }
                    else
                    {
                        var address = Addresses.FirstOrDefault();
                        var response = await SetDefaultAddress(address);
                        userContext.Address = address;
                    }
                }
                else
                {
                    userContext.Address = null;
                }
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.GetAddress);
            }
        }

        public async System.Threading.Tasks.Task<bool> SetDefaultAddress(UserAddress address)
        {
            bool result = false;
            try
            {
                var response = await _addressModel.SetDefaultAddress(address);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                }
                else
                {
                    UserContext userContext = ParametersManager.UserContext;
                    userContext.Address = address;
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                    ParametersManager.UserContext = userContext;
                    ParametersManager.ContainChanges = true;
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public async System.Threading.Tasks.Task<IList<UserAddress>> GetAddresses()
        {
            IList<UserAddress> address = new List<UserAddress>();
            try
            {
                AddressResponse response = await _addressModel.GetAddress();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    address = null;
                }
                else
                {
                    if (response.Addresses != null && response.Addresses.Any())
                    {
                        address = response.Addresses;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.GetAddress);
            }
            return address;
        }


        private async System.Threading.Tasks.Task GetTaxesBag()
        {
            try
            {
                TaxesModel _taxesModel = new TaxesModel(new TaxesService(DeviceManager.Instance));
                var response = await _taxesModel.GetTaxBag().ConfigureAwait(false);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    //Mostrar mensaje
                }
                else
                {
                    if (response.TaxBags != null && response.TaxBags.Any())
                    {
                        ProductCarModel productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
                        productCarModel.UpSertTaxBag(response.TaxBags);
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseAddressController, ConstantMethodName.GetTaxesBag);
            }
        }

        public async System.Threading.Tasks.Task RegisterCostumer()
        {
            try
            {
                if (ParametersManager.UserContext != null && !ParametersManager.UserContext.IsAnonymous)
                {
                    bool userActivateClifre = DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.UserActivateClifre);
                    if (!userActivateClifre)
                    {
                        UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                        RegisterCostumerParameters parameters = ModelHelper.RegisterCostumerParameters(ParametersManager.UserContext);
                        var response = await userModel.RegisterCostumer(parameters);

                        if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                        {
                            //Show message
                        }
                        else
                        {
                            if (response.Activate)
                            {
                                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.UserActivateClifre, response.Activate);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.RegisterCostumer);
            }
        }

        protected void GetProductsPrice()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetProductsPriceAsync();
                });
            }
        }

        private async System.Threading.Tasks.Task GetProductsPriceAsync()
        {
            try
            {
                if (ParametersManager.UserContext != null)
                {
                    ProductCarModel DataBase = new ProductCarModel(ProductCarDataBase.Instance);
                    List<Product> products = DataBase.GetProducts();
                    if (products.Any())
                    {
                        ProductsPriceParameters parameters = new ProductsPriceParameters()
                        {
                            DependencyId = ParametersManager.UserContext.DependencyId,
                            SkuIds = products.Select(x => x.SkuId).ToList(),
                            ProductsType = products.Select(x => x.ProductType).ToList(),
                            Pums = products.Select(x => x.UnityPum).ToList(),
                            Factors = products.Select(x => x.FactorPum).ToList(),
                        };
                        ProductsModel _productsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
                        ProductsPriceResponse response = await _productsModel.GetProductsPrice(parameters);
                        if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                        {
                        }
                        else
                        {
                            List<Price> productsPrice = new List<Price>(response.Prices);
                            List<Product> productsDeleted = DataBase.UpdateProductsPrice(productsPrice, products);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AppDelegate, ConstantMethodName.GetProductsCategories);
            }
        }
        #endregion
    }
}


