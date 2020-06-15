namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class LoginResponse : ResponseBase
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("expiresIn")]
        public double ExpiresIn { get; set; }
        [JsonProperty("client")]
        public User User { get; set; }
    }
}
