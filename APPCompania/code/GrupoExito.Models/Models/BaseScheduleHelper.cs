using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Parameters.Orders;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.Models
{
    public class BaseScheduleHelper : BaseHelper
    {
        private OrderScheduleModel orderScheduleModel;

        public BaseScheduleHelper(IDeviceManager deviceManager)
        {
            this.orderScheduleModel = new OrderScheduleModel(new OrderScheduleService(deviceManager));
        }

        internal async Task<OrderScheduleResponse> GetSchedule(OrderScheduleParameters parameters)
        {
            OrderScheduleResponse response = await orderScheduleModel.GetOrderSchedule(parameters);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<ValidateScheduleReservationResponse> ValidateScheduleReservation(ScheduleReservationParameters parameters)
        {
            ValidateScheduleReservationResponse response = await orderScheduleModel.ValidateScheduleReservation(parameters);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<ScheduleReservationResponse> ScheduleReservation(ScheduleReservationParameters parameters)
        {
            ScheduleReservationResponse response = await orderScheduleModel.ScheduleReservation(parameters);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }

        internal async Task<ScheduleContingencyResponse> GetScheduleContingency(ScheduleContingencyParameters parameters)
        {
            var response = await orderScheduleModel.GetScheduleContingency(parameters);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }

            return response;
        }
    }
}
