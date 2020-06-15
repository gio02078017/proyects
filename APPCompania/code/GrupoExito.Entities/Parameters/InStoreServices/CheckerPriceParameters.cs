namespace GrupoExito.Entities.Parameters
{
    using Newtonsoft.Json;

    public class CheckerPriceParameters
    {
        [JsonProperty("CODBAR")]
        public string Barcode { get; set; }

        [JsonProperty("DEPEN1")]
        public string DependencyId { get; set; }

        [JsonProperty("TAMIMA")]
        public string Size { get; set; }
    }
}
