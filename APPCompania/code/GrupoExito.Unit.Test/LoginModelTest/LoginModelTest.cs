using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Users;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace GrupoExito.Test.LoginModelTest
{
    public class LoginModelTest : BaseLoginModelTest
    {        
        [Fact]
        public async Task LoginSuccessful(){

            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var user = new User()
            {
                Id = "myId",
                FirstName = "myFirstName",
                LastName = "MyLastName",
                Password = "mycurrentPassword"                                               
            };

            var loginResponse = new LoginResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                User = user,
                Result = FakeResultWithMessage
            };

            LoginService.Setup(_ => _.Login(userCredentials)).ReturnsAsync(loginResponse);

            // Action
            var actual = await LoginModelBusiness.Login(userCredentials);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.User != null);
        }

        [Fact]
        public async Task LoginFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });
            FakeResultWithMessage.HasErrors = true;

            var loginResponse = new LoginResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                User = null,
                Result = FakeResultWithMessage
            };

            LoginService.Setup(_ => _.Login(userCredentials)).ReturnsAsync(loginResponse);

            //Action
            var actual = await LoginModelBusiness.Login(userCredentials);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.Null(actual.User);

        }

        [Fact]
        public void ValidateFieldsSuccessfull()
        {
            var validateFieldsResponse = string.Empty;
            userCredentials = new UserCredentials()
            {
                Email = "test@test.com",
                Password = "myPasswordTest"
            };

            // Action
            var actual = LoginModelBusiness.ValidateFields(userCredentials);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateFieldsFailed()
        {
            var userCredentialsFielsNotEmpty = new UserCredentials()
            {
                Password = "",
                Email = ""
            };

            var userCredentialsRegularExpressionsEmail = new UserCredentials()
            {
                Password = "myPasswordTest",
                Email = "test@"
            };

            // Action
            var actualValidationFields = LoginModelBusiness.ValidateFields(userCredentialsFielsNotEmpty);
            var actualValidationRegularExpresionEmail = LoginModelBusiness.ValidateFields(userCredentialsRegularExpressionsEmail);

            Assert.NotEmpty(actualValidationFields);
            Assert.NotEmpty(actualValidationRegularExpresionEmail);
        }
    }
}
