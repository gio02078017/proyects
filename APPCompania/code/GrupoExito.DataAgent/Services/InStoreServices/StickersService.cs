namespace GrupoExito.DataAgent.Services.InStoreServices
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.InStoreServices;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class StickersService : BaseApiService, IStickersService
    {
        public StickersService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<StickersResponse> GetSckers()
        {
            string endpoint = AppServiceConfiguration.GetBalanceEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new StickersResponse());
                return (StickersResponse)Convert.ChangeType(responseBase, typeof(StickersResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new StickersResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<StickersResponse>(data);
            result.HttpResponse = response;
            return result;
        }
    }
}
