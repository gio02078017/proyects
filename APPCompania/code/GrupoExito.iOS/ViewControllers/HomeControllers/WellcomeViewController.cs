using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using Foundation;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Source;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    public partial class WellcomeViewController : BaseAddressController
    {
        #region Attributes
        private UserCredentials _userInfo { get; set; }
        private UserAddress UserAddress;
        private Store UserStore { get; set; }
        private CLLocationManager LocationManager;
        private NSTimer Timer;
        private IList<TextGallery> GalleryTexts { get; set; }
        private IList<City> Cities;
        private IList<string> Predictions;
        private IList<City> CitiesStore { get; set; }
        private IList<Store> Store;
        private TutorialView tutorialView = null;
        private float CountPredictionPrevious = 0;
        private int PosSelectCity = -1;
        private int PosSelectStore = -1;
        private int TamHiddenService = 70;
        private int TamShowService = 270;
        private int TamShowServiceTitle = 60;
        private int TamCoverageStatusView = 400;
        private int TamShowAutocomplete = 150;
        private int TamHiddenAutocomplete = 0;
        private int NumberSlider = 0;
        #endregion

        #region Properties
        public void SetUserInfo(UserCredentials userInfo)
        {
            _userInfo = userInfo;
        }
        #endregion

        #region Constructors
        public WellcomeViewController(IntPtr handle) : base(handle) { }
        #endregion

        #region View lifecycle

        public override void ViewDidLoad()
        {          
            base.ViewDidLoad();
            try
            {
                IList<Tutorial> tutorialesList = new ContentsModel(new ContentsService(DeviceManager.Instance)).GetTutorials();
                foreach (Tutorial current in tutorialesList)
                {
                    if (ConstNameViewTutorial.Welcome.Equals(current.Name))
                    {
                        this.CreateTutorialView(this.View, tutorialView, current.ImagesTutorial, 0, false);
                    }
                }

                ValidateNetWorkingAsync();

                if (ParametersManager.UserContext.CellPhone == null) 
                {
                    ParametersManager.UserContext.CellPhone = "0000000000";
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            try
            {
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Welcome, nameof(WellcomeViewController));
                Timer = NSTimer.CreateRepeatingScheduledTimer(5, WelcomeTimerAction);
                Timer.Fire();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.ViewDidAppear);
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
                UnsubscribeHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.ViewDidDisappear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region methods 
        private async Task ValidateNetWorkingAsync()
        {
            this.LoadExternalViews();
            this.LoadHandlers();
            //this.LoadFonts();
            this.LoadCorners();
            StartActivityIndicatorCustom();
            toHomeViewHeightConstraint.Constant = TamHiddenService;
            toStoreViewHeightConstraint.Constant = TamHiddenService;
            HiddenShowControlsToHomeView(true);
            HiddenShowControlsToStoreView(true);
            GalleryTexts = new List<TextGallery>();
            GalleryTexts = JsonService.Deserialize<List<TextGallery>>(AppConfigurations.TextGalleryWelcomeSource);
            containerGenericPickerView.Hidden = true;

            if (GalleryTexts.Count > 0)
            {
                wellcomeTitleLabel.Text = GalleryTexts[NumberSlider].Title;
                messageContentLabel.Text = GalleryTexts[NumberSlider].Description;
                messageContentPageControl.Pages = GalleryTexts.Count;
                MessageContentPageControl_EditingChanged(messageContentPageControl, null);
            }

            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.LoadDataAsync();
                });
                this.InitializateTable();
                StopActivityIndicatorCustom();
                ValidateEnabledContinueButton(ConstantReusableViewName.ToHomeSelected, null);
                ValidateEnabledContinueButton(ConstantReusableViewName.ToStoreSelected, null);
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
            customSpinnerHomeView.Hidden = true;
            customSpinnerStoreView.Hidden = true;
            customSpinnerHomeView.StopAnimating();
            customSpinnerStoreView.StopAnimating();
            spinnerActivityIndicatorViewToHome.StopAnimating();
            spinnerActivityIndicatorViewToStore.StopAnimating();
            spinnerActivityIndicatorViewToHome.Hidden = true;
            spinnerActivityIndicatorViewToStore.Hidden = true;
        }

        private void CustomPageControl()
        {
            try
            {
                var x = 0;
                var width = 15;
                var height = 15;

                for (int i = 0; i < messageContentPageControl.Subviews.Length; i++)
                {
                    UIView dot = messageContentPageControl.Subviews[i];
                    dot.Frame = new CGRect(x, dot.Frame.Y, width, height);
                    dot.Layer.BorderWidth = 1;
                    dot.Layer.CornerRadius = dot.Frame.Width / 2;
                    dot.Layer.BorderColor = ConstantColor.UiPageControlDot.CGColor;

                    if (i == messageContentPageControl.CurrentPage)
                    {
                        dot.BackgroundColor = ConstantColor.UiPageControlDot;
                    }
                    else
                    {
                        dot.BackgroundColor = UIColor.Clear;
                    }
                }

                messageContentPageControl.ReloadInputViews();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.ConfigurePageControl);
                ShowMessageException(exception);
            }
        }

        private async Task LoadDataAsync()
        {
            coverageHeightConstraint.Constant = 0;
            coverageView.Hidden = true;

            UIImage[] images = Util.LoadAnimationImage(ConstantImages.FolderSpinnerLoad, ConstantViewSize.FolderSpinnerLoadCount);

            customSpinnerHomeView.Image = images[0];
            customSpinnerHomeView.AnimationImages = images;
            customSpinnerHomeView.AnimationDuration = 1;

            customSpinnerStoreView.Image = images[0];
            customSpinnerStoreView.AnimationImages = images;
            customSpinnerStoreView.AnimationDuration = 1;

            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                Cities = await LoadCitiesAddresses();
                CitiesStore = await LoadCitiesStore();
            }
        }

        private void HiddenShowControlsToHomeView(bool status)
        {
            cityHomeLabel.Hidden = status;
            cityHomeTextField.Hidden = status;
            addressHomeLabel.Hidden = status;
            addressHomeTextField.Hidden = status;
            infoAditionalHomeLabel.Hidden = status;
            autocompleteAddressView.Hidden = status;
        }

        private void HiddenShowControlsToStoreView(bool status)
        {
            cityStoreLabel.Hidden = status;
            cityStoreTextField.Hidden = status;
            addressStoreLabel.Hidden = status;
            addressStoreTextField.Hidden = status;
            //infoAditionalStoreLabel.Hidden = status;
            autocompleteAddressView.Hidden = status;
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                //LoadFooterView(footerView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                this.continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                serviceTypeTitleLabel.Layer.CornerRadius = ConstantStyle.CornerRadius;
                serviceTypeTitleLabel.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
                serviceTypeTitleLabel.ClipsToBounds = true;
                toStoreView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                toStoreView.Layer.MaskedCorners = CACornerMask.MaxXMaxYCorner | CACornerMask.MinXMaxYCorner;
                toStoreView.ClipsToBounds = true;
                changeAddressView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                changeStoreView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                coverageView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                coverageView.Superview.Layer.CornerRadius = ConstantStyle.CornerRadius;
                autocompleteAddressView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                containerGenericPickerView.Layer.CornerRadius = ConstantStyle.CornerRadius;

                goBackButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                goBackButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
                goBackButton.Layer.BorderColor = ConstantColor.UiBorderColorGoBackWelcome.CGColor;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            try
            {
                //Campos del view de A domicilio
                addressHomeLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.WelcomeMessage);
                cityHomeLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.WelcomeMessage);
                cityHomeTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);
                addressHomeTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);
                infoAditionalHomeLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);

                //Campos del view de recoger en tienda
                addressStoreLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.WelcomeMessage);
                cityStoreLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.WelcomeMessage);
                cityStoreTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);
                addressStoreTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);
                infoAditionalStoreLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);

                //Campos vista principal
                wellcomeTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.WelcomeTitle);
                messageContentLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeMessage);
                serviceTypeTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.WelcomeTitleServiceType);
                toHomeTitleButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TitleGeneric);
                toStoreButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TitleGeneric);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void SetNextSliderTimer()
        {
            if (NumberSlider < GalleryTexts.Count - 1)
            {
                messageContentPageControl.CurrentPage = NumberSlider;
            }
        }

        private async Task GetCurrentAddressAsync()
        {
            CLGeocoder geoCoder = new CLGeocoder();
            var places = await geoCoder.ReverseGeocodeLocationAsync(LocationManager.Location);
            TransformAddress(places[0]);
        }

        private void TransformAddress(CLPlacemark place)
        {
            string[] currentAddress = place.Description.Split(',');
            string[] addressAprox = currentAddress[0].Split('-');
            string newAddress = addressAprox[0] + (addressAprox.Length > 1 ? " " + addressAprox[addressAprox.Length - 1] : string.Empty);
            addressHomeTextField.Text = newAddress;
            cityHomeTextField.Text = currentAddress[2];
            StopActivityIndicatorCustom();
            ValidateEnabledContinueButton(ConstantReusableViewName.ToHomeSelected, null);
        }

        private void LoadHandlers()
        {
            WireUpSwipeRight();
            WireUpSwipeLeft();
            addressHomeTextField.EditingChanged += AddressHomeTextFieldEditingChanged;
            continueButton.TouchUpInside += ContinueButtonTouchUpInside;
            changeAddressButton.TouchUpInside += ChangeAddressButtonTouchUpInside;
            changeStoreButton.TouchUpInside += ChangeStoreButtonTouchUpInside;
            cityStoreButton.TouchUpInside += CitiesToStoreUpInside;
            cityHomeButton.TouchUpInside += CityHomeButtonTouchUpInside;
            toHomeTitleButton.TouchUpInside += ToHomeTitleButtonTouchUpInside;
            toStoreButton.TouchUpInside += ToStoreButtonUpInside;
            addressStoreTextField.ValueChanged += AddressStoreTextFieldValueChanged;
            addressHomeTextField.ShouldReturn += AddressHomeTextFieldShouldReturn;
            closeGenericPickerButton.TouchUpInside += CloseGenericPickerUpInside;
            goBackButton.TouchUpInside += GoBackButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void UnsubscribeHandlers()
        {
            addressHomeTextField.EditingChanged -= AddressHomeTextFieldEditingChanged;
            continueButton.TouchUpInside -= ContinueButtonTouchUpInside;
            changeAddressButton.TouchUpInside -= ChangeAddressButtonTouchUpInside;
            changeStoreButton.TouchUpInside -= ChangeStoreButtonTouchUpInside;
            cityStoreButton.TouchUpInside -= CitiesToStoreUpInside;
            cityHomeButton.TouchUpInside -= CityHomeButtonTouchUpInside;
            toHomeTitleButton.TouchUpInside -= ToHomeTitleButtonTouchUpInside;
            toStoreButton.TouchUpInside -= ToStoreButtonUpInside;
            addressStoreTextField.ValueChanged -= AddressStoreTextFieldValueChanged;
            addressHomeTextField.ShouldReturn -= AddressHomeTextFieldShouldReturn;
        }

        private UserAddress SetAddress()
        {
            UserContext user = ParametersManager.UserContext;
            City city = _addressModel.GetCity(Util.RemoveDiacritics(cityHomeTextField.Text.TrimStart().TrimEnd()), Cities);

            if (city == null)
            {
                city = Util.SearchCity(Util.RemoveDiacritics(cityHomeTextField.Text.TrimStart().TrimEnd()), Cities);
            }
            

            cityHomeTextField.Text = city != null ? city.Name : Util.RemoveDiacritics(cityHomeTextField.Text.TrimStart().TrimEnd());
            string latitude = null;
            string longitud = null;

            try
            {
                latitude = LocationManager.Location.Coordinate.Latitude.ToString();
            }
            catch
            {
                latitude = null;
            }

            try
            {
                longitud = LocationManager.Location.Coordinate.Longitude.ToString();
            }
            catch
            {
                longitud = null;
            }

            return new UserAddress()
            {
                CityId = city != null ? city.Id : string.Empty,
                IdPickup = city != null ? city.IdPickup : string.Empty,
                City = cityHomeTextField.Text,
                AddressComplete = addressHomeTextField.Text,
                StateId = city != null ? city.State : string.Empty,
                Latitude = (latitude != null ? Decimal.Parse(latitude) : 0),
                Longitude = (longitud != null ? Decimal.Parse(longitud) : 0),
                Region = city != null ? city.Region : string.Empty,
                CellPhone = !string.IsNullOrEmpty(user.CellPhone) ? user.CellPhone : AppConfigurations.DefaultCellphone


            };
        }

        private void ShowViewNoCoverage()
        {
            coverageHeightConstraint.Constant = TamCoverageStatusView;
            coverageView.Hidden = false;
            toHomeViewHeightConstraint.Constant = 0;
            HiddenShowControlsToHomeView(true);
            toHomeView.Hidden = true;
            toStoreViewHeightConstraint.Constant = 0;
            toStoreView.Hidden = true;
            serviceTypeHeightConstraint.Constant = 0;
            serviceTypeTitleLabel.Hidden = true;
            continueButton.Enabled = false;
            continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
        }

        private async Task ValidateCoverageResponse(CoverageAddressResponse response)
        {
            try
            {
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RegisterNoCoverage(UserAddress.City, UserAddress.AddressComplete);
                    ShowViewNoCoverage();
                }
                else
                {
                    UserContext userContext = ParametersManager.UserContext;
                    UserAddress = ModelHelper.SetCoverageInformation(response, UserAddress);
                    UserAddress.LastName = userContext.LastName;
                    UserAddress.CellPhone = !string.IsNullOrEmpty(userContext.CellPhone) ? userContext.CellPhone : AppConfigurations.DefaultCellphone;
                    UserAddress.Name = userContext.FirstName;
                    userContext.Address = UserAddress;
                    Entities.Responses.Base.ResponseBase responseAddAddress = await _addressModel.AddAddress(UserAddress);

                    if (responseAddAddress.Result != null && responseAddAddress.Result.HasErrors && responseAddAddress.Result.Messages != null)
                    {
                        StopActivityIndicatorCustom();
                        var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(responseAddAddress.Result),
                            UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                        PresentViewController(alertController, true, null);
                    }
                    else
                    {
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));

                        RegisterEventShipping();
                        PresentHomeView();
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.WellcomeViewController, ConstantMethodName.ValidateCoverageResponse } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void RegisterEventShipping()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventShippingRelated(AnalyticsEvent.WelcomeShipping);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEventShippingRelated(AnalyticsEvent.WelcomeShipping);
        }

        private void RegisterNoCoverage(string city, string address)
        {
            //FirebaseEventRegistrationService.Instance.WithoutCoverage(city, address);
            //FacebookRegistrationEventsService_Deprecated.Instance.WithoutCoverage(city, address);
        }

        private async Task SaveInStore()
        {
            try
            {
                UserStore = SetStore();

                UpdateDispatchRegionParameters parameters = new UpdateDispatchRegionParameters
                {
                    CityId = UserStore.CityId,
                    Region = UserStore.Region,
                    DependencyId = UserStore.Id
                };

                string message = _addressModel.ValidateFieldsInStore(UserStore);

                if (string.IsNullOrEmpty(message))
                {
                    var response = await UpdateDispatchRegion(parameters);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        string messageError = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, messageError);
                    }
                    else
                    {
                        UserContext userContext = ParametersManager.UserContext;
                        userContext.Address = null;
                        userContext.Store = UserStore;
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));


                        City city = CitiesStore[PosSelectCity];
                        RegisterPickUpEvent();
                        PresentHomeView();
                    }
                }
                else
                {
                    StartActivityErrorMessage(EnumErrorCode.UnknownError.ToString(), message);
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.SaveInformation);
            }
        }

        private void RegisterPickUpEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventPickUpRelated(AnalyticsEvent.WelcomePickUp, city.Name);
        }

        private Store SetStore()
        {
            City city = CitiesStore[PosSelectCity];
            Store storeCurrent = Store[PosSelectStore];
            if (city != null && storeCurrent != null)
            {
                storeCurrent.City = city.Name;
                storeCurrent.CityId = city.Id.ToString();
                storeCurrent.IdPickup = city.IdPickup;
                storeCurrent.Region = city.Region;
            }
            return storeCurrent;
        }

        public async Task SaveInformation()
        {
            try
            {
                UserAddress = this.SetAddress();
                string message = _addressModel.ValidateFieldsAtHome(UserAddress);

                if (string.IsNullOrEmpty(message))
                {
                    LocationAddress location = new LocationAddress
                    {
                        Address = UserAddress.AddressComplete,
                        City = _addressModel.GetShortCityId(UserAddress.City, Cities)

                    };

                    var coverageResponse = await _addressModel.CoverageAddress(location);
                    await this.ValidateCoverageResponse(coverageResponse);
                }
                else
                {
                    if (message.Equals(AppMessages.CoverageAddressMessage))
                    {
                        ShowViewNoCoverage();
                    }
                    else
                    {
                        if (message.Equals(AppMessages.RequiredFieldsText))
                        {
                            StartActivityErrorMessage(EnumErrorCode.UnknownError.ToString(), message);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.SaveInformation);
                ShowMessageException(exception);

            }
            finally
            {
                spinnerActivityIndicatorView.StopAnimating();
                _spinnerActivityIndicatorView.Image.StopAnimating();
                customSpinnerView.Hidden = true;
            }
        }

        private void TryGetCurrentAddress()
        {
            try
            {
                LocationManager = new CLLocationManager
                {
                    ActivityType = CLActivityType.OtherNavigation
                };
                LocationManager.AuthorizationChanged += async (sender, args) =>
                {
                    if (args.Status == CLAuthorizationStatus.AuthorizedWhenInUse)
                    {
                        await GetCurrentAddressAsync();
                    }
                    else if (args.Status == CLAuthorizationStatus.Denied)
                    {
                        StopActivityIndicatorCustom();
                    }
                };
                LocationManager.RequestWhenInUseAuthorization();
                LocationManager.StartUpdatingLocation();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.TryGetCurrentAddress);
            }
        }

        private void InitializateTable()
        {
            try
            {
                autocompleteAddressView.Hidden = true;
                autocompleteAddressHeightConstraint.Constant = 0;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.InitializateTable);
                ShowMessageException(exception);
            }
        }

        private async Task LoadStoreFromCity()
        {
            string cityText = cityStoreTextField.Text;
            if (!cityText.Trim().Equals(string.Empty) && !cityText.Trim().Equals("Selecciona"))
            {
                StartActivityIndicatorCustom();
                City city = null;
                int i = 0;

                foreach (City current in CitiesStore)
                {
                    if (current.Name.Equals(cityText))
                    {
                        city = current;
                        PosSelectCity = i;
                    }

                    i++;
                }

                if (city != null)
                {
                    if (!city.Id.Equals("0"))
                    {
                        StoreParameters parameters = new StoreParameters() { CityId = city.Id, HomeDelivery = false, PickUp = true, IdPickup = city.IdPickup };
                        Store = new List<Store>();
                        Store = await LoadStore(parameters) as List<Store>;
                    }

                    if (Store != null && Store.Count > 0)
                    {
                        CityStorePickerViewDelegate cityAddressStoreDelegate = new CityStorePickerViewDelegate(Store, addressStoreTextField);
                        cityAddressStoreDelegate.Action += StoreToStoreDelegateAction;
                        genericPickerView.Delegate = cityAddressStoreDelegate;
                        genericPickerView.DataSource = new CityStorePickerViewDataSource(Store);
                        genericPickerView.ReloadAllComponents();
                        cityAddressStoreDelegate.Selected(genericPickerView, 0, 0);
                        containerGenericPickerView.Hidden = false;
                    }
                    else
                    {
                        var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.NotFoundStoreAvailable, UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                        PresentViewController(alertController, true, null);
                        containerGenericPickerView.Hidden = true;
                    }

                    StopActivityIndicatorCustom();
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.YouMustSelectAvalidCity, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
            }
            else
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.YouMustSelectFirtsCity, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
        }

        #endregion

        #region Events
        void ToHomeTitleButtonTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                if (toHomeViewHeightConstraint.Constant == TamHiddenService)
                {
                    HiddenShowControlsToHomeView(false);
                    HiddenShowControlsToStoreView(true);
                    toHomeViewHeightConstraint.Constant = TamShowService;
                    toStoreViewHeightConstraint.Constant = TamHiddenService;
                    if (DeviceManager.Instance.IsNetworkAvailable().Result)
                    {
                        TryGetCurrentAddress();
                    }
                    cityHomeTextField.ResignFirstResponder();
                    addressHomeTextField.ResignFirstResponder();
                    ValidateEnabledContinueButton(ConstantReusableViewName.ToHomeSelected, null);
                    mainScrollView.ScrollRectToVisible(new CGRect(continueButton.Frame.X, continueButton.Frame.Y, mainScrollView.Frame.Width, mainScrollView.Frame.Height), true);
                }
                else
                {
                    cityHomeTextField.ResignFirstResponder();
                    addressHomeTextField.ResignFirstResponder();
                    toHomeViewHeightConstraint.Constant = TamHiddenService;
                    HiddenShowControlsToHomeView(true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantMethodName.ToHomeButton_UpInside);
                ShowMessageException(exception); ;
            }
        }

        void ToStoreButtonUpInside(object sender, EventArgs e)
        {
            try
            {
                if (toStoreViewHeightConstraint.Constant == TamShowService)
                {
                    toStoreViewHeightConstraint.Constant = TamHiddenService;
                    HiddenShowControlsToStoreView(true);
                    cityStoreTextField.ResignFirstResponder();
                }
                else
                {
                    toStoreViewHeightConstraint.Constant = TamShowService;
                    HiddenShowControlsToStoreView(false);
                    toHomeViewHeightConstraint.Constant = TamHiddenService;
                    HiddenShowControlsToHomeView(true);
                    ValidateEnabledContinueButton(ConstantReusableViewName.ToStoreSelected, null);
                    mainScrollView.ScrollRectToVisible(new CGRect(continueButton.Frame.X, continueButton.Frame.Y, mainScrollView.Frame.Width, mainScrollView.Frame.Height), true);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantEventName.ToStoreButton_UpInside);
                ShowMessageException(exception);
            }
        }

        void CityHomeButtonTouchUpInside(object sender, EventArgs e)
        {
            if (Cities != null && Cities.Count > 0)
            {
                cityHomeTextField.ResignFirstResponder();
                addressHomeTextField.ResignFirstResponder();
                containerGenericPickerView.Hidden = false;
                CityStorePickerViewDelegate delegateCityHome = new CityStorePickerViewDelegate(Cities, cityHomeTextField);
                delegateCityHome.Action += ValidateEnabledContinueButton;
                genericPickerView.Delegate = delegateCityHome;
                genericPickerView.DataSource = new CityStorePickerViewDataSource(Cities);
                genericPickerView.Hidden = false;
                genericPickerView.ReloadAllComponents();
                cityHomeTextField.Text = string.Empty;
            }
            else
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, "No se encontraron ciudades disponibles en este momento, por favor intente mas tarde", UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
        }

        void CitiesToStoreUpInside(object sender, EventArgs e)
        {
            if (CitiesStore != null && CitiesStore.Count > 0)
            {
                cityStoreTextField.ResignFirstResponder();
                addressStoreTextField.ResignFirstResponder();
                containerGenericPickerView.Hidden = false;
                CityStorePickerViewDelegate delegateCityStore = new CityStorePickerViewDelegate(CitiesStore, cityStoreTextField);
                delegateCityStore.Action += CityAddressStoreDelegateAction;
                genericPickerView.Delegate = delegateCityStore;
                genericPickerView.DataSource = new CityStorePickerViewDataSource(CitiesStore);
                genericPickerView.Hidden = false;
                genericPickerView.ReloadAllComponents();
                delegateCityStore.Selected(genericPickerView, 0, 0);
                addressStoreTextField.Text = string.Empty;
            }
            else
            {
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.CitiesNotFoundMessages, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
        }

        void ValidateEnabledContinueButton(object sender, EventArgs e)
        {
            if (((string)sender).Equals(ConstantReusableViewName.ToHomeSelected))
            {
                if (!cityHomeTextField.Text.TrimEnd().TrimStart().Equals(string.Empty) &&
                   !addressHomeTextField.Text.TrimStart().TrimEnd().Equals(string.Empty))
                {
                    continueButton.Enabled = true;
                    continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.CGColor;
                }
                else
                {
                    continueButton.Enabled = false;
                    continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
                }
            }
            else if (((string)sender).Equals(ConstantReusableViewName.ToStoreSelected))
            {
                if (!cityStoreTextField.Text.TrimEnd().TrimStart().Equals(string.Empty) &&
                    !addressStoreTextField.Text.TrimStart().TrimEnd().Equals(string.Empty))
                {
                    continueButton.Enabled = true;
                    continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.CGColor;
                }
                else
                {
                    continueButton.Enabled = false;
                    continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
                }
            }
        }

        private async Task HandlerHomeAddress(object sender, EventArgs args)
        {
            try
            {
                var currentTextAddress = sender as UITextField;
                var currentAddress = currentTextAddress.Text;
                autocompleteAddressView.Hidden = true;
                autocompleteAddressHeightConstraint.Constant = TamHiddenAutocomplete;
                toHomeViewHeightConstraint.Constant -= CountPredictionPrevious;
                CountPredictionPrevious = 0;

                if (!(String.IsNullOrEmpty(currentAddress)) && currentAddress.Length > 2)
                {
                    Predictions = await AutoCompleteAddress(currentAddress);

                    if (this.Predictions != null && this.Predictions.Count > 0)
                    {
                        CountPredictionPrevious = (Predictions.Count * ConstantViewSize.AutocompleteHeightCell);

                        if (CountPredictionPrevious > ConstantViewSize.AutocompleteHeightCell * 5)
                        {
                            CountPredictionPrevious = ConstantViewSize.AutocompleteHeightCell * 5;
                        }

                        toHomeViewHeightConstraint.Constant = TamShowService + CountPredictionPrevious;
                        autocompleteAddressView.Hidden = false;
                        autocompleteAddressHeightConstraint.Constant = TamShowAutocomplete;
                        autocompleteAddressTableView.Source = new AutoCompleteAddressSource(Predictions as List<string>, currentTextAddress);
                        autocompleteAddressTableView.ReloadData();
                    }
                    else
                    {
                        autocompleteAddressView.Hidden = true;
                        autocompleteAddressHeightConstraint.Constant = TamHiddenAutocomplete;
                        toHomeViewHeightConstraint.Constant = TamShowService;
                        HiddenShowControlsToHomeView(false);
                    }
                }
                else
                {
                    autocompleteAddressView.Hidden = true;
                    autocompleteAddressHeightConstraint.Constant = TamHiddenAutocomplete;
                    toHomeViewHeightConstraint.Constant = TamShowService;
                    HiddenShowControlsToHomeView(false);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantEventName.ToStoreButton_UpInside);
                ShowMessageException(exception);
            }
        }

        private async Task HandlerContinue(object sender, EventArgs args)
        {
            if (toHomeViewHeightConstraint.Constant == TamShowService)
            {
                await this.SaveInformation();
            }
            else if (toStoreViewHeightConstraint.Constant == TamShowService)
            {
                await this.SaveInStore();
            }
            else
            {
                continueButton.Enabled = false;
                continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
            }
        }

        void MessageContentPageControl_EditingChanged(object sender, EventArgs e)
        {
            messageContentPageControl.CurrentPage = NumberSlider;
            CustomPageControl();
        }

        partial void AddressToStoreUpInside(UIButton sender)
        {
            LoadStoreFromCity();
        }

        private void CloseGenericPickerUpInside(object sender, EventArgs e)
        {
            if (toHomeViewHeightConstraint.Constant == TamShowService)
            {
                ValidateEnabledContinueButton(ConstantReusableViewName.ToHomeSelected, null);
            }
            else if (toStoreViewHeightConstraint.Constant == TamShowService)
            {
                ValidateEnabledContinueButton(ConstantReusableViewName.ToStoreSelected, null);
            }
            else
            {
                continueButton.Enabled = false;
                continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
            }
            containerGenericPickerView.Hidden = true;
        }

        private async void AddressHomeTextFieldEditingChanged(object sender, EventArgs e)
        {
            await HandlerHomeAddress(sender, e);
        }

        private async void ContinueButtonTouchUpInside(object sender, EventArgs e)
        {
            StartActivityIndicatorCustom();
            await HandlerContinue(sender, e);
        }

        private void ChangeAddressButtonTouchUpInside(object sender, EventArgs e)
        {
            coverageHeightConstraint.Constant = 0;
            coverageView.Hidden = true;
            toHomeViewHeightConstraint.Constant = TamShowService;
            HiddenShowControlsToHomeView(false);
            HiddenShowControlsToStoreView(true);
            toHomeView.Hidden = false;
            toStoreViewHeightConstraint.Constant = TamHiddenService;
            toStoreView.Hidden = false;
            serviceTypeHeightConstraint.Constant = TamShowServiceTitle;
            serviceTypeTitleLabel.Hidden = false;
            continueButton.Enabled = false;
            continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
        }

        private void ChangeStoreButtonTouchUpInside(object sender, EventArgs e)
        {
            coverageHeightConstraint.Constant = 0;
            coverageView.Hidden = true;
            toHomeViewHeightConstraint.Constant = TamHiddenService;
            HiddenShowControlsToHomeView(true);
            toHomeView.Hidden = false;
            toStoreViewHeightConstraint.Constant = TamShowService;
            toStoreView.Hidden = false;
            serviceTypeHeightConstraint.Constant = TamShowServiceTitle;
            HiddenShowControlsToStoreView(false);
            serviceTypeTitleLabel.Hidden = false;
            continueButton.Enabled = false;
            continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
        }

        private void AddressStoreTextFieldValueChanged(object sender, EventArgs e)
        {
            PosSelectStore = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(ConstantReusableViewName.ToStoreSelected, null);
        }

        private bool AddressHomeTextFieldShouldReturn(UITextField textField)
        {
            PosSelectCity = (int)genericPickerView.SelectedRowInComponent(0);
            textField.ResignFirstResponder();
            autocompleteAddressView.Hidden = true;
            autocompleteAddressHeightConstraint.Constant = 0;
            ValidateEnabledContinueButton(ConstantReusableViewName.ToHomeSelected, null);
            return true;
        }

        void RetryTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                ValidateNetWorkingAsync();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.WellcomeViewController, ConstantEventName.RetryTouchUpInside);
                ShowMessageException(exception);
            }
        }

        private void CityAddressStoreDelegateAction(object sender, EventArgs e)
        {
            PosSelectCity = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(sender, e);
        }

        private void StoreToStoreDelegateAction(object sender, EventArgs e)
        {
            PosSelectStore = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(sender, e);
        }

        private void GoBackButtonTouchUpInside(object sender, EventArgs e)
        {
            RegisterEventGoBack();
            this.NavigationController.PopViewController(true);
        }

        private void RegisterEventGoBack()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserId(AnalyticsEvent.WelcomeGoBack);
        }
        #endregion

        #region Gesture
        private void WireUpSwipeRight()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            messageContentLabel.AddGestureRecognizer(gesture);
        }

        private void WireUpSwipeLeft()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            messageContentLabel.AddGestureRecognizer(gesture);
        }

        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
            Timer.Invalidate();

            if (recognizer.Direction == UISwipeGestureRecognizerDirection.Right)
            {
                NumberSlider -= 1;

                if (NumberSlider < 0)
                {
                    NumberSlider = GalleryTexts.Count - 1;
                }
            }
            else if (recognizer.Direction == UISwipeGestureRecognizerDirection.Left)
            {
                NumberSlider += 1;

                if (NumberSlider > GalleryTexts.Count - 1)
                {
                    NumberSlider = 0;
                }
            }

            MessageContentPageControl_EditingChanged(messageContentPageControl, null);
            UIView.Animate(0.5, 0, UIViewAnimationOptions.TransitionFlipFromTop, () =>
            {
                wellcomeTitleLabel.Text = GalleryTexts[NumberSlider].Title;
                messageContentLabel.Text = GalleryTexts[NumberSlider].Description;
            }, () =>
            {
                Timer = NSTimer.CreateRepeatingScheduledTimer(5, WelcomeTimerAction);
            });
        }

        void WelcomeTimerAction(NSTimer obj)
        {
            NumberSlider += 1;

            if (NumberSlider >= GalleryTexts.Count)
            {
                NumberSlider = 0;
            }

            MessageContentPageControl_EditingChanged(messageContentPageControl, null);
            UIView.Animate(0.5, 0, UIViewAnimationOptions.TransitionFlipFromLeft, () =>
            {
                wellcomeTitleLabel.Text = GalleryTexts[NumberSlider].Title;
                messageContentLabel.Text = GalleryTexts[NumberSlider].Description;
            }, () => { });
        }
        #endregion
    }
}