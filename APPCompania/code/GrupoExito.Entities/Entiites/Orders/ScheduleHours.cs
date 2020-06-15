namespace GrupoExito.Entities.Entiites
{
    public class ScheduleHours
    {
        public string Shedule { get; set; }
        public bool Active { get; set; }
        public string EndHour { get; set; }
        public string StartHour { get; set; }
        public int Capacity { get; set; }
        public string ShippingCostPlu { get; set; }
        public string ShippingCostValue {get; set;}
        public bool Store { get; set; }
    }
}
