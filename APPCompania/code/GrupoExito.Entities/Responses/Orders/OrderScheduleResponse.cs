namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class OrderScheduleResponse : ResponseBase
    {
        public OrderScheduleResponse()
        {
            this.Schedules = new List<ScheduleDays>();
        }

        public string PricePromise { get; set; }

        [JsonProperty("daysSchedules")]
        public IList<ScheduleDays> Schedules { get; set; }

        public bool IsExpress { get; set; }

        public int MinutesPromiseDelivery { get; set; }

        public string ShippingCostPlu { get; set; }
    }
}
