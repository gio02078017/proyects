namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class ClientResponse : ResponseBase
    {
        [JsonProperty("userId")]
        public string Id { get; set; }
        [JsonProperty("expiresIn")]
        public double ExpiresIn { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}