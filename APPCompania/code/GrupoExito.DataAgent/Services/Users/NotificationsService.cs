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

    public class NotificationsService : BaseApiService, INotificationsService
    {
        public NotificationsService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<NotificationsResponse> GetNotifications()
        {
            string endpoint = AppServiceConfiguration.NotificationsEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new NotificationsResponse());
                return (NotificationsResponse)Convert.ChangeType(responseBase, typeof(NotificationsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new NotificationsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<NotificationsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
