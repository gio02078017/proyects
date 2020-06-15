namespace GrupoExito.Logic.Models.Contacts
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System.Collections.Generic;

    public class ContactsModel
    {
        public List<Contact> GetContacts()
        {
            if (AppServiceConfiguration.SiteId.ToLower().Equals("exito"))
            {
                return JsonService.Deserialize<List<Contact>>(AppConfigurations.ContactsSourceExito);
            }
            else
            {
                return JsonService.Deserialize<List<Contact>>(AppConfigurations.ContactsSourceCarulla);
            }
        }
    }
}
