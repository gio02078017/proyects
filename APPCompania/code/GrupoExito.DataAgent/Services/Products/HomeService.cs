namespace GrupoExito.DataAgent.Services.Products
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HomeService : BaseApiService, IHomeService
    {
        private IDeviceManager _deviceManager;

        public HomeService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public async Task<DiscountProductsResponse> ProductsAppDiscounts(SearchProductsParameters parameters)
        {
            string endpoint = AppServiceConfiguration.ProductsAppDiscountsEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DiscountProductsResponse());
                return (DiscountProductsResponse)Convert.ChangeType(responseBase, typeof(DiscountProductsResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DiscountProductsResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DiscountProductsResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<CustomerProductsResponse> CustomerProducts(SearchProductsParameters parameters)
        {
            string endpoint = AppServiceConfiguration.CustomerProductsEndPoint;

            bool nextDateCacheExists = _deviceManager.ValidateAccessPreference(ConstPreferenceKeys.NextDateCache);

            parameters.NextDateCache = nextDateCacheExists ?
                _deviceManager.GetAccessPreference(ConstPreferenceKeys.NextDateCache)
                : parameters.NextDateCache;

            parameters.Plus = nextDateCacheExists ?
              JsonService.Deserialize<List<string>>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.Plus))
               : parameters.Plus;

            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CustomerProductsResponse());
                return (CustomerProductsResponse)Convert.ChangeType(responseBase, typeof(CustomerProductsResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new CustomerProductsResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<CustomerProductsResponse>(data);

            if (response.IsSuccessStatusCode)
            {
                await SaveProductsInPreferences(response);
            }

            result.HttpResponse = response;
            return result;
        }

        private async Task SaveProductsInPreferences(HttpResponseMessage result)
        {
            var response = new CustomerProductsResponse() { HttpResponse = result };
            response = await JsonService.GetSerializedResponse<CustomerProductsResponse>(result);

            if (response != null && response.Plus != null && response.Plus.Any())
            {
                _deviceManager.SaveAccessPreference(ConstPreferenceKeys.NextDateCache, response.NextDateCache);
                _deviceManager.SaveAccessPreference(ConstPreferenceKeys.Plus, JsonService.Serialize(response.Plus));
            }
        }
    }
}
