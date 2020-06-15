namespace GrupoExito.Android.Adapters
{
    using GrupoExito.Entities;

    public interface IOrders
    {
        void OnHistoricalOrderClicked(Order order);

        void OnCurrentOrderClicked(Order order);

    }   
}