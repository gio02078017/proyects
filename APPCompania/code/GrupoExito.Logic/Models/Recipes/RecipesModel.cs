namespace GrupoExito.Logic.Models.Recipes
{
    using GrupoExito.Entities.Responses.Recipes;
    using GrupoExito.Utilities.Contracts.Recipes;
    using System.Linq;
    using System.Threading.Tasks;

    public class RecipesModel
    {
        private IRecipesService _kitchenRecipesService;

        public RecipesModel(IRecipesService kitchenRecipesService)
        {
            _kitchenRecipesService = kitchenRecipesService;
        }

        public async Task<RecipeCategoryResponse> GetCategories()
        {
            return await _kitchenRecipesService.GetCategories();
        }

        public async Task<RecipesResponse> GetRecipesByCategory(string id)
        {
            return await _kitchenRecipesService.GetRecipesByCategory(id);
        }

        public async Task<RecipeResponse> GetRecipe(string id)
        {
            var response = await _kitchenRecipesService.GetRecipe(id);

            if (response != null && response.Recipe != null && response.Recipe.Any())
            {
                var preparationSteps = response.Recipe.First().Preparation.Split('|');
                response.Recipe.First().PreparationSteps = preparationSteps?.ToList();

                var ingredients = response.Recipe.First().Ingredient.Split('|');
                response.Recipe.First().Ingredients = ingredients?.ToList();
            }

            return response;
        }
    }
}
