namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class CheckerPriceResponse : ResponseBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Presentation { get; set; }

        [JsonProperty("urlMediumImage")]
        public string Image { get; set; }

        [JsonProperty("salePrice")]
        public string Price { get; set; }

        public string Pum { get; set; }
    }
}
