namespace GrupoExito.Logic.Models.Orders
{
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.Orders;
    using GrupoExito.Entities.Responses.Orders;
    using GrupoExito.Utilities.Contracts.Orders;
    using System.Threading.Tasks;

    public class OrderScheduleModel
    {
        IOrderScheduleService OrderScheduleService { get; set; }

        public OrderScheduleModel(IOrderScheduleService orderScheduleService)
        {
            this.OrderScheduleService = orderScheduleService;
        }

        public async Task<OrderScheduleResponse> GetOrderSchedule(OrderScheduleParameters parameters)
        {
            return await this.OrderScheduleService.GetOrderSchedule(parameters);
        }

        public async Task<ScheduleReservationResponse> ScheduleReservation(ScheduleReservationParameters parameters)
        {
            return await this.OrderScheduleService.ScheduleReservation(parameters);
        }

        public async Task<ValidateScheduleReservationResponse> ValidateScheduleReservation(ScheduleReservationParameters parameters)
        {
            return await this.OrderScheduleService.ValidateScheduleReservation(parameters);
        }

        public async Task<ScheduleContingencyResponse> GetScheduleContingency(ScheduleContingencyParameters parameters)
        {
            return await this.OrderScheduleService.GetScheduleContingency(parameters);
        }
    }
}
