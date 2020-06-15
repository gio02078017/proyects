namespace GrupoExito.DataAgent.Services.Users
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class LoginService : BaseApiService, ILoginService
    {
        public LoginService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<LoginResponse> Login(UserCredentials user)
        {
            var beforeResponse = await HttpClientBaseService.ValidateConnected();

            if (beforeResponse == null)
            {
                await SaveKey();
                user.Password = CryptographyService.Encrypt(user.Password, DeviceManager.GetAccessPreference(ConstPreferenceKeys.SharedKey));
                string endpoint = AppServiceConfiguration.GetLoginEndpoint;
                var response = await HttpClientBaseService.PostAsync(endpoint, user);

                if (response.Content == null)
                {
                    var responseBase = ValidateResponse(response, new LoginResponse());
                    return (LoginResponse)Convert.ChangeType(responseBase, typeof(LoginResponse));
                }
                else
                {
                    string data = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(data))
                    {
                        return new LoginResponse
                        {
                            Result = new MessagesContainer { HasErrors = false },
                            HttpResponse = GetNoContentResponse()
                        };
                    }

                    LoginResponse result = JsonService.Deserialize<LoginResponse>(data);

                    if (response.IsSuccessStatusCode && result != null && !string.IsNullOrEmpty(result.Token))
                    {
                        TokenResponse tokenResponse = await HttpClientBaseService.SaveToken(response);
                    }

                    if (result != null)
                    {
                        result.HttpResponse = response;
                    }

                    return result;
                }
            }
            else
            {

                var responseBase = ValidateResponse(beforeResponse, new LoginResponse());
                return (LoginResponse)Convert.ChangeType(responseBase, typeof(LoginResponse));
            }
        }
    }
}
