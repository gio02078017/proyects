namespace GrupoExito.Entities.Responses.Addresses
{
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class AddressResponse : ResponseBase
    {
        public AddressResponse()
        {
            Addresses = new List<UserAddress>();
        }

        public IList<UserAddress> Addresses { get; set; }
    }
}
