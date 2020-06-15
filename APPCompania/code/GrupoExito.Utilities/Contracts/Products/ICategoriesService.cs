namespace GrupoExito.Utilities.Contracts.Products
{
    using GrupoExito.Entities.Responses.Products;
    using System.Threading.Tasks;

    public interface ICategoriesService
    {
        Task<CategoriesResponse> GetCategories();
    }
}
