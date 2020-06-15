namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.Products;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using System.Threading.Tasks;

    public class ProductsModel
    {
        private IProductsService _productsService { get; set; }

        public ProductsModel(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<ProductsResponse> GetProducts(SearchProductsParameters parameters)
        {
            return await _productsService.GetProducts(parameters);
        }

        public async Task<ProductSearcherResponse> ProductSearcher(ProductSearcherParameters parameters)
        {
            return await _productsService.ProductSearcher(parameters);
        }

        public async Task<AddProductsResponse> AddProducts(Order order)
        {
            return await _productsService.AddProducts(order);
        }

        public async Task<ProductsPriceResponse> GetProductsPrice(ProductsPriceParameters parameters)
        {
            return await _productsService.GetProductsPrice(parameters);
        }
    }
}
