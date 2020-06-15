using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.iOS.Utilities
{
    public static class ParametersManager
    {
        public static IList<Product> Products { get; set; }
        public static IList<ProductFilter> Categories { get; set; }
        public static IList<ProductFilter> Brands { get; set; }
        public static IList<string> CategoriesName { get; set; }
        public static IList<string> BrandsName { get; set; }
        public static Category Category { get; set; }
        public static string UserQuery { get; set; }
        public static string OrderBy { get; set; } = ConstOrder.Name;
        public static string OrderByFilter { get; set; } = ConstOrder.Relevance;
        public static string OrderType { get; set; } = ConstOrderType.Desc;
        public static string From { get; set; } = "0";
        public static string Size { get; set; } = "12";
        public static string From_ProductsByCategory { get; set; } = "0";
        public static string Size_ProductsByCategory { get; set; } = "10";
        public static string FromRecommendProducts { get; set; } = "0";
        public static string SizeRecommendProducts { get; set; } = "25";
        public static int Count_ProductsByCategory { get; set; } = 10;
        public static bool IsAddProducts { get; set; }
        public static bool ProductAddedInWait { get; set; }
        public static bool ContainChanges { get; set; }
        public static bool ProductUpdated { get; set; }
        public static bool CreditCardChanges { get; set; }
        public static bool ChangeAddress { get; set; }
        public static string ShoppingListSelectedId { get; set; }
        private static UserContext User { get; set; }
        public static UserContext UserContext
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.User))
                {
                    return JsonService.Deserialize<UserContext>(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.User));
                }

                return User;
            }
            set
            {
                User = value;
            }
        }

        private static Order OrderEntity { get; set; }
        public static Order Order
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.Order))
                {
                    return JsonService.Deserialize<Order>(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.Order));
                }

                return OrderEntity;
            }
            set
            {
                OrderEntity = value;
            }
        }

        private static string MobileId { get; set; }
        public static string GetMobileId
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.MobileId))
                {
                    return DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.MobileId);
                }
                return MobileId;
            }
            set
            {
                MobileId = value;
            }
        }

        private static Ticket TicketId { get; set; }
        public static Ticket GetTicket
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.Ticket))
                {
                    return JsonService.Deserialize<Ticket>(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.Ticket));
                }
                return TicketId;
            }
            set
            {
                TicketId = value;
            }
        }

        private static string FirebasePushToken { get; set; }
        public static string GetFirebasePushToken
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.FirebaseToken))
                {
                    return JsonService.Deserialize<string>(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.FirebaseToken));
                }
                return FirebasePushToken;
            }
            set
            {
                FirebasePushToken = value;
            }
        }

        private static string BoxNumber { get; set; }
        public static string GetBoxNumber
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.BoxNumber))
                {
                    return JsonService.Deserialize<string>(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.BoxNumber));
                }
                return BoxNumber;
            }
            set
            {
                BoxNumber = value;
            }
        }
    }
}