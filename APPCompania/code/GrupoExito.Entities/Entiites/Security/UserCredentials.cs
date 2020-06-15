namespace GrupoExito.Entities
{
    using Newtonsoft.Json;

    public class UserCredentials
    {
        [JsonProperty("userName")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }      

        [JsonProperty("oldPassword")]
        public string OldPassword { get; set; }

        [JsonProperty("newPassword")]
        public string NewPassword { get; set; }

        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
