using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.Referentials
{
    public class BaseOrderController : UIViewControllerBase
    {
        #region Constructors
        public BaseOrderController(IntPtr handle) : base(handle){}
        #endregion

        #region Methods Async

        public async Task<OrdersResponse> GetOrders(OrderParameters orderParameters)
        {
            List<Order> orders = new List<Order>();
            OrdersResponse response = null;
            try
            {
                StartActivityIndicatorCustom();
                response = await _orderModel.GetOrders(orderParameters);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        InvokeOnMainThread(() =>
                        {
                            StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                        });
                    }
                }
                else
                {
                    if (response.Orders != null && response.Orders.Any())
                    {
                        StopActivityIndicatorCustom();
                        return response;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseOrderController, ConstantMethodName.GetOrder);
                ShowMessageException(exception);
            }
            return response;
        }

        public async Task<Order> SetOrder()
        {
            string orderId = string.Empty;
            Order order = null;

            if (ParametersManager.Order == null)
            {
                orderId = await GetOrder();
                order = !string.IsNullOrEmpty(orderId) ? new Order() { Id = orderId, DependencyId = ParametersManager.UserContext.DependencyId } :
                order = ParametersManager.Order;
            }
            else
            {
                order = ParametersManager.Order;
            }

            return order;
        }

        public async Task<string> GetOrder()
        {
            try
            {
                _orderModel = new OrderModel(new OrderService(DeviceManager.Instance));

                OrderResponse response = await _orderModel.GetOrder();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }
                else
                {
                    return response.ActiveOrderId;
                }

                return string.Empty;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseOrderController, ConstantMethodName.GetOrder);
                ShowMessageException(exception);
                return string.Empty;
            }
        }

        public async Task<OrderDetailResponse> GetHistoricalOrderDetail(string orderId)
        {
            try
            {
                StartActivityIndicatorCustom();
                OrderDetailResponse response = await _orderModel.GetHistoricalOrder(new Order { Id = orderId });

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    InvokeOnMainThread(() =>
                    {
                        StopActivityIndicatorCustom();
                        AlertViewController(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    return response;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseOrderController, ConstantMethodName.GetOrder);
                ShowMessageException(exception);
            }
            finally
            {
                StopActivityIndicatorCustom();
            }

            return null;
        }
        #endregion

        #region Methods

        public void SetOrder(string orderId, decimal total, int totalProduct)
        {
            Order order = ParametersManager.Order ?? new Order()
            {
                DependencyId = ParametersManager.UserContext.DependencyId,
                Id = orderId
            };
            order.Total = total;
            order.TotalProducts = totalProduct;
            order.DependencyId = ParametersManager.UserContext.DependencyId;

            if (!string.IsNullOrEmpty(orderId))
            {
                DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }

            NavigationView.UpdateCar(StringFormat.ToPrice(order.Total), StringFormat.ToQuantity(order.TotalProducts));
        }

        #endregion
    }
}