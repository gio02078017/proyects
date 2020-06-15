namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class OrderResponse : ProductResponseBase
    {
        public OrderResponse()
        {
            ProductsSummary = new List<Product>();
        }

        [JsonProperty("activeOrderId")]
        public string ActiveOrderId { get; set; }

        [JsonProperty("productsSummary")]
        public IList<Product> ProductsSummary { get; set; }
    }
}
