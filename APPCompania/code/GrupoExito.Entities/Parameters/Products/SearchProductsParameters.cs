namespace GrupoExito.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class SearchProductsParameters
    {
        [JsonProperty("dependencyId")]
        public string DependencyId { get; set; }

        [JsonProperty("userQuery")]
        public string UserQuery { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("brands")]
        public IList<string> Brands { get; set; }

        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("categoriesNames")]
        public IList<string> CategoriesNames { get; set; }

        [JsonProperty("orderBy")]
        public string OrderBy { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        public string NextDateCache { get; set; }
        public List<string> Plus { get; set; }
    }
}
