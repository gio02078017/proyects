namespace GrupoExito.DataAgent.Services.Generic
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class AppVersionService : BaseApiService, IAppVersionService
    {
        public AppVersionService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
        }

        public async Task<AppVersionResponse> GetAppVersion(string operatingASystem)
        {
            string endpoint = string.Format(AppServiceConfiguration.AppVersionEndPoint, operatingASystem);
            var response = await HttpClientBaseService.GetAsync(endpoint).ConfigureAwait(false);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new AppVersionResponse());
                return (AppVersionResponse)Convert.ChangeType(responseBase, typeof(AppVersionResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new AppVersionResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<AppVersionResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
