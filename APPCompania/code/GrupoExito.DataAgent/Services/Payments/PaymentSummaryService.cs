namespace GrupoExito.DataAgent.Services.Payments
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Payments;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Payments;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class PaymentSummaryService : BaseApiService, IPaymentSummaryService
    {
        public PaymentSummaryService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
        }

        public async Task<PaymentSummaryResponse> Summary(Order Order)
        {
            string endpoint = AppServiceConfiguration.PaymentSummaryEndPoint;            

            var response = await HttpClientBaseService.PostAsync(endpoint, Order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new PaymentSummaryResponse());
                return (PaymentSummaryResponse)Convert.ChangeType(responseBase, typeof(PaymentSummaryResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new PaymentSummaryResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<PaymentSummaryResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
