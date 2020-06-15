using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Resources;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GrupoExito.Test.OtherServicesModelTest
{
    public class OtherServicesModelTest : BaseOtherServicesModelTest
    {
        [Fact]
        public void GetMenuOtherServicesSucessfull()
        {
            JsonService.Setup(_ => _.Deserialize<List<MenuItem>>(AppConfigurations.MenuOtherServicesSource));

            var actual = Model.GetMenuOtherServices();

            Assert.True(actual.Any());
        }
    }
}
