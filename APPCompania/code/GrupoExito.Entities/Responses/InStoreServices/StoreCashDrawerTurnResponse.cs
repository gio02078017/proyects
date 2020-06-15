namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class StoreCashDrawerTurnResponse : ResponseBase
    {
        public StoreCashDrawerTurnResponse()
        {
            Stores = new List<StoreCashDrawerTurn>();
        }

        [JsonProperty("dependencies")]
        public IList<StoreCashDrawerTurn> Stores { get; set; }
    }
}
