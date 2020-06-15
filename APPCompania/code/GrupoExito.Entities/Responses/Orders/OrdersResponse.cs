namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class OrdersResponse : ResponseBase
    {
        public OrdersResponse()
        {
            this.Orders = new List<OrderInformation>();
            this.HistoricalOrders = new List<Order>();
        }

        [JsonProperty("orders")] 
        public IList<OrderInformation> Orders { get; set; }

        [JsonProperty("historicalOrders")]
        public IList<Order> HistoricalOrders { get; set; }
    }
}
