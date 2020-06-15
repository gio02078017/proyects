namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class OrderProductParameters
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("dependencyId")]
        public string DependencyId { get; set; }

        [JsonProperty("commerceItemId")]
        public string CommerceItemId { get; set; }       

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}
