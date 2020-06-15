namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using System.Threading.Tasks;

    public class CategoriesModel
    {
        private ICategoriesService _categoriesService { get; set; }

        public CategoriesModel(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<CategoriesResponse> GetCategories()
        {
            return await _categoriesService.GetCategories();
        }
    }
}
