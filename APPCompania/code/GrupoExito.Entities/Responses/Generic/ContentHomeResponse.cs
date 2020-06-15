namespace GrupoExito.Entities.Responses.Generic
{
    using GrupoExito.Entities.Entiites.Generic.Contents;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ContentHomeResponse : ResponseBase
    {
        public ContentHomeResponse()
        {
            Images = new List<BannerPromotion>();
        }

        [JsonProperty("sliders")]
        public IList<BannerPromotion> Images { get; set; }
    }
}
