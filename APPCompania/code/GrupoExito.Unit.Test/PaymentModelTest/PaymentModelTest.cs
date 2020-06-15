using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Payments;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.PaymentModelTest
{
    public class PaymentModelTest : BasePaymentModelTest
    {
        [Fact]
        public async Task PaymentOnDeliverySucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            Order Order = new Order()
            {
                Id = "any"
            };

            PaymentResponse response = new PaymentResponse()
            {
                OrderId = "any",
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            paymentService.Setup(_ => _.Pay(Order)).ReturnsAsync(response);
            var actual = await Model.Pay(Order);

            Assert.True(!string.IsNullOrEmpty(actual.OrderId));
            paymentService.VerifyAll();
        }

        [Fact]
        public async Task PaymentOnDeliveryFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;

            Order Order = new Order()
            {
                Id = "any"
            };

            PaymentResponse response = new PaymentResponse()
            {
                OrderId = "",
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            paymentService.Setup(_ => _.Pay(Order)).ReturnsAsync(response);
            var actual = await Model.Pay(Order);

            Assert.True(string.IsNullOrEmpty(actual.OrderId));
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            paymentService.VerifyAll();
        }
    }
}
