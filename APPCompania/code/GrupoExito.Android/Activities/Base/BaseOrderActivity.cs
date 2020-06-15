using Android.App;
using Android.Content.PM;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.Entities;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Base
{
    [Activity(Label = "Mis pedidos", ScreenOrientation = ScreenOrientation.Portrait)]
    public class BaseOrderActivity : BaseActivity
    {
        #region Properties

        private OrderModel orderModel;

        public OrderModel OrderModel
        {
            get { return orderModel; }
            set { orderModel = value; }
        }

        #endregion

        public async Task<OrdersResponse> GetOrders(OrderParameters orderParameters)
        {
            OrdersResponse response = null;

            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);
                response = await OrderModel.GetOrders(orderParameters);

                if (response != null)
                {
                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        response = null;

                        RunOnUiThread(() =>
                        {
                            HideProgressDialog();
                            DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText);
                        });
                    }
                    else
                    {
                        if (response.Orders != null && response.Orders.Any())
                        {
                            HideProgressDialog();
                            return response;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseOrderActivity, ConstantMethodName.GetOrders } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return response;
        }

        public async Task<OrderDetailResponse> GetHistoricalOrderDetail(string orderId)
        {
            try
            {
                ShowProgressDialog(AppMessages.Loading, AppMessages.WaitPlease);

                OrderDetailResponse response = await OrderModel.GetHistoricalOrder(new Order { Id = orderId });

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    string message = MessagesHelper.GetMessage(response.Result);
                    response = null;
                    RunOnUiThread(() =>
                    {
                        HideProgressDialog();
                        DefineShowErrorWay(AppMessages.ApplicationName, message, AppMessages.AcceptButtonText);
                    });
                }
                else
                {
                    return response;
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseOrderActivity, ConstantMethodName.GetOrders } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }
            finally
            {
                HideProgressDialog();
            }

            return null;
        }
    }
}