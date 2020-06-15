using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.Users;
using GrupoExito.Utilities.Resources;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.MyAccountModelTest
{
    public class MyAccountModelTest : BaseMyAccountModelTest
    {
        [Fact]
        public async Task GetMyAccountSuccessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var myAccount = new MyAccount()
            {
            };

            var response = new UserResponse()
            {
                User = new User()
                {
                    ActiveOrder = "Any"
                }
            };

            userService.Setup(_ => _.GetUser()).ReturnsAsync(response);
            //JsonService.Setup(_ => _.Deserialize<List<MenuItem>>(AppConfigurations.MenuMyAccountSource));

            // Action
            var actual = await Model.GetUser();

            // Assert
            Assert.True(actual.User.ActiveOrder == response.User.ActiveOrder);
        }

        [Fact]
        public async Task GetMyAccountFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.Messages.Add(new MessageAplication()
            {
                Code = "Any",
                Description = "Any",
                IsError = false
            });

            var myAccount = new MyAccount()
            {
            };

            var response = new UserResponse()
            {
                User = new User()
                {
                    ActiveOrder = ""
                }
            };

            userService.Setup(_ => _.GetUser()).ReturnsAsync(response);
            JsonService.Setup(_ => _.Deserialize<List<MenuItem>>(AppConfigurations.MenuMyAccountSource));

            // Action
            var actual = await Model.GetUser();

            // Assert
            Assert.True(string.IsNullOrEmpty(actual.User.ActiveOrder));
        }
    }
}
