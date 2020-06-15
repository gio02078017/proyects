namespace GrupoExito.DataAgent.Services.InStoreServices
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class DiscountsService : BaseApiService, IDiscountsService
    {
        private IDeviceManager _deviceManager;

        public DiscountsService(IDeviceManager deviceManager) :
            base(deviceManager)
        {
            _deviceManager = deviceManager;
        }

        public async Task<DisccountResponse> ActiveDisccount(DiscountParameters parameters)
        {
            parameters.TransactionId = Guid.NewGuid().ToString("D");
            string endpoint = AppServiceConfiguration.ActiveDiscountEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DisccountResponse());
                return (DisccountResponse)Convert.ChangeType(responseBase, typeof(DisccountResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DisccountResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DisccountResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<DisccountResponse> InactiveDisccount(DiscountParameters parameters)
        {
            parameters.TransactionId = Guid.NewGuid().ToString("D");
            string endpoint = AppServiceConfiguration.InactiveDiscountEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DisccountResponse());
                return (DisccountResponse)Convert.ChangeType(responseBase, typeof(DisccountResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DisccountResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DisccountResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<DiscountsResponse> GetDiscounts()
        {
            UserContext user = JsonService.Deserialize<UserContext>(_deviceManager.GetAccessPreference(ConstPreferenceKeys.User));

            DiscountParameters parameters = new DiscountParameters()
            {
                TransactionId = Guid.NewGuid().ToString("D"),
                CellPhone = user != null ? user.CellPhone : string.Empty
            };

            string endpoint = AppServiceConfiguration.DicountsEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DiscountsResponse());
                return (DiscountsResponse)Convert.ChangeType(responseBase, typeof(DiscountsResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DiscountsResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DiscountsResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<ValidateActivatedCouponsResponse> ValidateActivatedCoupons()
        {
            DiscountParameters parameters = new DiscountParameters()
            {
                TransactionId = Guid.NewGuid().ToString("D")
            };

            string endpoint = AppServiceConfiguration.ValidateActivatedCouponsEndPoint;

            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ValidateActivatedCouponsResponse());
                return (ValidateActivatedCouponsResponse)Convert.ChangeType(responseBase, typeof(ValidateActivatedCouponsResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new ValidateActivatedCouponsResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<ValidateActivatedCouponsResponse>(data);
            result.HttpResponse = response;
            return result;
        }
    }
}
