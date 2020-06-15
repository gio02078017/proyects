namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class Discount
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PosCode { get; set; }
        public string DhCuponId { get; set; }
        public string EventMode { get; set; }

        [JsonProperty("PluUrl")]
        public string Image { get; set; }

        [JsonProperty("PluDescription")]
        public string Description { get; set; }

        [JsonProperty("numberDays")]
        public int DaysRedeemLeft { get; set; }

        public string DiscountType { get; set; }
        public bool Active { get; set; }

        public bool Redeemable { get; set; } = true;

        public string Legal { get; set; }

        public string Plu { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("isFuture")]
        public bool Future { get; set; }
    }
}
