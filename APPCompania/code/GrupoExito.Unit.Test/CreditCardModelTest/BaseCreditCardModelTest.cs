using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Paymentes;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Payments;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.CreditCardModelTest
{
    public class BaseCreditCardModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;
        protected CreditCardModel Model;

        protected Mock<ICreditCardService> CreditCardService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;

        public BaseCreditCardModelTest()
        {
            CreditCardService = new Mock<ICreditCardService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            Model = new CreditCardModel(CreditCardService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
        }
    }
}
