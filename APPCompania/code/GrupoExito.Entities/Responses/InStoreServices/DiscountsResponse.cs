namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class DiscountsResponse : ResponseBase
    {
        public DiscountsResponse()
        {
            this.Coupons = new List<Discount>();
            this.RedeemedDiscounts = new List<Discount>();
            this.ActivatedDiscounts = new List<Discount>();
        }

        public string TransactionId { get; set; }
        public string Message { get; set; }
        [JsonProperty("activateCoupons")]
        public int ActivateCoupons { get; set; }
        public IList<Discount> Coupons { get; set; }
        public IList<Discount> RedeemedDiscounts { get; set; }
        public IList<Discount> ActivatedDiscounts { get; set; }
        public ActiveDiscounts ActiveDiscounts { get; set; }
        public bool PreviewCampaign { get; set; }
        public bool CorpEvent { get; set; }
        public string HeaderCampaign { get; set; }
        public int TotalActiveDiscounts { get; set; }
        public int TotalRedeemedDiscounts { get; set; }
        public int TotalActivatedDiscounts { get; set; }
        public bool IsRedeemable = true;
    }
}
