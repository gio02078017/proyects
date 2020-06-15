namespace GrupoExito.DataAgent.Services.Products
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.InStoreServices;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class CheckerPriceService : BaseApiService, ICheckerPriceService
    {
        public CheckerPriceService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<CheckerPriceResponse> CheckerPrice(CheckerPriceParameters parameters)
        {
            string endpoint = AppServiceConfiguration.CheckerPriceEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CheckerPriceResponse());
                return (CheckerPriceResponse)Convert.ChangeType(responseBase, typeof(CheckerPriceResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new CheckerPriceResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<CheckerPriceResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<CitiesResponse> GetCities()
        {
            string endpoint = AppServiceConfiguration.CheckerCitiesEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CitiesResponse());
                return (CitiesResponse)Convert.ChangeType(responseBase, typeof(CitiesResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new CitiesResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<CitiesResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<StoreResponse> GetStores(string cityId)
        {
            string endpoint = string.Format(AppServiceConfiguration.CheckerDependenciesEndPoint, cityId);
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new StoreResponse());
                return (StoreResponse)Convert.ChangeType(responseBase, typeof(StoreResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new StoreResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<StoreResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<CheckerPriceCoverageResponse> CheckerPriceCoverage(CheckerPriceCoverageParameters parameters)
        {
            string endpoint = AppServiceConfiguration.CheckerPriceCoverageEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CheckerPriceCoverageResponse());
                return (CheckerPriceCoverageResponse)Convert.ChangeType(responseBase, typeof(CheckerPriceCoverageResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new CheckerPriceCoverageResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<CheckerPriceCoverageResponse>(data);
            result.HttpResponse = response;
            return result;
        }
    }
}
