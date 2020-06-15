using GrupoExito.Entities;
using GrupoExito.Entities.Responses;
using GrupoExito.Entities.Responses.Base;
using GrupoExito.Utilities.Resources;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.PasswordModelTest
{
    public class PasswordModelTest : BasePasswordModelTest
    {
        [Fact]
        public void ValidateFieldsResetPasswordSuccessfull()
        {
            var actualValidationFields = Model.ValidateFieldsResetPassword("any@any.com");

            Assert.Empty(actualValidationFields);
        }

        [Fact]
        public void ValidateFieldsResetPasswordFailedEmailFormat()
        {
            var actualValidationFields = Model.ValidateFieldsResetPassword("any@com");

            Assert.Equal(actualValidationFields, AppMessages.EmailFormatErrorText);
        }

        [Fact]
        public void ValidateFieldsResetPasswordFailedEmailEmpty()
        {
            var actualValidationFields = Model.ValidateFieldsResetPassword("");

            Assert.Equal(actualValidationFields, AppMessages.RequiredFieldsText);
        }

        [Fact]
        public void ValidateFieldsChangePasswordSuccessfull()
        {
            UserCredentials userCredentials = new UserCredentials()
            {
                NewPassword = "abc12345",
                ConfirmPassword = "abc12345",
                OldPassword = "abc123456"
            };

            var actualValidationFields = Model.ValidateFieldsChangePassword(userCredentials);

            Assert.Empty(actualValidationFields);
        }

        [Fact]
        public void ValidateFieldsChangePasswordFailedPasswordCompare()
        {
            UserCredentials userCredentials = new UserCredentials()
            {
                NewPassword = "abc123455",
                ConfirmPassword = "abc12345",
                OldPassword = "abc123456"
            };

            var actualValidationFields = Model.ValidateFieldsChangePassword(userCredentials);

            Assert.Equal(actualValidationFields, AppMessages.PasswordConfirmationValidationText);
        }

        [Fact]
        public void ValidateFieldsChangePasswordFailedPasswordLength()
        {
            UserCredentials userCredentials = new UserCredentials()
            {
                NewPassword = "abc",
                ConfirmPassword = "abc",
                OldPassword = "abc123456"
            };

            var actualValidationFields = Model.ValidateFieldsChangePassword(userCredentials);

            Assert.Equal(actualValidationFields, AppMessages.PasswordLengthValidationText);
        }

        [Fact]
        public async Task ResetPasswordFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });


            FakeResultWithMessage.HasErrors = true;

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            passwordService.Setup(_ => _.ResetPassword("any@any.com")).ReturnsAsync(response);

            // Action
            var actual = await Model.ResetPassword("any@any.com");

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task ResetPasswordSuccessful()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });


            FakeResultWithMessage.HasErrors = true;

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            passwordService.Setup(_ => _.ResetPassword("any@any.com")).ReturnsAsync(response);

            // Action
            var actual = await Model.ResetPassword("any@any.com");

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task ChangePasswordFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });


            FakeResultWithMessage.HasErrors = true;

            UserCredentials userCredentials = new UserCredentials()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            passwordService.Setup(_ => _.ChangePassword(userCredentials)).ReturnsAsync(response);

            // Action
            var actual = await Model.ChangePassword(userCredentials);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            Assert.True(actual.Result.HasErrors);
        }

        [Fact]
        public async Task ChangePasswordSuccesfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = true
            });


            FakeResultWithMessage.HasErrors = false;

            UserCredentials userCredentials = new UserCredentials()
            {
            };

            var response = new ResponseBase()
            {
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            passwordService.Setup(_ => _.ChangePassword(userCredentials)).ReturnsAsync(response);

            // Action
            var actual = await Model.ChangePassword(userCredentials);

            // Assert
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.False(actual.Result.HasErrors);
        }
    }
}
