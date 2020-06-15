using GrupoExito.Entities;
using GrupoExito.Entities.Containers;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Contracts;
using GrupoExito.Utilities.Contracts.Base;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Contracts.Users;
using Moq;
using System.Collections.Generic;
using System.Net.Http;

namespace GrupoExito.Test.LoginModelTest
{
    public class BaseLoginModelTest
    {
        protected HttpResponseMessage FakeResponseWithStatusCode;
        protected MessagesContainer FakeResultWithMessage;
        protected UserCredentials userCredentials;
        protected LoginModel LoginModelBusiness;

        protected Mock<ILoginService> LoginService;
        protected Mock<ICryptographyService> CryptographyService;
        protected Mock<IDiffieHellmanService> DiffieHellmanService;
        protected Mock<IHttpClientService> HttpClientService;
        protected Mock<IJsonService> JsonService;

        public BaseLoginModelTest()
        {
            LoginService = new Mock<ILoginService>();
            CryptographyService = new Mock<ICryptographyService>();
            DiffieHellmanService = new Mock<IDiffieHellmanService>();
            HttpClientService = new Mock<IHttpClientService>();
            JsonService = new Mock<IJsonService>();
            LoginModelBusiness = new LoginModel(LoginService.Object);
            FakeResponseWithStatusCode = new HttpResponseMessage();
            FakeResultWithMessage = new MessagesContainer() { HasErrors = false, Messages = new List<MessageAplication>() };
            userCredentials = new UserCredentials();
        }
    }
}
