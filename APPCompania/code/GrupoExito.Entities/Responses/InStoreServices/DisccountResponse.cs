namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Responses.Base;

    public class DisccountResponse : ResponseBase
    {
        public string TransactionId { get; set; }
        public bool Respose { get; set; }
        public string MessageDetail { get; set; }
        public string Message { get; set; }
    }
}
