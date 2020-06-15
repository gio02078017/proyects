namespace GrupoExito.Logic.Models.InStoreServices
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Resources;
    using System.Threading.Tasks;

    public class CashDrawerTurnModel
    {
        private ICashDrawerTurnService _cashDrawerTurnService { get; set; }

        public CashDrawerTurnModel(ICashDrawerTurnService cashDrawerTurnService)
        {
            this._cashDrawerTurnService = cashDrawerTurnService;
        }

        public async Task<StoreCashDrawerTurnResponse> GetStores()
        {
            var response = await _cashDrawerTurnService.GetStores();

            if (response != null)
            {
                response.Stores.Insert(0, new StoreCashDrawerTurn { Id = "0", Name = AppMessages.Choose });
            }

            return response;
        }

        public async Task<DeviceResponse> UpdateDevice(AppDevice appDevice)
        {
            return await _cashDrawerTurnService.UpdateDevice(appDevice);
        }

        public async Task<DeviceResponse> Device(AppDevice appDevice)
        {
            return await _cashDrawerTurnService.Device(appDevice);
        }

        public async Task<StatusCashDrawerTurnResponse> StatusCashDrawerTurn(StatusCashDrawerTurn parameters)
        {
            return await _cashDrawerTurnService.StatusCashDrawerTurn(parameters);
        }

        public async Task<TicketResponse> Ticket(Ticket parameters)
        {
            return await _cashDrawerTurnService.Ticket(parameters);
        }

        public async Task<TicketResponse> CancelTicket(Ticket parameters)
        {
            return await _cashDrawerTurnService.CancelTicket(parameters);
        }

        public async Task<TicketResponse> StatusTicket(Ticket parameters)
        {
            return await _cashDrawerTurnService.StatusTicket(parameters);
        }

        public async Task<DeviceResponse> GetMobileId(AppDevice appDevice)
        {
            return await _cashDrawerTurnService.GetMobileId(appDevice);
        }
    }
}
