using System;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.Base;
using GrupoExito.Entities.Containers;
using GrupoExito.Entities.Responses.Recipes;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Recipes;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.DataAgent.Services.Recipes
{
    public class RecipesService : BaseApiService, IRecipesService
    {
        public RecipesService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<RecipeCategoryResponse> GetCategories()
        {
            string endpoint = Utilities.Resources.AppServiceConfiguration.CategoryOfRecipesEndPoint;

            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new RecipeCategoryResponse());
                return (RecipeCategoryResponse)Convert.ChangeType(responseBase, typeof(RecipeCategoryResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new RecipeCategoryResponse()
                    {
                        Result = new MessagesContainer() { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<RecipeCategoryResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<RecipesResponse> GetRecipesByCategory(string id)
        {
            string endpoint = string.Format(Utilities.Resources.AppServiceConfiguration.RecipiesByCategoryEndPoint, id);

            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new RecipesResponse());
                return (RecipesResponse)Convert.ChangeType(responseBase, typeof(RecipesResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new RecipesResponse()
                    {
                        Result = new MessagesContainer() { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<RecipesResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<RecipeResponse> GetRecipe(string id)
        {
            string endpoint = string.Format(Utilities.Resources.AppServiceConfiguration.RecipeEndPoint, id);

            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new RecipeResponse());
                return (RecipeResponse)Convert.ChangeType(responseBase, typeof(RecipeResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new RecipeResponse()
                    {
                        Result = new MessagesContainer() { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<RecipeResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
