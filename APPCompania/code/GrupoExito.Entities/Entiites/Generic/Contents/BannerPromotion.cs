namespace GrupoExito.Entities.Entiites.Generic.Contents
{
    using Newtonsoft.Json;

    public class BannerPromotion : Content
    {
        [JsonProperty("idSlider")]
        public int Id { get; set; }
        [JsonProperty("urlImage")]
        public string Image { get; set; }
        [JsonProperty("actionDroid")]
        public string ActionDroid { get; set; }
        [JsonProperty("actionIos")]
        public string ActionIos { get; set; }
        [JsonProperty("parameterAction")]
        public string ParameterAction { get; set; }
    }
}
