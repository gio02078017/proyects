using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class MyAddressTableViewSource : UITableViewSource
    {
        #region Attributes
        private List<UserAddress> Addresses { get; set; }
        private UIViewControllerBase ControllerBase;
        private UserAddress DefaultAddress;
        private UIActivityIndicatorView spinnerAcitivityIndicatorView;
        private CustomActivityIndicatorView spinnerActivityIndicatorView_;
        private UIView customSpinnerView;
        private NSLayoutConstraint myAddressHeightTableViewConstraint;
        private AddressTableViewCell addressDefaultView_;
        private AddressModel _addressModel;
        #endregion

        #region Constructors
        static MyAddressTableViewSource()
        {
            //Static default constructor this class
        }

        protected MyAddressTableViewSource(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }

        public MyAddressTableViewSource(List<UserAddress> addresses, UIViewControllerBase viewControllerBase, UIActivityIndicatorView spinnerAcitivityIndicatorView, UIView customSpinnerView, ref CustomActivityIndicatorView spinnerActivityIndicatorView_, NSLayoutConstraint myAddressHeightTableViewConstraint, AddressTableViewCell addressDefaultView_)
        {
            this.Addresses = addresses;
            this.ControllerBase = viewControllerBase;
            this.customSpinnerView = customSpinnerView;
            this.spinnerAcitivityIndicatorView = spinnerAcitivityIndicatorView;
            this.spinnerActivityIndicatorView_ = spinnerActivityIndicatorView_;
            this.myAddressHeightTableViewConstraint = myAddressHeightTableViewConstraint;
            this.addressDefaultView_ = addressDefaultView_;
            _addressModel = new AddressModel(new AddressService(DeviceManager.Instance));
        }
        #endregion

        #region Methods 
        private void AddressDefault(UserAddress userAddress)
        {
            try
            {
                addressDefaultView_.DetailAddress.Hidden = false;
                addressDefaultView_.TitleLabel.Text = userAddress.Description;
                addressDefaultView_.Address.Text = userAddress.AddressComplete;
                Util.SetConstraint(addressDefaultView_.DetailAddress, addressDefaultView_.DetailAddress.Frame.Width, 25);
                Util.SetConstraint(addressDefaultView_.DetailAddress, addressDefaultView_.DetailAddress.Frame.Height, 25);
                addressDefaultView_.DetailAddress.Image = UIImage.FromFile(ConstantImages.LapizEditar);
                if (userAddress.Description != null)
                {
                    switch (userAddress.Description)
                    {
                        case ConstAddressType.Home:
                            addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.CasaPrimaria);
                            break;
                        case ConstAddressType.Office:
                            addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.OficinaPrimario);
                            break;
                        case ConstAddressType.Couple:
                            addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.ParejaPrimario);
                            break;
                        default:
                            addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.OtroPrimario);
                            break;
                    }
                }
                else
                {
                    addressDefaultView_.IconAddress.Image = UIImage.FromFile(ConstantImages.CasaPrimaria);
                }
            }
            catch (Exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantControllersName.MyAddressViewController, ConstantMethodName.AddressDefault } };
            }
        }

        public async Task DeleteAddressAsync(UserAddress address, UITableView tableView)
        {
            await DeleteAddress(address, tableView);
        }

        public async Task<bool> DeleteAddress(UserAddress address, UITableView tableView)
        {
            bool result = false;
            try
            {
                spinnerAcitivityIndicatorView.StartAnimating();
                spinnerActivityIndicatorView_.Image.StartAnimating();
                spinnerActivityIndicatorView_.Message.Text = string.Empty;
                customSpinnerView.Hidden = false;
                var response = await _addressModel.DeleteAddress(address);

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
                    spinnerAcitivityIndicatorView.StopAnimating();
                    spinnerActivityIndicatorView_.Image.StopAnimating();
                    customSpinnerView.Hidden = true;
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteAddressMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                    ControllerBase.PresentViewController(alertController, true, null);
                    Addresses.Remove(address);
                    myAddressHeightTableViewConstraint.Constant = (ConstantViewSize.MyAddressHeightCell * (Addresses.Count));
                    tableView.ReloadData();
                }
                return result;
            }
            catch (Exception)
            {
                spinnerAcitivityIndicatorView.StopAnimating();
                spinnerActivityIndicatorView_.Image.StopAnimating();
                customSpinnerView.Hidden = true;
                return result;
            }
        }

        private void ClearAddressDefault()
        {
            for (int i = 0; i < Addresses.Count; i++)
            {
                Addresses[i].IsDefaultAddress = false;
            }
        }

        protected void StartActivityErrorMessage(string code, string message)
        {
            customSpinnerView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            spinnerActivityIndicatorView_.Image.StopAnimating();
            spinnerActivityIndicatorView_.Message.Hidden = false;
            spinnerActivityIndicatorView_.Message.TextColor = ConstantColor.UiMessageError;
            spinnerActivityIndicatorView_.Retry.Hidden = false;
            spinnerActivityIndicatorView_.CodeMesage = code;
            if (code.Equals(EnumErrorCode.InternetErrorMessage.ToString()))
            {
                spinnerActivityIndicatorView_.LoadImage(ConstantImages.SinConexion);
                spinnerActivityIndicatorView_.Message.Text = message;
            }
            else
            {
                spinnerActivityIndicatorView_.LoadImage(ConstantImages.SinInformacion);
                spinnerActivityIndicatorView_.Message.Text = message;
            }
        }
        #endregion 

        #region Methods Override
        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.MyAddressHeightCell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Addresses.Count;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            AddressTableViewCell cell = (AddressTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.MyAddressIdentifier, indexPath);
            try
            {
                UserAddress address = Addresses[indexPath.Row];
                cell.LoadData(address);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.GetCell);
            }
            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (addressDefaultView_ != null)
            {
                MessageConfirmView messageConfirmView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageConfirmView, Self, null).GetItem<MessageConfirmView>(0);
                CGRect messageConfirmViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
                messageConfirmView_.Frame = messageConfirmViewFrame;
                customSpinnerView.AddSubview(messageConfirmView_);
                spinnerActivityIndicatorView_.Hidden = true;
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.3f);
                spinnerAcitivityIndicatorView.StartAnimating();
                customSpinnerView.Hidden = false;
                customSpinnerView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                Util.SetConstraint(customSpinnerView, ConstantViewSize.customSpinnerViewHeightDefault, ConstantViewSize.messageCustomViewHeightDefault);
                messageConfirmView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                messageConfirmView_.Negation.TouchUpInside += (object sender, EventArgs e) =>
                {
                    spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    spinnerAcitivityIndicatorView.StopAnimating();
                    messageConfirmView_.RemoveFromSuperview();
                    customSpinnerView.BackgroundColor = UIColor.Clear;
                    customSpinnerView.Hidden = true;
                    Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
                };

                messageConfirmView_.Afirmation.TouchUpInside += (object sender, EventArgs e) =>
                {
                    Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
                    ClearAddressDefault();
                    spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    spinnerAcitivityIndicatorView.StopAnimating();
                    messageConfirmView_.RemoveFromSuperview();
                    customSpinnerView.BackgroundColor = UIColor.Clear;
                    spinnerActivityIndicatorView_.Hidden = false;
                    Addresses[indexPath.Row].IsDefaultAddress = true;
                    DefaultAddress = Addresses[indexPath.Row];
                    if (addressDefaultView_ != null)
                    {
                        if (addressDefaultView_.DefaultAddress != null)
                        {
                            addressDefaultView_.DefaultAddress.IsDefaultAddress = false;
                            Addresses[indexPath.Row] = addressDefaultView_.DefaultAddress;
                        }
                        else
                        {
                            Addresses.RemoveAt(indexPath.Row);
                        }
                        addressDefaultView_.DefaultAddress = DefaultAddress;
                    }
                    DefaultAddress.SelectedAddress = DefaultAddress.AddressName;
                    SetDefaultAddressAsync(DefaultAddress, tableView, indexPath);
                };
            }
            else
            {
                DefaultAddress = Addresses[indexPath.Row];
                var adress = Addresses.Where(x => x.IsDefaultAddress = true)?.FirstOrDefault();
                DefaultAddress.SelectedAddress = adress != null ? adress.Name : string.Empty;
                SetDefaultAddressAsync(DefaultAddress, tableView, indexPath);
            }
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction[] actions = new UITableViewRowAction[2];
            UITableViewRowAction edit = UITableViewRowAction.Create(UITableViewRowActionStyle.Default, string.Empty, (action, indexpath) =>
            {
                AddressViewController addressViewController_ = (AddressViewController)ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.AddressViewController);
                addressViewController_.HidesBottomBarWhenPushed = true;
                addressViewController_.Address = Addresses[indexPath.Row];
                addressViewController_.IsUpdateAddress = true;
                ControllerBase.NavigationController.PushViewController(addressViewController_, true);
            });
            edit.BackgroundColor = new UIColor(new UIImage(ConstantImages.AccionEditar));
            actions[1] = edit;

            UITableViewRowAction delete = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, string.Empty, (action, indexpath) =>
            {
                MessageConfirmView messageConfirmView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageConfirmView, Self, null).GetItem<MessageConfirmView>(0);
                CGRect messageConfirmViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
                messageConfirmView_.Frame = messageConfirmViewFrame;
                customSpinnerView.AddSubview(messageConfirmView_);
                spinnerActivityIndicatorView_.Hidden = true;
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.3f);
                spinnerAcitivityIndicatorView.StartAnimating();
                customSpinnerView.Hidden = false;
                customSpinnerView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                messageConfirmView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                messageConfirmView_.Message.Text = AppMessages.DeleteAddressChangeMessage;
                Util.SetConstraint(customSpinnerView, ConstantViewSize.customSpinnerViewHeightDefault, ConstantViewSize.messageCustomViewHeightDefault);
                messageConfirmView_.Negation.TouchUpInside += (object sender, EventArgs e) =>
                {
                    spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    spinnerAcitivityIndicatorView.StopAnimating();
                    messageConfirmView_.RemoveFromSuperview();
                    customSpinnerView.BackgroundColor = UIColor.Clear;
                    customSpinnerView.Hidden = true;
                    spinnerActivityIndicatorView_.Hidden = false;
                    Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
                };

                messageConfirmView_.Afirmation.TouchUpInside += (object sender, EventArgs e) =>
                {
                    spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    spinnerAcitivityIndicatorView.StopAnimating();
                    messageConfirmView_.RemoveFromSuperview();
                    customSpinnerView.BackgroundColor = UIColor.Clear;
                    spinnerActivityIndicatorView_.Hidden = false;
                    DeleteAddressAsync(Addresses[indexPath.Row], tableView);
                    Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
                };
            });
            delete.BackgroundColor = new UIColor(new UIImage(ConstantImages.AccionEliminar));
            actions[0] = delete;
            return actions;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }
        #endregion

        #region Events
        public async Task SetDefaultAddressAsync(UserAddress address, UITableView tableView, NSIndexPath indexPath)
        {
            try
            {
                bool result = await SetDefaultAddress(address);
                if (result)
                {
                    UserContext user = ParametersManager.UserContext;
                    if (user != null)
                    {
                        UserAddress addressCurrent = user.Address;
                        if((addressCurrent != null && address.CityId != addressCurrent.CityId) || (user.Store != null && user.Store.CityId != address.CityId))
                        {
                            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.LastDateUpdated, string.Empty);
                            DeviceManager.Instance.DeleteAccessPreference(ConstPreferenceKeys.DoNotShowAgain);
                        }

                        user.Address = address;
                        user.Store = null;
                    }
                    else
                    {
                        user = new UserContext
                        {
                            Address = address
                        };
                    }
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
                    RegisterEventShipping();
                    ParametersManager.ContainChanges = true;
                    ParametersManager.ChangeAddress = true;
                    if (addressDefaultView_ != null)
                    {
                        AddressDefault(DefaultAddress);
                        tableView.ReloadData();
                    }
                    else
                    {
                        ControllerBase.NavigationController.PopViewController(true);
                    }
                }
            }catch(Exception exception){
                Util.LogException(exception, ConstantControllersName.MyAddressViewController, ConstantMethodName.SetDefaultAddressAsync);
            }
        }

        private void RegisterEventShipping()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventShippingRelated(AnalyticsEvent.LaterIncomeShipping);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEventShippingRelated(AnalyticsEvent.LaterIncomeShipping);
        }

        public async Task<bool> SetDefaultAddress(UserAddress address)
        {
            bool result = false;
            try
            {
                spinnerAcitivityIndicatorView.StartAnimating();
                spinnerActivityIndicatorView_.Image.StartAnimating();
                spinnerActivityIndicatorView_.Message.Text = string.Empty;
                customSpinnerView.Hidden = false;
                var response = await _addressModel.SetDefaultAddress(address);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    result = false;
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
                    spinnerAcitivityIndicatorView.StopAnimating();
                    spinnerActivityIndicatorView_.Image.StopAnimating();
                    customSpinnerView.Hidden = true;
                    if (addressDefaultView_ != null)
                    {
                        var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DefaultAddressMessage, UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
                        ControllerBase.PresentViewController(alertController, true, null);
                    }
                    result = true;
                }
                return result;
            }
            catch (Exception)
            {
                spinnerAcitivityIndicatorView.StopAnimating();
                spinnerActivityIndicatorView_.Image.StopAnimating();
                customSpinnerView.Hidden = true;
                return result;
            }
        }
        #endregion 
    }
}