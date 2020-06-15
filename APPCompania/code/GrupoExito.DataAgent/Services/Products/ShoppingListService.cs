namespace GrupoExito.DataAgent.Services.Products
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class ShoppingListService : BaseApiService, IShoppingListService
    {
        public ShoppingListService(IDeviceManager deviceManager)
            : base(deviceManager)
        {
        }

        public async Task<SuggestedProductsResponse> GetSuggestedProducts(ProductParameters parameters)
        {
            string endpoint = AppServiceConfiguration.SuggestedProductsEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new SuggestedProductsResponse());
                return (SuggestedProductsResponse)Convert.ChangeType(responseBase, typeof(SuggestedProductsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new SuggestedProductsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<SuggestedProductsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ShoppingListsResponse> GetShoppingLists()
        {
            string endpoint = AppServiceConfiguration.ShoppingListsEndPoint;
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ShoppingListsResponse());
                return (ShoppingListsResponse)Convert.ChangeType(responseBase, typeof(ShoppingListsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ShoppingListsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ShoppingListsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ShoppingListsResponse> AddShoppingList(ShoppingList shoppingList)
        {
            string endpoint = AppServiceConfiguration.AddShoppingListEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, shoppingList);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ShoppingListsResponse());
                return (ShoppingListsResponse)Convert.ChangeType(responseBase, typeof(ShoppingListsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ShoppingListsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ShoppingListsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> UpdateShoppingList(ShoppingList shoppingList)
        {
            string endpoint = AppServiceConfiguration.UpdateShoppingListEndPoint;
            var response = await HttpClientBaseService.PutAsync(endpoint, shoppingList);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> DeleteShoppingList(string shoppingListId)
        {
            string endpoint = string.Format(AppServiceConfiguration.DeleteShoppingListEndPoint, shoppingListId);
            var response = await HttpClientBaseService.DeleteAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> AddProductShoppingList(Product product)
        {
            string endpoint = AppServiceConfiguration.AddProductShoppingListEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, product);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> DeleteProductShoppingList(string productId, string shoppingListId)
        {
            string endpoint = string.Format(AppServiceConfiguration.DeleteProductShoppingListEndPoint, shoppingListId, productId);
            var response = await HttpClientBaseService.DeleteAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<SuggestedProductsResponse> GetProductsShoppingList(string shoppingListId)
        {
            string endpoint = string.Format(AppServiceConfiguration.GetProductsShoppingListEndPoint, shoppingListId);
            var response = await HttpClientBaseService.GetAsync(endpoint);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new SuggestedProductsResponse());
                return (SuggestedProductsResponse)Convert.ChangeType(responseBase, typeof(SuggestedProductsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new SuggestedProductsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<SuggestedProductsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ResponseBase> UpdateQuantityItemList(ShoppingList shoppingList)
        {
            string endpoint = AppServiceConfiguration.UpdateQuantityItemListEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, shoppingList);

            String json = JsonService.Serialize<ShoppingList>(shoppingList);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ResponseBase());
                return (ResponseBase)Convert.ChangeType(responseBase, typeof(ResponseBase));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ResponseBase
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ResponseBase>(data);
                result.HttpResponse = response;
                return result;
            }
        }

    }
}
