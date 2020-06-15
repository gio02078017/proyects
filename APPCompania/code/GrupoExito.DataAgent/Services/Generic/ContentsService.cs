namespace GrupoExito.DataAgent.Services.Generic
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters.Generic;
    using GrupoExito.Entities.Responses.Generic;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class ContentsService : BaseApiService, IContentsService
    {
        private IDeviceManager _deviceManager;

        public ContentsService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public async Task<ContentHomeResponse> GetContentHome()
        {
            string endpoint = string.Format(AppServiceConfiguration.GetContentHomeEndPoint, GetCityId());
            var response = await HttpClientBaseService.GetAsync(endpoint).ConfigureAwait(false);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ContentHomeResponse());
                return (ContentHomeResponse)Convert.ChangeType(responseBase, typeof(ContentHomeResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ContentHomeResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ContentHomeResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<PromotionResponse> GetPromotions(PromotionParameters parameters)
        {
            parameters.LastDateUpdated = _deviceManager.GetAccessPreference(ConstPreferenceKeys.LastDateUpdated);
            parameters.IdCity = GetCityId();

            string endpoint = AppServiceConfiguration.PromotionEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters).ConfigureAwait(false);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ContentHomeResponse());
                return (PromotionResponse)Convert.ChangeType(responseBase, typeof(PromotionResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new PromotionResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<PromotionResponse>(data);

                if (response.IsSuccessStatusCode)
                {
                    await SaveDateUpdatePromotion(response);
                }

                result.HttpResponse = response;
                return result;
            }
        }

        private string GetCityId()
        {
            UserContext user = JsonService.Deserialize<UserContext>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.User));

            if (user != null)
            {
                return user.Address != null ? user.Address.CityId : user.Store.CityId;
            }

            return string.Empty;
        }

        private async Task SaveDateUpdatePromotion(HttpResponseMessage result)
        {
            var response = new PromotionResponse() { HttpResponse = result };
            response = await JsonService.GetSerializedResponse<PromotionResponse>(result);

            if (response != null && !string.IsNullOrEmpty(response.LastDateUpdated))
            {
                _deviceManager.SaveAccessPreference(ConstPreferenceKeys.LastDateUpdated, response.LastDateUpdated);
            }
        }
    }
}
