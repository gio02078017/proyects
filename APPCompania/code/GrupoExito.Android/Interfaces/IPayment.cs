namespace GrupoExito.Android.Adapters
{
    using GrupoExito.Entities.Entiites;

    public interface IPayment
    {
        void OnCardSelected(CreditCard myCard);
        void OnCardSelectedTypeOnDeliver(int PaymentType);
    }   
}