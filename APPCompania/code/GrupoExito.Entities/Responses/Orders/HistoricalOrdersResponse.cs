namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class HistoricalOrdersResponse : ResponseBase
    {
        public HistoricalOrdersResponse()
        {
            this.Orders = new List<Order>();
        }

        [JsonProperty("orders")] 
        public IList<Order> Orders { get; set; }
    }
}
