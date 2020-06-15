namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using System.Threading.Tasks;

    public class HomeModel
    {
        private IHomeService _homeService { get; set; }

        public HomeModel(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<DiscountProductsResponse> ProductsAppDiscounts(SearchProductsParameters parameters)
        {
            return await _homeService.ProductsAppDiscounts(parameters);
        }

        public async Task<CustomerProductsResponse> CustomerProducts(SearchProductsParameters parameters)
        {
            return await _homeService.CustomerProducts(parameters);
        }
    }
}
