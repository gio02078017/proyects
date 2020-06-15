using GrupoExito.Entities;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Utilities.Resources;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.SignUpModelTest
{
    public class SignUpModelTest : BaseSignUpModelTest
    {
        [Fact]
        public async Task RegisterUserSuccessfull()
        {
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

            var clientResponse = new ClientResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Token = "any"
            };

            SignUpService.Setup(_ => _.RegisterUser(user)).ReturnsAsync(clientResponse);

            // Action
            var actual = await SgnUpModelBusiness.RegisterUser(user);


            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
            Assert.True(actual.Token != null);
        }

        [Fact]
        public async Task RegisterUserFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });

            var user = new User()
            {
                Id = "myId",
                FirstName = "myFirstName",
                LastName = "MyLastName",
                Password = "mycurrentPassword"
            };

            FakeResultWithMessage.HasErrors = true;

            var clientResponse = new ClientResponse()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage,
                Token = null
            };

            SignUpService.Setup(_ => _.RegisterUser(user)).ReturnsAsync(clientResponse);

            // Action
            var actual = await SgnUpModelBusiness.RegisterUser(user);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
            Assert.Null(actual.Token);

        }     

        [Fact]
        public void ValidateFieldsFailed()
        {
            var user = new User()
            {
                Password = "Any",
                Email = "Any",
                AcceptTerms = false,
                FirstName = "Any",
                LastName = "Any"
            };

            var useRegularExpressionsEmail = new User()
            {
                Password = "Any",
                Email = "test@"
            };

            // Action
            var actualValidationFields = SgnUpModelBusiness.ValidateFields(user);
            var actualValidationRegularExpresionEmail = SgnUpModelBusiness.ValidateFields(useRegularExpressionsEmail);

            Assert.NotEmpty(actualValidationFields);
            Assert.NotEmpty(actualValidationRegularExpresionEmail);
        }

        [Fact]
        public void ValidateFieldsSuccessfull()
        {
            var user = new User()
            {
                Password = "1234567890A",
                Email = "any@any.com",
                AcceptTerms = true,
                FirstName = "Any",
                LastName = "Any",
                CellPhone = "3016005517",
                DocumentNumber = "1212121",
                DocumentType = 1,
                Gender = "any",
                DateOfBirth= "01/08/1987",
                ConfirmPassword = "1234567890A"
            };

            // Action
            var actualValidationFields = SgnUpModelBusiness.ValidateFields(user);

            Assert.Empty(actualValidationFields);
        }

        [Fact]
        public void ValidateFieldsPasswordAndConfirmPasswordDiferents()
        {
            var user = new User()
            {
                Password = "1234567890A",
                Email = "any@any.com",
                AcceptTerms = true,
                FirstName = "Any",
                LastName = "Any",
                CellPhone = "3016005517",
                DocumentNumber = "1212121",
                DocumentType = 1,
                Gender = "any",
                DateOfBirth = "01/08/1987",
                ConfirmPassword = "1234567890C"
            };

            // Action
            var actualValidationFields = SgnUpModelBusiness.ValidateFields(user);

            Assert.Equal(actualValidationFields, AppMessages.PasswordConfirmationValidationText);
        }

        [Fact]
        public void ValidateFieldsNotAcceptTermsAndConditions()
        {
            var user = new User()
            {
                Password = "1234567890A",
                Email = "any@any.com",
                AcceptTerms = false,
                FirstName = "Any",
                LastName = "Any",
                CellPhone = "3016005517",
                DocumentNumber = "1212121",
                DocumentType = 1,
                Gender = "any",
                DateOfBirth = "01/08/1987",
                ConfirmPassword = "1234567890A"
            };

            // Action
            var actualValidationFields = SgnUpModelBusiness.ValidateFields(user);

            Assert.Equal(actualValidationFields, AppMessages.TermsAndConditionValidationText);
        }
    }
}
