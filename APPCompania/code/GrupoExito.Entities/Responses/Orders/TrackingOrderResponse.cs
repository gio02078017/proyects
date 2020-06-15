namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class TrackingOrderResponse : ResponseBase
    {
        public TrackingOrderResponse()
        {
            TrackingOrders = new List<Tracking>();
        }

        [JsonProperty("orderTrackingStates")]
        public IList<Tracking> TrackingOrders { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }
    }
}
