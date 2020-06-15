namespace GrupoExito.Entities.Responses.Payments
{
    using GrupoExito.Entities.Responses.Base;

    public class CardResponse : ResponseBase
    {
        public string Type { get; set; }
        public string Help { get; set; }
        public string Description { get; set; }
        public string Bin { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
    }
}
