using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class EditProfileViewController : UIViewControllerBase
    {
        #region Attributes
        private User user;
        private MyAccountModel myAccountModel;
        private UserModel userModel;
        private ResponseBase responseBase;
        private DocumentTypesModel _documentTypesModel;
        private IList<DocumentType> documentsType;
        private RegistrationValidationViewController validationViewController;
        #endregion


        public EditProfileViewController(IntPtr handle) : base(handle)
        {
            myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
            userModel = new UserModel(new UserService(DeviceManager.Instance));
            _documentTypesModel = new DocumentTypesModel(new DocumentTypesService(DeviceManager.Instance));
            documentsType = new List<DocumentType>();
        }

        #region lyfe cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            validationViewController = new RegistrationValidationViewController();
            this.LoadExternalViews();
            this.ReadOnly();
            this.LoadCorners();
            InvokeOnMainThread(async () =>
            {
                await this.LoadDocumentsType();
                await this.GetUser(); 
                if (ParametersManager.UserContext.CellPhone == null)
                {
                    ParametersManager.UserContext.CellPhone = "0000000000";
                }
            });
            this.LoadHandlers();
            this.validateTexfieldLength();
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
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            if (validationViewController.IsValidationModified)
            {
                VerifyUserParameters parameters = new VerifyUserParameters()
                {
                    CellPhone = ParametersManager.UserContext.CellPhone,
                    DocumentNumber = ParametersManager.UserContext.DocumentNumber,
                    SiteId = AppServiceConfiguration.SiteId,
                    IsValidated = ParametersManager.UserContext.UserActivate
                };

                ReverseCellphoneChange(parameters);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods
        private async Task GetUser()
        {
            try
            {
                StartActivityIndicatorCustom();
                var response = await myAccountModel.GetUser();
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    string message = MessagesHelper.GetMessage(response.Result);
                    StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                }
                else
                {
                    SetUser(response.User);
                    LoadParameters();
                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.UserProfileViewController, ConstantMethodName.GetMyAccount);
                StopActivityIndicatorCustom();
            }
        }

        private async Task EditUser()
        {
            try
            {
                user = this.SetUser();
                string message = myAccountModel.ValidateFields(user);
                StopActivityIndicatorCustom();
                if (string.IsNullOrEmpty(message))
                {
                    if (!ParametersManager.UserContext.CellPhone.Equals(numberTextField.Text))
                    {
                        validationViewController.OperationDoneAction += (result) =>
                        {
                            RemoveChild(validationViewController);
                            this.NavigationController.NavigationBarHidden = false;

                            if (result)
                            {
                                base.StartActivityIndicatorCustom();
                                UpdateUser();
                            }
                            else
                            {
                                ShowError(AppMessages.VerifyUserError);

                                //VerifyUserParameters parameters = new VerifyUserParameters()
                                //{
                                //    CellPhone = ParametersManager.UserContext.CellPhone,
                                //    DocumentNumber = ParametersManager.UserContext.DocumentNumber,
                                //    SiteId = AppServiceConfiguration.SiteId,
                                //    IsValidated = ParametersManager.UserContext.UserActivate
                                //};

                                //ReverseCellphoneChange(parameters);
                            }
                        };

                        validationViewController.OperationCanceledAction += () =>
                        {
                            RemoveChild(validationViewController);
                            this.NavigationController.NavigationBarHidden = false;

                            //VerifyUserParameters parameters = new VerifyUserParameters()
                            //{
                            //    CellPhone = ParametersManager.UserContext.CellPhone,
                            //    DocumentNumber = ParametersManager.UserContext.DocumentNumber,
                            //    SiteId = AppServiceConfiguration.SiteId,
                            //    IsValidated = ParametersManager.UserContext.UserActivate
                            //};

                            //ReverseCellphoneChange(parameters);
                        };

                        AddChildViewController(validationViewController);

                        this.NavigationController.NavigationBarHidden = true;
                        validationViewController.View.Frame = View.Bounds;
                        validationViewController.View.LayoutIfNeeded();

                        validationViewController.Cellphone = numberTextField.Text;
                        View.AddSubview(validationViewController.View);
                        validationViewController.DidMoveToParentViewController(this);
                    }
                    else
                    {
                        await UpdateUser();
                    }
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
                    StopActivityIndicatorCustom();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantMethodName.RegisterUser);
                StopActivityIndicatorCustom();
            }
        }

        private async Task ReverseCellphoneChange(VerifyUserParameters parameters)
        {
            responseBase = await userModel.UpdateVerifyUser(parameters);
            if (responseBase.Result != null && responseBase.Result.HasErrors && responseBase.Result.Messages != null)
            {
                StartActivityErrorMessage(responseBase.Result.Messages[0].Code, responseBase.Result.Messages[0].Description);
            }
        }

        private async Task UpdateUser()
        {
            responseBase = await myAccountModel.UpdateUser(user);
            if (responseBase.Result != null && responseBase.Result.HasErrors && responseBase.Result.Messages != null)
            {
                StartActivityErrorMessage(responseBase.Result.Messages[0].Code, responseBase.Result.Messages[0].Description);
            }
            else
            {
                UserContext userContext = ParametersManager.UserContext;
                userContext.FirstName = nameTextField.Text;
                userContext.LastName = surnameTextField.Text;
                userContext.DateOfBirth = dateTextField.Text;
                userContext.CellPhone = numberTextField.Text;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));

                var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.EditInformation, UIAlertControllerStyle.Alert);
                alertController.AddAction(UIAlertAction.Create(AppMessages.OK, UIAlertActionStyle.Default, action =>
                {
                    this.NavigationController.PopViewController(true);
                }));

                this.PresentViewController(alertController, true, null);
            }
            base.StopActivityIndicatorCustom();
        }

        private void RemoveChild(UIViewController vc)
        {
            vc.WillMoveToParentViewController(null);
            vc.View.RemoveFromSuperview();
            vc.RemoveFromParentViewController();
        }

        public async Task LoadDocumentsType()
        {
            try
            {
                base.StartActivityIndicatorCustom();
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
                }
                base.StopActivityIndicatorCustom();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantMethodName.GetDocumentTypes);
                ShowMessageException(exception);
            }
        }

        private string GetDocument(int type)
        {
            foreach (DocumentType item in documentsType)
            {
                if (item.Id.Equals(type.ToString()))
                {
                    return item.Name;
                }
            }
            return "";
        }


        private void LoadParameters()
        {
            UserContext userContext = ParametersManager.UserContext;
            try
            {
                if (!string.IsNullOrEmpty(userContext.DateOfBirth))
                {
                    DateTime dateTime = new DateTime();
                    {
                        dateTime = DateTime.ParseExact(userContext.DateOfBirth, "MM/dd/yyyy", null);
                    }
                    if (dateTime != null)
                    {
                        NSDate date = ConvertDateTimeToNSDate(dateTime);
                        genericPickerView.SetDate(date, true);
                    }
                }
            }catch(Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantMethodName.GetDocumentTypes);
            }
            nameTextField.Text = userContext.FirstName;
            surnameTextField.Text = userContext.LastName;
            dateTextField.Text = userContext.DateOfBirth;
            docummentTextField.Text = userContext.DocumentNumber;
            emailTextField.Text = userContext.Email;
            numberTextField.Text = userContext.CellPhone;
            typeIdTextField.Text = GetDocument(userContext.DocumentType);
        }

        private NSDate ConvertDateTimeToNSDate(DateTime date)
        {
            DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - newDate).TotalSeconds);
        }

        private void ReadOnly()
        {
            typeIdTextField.Enabled = false;
            docummentTextField.Enabled = false;
            emailTextField.Enabled = false;
        }

        private void LoadCorners()
        {

            saveButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadHandlers()
        {
            dateButton.TouchUpInside += DateButtonTouchUpInside;
            CloseButton.TouchUpInside += CloseButtonTouchUpInside;
            saveButton.TouchUpInside += SaveButtonTouchUpInsideAsync;
        }

        private void LoadExternalViews()
        {
            try
            {
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
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
                string dateOfBirth = dateFormatter.ToString(genericPickerView.Date);
                return dateOfBirth;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void validateTexfieldLength()
        {

            nameTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 30;
            };

            surnameTextField.ShouldChangeCharacters = (textField, range, replacementString) =>
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= 30;
            };
        }

        private UITextField ShowErrorTextField(UITextField textField)
        {
            textField.Layer.BorderColor = ConstantColor.UiValidError.CGColor;
            textField.Layer.BorderWidth = ConstantStyle.BorderWidthTextField;
            textField.Layer.CornerRadius = ConstantStyle.CornerRadiusTextField;
            textField.Layer.BackgroundColor = ConstantColor.UiValidError.ColorWithAlpha(0.5f).CGColor;
            return textField;
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

        private bool ValidateTextFields()
        {
            if (string.IsNullOrEmpty(nameTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(surnameTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(dateTextField.Text.Trim()))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(numberTextField.Text.Trim()))
            {
                return false;
            }
            return true;
        }

        private void ModifyFieldsStyle()
        {
            if (string.IsNullOrEmpty(user.FirstName))
            {
                nameTextField = ShowErrorTextField(nameTextField);
            }
            else
            {
                nameTextField = ShowSuccessTextField(nameTextField);
            }
            if (string.IsNullOrEmpty(user.LastName))
            {
                surnameTextField = ShowErrorTextField(surnameTextField);
            }
            else
            {
                surnameTextField = ShowSuccessTextField(surnameTextField);

            }
            if (string.IsNullOrEmpty(user.DateOfBirth))
            {
                dateTextField = ShowErrorTextField(dateTextField);
            }
            else
            {
                dateTextField = ShowSuccessTextField(dateTextField);

            }
            if (string.IsNullOrEmpty(user.CellPhone))
            {
                numberTextField = ShowErrorTextField(numberTextField);
            }
            else
            {
                numberTextField = ShowSuccessTextField(numberTextField);
            }
        }

        private User SetUser()
        {
            try
            {
                return new User()
                {
                    FirstName = nameTextField.Text,
                    LastName = surnameTextField.Text,
                    DateOfBirth = this.GetDateOfBirth(),
                    CellPhone = numberTextField.Text,
                    Email = emailTextField.Text,
                    AcceptTerms = agreeSwitch.On
                };
            }
            catch (Exception exception)
            {
                StopActivityIndicatorCustom();
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantMethodName.SetUser);
                ShowMessageException(exception);
                return null;
            }
        }
        #endregion

        #region Events
        private void DateButtonTouchUpInside(object sender, EventArgs e)
        {
            dateTextField.BecomeFirstResponder();
            dateTextField.ResignFirstResponder();
            genericPickerView.EditingChanged += GenericPickerDateViewValueChanged;
            genericPickerView.ValueChanged += GenericPickerDateViewValueChanged;
            containerGenericPickerView.Hidden = false;
        }

        private void GenericPickerDateViewValueChanged(object sender, EventArgs e)
        {
            NSDateFormatter dateFormatter = new NSDateFormatter
            {
                DateFormat = AppConfigurations.DateFormat
            };

            dateTextField.Text = dateFormatter.ToString(genericPickerView.Date);
        }

        private void CloseButtonTouchUpInside(object sender, EventArgs e)
        {
            if (!genericPickerView.Hidden)
            {
                NSDateFormatter dateFormatter = new NSDateFormatter
                {
                    DateFormat = AppConfigurations.DateFormat
                };
                dateTextField.Text = dateFormatter.ToString(genericPickerView.Date);

            }

            containerGenericPickerView.Hidden = true;
        }

        private async void SaveButtonTouchUpInsideAsync(object sender, EventArgs e)
        {
            try
            {
                base.StartActivityIndicatorCustom();
                await EditUser();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.EditProfileViewController, ConstantEventName.RegisterUserUpInside);
                ShowMessageException(exception);


            }
            #endregion

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
    }
}

