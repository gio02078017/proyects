namespace GrupoExito.Entities
{
    using GrupoExito.Entities.Entiites.Users;
    using Newtonsoft.Json;

    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }

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

        [JsonProperty("password")]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("acceptTerms")]
        public bool AcceptTerms { get; set; }

        [JsonProperty("generatedPassword")]
        public bool GeneratedPassword { get; set; }

        [JsonProperty("prime")]
        public bool Prime { get; set; }

        [JsonProperty("Points")]
        public double Points { get; set; }

        [JsonProperty("activeOrder")]
        public string ActiveOrder { get; set; }

        public string PaymentMethodPrime { get; set; }
        public string EndDatePrime { get; set; }
        public string StartDatePrime { get; set; }
        public int ActiveOrderCount { get; set; }
        public string PeriodicityPrime { get; set; }
        [JsonProperty("segment")]
        public Segment SegmentClient { get; set; }

        [JsonProperty("isValidated")]
        public bool UserActivate { get; set; } 
    }
}
