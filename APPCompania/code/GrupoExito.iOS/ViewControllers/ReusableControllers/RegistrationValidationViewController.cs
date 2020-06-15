using System;
using System.Text.RegularExpressions;
using Foundation;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.ViewModels.Registration;
using GrupoExito.Utilities.Resources;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class RegistrationValidationViewController : UIViewController
    {
        RegistrationValidationViewModel viewModel;
        private bool isSpinnerAdded = false;
        private CustomSpinnerView customSpinnerView;
        private int validationStatus;

        public Action<bool> OperationDoneAction { get; set; }
        public Action OperationCanceledAction { get; set; }
        public string Cellphone { get; internal set; }
        public string DocumentNumber { get; internal set; }
        public bool FromMyDiscount { get; internal set; }
        public bool IsValidationModified { get; set; }

        public RegistrationValidationViewController() : base("RegistrationValidationViewController", null)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            var arr = NSBundle.MainBundle.LoadNib(nameof(RegistrationValidationViewController), this, null);
            var v = Runtime.GetNSObject<UIView>(arr.ValueAt(0));

            View = v;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            customSpinnerView = CustomSpinnerView.Create();

            viewModel = new RegistrationValidationViewModel(ParametersManager.UserContext, DeviceManager.Instance);

            contentView.Layer.CornerRadius = 10;
            validateButton.Layer.CornerRadius = 10;
            errorView.Layer.CornerRadius = 10;
            closeErrorButton.Layer.CornerRadius = 10;

            codeTextField.Delegate = this;

            editButton.TouchUpInside += EditButton_TouchUpInside;

            exitButton.TouchUpInside += Cancel_TouchUpInside;
            cancelButton.TouchUpInside += Cancel_TouchUpInside;
            validateButton.TouchUpInside += ActionButton_TouchUpInside;

            errorView.Hidden = true;
            closeErrorButton.TouchUpInside += (sender, e) =>
            {
                errorView.Hidden = true;
                contentView.Hidden = false;
            };

            cornerErrorButton.TouchUpInside += (sender, e) =>
            {
                errorView.Hidden = true;
                contentView.Hidden = false;
            };
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
        }

        private void NoPhoneSetStatus(bool fromEditView = false)
        {
            descriptionLabel.Text = AppMessages.EntryNewVerificationPhone;
            cellphoneLabel.Hidden = true;
            codeTextField.Text = "";
            codeTextField.Hidden = false;
            codeTextField.BecomeFirstResponder();
            validateButton.SetTitle(AppMessages.GoBack, UIControlState.Normal);

            cancelButton.SetAttributedTitle(new NSAttributedString(AppMessages.GenericBackAction), UIControlState.Normal);

            cancelButton.Hidden = false;
            validationStatus = 0;
            editButton.Hidden = true;

            if(fromEditView)
            {
                cancelButton.TouchUpInside -= Cancel_TouchUpInside;
                cancelButton.TouchUpInside += Return_TouchUpInside;
            }
        }

        private void SendSMSStatus()
        {
            descriptionLabel.Text = String.Format(AppMessages.VerifyMessageWillBeSent, "");
            cellphoneLabel.Text = Cellphone;
            cellphoneLabel.Hidden = false;
            codeTextField.Hidden = true;
            validateButton.SetTitle("ENVIAR", UIControlState.Normal);
            cancelButton.Hidden = true;
            validationStatus = 1;

            editButton.Hidden = false;

            contentView.SetNeedsLayout();
            contentView.LayoutIfNeeded();

            cancelButton.TouchUpInside -= Return_TouchUpInside;
            cancelButton.TouchUpInside += Cancel_TouchUpInside;
        }

        private void VerifyStatus()
        {
            if (FromMyDiscount)
            {
                descriptionLabel.Text = String.Format(AppMessages.EntryVerificationCodeReceived, "");
                cellphoneLabel.Text = Cellphone;
                cellphoneLabel.Hidden = false;
            }
            else
            {
                descriptionLabel.Text = String.Format(AppMessages.VerifyMessageSent, "");
                cellphoneLabel.Text = Cellphone;
                cellphoneLabel.Hidden = false;
            }

            codeTextField.Hidden = false;
            codeTextField.Text = "";
            codeTextField.BecomeFirstResponder();
            validateButton.SetTitle(AppMessages.Verify, UIControlState.Normal);
            cancelButton.Hidden = false;
            cancelButton.TouchUpInside -= Return_TouchUpInside;
            cancelButton.TouchUpInside -= Cancel_TouchUpInside;
            cancelButton.TouchUpInside += ResendCode_TouchUpInside;
            cancelButton.SetAttributedTitle(new NSAttributedString(AppMessages.ResendCode), UIControlState.Normal);

            validationStatus = 2;

            editButton.Hidden = true;
        }

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            if (parent != null)
            {
                if (!string.IsNullOrEmpty(Cellphone))
                {
                    if(FromMyDiscount)
                    {
                        SendSMSStatus();
                    }
                    else
                    {
                        VerifyUserParameters params1 = new VerifyUserParameters
                        {
                            CellPhone = Cellphone,
                            SiteId = AppServiceConfiguration.SiteId,
                            DocumentNumber = ParametersManager.UserContext != null ? ParametersManager.UserContext.DocumentNumber : DocumentNumber
                        };

                        viewModel.SendMessageCommand.Execute(params1);
                    }
                }
                else
                {
                    NoPhoneSetStatus();
                }
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        void ActionButton_TouchUpInside(object sender, EventArgs e)
        {
            switch (validationStatus)
            {
                case 0:
                    if (!string.IsNullOrEmpty(codeTextField.Text) && codeTextField.Text.Length == 10)
                    {
                        var mobileOperator = codeTextField.Text.Substring(0, 3);
                        var validMobileOperator = Regex.IsMatch(mobileOperator, AppConfigurations.MobilePhoneFormat);

                        if (validMobileOperator)
                        {
                            Cellphone = codeTextField.Text;
                            codeTextField.ResignFirstResponder();
                            SendSMSStatus();
                        }
                        else
                        {
                            ShowError("", AppMessages.MobileNumberOperatorValidationText);
                        }
                    }
                    else
                    {
                        ShowError("", AppMessages.MobileNumberOperatorValidationText);
                    }
                    break;
                case 1:
                    VerifyUserParameters params1 = new VerifyUserParameters
                    {
                        CellPhone = Cellphone,
                        SiteId = AppServiceConfiguration.SiteId,
                        DocumentNumber = ParametersManager.UserContext != null ? ParametersManager.UserContext.DocumentNumber : DocumentNumber
                    };

                    viewModel.SendMessageCommand.Execute(params1);
                    break;
                case 2:
                    VerifyUserParameters params2 = new VerifyUserParameters
                    {
                        CellPhone = Cellphone,
                        SiteId = AppServiceConfiguration.SiteId,
                        DocumentNumber = ParametersManager.UserContext != null ? ParametersManager.UserContext.DocumentNumber : DocumentNumber,
                        Code = codeTextField.Text
                    };

                    viewModel.ValidateCommand.Execute(params2);
                    break;
                default:
                    break;
            }
        }

        void EditButton_TouchUpInside(object sender, EventArgs e)
        {
            NoPhoneSetStatus(true);
        }

        void ResendCode_TouchUpInside(object sender, EventArgs e)
        {
            VerifyUserParameters params1 = new VerifyUserParameters
            {
                CellPhone = Cellphone,
                SiteId = AppServiceConfiguration.SiteId,
                DocumentNumber = ParametersManager.UserContext != null ? ParametersManager.UserContext.DocumentNumber : DocumentNumber
            };

            viewModel.SendMessageCommand.Execute(params1);
        }

        void Cancel_TouchUpInside(object sender, EventArgs e)
        {
            OperationCanceledAction?.Invoke();
        }

        void Return_TouchUpInside(object sender, EventArgs e)
        {
            codeTextField.ResignFirstResponder();
            SendSMSStatus();
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                var propertyName = e.PropertyName;
                switch (propertyName)
                {
                    case nameof(viewModel.IsBusy):
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (viewModel.IsBusy && !isSpinnerAdded)
                                    ShowSpinner();
                                else if (!viewModel.IsBusy)
                                    HideSpinner();
                            });
                        }
                        break;
                    case nameof(viewModel.InvalidEntries):
                        {
                        }
                        break;
                    case nameof(viewModel.ConnectionUnavailable):
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (!viewModel.ConnectionUnavailable)
                                {
                                    GenericErrorView genericErrorView = GenericErrorView.Create();
                                    genericErrorView.Configure(nameof(EnumErrorCode.InternetErrorMessage), AppMessages.InternetErrorMessage, (senderError, eError) => { genericErrorView.RemoveFromSuperview(); });
                                    genericErrorView.Frame = View.Bounds;
                                    View.AddSubview(genericErrorView);
                                }
                            });
                        }
                        break;
                    case nameof(viewModel.RegistrationValidated):
                        {
                            InvokeOnMainThread(() =>
                            {
                                IsValidationModified = false;
                                OperationDoneAction?.Invoke(viewModel.RegistrationValidated);
                            });
                        }
                        break;
                    case nameof(viewModel.MessageSent):
                        {
                            InvokeOnMainThread(() =>
                            {
                                if (viewModel.MessageSent)
                                {
                                    IsValidationModified = true;
                                    VerifyStatus();
                                }
                                else
                                {
                                    ShowError("", AppMessages.SedSMSError);
                                }
                            });
                        }
                        break;
                    case nameof(viewModel.Exception):
                        {
                            ShowError(viewModel.Exception.Data[nameof(EnumExceptionDataKeys.Code)].ToString(), viewModel.Exception.Data[nameof(EnumExceptionDataKeys.Message)].ToString());
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Util.LogException(ex, nameof(RegistrationValidationViewController), nameof(ViewModel_PropertyChanged));
            }
        }

        private void ShowSpinner()
        {
            if (!isSpinnerAdded)
            {
                isSpinnerAdded = true;
                View.AddSubview(customSpinnerView);
                customSpinnerView.Frame = View.Bounds;
                customSpinnerView.Start();
            }
        }

        private void HideSpinner()
        {
            if (isSpinnerAdded)
            {
                isSpinnerAdded = false;
                customSpinnerView.Stop();
                customSpinnerView.RemoveFromSuperview();
            }
        }

        private void ShowError(string code, string message)
        {
            InvokeOnMainThread(() =>
            {
                codeTextField.ResignFirstResponder();
                errorView.Hidden = false;
                contentView.Hidden = true;

                errorTitleLabel.Text = AppMessages.SomethingsWrong;
                errorDescriptionLabel.Text = message;

                if(!FromMyDiscount)
                {
                    closeErrorButton.TouchUpInside += (sender, e) => OperationCanceledAction?.Invoke();
                    cornerErrorButton.TouchUpInside += (sender, e) => OperationCanceledAction?.Invoke();
                }
            });
        }
    }

    public partial class RegistrationValidationViewController : IUITextFieldDelegate
    {
        [Export("textField:shouldChangeCharactersInRange:replacementString:")]
        public virtual bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            NSMutableAttributedString currentText = new NSMutableAttributedString(textField.Text);
            currentText.Replace(range, replacementString);
            return validationStatus == 0 || currentText.Length <= 6;
        }
    }
}