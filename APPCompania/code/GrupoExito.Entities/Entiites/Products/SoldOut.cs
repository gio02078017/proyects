using Newtonsoft.Json;

namespace GrupoExito.Entities.Entiites
{
    public class SoldOut
    {
        [JsonProperty("prdId")]
        public string Id { get; set; }
        public string SkuId { get; set; }
        public string CommerceItemId { get; set; }

        [JsonProperty("qty")]
        public int Quantity { get; set; }
        public bool PickupAvailable { get; set; }
        public bool DriveInAvailable { get; set; }      
        public bool PriceIsPromotional { get; set; }
        public bool Marketplace { get; set; }
        public bool IsStrategyDiscount { get; set; }
        public bool InventoryManagerModified { get; set; }
        public string RelatedChildWarrantySKUId { get; set; }
        public string RelatedChildInsuranceSKUId { get; set; }
        public string ParentSKUId { get; set; }
        public int QtyRelationship { get; set; }
        public bool FulFillment { get; set; }
        public string MsisdnRecharge { get; set; }
        public bool PermiteDevolver { get; set; }
        public bool IsEstimatedWeight { get; set; }
        public bool Available { get; set; }
        public string Name { get; set; }
        public string Presentation { get; set; }
        public string ImagePath { get; set; }
    }
}
