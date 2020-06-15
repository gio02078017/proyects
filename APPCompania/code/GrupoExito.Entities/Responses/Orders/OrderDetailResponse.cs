namespace GrupoExito.Entities.Responses.Orders
{
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class OrderDetailResponse : ResponseBase
    {
        public OrderDetailResponse()
        {
            this.Products = new List<Product>();
        }

        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public decimal Total { get; set; }
        public IList<Product> Products { get; set; }
    }
}
