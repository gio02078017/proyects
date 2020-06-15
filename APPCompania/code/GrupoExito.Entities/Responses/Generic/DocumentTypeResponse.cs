namespace GrupoExito.Entities.Responses.Generic
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class DocumentTypeResponse : ResponseBase
    {
        public DocumentTypeResponse()
        {
            this.DocumentTypes = new List<DocumentType>();
        }

        [JsonProperty("documentTypes")]
        public IList<DocumentType> DocumentTypes { get; set; }
    }
}
