namespace GrupoExito.Entities.Responses.Payments
{
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class PaymentResponse : ProductResponseBase
    {
        public PaymentResponse()
        {
            this.Coupon = new List<string>();
            this.ShippingMethods = new List<ShippingMethod>();
            this.ProductsRemoved = new List<SoldOut>();
            this.ProductsChanged = new List<SoldOut>();
        }

        public string OrderId { get; set; }
        public int ProductsQuantity { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public string PaymentMethodName { get; set; }
        public IList<string> Coupon { get; set; }
        public string TransactionDate { get; set; }
        public bool ProductsSubstitution { get; set; }
        public string GatewayMessage { get; set; }
        public string  TransactionState { get; set; }
        public IList<ShippingMethod> ShippingMethods { get; set; }
        public bool FinishProcess { get; set; }
        public bool IsPrime { get; set; }
        public decimal CostRemaining { get; set; }
        public IList<SoldOut> ProductsRemoved { get; set; }
        public IList<SoldOut> ProductsChanged { get; set; }
        public string StatusCode { get; set; }
    }
}
