using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.InStoreServices;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.DiscountsModelTest
{
    public class DiscountsModelTest : BaseDiscountsModelTest
    {
        [Fact]
        public async Task ActiveDisccountSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            DiscountParameters parameters = new DiscountParameters()
            {
                PosCode = "any"
            };

            DisccountResponse response = new DisccountResponse()
            {
                TransactionId = "any",
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.ActiveDisccount(parameters)).ReturnsAsync(response);
            var actual = await Model.ActiveDisccount(parameters);

            Assert.NotEmpty(actual.TransactionId);
            Service.VerifyAll();
        }

        [Fact]
        public async Task ActiveDisccountFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

            DiscountParameters parameters = new DiscountParameters()
            {
                PosCode = "any"
            };

            DisccountResponse response = new DisccountResponse()
            {
                TransactionId = string.Empty,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.ActiveDisccount(parameters)).ReturnsAsync(response);
            var actual = await Model.ActiveDisccount(parameters);

            Assert.Empty(actual.TransactionId);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetDiscountsSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            DiscountParameters parameters = new DiscountParameters()
            {
                PosCode = "any"
            };

            DiscountsResponse response = new DiscountsResponse()
            {
                TransactionId = "any",
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetDiscounts()).ReturnsAsync(response);
            var actual = await Model.GetDiscounts();

            Assert.NotEmpty(actual.TransactionId);
            Service.VerifyAll();
        }

        [Fact]
        public async Task GetDiscountsFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

            DiscountParameters parameters = new DiscountParameters()
            {
                PosCode = "any"
            };

            DiscountsResponse response = new DiscountsResponse()
            {
                TransactionId = string.Empty,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            Service.Setup(_ => _.GetDiscounts()).ReturnsAsync(response);
            var actual = await Model.GetDiscounts();

            Assert.Empty(actual.TransactionId);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
        }
    }
}
