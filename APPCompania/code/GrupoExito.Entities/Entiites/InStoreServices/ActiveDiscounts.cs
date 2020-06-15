namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ActiveDiscounts
    {
        public ActiveDiscounts()
        {
            this.Killers = new List<Discount>();
            this.AlreadyPurchased = new List<Discount>();
            this.CouldLike = new List<Discount>();
        }

        [JsonProperty("killer")]
        public IList<Discount> Killers { get; set; }
        [JsonProperty("alreadyShopping")]
        public IList<Discount> AlreadyPurchased { get; set; }
        [JsonProperty("mightLike")]
        public IList<Discount> CouldLike { get; set; }
    }
}
