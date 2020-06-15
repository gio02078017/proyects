using GrupoExito.Entities;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Helpers;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class CashPayment : IPayment
    {
        private EnumPaymentType _paymentType { get; set; }

        public CashPayment(EnumPaymentType type)
        {
            this._paymentType = type;
        }

        public Order GetOrder()
        {
            return ParametersManager.Order;
        }

        public void SetDues(string dues)
        {
        }

        public string ValidatePayment()
        {
            var order = OrderHelper.SetCashOnDeliveryOrder(ParametersManager.Order, (int)_paymentType);
            return string.Empty;
        }

        public void ClearDues()
        {
        }
    }
}
