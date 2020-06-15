using System;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
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
    public partial class ChangePasswordUserViewController : UIViewControllerBase
    {
        #region Attributes
        private bool isloadFromLogin = false;
        private PasswordModel _passwordModel;
        #endregion

        #region Properties
        public bool IsloadFromLogin { get => isloadFromLogin; set => isloadFromLogin = value; }
        #endregion

        #region Constructors 
        public ChangePasswordUserViewController(IntPtr handle) : base(handle)
        {
            _passwordModel = new PasswordModel(new PasswordService(DeviceManager.Instance));
        }
        #endregion

        #region life Cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ChangePassword, nameof(ChangePasswordUserViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {                
                this.LoadExternalViews();
                this.LoadFonts();
                this.LoadCorners();
                this.LoadHandlers();
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ChangeKeyUserViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(IsloadFromLogin, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ChangeKeyUserViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
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
        private void LoadFonts()
        {
            try
            {
                ///Load font size and style
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ChangeKeyUserViewController, ConstantMethodName.LoadFonts);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                UpdateButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
                UpdateButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
                UpdateButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ChangeKeyUserViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ChangeKeyUserViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            UpdateButton.TouchUpInside += UpdatePasswordUpInside;
            ShowHiddenCurrentPassword.TouchUpInside += ShowHiddenCurrentPasswordUpInside;
            ShowHiddenNewPasswordButton.TouchUpInside += ShowHiddenNewPasswordUpInside;
            ShowHiddenConfirmNewButton.TouchUpInside += ShowHiddenConfirmNewPasswordUpInside;
            CurrentPasswordTextField.ShouldReturn += CurrentPasswordTextFieldShouldReturn;
            NewPasswordTextField.ShouldReturn += NewPasswordTextFieldShouldReturn;
            ConfirmNewPasswordTextField.ShouldReturn += ConfirmNewPasswordTextFieldShouldReturn;
        }

        private void UnsubscribeHandlers()
        {
            UpdateButton.TouchUpInside -= UpdatePasswordUpInside;
            ShowHiddenCurrentPassword.TouchUpInside -= ShowHiddenCurrentPasswordUpInside;
            ShowHiddenNewPasswordButton.TouchUpInside -= ShowHiddenNewPasswordUpInside;
            ShowHiddenConfirmNewButton.TouchUpInside -= ShowHiddenConfirmNewPasswordUpInside;
            CurrentPasswordTextField.ShouldReturn -= CurrentPasswordTextFieldShouldReturn;
            NewPasswordTextField.ShouldReturn -= NewPasswordTextFieldShouldReturn;
            ConfirmNewPasswordTextField.ShouldReturn -= ConfirmNewPasswordTextFieldShouldReturn;
        }

        private UserCredentials GetUserCredential()
        {
            return new UserCredentials()
            {
                OldPassword = CurrentPasswordTextField.Text,
                NewPassword = NewPasswordTextField.Text,
                ConfirmPassword = ConfirmNewPasswordTextField.Text
            };
        }

        private async Task ChangePasswordAsync()
        {
            await ChangePassword();
        }

        private async Task ChangePassword()
        {
            try
            {
                StartActivityIndicatorCustom();
                var userCredential = GetUserCredential();
                var message = _passwordModel.ValidateFieldsChangePassword(userCredential);

                if (string.IsNullOrEmpty(message))
                {
                    var response = await _passwordModel.ChangePassword(userCredential);
                    customSpinnerView.LayoutIfNeeded();
                    _spinnerActivityIndicatorView.RemoveFromSuperview();

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        customSpinnerView.Hidden = false;
                        customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        _spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        customSpinnerViewHeightConstraint.Constant = ConstantViewSize.MessageStatusViewSize;
                        spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                        spinnerActivityIndicatorView.StartAnimating();
                        MessageStatusView messageStatusView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                        CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerViewHeightConstraint.Constant);
                        messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        messageStatusView_.Frame = messageViewFrame;
                        messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        messageStatusView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                        messageStatusView_.Title.Text = AppMessages.SomethingsWrong;
                        messageStatusView_.Message.Text = MessagesHelper.GetMessage(response.Result);
                        messageStatusView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                        messageStatusView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                        {
                            messageStatusView_.RemoveFromSuperview();
                            spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundWaitData.CGColor;
                            spinnerActivityIndicatorView.StopAnimating();
                            customSpinnerView.Hidden = true;
                        };

                        customSpinnerView.AddSubview(messageStatusView_);
                    }
                    else
                    {
                        customSpinnerView.Hidden = false;
                        spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                        spinnerActivityIndicatorView.StartAnimating();
                        MessageStatusView messageView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                        CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerViewHeightConstraint.Constant);
                        messageView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                        messageView_.Frame = messageViewFrame;
                        messageView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Verificado);
                        messageView_.Title.Text = AppMessages.WellDone;
                        messageView_.Message.Text = AppMessages.ChangePasswordMessage;
                        messageView_.Action.SetTitle(AppMessages.AcceptButtonText, UIControlState.Normal);
                        messageView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                        {
                            if (IsloadFromLogin)
                            {
                                StopActivityIndicatorCustom();
                                PresentLobbyView();
                            }
                            else
                            {
                                this.NavigationController.PopViewController(true);
                            }
                            spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundWaitData.CGColor;
                        };
                        customSpinnerView.AddSubview(messageView_);
                        customSpinnerView.Hidden = false;
                    }
                }
                else
                {
                    StopActivityIndicatorCustom();
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default,
                                                         action => { }));
                    PresentViewController(alertController, true, null);
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.ChangeKeyUserViewController, ConstantMethodName.UpdateKey);
                ShowMessageException(exception);
            }
        }

        #endregion

        #region Events
        private void UpdatePasswordUpInside(object sender, EventArgs e)
        {
            LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
            ChangePasswordAsync();
        }

        private void ShowHiddenNewPasswordUpInside(object sender, EventArgs e)
        {
            NewPasswordTextField.SecureTextEntry = !NewPasswordTextField.SecureTextEntry;
        }

        private void ShowHiddenConfirmNewPasswordUpInside(object sender, EventArgs e)
        {
            ConfirmNewPasswordTextField.SecureTextEntry = !ConfirmNewPasswordTextField.SecureTextEntry;
        }

        private void ShowHiddenCurrentPasswordUpInside(object sender, EventArgs e)
        {
            CurrentPasswordTextField.SecureTextEntry = !CurrentPasswordTextField.SecureTextEntry;
        }

        private bool CurrentPasswordTextFieldShouldReturn(UITextField textField)
        {
            CurrentPasswordTextField.ResignFirstResponder();
            NewPasswordTextField.BecomeFirstResponder();
            return true;
        }

        private bool NewPasswordTextFieldShouldReturn(UITextField textField)
        {
            NewPasswordTextField.ResignFirstResponder(); ;
            ConfirmNewPasswordTextField.BecomeFirstResponder();
            return true;
        }

        private bool ConfirmNewPasswordTextFieldShouldReturn(UITextField textField)
        {
            ConfirmNewPasswordTextField.ResignFirstResponder();
            return true;
        }

        #endregion
    }
}

