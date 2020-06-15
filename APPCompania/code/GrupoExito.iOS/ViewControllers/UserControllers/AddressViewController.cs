using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Responses.Addresses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Source;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class AddressViewController : BaseAddressController
    {
        #region Attributes 
        private IList<City> cities;
        private string AddressType { get; set; }
        private string PreviousActivity { get; set; }
        private IList<string> predictions;
        private float countPredictionPrevious = 0;
        private AddressTypeView addressTypeView_;
        private IList<City> citiesSuggested;
        private int autocomplete_show = 150;
        private int autocomplete_hidden = 0;
        private UserAddress _address;
        private bool _assignDefaultAddress = false;
        public Boolean IsUpdateAddress = false;
        #endregion

        #region Properties
        public UserAddress Address { get => _address; set => _address = value; }
        public bool AssignDefaultAddress { get => _assignDefaultAddress; set => _assignDefaultAddress = value; }
        #endregion

        #region Constructors
        public AddressViewController(IntPtr handle) : base(handle)
        {
            this.cities = new List<City>();
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {                
                this.LoadExternalViews();
                this.LoadFonts();
                this.LoadHandlers();
                this.InitializateTable();
                this.LoadAddresses();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                this.NavigationController.NavigationBarHidden = false;
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(this.IsUpdateAddress ? AnalyticsScreenView.EditAddress : AnalyticsScreenView.AddAddress, nameof(AddressViewController));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            UnsubscribeHandlers();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        private void LoadData()
        {
            if (Address != null)
            {
                cityNameTextField.Text = _addressModel.GetCityName(Address.CityId, this.cities);
                addressTextField.Text = Address.AddressComplete;
                aditionalInfoTextField.Text = Address.AditionalInformationAddress;
                addressTypeView_.LoadImage(Address.Description);
                cellPhoneTextField.Text = string.IsNullOrEmpty(_addressModel.ValidateFieldCellPhone(Address.CellPhone)) ? Address.CellPhone : ParametersManager.UserContext.CellPhone;
                IsUpdateAddress = true;
                titleAddressLabel.Text = AppMessages.EditAddress;
                messageAddressLabel.Text = AppMessages.isModifiedAddress;
                saveAddress.SetTitle(AppMessages.ButtonUpdateText, UIControlState.Normal);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                SpinnerActivityIndicatorViewFromBase = spinnerAcitivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
                addressTypeView.LayoutIfNeeded();
                addressTypeView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.AddressTypeView, Self, null).GetItem<AddressTypeView>(0);
                CGRect addressTypeViewFrame = new CGRect(0, 0, addressTypeView.Frame.Size.Width, addressTypeView.Frame.Size.Height);
                addressTypeView_.Frame = addressTypeViewFrame;
                addressTypeView.AddSubview(addressTypeView_);

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            cityNameButton.TouchUpInside += CityNameButtonTouchUpInside;
            addressTextField.EditingChanged += AddressTextFieldEditingChanged;
            saveAddress.TouchUpInside += SaveAddressUpInside;
            cityNameTextField.ShouldReturn += CityNameTextFieldShouldReturn;
            addressTextField.ShouldReturn += AddressTextFieldShouldReturn;
            aditionalInfoTextField.ShouldReturn += AditionalInfoTextFieldShouldReturn;
            closeGenericPickerButton.TouchUpInside += CloseGenericPickerTouchUpInside;
        }

        public void UnsubscribeHandlers()
        {
            cityNameButton.TouchUpInside -= CityNameButtonTouchUpInside;
            addressTextField.EditingChanged -= AddressTextFieldEditingChanged;
            saveAddress.TouchUpInside -= SaveAddressUpInside;
            cityNameTextField.ShouldReturn -= CityNameTextFieldShouldReturn;
            addressTextField.ShouldReturn -= AddressTextFieldShouldReturn;
            aditionalInfoTextField.ShouldReturn -= AditionalInfoTextFieldShouldReturn;
        }

        private void LoadFonts()
        {
            cellPhoneTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 10;
            };
        
            try
            {
                titleAddressLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.UserNameHome);
                messageAddressLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.SubtitleGeneric);
                cityNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
                cityNameTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
                addressLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
                addressTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
                mapViewButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.AddressTextField);
                aditionalInfoLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
                aditionalInfoTextField.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.AddressTextField);
                saveAddress.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.AddressTextField);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void ClearControls()
        {
            addressTextField.Text = string.Empty;
            cityNameTextField.Text = string.Empty;
            aditionalInfoTextField.Text = string.Empty;
        }

        private async Task LoadAddresses()
        {
            cities = await GetCitiesAddresses() as List<City>;
            LoadData();
        }

        public async Task<IList<City>> GetCitiesAddresses()
        {
            cities = new List<City>();

            try
            {
                StartActivityIndicatorCustom();
                CitiesFilter parameters = new CitiesFilter() { HomeDelivery = "true", Pickup = "false" };
                var response = await _addressModel.GetCities(parameters);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result),
                        UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
                else
                {
                    StopActivityIndicatorCustom();
                    cities = response.Cities;
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.GetCitiesAddresses);
                ShowMessageException(exception);
            }

            return cities;
        }

        private async Task ValidateCoverageAddress()
        {
            try
            {
                StartActivityIndicatorCustom();
                Address = this.SetAddress();
                var city = _addressModel.GetCity(Util.RemoveDiacritics(cityNameTextField.Text.TrimStart().TrimEnd()), cities);
                if (city == null)
                {
                    city = Util.SearchCity(Util.RemoveDiacritics(cityNameTextField.Text.TrimStart().TrimEnd()), cities);
                }
                string message = city == null ? AppMessages.WrongCity : _addressModel.ValidateFields(Address);

                if (string.IsNullOrEmpty(message))
                {
                    LocationAddress location = new LocationAddress() { Address = addressTextField.Text, City = _addressModel.GetShortCityId(cityNameTextField.Text, cities) };
                    CoverageAddressResponse coverageResponse = await _addressModel.CoverageAddress(location);
                    await this.ValidateCoverageResponse(coverageResponse);
                }
                else
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.CoverageAddress);
                ShowMessageException(exception);
            }
        }

        private async Task ValidateCoverageResponse(CoverageAddressResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                StopActivityIndicatorCustom();
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result),
                    UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                PresentViewController(alertController, true, null);
            }
            else
            {
                Address = ModelHelper.SetCoverageInformation(response, Address);
                if (IsUpdateAddress && Address.IsDefaultAddress)
                {
                    await SetDefaultAddress(Address);
                }
                else if (IsUpdateAddress)
                {
                    await this.UpdateAddress();
                }
                else
                {
                    await this.AddAddress();
                }
            }
        }

        private async Task AddAddress()
        {
            try
            {
                var response = await _addressModel.AddAddress(Address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result),
                        UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
                else
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.AddAddressMessage,
                        UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action =>
                    {
                        ParametersManager.ContainChanges = true;
                        this.NavigationController.PopViewController(true);
                    }));
                    PresentViewController(alertController, true, null);
                    ClearControls();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.AddAddress);
                ShowMessageException(exception);
            }
        }

        private async Task UpdateAddress()
        {
            try
            {
                ResponseBase response = await _addressModel.UpdateAddress(Address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
                else
                {
                    if (!Address.IsDefaultAddress)
                    {
                        StopActivityIndicatorCustom();
                        var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.UpdateAddressMessage, UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action =>
                        {
                            ParametersManager.ContainChanges = true;
                            this.NavigationController.PopViewController(true);
                        }));
                        PresentViewController(alertController, true, null);
                        ClearControls();
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.UpdateAddress);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }

        private UserAddress SetAddress()
        {
            UserContext user = ParametersManager.UserContext;
            City city = _addressModel.GetCity(Util.RemoveDiacritics(cityNameTextField.Text), cities);

            if (city == null)
            {
                city = Util.SearchCity(Util.RemoveDiacritics(cityNameTextField.Text.TrimStart().TrimEnd()), cities);
            }

            cityNameTextField.Text = city != null ? city.Name : cityNameTextField.Text;

            return new UserAddress()
            {
                AddressKey = this.IsUpdateAddress ? Address.AddressKey : string.Empty,
                AddressName = this.IsUpdateAddress ? Address.AddressName : null,
                City = city != null ? city.Name : string.Empty,
                CityId = city != null ? city.Id : string.Empty,
                AddressComplete = addressTextField.Text,
                AditionalInformationAddress = aditionalInfoTextField.Text,
                Description = LoadAddressesTypeSelect(),
                Name = user.FirstName,
                LastName = user.LastName,
                CellPhone = cellPhoneTextField.Text,
                StateId = city != null ? city.State : string.Empty,
                Region = city != null ? city.Region : string.Empty,
                IsDefaultAddress = this.IsUpdateAddress ? Address.IsDefaultAddress : false,
                IdPickup = city != null ? city.IdPickup : string.Empty,
                DependencyId = this.IsUpdateAddress ? Address.DependencyId : string.Empty
            };
        }

        public async Task saveAddressUser()
        {
            if (string.IsNullOrEmpty(_addressModel.ValidateFieldCellPhone(cellPhoneTextField.Text)))
            {
                await ValidateCoverageAddress();
            }
            else
            {
                var defaultAlert = UIAlertController.Create(AppMessages.TitleGenericDialog, AppMessages.MobileNumberLenghtValidationText, UIAlertControllerStyle.Alert);
                defaultAlert.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                PresentViewController(defaultAlert, true, null);
            }
        }

        private string LoadAddressesTypeSelect()
        {
            return addressTypeView_.OptionSelected;
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
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantMethodName.InitializateTable);
                ShowMessageException(exception); ;
            }
        }

        private void FindCitiesSuggested(string currentTextCity)
        {
            citiesSuggested = new List<City>();

            foreach (var city in cities)
            {
                if (city.Name.ToLowerInvariant().ToString().Contains(currentTextCity.ToLowerInvariant().ToString()))
                {
                    citiesSuggested.Add(city);
                }
            }
        }

        public async Task<bool> SetDefaultAddress(UserAddress address)
        {
            await this.UpdateAddress();
            bool result = false;
            try
            {
                StartActivityIndicatorCustom();
                var response = await _addressModel.SetDefaultAddress(address);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
                else
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DefaultAddressMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action =>
                    {
                        UserContext userContext = ParametersManager.UserContext;
                        UserAddress addressCurrent = userContext.Address;
                        if ((addressCurrent != null && address.CityId != addressCurrent.CityId) || (userContext.Store != null && userContext.Store.CityId != address.CityId))
                        {
                            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.LastDateUpdated, string.Empty);
                            DeviceManager.Instance.DeleteAccessPreference(ConstPreferenceKeys.DoNotShowAgain);
                        }
                        userContext.Address = address;
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                        ParametersManager.UserContext = userContext;
                        ParametersManager.ContainChanges = true;
                        this.NavigationController.PopViewController(true);
                    }));
                    PresentViewController(alertController, true, null);
                }
                return result;
            }
            catch (Exception)
            {
                StopActivityIndicatorCustom();
                return result;
            }
        }
        #endregion

        #region Events
        private void CityNameButtonTouchUpInside(object sender, EventArgs e)
        {
            containerGenericPickerView.Hidden = false;
            cityNameTextField.BecomeFirstResponder();
            cityNameTextField.ResignFirstResponder();
            CityStorePickerViewDelegate delegateCityStore = new CityStorePickerViewDelegate(cities, cityNameTextField);
            genericPickerView.Delegate = delegateCityStore;
            genericPickerView.DataSource = new CityStorePickerViewDataSource(cities);
            genericPickerView.Hidden = false;
            genericPickerView.ReloadAllComponents();
            delegateCityStore.Selected(genericPickerView, 0, 0);
            cityNameTextField.Text = string.Empty;

        }

        private async Task HandlerHomeAddress(object sender, EventArgs args)
        {
            try
            {
                var currentTextAddress = sender as UITextField;
                var currentAddress = currentTextAddress.Text;
                autocompleteAddressView.Hidden = true;
                autocompleteAddressHeightConstraint.Constant = autocomplete_hidden;
                countPredictionPrevious = 0;

                if (!(String.IsNullOrEmpty(currentAddress)) && currentAddress.Length > 2)
                {
                    predictions = await AutoCompleteAddress(currentAddress);

                    if (this.predictions.Any())
                    {
                        countPredictionPrevious = (predictions.Count * ConstantViewSize.AutocompleteHeightCell);
                        autocompleteAddressView.Hidden = false;
                        autocompleteAddressHeightConstraint.Constant = autocomplete_show;
                        autocompleteAddressTableView.Source = new AutoCompleteAddressSource(predictions as List<string>, currentTextAddress);
                        autocompleteAddressTableView.ReloadData();
                    }
                    else
                    {
                        autocompleteAddressView.Hidden = true;
                        autocompleteAddressHeightConstraint.Constant = autocomplete_hidden;
                    }
                }
                else
                {
                    autocompleteAddressView.Hidden = true;
                    autocompleteAddressHeightConstraint.Constant = autocomplete_hidden;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.AddressViewController, ConstantEventName.ToStoreButton_UpInside);
                ShowMessageException(exception);
            }
        }

        private void SaveAddressUpInside(object sender, EventArgs e)
        {
            saveAddressUser();
        }

        private async void AddressTextFieldEditingChanged(object sender, EventArgs e)
        {
            await HandlerHomeAddress(sender, e);
        }

        private bool CityNameTextFieldShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            addressTextField.BecomeFirstResponder();
            return true;
        }

        private bool AddressTextFieldShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            autocompleteAddressView.Hidden = true;
            autocompleteAddressHeightConstraint.Constant = autocomplete_hidden;
            aditionalInfoTextField.BecomeFirstResponder();
            return true;
        }

        private bool AditionalInfoTextFieldShouldReturn(UITextField textField)
        {
            autocompleteAddressView.Hidden = true;
            autocompleteAddressHeightConstraint.Constant = autocomplete_hidden;
            textField.ResignFirstResponder();
            return true;
        }

        private void CloseGenericPickerTouchUpInside(object sender, EventArgs e)
        {
            containerGenericPickerView.Hidden = true;
        }
        #endregion
    }
}

