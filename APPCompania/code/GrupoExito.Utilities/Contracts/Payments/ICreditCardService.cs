namespace GrupoExito.Utilities.Contracts.Payments
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Payments;
    using System.Threading.Tasks;

    public interface ICreditCardService
    {
        Task<CreditCardResponse> GetCreditCards();
        Task<ResponseBase> DeleteCreditCard(CreditCard creditCard);

        Task<CreditCardResponse> GetCreditCardsPaymentez();
        Task<ResponseBase> DeleteCreditCardPaymentez(CreditCard creditCard);
    }
}
