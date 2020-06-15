using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.iOS.Utilities.Helpers
{
    public class OrderHelper
    {
        public static Order SetCreditCardOrder(Order order, CreditCard creditCard, string dues)
        {
            order = SetCommonOrderParameters(order);
            order.PaymentType = (int)EnumPaymentType.Dataphone;
            order.Bin = creditCard.Bin;
            order.NumberDues = dues;
            order.TypePayment = ConstTypePayment.CreditCard;

            SaveOrder(order);
            return order;
        }

        public static Order SetExitoCreditCardOrder(Order order, CreditCard creditCard, string dues)
        {
            order = SetCommonOrderParameters(order);
            order.PaymentType = (int)EnumPaymentType.Dataphone;
            order.Bin = creditCard.Bin;
            order.NumberDues = dues;
            order.TypePayment = ConstTypePayment.CreditCardExito;
            order.Number = creditCard.Number;

            SaveOrder(order);
            return order;
        }

        public static Order SetCashOnDeliveryOrder(Order order, int paymentType)
        {
            order = SetCommonOrderParameters(order);
            order.TypePayment = ConstTypePayment.Delivery;
            order.PaymentType = paymentType;

            SaveOrder(order);
            return order;
        }

        private static Order SetCommonOrderParameters(Order order)
        {
            if (order != null)
            {
                string dependencyId = ParametersManager.UserContext.DependencyId;
                bool selectedStore = ParametersManager.UserContext.Store != null ? true : false;
                if(!order.Contingency)
                {
                    order.TypeOfDispatch = selectedStore ? ConstTypeOfDispatch.Store : ConstTypeOfDispatch.Delivery;
                    order.TypeDispatch = order.TypeModality == ConstTypeModality.ScheduledPickup ? ConstTypeModality.Scheduled : ConstTypeModality.Express;
                }

                order.DependencyAddress = selectedStore ? ParametersManager.UserContext.Store.Address : string.Empty;
                order.DependencyName = selectedStore ? ParametersManager.UserContext.Store.Name : string.Empty;
                //order.DependencyId = ParametersManager.UserContext.DependencyId;
                order.DependencyId = int.Parse(dependencyId).ToString();
            }

            return order;
        }

        private static void SaveOrder(Order order)
        {
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
        }
    }
}
