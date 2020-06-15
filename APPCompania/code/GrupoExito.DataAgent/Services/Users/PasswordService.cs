namespace GrupoExito.DataAgent.Services.Users
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class PasswordService : BaseApiService, IPasswordService
    {
        public PasswordService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<ResponseBase> ChangePassword(UserCredentials userCredentials)
        {
            await SaveKey();
            userCredentials.OldPassword = CryptographyService.Encrypt(userCredentials.OldPassword,
                DeviceManager.GetAccessPreference(ConstPreferenceKeys.SharedKey));

            userCredentials.NewPassword = CryptographyService.Encrypt(userCredentials.NewPassword,
                DeviceManager.GetAccessPreference(ConstPreferenceKeys.SharedKey));

            userCredentials.ConfirmPassword = CryptographyService.Encrypt(userCredentials.ConfirmPassword,
                DeviceManager.GetAccessPreference(ConstPreferenceKeys.SharedKey));

            string endpoint = AppServiceConfiguration.UpdatePasswordEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, userCredentials).ConfigureAwait(false);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> ResetPassword(string email)
        {
            string endpoint = string.Format(AppServiceConfiguration.RecoverPasswordEndpoint, email);
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                ResponseBase result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
