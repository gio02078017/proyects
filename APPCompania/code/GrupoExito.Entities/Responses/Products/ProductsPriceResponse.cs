namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ProductsPriceResponse : ResponseBase
    {
        public ProductsPriceResponse()
        {
            this.Prices = new List<Price>();
        }

        [JsonProperty("prices")]
        public IList<Price> Prices { get; set; }
    }
}
