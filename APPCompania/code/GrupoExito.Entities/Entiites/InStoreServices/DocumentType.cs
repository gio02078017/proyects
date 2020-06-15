namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class DocumentType
    {
        [JsonProperty("idDocumentType")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("codeSoat")]
        public string Code { get; set; }

        public string CodeClifre { get; set; }

        public string CodeProcessa { get; set; }
        public string Description { get; set; }
    }
}