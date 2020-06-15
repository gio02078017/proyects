using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Inicio de sesión", ScreenOrientation = ScreenOrientation.Portrait)]
    public class LoginActivity : BaseAddressesActivity
    {
        #region Controls

        private View viewRecoverPassword;
        private EditText EtEmail;
        private EditText EtPassword;
        private EditText EtEmailRecoverPassword;
        private Button BtnLogin;
        private Button BtnSend;
        private Button BtnReturn;
        private Button BtnAccept;
        private TextView TvAccountLabel;
        private TextView TvExitoWorld;
        private TextView TvEmail;
        private TextInputLayout TviEmail;
        private TextView TvPassword;
        private TextInputLayout TviPassword;
        private TextView TvPasswordRule;
        private TextView TvForgotPassword;
        private TextView TvLogIn;
        private TextView TvNoAccount;
        private TextView TvCannotFinishChange;
        private TextView TvApology;
        private TextView TvSuccessMessage;
        private TextView TvSuccess;
        private TextView TvSignUp;
        private ImageView ImgCloseOne;
        private ImageView ImgCloseTwo;
        private LinearLayout LlChangePassword;
        private LinearLayout LlChangePasswordError;
        private LinearLayout LlChangePasswordSuccess;
        private AlertDialog ForgotDialog;
        private NestedScrollView NsvBody;

        #endregion

        #region Properties

        private bool PasswordShowed = false;
        private LoginModel _loginModel;
        private LoginResponse LoginResponse { get; set; }

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            try
            {
                if (LoginResponse != null)
                {
                    var errorCode = (EnumErrorCode)Enum.Parse(typeof(EnumErrorCode), LoginResponse.Result.Messages.First().Code);

                    if (errorCode == EnumErrorCode.ClientNotRegistered)
                    {
                        this.GoToSignUp();
                        this.LoginResponse = null;
                    }
                }
            }
            catch
            {
                HideProgressDialog();
                this.LoginResponse = null;
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _loginModel = new LoginModel(new LoginService(DeviceManager.Instance));
            SetContentView(Resource.Layout.ActivityLogin);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            this.SetControlsProperties();
            this.EditFonts();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.Login, typeof(LoginActivity).Name);
        }

        private void SetControlsProperties()
        {
            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            EtEmail = FindViewById<EditText>(Resource.Id.etEmail);
            TviEmail = FindViewById<TextInputLayout>(Resource.Id.tviEmail);
            EtPassword = FindViewById<EditText>(Resource.Id.etPassword);
            TviPassword = FindViewById<TextInputLayout>(Resource.Id.tviPassword);
            BtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            TvSignUp = FindViewById<TextView>(Resource.Id.tvSignUp);
            TvAccountLabel = FindViewById<TextView>(Resource.Id.tvExitoAccount);
            TvLogIn = FindViewById<TextView>(Resource.Id.tvLogin);
            TvExitoWorld = FindViewById<TextView>(Resource.Id.tvExitoWorld);
            SpannableStringBuilder strExitoWorld = new SpannableStringBuilder(GetString(Resource.String.str_exito_world));
            strExitoWorld.SetSpan(new StyleSpan(TypefaceStyle.Bold), 13, 24, SpanTypes.ExclusiveExclusive);
            TvExitoWorld.TextFormatted = strExitoWorld;
            TvEmail = FindViewById<TextView>(Resource.Id.tvEmail);
            TvPassword = FindViewById<TextView>(Resource.Id.tvPassword);
            TvPasswordRule = FindViewById<TextView>(Resource.Id.tvPasswordRule);
            TvForgotPassword = FindViewById<TextView>(Resource.Id.tvForgotPassword);
            TvNoAccount = FindViewById<TextView>(Resource.Id.tvNoAccount);
            SpannableStringBuilder strExitoAccount = new SpannableStringBuilder(GetString(Resource.String.str_exito_account_label));
            strExitoAccount.SetSpan(new StyleSpan(TypefaceStyle.Bold), 18, 29, SpanTypes.ExclusiveExclusive);
            TvAccountLabel.TextFormatted = strExitoAccount;
            EtEmail.TextChanged += EditText_TextChanged;
            EtPassword.TextChanged += EditText_TextChanged;
            BtnLogin.Enabled = false;
            BtnLogin.Click += async delegate { await Login(); };
            TvSignUp.Click += delegate { GoToSignUp(); };
            TvForgotPassword.Click += delegate { ShowForgotPasswordDialog(); };
            NsvBody = FindViewById<NestedScrollView>(Resource.Id.nsvBody);
            BtnLogin.Enabled = false;
            BtnLogin.SetBackgroundResource(Resource.Drawable.button_transparent);
            EtEmail.AddTextChangedListener(new MyTextWatcher(this, EtEmail));
            EtPassword.AddTextChangedListener(new MyTextWatcher(this, EtPassword));
        }

        private void EditFonts()
        {
            TvLogIn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvExitoWorld.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvAccountLabel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            EtEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvPasswordRule.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvForgotPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnLogin.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvNoAccount.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            TvSignUp.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
        }

        private void ShowOrHidePasword()
        {
            if (PasswordShowed)
            {
                PasswordShowed = false;
                EtPassword.TransformationMethod = new PasswordTransformationMethod();
            }
            else
            {
                PasswordShowed = true;
                EtPassword.TransformationMethod = null;
            }
        }

        private void EditText_TextChanged(object sender, global::Android.Text.TextChangedEventArgs e)
        {
        }

        private void GoToSignUp()
        {
            HideProgressDialog();
            DeviceManager.HideKeyboard(this);
            var signUpView = new Intent(this, typeof(SignUpActivity));
            StartActivity(signUpView);
            Finish();
        }

        private async Task Login()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                UserCredentials userCredentials = SetUserCredential();
                string messageValidation = _loginModel.ValidateFields(userCredentials);

                if (string.IsNullOrEmpty(messageValidation))
                {
                    DeviceManager.HideKeyboard(this);
                    LoginResponse = await _loginModel.Login(userCredentials);
                    ValidateResponseLogin();
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, messageValidation, AppMessages.AcceptButtonText);
                    });
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.LoginActivity, ConstantMethodName.Login } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private UserCredentials SetUserCredential()
        {
            return new UserCredentials
            {
                Email = EtEmail.Text,
                Password = EtPassword.Text
            };
        }

        private void ShowForgotPasswordDialog()
        {
            ForgotDialog = new AlertDialog.Builder(this).Create();
            viewRecoverPassword = LayoutInflater.Inflate(Resource.Layout.DialogForgotePassword, null);
            ForgotDialog.SetView(viewRecoverPassword);
            ForgotDialog.SetCanceledOnTouchOutside(false);
            ImgCloseOne = viewRecoverPassword.FindViewById<ImageView>(Resource.Id.imgCloseOne);
            ImgCloseTwo = viewRecoverPassword.FindViewById<ImageView>(Resource.Id.imgCloseTwo);
            LlChangePassword = viewRecoverPassword.FindViewById<LinearLayout>(Resource.Id.llChangePassword);
            LlChangePasswordError = viewRecoverPassword.FindViewById<LinearLayout>(Resource.Id.llChangePasswordError);
            LlChangePasswordSuccess = viewRecoverPassword.FindViewById<LinearLayout>(Resource.Id.llChangePasswordSuccess);
            EtEmailRecoverPassword = viewRecoverPassword.FindViewById<EditText>(Resource.Id.etEmailRecoverPassword);
            BtnSend = viewRecoverPassword.FindViewById<Button>(Resource.Id.btnSend);
            BtnReturn = viewRecoverPassword.FindViewById<Button>(Resource.Id.btnReturn);
            BtnAccept = viewRecoverPassword.FindViewById<Button>(Resource.Id.btnAccept);
            TvApology = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvApology);
            TvCannotFinishChange = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvCannotFinishChange);
            TvSuccessMessage = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvSuccessMessage);
            TvSuccess = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvSuccess);
            TextView tvMessageChange = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvMessageChange);
            TextView tvTypeEmail = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvTypeEmail);
            TextView tvChangePassword = viewRecoverPassword.FindViewById<TextView>(Resource.Id.tvChangePassword);
            EtEmailRecoverPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvChangePassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvApology.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnSend.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnReturn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnAccept.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            tvMessageChange.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            tvTypeEmail.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvCannotFinishChange.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvSuccess.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvSuccessMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);

            ImgCloseOne.Click += delegate { ForgotDialog.Dismiss(); };
            ImgCloseTwo.Click += delegate { ForgotDialog.Dismiss(); };
            BtnSend.Click += async delegate { await RecoverPassword(); };
            BtnAccept.Click += delegate { ForgotDialog.Dismiss(); };
            BtnReturn.Click += delegate
            {
                LlChangePassword.Visibility = ViewStates.Visible;
                LlChangePasswordError.Visibility = ViewStates.Gone;
            };

            ForgotDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            ForgotDialog.Show();
        }

        private async Task RecoverPassword()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var passwordModel = new PasswordModel(new PasswordService(DeviceManager.Instance));

                string messageValidation = passwordModel.ValidateFieldsResetPassword(EtEmailRecoverPassword.Text.Trim());

                if (string.IsNullOrEmpty(messageValidation))
                {
                    var response = await passwordModel.ResetPassword(EtEmailRecoverPassword.Text);

                    HideProgressDialog();
                    LlChangePassword.Visibility = ViewStates.Visible;
                    LlChangePasswordError.Visibility = ViewStates.Gone;
                    LlChangePasswordSuccess.Visibility = ViewStates.Gone;

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        RunOnUiThread(() =>
                        {
                            LlChangePassword.Visibility = ViewStates.Gone;
                            LlChangePasswordSuccess.Visibility = ViewStates.Gone;
                            LlChangePasswordError.Visibility = ViewStates.Visible;
                            TvCannotFinishChange.Text = MessagesHelper.GetMessage(response.Result);
                        });
                    }
                    else
                    {
                        LlChangePassword.Visibility = ViewStates.Gone;
                        LlChangePasswordSuccess.Visibility = ViewStates.Visible;
                        LlChangePasswordError.Visibility = ViewStates.Gone;
                        TvSuccessMessage.Text = AppMessages.RecoverPasswordMessage;
                    }
                }
                else
                {
                    LlChangePassword.Visibility = ViewStates.Gone;
                    LlChangePasswordError.Visibility = ViewStates.Visible;
                    LlChangePasswordSuccess.Visibility = ViewStates.Gone;
                    TvCannotFinishChange.Text = messageValidation;
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.LoginActivity, ConstantMethodName.Login } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        private void ModifyFieldsStyle()
        {
            if (string.IsNullOrEmpty(EtEmailRecoverPassword.Text))
            {
                EtEmailRecoverPassword.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtEmailRecoverPassword.SetBackgroundResource(Resource.Drawable.button_products);
            }
        }

        private void ValidateResponseLogin()
        {
            if (LoginResponse != null && LoginResponse.Result != null && LoginResponse.Result.HasErrors && LoginResponse.Result.Messages != null)
            {
                this.ShowMessageError();
            }
            else
            {
                if (LoginResponse != null && LoginResponse.User != null && !string.IsNullOrEmpty(LoginResponse.User.Id))
                {
                    UserContext user = ParametersManager.UserContext;
                    UserContext userContext = ModelHelper.SetUserContext(LoginResponse.User);
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                    this.AnalyticLoginSuccessfulEvent();

                    RunOnUiThread(async () =>
                    {
                        if (ParametersManager.UserContext != null)
                        {
                            await GetUserAddress();
                        }
                    });

                    RunOnUiThread(async () =>
                    {
                        await RegisterCostumer();
                    });

                    this.ValidateGeneratePassword();
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
                    });
                }
            }
        }

        private async Task GetUserAddress()
        {
            try
            {
                var response = await GetAddresses();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    //Mostrar mensaje
                }
                else
                {
                    RunOnUiThread(async () =>
                    {
                        if (response != null && response.Addresses != null && response.Addresses.Any())
                        {
                            List<UserAddress> addresses = response.Addresses.ToList();

                            bool hasAnAddress = addresses != null && addresses.Any() ? true : false;
                            bool hasAnAddressDefault = hasAnAddress && addresses.Where(x => x.IsDefaultAddress == true) != null
                             && addresses.Where(x => x.IsDefaultAddress == true).Any() ? true : false;

                            if (hasAnAddressDefault)
                            {
                                var address = addresses.Where(x => x.IsDefaultAddress == true).FirstOrDefault();
                                SavePreferences(address);
                            }
                            else
                            {
                                var address = addresses.FirstOrDefault();
                                var responseSetDefaultAddress = await SetDefaultAddress(address, false);

                                if (responseSetDefaultAddress && addresses != null && addresses.Any())
                                {
                                    SavePreferences(addresses[0]);
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.LoginActivity, ConstantMethodName.GetUserAddress } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
        }

        private void SavePreferences(UserAddress address)
        {
            UserContext user = ParametersManager.UserContext;
            user.Address = address;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
            RegisterNotificationTags();
        }

        private void ShowMessageError()
        {
            HideProgressDialog();

            if (LoginResponse.Result.Messages.Any())
            {
                RunOnUiThread(() =>
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(LoginResponse.Result), AppMessages.AcceptButtonText);
                });
            }
            else
            {
                RunOnUiThread(() =>
                {
                    DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
                });
            }
        }

        private void ValidateGeneratePassword()
        {
            Intent intent = null;

            if (LoginResponse.User.GeneratedPassword)
            {
                intent = new Intent(this, typeof(ChangePasswordActivity));
                intent.PutExtra(ConstantPreference.IsLogin, true);
            }
            else
            {
                intent = new Intent(this, typeof(LobbyActivity));
            }

            StartActivity(intent);
            Finish();
        }

        private void AnalyticLoginSuccessfulEvent()
        {
            FirebaseRegistrationEventsService.Instance.SignIn();
        }

        public class MyTextWatcher : Java.Lang.Object, ITextWatcher
        {
            LoginActivity _LoginActivity;
            EditText _editText { get; set; }

            public MyTextWatcher(LoginActivity _LoginActivity, EditText editText)
            {
                this._LoginActivity = _LoginActivity;
                this._editText = editText;
            }
            public void AfterTextChanged(IEditable s) { }
            public void BeforeTextChanged(Java.Lang.ICharSequence arg0, int start, int count, int after) { }
            public void OnTextChanged(Java.Lang.ICharSequence arg0, int start, int before, int count)
            {
                _LoginActivity.ValidatedFields(arg0, _editText);
            }
        }

        public void ValidatedFields(Java.Lang.ICharSequence arg0, EditText editText)
        {

            switch (editText.Id)
            {
                case Resource.Id.etEmail:
                    ValidateEmail(arg0, true);
                    break;
                case Resource.Id.etPassword:
                    ValidatePassword(arg0, false);
                    break;
            }

            ValidatedButton();
        }

        public bool ValidateEmail(Java.Lang.ICharSequence arg0, bool drawError)
        {
            bool passed;

            if (!Patterns.EmailAddress.Matcher(arg0).Matches())
            {
                if (drawError)
                {
                    TviEmail.Error = AppMessages.EmailFormatErrorText;
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

            if (arg0.Length() < 8)
            {
                if (drawError)
                {
                    TviPassword.Error = AppMessages.PasswordLengthValidationText;
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

        public void ValidatedButton()
        {
            bool passed = true;

            if (!ValidateEmail(GetCharSequence(EtEmail.Text.ToString()), false))
            {
                passed = false;
            }

            if (!ValidatePassword(GetCharSequence(EtPassword.Text.ToString()), false))
            {
                passed = false;
            }

            EnabledButtom(passed);
        }

        public Java.Lang.ICharSequence GetCharSequence(string texto)
        {
            Java.Lang.ICharSequence charSequence = new Java.Lang.StringBuffer(texto);
            return charSequence;
        }

        public void EnabledButtom(bool enabled)
        {
            if (!enabled)
            {
                BtnLogin.Enabled = false;
                BtnLogin.SetBackgroundResource(Resource.Drawable.button_transparent);
            }
            else
            {
                BtnLogin.Enabled = true;
                BtnLogin.SetBackgroundResource(Resource.Drawable.button_yellow);
            }
        }
    }
}