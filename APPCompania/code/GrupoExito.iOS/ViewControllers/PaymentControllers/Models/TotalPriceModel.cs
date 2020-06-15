namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class TotalPriceModel
    {
        #region Attributes
        private decimal _totalPrice;
        private decimal _subtotalPrice;
        private decimal _shipCost;
        private decimal _bagTax;
        private decimal _discounts;
        #endregion

        #region Properties
        public decimal TotalPrice { get => _totalPrice; set => _totalPrice = value; }
        public decimal SubtotalPrice { get => _subtotalPrice; set => _subtotalPrice = value; }
        public decimal ShipCost { get => _shipCost; set => _shipCost = value; }
        public decimal BagTax { get => _bagTax; set => _bagTax = value; }
        public decimal Discounts { get => _discounts; set => _discounts = value; }
        #endregion

        #region Constructors
        public TotalPriceModel(decimal totalPrice, decimal subtotalPrice, decimal shipCost, decimal bagTax, decimal discounts)
        {
            TotalPrice = totalPrice;
            SubtotalPrice = subtotalPrice;
            ShipCost = shipCost;
            BagTax = bagTax;
            Discounts = discounts;
        }
        #endregion
    }
}
