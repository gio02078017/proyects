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

    public class SignUpService : BaseApiService, ISignUpService
    {
        public SignUpService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
        }

        public async Task<ClientResponse> RegisterUser(User user)
        {
            await SaveKey();
            user.Password = CryptographyService.Encrypt(user.Password, DeviceManager.GetAccessPreference(ConstPreferenceKeys.SharedKey));
            user.ConfirmPassword = string.Empty;
            string endpoint = AppServiceConfiguration.RegisteClientEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, user);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ClientResponse());
                return (ClientResponse)Convert.ChangeType(responseBase, typeof(ClientResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ClientResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ClientResponse>(data);
                result.HttpResponse = response;

                await HttpClientBaseService.SaveToken(response);

                return result;
            }
        }
    }
}
