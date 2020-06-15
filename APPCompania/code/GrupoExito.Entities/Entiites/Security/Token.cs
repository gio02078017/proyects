namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class Token
    {
        [JsonProperty("token")]
        public string AccessToken { get; set; }

        [JsonProperty("expiresIn")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
