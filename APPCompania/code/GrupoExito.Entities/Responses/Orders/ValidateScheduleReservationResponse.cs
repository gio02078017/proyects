namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;

    public class ValidateScheduleReservationResponse : ResponseBase
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public string ChangeStatus { get; set; }
        public bool ActiveReservation { get; set; }
    }
}
