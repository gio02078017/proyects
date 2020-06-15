namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class Soat
    {
        [JsonProperty("type")]
        public string DocumentType { get; set; }

        [JsonProperty("document")]
        public string DocumentNumber { get; set; }

        [JsonProperty("licensePlate")]
        public string LicensePlate { get; set; }

        [JsonProperty("format")]
        public string ImageFormat { get; set; }

        [JsonProperty("codigoQR")]
        public string QRCode { get; set; }

        [JsonProperty("fechaExpedicion")]
        public string ExpeditionDate { get; set; }

        [JsonProperty("fechafinVigencia")]
        public string EffectiveDate { get; set; }

        [JsonProperty("numeroDocumentoTomador")]
        public string PersonDocumentNumber { get; set; }

        [JsonProperty("numeroPoliza")]
        public string PolicyNumber { get; set; }

        [JsonProperty("placa")]
        public string Plate { get; set; }

        [JsonProperty("tipoDocumentoTomador")]
        public string PersonDocumentType { get; set; }

        [JsonProperty("vin")]
        public string Vin { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("mensaje")]
        public string Message { get; set; }

        [JsonProperty("mensajeTecnico")]
        public string TechnicalMessage { get; set; }

        //public Image LocalImage { get; set; }
    }
}
