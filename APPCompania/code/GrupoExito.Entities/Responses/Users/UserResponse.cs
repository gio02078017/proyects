namespace GrupoExito.Entities.Responses.Users
{
    using GrupoExito.Entities.Responses.Base;
    using Newtonsoft.Json;

    public class UserResponse : ResponseBase
    {
        [JsonProperty("client")]
        public User User { get; set; }
    }
}
