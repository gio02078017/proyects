namespace GrupoExito.DataAgent.Services.Products
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class CategoriesService : BaseApiService, ICategoriesService
    {
        public CategoriesService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
        }

        public async Task<CategoriesResponse> GetCategories()
        {
            string endpoint = AppServiceConfiguration.CategoriesEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new CategoriesResponse());
                return (CategoriesResponse)Convert.ChangeType(responseBase, typeof(CategoriesResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new CategoriesResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<CategoriesResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
