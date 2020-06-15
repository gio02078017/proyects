namespace GrupoExito.DataAgent.Services.InStoreServices
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.InStoreServices;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class CashDrawerTurnService : BaseApiService, ICashDrawerTurnService
    {
        public CashDrawerTurnService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<StoreCashDrawerTurnResponse> GetStores()
        {
            var response = await HttpClientBaseService.GetAsync(AppServiceConfiguration.StoresCashDrawerTurnEndPoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new StoreCashDrawerTurnResponse());
                return (StoreCashDrawerTurnResponse)Convert.ChangeType(responseBase, typeof(StoreCashDrawerTurnResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new StoreCashDrawerTurnResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<StoreCashDrawerTurnResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<DeviceResponse> Device(AppDevice appDevice)
        {
            var response = await HttpClientBaseService.PostAsync(AppServiceConfiguration.DeviceCashDrawerTurnEndPoint, appDevice);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DeviceResponse());
                return (DeviceResponse)Convert.ChangeType(responseBase, typeof(DeviceResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DeviceResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DeviceResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        public async Task<DeviceResponse> UpdateDevice(AppDevice appDevice)
        {
            var response = await HttpClientBaseService.PutAsync(AppServiceConfiguration.UpdateDeviceCashDrawerTurnEndPoint, appDevice);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DeviceResponse());
                return (DeviceResponse)Convert.ChangeType(responseBase, typeof(DeviceResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DeviceResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DeviceResponse>(data);
            result.HttpResponse = response;
            return result;
        }

        /// Consulta los turnos que van primero antes de pedir el turno.
        public async Task<StatusCashDrawerTurnResponse> StatusCashDrawerTurn(StatusCashDrawerTurn parameters)
        {
            string endpoint = AppServiceConfiguration.StatusCashDrawerTurnEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new StatusCashDrawerTurnResponse());
                return (StatusCashDrawerTurnResponse)Convert.ChangeType(responseBase, typeof(StatusCashDrawerTurnResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new StatusCashDrawerTurnResponse()
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<StatusCashDrawerTurnResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<TicketResponse> Ticket(Ticket parameters)
        {
            string endpoint = AppServiceConfiguration.TicketEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new TicketResponse());
                return (TicketResponse)Convert.ChangeType(responseBase, typeof(TicketResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new TicketResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<TicketResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<TicketResponse> CancelTicket(Ticket parameters)
        {
            string endpoint = AppServiceConfiguration.CancelTicketEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new TicketResponse());
                return (TicketResponse)Convert.ChangeType(responseBase, typeof(TicketResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new TicketResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<TicketResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        /// Consulta el estado del ticket
        public async Task<TicketResponse> StatusTicket(Ticket parameters)
        {
            string endpoint = AppServiceConfiguration.StatusTicketEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new TicketResponse());
                return (TicketResponse)Convert.ChangeType(responseBase, typeof(TicketResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new TicketResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<TicketResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        ///Retorna el mobile Id si ya esta registrado.
        public async Task<DeviceResponse> GetMobileId(AppDevice appDevice)
        {
            string endpoint = AppServiceConfiguration.GetDeviceCashDrawerTurnEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, appDevice);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new DeviceResponse());
                return (DeviceResponse)Convert.ChangeType(responseBase, typeof(DeviceResponse));
            }

            string data = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(data))
            {
                return new DeviceResponse
                {
                    Result = new MessagesContainer { HasErrors = false },
                    HttpResponse = GetNoContentResponse()
                };
            }

            var result = JsonService.Deserialize<DeviceResponse>(data);
            result.HttpResponse = response;
            return result;
        }
    }
}
