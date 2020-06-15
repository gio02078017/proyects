namespace GrupoExito.Entities
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.SubCategories = new List<Category>();
        }

        [JsonProperty("idCategory")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("idParentCategory")]
        public string IdParentCategory { get; set; }

        [JsonProperty("imageMiniature")]
        public string IconCategory { get; set; }

        [JsonProperty("imageMiniatureGray")]
        public string IconCategoryGray { get; set; }

        [JsonProperty("imageMedium")]
        public string ImageCategory { get; set; }

        [JsonProperty("siteId")]
        public string SiteId { get; set; }

        public IList<Category> SubCategories { get; set; }
    }
}
