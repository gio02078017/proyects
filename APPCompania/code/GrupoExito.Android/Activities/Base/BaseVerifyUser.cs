using Android;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asv7 = Android.Support.V7.App;

namespace GrupoExito.Android.Activities.Base
{
    public class BaseVerifyUser : BaseActivity
    {
        #region Properties

        private Asv7.AlertDialog activeCodeDialog;
        public Asv7.AlertDialog ActiveCodeDialog
        {
            get { return activeCodeDialog; }
            set { activeCodeDialog = value; }
        }

        private EditText etVerifyCode;
        public EditText EtVerifyCode
        {
            get { return etVerifyCode; }
            set { etVerifyCode = value; }
        }

        private EditText etCellPhoneVerifyCode;
        public EditText EtCellPhoneVerifyCode
        {
            get { return etCellPhoneVerifyCode; }
            set { etCellPhoneVerifyCode = value; }
        }

        private string messageVerifyCode;
        public string MessageVerifyCode
        {
            get { return messageVerifyCode; }
            set { messageVerifyCode = value; }
        }

        private bool firstTimeSendCode;
        public bool FirstTimeSendCode
        {
            get { return firstTimeSendCode; }
            set { firstTimeSendCode = value; }
        }

        private UserModel _userModel;
        public UserModel _UserModel
        {
            get { return _userModel; }
            set { _userModel = value; }
        }

        private TextView TvVerifyCodeSubtitle;
        private TextView TvSendCodeMessage;
        private ImageView ImgClose;
        private LinearLayout LyEvents;
        private LinearLayout LyError;
        private LinearLayout LySuccess;
        private LinearLayout LyVerify;
        private LinearLayout LyIntoCellPhone;
        private LinearLayout LySendCod;
        private EnumTypesVerifyCode LastEvent = EnumTypesVerifyCode.None;
        private const int RequestSmsnId = 1;
        private string _cellPhone;

        #endregion

        #region Public

        public virtual void SendSmsCodeAsync()
        {
        }

        public virtual void EditCellPhoneAsync()
        {
        }

        public virtual void VerifyCodeAsync()
        {
        }

        public virtual void ResendAsync()
        {
        }

        public virtual void SaveCellPhone(string cellPhone)
        {
        }

        public void SetCellPhone(string cellPhone)
        {
            _cellPhone = cellPhone;
            var activityName = this.GetType().Name;

            if (activityName.Equals(ConstantActivityName.MyDiscountsActivity))
            {
                if (TvVerifyCodeSubtitle != null && !string.IsNullOrEmpty(cellPhone))
                {
                    TvVerifyCodeSubtitle.Text = string.Format(AppMessages.EntryVerificationCodeReceived, cellPhone);
                }
            }
            else
            {
                if (TvVerifyCodeSubtitle != null && !string.IsNullOrEmpty(cellPhone))
                {
                    TvVerifyCodeSubtitle.Text = string.Format(AppMessages.VerifyMessageSent, cellPhone);
                }
            }
        }

        public void SetCellPhoneVerifyCode(string cellPhone)
        {
            _cellPhone = cellPhone;

            if (TvSendCodeMessage != null && !string.IsNullOrEmpty(cellPhone))
            {
                TvSendCodeMessage.Text = string.Format(AppMessages.VerifyCodeMessageSendCode, cellPhone);
            }
        }

        public void CloseModal()
        {
            ActiveCodeDialog.Dismiss();
        }

        public void ValidateReadSmsAvailability()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadSms) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.ReadSms, Manifest.Permission.ReceiveSms }, RequestSmsnId);
            }
        }

        public async Task<SendMessageVerifyUserResponse> SendSMSVerifyCode(VerifyUserParameters parameters)
        {
            UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
            return await userModel.SendMessageVerifyUser(parameters);
        }

        public async Task<VerifyUserResponse> VerifyUser(VerifyUserParameters parameters)
        {
            UserModel userModel = new UserModel(new UserService(DeviceManager.Instance));
            return await userModel.VerifyUser(parameters);
        }

        public void ShowVerifyCodeDialog(EnumTypesVerifyCode typeEvent)
        {
            if (ActiveCodeDialog != null && ActiveCodeDialog.IsShowing)
            {
                CloseModal();
            }

            ActiveCodeDialog = new Asv7.AlertDialog.Builder(this).Create();
            View viewActiveCode = LayoutInflater.Inflate(Resource.Layout.DialogVerifyCode, null);
            ActiveCodeDialog.SetView(viewActiveCode);
            ActiveCodeDialog.SetCanceledOnTouchOutside(false);
            ImgClose = viewActiveCode.FindViewById<ImageView>(Resource.Id.imgClose);
            LyEvents = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyEvents);
            LyError = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyError);
            LySuccess = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lySuccess);

            if (typeEvent == EnumTypesVerifyCode.Error)
            {
                this.ScreenDialogError(viewActiveCode);
                LyEvents.Visibility = ViewStates.Gone;
            }
            else if (typeEvent == EnumTypesVerifyCode.Success)
            {
                this.ScreenDialogSuccess(viewActiveCode);
                LyEvents.Visibility = ViewStates.Gone;
            }
            else
            {
                LastEvent = typeEvent;
                ScreenDialogEvents(viewActiveCode, typeEvent);
            }

            ImgClose.Click += delegate { CloseModal(); };
            ActiveCodeDialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
            ActiveCodeDialog.Show();
        }

        public void SaveValidatedUser(bool userActivate)
        {
            UserContext userContext = ParametersManager.UserContext;
            userContext.UserActivate = userActivate;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
        }

        public void SaveValidatedCellPhone(string cellPhone)
        {
            UserContext userContext = ParametersManager.UserContext;
            userContext.CellPhone = cellPhone;
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
        }

        public async Task UpdateVerifyUser(User UserConsulted)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                VerifyUserParameters parameters = new VerifyUserParameters
                {
                    CellPhone = UserConsulted.CellPhone,
                    IsValidated = UserConsulted.UserActivate,
                    DocumentNumber = UserConsulted.DocumentNumber,
                    SiteId = AppServiceConfiguration.SiteId
                };

                await _userModel.UpdateVerifyUser(parameters);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseVerifyUser, ConstantMethodName.UpdateVerifyUser } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }
        }

        #endregion

        #region Private

        private void ScreenDialogError(View viewActiveCode)
        {
            LyError.Visibility = ViewStates.Visible;
            LySuccess.Visibility = ViewStates.Gone;
            TextView TvApology = viewActiveCode.FindViewById<TextView>(Resource.Id.tvApology);
            TextView TvCannotFinishChange = viewActiveCode.FindViewById<TextView>(Resource.Id.tvCannotFinishChange);
            Button BtnReturn = viewActiveCode.FindViewById<Button>(Resource.Id.btnReturn);
            TvApology.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvCannotFinishChange.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnReturn.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvCannotFinishChange.Text = messageVerifyCode;

            BtnReturn.Click += delegate
            {
                ReturnView();
            };
        }

        private void ScreenDialogSuccess(View viewActiveCode)
        {
            LyError.Visibility = ViewStates.Gone;
            LySuccess.Visibility = ViewStates.Visible;
            TextView TvSuccessMessage = viewActiveCode.FindViewById<TextView>(Resource.Id.tvSuccessMessage);
            TextView TvSuccess = viewActiveCode.FindViewById<TextView>(Resource.Id.tvSuccess);
            Button BtnAccept = viewActiveCode.FindViewById<Button>(Resource.Id.btnAccept);
            TvSuccess.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            TvSuccessMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            BtnAccept.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Bold);
            BtnAccept.Click += delegate { CloseModal(); };
        }

        private void ScreenDialogEvents(View viewActiveCode, EnumTypesVerifyCode typeEvent)
        {
            LyEvents.Visibility = ViewStates.Visible;
            LyError.Visibility = ViewStates.Gone;
            LySuccess.Visibility = ViewStates.Gone;
            LyVerify = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyVerify);
            LyIntoCellPhone = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyIntoCellPhone);
            LySendCod = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lySendCod);
            TextView TvVerifyCodeTitle = viewActiveCode.FindViewById<TextView>(Resource.Id.tvVerifyCodeTitle);
            TvVerifyCodeTitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayExtraBold), TypefaceStyle.Normal);

            if (typeEvent == EnumTypesVerifyCode.VerifyCode)
            {
                ScreenDialogVerifyCode(viewActiveCode);
            }
            else if (typeEvent == EnumTypesVerifyCode.SendCod)
            {
                ScreenDialogSendCod(viewActiveCode);
            }
            else
            {
                ScreenDialogGetInfoCellPhone(viewActiveCode, typeEvent);
            }
        }

        private void ScreenDialogVerifyCode(View viewActiveCode)
        {
            LyVerify.Visibility = ViewStates.Visible;
            LyIntoCellPhone.Visibility = ViewStates.Gone;
            LySendCod.Visibility = ViewStates.Gone;
            TvVerifyCodeSubtitle = viewActiveCode.FindViewById<TextView>(Resource.Id.tvVerifyCodeSubtitle);
            EtVerifyCode = viewActiveCode.FindViewById<EditText>(Resource.Id.etVerifyCode);
            TvVerifyCodeSubtitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            EtVerifyCode.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LinearLayout LyBtnVerify = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyBtnVerify);
            TextView TvVerify = viewActiveCode.FindViewById<TextView>(Resource.Id.tvVerify);
            TvVerify.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TextView TvReSend = viewActiveCode.FindViewById<TextView>(Resource.Id.tvReSend);
            TvReSend.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            LyBtnVerify.Click += delegate { VerifyCodeAsync(); };
            TvReSend.Click += delegate { ResendAsync(); };
        }

        private void ScreenDialogGetInfoCellPhone(View viewActiveCode, EnumTypesVerifyCode typeEvent)
        {
            LyVerify.Visibility = ViewStates.Gone;
            LyIntoCellPhone.Visibility = ViewStates.Visible;
            LySendCod.Visibility = ViewStates.Gone;
            TextView TvIntoCellPhone = viewActiveCode.FindViewById<TextView>(Resource.Id.tvIntoCellPhone);
            EtCellPhoneVerifyCode = viewActiveCode.FindViewById<EditText>(Resource.Id.etCellPhone);
            TvIntoCellPhone.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            EtCellPhoneVerifyCode.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Normal);
            LinearLayout LyBtnContinue = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyBtnContinue);
            TextView TvBtnContinue = viewActiveCode.FindViewById<TextView>(Resource.Id.tvBtnContinue);
            TvBtnContinue.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            TextView TvCancel = viewActiveCode.FindViewById<TextView>(Resource.Id.tvCancel);
            TvCancel.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            LyBtnContinue.Click += delegate { SaveCellPhone(EtCellPhoneVerifyCode.Text); };

            if (typeEvent == EnumTypesVerifyCode.EditCellPhone)
            {
                TvCancel.Text = GetString(Resource.String.str_return);
                TvIntoCellPhone.Text = GetString(Resource.String.str_into_cell_phone);
                TvCancel.Click += delegate { ScreenDialogSendCod(viewActiveCode); };
            }
            else
            {
                TvCancel.Text = GetString(Resource.String.str_cancel);
                TvCancel.Click += delegate { CloseModal(); };
                TvIntoCellPhone.Text = GetString(Resource.String.str_ask_cell_phone);
            }
        }

        private void ScreenDialogSendCod(View viewActiveCode)
        {
            LyVerify.Visibility = ViewStates.Gone;
            LyIntoCellPhone.Visibility = ViewStates.Gone;
            LySendCod.Visibility = ViewStates.Visible;
            TextView TvSendCodeSubtitle = viewActiveCode.FindViewById<TextView>(Resource.Id.tvSendCodeSubtitle);
            TvSendCodeMessage = viewActiveCode.FindViewById<TextView>(Resource.Id.tvSendCodeMessage);
            TvSendCodeSubtitle.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            TvSendCodeMessage.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewayMedium), TypefaceStyle.Bold);
            LinearLayout LyBtnSendCod = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyBtnSendCod);
            TextView TvSendCod = viewActiveCode.FindViewById<TextView>(Resource.Id.tvSendCod);
            TvSendCod.SetTypeface(FontManager.Instance.GetTypeFace(this, FontManager.RalewaySemiBold), TypefaceStyle.Bold);
            LinearLayout LyEdit = viewActiveCode.FindViewById<LinearLayout>(Resource.Id.lyEdit);
            LyBtnSendCod.Click += delegate { SendSmsCodeAsync(); };
            LyEdit.Click += delegate { EditCellPhoneAsync(); };
            SetCellPhoneVerifyCode(_cellPhone);
        }

        private void ReturnView()
        {
            if (LastEvent != EnumTypesVerifyCode.None && !FirstTimeSendCode)
            {
                ShowVerifyCodeDialog(LastEvent);
                SetCellPhone(_cellPhone);
            }
            else
            {
                CloseModal();
            }

            FirstTimeSendCode = false;
        }

        #endregion

        #region Verify code

        #endregion
    }
}