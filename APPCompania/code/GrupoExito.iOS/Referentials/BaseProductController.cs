using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters.Products;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.OrderScheduleControllers;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.Referentials
{
    public class BaseProductController : UIViewControllerBase
    {
        #region Attributes
        protected ProductsModel _productsModel;
        #endregion

        #region Constructors
        public BaseProductController(IntPtr handle) : base(handle)
        {
            _productsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        public async Task GetOrder(Product selectedProduct)
        {
            try
            {
                OrderResponse response = await _orderModel.GetOrder();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        if (!string.IsNullOrEmpty(message))
                        {
                            InvokeOnMainThread(() =>
                            {
                                StartActivityErrorMessage(response.Result.Messages.First().Code, message);
                            });
                        }
                    }
                }
                else
                {
                    var order = new Order() { DependencyId = ParametersManager.UserContext.Address.DependencyId, Id = response.ActiveOrderId };
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
                    //await AddProduct(selectedProduct);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseProductController, ConstantMethodName.GetOrder);
                ShowMessageException(exception);
            }
        }

        public async Task<string> GetOrderId()
        {
            string orderId = await GetOrder();

            if (ParametersManager.Order != null)
            {
                if (!orderId.Equals(ParametersManager.Order.Id))
                {
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(
                        new Order()
                        {
                            Id = orderId,
                            DependencyId = ParametersManager.UserContext.DependencyId
                        }));
                }
            }
            return orderId;
        }

        public async Task<string> GetOrder()
        {
            try
            {
                OrderResponse response = await _orderModel.GetOrder();

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null
                    && response.Result.Messages.Any())
                {
                    string message = MessagesHelper.GetMessage(response.Result);
                    if (!string.IsNullOrEmpty(message))
                    {
                        InvokeOnMainThread(() =>
                        {
                            StartActivityErrorMessage(response.Result.Messages.First().Code, message);
                        });
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
                Util.LogException(exception, ConstantControllersName.BaseProductController, ConstantMethodName.GetOrder);
                ShowMessageException(exception);
                return string.Empty;
            }
        }

        public async Task<Order> SetOrder()
        {
            string orderId = string.Empty;
            Order order = null;
            if (ParametersManager.Order == null)
            {
                orderId = await GetOrder();
                order = !string.IsNullOrEmpty(orderId) ? 
                        new Order() 
                            { 
                                Id = orderId, DependencyId = ParametersManager.UserContext.DependencyId 
                            } : order = ParametersManager.Order;
            }
            else
            {
                order = ParametersManager.Order;
            }
            return order;
        }
        #endregion

        #region Methods Dependency Change
        protected void GetProductsPrice()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                InvokeOnMainThread(async () =>
                {
                    await this.GetProductsPriceAsync();
                });
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private async Task GetProductsPriceAsync()
        {
            try
            {
                List<Product> products = DataBase.GetProducts();
                if (products.Any())
                {
                    ProductsPriceParameters parameters = new ProductsPriceParameters()
                    {
                        DependencyId = ParametersManager.UserContext.DependencyId,
                        SkuIds = products.Select(x => x.SkuId).ToList(),
                        ProductsType = products.Select(x => x.ProductType).ToList(),
                        Pums = products.Select(x => x.UnityPum).ToList(),
                        Factors = products.Select(x => x.FactorPum).ToList(),
                    };
                    ProductsPriceResponse response = await _productsModel.GetProductsPrice(parameters);
                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {

                    }
                    else
                    {
                        List<Price> productsPrice = new List<Price>(response.Prices);
                        if (productsPrice.Any())
                        {
                            List<Product> productsDeleted = DataBase.UpdateProductsPrice(productsPrice, products);
                            UpdateCar(true);
                            if (productsDeleted.Any())
                            {
                                ChangedOrderViewController changedOrderViewController = (ChangedOrderViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.ChangedOrderViewController);
                                changedOrderViewController.Products = productsDeleted;
                                this.PresentModalViewController(changedOrderViewController, true);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductByCategoryViewController, ConstantMethodName.GetProductsCategories);
                ShowMessageException(exception);
            }
        }
        #endregion
    }
}