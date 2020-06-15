using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.Entities.Responses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Products;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.ShpoppingListModelTest
{
    public class ShpoppingListModelTest : BaseShpoppingListModelTest
    {
        [Fact]
        public async Task GetSuggestedProductsSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var products = new List<ProductList>()
            {
                new ProductList() { Id = "Any", Name = "Any" }
            };

            ProductParameters parameters = new ProductParameters();

            SuggestedProductsResponse response = new SuggestedProductsResponse()
            {
                ProductsClient = products,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetSuggestedProducts(parameters)).ReturnsAsync(response);
            var actual = await Model.GetSuggestedProducts(parameters);

            Assert.True(actual.ProductsClient.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetSuggestedProductsFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            var products = new List<ProductList>();

            ProductParameters parameters = new ProductParameters();

            SuggestedProductsResponse response = new SuggestedProductsResponse()
            {
                ProductsClient = products,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetSuggestedProducts(parameters)).ReturnsAsync(response);
            var actual = await Model.GetSuggestedProducts(parameters);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.ProductsClient.Any());
        }

        [Fact]
        public async Task GetShoppingListsSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var shoppingList = new List<ShoppingList>()
            {
                new ShoppingList() { Id = "0122", Name = "Any" }
            };

            ShoppingListsResponse response = new ShoppingListsResponse()
            {
                ShpoppingLists = shoppingList,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetShoppingLists()).ReturnsAsync(response);
            var actual = await Model.GetShoppingLists();

            Assert.True(actual.ShpoppingLists.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetShoppingListsFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            var shoppingList = new List<ShoppingList>();

            ShoppingListsResponse response = new ShoppingListsResponse()
            {
                ShpoppingLists = shoppingList,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetShoppingLists()).ReturnsAsync(response);
            var actual = await Model.GetShoppingLists();

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.ShpoppingLists.Any());
        }

        [Fact]
        public async Task AddShoppingListSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var shoppingList = new ShoppingList() { Id = "0122", Name = "Any" };

            ShoppingListsResponse response = new ShoppingListsResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.AddShoppingList(shoppingList)).ReturnsAsync(response);
            var actual = await Model.AddShoppingList(shoppingList);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Service.VerifyAll();
        }

        [Fact]
        public async Task AddShoppingListFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            var shoppingList = new ShoppingList() { Id = "0122", Name = "Any" };

            ShoppingListsResponse response = new ShoppingListsResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.AddShoppingList(shoppingList)).ReturnsAsync(response);
            var actual = await Model.AddShoppingList(shoppingList);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task UpdateShoppingListSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var shoppingList = new ShoppingList() { Id = "0122", Name = "Any" };

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.UpdateShoppingList(shoppingList)).ReturnsAsync(response);
            var actual = await Model.UpdateShoppingList(shoppingList);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Service.VerifyAll();
        }

        [Fact]
        public async Task UpdateShoppingListFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            var shoppingList = new ShoppingList() { Id = "0122", Name = "Any" };

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.UpdateShoppingList(shoppingList)).ReturnsAsync(response);
            var actual = await Model.UpdateShoppingList(shoppingList);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task DeleteShoppingListSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            string id = "any";

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.DeleteShoppingList(id)).ReturnsAsync(response);
            var actual = await Model.DeleteShoppingList(id);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Service.VerifyAll();
        }

        [Fact]
        public async Task DeleteShoppingListFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            string id = "any";

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.DeleteShoppingList(id)).ReturnsAsync(response);
            var actual = await Model.DeleteShoppingList(id);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task AddProductShoppingListSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var product = new Product() { Id = "any" };

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.AddProductShoppingList(product)).ReturnsAsync(response);
            var actual = await Model.AddProductShoppingList(product);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Service.VerifyAll();
        }

        [Fact]
        public async Task AddProductShoppingListFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            var product = new Product() { Id = "any" };

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.AddProductShoppingList(product)).ReturnsAsync(response);
            var actual = await Model.AddProductShoppingList(product);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task DeleteProductShoppingListSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            string productId = "any";
            string shoppingListId = "any";

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.DeleteProductShoppingList(productId, shoppingListId)).ReturnsAsync(response);
            var actual = await Model.DeleteProductShoppingList(productId, shoppingListId);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Service.VerifyAll();
        }

        [Fact]
        public async Task DeleteProductShoppingListFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            string productId = "any";
            string shoppingListId = "any";

            ResponseBase response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.DeleteProductShoppingList(productId, shoppingListId)).ReturnsAsync(response);
            var actual = await Model.DeleteProductShoppingList(productId, shoppingListId);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task GetProductsShoppingListSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            string shoppingListId = "any";
            var products = new List<ProductList>
            {
                new ProductList() { Id = "any" }
            };

            SuggestedProductsResponse response = new SuggestedProductsResponse()
            {
                ProductsClient = products,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetProductsShoppingList(shoppingListId)).ReturnsAsync(response);
            var actual = await Model.GetProductsShoppingList(shoppingListId);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(actual.ProductsClient.Any());
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetProductsShoppingListFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            FakeResultWithMessage.HasErrors = true;

            string shoppingListId = "any";
            var products = new List<ProductList>();

            SuggestedProductsResponse response = new SuggestedProductsResponse()
            {
                ProductsClient = products,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetProductsShoppingList(shoppingListId)).ReturnsAsync(response);
            var actual = await Model.GetProductsShoppingList(shoppingListId);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.False(actual.ProductsClient.Any());
        }
    }
}
