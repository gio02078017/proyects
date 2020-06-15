namespace GrupoExito.Logic.Models.Orders
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Orders;
    using GrupoExito.Utilities.Contracts.Orders;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderModel
    {
        private IOrderService _orderService { get; set; }

        public OrderModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Orders

        public async Task<OrderResponse> GetOrder()
        {
            return await _orderService.GetOrder();
        }

        public async Task<OrdersResponse> GetOrders(OrderParameters parameters)
        {
            return await _orderService.GetOrders(parameters);
        }

        #endregion

        #region  Historical orders

        public async Task<HistoricalOrdersResponse> GetHistoricalOrders(OrderParameters parameters)
        {
            return await _orderService.GetHistoricalOrders(parameters);
        }

        public async Task<OrderDetailResponse> GetHistoricalOrder(Order order)
        {
            return await _orderService.GetHistoricalOrder(order);
        }

        #endregion

        #region Summary

        public async Task<SummaryResponse> GetSummary(Order order)
        {
            return await _orderService.GetSummary(order);
        }

        public async Task<ResponseBase> FlushCar(Order order)
        {
            return await _orderService.FlushCar(order);
        }

        #endregion

        #region Products Order

        public async Task<ResponseBase> UpdateOrderNote(UpdateNoteOrderParameters parameters)
        {
            return await _orderService.UpdateOrderNote(parameters);
        }

        #endregion

        #region Tracking Order

        public async Task<TrackingOrderResponse> GetTrackingOrder(Order order)
        {
            return await _orderService.GetTrackingOrder(order);
        }

        #endregion
    }
}
