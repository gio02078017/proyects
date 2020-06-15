namespace GrupoExito.Utilities.Contracts.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Products;
    using System.Threading.Tasks;

    public interface IHomeService
    {
        Task<DiscountProductsResponse> ProductsAppDiscounts(SearchProductsParameters parameters);
        Task<CustomerProductsResponse> CustomerProducts(SearchProductsParameters parameters);
    }
}
