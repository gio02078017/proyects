namespace GrupoExito.Utilities.Contracts.Orders
{
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.Orders;
    using GrupoExito.Entities.Responses.Orders;
    using System.Threading.Tasks;

    public interface IOrderScheduleService
    {
        Task<OrderScheduleResponse> GetOrderSchedule(OrderScheduleParameters parameters);
        Task<ScheduleReservationResponse> ScheduleReservation(ScheduleReservationParameters parameters);
        Task<ValidateScheduleReservationResponse> ValidateScheduleReservation(ScheduleReservationParameters parameters);
        Task<ScheduleContingencyResponse> GetScheduleContingency(ScheduleContingencyParameters parameters);
    }
}
