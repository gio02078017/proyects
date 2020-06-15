namespace GrupoExito.DataAgent.Services.Payments
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Payments;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Payments;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class CreditCardService : BaseApiService, ICreditCardService
    {
        public CreditCardService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<CreditCardResponse> GetCreditCards()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.GetCreditCardsEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CreditCardResponse());
                return (CreditCardResponse)Convert.ChangeType(responseBase, typeof(CreditCardResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new CreditCardResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<CreditCardResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> DeleteCreditCard(CreditCard creditCard)
        {
            string endpoint = string.Format(AppServiceConfiguration.DeleteCreditCardEndPoint, creditCard.Bin);

            var response = await HttpClientBaseService.DeleteAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

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

        public async Task<CreditCardResponse> GetCreditCardsPaymentez()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.GetCreditCardsPaymentezEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CreditCardResponse());
                return (CreditCardResponse)Convert.ChangeType(responseBase, typeof(CreditCardResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new CreditCardResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<CreditCardResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> DeleteCreditCardPaymentez(CreditCard creditCard)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.DeleteCreditCardPaymentezEndPoint, creditCard);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

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
}
