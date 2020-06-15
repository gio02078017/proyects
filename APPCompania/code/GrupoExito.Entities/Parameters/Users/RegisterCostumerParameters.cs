namespace GrupoExito.Entities.Parameters.Users
{
    using Newtonsoft.Json;

    public class RegisterCostumerParameters
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("documentType")]
        public int DocumentType { get; set; }

        [JsonProperty("documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty("cellPhone")]
        public string CellPhone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }
    }
}
