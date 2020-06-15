using System;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Payments;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.Models
{
    public class BaseCreditCardHelper : BaseHelper
    {
        private CreditCardModel creditCardModel;

        public BaseCreditCardHelper(IDeviceManager deviceManager)
        {
            this.creditCardModel = new CreditCardModel(new CreditCardService(deviceManager));
        }

        internal async Task<ResponseBase> DeleteCreditCard(CreditCard creditCard)
        {
            ResponseBase response = await creditCardModel.DeleteCreditCard(creditCard);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<CreditCardResponse> GetCreditCards()
        {
            CreditCardResponse response = await creditCardModel.GetAllCreditCards();
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<CreditCardResponse> GetAllPaymentMethods()
        {
            CreditCardResponse response = await creditCardModel.GetAllPaymentMethods();
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<ResponseBase> DeletePaymentezCreditCard(CreditCard creditCard)
        {
            ResponseBase response = await creditCardModel.DeleteCreditCardPaymentez(creditCard);
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }
    }
}
