namespace GrupoExito.Entities.Parameters
{
    using Newtonsoft.Json;

    public class ProductSearcherParameters
    {
        [JsonProperty("dependencyId")]
        public string DependencyId { get; set; }

        [JsonProperty("prefix")]
        public string Prefix { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        public string OrderId { get; set; }
    }
}
