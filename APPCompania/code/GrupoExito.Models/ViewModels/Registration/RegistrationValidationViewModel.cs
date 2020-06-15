using System;
using System.Threading.Tasks;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Parameters.Users;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Models.Models;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Registration
{
    public class RegistrationValidationViewModel : BaseViewModel
    {
        private UserContext userContext;
        private IDeviceManager deviceManager;
        private BaseUserHelper baseUserHelper;

        private Exception exception;
        public Exception Exception
        {
            get { return exception; }
            set { SetProperty(ref exception, value); }
        }

        private bool registrationValidated;
        public bool RegistrationValidated
        {
            get { return registrationValidated; }
            set { SetProperty(ref registrationValidated, value); }
        }

        private bool messageSent;
        public bool MessageSent
        {
            get { return messageSent; }
            set { SetProperty(ref messageSent, value); }
        }

        private bool connectionUnavailable;
        public bool ConnectionUnavailable
        {
            get { return connectionUnavailable; }
            set { SetProperty(ref connectionUnavailable, value); }
        }

        private bool invalidEntries;
        public bool InvalidEntries
        {
            get { return invalidEntries; }
            set { SetProperty(ref invalidEntries, value); }
        }

        public Command EditProfileCommand { get; set; }
        public Command ValidateCommand { get; set; }
        public Command SendMessageCommand { get; set; }

        public RegistrationValidationViewModel(UserContext userContext, IDeviceManager deviceManager)
        {
            this.userContext = userContext;
            this.deviceManager = deviceManager;
            this.baseUserHelper = new BaseUserHelper(deviceManager);

            EditProfileCommand = new Command(async () => await ExecuteEditProfileCommand());
            ValidateCommand = new Command<VerifyUserParameters>(async (parameters) => await ExecuteValidateCommand(parameters));
            SendMessageCommand = new Command<VerifyUserParameters>(async (parameters) => await ExecuteSendMessageCommand(parameters));
        }

        private async Task ExecuteEditProfileCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (deviceManager.IsNetworkAvailable().Result)
                {

                }
                else ConnectionUnavailable = false;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteSendMessageCommand(VerifyUserParameters parameters)
        {
            if (parameters == null)
            {
                InvalidEntries = true;
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (deviceManager.IsNetworkAvailable().Result)
                {
                    MessageSent = await baseUserHelper.SendVerificationMessage(parameters);
                }
                else ConnectionUnavailable = false;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteValidateCommand(VerifyUserParameters parameters)
        {
            if (parameters == null || string.IsNullOrEmpty(parameters.Code))
            {
                InvalidEntries = true;
                return;
            }

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (deviceManager.IsNetworkAvailable().Result)
                {
                    bool validated = await baseUserHelper.VerifyUserEnd(parameters);

                    if (deviceManager.ValidateAccessPreference(ConstPreferenceKeys.User))
                    {
                        UpdateCellPhoneParameters phoneParameters = new UpdateCellPhoneParameters
                        {
                            CellPhone = parameters.CellPhone
                         };

                        await baseUserHelper.UpdateMobile(phoneParameters);
                    }

                    RegistrationValidated = validated;
                }
                else ConnectionUnavailable = false;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
