namespace GrupoExito.Entities.Entiites
{
    using Newtonsoft.Json;

    public class StatusCashDrawerTurn
    {
        [JsonProperty("branch_id")]
        public string StoreId { get; set; }
    }
}
