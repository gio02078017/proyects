using Newtonsoft.Json;

namespace GrupoExito.Entities.Parameters.Orders
{
    public class ScheduleContingencyParameters
    {
        [JsonProperty("QuantityUnits")]
        public int Quantity { get; set; }
    }
}
