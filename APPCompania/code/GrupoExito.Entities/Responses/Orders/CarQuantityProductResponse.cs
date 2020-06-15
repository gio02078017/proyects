namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;

    public class CarQuantityProductResponse : ProductResponseBase
    {
        public int Quantity { get; set; }
        public bool Error { get; set; }
        public string SuccessCode { get; set; }
    }
}
