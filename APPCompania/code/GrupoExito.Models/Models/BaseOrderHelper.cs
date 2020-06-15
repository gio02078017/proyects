using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Containers;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.Models
{
    public class BaseOrderHelper : BaseHelper
    {
        protected readonly OrderModel orderModel;
        protected UserContext userContext;
        protected IDeviceManager deviceManager;

        public BaseOrderHelper(IDeviceManager deviceManager, UserContext userContext)
        {
            this.orderModel = new OrderModel(new OrderService(deviceManager));
            this.userContext = userContext;
            this.deviceManager = deviceManager;
        }

        internal async Task<OrderResponse> GetOrder()
        {
            OrderResponse response = await orderModel.GetOrder();

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else return response;
        }

        internal async Task<Order> SetOrder(List<Product> products)
        {
            Order order = null;
            OrderResponse orderResponse = await GetOrder();

            if (orderResponse.Result != null && orderResponse.Result.HasErrors && orderResponse.Result.Messages != null)
            {
                throw CreateNewException(orderResponse.Result);
            }
            else
            {
                order = new Order()
                {
                    Id = orderResponse.ActiveOrderId,
                    DependencyId = userContext.DependencyId,
                    Products = products,
                    TotalProducts = products.Count()
                };

                deviceManager.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }

            return order;
        }
    }
}
