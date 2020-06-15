namespace GrupoExito.Entities.Responses.Generic
{
    using GrupoExito.Entities.Entiites.Generic;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class TaxBagResponse : ResponseBase
    {
        public TaxBagResponse()
        {
            TaxBags = new List<TaxBag>();
        }

        [JsonProperty("taxs")]
        public IList<TaxBag> TaxBags { get; set; }
    }
}
