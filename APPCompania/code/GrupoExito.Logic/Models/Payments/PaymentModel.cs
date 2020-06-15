namespace GrupoExito.Logic.Models.Paymentes
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Payments;
    using GrupoExito.Utilities.Contracts.Payments;
    using System.Threading.Tasks;

    public class PaymentModel
    {
        private IPaymentService _paymentService { get; set; }

        public PaymentModel(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<PaymentResponse> Pay(Order Order)
        {
            return await _paymentService.Pay(Order);
        }
    }
}
