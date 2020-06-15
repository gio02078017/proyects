using GrupoExito.Entities;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class TotalViewModel : BaseViewModel
    {
        private Order order { get; set; }

        public string Total { get; set; }
        public string Subtotal { get; set; }
        public string BagTax { get; set; }
        public string Discounts { get; set; }

        public TotalViewModel(Order order)
        {
            this.order = order;
        }
    }
}
