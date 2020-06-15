namespace GrupoExito.DataAgent.Services.Addresses
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Utilities.Contracts.Addresses;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class AddressService : BaseApiService, IAddressService
    {
        public AddressService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<CitiesResponse> GetCities(CitiesFilter parameters)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.GetCitiesEndPoint, parameters);

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

        public async Task<AddressPredictionResponse> AutoCompleteAddress(string text)
        {
            string endpoint = string.Format(AppServiceConfiguration.AutocompleteAddressEndPoint, text, AppConfigurations.ApiKeyGoogle);
            var response = await HttpClientBaseService.GetAddressAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new AddressPredictionResponse());
                return (AddressPredictionResponse)Convert.ChangeType(responseBase, typeof(AddressPredictionResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new AddressPredictionResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<AddressPredictionResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<CoverageAddressResponse> CoverageAddress(LocationAddress location)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.CoverageAddressEndPoint, location);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CoverageAddressResponse());
                return (CoverageAddressResponse)Convert.ChangeType(responseBase, typeof(CoverageAddressResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new CoverageAddressResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<CoverageAddressResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<ResponseBase> AddAddress(UserAddress address)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.AddAddressEndPoint, address);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new ResponseBase
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<AddressResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<AddressResponse> GetAddress()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.AddAddressEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new AddressResponse());
                return (AddressResponse)Convert.ChangeType(responseBase, typeof(AddressResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new AddressResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<AddressResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<ResponseBase> UpdateAddress(UserAddress address)
        {
            var response = await HttpClientBaseService.PutAsync(AppServiceConfiguration.UpdateAddressEndPoint, address);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new ResponseBase
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<ResponseBase>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<ResponseBase> DeleteAddress(UserAddress address)
        {
            string endpoint = string.Format(AppServiceConfiguration.DeleteAddressEndPoint, address.AddressKey);
            var response = await HttpClientBaseService.DeleteAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new ResponseBase
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<ResponseBase>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<StoreResponse> GetStores(StoreParameters parameters)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.StoresByCityEndPoint, parameters);

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

        public async Task<StoreResponse> GetStores(SearchStoresParameters parameters)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.StoreEndPoint, parameters);

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

        public async Task<ResponseBase> SetDefaultAddress(UserAddress address)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.SetDefaultAddressEndPoint, address);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new ResponseBase
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<AddressResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<CorrespondenceRespondese> ValidateCorrespondence()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.ValidateCorrespondenceEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CorrespondenceRespondese());
                return (CorrespondenceRespondese)Convert.ChangeType(responseBase, typeof(CorrespondenceRespondese));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new CorrespondenceRespondese
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<CorrespondenceRespondese>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<CorrespondenceRespondese> SaveCorrespondence(UserAddress address)
        {
            var response = await HttpClientBaseService.PutAsync(AppServiceConfiguration.SaveCorrespondenceEndPoint, address);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CorrespondenceRespondese());
                return (CorrespondenceRespondese)Convert.ChangeType(responseBase, typeof(CorrespondenceRespondese));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new CorrespondenceRespondese
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<CorrespondenceRespondese>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<ResponseBase> UpdateDispatchRegion(UpdateDispatchRegionParameters parameters)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.UpdateDispatchRegionEndPoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new ResponseBase
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<ResponseBase>(data);
            result.HttpResponse = response;
            return result;
        }
    }
}
