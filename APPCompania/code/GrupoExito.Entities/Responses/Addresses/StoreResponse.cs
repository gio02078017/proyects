namespace GrupoExito.Entities.Responses.Addresses
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class StoreResponse : ResponseBase
    {
        public StoreResponse()
        {
            Stores = new List<Store>();
        }

        [JsonProperty("dependencies")]
        public IList<Store> Stores { get; set; }
    }
}
