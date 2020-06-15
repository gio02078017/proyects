using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Entities.Responses.Payments;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.CreditCardModelTest
{
    public class CreditCardModelTest : BaseCreditCardModelTest
    {
        [Fact]
        public async Task GetCreditCardsSucessfull()
        {
            //Arrange
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication() { Code = "Any", Description = "Any", IsError = false });

            var creditCards = new List<CreditCard>()
            {
                new CreditCard() { Bin = "Any", Name = "Any" }
            };

            CreditCardResponse response = new CreditCardResponse()
            {
                CreditCards = creditCards,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            CreditCardService.Setup(_ => _.GetCreditCards()).ReturnsAsync(response);

            //Action
            //var actual = await Model.GetCreditCards();

            //Assert
            //Assert.True(actual.CreditCards.Any());
            //CreditCardService.VerifyAll();
        }

        [Fact]
        public async Task GetCreditCardsFailed()
        {
            //Arrange
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication() { Code = "Any", Description = "Any", IsError = false });
            FakeResultWithMessage.HasErrors = true;
            CreditCardResponse response = new CreditCardResponse()
            {
                CreditCards = new List<CreditCard>(),
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            CreditCardService.Setup(_ => _.GetCreditCards()).ReturnsAsync(response);

            //Action
            //var actual = await Model.GetCreditCards();

            //Assert
            //Assert.True(actual.CreditCards.Count == 0);
            //Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            //Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task DeleteCreditCardSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            CreditCard creditCard = new CreditCard()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            CreditCardService.Setup(_ => _.DeleteCreditCard(creditCard)).ReturnsAsync(response);

            // Action
            var actual = await Model.DeleteCreditCard(creditCard);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }

        [Fact]
        public async Task DeleteCreditCardFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });

            FakeResultWithMessage.HasErrors = true;

            CreditCard creditCard = new CreditCard()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            CreditCardService.Setup(_ => _.DeleteCreditCard(creditCard)).ReturnsAsync(response);

            // Action
            var actual = await Model.DeleteCreditCard(creditCard);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }
    }
}
