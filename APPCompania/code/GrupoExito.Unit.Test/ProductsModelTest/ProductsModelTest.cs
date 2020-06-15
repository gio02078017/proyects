using GrupoExito.Entities;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.Products;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.ProductsModelTest
{
    public class ProductsModelTest : BaseProductsModelTest
    {
        [Fact]
        public async Task GetProductsSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var products = new List<Product>()
            {
                new Product() { Id = "Any", Name = "Any" }
            };

            var categoriesIds = new List<string>
            {
                "000000000"
            };

            ProductsResponse response = new ProductsResponse()
            {
                Products = products,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            ProductsService.Setup(_ => _.GetProducts(Parameters)).ReturnsAsync(response);
            var actual = await ProductsModel.GetProducts(Parameters);

            Assert.True(actual.Products.Any());
            ProductsService.VerifyAll();
        }

        [Fact]
        public async Task GetProductsFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

            var categoriesIds = new List<string>
            {
                "000000000"
            };

            ProductsResponse response = new ProductsResponse()
            {
                Products = new List<Product>(),
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            ProductsService.Setup(_ => _.GetProducts(Parameters)).ReturnsAsync(response);
            var actual = await ProductsModel.GetProducts(Parameters);

            Assert.True(actual.Products.Count == 0);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            ProductsService.VerifyAll();
        }

        [Fact]
        public async Task ProductSearcherSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var products = new List<Product>()
            {
                new Product() { Id = "Any", Name = "Any" }
            };

            ProductSearcherParameters parameters = new ProductSearcherParameters()
            {
                DependencyId = "001",
                From = "0",
                Prefix = "any",
                Size = "10"
            };

            var BrandSuggest = new List<Item>
            {
                new Item() { Id = "any", Text = "Any" }
            };
            var CategorySuggest = new List<Item>
            {
                new Item() { Id = "any", Text = "Any" }
            };
            var NameSuggest = new List<Item>
            {
                new Item() { Id = "any", Text = "Any" }
            };

            ProductSearcherResponse response = new ProductSearcherResponse()
            {
                BrandSuggest = BrandSuggest,
                CategorySuggest = CategorySuggest,
                NameSuggest = NameSuggest,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            ProductsService.Setup(_ => _.ProductSearcher(parameters)).ReturnsAsync(response);
            var actual = await ProductsModel.ProductSearcher(parameters);

            Assert.True(actual.BrandSuggest.Any());
            Assert.True(actual.CategorySuggest.Any());
            Assert.True(actual.NameSuggest.Any());
            ProductsService.VerifyAll();
        }

        [Fact]
        public async Task ProductSearcherFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.BadRequest;

            var products = new List<Product>()
            {
                new Product() { Id = "Any", Name = "Any" }
            };

            ProductSearcherParameters parameters = new ProductSearcherParameters()
            {
                DependencyId = "001",
                From = "0",
                Prefix = "any",
                Size = "10"
            };

            var BrandSuggest = new List<Item>();
            var CategorySuggest = new List<Item>();
            var NameSuggest = new List<Item>();

            ProductSearcherResponse response = new ProductSearcherResponse()
            {
                BrandSuggest = BrandSuggest,
                CategorySuggest = CategorySuggest,
                NameSuggest = NameSuggest,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            ProductsService.Setup(_ => _.ProductSearcher(parameters)).ReturnsAsync(response);
            var actual = await ProductsModel.ProductSearcher(parameters);

            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest);
            Assert.False(actual.BrandSuggest.Any());
            Assert.False(actual.CategorySuggest.Any());
            Assert.False(actual.NameSuggest.Any());
            ProductsService.VerifyAll();
        }
    }
}
