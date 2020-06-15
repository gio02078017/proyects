namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Responses.Base;

    public class StatusCashDrawerTurnResponse : ResponseBase
    {
        public int WaitingTickets { get; set; }
        public decimal AvgWaitTime { get; set; }
        public decimal AvgWaitServ { get; set; }
    }
}
