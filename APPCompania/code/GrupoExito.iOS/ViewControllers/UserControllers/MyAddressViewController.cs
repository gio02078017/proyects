using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class MyAddressViewController : BaseAddressController
    {
        #region Attributes 
        private UserAddress Address { get; set; }
        private List<UserAddress> addresses { get; set; }
        private UserAddress defaultAddress;
        private AddressTableViewCell addressDefaultView_;
        private bool ContainChanges = false;
        #endregion

        #region Constructors
        public MyAddressViewController(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.MyAddresses, nameof(MyAddressViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {                
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadFonts();
                this.GetAddressAsync();
               
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.ViewDidLoad);
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
                    GetAddressAsync();
                    this.ContainChanges = true;
                    ParametersManager.ContainChanges = false;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ParametersManager.ContainChanges = this.ContainChanges;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 

        private void LoadCorners()
        {
            try
            {
                addressDefaultView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                addressDefaultView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                infoAddressView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                detailInfoView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            try
            {
                addressDefaultView_.TitleLabel.TextColor = UIColor.White;
                addressDefaultView_.Address.TextColor = UIColor.White;
                titleMyAddressLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.MyAddressTitle);
                messageMyAddressLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyAddressMessageSubtitle);
                otherAddressLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.MyAddressOtherMessage);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                myAddressTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.AddressTableViewCell, NSBundle.MainBundle), ConstantIdentifier.MyAddressIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                addressDefaultView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.AddressTableViewCell, Self, null).GetItem<AddressTableViewCell>(0);
                addressDefaultView.LayoutIfNeeded();
                CGRect addressDefaultFrame = new CGRect(0, 0, addressDefaultView.Frame.Size.Width, addressDefaultView.Frame.Size.Height);
                addressDefaultView_.Frame = addressDefaultFrame;
                addressDefaultView.AddSubview(addressDefaultView_);

                addressDefaultView_.EditAddress.TouchUpInside += EditAddressUpInside;

                SpinnerActivityIndicatorViewFromBase = spinnerAcitivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void DrawAddresses()
        {
            try
            {
                int posToRemove = -1;
                if (addresses != null && addresses.Any() && ParametersManager.UserContext.Store == null)
                {
                    for (int i = 0; i < addresses.Count; i++)
                    {
                        if (addresses[i].IsDefaultAddress)
                        {
                            posToRemove = i;
                            defaultAddress = addresses[i];
                        }
                    }
                }
                else
                {
                    defaultAddress = null;
                }
                if (posToRemove != -1)
                {
                    addresses.RemoveAt(posToRemove);
                }
                this.AddressDefault();
                myAddressHeightTableViewConstraint.Constant = (ConstantViewSize.MyAddressHeightCell * (addresses.Count));
                myAddressTableView.Source = new MyAddressTableViewSource(addresses, this, spinnerAcitivityIndicatorView, customSpinnerView, ref _spinnerActivityIndicatorView, myAddressHeightTableViewConstraint, addressDefaultView_);
                myAddressTableView.ReloadData();
                if (_spinnerActivityIndicatorView.CodeMesage.Equals(string.Empty))
                {
                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.DrawAddresses);
                ShowMessageException(exception);
            }
        }

        private void AddressDefault()
        {
            try
            {
                Store store = ParametersManager.UserContext.Store;
                if (defaultAddress != null)
                {
                    Util.SetConstraint(addressDefaultView_.DetailAddress, addressDefaultView_.DetailAddress.Frame.Width, 25);
                    Util.SetConstraint(addressDefaultView_.DetailAddress, addressDefaultView_.DetailAddress.Frame.Height, 25);
                    addressDefaultView_.DefaultAddress = defaultAddress;
                    addressDefaultView_.TitleLabel.Text = defaultAddress.Description;
                    addressDefaultView_.Address.Text = defaultAddress.AddressComplete;
                    addressDefaultView_.DetailAddress.Image = UIImage.FromFile(ConstantImages.LapizEditar);
                    if (defaultAddress.Description != null)
                    {
                        addressDefaultView_.IconAddress.Image = UIImage.FromFile(defaultAddress.Description.ToLower());
                    }
                    else
                    {
                        addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.CasaPrimaria);
                    }
                    addressDefaultView_.DetailAddress.Hidden = false;
                }
                else if (store != null)
                {
                    messageMyAddressLabel.Text = AppMessages.todayPickupYourMarketIn;
                    addressDefaultView_.DefaultStore = store;
                    addressDefaultView_.TitleLabel.Text = store.Name;
                    addressDefaultView_.Address.Text = store.Address;
                    addressDefaultView_.DetailAddress.Image = UIImage.FromFile(ConstantImages.LapizEditar);
                    addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.ServiciosAlmacen);
                    addressDefaultView_.DetailAddress.Hidden = true;
                }
                else
                {
                    Util.SetConstraint(addressDefaultView, addressDefaultView.Frame.Height, 0);
                    addressDefaultView_.RemoveFromSuperview();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.AddressDefault);
                ShowMessageException(exception);
            }
        }

        private async Task GetAddressAsync()
        {
            try
            {
                addresses = await GetAddresses() as List<UserAddress>;
                    DrawAddresses();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.GetAddressAsync);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Events
        private void EditAddressUpInside(object sender, EventArgs e)
        {
            if (addressDefaultView_.DefaultAddress != null)
            {
                AddressViewController addressViewController_ = (AddressViewController)Storyboard.InstantiateViewController(ConstantControllersName.AddressViewController);
                addressViewController_.HidesBottomBarWhenPushed = true;
                addressViewController_.Address = addressDefaultView_.DefaultAddress;
                NavigationController.PushViewController(addressViewController_, true);
            }
            else
            {
                //Aqui se debe enviar a una vista donde pueda modificar el almacen de recogida
            }
        }
        #endregion
    }
}



