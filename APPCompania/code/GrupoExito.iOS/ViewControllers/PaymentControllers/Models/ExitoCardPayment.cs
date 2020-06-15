using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Helpers;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class ExitoCardPayment : IPayment
    {
        private string duesNumber;
        private CreditCard _creditCard { get; set; }

        public ExitoCardPayment(CreditCard creditCard)
        {
            this._creditCard = creditCard;
        }

        public Order GetOrder()
        {
            return ParametersManager.Order;
        }

        public void SetDues(string dues)
        {
            duesNumber = dues;
        }

        public string ValidatePayment()
        {
            if (string.IsNullOrEmpty(duesNumber))
            {
                return AppMessages.CreditCardQuoteMessage;
            }
            else
            {
                var order = OrderHelper.SetExitoCreditCardOrder(ParametersManager.Order, _creditCard, duesNumber);
                return string.Empty;
            }
        }

        public void ClearDues()
        {
            duesNumber = string.Empty;
        }
    }
}
