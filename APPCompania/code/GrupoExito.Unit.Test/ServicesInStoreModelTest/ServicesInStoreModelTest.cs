using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Resources;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GrupoExito.Test.ServicesInStoreModelTest
{
    public class ServicesInStoreModelTest : BaseServicesInStoreModelTest
    {
        [Fact]
        public void GetMenuServicesInStoreSucessfull()
        {
            JsonService.Setup(_ => _.Deserialize<List<MenuItem>>(AppConfigurations.MenuServicesInStoreSource));

            var actual = Model.GetMenuServicesInStore();

            Assert.True(actual.Any());
        }
    }
}
