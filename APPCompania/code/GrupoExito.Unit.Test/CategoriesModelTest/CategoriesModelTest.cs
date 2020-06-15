namespace GrupoExito.Test.CategoriesModelTest
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Logic.Models.Products;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CategoriesModelTest : BaseCategoriesModelTest
    {
        [Fact]
        public async Task GetCategoriesSucessfull()
        {          
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            var categories = new List<Category>()
            {
                new Category() { Id = "Any", Name = "Any" }
            };


            CategoriesResponse response = new CategoriesResponse()
            {
                Categories = categories,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            CategoriesService.Setup(_ => _.GetCategories()).ReturnsAsync(response);
            var actual = await Model.GetCategories();

            Assert.True(actual.Categories.Any());
            CategoriesService.VerifyAll();
        }

        [Fact]
        public async Task GetCategoriesFailed()
        {
            CategoriesModel model = new CategoriesModel(CategoriesService.Object);
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

            var categories = new List<Category>
            {
                new Category() { Id = "Any", Name = "Any" }
            };

            CategoriesResponse response = new CategoriesResponse()
            {
                Categories = new List<Category>(),
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            CategoriesService.Setup(_ => _.GetCategories()).ReturnsAsync(response);
            var actual = await model.GetCategories();

            Assert.False(actual.Categories.Any());
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            CategoriesService.VerifyAll();
        }
    }
}
