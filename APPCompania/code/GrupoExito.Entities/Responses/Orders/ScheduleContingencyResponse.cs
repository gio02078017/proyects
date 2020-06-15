namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;

    public class ScheduleContingencyResponse : ResponseBase
    {
        public string ShippingCost { get; set; }
        public string TypeShippingGroup { get; set; }
        public string TypeDispatch { get; set; }
        public string PluDispatch { get; set; }
        public string UserSchedule { get; set; }
        public string DateSelected { get; set; }        
        public string HourPromiseDelivery { get; set; }       
    }
}
