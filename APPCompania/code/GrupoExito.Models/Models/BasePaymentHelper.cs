using System;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Logic.Models.Payments;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.Models.Models
{
    public class BasePaymentHelper : BaseHelper
    {
        private PaymentModel paymentModel;
        private PaymentSummaryModel paymentSummaryModel;

        public BasePaymentHelper(IDeviceManager deviceManager)
        {
            this.paymentModel = new PaymentModel(new PaymentService(deviceManager));
            this.paymentSummaryModel = new PaymentSummaryModel(new PaymentSummaryService(deviceManager));
        }

        internal async Task<PaymentSummaryResponse> SummaryCheckout(Order order)
        {
            PaymentSummaryResponse response = await paymentSummaryModel.Summary(order);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<PaymentResponse> Pay(Order order)
        {
            PaymentResponse response = await paymentModel.Pay(order);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewPayException(response.Result);
            }

            return response;
        }

        internal Exception CreateNewPayException(MessagesContainer result)
        {
            Exception ex = new Exception();

            if (result.Messages.Any())
            {
                String message = MessagesHelper.GetMessage(result);

                ex.Data.Add("Message", AppMessages.ApologyPaymentErrorMessage);
                ex.Data.Add("Code", result.Messages[0].Code);
            }

            LogExceptionHelper.LogException(ex);
            return ex;
        }
    }
}
