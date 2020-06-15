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

    public class PaymentService : BaseApiService, IPaymentService
    {
        public PaymentService(IDeviceManager deviceManager)
           : base(deviceManager)
        {
        }

        public async Task<PaymentResponse> Pay(Order Order)
        {
            string endpoint = AppServiceConfiguration.OrderProductCheckoutEndPoint;

            var response = await HttpClientBaseService.PostAsync(endpoint, Order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new PaymentResponse());
                return (PaymentResponse)Convert.ChangeType(responseBase, typeof(PaymentResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new PaymentResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<PaymentResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
