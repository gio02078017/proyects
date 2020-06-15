using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UIKit;
using WebKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class RegistryUserViewController : UIViewControllerBase, IUITextViewDelegate
    {
        #region Attributes
        private string gender;
        private string addressType;
        private User user;
        private ClientResponse SignUpResponse;
        private SignUpModel _signUpModel;
        private DocumentTypesModel _documentTypesModel;
        private IList<DocumentType> documentsType;
        private int PosDocument;
        #endregion

        #region Properties
        public string Gender { get => gender; set => gender = value; }
        public string AddressType { get => addressType; set => addressType = value; }
        public User User { get => user; set => user = value; }
        public bool UserValidated { get; private set; }

        private NSString termsAndConditions = new NSString("termsAndConditions");
        private NSString habeasData = new NSString("habeasData");
        #endregion

        #region Constructors
        public RegistryUserViewController(IntPtr handle) : base(handle)
        {
            _signUpModel = new SignUpModel(new SignUpService(DeviceManager.Instance));
            _documentTypesModel = new DocumentTypesModel(new DocumentTypesService(DeviceManager.Instance));
            documentsType = new List<DocumentType>();
        }
        #endregion

        #region life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {
                this.LoadHabeasData();
                this.LoadExternalViews();
                this.LoadCorners();
                this.LoadDocumentsType();
                this.LoadHandlers();
                this.LoadData();
                this.ValidateSizeOffields();
                StopActivityIndicatorCustom();

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.SignUp, nameof(RegistryUserViewController));
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                this.NavigationController.NavigationBarHidden = false;
                NavigationView = this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1] as NavigationHeaderView;
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();

            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods

        private void LoadHabeasData()
        {
            NSMutableAttributedString attributedOriginalText = new NSMutableAttributedString(AppMessages.HabeasDataAuthorizationText);

            NSRange range1 = attributedOriginalText.MutableString.LocalizedStandardRangeOfString(new NSString("términos y condiciones"));
            attributedOriginalText.AddAttribute(UIStringAttributeKey.Link, termsAndConditions, range1);
            attributedOriginalText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterSubtitle, 13), range1);

            NSRange range2 = attributedOriginalText.MutableString.LocalizedStandardRangeOfString(new NSString("tratamiento de datos personales"));
            attributedOriginalText.AddAttribute(UIStringAttributeKey.Link, habeasData, range2);
            attributedOriginalText.AddAttribute(UIStringAttributeKey.Font, UIFont.FromName(ConstantFontSize.LetterSubtitle, 13), range2);

            habeasDataTextView.AttributedText = attributedOriginalText;

            habeasDataTextView.Editable = false;
            habeasDataTextView.Delegate = this;
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                this.NavigationController.NavigationBarHidden = false;
                NavigationView.LoadWidthSuperView();
                AddressTypeView addressTypeView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.AddressTypeView, Self, null).GetItem<AddressTypeView>(0);
                CGRect addressTypeViewFrame = new CGRect(0, 0, addressTypeView.Frame.Size.Width, addressTypeView.Frame.Size.Height);
                addressTypeView_.Frame = addressTypeViewFrame;
                addressTypeView.AddSubview(addressTypeView_);
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
                containerGenericPickerView.Hidden = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }


        private void ValidateSizeOffields()
        {
            documentNumberTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 13;
            };

            nameTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 40;
            };

            lastNameTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 40;
            };

            telephoneCelTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 10;
            };
        }

        public void LoadHandlers()
        {
            try
            {
                nameTextField.EditingChanged += (object sender, EventArgs e) =>
                {
                    nameMessageLabel.Hidden = ((UITextField)sender).Text.Length <= 1 ? false : true;
                };

                lastNameTextField.EditingChanged += (object sender, EventArgs e) =>
                {
                    lastNameMessageLabel.Hidden = ((UITextField)sender).Text.Length <= 1 ? false : true;
                };

                documentNumberTextField.EditingChanged += (object sender, EventArgs e) =>
                {
                    if (((UITextField)sender).Text.Length > 13)
                    {
                        ((UITextField)sender).Text = documentNumberTextField.Text;
                    }
                };

                telephoneCelTextField.EditingChanged += (object sender, EventArgs e) =>
                {
                    if (((UITextField)sender).Text.Length > 13)
                    {
                        ((UITextField)sender).Text = telephoneCelTextField.Text;
                    }
                };

                emailTextField.EditingChanged += (object sender, EventArgs e) =>
                {

                    if (emailTextField.Text == "")
                    {
                        TextField(emailTextField);
                        emailLabel.Hidden = true;

                    }
                    else
                    {
                        if (Regex.Match(emailTextField.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                        {
                            ShowSuccessTextField(emailTextField);
                            emailLabel.Hidden = true;
                        }
                        else
                        {
                            ShowErrorTextField(emailTextField);
                            emailLabel.Hidden = false;
                        }
                    }
                };

                passwordConfirmTextField.EditingChanged += (object sender, EventArgs e) =>
                {
                    passwordConfirmTextField.Text = passwordConfirmTextField.Text.Trim();
                    if (passwordConfirmTextField.Text == "")
                    {
                        TextField(passwordConfirmTextField);
                        passwordConfirmLabel.Hidden = true;
                    }
                    else
                    {
                        if (passwordConfirmTextField.Text != passwordTextField.Text)
                        {
                            ShowErrorTextField(passwordConfirmTextField);
                            passwordConfirmLabel.Hidden = false;
                            TextField(passwordTextField);

                        }
                        else
                        {
                            ShowSuccessTextField(passwordConfirmTextField);
                            ShowSuccessTextField(passwordTextField);
                            passwordConfirmLabel.Hidden = true;
                        }
                    }
                };

                passwordTextField.EditingChanged += (object sender, EventArgs e) =>
                {
                    passwordTextField.Text = passwordTextField.Text.Trim();
                    if (passwordTextField.Text == "")
                    {
                        TextField(passwordTextField);
                        passwordMessageLabel.Text = "8 Carácteres entre números y letras.";
                        passwordMessageLabel.MinimumScaleFactor = 13;
                        passwordMessageLabel.TextColor = UIColor.LightGray;

                    }
                    else
                    {
                        if (passwordTextField.Text.Length <= 8)
                        {
                            passwordMessageLabel.MinimumScaleFactor = 13;
                            passwordMessageLabel.TextColor = UIColor.Red;
                            passwordMessageLabel.Text = "Tu contraseña es muy corta";
                            ShowErrorTextField(passwordTextField);
                        }
                        else
                        {
                            ShowSuccessTextField(passwordTextField);
                            passwordMessageLabel.Text = "8 Carácteres entre números y letras.";
                            passwordMessageLabel.MinimumScaleFactor = 13;
                            passwordMessageLabel.TextColor = UIColor.LightGray;
                        }

                    }
                };

                nameTextField.ShouldReturn = (textField) =>
                {

                    if (!string.IsNullOrEmpty(textField.Text.Trim()))
                    {
                        ShowSuccessTextField(ref textField);

                    }
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        lastNameTextField.BecomeFirstResponder();
                    }
                    return true;
                };

                lastNameTextField.ShouldReturn = (textField) =>
                {
                    if (!string.IsNullOrEmpty(textField.Text.Trim()))
                    {
                        ShowSuccessTextField(ref textField);
                    }
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        documentNumberTextField.BecomeFirstResponder();
                    }
                    return true;
                };

                documentNumberTextField.ShouldReturn = (textField) =>
                {
                    if (!string.IsNullOrEmpty(textField.Text.Trim()))
                    {
                        ShowSuccessTextField(ref textField);
                    }
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        telephoneCelTextField.BecomeFirstResponder();
                    }
                    return true;
                };

                telephoneCelTextField.ShouldReturn = (textField) =>
                {
                    if (!string.IsNullOrEmpty(textField.Text.Trim()))
                    {
                        ShowSuccessTextField(ref textField);
                    }
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        aditionalDataAddressTextField.BecomeFirstResponder();
                    }
                    return true;
                };

                aditionalDataAddressTextField.ShouldReturn = (textField) =>
                {
                    if (!string.IsNullOrEmpty(textField.Text.Trim()))
                    {
                        ShowSuccessTextField(ref textField);
                    }
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        emailTextField.BecomeFirstResponder();
                    }
                    return true;
                };



                emailTextField.ShouldReturn = (textField) =>
                {
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        passwordTextField.BecomeFirstResponder();
                    }
                    return true;
                };

                passwordTextField.ShouldReturn = (textField) =>
                {
                    if (ValidateTextFields())
                    {
                        textField.ResignFirstResponder();
                    }
                    else
                    {
                        passwordConfirmTextField.BecomeFirstResponder();
                    }
                    return true;
                };

                passwordConfirmTextField.ShouldReturn = (textField) =>
                {
                    textField.ResignFirstResponder();
                    return true;
                };
                closeGenericPickerButton.TouchUpInside += CloseGenericPickerUpInside;
                dateBirthButton.TouchUpInside += DayBirthDateUpInside;
                typeButton.TouchUpInside += DocumentTypeUpInside;
                registerDataButton.TouchUpInside += RegisterUserButtonUpInside;
                showHidePasswordButton.TouchUpInside += ShowHiddenPasswordButtonTouchUpInside;
                showHideConfirmPasswordButton.TouchUpInside += ShowHiddenConfirmPasswordButtonTouchUpInside;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.LoadEventsTextField);
                ShowMessageException(exception);
            }
        }

        private void LoadCorners()
        {
            try
            {
                registerDataButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.LoadCorners);
                ShowMessageException(exception);
            }
        }

        [Export("textView:shouldInteractWithURL:inRange:")]
        public bool ShouldInteractWithUrl(UITextView view, NSUrl url, NSRange range)
        {
            if (url.AbsoluteString.Equals(termsAndConditions))
            {
                PerformSegue("TermsAndConditionsSegue", null);
            }
            else if (url.AbsoluteString.Equals(habeasData))
            {
                var habeasDataUrl = new NSUrl(AppServiceConfiguration.PersonalDataManagementUrl);
                var req = new NSUrlRequest(habeasDataUrl);
                UIViewController habeasDataViewController = new UIViewController();
                WKWebView webView = new WKWebView(View.Bounds, new WKWebViewConfiguration());
                habeasDataViewController.View.AddSubview(webView);
                var request = new NSUrlRequest(habeasDataUrl);
                webView.LoadRequest(request);
                NavigationController.PushViewController(habeasDataViewController, true);
            }
            return false;
        }

        public async Task LoadDocumentsType()
        {
            try
            {
                var response = await _documentTypesModel.GetDocumentTypes();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    PresentViewController(alertController, true, null);
                }
                else
                {
                    documentsType = response.DocumentTypes;
                    PullSpinners();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.GetDocumentTypes);
                ShowMessageException(exception);
            }
        }

        private void PullSpinners()
        {
            documentTypePickerView.DataSource = new PickerViewSource(documentsType);
            documentTypePickerView.Delegate = new PickerViewDelegate(documentsType, documentTypeTextField);
        }

        private User SetUser()
        {
            try
            {
                return new User()
                {
                    FirstName = nameTextField.Text,
                    LastName = lastNameTextField.Text,
                    DocumentType = int.Parse(documentsType[PosDocument].Id),
                    DocumentNumber = documentNumberTextField.Text,
                    DateOfBirth = this.GetDateOfBirth(),
                    CellPhone = telephoneCelTextField.Text,
                    Email = emailTextField.Text,
                    Password = passwordTextField.Text,
                    ConfirmPassword = passwordConfirmTextField.Text,
                    AcceptTerms = aceptConditionSwitch.On
                };
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.SetUser);
                ShowMessageException(exception);
                return null;
            }
        }

        private string GetDateOfBirth()
        {
            try
            {
                NSDateFormatter dateFormatter = new NSDateFormatter
                {
                    DateFormat = AppConfigurations.DateFormat
                };
                string dateOfBirth = dateFormatter.ToString(genericPickerDateView.Date);
                return dateOfBirth;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void ValidateResponseSignUp()
        {
            if (SignUpResponse.Result != null && SignUpResponse.Result.HasErrors && SignUpResponse.Result.Messages != null)
            {

                var errorClientExists = SignUpResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ClientExists)).Any();

                if (errorClientExists)
                {
                    var errorCode = SignUpResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ClientExists)).First();
                }

                StopActivityIndicatorCustom();
                var alertController = UIAlertController.Create(AppMessages.ApplicationName, MessagesHelper.GetMessage(SignUpResponse.Result), UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                PresentViewController(alertController, true, null);
            }
            else
            {
                User.Id = SignUpResponse.Id;
                UserContext userContext = ModelHelper.SetUserContext(User);
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                this.SuccessSignUp();
            }
        }

        private void LoadData()
        {
            try
            {
                if (ParametersManager.UserContext != null && ParametersManager.UserContext.Address != null)
                {
                    addressRegisterLabel.Text = ParametersManager.UserContext.Address.AddressComplete;
                }
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.LoadData);
                ShowMessageException(exception);
            }
        }

        private void SuccessSignUp()
        {
            try
            {
                RegisterEventSignUp();
                RegistryUserSuccessViewController registryUserSuccessViewController_ = (RegistryUserSuccessViewController)Storyboard.InstantiateViewController(ConstantControllersName.RegistryUserSuccessViewController);
                if (registryUserSuccessViewController_ != null)
                {
                    this.NavigationController.PushViewController(registryUserSuccessViewController_, true);
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.SuccessSignUp);
                ShowMessageException(exception);
            }
        }

        private void RegisterEventSignUp()
        {
            FirebaseEventRegistrationService.Instance.SignUp();
            FacebookEventRegistrationService.Instance.CompleteRegistration("normal");
        }

        private void RegisterUser()
        {
            try
            {
                User = this.SetUser();
                string message = _signUpModel.ValidateFields(User);
                if (string.IsNullOrEmpty(message))
                {
                    RegistrationValidationViewController validationViewController = new RegistrationValidationViewController();

                    validationViewController.OperationDoneAction += (result) =>
                    {
                        UserValidated = result;
                        RemoveChild(validationViewController);
                        this.NavigationController.NavigationBarHidden = false;

                        if (result)
                        {
                            base.StartActivityIndicatorCustom();
                            RegisterUserAsync();
                        }
                        else
                        {
                            ShowError(AppMessages.VerifyUserError);
                        }
                    };

                    validationViewController.OperationCanceledAction += () =>
                    {
                        RemoveChild(validationViewController);
                        this.NavigationController.NavigationBarHidden = false;
                    };

                    AddChildViewController(validationViewController);

                    this.NavigationController.NavigationBarHidden = true;
                    validationViewController.View.LayoutIfNeeded();
                    validationViewController.View.Frame = View.Bounds;
                    validationViewController.Cellphone = telephoneCelTextField.Text;
                    validationViewController.DocumentNumber = documentNumberTextField.Text;
                    View.AddSubview(validationViewController.View);
                    validationViewController.DidMoveToParentViewController(this);
                }
                else
                {
                    if (message.Equals(AppMessages.RequiredFieldsText))
                    {
                        this.ModifyFieldsStyle();
                    }

                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, message, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    PresentViewController(alertController, true, null);
                }
                StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantMethodName.RegisterUser);
                ShowMessageException(exception);
                StopActivityIndicatorCustom();
            }
        }

        private async Task RegisterUserAsync()
        {
            SignUpResponse = await _signUpModel.RegisterUser(User);
            this.ValidateResponseSignUp();
        }

        private void RemoveChild(UIViewController vc)
        {
            vc.WillMoveToParentViewController(null);
            vc.View.RemoveFromSuperview();
            vc.RemoveFromParentViewController();
        }

        private void ShowError(string message)
        {
            InvokeOnMainThread(() =>
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure("", message,
                    (errorSender, ea) => errorView.RemoveFromSuperview());
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            });
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

        private void ShowSuccessTextField(ref UITextField textField)
        {
            textField.Layer.BorderColor = ConstantColor.UiValidSuccess.CGColor;
            textField.Layer.BackgroundColor = ConstantColor.UiValidSuccess.ColorWithAlpha(0.5f).CGColor;
            textField.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            textField.Layer.BorderWidth = ConstantStyle.BorderWidthTextField;
        }

        private UITextField ShowSuccessTextField(UITextField textField)
        {
            textField.Layer.BorderColor = ConstantColor.UiValidSuccess.CGColor;
            textField.Layer.BackgroundColor = ConstantColor.UiValidSuccess.ColorWithAlpha(0.5f).CGColor;
            textField.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            textField.Layer.BorderWidth = ConstantStyle.BorderWidthTextField;
            return textField;
        }

        private UIPickerView ShowErrorPickerView(UIPickerView pickerView)
        {
            pickerView.Layer.BackgroundColor = ConstantColor.UiValidError.ColorWithAlpha(0.5f).CGColor;
            pickerView.Layer.BorderColor = ConstantColor.UiValidError.CGColor;
            pickerView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            return pickerView;
        }

        private UIPickerView ShowSuccessPickerView(UIPickerView pickerView)
        {
            pickerView.Layer.BackgroundColor = ConstantColor.UiValidSuccess.ColorWithAlpha(0.5f).CGColor;
            pickerView.Layer.BorderColor = ConstantColor.UiValidSuccess.CGColor;
            pickerView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            return pickerView;
        }

        private void ModifyFieldsStyle()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                nameTextField = ShowErrorTextField(nameTextField);
            }
            else
            {
                nameTextField = ShowSuccessTextField(nameTextField);
            }
            if (string.IsNullOrEmpty(User.LastName))
            {
                lastNameTextField = ShowErrorTextField(lastNameTextField);
            }
            else
            {
                lastNameTextField = ShowSuccessTextField(lastNameTextField);

            }
            if (string.IsNullOrEmpty(User.DocumentNumber))
            {
                documentNumberTextField = ShowErrorTextField(documentNumberTextField);
            }
            else
            {
                documentNumberTextField = ShowSuccessTextField(documentNumberTextField);

            }
            if (string.IsNullOrEmpty(User.CellPhone))
            {
                telephoneCelTextField = ShowErrorTextField(telephoneCelTextField);
            }
            else
            {
                telephoneCelTextField = ShowSuccessTextField(telephoneCelTextField);
            }
            if (string.IsNullOrEmpty(User.Email))
            {
                emailTextField = ShowErrorTextField(emailTextField);
            }
            else
            {
                emailTextField = ShowSuccessTextField(emailTextField);
            }
            if (string.IsNullOrEmpty(User.Password))
            {
                passwordTextField = ShowErrorTextField(passwordTextField);
            }
            else
            {
                passwordTextField = ShowSuccessTextField(passwordTextField);
            }
            if (string.IsNullOrEmpty(User.ConfirmPassword))
            {
                passwordConfirmTextField = ShowErrorTextField(passwordConfirmTextField);
            }
            else
            {
                passwordConfirmTextField = ShowSuccessTextField(passwordConfirmTextField);
            }
            if (string.IsNullOrEmpty(User.DocumentType.ToString()))
            {
                documentTypeTextField = ShowErrorTextField(documentTypeTextField);
            }
            else
            {
                documentTypeTextField = ShowSuccessTextField(documentTypeTextField);
            }
            if (string.IsNullOrEmpty(User.DateOfBirth))
            {
                dateTextField = ShowErrorTextField(dateTextField);
            }
            else
            {
                dateTextField = ShowSuccessTextField(dateTextField);
            }
        }

        private bool ValidateTextFields()
        {
            if (string.IsNullOrEmpty(nameTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(lastNameTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(documentNumberTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(telephoneCelTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(emailTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(passwordTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(passwordConfirmTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(documentTypeTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(dateTextField.Text.Trim()))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Events
        private void RegisterUserButtonUpInside(object sender, EventArgs e)
        {
            try
            {
                if (documentTypeTextField.Text != "")
                {
                    base.StartActivityIndicatorCustom();
                    RegisterUser();
                }
                else
                {
                    var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.ValidateDocumentType, UIAlertControllerStyle.Alert);
                    alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Cancel, null));
                    ShowErrorTextField(documentTypeTextField);
                    validateTypeDocumentLabel.Hidden = false;
                    PresentViewController(alertController, true, null);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.RegistryUserViewController, ConstantEventName.RegisterUserUpInside);
                ShowMessageException(exception);
            }
        }

        void HabeasDataButton_TouchUpInside(object sender, EventArgs e)
        {
            var url = new NSUrl(AppServiceConfiguration.PersonalDataManagementUrl);
            var req = new NSUrlRequest(url);
            UIViewController habeasDataViewController = new UIViewController();
            WKWebView webView = new WKWebView(View.Bounds, new WKWebViewConfiguration());
            habeasDataViewController.View.AddSubview(webView);
            var request = new NSUrlRequest(url);
            webView.LoadRequest(request);
            NavigationController.PushViewController(habeasDataViewController, true);
        }


        private void DocumentTypeUpInside(object sender, EventArgs e)
        {
            nameTextField.BecomeFirstResponder();
            nameTextField.ResignFirstResponder();
            genericPickerDateView.Hidden = true;
            genericPickerView.Hidden = false;
            PickerViewDelegate pickerViewDelegate = new PickerViewDelegate(documentsType, documentTypeTextField)
            {
                Action = TypeDocumentSelectedDelegateAction
            };
            genericPickerView.Delegate = pickerViewDelegate;
            genericPickerView.DataSource = new PickerViewSource(documentsType);
            genericPickerView.ReloadAllComponents();
            pickerViewDelegate.Selected(genericPickerView, 0, 0);
            containerGenericPickerView.Hidden = false;
            arrowImageView.Hidden = true;
        }

        private void TypeDocumentSelectedDelegateAction(object sender, EventArgs e)
        {
            PosDocument = (int)genericPickerView.SelectedRowInComponent(0);
            genericPickerDateView.Hidden = true;
            if (documentsType[PosDocument].Name.Equals("PA"))
            {
                documentNumberTextField.TextContentType = UITextContentType.EmailAddress;
                documentNumberTextField.KeyboardType = UIKeyboardType.EmailAddress;
            }
            else
            {
                documentNumberTextField.TextContentType = UITextContentType.TelephoneNumber;
                documentNumberTextField.KeyboardType = UIKeyboardType.NumberPad;
            }
            documentNumberTextField.ReloadInputViews();
        }

        private void DayBirthDateUpInside(object sender, EventArgs e)
        {
            genericPickerView.Hidden = true;
            genericPickerDateView.Hidden = false;
            nameTextField.BecomeFirstResponder();
            nameTextField.ResignFirstResponder();
            genericPickerDateView.EditingChanged += GenericPickerDateViewValueChanged;
            genericPickerDateView.ValueChanged += GenericPickerDateViewValueChanged;
            containerGenericPickerView.Hidden = false;
        }

        private void GenericPickerDateViewValueChanged(object sender, EventArgs e)
        {
            NSDateFormatter dateFormatter = new NSDateFormatter
            {
                DateFormat = AppConfigurations.DateFormat
            };

            dateTextField.Text = dateFormatter.ToString(genericPickerDateView.Date);
        }

        private void CloseGenericPickerUpInside(object sender, EventArgs e)
        {
            if (!genericPickerDateView.Hidden)
            {
                NSDateFormatter dateFormatter = new NSDateFormatter
                {
                    DateFormat = AppConfigurations.DateFormat
                };
                dateTextField.Text = dateFormatter.ToString(genericPickerDateView.Date);

            }
            else
            {
                if (documentTypeTextField.Text != "")
                {
                    validateTypeDocumentLabel.Hidden = true;
                    ShowSuccessTextField(documentTypeTextField);
                }
                else
                {
                    validateTypeDocumentLabel.Hidden = false;
                    ShowErrorTextField(documentTypeTextField);
                }
            }
            containerGenericPickerView.Hidden = true;
        }

        private void ShowHiddenPasswordButtonTouchUpInside(object sender, EventArgs events)
        {
            passwordTextField.SecureTextEntry = !passwordTextField.SecureTextEntry;
        }

        private void ShowHiddenConfirmPasswordButtonTouchUpInside(object sender, EventArgs events)
        {
            passwordConfirmTextField.SecureTextEntry = !passwordConfirmTextField.SecureTextEntry;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
            nameTextField.BecomeFirstResponder();
            nameTextField.ResignFirstResponder();
        }
        #endregion
    }
}

