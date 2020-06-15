namespace GrupoExito.Utilities.Contracts.Recipes
{
    using GrupoExito.Entities.Responses.Recipes;
    using System.Threading.Tasks;

    public interface IRecipesService
    {
        Task<RecipeCategoryResponse> GetCategories();

        Task<RecipesResponse> GetRecipesByCategory(string id);

        Task<RecipeResponse> GetRecipe(string id);
    }
}
