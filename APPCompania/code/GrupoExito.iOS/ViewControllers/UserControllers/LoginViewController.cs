using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class LoginViewController : BaseAddressController
    {
        #region private Attributes 
        private LoginModel _loginModel;
        private LoginResponse LoginResponse { get; set; }
        private RecoverPasswordView recoverPasswordView;
        #endregion

        #region Constructors
        public LoginViewController(IntPtr handle) : base(handle)
        {
            _loginModel = new LoginModel(new LoginService(DeviceManager.Instance));
        }
        #endregion

        #region View lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {               
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadFonts();
                this.ValidateContinueButton();
                this.ValidateTextField();
                this.spinnerActivityIndicatorView.StopAnimating();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.Login, nameof(LoginViewController));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController.NavigationBarHidden = true;
            this.LoadHandlers();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            UnsubscribeHandlers();
            StopActivityIndicatorCustom();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        private void LoadExternalViews()
        {
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            CustomSpinnerViewFromBase = customSpinnerView;
        }

        private void LoadCorners()
        {
            try
            {
                registerButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                loginTitleLabel.Layer.CornerRadius = ConstantStyle.CornerRadius;
                loginTitleLabel.Layer.MaskedCorners = CACornerMask.MinXMinYCorner | CACornerMask.MaxXMinYCorner;
                loginTitleLabel.ClipsToBounds = true;
                loginContentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                loginContentView.Layer.MaskedCorners = CACornerMask.MaxXMaxYCorner | CACornerMask.MinXMaxYCorner;
                loginContentView.ClipsToBounds = true;
                loginButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                registerButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadFonts()
        {
            //Load font size and style
        }

        public void LoadHandlers()
        {
            try
            {
                emailTextField.EditingChanged += TextFieldEditingChanged;
                passwordTextField.EditingChanged += TextFieldEditingChanged;
                emailTextField.ValueChanged += TextFieldEditingChanged;
                passwordTextField.ValueChanged += TextFieldEditingChanged;
                emailTextField.ShouldReturn += EmailTextFieldShouldReturn;
                passwordTextField.ShouldReturn += PasswordTextFieldShouldReturn;
                showHidePasswordButton.TouchUpInside += ShowHiddenPasswordButtonTouchUpInside;
                _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.LoadEventsTextField);
                ShowMessageException(exception);
            }
        }


        private void UnsubscribeHandlers()
        {
            emailTextField.EditingChanged -= TextFieldEditingChanged;
            passwordTextField.EditingChanged -= TextFieldEditingChanged;
            emailTextField.ShouldReturn -= EmailTextFieldShouldReturn;
            passwordTextField.ShouldReturn -= PasswordTextFieldShouldReturn;
            showHidePasswordButton.TouchUpInside -= ShowHiddenPasswordButtonTouchUpInside;
        }

        private void ValidateGeneratePassword()
        {
            if (LoginResponse.User.GeneratedPassword)
            {
                ChangePasswordUserViewController changePasswordUserViewController_ = (ChangePasswordUserViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ChangeKeyUserViewController);
                if (changePasswordUserViewController_ != null)
                {
                    changePasswordUserViewController_.IsloadFromLogin = true;
                    this.NavigationController.PushViewController(changePasswordUserViewController_, true);
                    StopActivityIndicatorCustom();
                }
            }
            else
            {
                try
                {
                    if (ParametersManager.UserContext != null && ParametersManager.UserContext.Store == null)
                    {
                        GetUserAddress();
                    }
                    else
                    {
                        PresentLobbyView();
                    }
                }
                catch (Exception exception)
                {
                    Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.ValidateGenerateKey);
                    ShowMessageException(exception);
                }
            }
        }
        #endregion

        #region Methods Async 
        private async Task Login()
        {
            try
            {
                if (DeviceManager.Instance.IsNetworkAvailable().Result)
                {
                    StartActivityIndicatorCustom();
                    UserContext user = ParametersManager.UserContext;
                    UserCredentials userCredentials = new UserCredentials()
                    {
                        Email = emailTextField.Text,
                        Password = passwordTextField.Text
                    };
                    string messageValidation = _loginModel.ValidateFields(userCredentials);
                    if (string.IsNullOrEmpty(messageValidation))
                    {
                        LoginResponse = await _loginModel.Login(userCredentials);
                        ValidateResponseLogin();
                    }
                    else
                    {
                        var alertController = UIAlertController.Create(AppMessages.ApplicationName, messageValidation, UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                        PresentViewController(alertController, true, null);
                        StopActivityIndicatorCustom();
                    }
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.InternetErrorMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    PresentViewController(alertController, true, null);
                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.Login);
                ShowMessageException(exception);
            }
        }

        private void ValidateResponseLogin()
        {
            if (LoginResponse.Result != null && LoginResponse.Result.HasErrors && LoginResponse.Result.Messages != null)
            {
                if (LoginResponse.Result.Messages.Any())
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(LoginResponse.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    PresentViewController(alertController, true, null);
                    StopActivityIndicatorCustom();
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    PresentViewController(alertController, true, null);
                    StopActivityIndicatorCustom();
                }
            }
            else
            {
                if (LoginResponse != null && LoginResponse.User != null && !string.IsNullOrEmpty(LoginResponse.User.Id))
                {
                    this.SetUserResponse();
                }
                else
                {
                    StartActivityErrorMessage(EnumErrorCode.ErrorServiceUnavailable.ToString(), AppMessages.ErrorServicesUnavailables);
                }
            }
        }

        private void SetUserResponse()
        {
            try
            {
                UserContext user = ParametersManager.UserContext;
                UserContext userContext = ModelHelper.SetUserContext(LoginResponse.User);
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));

                RegisterEventLogin();

                InvokeOnMainThread(async () =>
                {
                    await this.RegisterCostumer();
                });

                this.ValidateGeneratePassword();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.SetUserResponse);
                ShowMessageException(exception);
            }
        }


        private void ValidateTextField() {

            emailTextField.EditingChanged += (object sender, EventArgs e) => {

                if (emailTextField.Text == "")
                {
                    TextField(emailTextField);
                    validateEmail.Hidden = true;

                }
                else
                {

                    if (Regex.Match(emailTextField.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                    {
                        ShowSuccessTextField(emailTextField);
                        validateEmail.Hidden = true;
                    }
                    else
                    {
                        ShowErrorTextField(emailTextField);
                        validateEmail.Hidden = false;
                    }
                }
            };
        }

        private void RegisterEventLogin()
        {
            FirebaseEventRegistrationService.Instance.SignIn();
            //FacebookRegistrationEventsService_Deprecated.Instance.LoginSuccessful();
        }

        private async Task GetUserAddress()
        {
            try
            {
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
                PresentLobbyView();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.InitialAccessViewController, ConstantMethodName.GetAddress);
            }
        }

        public async Task<bool> SetDefaultAddress(UserAddress address)
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

        private async Task RecoverPassword(UITextField textField)
        {
            try
            {
                StartActivityIndicatorCustom();
                _spinnerActivityIndicatorView.Message.Text = string.Empty;
                var passwordModel = new PasswordModel(new PasswordService(DeviceManager.Instance));
                string messageValidation = passwordModel.ValidateFieldsResetPassword(textField.Text.Trim());
                customSpinnerViewHeightConstraint.Constant = ConstantViewSize.MessageStatusViewSize;
                if (string.IsNullOrEmpty(messageValidation))
                {
                    var response = await passwordModel.ResetPassword(textField.Text);
                    spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        MessageStatusView messageView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                        CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                        messageView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        messageView_.Frame = messageViewFrame;
                        messageView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                        messageView_.Title.Text = AppMessages.SomethingsWrong;
                        messageView_.Message.Text = MessagesHelper.GetMessage(response.Result);
                        messageView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                        messageView_.Action.Layer.BackgroundColor = ConstantColor.UiMessageErrorButton.CGColor;
                        messageView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                        {
                            customSpinnerViewHeightConstraint.Constant = ConstantViewSize.RecoverPasswordViewSize;
                            messageView_.RemoveFromSuperview();
                            recoverPasswordView.Hidden = false;
                        };
                        messageView_.Close.TouchUpInside += (object sender, EventArgs e) =>
                        {
                            customSpinnerViewHeightConstraint.Constant = ConstantViewSize.RecoverPasswordViewSize;
                            messageView_.RemoveFromSuperview();
                            recoverPasswordView.Hidden = false;
                        };
                        customSpinnerView.AddSubview(messageView_);
                        customSpinnerView.Hidden = false;
                    }
                    else
                    {
                        RegisterEventRecoveryPassword(textField.Text);
                        MessageStatusView messageView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                        CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                        messageView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        messageView_.Frame = messageViewFrame;
                        messageView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Verificado);
                        messageView_.Title.Text = AppMessages.WellDone;
                        messageView_.Message.Text = AppMessages.ANewPasswordHasBeenSent;
                        messageView_.Action.SetTitle(AppMessages.AcceptButtonText, UIControlState.Normal);
                        messageView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                        {
                            spinnerActivityIndicatorView.StopAnimating();
                            spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundWaitData.CGColor;
                            messageView_.RemoveFromSuperview();
                            recoverPasswordView.RemoveFromSuperview();
                            customSpinnerViewHeightConstraint.Constant = ConstantViewSize.LoadingDataViewSize;
                            customSpinnerView.Hidden = true;
                        };
                        customSpinnerView.AddSubview(messageView_);
                        customSpinnerView.Hidden = false;
                    }
                }
                else
                {
                    //_spinnerActivityIndicatorView.RemoveFromSuperview();
                    spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                    MessageStatusView messageView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                    CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                    messageView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageView_.Frame = messageViewFrame;
                    messageView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                    messageView_.Title.Text = AppMessages.SomethingsWrong;
                    messageView_.Message.Text = messageValidation;
                    messageView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                    messageView_.Action.Layer.BackgroundColor = ConstantColor.UiMessageErrorButton.CGColor;
                    messageView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        customSpinnerViewHeightConstraint.Constant = ConstantViewSize.RecoverPasswordViewSize;
                        messageView_.RemoveFromSuperview();
                        recoverPasswordView.Hidden = false;
                    };
                    messageView_.Close.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        customSpinnerViewHeightConstraint.Constant = ConstantViewSize.RecoverPasswordViewSize;
                        messageView_.RemoveFromSuperview();
                        recoverPasswordView.Hidden = false;
                    };
                    customSpinnerView.AddSubview(messageView_);
                    customSpinnerView.Hidden = false;
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantMethodName.RecoverKey);
                ShowMessageException(exception);
            }
        }

        private void RegisterEventRecoveryPassword(string text)
        {
            //FirebaseEventRegistrationService.Instance.RecoveryPassword();
            //FacebookRegistrationEventsService_Deprecated.Instance.RecoveryPassword();
        }

        private UITextField ShowErrorTextField(UITextField textField)
        {
            textField.Layer.BorderColor = ConstantColor.UiValidError.CGColor;
            textField.Layer.BorderWidth = ConstantStyle.BorderWidthTextField;
            textField.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            textField.Layer.BackgroundColor = ConstantColor.UiValidError.ColorWithAlpha(0.5f).CGColor;
            return textField;
        }

        private UITextField TextField(UITextField textField)
        {
            textField.Layer.BorderColor = UIColor.GroupTableViewBackgroundColor.CGColor;
            textField.Layer.BorderWidth = ConstantStyle.BorderWidthTextField;
            textField.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            return textField;
        }

        private void ShowSuccessTextField(UITextField textField)
        {
            textField.Layer.BorderColor = ConstantColor.UiValidSuccess.CGColor;
            textField.Layer.BackgroundColor = ConstantColor.UiValidSuccess.ColorWithAlpha(0.5f).CGColor;
            textField.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            textField.Layer.BorderWidth = ConstantStyle.BorderWidthTextField;
        }

        public async Task RegisterCostumer()
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

        #endregion

        #region Events 
        partial void LoginButton_UpInside(UIButton sender)
        {
            try
            {
                passwordTextField.ResignFirstResponder();
                emailTextField.ResignFirstResponder();
                customSpinnerViewHeightConstraint.Constant = ConstantViewSize.LoadingDataViewSize;
                StartActivityIndicatorCustom();
                Login();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.LoginViewController, ConstantEventName.LoginClicked);
                ShowMessageException(exception);
            }
        }

        partial void ForgetPasswordbtn_TouchUpInside(UIButton sender)
        {
            if(_spinnerActivityIndicatorView != null)
            {
                _spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            customSpinnerView.LayoutIfNeeded();
            customSpinnerViewHeightConstraint.Constant = ConstantViewSize.RecoverPasswordViewSize;
            customSpinnerView.BackgroundColor = UIColor.Clear;
            spinnerActivityIndicatorView.StartAnimating();
            spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
            recoverPasswordView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.RecoverKeyView, Self, null).GetItem<RecoverPasswordView>(0);
            CGRect recoverPasswordViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.RecoverPasswordViewSize);
            recoverPasswordView.Frame = recoverPasswordViewFrame;
            recoverPasswordView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            recoverPasswordView.Close.TouchUpInside += (object sender2, EventArgs e) =>
            {
                recoverPasswordView.RemoveFromSuperview();
                customSpinnerView.Hidden = true;
                customSpinnerView.BackgroundColor = UIColor.White;
                spinnerActivityIndicatorView.StopAnimating();
                spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundWaitData.CGColor;
                _spinnerActivityIndicatorView.Image.Hidden = false;
            };

            recoverPasswordView.Send.TouchUpInside += (object senderAction, EventArgs e) =>
            {
                spinnerActivityIndicatorView.StopAnimating();
                spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundWaitData.CGColor;
                recoverPasswordView.Hidden = true;
                RecoverPassword(recoverPasswordView.Email);
            };
            customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            customSpinnerView.AddSubview(recoverPasswordView);
            customSpinnerView.Hidden = false;

            RegisterEventForgotPassword();
        }

        private void RegisterEventForgotPassword()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEvent(AnalyticsEvent.LoginForgotPassword);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEvent(AnalyticsEvent.LoginForgotPassword);
        }

        private void ShowHiddenPasswordButtonTouchUpInside(object sender, EventArgs events)
        {
            passwordTextField.SecureTextEntry = !passwordTextField.SecureTextEntry;
        }

        partial void ReturnLogin_UpInside(UIButton sender)
        {
            spinnerActivityIndicatorView.StopAnimating();
            spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            contentViewSkipeLogin.Hidden = true;
        }

        partial void registerButton_UpInside(UIButton sender)
        {
            RegistryUserViewController registryUserViewController_ = (RegistryUserViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.RegistryUserViewController);
            this.NavigationController.PushViewController(registryUserViewController_, true);
            RegisterEventSignUp();
        }

        private void RegisterEventSignUp()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEvent(AnalyticsEvent.LoginSignUp);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEvent(AnalyticsEvent.LoginSignUp);
        }

        private void TextFieldEditingChanged(object sender, EventArgs e)
        {
            ValidateContinueButton();
        }

        private bool EmailTextFieldShouldReturn(UITextField textField)
        {
            passwordTextField.BecomeFirstResponder();
            ValidateContinueButton();
            return true;
        }

        private bool PasswordTextFieldShouldReturn(UITextField textField)
        {
            textField.ResignFirstResponder();
            ValidateContinueButton();
            return true;
        }

        private void ValidateContinueButton()
        {
            if (!emailTextField.Text.Trim().Equals(string.Empty) &&
                !passwordTextField.Text.Trim().Equals(string.Empty))
            {
                loginButton.BackgroundColor = ConstantColor.UiPrimary;
                loginButton.Enabled = true;
            }
            else
            {
                loginButton.BackgroundColor = UIColor.LightGray.ColorWithAlpha(0.3f);
                loginButton.Enabled = false;
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            StopActivityIndicatorCustom();
        }


        #endregion
    }
}