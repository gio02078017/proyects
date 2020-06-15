using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Utilities.Resources;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GrupoExito.Test.InsuranceModelTest
{
    public class InsuranceModelTest : BaseInsuranceModelTest
    {
        [Fact]
        public async Task GetSoatSucessfull()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.OK;

            Soat soat = new Soat() { };

            SoatResponse response = new SoatResponse()
            {
                QRCode = "any",
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            InsuranceService.Setup(_ => _.GetSoat(soat)).ReturnsAsync(response);
            var actual = await Model.GetSoat(soat);

            Assert.NotEmpty(actual.QRCode);
            InsuranceService.VerifyAll();
        }

        [Fact]
        public async Task GetSoatFailed()
        {
            FakeResponseWithStatusCode.StatusCode = System.Net.HttpStatusCode.Forbidden;
            FakeResultWithMessage.HasErrors = true;
            Soat soat = new Soat() { };

            SoatResponse response = new SoatResponse()
            {
                QRCode = string.Empty,
                HttpResponse = FakeResponseWithStatusCode,
                Result = FakeResultWithMessage
            };

            InsuranceService.Setup(_ => _.GetSoat(soat)).ReturnsAsync(response);
            var actual = await Model.GetSoat(soat);

            Assert.Empty(actual.QRCode);
            Assert.True(actual.Result.HasErrors);
            Assert.True(actual.HttpResponse.StatusCode == System.Net.HttpStatusCode.Forbidden);
            InsuranceService.VerifyAll();
        }

        [Fact]
        public void ValidateFieldsFailed()
        {
            Soat soat = new Soat
            {
                DocumentNumber = string.Empty
            };

            var actualValidationFields = Model.ValidateFields(soat);

            Assert.NotEmpty(actualValidationFields);
            Assert.Equal(AppMessages.RequiredFieldsText, actualValidationFields);
        }

        [Fact]
        public void ValidateFieldsSuccessfull()
        {
            Soat soat = new Soat
            {
                DocumentNumber = "Any",
                DocumentType = "Any",
                LicensePlate = "any"
            };

            var actualValidationFields = Model.ValidateFields(soat);
            Assert.Empty(actualValidationFields);
        }
    }
}
