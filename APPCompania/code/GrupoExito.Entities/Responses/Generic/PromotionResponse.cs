namespace GrupoExito.Entities.Responses.Generic
{
    using GrupoExito.Entities.Entiites.Generic.Contents;
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class PromotionResponse : ResponseBase
    {
        public PromotionResponse()
        {
            Promotions = new List<Promotion>();
        }

        public string LastDateUpdated { get; set; }
        public bool HaveNewPromotion { get; set; }

        public IList<Promotion> Promotions { get; set; }
    }
}
