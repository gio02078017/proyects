using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.KitchenRecipesModelTest
{
    public class RecipesModelTest : BaseRecipesModelTest
    {
        [Fact]
        public async Task GetCategoriesSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            IList<RecipeCategory> categories = new List<RecipeCategory>();
            categories.Add(new RecipeCategory() { });

            RecipeCategoryResponse response = new RecipeCategoryResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Categories = categories
            };

            Service.Setup(_ => _.GetCategories()).ReturnsAsync(response);
            var actual = await Model.GetCategories();

            Assert.True(actual.Categories.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetCategoriesFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;

            IList<RecipeCategory> categories = new List<RecipeCategory>();

            RecipeCategoryResponse response = new RecipeCategoryResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Categories = categories
            };

            Service.Setup(_ => _.GetCategories()).ReturnsAsync(response);
            var actual = await Model.GetCategories();

            Service.VerifyAll();
            Assert.False(actual.Categories.Any());
            Assert.True(actual.Result.HasErrors);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetRecipesByCategorySucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            IList<Recipe> kitchenRecipes = new List<Recipe>();
            kitchenRecipes.Add(new Recipe() { });
            string categoryId = "any";
            RecipesResponse response = new RecipesResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Recipes = kitchenRecipes
            };

            Service.Setup(_ => _.GetRecipesByCategory(categoryId)).ReturnsAsync(response);
            var actual = await Model.GetRecipesByCategory(categoryId);

            Assert.True(actual.Recipes.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetRecipesByCategoryFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;

            IList<Recipe> kitchenRecipes = new List<Recipe>();
            string categoryId = "any";
            RecipesResponse response = new RecipesResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Recipes = kitchenRecipes
            };

            Service.Setup(_ => _.GetRecipesByCategory(categoryId)).ReturnsAsync(response);
            var actual = await Model.GetRecipesByCategory(categoryId);

            Service.VerifyAll();
            Assert.False(actual.Recipes.Any());
            Assert.True(actual.Result.HasErrors);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetRecipeSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            string id = "any";
            List<RecipeDetail> list = new List<RecipeDetail>();
            list.Add(new RecipeDetail { Title = "Any" });
            RecipeResponse response = new RecipeResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Recipe = list
            };

            Service.Setup(_ => _.GetRecipe(id)).ReturnsAsync(response);
            var actual = await Model.GetRecipe(id);

            Assert.True(actual.Recipe.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetRecipeFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;
            string id = "any";
            List<RecipeDetail> list = new List<RecipeDetail>();
            RecipeResponse response = new RecipeResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Recipe = list
            };

            Service.Setup(_ => _.GetRecipe(id)).ReturnsAsync(response);
            var actual = await Model.GetRecipe(id);

            Service.VerifyAll();
            Assert.False(actual.Recipe.Any());
            Assert.True(actual.Result.HasErrors);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }
    }
}
