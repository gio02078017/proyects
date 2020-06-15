
namespace GrupoExito.DataAgent.Services.Users
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Users;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Users;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class PointsService : BaseApiService, IPointsService
    {
        public PointsService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<PointsResponse> GetUserPoints()
        {
            string endpoint = AppServiceConfiguration.UserPointsEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new PointsResponse());
                return (PointsResponse)Convert.ChangeType(responseBase, typeof(PointsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new PointsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<PointsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
