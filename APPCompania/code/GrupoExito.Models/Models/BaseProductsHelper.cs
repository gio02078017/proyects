using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities;
using GrupoExito.Entities.Parameters.Products;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Contracts.Generic;

namespace GrupoExito.Models.Models
{
    public class BaseProductsHelper : BaseHelper
    {
        protected IDeviceManager deviceManager;
        protected readonly ProductsModel productsModel;
        protected UserContext userContext;

        public BaseProductsHelper(IDeviceManager deviceManager, UserContext userContext)
        {
            this.deviceManager = deviceManager;
            this.userContext = userContext;
            this.productsModel = new ProductsModel(new ProductsService(deviceManager));
        }

        public async Task<bool> UploadOrder(Order order)
        {
            bool result = false;
            var response = await productsModel.AddProducts(order);

            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                throw CreateNewException(response.Result);
            }
            else
            {
                result = true;
            }

            return result;
        }

        public async Task<List<Price>> GetPrices(List<Product> products, string dependency)
        {
            List<Price> prices = new List<Price>();
            if (products.Any())
            {
                ProductsPriceParameters parameters = new ProductsPriceParameters()
                {
                    DependencyId = dependency,
                    SkuIds = products.Select(x => x.SkuId).ToList(),
                    ProductsType = products.Select(x => x.ProductType).ToList(),
                    Pums = products.Select(x => x.UnityPum).ToList(),
                    Factors = products.Select(x => x.FactorPum).ToList(),
                };
                ProductsPriceResponse response = await productsModel.GetProductsPrice(parameters);
                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    //throw CreateNewException(response.Result);
                }
                else
                {
                    prices = new List<Price>(response.Prices);
                }
            }
            return prices;
        }
    }
}
