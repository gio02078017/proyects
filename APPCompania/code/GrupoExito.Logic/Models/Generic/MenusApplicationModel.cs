namespace GrupoExito.Logic.Models.Generic
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System.Collections.Generic;

    public class MenusApplicationModel
    {
        public List<MenuItem> GetMenuOtherServices()
        {
            return JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuOtherServicesSource);
        }

        public List<MenuItem> GetMenuLobby()
        {
            return JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuLobbySource);
        }

        public List<MenuItem> GetMenuServicesInStore()
        {
            return JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuServicesInStoreSource);
        }
    }
}
