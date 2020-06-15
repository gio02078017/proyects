namespace GrupoExito.DataAgent.Services.Orders
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.Orders;
    using GrupoExito.Entities.Responses.Orders;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Orders;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class OrderScheduleService : BaseApiService, IOrderScheduleService
    {
        public OrderScheduleService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<OrderScheduleResponse> GetOrderSchedule(OrderScheduleParameters parameters)
        {
            string endpoint = AppServiceConfiguration.GetOrderSchedulesEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new OrderScheduleResponse());
                return (OrderScheduleResponse)Convert.ChangeType(responseBase, typeof(OrderScheduleResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new OrderScheduleResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                OrderScheduleResponse result = JsonService.Deserialize<OrderScheduleResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ScheduleContingencyResponse> GetScheduleContingency(ScheduleContingencyParameters parameters)
        {
            string endpoint = AppServiceConfiguration.GetScheduleContingencyEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ScheduleContingencyResponse());
                return (ScheduleContingencyResponse)Convert.ChangeType(responseBase, typeof(ScheduleContingencyResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ScheduleContingencyResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                ScheduleContingencyResponse result = JsonService.Deserialize<ScheduleContingencyResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ScheduleReservationResponse> ScheduleReservation(ScheduleReservationParameters parameters)
        {
            string endpoint = AppServiceConfiguration.ScheduleReservationEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ScheduleReservationResponse());
                return (ScheduleReservationResponse)Convert.ChangeType(responseBase, typeof(ScheduleReservationResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ScheduleReservationResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                ScheduleReservationResponse result = JsonService.Deserialize<ScheduleReservationResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ValidateScheduleReservationResponse> ValidateScheduleReservation(ScheduleReservationParameters parameters)
        {
            string endpoint = AppServiceConfiguration.ValidateScheduleReservationEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ValidateScheduleReservationResponse());
                return (ValidateScheduleReservationResponse)Convert.ChangeType(responseBase, typeof(ValidateScheduleReservationResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ValidateScheduleReservationResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                ValidateScheduleReservationResponse result = JsonService.Deserialize<ValidateScheduleReservationResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
