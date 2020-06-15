namespace GrupoExito.Utilities.Contracts.Payments
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Payments;
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        Task<PaymentResponse> Pay(Order Order);
    }
}
