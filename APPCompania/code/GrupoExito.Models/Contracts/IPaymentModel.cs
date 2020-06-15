using System;
using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Payments;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.Contracts
{
    public interface IPaymentModel : IErrorHandler
    {
        void ThereIsNotActiveReservation();
        void PaymentFinished(PaymentResponse response);
        void CheckoutSummaryFetched(PaymentSummaryResponse summaryResponse);
        void OrderNotValid();
        void ProductNews(PaymentSummaryResponse summaryResponse);
    }
}
