using GrupoExito.Entities;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces
{
    public interface IPayment
    {
        string ValidatePayment();
        Order GetOrder();
        void SetDues(string dues);
        void ClearDues();
    }
}
