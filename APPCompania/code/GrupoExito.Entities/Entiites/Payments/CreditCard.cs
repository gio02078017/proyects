using Newtonsoft.Json;

namespace GrupoExito.Entities.Entiites
{
    public class CreditCard
    {
        public string Bin { get; set; }
        public string Token { get; set; }
        [JsonProperty("cardName")]
        public string Name { get; set; }
        [JsonProperty("expirationMonth")]
        public string ExpirationMonth { get; set; }
        [JsonProperty("expirationYear")]
        public string ExpirationYear { get; set; }
        public bool Favorite { get; set; }

        [JsonProperty("urlImage")]
        public string Image { get; set; }
        public bool IsNextCaduced { get; set; }
        [JsonProperty("MaskNumberCard")]
        public string NumberCard { get; set; }
        public bool Selected { get; set; }
        [JsonProperty("transaction_reference")]
        public string Reference { get; set; }
        public string Number { get; set; }
        public string Status { get; set; }

        public string Type { get; set; }
        public bool Paymentez { get; set; }
        public string TypePayment { get; set; }
    }
}
