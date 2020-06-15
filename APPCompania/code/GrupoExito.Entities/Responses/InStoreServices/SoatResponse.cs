namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class SoatResponse : ResponseBase
    {
        [JsonProperty("codigoQR")]
        public string QRCode { get; set; }

        [JsonProperty("placa")]
        public string Plate { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("mensaje")]
        public string MessageSoat { get; set; }
    }
}
