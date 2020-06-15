namespace GrupoExito.Utilities.Contracts.Generic
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.InStoreServices;
    using System.Threading.Tasks;

    public interface ICashDrawerTurnService
    {
        Task<StoreCashDrawerTurnResponse> GetStores();
        Task<DeviceResponse> Device(AppDevice appDevice);
        Task<StatusCashDrawerTurnResponse> StatusCashDrawerTurn(StatusCashDrawerTurn parameters);
        Task<TicketResponse> Ticket(Ticket parameters);
        Task<TicketResponse> CancelTicket(Ticket parameters);
        Task<TicketResponse> StatusTicket(Ticket parameters);
        Task<DeviceResponse> GetMobileId(AppDevice appDevice);
        Task<DeviceResponse> UpdateDevice(AppDevice appDevice);
    }
}
