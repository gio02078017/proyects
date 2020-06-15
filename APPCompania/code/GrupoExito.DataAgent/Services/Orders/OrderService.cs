namespace GrupoExito.DataAgent.Services.Orders
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Orders;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Orders;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class OrderService : BaseApiService, IOrderService
    {
        public OrderService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        #region Orders

        public async Task<OrderResponse> GetOrder()
        {
            string endpoint = AppServiceConfiguration.GetActiveOrderEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                return new OrderResponse()
                {
                    Result = GetDefaultError(),
                    HttpResponse = GetBadRequestResponse()
                };
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new OrderResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<OrderResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<OrdersResponse> GetOrders(OrderParameters parameters)
        {
            string endpoint = AppServiceConfiguration.GetOrdersEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new OrdersResponse());
                return (OrdersResponse)Convert.ChangeType(responseBase, typeof(OrdersResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new OrdersResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<OrdersResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        #endregion

        #region Historical orders

        public async Task<HistoricalOrdersResponse> GetHistoricalOrders(OrderParameters parameters)
        {
            string endpoint = AppServiceConfiguration.OrdersHistoricalEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new HistoricalOrdersResponse());
                return (HistoricalOrdersResponse)Convert.ChangeType(responseBase, typeof(HistoricalOrdersResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new HistoricalOrdersResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<HistoricalOrdersResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<OrderDetailResponse> GetHistoricalOrder(Order order)
        {
            string endpoint = AppServiceConfiguration.GetOrderHistoricalEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new OrderDetailResponse());
                return (OrderDetailResponse)Convert.ChangeType(responseBase, typeof(OrderDetailResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new OrderDetailResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<OrderDetailResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        #endregion

        #region Summary

        public async Task<SummaryResponse> GetSummary(Order order)
        {
            string endpoint = AppServiceConfiguration.SummaryEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new SummaryResponse());
                return (SummaryResponse)Convert.ChangeType(responseBase, typeof(SummaryResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new SummaryResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<SummaryResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> FlushCar(Order order)
        {
            string endpoint = AppServiceConfiguration.FlushCarEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        #endregion

        #region Products Order

        public async Task<ResponseBase> UpdateOrderNote(UpdateNoteOrderParameters parameters)
        {
            string endpoint = AppServiceConfiguration.UpdateOrderNoteEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        #endregion

        #region Tracking Order

        public async Task<TrackingOrderResponse> GetTrackingOrder(Order order)
        {
            string endpoint = AppServiceConfiguration.GetOrderTrackingEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new TrackingOrderResponse());
                return (TrackingOrderResponse)Convert.ChangeType(responseBase, typeof(TrackingOrderResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new TrackingOrderResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<TrackingOrderResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        #endregion
    }
}
