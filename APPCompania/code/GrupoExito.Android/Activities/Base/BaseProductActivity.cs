using Android.Content;
using Android.OS;
using GrupoExito.Android.Activities.Products;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.Utilities.Constants;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.Addresses;
using GrupoExito.DataAgent.Services.Orders;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Parameters.Products;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Addresses;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoExito.Android.Activities.Base
{
    public abstract class BaseProductActivity : BaseActivity
    {
        #region Properties

        public OrderModel OrderModel { get; set; }

        private ProductsModel _productsModel;
        public ProductsModel ProductsModel
        {
            get { return _productsModel; }
            set { _productsModel = value; }
        }

        private AddressModel addressModel;
        public AddressModel AddressModel
        {
            get { return addressModel; }
            set { addressModel = value; }
        }

        private ProductCarModel productCarModel;
        public ProductCarModel ProductCarModel
        {
            get { return productCarModel; }
            set { productCarModel = value; }
        }

        #endregion

        #region Protected Method

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            OrderModel = new OrderModel(new OrderService(DeviceManager.Instance));
            ProductsModel = new ProductsModel(new ProductsService(DeviceManager.Instance));
            ProductCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            AddressModel = new AddressModel(new AddressService(DeviceManager.Instance));
            ProductCarModel.RecalculateSummary();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (TvToolbarPrice != null)
            {
                SetToolbarCarItems();
            }
        }

        protected virtual void RefreshProductList(Product ActualProductId)
        {
        }

        protected virtual void DeleteSummaryProduct(string productId)
        {
        }

        #endregion

        #region Public Methods

        public override void OnBackPressed()
        {
            ParametersManager.UserQuery = null;
            base.OnBackPressed();
        }

        public async Task<ProductSearcherResponse> ProductSearcher(ProductSearcherParameters parameters)
        {
            ProductSearcherResponse response = null;

            try
            {
                response = await ProductsModel.ProductSearcher(parameters);
            }
            catch (Exception exception)
            {
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseProductActivity, ConstantMethodName.ProductSearcher } };
                ShowAndRegisterMessageExceptions(exception, properties);
            }

            return response;
        }

        public async Task UpdateProductsPrice()
        {
            try
            {
                List<Product> products = ProductCarModel.GetProducts();

                if (products != null && products.Any())
                {
                    ProductsPriceParameters parameters = GetProductsPriceParameters(products);
                    ProductsPriceResponse response = await ProductsModel.GetProductsPrice(parameters);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        DefineShowErrorWay(AppMessages.ApplicationName, MessagesHelper.GetMessage(response.Result), AppMessages.AcceptButtonText);
                    }
                    else
                    {
                        List<Product> productsDeleted = ProductCarModel.UpdateProductsPrice(response.Prices.ToList(), products);

                        ProductCarModel.RecalculateSummary();                        

                        if (productsDeleted != null && productsDeleted.Any())
                        {                            
                            Intent intent = new Intent(this, typeof(ProductsDeleteActivity));
                            intent.PutExtra(ConstantPreference.DeleteProducts, JsonService.Serialize(productsDeleted));
                            StartActivity(intent);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseProductActivity, ConstantMethodName.UpdateProductsPrice } };
                ShowAndRegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        public async Task UpdateProductsPriceInMainActivity()
        {
            try
            {
                List<Product> products = ProductCarModel.GetProducts();

                if (products != null && products.Any())
                {
                    ProductsPriceParameters parameters = GetProductsPriceParameters(products);
                    ProductsPriceResponse response = await ProductsModel.GetProductsPrice(parameters);

                    if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                    {
                        // Not show error
                    }
                    else
                    {
                        List<Product> productsDeleted = ProductCarModel.UpdateProductsPrice(response.Prices.ToList(), products);

                        if (productsDeleted != null && productsDeleted.Any())
                        {
                            ProductCarModel.RecalculateSummary();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                HideProgressDialog();
                Dictionary<string, string> properties = new Dictionary<string, string>() {
                    { ConstantActivityName.BaseProductActivity, ConstantMethodName.UpdateProductsPrice } };
                ShowAndRegisterMessageExceptions(exception, properties);
                DefineShowErrorWay(AppMessages.ApplicationName, AppMessages.UnexpectedErrorMessage, AppMessages.AcceptButtonText);
            }
        }

        private ProductsPriceParameters GetProductsPriceParameters(List<Product> products)
        {
            ProductsPriceParameters parameters = new ProductsPriceParameters()
            {
                DependencyId = ParametersManager.UserContext.DependencyId,
                SkuIds = products.Select(x => x.SkuId).ToList(),
                ProductsType = products.Select(x => x.ProductType).ToList(),
                Pums = products.Select(x => x.UnityPum).ToList(),
                Factors = products.Select(x => x.FactorPum).ToList(),
            };

            return parameters;
        }

        #endregion     
    }
}