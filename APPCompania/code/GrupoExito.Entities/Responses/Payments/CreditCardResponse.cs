namespace GrupoExito.Entities.Responses.Payments
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class CreditCardResponse : ResponseBase
    {
        public CreditCardResponse()
        {
            this.CreditCards = new List<CreditCard>();
        }

        [JsonProperty("userCards")]
        public IList<CreditCard> CreditCards { get; set; }
    }
}
