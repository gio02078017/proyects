namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class DependencyType
    {
        [JsonProperty("idDependencyType")]
        public int Id { get; set; }

        [JsonProperty("dependencyTypeName")]
        public string Name { get; set; }
    }
}
