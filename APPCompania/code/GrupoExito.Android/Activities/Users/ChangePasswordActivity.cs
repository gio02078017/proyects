using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Activities.Base;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Services;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Users
{
    [Activity(Label = "Cambiar Contraseña", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ChangePasswordActivity : BaseActivity
    {
        #region Controls

        private ImageView IvShowPassword;
        private ImageView IvShowConfirmPassword;
        private TextView TvChangePassword;
        private TextView TvChangingPassword;
        private TextView TvActualPassword;
        private TextView TvNewPassword;
        private TextView TvPasswordRule;
        private TextView TvConfirmPassword;
        private EditText EtActualPassword;
        private EditText EtNewPassword;
        private EditText EtConfirmPassword;
        private Button BtnUpdate;
        private LinearLayout LyConfirmPassword;
        private LinearLayout LyNewPassword;

        #endregion

        #region Properties

        private PasswordModel _passwordModel;
        private bool PasswordShowed = false;
        private bool ConfirmPasswordShowed = false;
        private bool PasswordChanged = false;
        private bool IsLogin = false;

        #endregion

        public override void OnBackPressed()
        {
            Finish();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ActivityChangePassword);
            _passwordModel = new PasswordModel(new PasswordService(DeviceManager.Instance));
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.mainToolbar);
            SetActionBar(toolbar);

            if (SupportActionBar != null)
            {
                SupportActionBar.Hide();
            }

            SetControlsProperties();
            HideItemsToolbar(this);
            EditFonts();            
        }

        protected override void OnResume()
        {
            base.OnResume();
            FirebaseRegistrationEventsService.Instance.RegisterScreen(this, AnalyticsScreenView.ChangePassword, typeof(ChangePasswordActivity).Name);
        }

        protected override void HandlerOkButton(object sender, DialogClickEventArgs e)
        {
            base.HandlerOkButton(sender, e);

            if (PasswordChanged)
            {
                if (IsLogin)
                {
                    GoToLobby();
                }
                else
                {
                    OnBackPressed();
                }
            }
        }

        private void SetControlsProperties()
        {
            Bundle bundle = Intent.Extras;

            if (bundle != null)
            {
                IsLogin = Intent.Extras.GetBoolean(ConstantPreference.IsLogin);

                if (IsLogin)
                {
                    FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Gone;
                }
                else
                {
                    FindViewById<ImageView>(Resource.Id.ivToolbarBack).Visibility = ViewStates.Visible;
                }
            }

            SetLoader(FindViewById<RelativeLayout>(Resource.Id.layoutLoader));
            FindViewById<LinearLayout>(Resource.Id.lyPrice).Visibility = ViewStates.Invisible;
            IvToolbarBack = FindViewById<ImageView>(Resource.Id.ivToolbarBack);
            IvToolbarBack.Click += delegate { OnBackPressed(); };
            FindViewById<LinearLayout>(Resource.Id.lyUser).Click += delegate { OnBackPressed(); };
            LyConfirmPassword = FindViewById<LinearLayout>(Resource.Id.lyConfirmPassword);
            LyNewPassword = FindViewById<LinearLayout>(Resource.Id.lyNewPassword);
            TvChangePassword = FindViewById<TextView>(Resource.Id.tvChangePassword);
            TvChangingPassword = FindViewById<TextView>(Resource.Id.tvChangingPassword);
            TvActualPassword = FindViewById<TextView>(Resource.Id.tvActualPassword);
            TvNewPassword = FindViewById<TextView>(Resource.Id.tvNewPassword);
            TvPasswordRule = FindViewById<TextView>(Resource.Id.tvPasswordRule);
            TvConfirmPassword = FindViewById<TextView>(Resource.Id.tvConfirmPassword);
            EtActualPassword = FindViewById<EditText>(Resource.Id.etActualPassword);
            EtNewPassword = FindViewById<EditText>(Resource.Id.etNewPassword);
            EtConfirmPassword = FindViewById<EditText>(Resource.Id.etConfirmPassword);
            BtnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);
            IvShowPassword = FindViewById<ImageView>(Resource.Id.ivShowPassword);
            IvShowConfirmPassword = FindViewById<ImageView>(Resource.Id.ivShowConfirmPassword);

            SpannableStringBuilder strChangingPassword = new SpannableStringBuilder(GetString(Resource.String.str_changing_password));
            strChangingPassword.SetSpan(new ForegroundColorSpan(Color.Black), 33, 40, SpanTypes.ExclusiveExclusive);
            strChangingPassword.SetSpan(new StyleSpan(TypefaceStyle.Bold), 33, 40, SpanTypes.ExclusiveExclusive);
            TvChangingPassword.TextFormatted = strChangingPassword;

            BtnUpdate.Click += async delegate { await ChangePassword().ConfigureAwait(false); };
            IvShowPassword.Click += delegate { ShowOrHideNewPasword(); };
            IvShowConfirmPassword.Click += delegate { ShowOrHideConfirmPasword(); };
        }

        private void EditFonts()
        {
            TvChangePassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvActualPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvNewPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvConfirmPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnUpdate.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);

            TvChangingPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            TvPasswordRule.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtActualPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtNewPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            EtConfirmPassword.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
        }

        private void ShowOrHideNewPasword()
        {
            if (PasswordShowed)
            {
                PasswordShowed = false;
                EtNewPassword.TransformationMethod = new PasswordTransformationMethod();
            }
            else
            {
                PasswordShowed = true;
                EtNewPassword.TransformationMethod = null;
            }
        }

        private void ShowOrHideConfirmPasword()
        {
            if (ConfirmPasswordShowed)
            {
                ConfirmPasswordShowed = false;
                EtConfirmPassword.TransformationMethod = new PasswordTransformationMethod();
            }
            else
            {
                ConfirmPasswordShowed = true;
                EtConfirmPassword.TransformationMethod = null;
            }
        }

        private UserCredentials GetUserCredential()
        {
            return new UserCredentials
            {
                OldPassword = EtActualPassword.Text,
                NewPassword = EtNewPassword.Text,
                ConfirmPassword = EtConfirmPassword.Text
            };
        }

        private async Task ChangePassword()
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                var userCredential = GetUserCredential();
                var message = _passwordModel.ValidateFieldsChangePassword(userCredential);

                if (string.IsNullOrEmpty(message))
                {
                    var response = await _passwordModel.ChangePassword(userCredential).ConfigureAwait(false);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    }
                    else
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.ChangePasswordMessage, AppMessages.AcceptButtonText);
                        PasswordChanged = true;
                    }
                }
                else
                {
                    if (message.Equals(AppMessages.RequiredFieldsText))
                    {
                        this.ModifyFieldsStyle(userCredential);
                    }

                    HideProgressDialog();
                    DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText);
                }
            }
            catch (Exception ex)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.ChangeKeyActivity, ConstantMethodName.UpdateKey } };
                ShowAndRegisterMessageExceptions(ex, properties);
            }
        }

        private void GoToLobby()
        {
            Intent intent = new Intent(this, typeof(LobbyActivity));
            StartActivity(intent);
            Finish();
        }

        private void ModifyFieldsStyle(UserCredentials userCredential)
        {
            if (string.IsNullOrEmpty(userCredential.OldPassword))
            {
                EtActualPassword.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                EtActualPassword.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(userCredential.NewPassword))
            {
                LyNewPassword.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                LyNewPassword.SetBackgroundResource(Resource.Drawable.button_products);
            }

            if (string.IsNullOrEmpty(userCredential.ConfirmPassword))
            {
                LyConfirmPassword.SetBackgroundResource(Resource.Drawable.field_error);
            }
            else
            {
                LyConfirmPassword.SetBackgroundResource(Resource.Drawable.button_products);
            }
        }
    }
}