using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Source;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    public partial class LaterIncomeViewController : BaseAddressController
    {
        #region Attributes 
        private List<UserAddress> Addresses { get; set; }
        private IList<City> CitiesStore { get; set; }
        private IList<Store> Stores { get; set; }
        private ServiceTypeTableViewCell _serviceTypeTableViewCell;
        private Store UserStore { get; set; }
        private int PosCitySelected = -1;
        private int PosStoreSelected = -1;
        #endregion

        #region Constructors 
        public LaterIncomeViewController(IntPtr handle) : base(handle){}
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {             
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadHandlers();
                this.LoadData();
                this.toHomeSelectedView.Hidden = true;
                this.ToHomeButtonTouchUpInside(null, null);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaterIncomeViewController, ConstantMethodName.ViewDidLoad);
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

                if (ParametersManager.ContainChanges)
                {
                    this.GetAddressAsync();
                    ParametersManager.ContainChanges = false;
                }
                this.SetUserName();
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaterIncomeViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            GC.Collect();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
        #endregion

        #region Methods
        private void SetUserName()
        {
            try
            {
                if (ParametersManager.UserContext == null || ParametersManager.UserContext.IsAnonymous)
                {
                    userNameLabel.Hidden = true;
                }
                else
                {
                    userNameLabel.Text = AppMessages.Hello + " " + Util.Capitalize(ParametersManager.UserContext.FirstName).TrimEnd() + "!";
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaterIncomeViewController, ConstantMethodName.SetUserName);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            userNameLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.UserNameHome);
            messageTitleLabel.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.HomeDescriptionUser);
        }

        private void LoadHandlers()
        {
            toHomeButton.TouchUpInside += ToHomeButtonTouchUpInside;
            toStoreButton.TouchUpInside += ToStoreButtonTouchUpInside;
            addAddressButton.TouchUpInside += AddAddressButtonTouchUpInside;
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            closeGenericPickerButton.TouchUpInside += CloseCityAddressPickerButtonTouchUpInside;
        }


        private void LoadData(){
            addAddressButton.SetTitle(AppMessages.AddAddressText, UIControlState.Normal);
        }

        private void LoadCorners()
        {
            customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            containerView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            containerView.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
        }

        private void dropShadowContainer(UIColor color,float opacity,CGSize offSet ,float radius, bool scale = true)
        {
            containerView.Layer.MasksToBounds = false;
            containerView.Layer.ShadowColor = color.CGColor;
            containerView.Layer.ShadowOpacity = opacity;
            containerView.Layer.ShadowOffset = offSet;
            containerView.Layer.ShadowRadius = radius;
            containerView.Layer.ShouldRasterize = true;
            containerView.Layer.RasterizationScale = scale ? UIScreen.MainScreen.Scale : 1;
        }

        private void LoadExternalViews()
        {
            try
            {
                AddressStoreTableView.RegisterNibForCellReuse(NotCreditCardViewCell.Nib, NotCreditCardViewCell.Key);
                AddressStoreTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.AddressTableViewCell, NSBundle.MainBundle), ConstantIdentifier.MyAddressIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                LoadFooterView(footerView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerAcitivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaterIncomeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private async Task GetAddressAsync()
        {
            try
            {
                Addresses = await GetAddresses() as List<UserAddress>;
                DrawAddresses();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaterIncomeViewController, ConstantMethodName.GetAddressAsync);
                ShowMessageException(exception);
            }
        }

        private void GetStores(){
            StartActivityIndicatorCustom();
            Stores = new List<Store>();
            if (ParametersManager.UserContext != null)
            {
                Store store = ParametersManager.UserContext.Store;
                if (store != null)
                {
                    Stores.Add(store);
                }
            }
            MyStoresTableViewSource storesTableViewSource = new MyStoresTableViewSource(Stores as List<Store>, this, spinnerAcitivityIndicatorView, customSpinnerView, ref _spinnerActivityIndicatorView);
            AddressStoreTableView.Source = storesTableViewSource;
            storesTableViewSource.SelectStoreAction += AddAddressButtonTouchUpInside;
            if (Stores.Count == 0)
            {
                addressStoreHeightTableViewConstraint.Constant = 400;
                addAddressButton.Hidden = true;
                addAddressImageView.Hidden = true;
            }
            else
            {
                addressStoreHeightTableViewConstraint.Constant = (ConstantViewSize.MyAddressHeightCell * (Stores.Count));
            }
            AddressStoreTableView.RowHeight = UITableView.AutomaticDimension;
            AddressStoreTableView.ReloadData();
            StopActivityIndicatorCustom();
        }

        private void DrawAddresses()
        {
            try
            {
                addressStoreHeightTableViewConstraint.Constant = (ConstantViewSize.MyAddressHeightCell * (Addresses.Count));
                AddressStoreTableView.Source = new MyAddressTableViewSource(Addresses, this, spinnerAcitivityIndicatorView, customSpinnerView, ref _spinnerActivityIndicatorView, addressStoreHeightTableViewConstraint, null);
                AddressStoreTableView.EstimatedRowHeight = UITableView.AutomaticDimension;
                AddressStoreTableView.ReloadData();
                toStoreSelectedView.Hidden = true;
                if(Addresses.Count > 0)
                {
                    addAddressButton.Hidden = false;
                    addAddressImageView.Hidden = false;
                }
                if (_spinnerActivityIndicatorView.CodeMesage.Equals(string.Empty) || (Addresses != null && Addresses.Count  > 0))
                {
                    StopActivityIndicatorCustom();
                   
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LaterIncomeViewController, ConstantMethodName.DrawAddresses);
                ShowMessageException(exception);
            }
        }

        private async Task loadStoreFromCity()
        {
            string cityText = _serviceTypeTableViewCell.City.Text;
            if (!cityText.Trim().Equals(string.Empty))
            {
                _serviceTypeTableViewCell.SpinnerActivityIndicator.StartAnimating();
                _serviceTypeTableViewCell._spinnerActivityIndicatorView.Hidden = false;
                _serviceTypeTableViewCell._spinnerActivityIndicatorView.StartAnimating();
                City city = null;
                int i = 0;
                foreach (City current in CitiesStore)
                {
                    if (current.Name.Equals(cityText))
                    {
                        city = current;
                        PosCitySelected = i;
                    }
                    i++;
                }
                if (city != null)
                {
                    if (!city.Id.Equals("0"))
                    {
                        StoreParameters parameters = new StoreParameters() { CityId = city.Id, HomeDelivery = false, PickUp = true, IdPickup = city.IdPickup };
                        Stores = new List<Store>();
                        Stores = await LoadStore(parameters) as List<Store>;
                    }
                    if (Stores != null && Stores.Count > 0)
                    {
                        CityStorePickerViewDelegate cityAddressStoreDelegate = new CityStorePickerViewDelegate(Stores, _serviceTypeTableViewCell.Address);
                        cityAddressStoreDelegate.Action += CityAddressStoreDelegate_Action; ;
                        genericPickerView.Delegate = cityAddressStoreDelegate;
                        genericPickerView.DataSource = new CityStorePickerViewDataSource(Stores);
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
                    _serviceTypeTableViewCell.SpinnerActivityIndicator.StopAnimating();
                    _serviceTypeTableViewCell._spinnerActivityIndicatorView.Hidden = true;
                    _serviceTypeTableViewCell._spinnerActivityIndicatorView.StopAnimating();
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

        private async Task SaveInStore()
        {
            try
            {
                SetStore();

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
                        UserAddress addressCurrent = userContext.Address;
                        if ((addressCurrent != null && UserStore.CityId != addressCurrent.CityId) || (userContext.Store != null && userContext.Store.CityId != UserStore.CityId))
                        {
                            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.LastDateUpdated, string.Empty);
                            DeviceManager.Instance.DeleteAccessPreference(ConstPreferenceKeys.DoNotShowAgain);
                        }

                        userContext.Address = null;
                        userContext.Store = UserStore;
                        ParametersManager.ContainChanges = true;
                        ParametersManager.ChangeAddress = true;
                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                        RegisterPickUpEvent();
                        NavigationController.PopViewController(true);
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
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.AddStore, nameof(LaterIncomeViewController));
        }

        private void SetStore()
        {
            City city = null;
            city = CitiesStore[PosCitySelected];
            UserStore = Stores[PosStoreSelected];
            if (city != null)
            {
                UserStore.City = city.Name;
                UserStore.CityId = city.Id;
                UserStore.IdPickup = city.IdPickup;
                UserStore.Region = city.Region;
            }
        }

        #endregion

        #region Event

        private void ToHomeButtonTouchUpInside(object sender, EventArgs e)
        {
            if (toHomeSelectedView.Hidden)
            {
                addAddressButton.SetTitle(AppMessages.AddAddressText, UIControlState.Normal);
                toHomeSelectedView.Hidden = false;
                toHomeButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ToHomeStoreOption);
                toHomeButton.SetTitleColor(ConstantColor.UiLaterIncomeTitleButtonsSelected, UIControlState.Normal);
                toStoreButton.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.ToHomeStoreOption);
                toStoreButton.SetTitleColor(ConstantColor.UiLaterIncomeTitleButtonsNotSelected, UIControlState.Normal);
                toStoreSelectedView.Hidden = true;
                GetAddressAsync();
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.AddressesDialogDeliver, nameof(LaterIncomeViewController));
            }
        }

        private void ToStoreButtonTouchUpInside(object sender, EventArgs e)
        {
            if (toStoreSelectedView.Hidden)
            {
                addAddressButton.SetTitle(AppMessages.AddStoreText, UIControlState.Normal);
                toHomeButton.Font = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.ToHomeStoreOption);
                toHomeButton.SetTitleColor(ConstantColor.UiLaterIncomeTitleButtonsNotSelected, UIControlState.Normal);
                toStoreButton.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.ToHomeStoreOption);
                toStoreButton.SetTitleColor(ConstantColor.UiLaterIncomeTitleButtonsSelected, UIControlState.Normal);
                toHomeSelectedView.Hidden = true;
                toStoreSelectedView.Hidden = false;
                GetStores();
                FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.AddressesDialogStore, nameof(LaterIncomeViewController));
            }
        }

        private void AddAddressButtonTouchUpInside(object sender, EventArgs e)
        {
            if (addAddressButton.Title(UIControlState.Normal).Equals(AppMessages.AddAddressText))
            {
                AddressViewController addressViewController_ = (AddressViewController)Storyboard.InstantiateViewController(ConstantControllersName.AddressViewController);
                addressViewController_.HidesBottomBarWhenPushed = true;
                NavigationController.PushViewController(addressViewController_, true);
            }else{
                _serviceTypeTableViewCell = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.ServiceTypeTableViewCell, Self, null).GetItem<ServiceTypeTableViewCell>(0);
                CGRect serviceTypeTableViewCellFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
                _serviceTypeTableViewCell.Frame = serviceTypeTableViewCellFrame;
                _serviceTypeTableViewCell.Layer.CornerRadius = ConstantStyle.CornerRadius;
                _spinnerActivityIndicatorView.Hidden = true;
                customSpinnerView.AddSubview(_serviceTypeTableViewCell);
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.5f);
                spinnerAcitivityIndicatorView.StartAnimating();
                customSpinnerView.Hidden = false;
                _serviceTypeTableViewCell.SpinnerActivityIndicator.StopAnimating();
                _serviceTypeTableViewCell.LoadAnimationImages(ConstantImages.FolderSpinnerLoad, ConstantViewSize.FolderSpinnerLoadCount);
                _serviceTypeTableViewCell._spinnerActivityIndicatorView.StopAnimating();
                _serviceTypeTableViewCell._spinnerActivityIndicatorView.Hidden = true;
                _serviceTypeTableViewCell.Cancel.TouchUpInside += CancelTouchUpInside;
                _serviceTypeTableViewCell.Acept.TouchUpInside += addStoreTouchUpInside;
                ValidateEnabledContinueButton(_serviceTypeTableViewCell.Acept, null);
                _serviceTypeTableViewCell.CityActiveButton.TouchUpInside += CityActiveButtonTouchUpInside;
                _serviceTypeTableViewCell.StoreActiveButton.TouchUpInside += StoreActiveButtonTouchUpInside;
            }
        }

        private async void CityActiveButtonTouchUpInside(object sender, EventArgs e)
        {
            _serviceTypeTableViewCell.SpinnerActivityIndicator.StartAnimating();
            _serviceTypeTableViewCell._spinnerActivityIndicatorView.StartAnimating();
            _serviceTypeTableViewCell._spinnerActivityIndicatorView.Hidden = false;
            _spinnerActivityIndicatorView.CodeMesage = string.Empty;
            CitiesStore = await LoadCitiesStore();
            if (_spinnerActivityIndicatorView.CodeMesage.Equals(string.Empty))
            {
                if (CitiesStore != null && CitiesStore.Count > 0)
                {
                    _serviceTypeTableViewCell.City.ResignFirstResponder();
                    _serviceTypeTableViewCell.Address.ResignFirstResponder();
                    containerGenericPickerView.Hidden = false;
                    CityStorePickerViewDelegate delegateCityStore = new CityStorePickerViewDelegate(CitiesStore, _serviceTypeTableViewCell.City);
                    delegateCityStore.Action += DelegateCityStoreAction;;
                    genericPickerView.Delegate = delegateCityStore;
                    genericPickerView.DataSource = new CityStorePickerViewDataSource(CitiesStore);
                    genericPickerView.Hidden = false;
                    genericPickerView.ReloadAllComponents();
                    delegateCityStore.Selected(genericPickerView, 0, 0);
                    _serviceTypeTableViewCell.Address.Text = string.Empty;
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, "No se encontraron ciudades disponibles en este momento, por favor intente mas tarde", UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    PresentViewController(alertController, true, null);
                }
                _serviceTypeTableViewCell.SpinnerActivityIndicator.StopAnimating();
                _serviceTypeTableViewCell._spinnerActivityIndicatorView.StopAnimating();
                _serviceTypeTableViewCell._spinnerActivityIndicatorView.Hidden = true;
            }
            else
            {
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), _spinnerActivityIndicatorView.CodeMesage);
                _serviceTypeTableViewCell.RemoveFromSuperview();
            }
        }

        private void StoreActiveButtonTouchUpInside(object sender, EventArgs e)
        {
            loadStoreFromCity();
        }

        private void DelegateCityStoreAction(object sender, EventArgs e)
        {
            PosCitySelected = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(sender, e);
        }

        private void CityAddressStoreDelegate_Action(object sender, EventArgs e)
        {
            PosStoreSelected = (int)genericPickerView.SelectedRowInComponent(0);
            ValidateEnabledContinueButton(sender, e);
        }

        private void ValidateEnabledContinueButton(object sender, EventArgs e)
        {
            if(_serviceTypeTableViewCell.City.Text.Trim().Equals(string.Empty) || _serviceTypeTableViewCell.Address.Text.Trim().Equals(string.Empty)){
                _serviceTypeTableViewCell.Acept.Enabled = false;
                _serviceTypeTableViewCell.Acept.BackgroundColor = _serviceTypeTableViewCell.Acept.BackgroundColor.ColorWithAlpha(0.3f);
            }else{
                _serviceTypeTableViewCell.Acept.Enabled = true;
                _serviceTypeTableViewCell.Acept.BackgroundColor = ConstantColor.UiPrimary.ColorWithAlpha(1f);
            }
        }

        private async void addStoreTouchUpInside(object sender, EventArgs e)
        {
            StartActivityIndicatorCustom();
            await this.SaveInStore();
        }

        private void CancelTouchUpInside(object sender, EventArgs e)
        {
            spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            spinnerAcitivityIndicatorView.StopAnimating();
            _serviceTypeTableViewCell.RemoveFromSuperview();
            _spinnerActivityIndicatorView.Hidden = false;
            customSpinnerView.Hidden = true;
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            _spinnerActivityIndicatorView.Retry.Hidden = true;
            if (addAddressButton.Title(UIControlState.Normal).Equals(AppMessages.AddAddressText))
            {
                GetAddressAsync();
            }
            else if (addAddressButton.Title(UIControlState.Normal).Equals(AppMessages.AddStoreText))
            {
                GetStores();
            }
            else if (_spinnerActivityIndicatorView.Retry.Title(UIControlState.Normal).Equals(AppMessages.StoreSelect))
            {
                AddAddressButtonTouchUpInside(sender, e);
            }
        }

        private void CloseCityAddressPickerButtonTouchUpInside(object sender, EventArgs e)
        {
            containerGenericPickerView.Hidden = true;
        }
        #endregion
    }
}

