namespace GrupoExito.Logic.Models.Payments
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Payments;
    using GrupoExito.Utilities.Contracts.Payments;
    using System.Threading.Tasks;

    public class PaymentSummaryModel
    {
        private IPaymentSummaryService _paymentSummaryService { get; set; }

        public PaymentSummaryModel(IPaymentSummaryService paymentSummaryService)
        {
            _paymentSummaryService = paymentSummaryService;
        }

        public async Task<PaymentSummaryResponse> Summary(Order Order)
        {
            return await _paymentSummaryService.Summary(Order);
        }
    }
}
