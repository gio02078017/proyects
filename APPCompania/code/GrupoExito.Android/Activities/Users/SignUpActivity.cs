using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Android.Utilities.Listeners;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Droid = Android;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Registro", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class SignUpActivity : BaseVerifyUser, ISmsListener
    {
        #region Controls

        private Spinner SpDocumentType, SpDay, SpMonth, SpYear;
        private TextView TvRecord;
        private TextView TvSaveYourData;
        private TextView TvFieldsRequired;
        private TextView TvBasicData;
        private TextView TvNames;
        private TextView TvLastNames;
        private TextView TvDocumentType;
        private TextView TvCellPhone;
        private TextView TvAccessInformation;
        private TextView TvEmail;
        private TextView TvPassword;
        private TextView TvPasswordRule;
        private TextView TvConfirmPassword;
        private TextView TvTerms;
        private TextView TvPersonalData;
        private TextInputLayout TviNames;
        private TextInputLayout TviLastNames;
        private TextInputLayout TviDocumentNumber;
        private TextInputLayout TviCellphone;
        private TextInputLayout TviEmail;
        private TextInputLayout TviPassword;
        private TextInputLayout TviConfirmPassword;
        private TextInputEditText EtNames;
        private EditText EtLastNames;
        private EditText EtDocumentNumber;
        private EditText EtCellphone;
        private EditText EtEmail;
        private EditText EtPassword;
        private EditText EtConfirmPassword;
        private EditText EtDocumentType;
        private CheckBox ChkTerms;
        private Button BtnSignUp;

        #endregion

        #region Properties

        private SignUpModel _signUpModel;
        private string Gender { get; set; }
        private IList<DocumentType> documentsType;
        private ClientResponse SignUpResponse { get; set; }
        private User User { get; set; }

        #endregion

        #region Publics 

        public void MessageReceived(string message)
        {
            if (EtVerifyCode != null)
            {
                EtVerifyCode.Text = message;
            }
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            var loginView = new Intent(this, typeof(LoginActivity));
            HideProgressDialog();
            StartActivity(loginView);
            Finish();
        }

        public override void VerifyCodeAsync()
        {
            string code = EtVerifyCode.Text;

            RunOnUiThread(async () =>
            {
                CloseModal();
                await VerifyUser(code);
            });
        }

        public override void ResendAsync()
        {
            CloseModal();
            RunOnUiThread(async () =>
            {
                await SendSMSVerifyCode();
            });
        }

        #endregion

        #region Protected

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (BroadcastService == null)
            {
                BroadcastService = new SmsMessageService();
            }

            BroadcastService.BindListener(this);
            LocalBroadcastManager.GetInstance(this).RegisterReceiver(BroadcastService, new IntentFilter(Telephony.Sms.Intents.SmsReceivedAction));           
        }

        protected override void OnPause()
        {
            if (BroadcastService != null)
            {
                LocalBroadcastManager.GetInstance(this).UnregisterReceiver(BroadcastService);
                BroadcastService = null;
            }

            base.OnPause();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.SignUp, typeof(SignUpActivity).Name);
        }

        protected override void OnDestroy()
        {
            if (BroadcastService != null)
            {
                LocalBroadcastManager.GetInstance(this).UnregisterReceiver(BroadcastService);
                BroadcastService = null;
            }

            base.OnDestroy();
        }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _signUpModel = new SignUpModel(new SignUpService(DeviceManager.Instance));           
            documentsType = new List<DocumentType>();
            SetContentView(Resource.Layout.ActivitySignUp);
            HideItemsToolbar(this);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            await this.GetDocumentsType();
            ValidateReadSmsAvailability();            
        }

        #endregion

        #region Privates       

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            SpDocumentType = FindViewById<Spinner>(Resource.Id.spDocumentType);
            SpDay = FindViewById<Spinner>(Resource.Id.spDay);
            SpMonth = FindViewById<Spinner>(Resource.Id.spMonth);
            SpYear = FindViewById<Spinner>(Resource.Id.spYear);
            TvRecord = FindViewById<TextView>(Resource.Id.tvRecord);
            TvSaveYourData = FindViewById<TextView>(Resource.Id.tvSaveYourData);
            TvFieldsRequired = FindViewById<TextView>(Resource.Id.tvDataRequired);
            TvBasicData = FindViewById<TextView>(Resource.Id.tvBasicData);
            TvNames = FindViewById<TextView>(Resource.Id.tvNames);
            TvLastNames = FindViewById<TextView>(Resource.Id.tvLastNames);
            TvDocumentType = FindViewById<TextView>(Resource.Id.tvDocumentType);
            TvCellPhone = FindViewById<TextView>(Resource.Id.tvPhoneNumber);
            TvAccessInformation = FindViewById<TextView>(Resource.Id.tvAccessInformation);
            TvEmail = FindViewById<TextView>(Resource.Id.tvEmail);
            TvPassword = FindViewById<TextView>(Resource.Id.tvPassword);
            TvPasswordRule = FindViewById<TextView>(Resource.Id.tvPasswordRule);
            TvConfirmPassword = FindViewById<TextView>(Resource.Id.tvConfirmPassword);
            TvTerms = FindViewById<TextView>(Resource.Id.tvTerms);
            TvPersonalData = FindViewById<TextView>(Resource.Id.tvPersonalData);
            TviNames = FindViewById<TextInputLayout>(Resource.Id.tviName);
            TviLastNames = FindViewById<TextInputLayout>(Resource.Id.tviLastName);
            TviDocumentNumber = FindViewById<TextInputLayout>(Resource.Id.tviDocumentNumber);
            TviCellphone = FindViewById<TextInputLayout>(Resource.Id.tviPhoneNumber);
            TviEmail = FindViewById<TextInputLayout>(Resource.Id.tviEmail);
            TviPassword = FindViewById<TextInputLayout>(Resource.Id.tviPassword);
            TviConfirmPassword = FindViewById<TextInputLayout>(Resource.Id.tviRepeatPassword);
            EtNames = FindViewById<TextInputEditText>(Resource.Id.etName);
            EtLastNames = FindViewById<EditText>(Resource.Id.etLastName);
            EtDocumentNumber = FindViewById<EditText>(Resource.Id.etDocumentNumber);
            EtCellphone = FindViewById<EditText>(Resource.Id.etPhoneNumber);
            EtEmail = FindViewById<EditText>(Resource.Id.etEmail);
            EtPassword = FindViewById<EditText>(Resource.Id.etPassword);
            EtConfirmPassword = FindViewById<EditText>(Resource.Id.etRepeatPassword);
            EtDocumentType = FindViewById<EditText>(Resource.Id.etDocumentType);
            ChkTerms = FindViewById<CheckBox>(Resource.Id.chkTerms);
            BtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            TvTerms.Click += delegate { OpenTerms(); };
            TvPersonalData.Click += delegate { OpenPersonalDataManagement(); };
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            BtnSignUp.Click += async delegate { await SignUp(); };
            this.EditFonts();
            var chooseList = new List<DocumentType>();
            Typeface typeface = FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium);
            CustomSpinnerAdapter documentTypeAdapter = new CustomSpinnerAdapter(this, Droid.Resource.Layout.SelectDialogItem, chooseList.ToList().Select(x => x.Name).ToArray(), typeface);
            SpDocumentType.Adapter = documentTypeAdapter;

            EtDocumentType.Click += delegate { SpDocumentType.PerformClick(); };
            EtNames.AddTextChangedListener(new MyTextWatcher(this, EtNames));
            EtLastNames.AddTextChangedListener(new MyTextWatcher(this, EtLastNames));
            EtDocumentNumber.AddTextChangedListener(new MyTextWatcher(this, EtDocumentNumber));
            EtCellphone.AddTextChangedListener(new MyTextWatcher(this, EtCellphone));
            EtEmail.AddTextChangedListener(new MyTextWatcher(this, EtEmail));
            EtPassword.AddTextChangedListener(new MyTextWatcher(this, EtPassword));
            EtConfirmPassword.AddTextChangedListener(new MyTextWatcher(this, EtConfirmPassword));
            EtPassword.SetFilters(new IInputFilter[] { new CustomInputFilter(ConstantEventName.SpaceFilter, "", -1) });
            EtConfirmPassword.SetFilters(new IInputFilter[] { new CustomInputFilter(ConstantEventName.SpaceFilter, "", -1) });
            SpDocumentType.ItemSelected += SpDocumentType_ItemSelected;
            SpDay.ItemSelected += SpinnerItemSelected;
            SpMonth.ItemSelected += SpinnerItemSelected;
            SpYear.ItemSelected += SpinnerItemSelected;

        }

        private void SpDocumentType_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            EtDocumentType.Text = documentsType[e.Position].Name;

            if (EtDocumentType.Text.Equals("PA"))
            {
                EtDocumentNumber.Text = null;
                EtDocumentNumber.InputType = InputTypes.ClassText;
            }
            else
            {
                EtDocumentNumber.Text = null;
                EtDocumentNumber.InputType = InputTypes.ClassNumber;
            }
        }

        private void OpenTerms()
        {
            Intent intent = new Intent(this, typeof(TermsActivity));
            intent.PutExtra(ConstantPreference.Terms, AppServiceConfiguration.TermsAndConditionApp);
            StartActivity(intent);
        }

        private void OpenPersonalDataManagement()
        {
            Intent intent = new Intent(this, typeof(PersonalDataManagementActivity));
            StartActivity(intent);
        }

        private void EditFonts()
        {
            TvRecord.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvSaveYourData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvFieldsRequired.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvBasicData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvLastNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDocumentType.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvCellPhone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvAccessInformation.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvPasswordRule.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            FindViewById<TextView>(Resource.Id.tvAnd).SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvConfirmPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvTerms.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvPersonalData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            EtNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtLastNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtDocumentNumber.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtCellphone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtConfirmPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnSignUp.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            ChkTerms.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void PullSpinners()
        {
            RunOnUiThread(() =>
            {
                Typeface typeface = FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium);

                CustomSpinnerAdapter documentTypeAdapter = new CustomSpinnerAdapter(this, Resource.Layout.spinner_layout, documentsType.ToList().Select(x => x.Name).ToArray(), typeface);
                SpDocumentType.Adapter = documentTypeAdapter;

                List<Item> itemsDays = JsonService.Deserialize<List<Item>>(AppConfigurations.DaysSource);
                List<Item> itemsMonths = JsonService.Deserialize<List<Item>>(AppConfigurations.MonthsSource);
                List<Item> itemsYears = ModelHelper.GetYears();

                ArrayAdapter<string> dayAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, itemsDays.Select(x => x.Text).ToArray());
                SpDay.Adapter = dayAdapter;

                MySpinnerAdapter monthAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, itemsMonths.Select(x => x.Text).ToArray());
                SpMonth.Adapter = monthAdapter;

                MySpinnerAdapter yearAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, itemsYears.Select(x => x.Text).ToArray());
                SpYear.Adapter = yearAdapter;
            });
        }

        private async Task GetDocumentsType()
        {
            try
            {
                var response = await LoadDocumentsType();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    RunOnUiThread(() =>
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    documentsType = response.DocumentTypes;
                    PullSpinners();
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SignUpActivity, ConstantMethodName.GetDocumentTypes } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private User SetUser()
        {
            return new User
            {
                FirstName = EtNames.Text,
                LastName = EtLastNames.Text,
                DocumentType = int.Parse(documentsType[SpDocumentType.SelectedItemPosition].Id),
                DocumentNumber = EtDocumentNumber.Text,
                DateOfBirth = this.GetDateOfBirth(),
                Gender = this.Gender,
                CellPhone = EtCellphone.Text,
                Email = EtEmail.Text,
                Password = EtPassword.Text,
                ConfirmPassword = EtConfirmPassword.Text,
                AcceptTerms = ChkTerms.Checked
            };
        }

        private async Task SignUp()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                User = this.SetUser();

                if (ValidatedBeforeFields())
                {
                    string message = _signUpModel.ValidateFields(User);

                    if (string.IsNullOrEmpty(message))
                    {
                        await SendSMSVerifyCode();
                    }
                    else
                    {
                        if (message.Equals(AppMessages.RequiredFieldsText))
                        {
                            this.ModifyFieldsStyle();
                        }

                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText);
                    }
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SignUpActivity, ConstantMethodName.SignUp } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private string GetDateOfBirth()
        {
            string selectecDay = !SpDay.SelectedItem.ToString().Equals("Día") &&
                int.Parse(SpDay.SelectedItem.ToString()) > 0
                && int.Parse(SpDay.SelectedItem.ToString()) <= 9 ?
                "0" + SpDay.SelectedItem.ToString() : SpDay.SelectedItem.ToString();

            string selectecMonth = SpMonth.SelectedItemPosition != 0 &&
               SpMonth.SelectedItemPosition > 0
               && SpMonth.SelectedItemPosition <= 9 ?
               "0" + SpMonth.SelectedItemPosition.ToString() : SpMonth.SelectedItem.ToString();

            string dateOfBirth = SpMonth.SelectedItemPosition == 0
                || SpDay.SelectedItemPosition == 0
                || SpYear.SelectedItemPosition == 0 ?
                string.Empty :
                 selectecMonth + "/" + selectecDay + "/" + SpYear.SelectedItem.ToString();

            return dateOfBirth;
        }

        private void ModifyFieldsStyle()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                EtNames.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtNames.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                EtLastNames.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtLastNames.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(User.DocumentNumber))
            {
                EtDocumentNumber.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtDocumentNumber.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(User.CellPhone))
            {
                EtCellphone.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtCellphone.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                EtEmail.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtEmail.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(User.Password))
            {
                EtPassword.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtPassword.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(User.ConfirmPassword))
            {
                EtConfirmPassword.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtConfirmPassword.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (SpMonth.SelectedItemPosition == 0)
            {
                SpMonth.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpMonth.SetBackgroundResource(Resource.Drawable.spinner);
            }

            if (SpDay.SelectedItemPosition == 0)
            {
                SpDay.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpDay.SetBackgroundResource(Resource.Drawable.spinner);
            }

            if (SpYear.SelectedItemPosition == 0)
            {
                SpYear.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpYear.SetBackgroundResource(Resource.Drawable.spinner);
            }

            if (SpDocumentType.SelectedItemPosition == 0)
            {
                SpDocumentType.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpDocumentType.SetBackgroundResource(Resource.Drawable.spinner);
            }
        }

        private void ValidateResponseSignUp()
        {
            if (SignUpResponse.Result != null && SignUpResponse.Result.HasErrors && SignUpResponse.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();

                    var errorClientExists = SignUpResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ClientExists)).Any();

                    if (errorClientExists)
                    {
                        var errorCode = SignUpResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ClientExists)).First();
                        DefineShowErrorWay(AppMessages.ApplicationName, errorCode.Description, AppMessages.AcceptButtonText);
                    }
                    else
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
                    }
                });
            }
            else
            {
                HideProgressDialog();
                User.Id = SignUpResponse.Id;
                UserContext userContext = ModelHelper.SetUserContext(User);
                //userContext.ValidatedUser = true;
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                this.SuccessSignUp();
            }
        }

        private void SuccessSignUp()
        {
            AnalyticSignUpEvent();
            HideProgressDialog();
            Intent intent = new Intent(this, typeof(SuccessSignUpActivity));
            StartActivity(intent);
            Finish();
        }

        private async Task SendSMSVerifyCode()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                string messageValidation = userModel.ValidateCellPhoneField(User.CellPhone);

                if (string.IsNullOrEmpty(messageValidation))
                {
                    VerifyUserParameters parameters = SetParametersSendSMS();
                    SendMessageVerifyUserResponse response = await SendSMSVerifyCode(parameters);
                    ValidateResponseSendSms(response);
                }
                else
                {
                    FirstTimeSendCode = true;
                    HideProgressDialog();
                    MessageVerifyCode = messageValidation;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SignUpActivity, ConstantMethodName.SendSMSVerifyCode } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ValidateResponseSendSms(SendMessageVerifyUserResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    FirstTimeSendCode = true;
                    HideProgressDialog();
                    MessageVerifyCode = MessagesHelper.GetMessage(response.Result);
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                });
            }
            else if (response.MessageSent)
            {
                HideProgressDialog();
                ShowVerifyCodeDialog(EnumTypesVerifyCode.VerifyCode);
                SetCellPhone(EtCellphone.Text);
            }
            else
            {
                RunOnUiThread(() =>
                {
                    FirstTimeSendCode = true;
                    HideProgressDialog();
                    MessageVerifyCode = AppMessages.SedSMSError;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                });
            }
        }

        public VerifyUserParameters SetParametersSendSMS()
        {
            VerifyUserParameters parameters = new VerifyUserParameters
            {
                CellPhone = EtCellphone.Text,
                DocumentNumber = EtDocumentNumber.Text,
                SiteId = AppServiceConfiguration.SiteId,
            };

            return parameters;
        }

        private async Task VerifyUser(string code)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
                string messageValidation = userModel.ValidateCodeField(code.TrimStart().TrimEnd());

                if (string.IsNullOrEmpty(messageValidation))
                {
                    VerifyUserParameters parameters = SetParametersVerifyUser(code);
                    VerifyUserResponse response = await VerifyUser(parameters);
                    await this.ValidateResponseVerifyUser(response);
                }
                else
                {
                    HideProgressDialog();
                    MessageVerifyCode = messageValidation;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.SignUpActivity, ConstantMethodName.VerifyUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private VerifyUserParameters SetParametersVerifyUser(string code)
        {
            VerifyUserParameters parameters = new VerifyUserParameters
            {
                CellPhone = EtCellphone.Text,
                DocumentNumber = EtDocumentNumber.Text,
                SiteId = AppServiceConfiguration.SiteId,
                Code = code.TrimStart().TrimEnd()
            };

            return parameters;
        }

        private async Task ValidateResponseVerifyUser(VerifyUserResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();

                    var errorCellPhoneRegistered = response.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ErrorCellPhoneRegistered)).Any();

                    if (errorCellPhoneRegistered)
                    {
                        string messageCellPhoneRegistered = response.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).
                        Equals(EnumErrorCode.ErrorCellPhoneRegistered)).First().Description;

                        MessageVerifyCode = messageCellPhoneRegistered;
                        ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                    }
                    else
                    {
                        MessageVerifyCode = MessagesHelper.GetMessage(response.Result);
                        ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                    }
                });
            }
            else if (response.Verified)
            {
                SignUpResponse = await _signUpModel.RegisterUser(User);
                this.ValidateResponseSignUp();
            }
            else
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.VerifyUserError, AppMessages.AcceptButtonText);
                });
            }
        }

        #endregion

        #region Validate Fiels

        public class MyTextWatcher : Java.Lang.Object, ITextWatcher
        {
            SignUpActivity _SignUpActivity;
            EditText _editText { get; set; }

            public MyTextWatcher(SignUpActivity _SignUpActivity, EditText editText)
            {
                this._SignUpActivity = _SignUpActivity;
                this._editText = editText;
            }
            public void AfterTextChanged(IEditable s)
            {
            }

            public void BeforeTextChanged(Java.Lang.ICharSequence arg0, int start, int count, int after)
            {
            }

            public void OnTextChanged(Java.Lang.ICharSequence arg0, int start, int before, int count)
            {
                _SignUpActivity.ValidatedFields(arg0, _editText);
            }
        }

        public void ValidatedFields(Java.Lang.ICharSequence arg0, EditText editText)
        {
            switch (editText.Id)
            {
                case Resource.Id.etName:
                    ValidateNames(TviNames, arg0, true, AppMessages.NameText, AppMessages.ItCanNotNameOnlyLetter);
                    break;
                case Resource.Id.etLastName:
                    ValidateNames(TviLastNames, arg0, true, AppMessages.LastNameText, AppMessages.ItCanNotLastNameOnlyLetter);
                    break;
                case Resource.Id.etDocumentNumber:
                    ValidateDocumentNumber(arg0, true);
                    break;
                case Resource.Id.etPhoneNumber:
                    ValidateCellphone(arg0, true);
                    break;
                case Resource.Id.etEmail:
                    ValidateEmail(arg0, true);
                    break;
                case Resource.Id.etPassword:
                    ValidatePassword(arg0, true);
                    if (EtConfirmPassword.Text.Trim().Length > 0)
                    {
                        ValidateConfirmPassword(GetCharSequence(EtConfirmPassword.Text.Trim()), true);
                    }
                    break;
                case Resource.Id.etRepeatPassword:
                    ValidateConfirmPassword(arg0, true);
                    break;
            }
        }

        public bool ValidateNames(TextInputLayout TypeName, Java.Lang.ICharSequence arg0, bool drawError, String name, string message)
        {
            bool passed;

            string data = arg0.ToString();

            Regex regexString = new Regex(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$");

            if (arg0.Length() > 1)
            {
                if (!regexString.IsMatch(data.ToUpper().TrimStart().TrimEnd()))
                {
                    if (drawError)
                    {
                        TypeName.Error = string.Format(AppMessages.SpecialCharactersMessage, name);
                        TypeName.ErrorEnabled = true;
                    }
                    passed = false;
                }
                else
                {
                    if (drawError)
                    {
                        TypeName.ErrorEnabled = false;
                    }
                    passed = true;
                }
            }
            else
            {
                if (drawError)
                {
                    if (arg0.Length() == 0)
                    {
                        TypeName.Error = AppMessages.RequiredFieldText;
                    }
                    else if (arg0.Length() == 1)
                    {
                        TypeName.Error = message;
                    }
                    TypeName.ErrorEnabled = true;
                }
                passed = false;
            }

            return passed;
        }

        public bool ValidateDocumentNumber(Java.Lang.ICharSequence arg0, bool drawError)
        {
            bool passed;

            if (arg0.Length() < 6)
            {
                if (drawError)
                {
                    TviDocumentNumber.Error = AppMessages.DocumentLengthValidationText;
                    TviDocumentNumber.ErrorEnabled = true;
                }
                passed = false;
            }
            else
            {
                if (drawError)
                {
                    TviDocumentNumber.ErrorEnabled = false;
                }
                passed = true;
            }

            return passed;
        }

        public bool ValidateCellphone(Java.Lang.ICharSequence arg0, bool drawError)
        {
            bool passed;

            string data = FromCharSequenceToString(arg0);

            if (data.Length > 0 && data.Length == 10)
            {
                var mobileOperator = data.Substring(0, 3);
                var validMobileOperator = Regex.IsMatch(mobileOperator, AppConfigurations.MobilePhoneFormat);

                if (!validMobileOperator)
                {
                    if (drawError)
                    {
                        TviCellphone.Error = AppMessages.MobileNumberOperatorValidationText;
                        TviCellphone.ErrorEnabled = true;
                    }
                    passed = false;
                }
                else
                {
                    if (drawError)
                    {
                        TviCellphone.ErrorEnabled = false;
                    }
                    passed = true;
                }
            }
            else
            {
                if (drawError)
                {
                    TviCellphone.Error = AppMessages.MobileNumberLenghtValidationText;
                    TviCellphone.ErrorEnabled = true;
                }
                passed = false;
            }
            return passed;
        }

        public bool ValidateEmail(Java.Lang.ICharSequence arg0, bool drawError)
        {
            bool passed;

            if (!Patterns.EmailAddress.Matcher(arg0).Matches())
            {
                if (drawError)
                {
                    TviEmail.Error = AppMessages.EmailFormatErrorTextShort;
                    TviEmail.ErrorEnabled = true;
                }

                passed = false;
            }
            else
            {
                if (drawError)
                {
                    TviEmail.ErrorEnabled = false;
                }
                passed = true;
            }

            return passed;
        }

        public bool ValidatePassword(Java.Lang.ICharSequence arg0, bool drawError)
        {
            bool passed;

            if (arg0.Length() < 8 || arg0.Length() > 25)
            {
                if (drawError)
                {
                    TviPassword.Error = AppMessages.PasswordLengthValidationTextShort;
                    TviPassword.ErrorEnabled = true;
                }
                passed = false;
            }
            else
            {
                if (drawError)
                {
                    TviPassword.ErrorEnabled = false;
                }
                passed = true;
            }

            return passed;
        }

        public bool ValidateConfirmPassword(Java.Lang.ICharSequence arg0, bool drawError)
        {
            bool passed;

            string data = arg0.ToString();

            if (arg0.Length() < 8 || !data.Equals(EtPassword.Text.ToString()))
            {
                if (drawError)
                {
                    TviConfirmPassword.Error = AppMessages.PasswordConfirmationValidationTextShort;
                    TviConfirmPassword.ErrorEnabled = true;
                }
                passed = false;
            }
            else
            {
                if (drawError)
                {
                    TviConfirmPassword.ErrorEnabled = false;
                }
                passed = true;
            }

            return passed;
        }

        public bool ValidatedBeforeFields()
        {
            bool passed = true;

            if (!ValidateNames(TviNames, GetCharSequence(EtNames.Text.ToString()), true, AppMessages.NameText, AppMessages.ItCanNotNameOnlyLetter))
            {
                passed = false;
            }

            if (!ValidateNames(TviLastNames, GetCharSequence(EtLastNames.Text.ToString()), true, AppMessages.LastNameText, AppMessages.ItCanNotLastNameOnlyLetter))
            {
                passed = false;
            }

            if (!ValidateDocumentNumber(GetCharSequence(EtDocumentNumber.Text.ToString()), true))
            {
                passed = false;
            }

            if (!ValidateCellphone(GetCharSequence(EtCellphone.Text.ToString()), true))
            {
                passed = false;
            }

            if (!ValidateEmail(GetCharSequence(EtEmail.Text.ToString()), true))
            {
                passed = false;
            }

            if (!ValidatePassword(GetCharSequence(EtPassword.Text.ToString()), true))
            {
                passed = false;
            }

            if (!ValidateConfirmPassword(GetCharSequence(EtConfirmPassword.Text.ToString()), true))
            {
                passed = false;
            }

            if (SpDay.SelectedItemPosition == 0)
            {
                SpDay.SetBackgroundResource(Resource.Drawable.spinner_error);
                passed = false;
            }

            if (SpMonth.SelectedItemPosition == 0)
            {
                SpMonth.SetBackgroundResource(Resource.Drawable.spinner_error);
                passed = false;
            }

            if (SpYear.SelectedItemPosition == 0)
            {
                SpYear.SetBackgroundResource(Resource.Drawable.spinner_error);
                passed = false;
            }

            return passed;

        }

        public Java.Lang.ICharSequence GetCharSequence(string texto)
        {
            Java.Lang.ICharSequence charSequence = new Java.Lang.StringBuffer(texto);
            return charSequence;
        }

        public string FromCharSequenceToString(Java.Lang.ICharSequence arg0)
        {
            string convert = "";

            if (arg0.Length() > 0)
            {
                convert = arg0.ToString();
            }

            return convert;
        }

        private void SpinnerItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            if (SpDay.SelectedItemPosition == 0)
            {
                SpDay.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpDay.SetBackgroundResource(Resource.Drawable.spinner);
            }

            if (SpMonth.SelectedItemPosition == 0)
            {
                SpMonth.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpMonth.SetBackgroundResource(Resource.Drawable.spinner);
            }

            if (SpYear.SelectedItemPosition == 0)
            {
                SpYear.SetBackgroundResource(Resource.Drawable.spinner_error);
            }
            else
            {
                SpYear.SetBackgroundResource(Resource.Drawable.spinner);
            }
        }

        #endregion

        #region Analytic

        private void AnalyticSignUpEvent()
        {
            FirebaseRegistrationEventsService.Instance.SignUp();
            FacebookRegistrationEventsService.Instance.CompleteRegistration("normal");
        }

        #endregion
    }
}