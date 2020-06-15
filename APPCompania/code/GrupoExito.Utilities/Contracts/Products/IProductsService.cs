namespace GrupoExito.Utilities.Contracts.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.Products;
    using GrupoExito.Entities.Responses.Products;
    using System.Threading.Tasks;

    public interface IProductsService
    {
        Task<ProductsResponse> GetProducts(SearchProductsParameters parameters);
        Task<ProductResponse> GetProduct(ProductParameters parameters);
        Task<ProductSearcherResponse> ProductSearcher(ProductSearcherParameters parameters);
        Task<AddProductsResponse> AddProducts(Order order);
        Task<ProductsPriceResponse> GetProductsPrice(ProductsPriceParameters parameters);
    }
}
