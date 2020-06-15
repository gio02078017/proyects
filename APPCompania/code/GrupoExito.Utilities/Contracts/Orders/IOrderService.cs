namespace GrupoExito.Utilities.Contracts.Orders
{
    using System.Threading.Tasks;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Orders;

    public interface IOrderService
    {
        Task<OrderResponse> GetOrder();
        Task<OrdersResponse> GetOrders(OrderParameters parameters);

        Task<SummaryResponse> GetSummary(Order order);

        Task<ResponseBase> FlushCar(Order order);

        Task<ResponseBase> UpdateOrderNote(UpdateNoteOrderParameters parameters);

        Task<HistoricalOrdersResponse> GetHistoricalOrders(OrderParameters parameters);

        Task<OrderDetailResponse> GetHistoricalOrder(Order order);

        Task<TrackingOrderResponse> GetTrackingOrder(Order orderId);

    }
}
