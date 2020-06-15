namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using System.Threading.Tasks;

    public class ProductModel
    {
        private IProductsService _productsService { get; set; }

        public ProductModel(IProductsService productsService)
        {
            _productsService = productsService;
        }

        public async Task<ProductResponse> GetProduct(ProductParameters pamameters)
        {
            return await _productsService.GetProduct(pamameters);
        }
    }
}
