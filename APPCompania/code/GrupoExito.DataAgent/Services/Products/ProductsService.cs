namespace GrupoExito.DataAgent.Services.Products
{
    using GrupoExito.DataAgent.Services.Base;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Containers;
    using GrupoExito.Entities.Parameters;
    using GrupoExito.Entities.Parameters.Products;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Generic;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Threading.Tasks;

    public class ProductsService : BaseApiService, IProductsService
    {
        public ProductsService(IDeviceManager deviceManager) 
            : base(deviceManager)
        {
        }

        public async Task<ProductsResponse> GetProducts(SearchProductsParameters parameters)
        {
            string endpoint = AppServiceConfiguration.SearchProductsEndpoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ProductsResponse());
                return (ProductsResponse)Convert.ChangeType(responseBase, typeof(ProductsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ProductsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ProductsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ProductResponse> GetProduct(ProductParameters parameters)
        {
            string endpoint = AppServiceConfiguration.ProductEndpoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ProductResponse());
                return (ProductResponse)Convert.ChangeType(responseBase, typeof(ProductResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ProductResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ProductResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ProductSearcherResponse> ProductSearcher(ProductSearcherParameters parameters)
        {
            string endpoint = AppServiceConfiguration.SuggestProductsEndpoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ProductSearcherResponse());
                return (ProductSearcherResponse)Convert.ChangeType(responseBase, typeof(ProductSearcherResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ProductSearcherResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ProductSearcherResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<AddProductsResponse> AddProducts(Order order)
        {
            string endpoint = AppServiceConfiguration.AddProductsToCarEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, order);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new AddProductsResponse());
                return (AddProductsResponse)Convert.ChangeType(responseBase, typeof(AddProductsResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new AddProductsResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<AddProductsResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }

        public async Task<ProductsPriceResponse> GetProductsPrice(ProductsPriceParameters parameters)
        {
            string endpoint = AppServiceConfiguration.GetProductsPriceEndPoint;
            var response = await HttpClientBaseService.PostAsync(endpoint, parameters);

            if (response.Content == null)
            {
                var responseBase = ValidateResponse(response, new ProductsPriceResponse());
                return (ProductsPriceResponse)Convert.ChangeType(responseBase, typeof(ProductsPriceResponse));
            }
            else
            {
                string data = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(data))
                {
                    return new ProductsPriceResponse
                    {
                        Result = new MessagesContainer { HasErrors = false },
                        HttpResponse = GetNoContentResponse()
                    };
                }

                var result = JsonService.Deserialize<ProductsPriceResponse>(data);
                result.HttpResponse = response;
                return result;
            }
        }
    }
}
