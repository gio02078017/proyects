using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Interfaces;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using Java.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Editar perfil", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class EditProfileActivity : BaseVerifyUser, ISmsListener
    {
        #region Controls

        private Spinner SpDocumentType, SpDay, SpMonth, SpYear;
        private TextView TvEdit, TvEditYourData, TvNames, TvLastNames, TvDocumentType,
            TvBirthDate, TvCellPhone, TvEmail, TvTerms;
        private EditText EtNames, EtLastNames, EtDocumentNumber, EtCellphone, EtEmail;
        private CheckBox ChkTerms;
        private Button BtnEdit;

        #endregion

        #region Properties

        private MyAccountModel _myAccountModel;        
        private string Gender { get; set; }
        private IList<DocumentType> documentsType;
        private ResponseBase EditProfileResponse { get; set; }
        private User User { get; set; }
        private User UserConsulted { get; set; }
        private bool ProcessChangeCellPhone { get; set; }

        #endregion

        #region Publics

        public void MessageReceived(string message)
        {
            if (EtVerifyCode != null)
            {
                EtVerifyCode.Text = message;
            }
        }

        public override void ResendAsync()
        {
            CloseModal();

            RunOnUiThread(async () =>
            {
                await SendSMSVerifyCode();
            });
        }

        public override void VerifyCodeAsync()
        {
            string code = EtVerifyCode.Text;
            CloseModal();
            RunOnUiThread(async () =>
            {
                await VerifyUser(code);
            });
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            if (ProcessChangeCellPhone)
            {
                RunOnUiThread(async () =>
                {
                    await UpdateVerifyUser(UserConsulted);
                });
            }

            HideProgressDialog();
            Finish();
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
            _myAccountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
            _UserModel = new UserModel(new UserService(DeviceManager.Instance));
            documentsType = new List<DocumentType>();
            SetContentView(Resource.Layout.ActivityEditProfile);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();
            await this.GetDocumentsType();
            await GetUser();
            ValidateReadSmsAvailability();
        }

        #endregion

        #region Private

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            SpDocumentType = FindViewById<Spinner>(Resource.Id.spDocumentType);
            SpDay = FindViewById<Spinner>(Resource.Id.spDay);
            SpMonth = FindViewById<Spinner>(Resource.Id.spMonth);
            SpYear = FindViewById<Spinner>(Resource.Id.spYear);
            TvEdit = FindViewById<TextView>(Resource.Id.tvEdit);
            TvEditYourData = FindViewById<TextView>(Resource.Id.tvEditYourData);
            TvNames = FindViewById<TextView>(Resource.Id.tvNames);
            TvLastNames = FindViewById<TextView>(Resource.Id.tvLastNames);
            TvDocumentType = FindViewById<TextView>(Resource.Id.tvDocumentType);
            TvBirthDate = FindViewById<TextView>(Resource.Id.tvBirthDate);
            TvCellPhone = FindViewById<TextView>(Resource.Id.tvPhoneNumber);
            TvEmail = FindViewById<TextView>(Resource.Id.tvEmail);
            TvTerms = FindViewById<TextView>(Resource.Id.tvTerms);
            EtNames = FindViewById<EditText>(Resource.Id.etName);
            EtLastNames = FindViewById<EditText>(Resource.Id.etLastName);
            EtDocumentNumber = FindViewById<EditText>(Resource.Id.etDocumentNumber);
            EtCellphone = FindViewById<EditText>(Resource.Id.etPhoneNumber);
            EtEmail = FindViewById<EditText>(Resource.Id.etEmail);
            ChkTerms = FindViewById<CheckBox>(Resource.Id.chkTerms);
            BtnEdit = FindViewById<Button>(Resource.Id.btnEdit);
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            TvTerms.Click += delegate { OpenTerms(); };
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            BtnEdit.Click += async delegate { await EditProfile(); };
            this.HideItemsToolbar(this);
            this.EditFonts();
            var chooseList = new List<DocumentType>();
            Typeface typeface = FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium);
            MySpinnerAdapter documentTypeAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, chooseList.ToList().Select(x => x.Name).ToArray());
            SpDocumentType.Adapter = documentTypeAdapter;
            List<Item> itemsDays = JsonService.Deserialize<List<Item>>(AppConfigurations.DaysSource);
            List<Item> itemsMonths = JsonService.Deserialize<List<Item>>(AppConfigurations.MonthsSource);
            List<Item> itemsYears = ModelHelper.GetYears();

            MySpinnerAdapter dayAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, itemsDays.Select(x => x.Text).ToArray());
            SpDay.Adapter = dayAdapter;

            MySpinnerAdapter monthAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, itemsMonths.Select(x => x.Text).ToArray());
            SpMonth.Adapter = monthAdapter;

            MySpinnerAdapter yearAdapter = new MySpinnerAdapter(this, Resource.Layout.spinner_layout, itemsYears.Select(x => x.Text).ToArray());
            SpYear.Adapter = yearAdapter;
            HideItemsToolbar(this);
        }

        private void OnDrawData()
        {
            EtNames.Text = ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.FirstName) ? ParametersManager.UserContext.FirstName : string.Empty;
            EtLastNames.Text = ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.LastName) ? ParametersManager.UserContext.LastName : string.Empty;
            EtDocumentNumber.Text = ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.DocumentNumber) ? ParametersManager.UserContext.DocumentNumber : string.Empty;
            EtEmail.Text = ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.Email) ? ParametersManager.UserContext.Email : string.Empty;
            EtCellphone.Text = ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone) ? ParametersManager.UserContext.CellPhone : string.Empty;

            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.DocumentType.ToString()))
            {
                var nameDocument = documentsType.Where(x => x.Id.Equals(ParametersManager.UserContext.DocumentType.ToString())).Select(x => x.Name).FirstOrDefault();

                if (!string.IsNullOrEmpty(nameDocument))
                {
                    ArrayAdapter<string> adaptador = (ArrayAdapter<string>)SpDocumentType.Adapter;
                    SpDocumentType.SetSelection(adaptador.GetPosition(nameDocument));
                    SpDocumentType.Enabled = false;
                    SpDocumentType.Clickable = false;
                }
            }

            if (ParametersManager.UserContext != null && !string.IsNullOrEmpty(ParametersManager.UserContext.DateOfBirth.ToString()))
            {
                var dateOfBirth = ParametersManager.UserContext.DateOfBirth.ToString();

                if (!string.IsNullOrEmpty(dateOfBirth))
                {
                    string[] date = dateOfBirth.Split(new char[] { '/' });
                    string month = date[0];
                    if (month.StartsWith('0')) { month = month.Substring(1); }
                    string day = date[1];
                    if (day.StartsWith('0')) { day = day.Substring(1); }
                    string year = date[2];
                    ArrayAdapter<string> adaptadorDay = (ArrayAdapter<string>)SpDay.Adapter;
                    SpDay.SetSelection(adaptadorDay.GetPosition(day));

                    ArrayAdapter<string> adaptadorMonth = (ArrayAdapter<string>)SpMonth.Adapter;
                    SpMonth.SetSelection(adaptadorMonth.GetPosition(month));

                    ArrayAdapter<string> adaptadorYear = (ArrayAdapter<string>)SpYear.Adapter;
                    SpYear.SetSelection(adaptadorYear.GetPosition(year));
                }
            }
        }

        private void OpenTerms()
        {
            Intent intent = new Intent(this, typeof(TermsActivity));
            intent.PutExtra(ConstantPreference.Terms, AppServiceConfiguration.TermsAndConditionApp);
            StartActivity(intent);
        }

        private void EditFonts()
        {
            TvEdit.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvEditYourData.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvLastNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvDocumentType.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvBirthDate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvCellPhone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvTerms.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            EtNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtLastNames.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtDocumentNumber.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtCellphone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnEdit.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            ChkTerms.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void PullSpinners()
        {
            RunOnUiThread(() =>
            {
                Typeface typeface = FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium);
                CustomSpinnerAdapter documentTypeAdapter = new CustomSpinnerAdapter(this, Resource.Layout.spinner_layout,
                    documentsType.ToList().Select(x => x.Name).ToArray(), typeface);
                SpDocumentType.Adapter = documentTypeAdapter;
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
                    { ConstantActivityName.EditProfileActivity, ConstantMethodName.GetDocumentTypes } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private User SetUser()
        {
            return new User()
            {
                FirstName = EtNames.Text,
                LastName = EtLastNames.Text,
                DocumentType = int.Parse(documentsType[SpDocumentType.SelectedItemPosition].Id),
                DocumentNumber = EtDocumentNumber.Text,
                DateOfBirth = GetDateOfBirth(),
                CellPhone = EtCellphone.Text,
                Email = EtEmail.Text,
                AcceptTerms = ChkTerms.Checked
            };
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

        private async Task EditProfile()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                User = this.SetUser();
                string message = _myAccountModel.ValidateFields(User);

                if (string.IsNullOrEmpty(message))
                {
                    if (!UserConsulted.CellPhone.Equals(User.CellPhone))
                    {
                        await SendSMSVerifyCode();
                    }
                    else
                    {
                        EditProfileResponse = await _myAccountModel.UpdateUser(User);
                        this.ValidateResponseEditProfile();
                    }
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
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.EditProfileActivity, ConstantMethodName.EditProfile } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
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

        private void ValidateResponseEditProfile()
        {
            if (EditProfileResponse.Result != null && EditProfileResponse.Result.HasErrors && EditProfileResponse.Result.Messages != null)
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();

                    var errorClientExists = EditProfileResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ClientExists)).Any();

                    if (errorClientExists)
                    {
                        var errorCode = EditProfileResponse.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ClientExists)).First();
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
                ProcessChangeCellPhone = false;
                this.SuccessEditProfile();
            }
        }

        private void SuccessEditProfile()
        {
            HideProgressDialog();
            this.OnBackPressed();
            Finish();
        }

        private async Task GetUser()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var response = await _myAccountModel.GetUser();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    ShowErrorLayout(MessagesHelper.GetMessage(response.Result));
                }
                else
                {
                    SavetUser(response.User);
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ClientPrimeActivity, ConstantMethodName.GetUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void SavetUser(User user)
        {
            UserConsulted = user;
            UserContext userContext = ModelHelper.UpdateUserContext(ParametersManager.UserContext, user);
            userContext.Address = ParametersManager.UserContext.Address;
            userContext.Store = ParametersManager.UserContext.Store;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
            this.OnDrawData();
        }

        public async Task SendSMSVerifyCode()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                string messageValidation = _UserModel.ValidateCellPhoneField(User.CellPhone);

                if (string.IsNullOrEmpty(messageValidation))
                {
                    VerifyUserParameters parameters = SetParametersSendSMS();
                    SendMessageVerifyUserResponse response = await SendSMSVerifyCode(parameters);
                    this.ValidateResponseSendSms(response);
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
                    { ConstantActivityName.EditProfileActivity, ConstantMethodName.SendSMSVerifyCode } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        public VerifyUserParameters SetParametersSendSMS()
        {
            VerifyUserParameters parameters = new VerifyUserParameters
            {
                CellPhone = User.CellPhone,
                DocumentNumber = User.DocumentNumber,
                SiteId = AppServiceConfiguration.SiteId,
            };

            return parameters;
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
                ProcessChangeCellPhone = true;
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

        private async Task VerifyUser(string code)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                string messageValidation = _UserModel.ValidateCodeField(code.TrimStart().TrimEnd());

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
                    { ConstantActivityName.EditProfileActivity, ConstantMethodName.VerifyUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
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
                        HideProgressDialog();
                        MessageVerifyCode = MessagesHelper.GetMessage(response.Result);
                        ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                    }
                });
            }
            else if (response.Verified)
            {
                EditProfileResponse = await _myAccountModel.UpdateUser(User);
                this.ValidateResponseEditProfile();
            }
            else
            {
                RunOnUiThread(() =>
                {
                    HideProgressDialog();
                    MessageVerifyCode = AppMessages.VerifyUserError;
                    ShowVerifyCodeDialog(EnumTypesVerifyCode.Error);
                });
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

        #endregion
    }
}