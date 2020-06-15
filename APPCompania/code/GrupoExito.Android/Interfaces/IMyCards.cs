namespace GrupoExito.Android.Adapters
{
    using GrupoExito.Entities.Entiites;

    public interface IMyCards
    {
        void OnDeleteMyCardsClicked(CreditCard myCard);
        void OnCardSelected(CreditCard myCard);
    }   
}