namespace GrupoExito.Android.Utilities
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Entiites.Generic.Contents;
    using GrupoExito.Utilities.Helpers;
    using System.Collections.Generic;

    public static class ParametersManager
    {
        public static List<Product> Products { get; set; }
        public static IList<ProductFilter> Categories { get; set; }
        public static IList<string> CategoryNames { get; set; }
        public static IList<ProductFilter> Brands { get; set; }
        public static string UserQuery { get; set; }
        public static IList<string> BrandNames { get; set; }
        public static string OrderBy { get; set; } = ConstOrder.Relevance;
        public static string OrderType { get; set; } = ConstOrderType.Desc;
        public static string HomeFrom { get; set; } = "0";
        public static string HomeSize { get; set; } = "12";
        public static string From { get; set; } = "0";
        public static string Size { get; set; } = "10";
        public static string FromRecommendProducts { get; set; } = "0";
        public static string SizeRecommendProducts { get; set; } = "25";
        public static bool IsInBackground { get; set; }
        private static Order UserOrder { get; set; }
        public static Order Order
        {
            get
            {
                if (DeviceManager.Instance.ValidateAccessPreference(ConstPreferenceKeys.Order))
                {
                    return JsonService.Deserialize<Order>(DeviceManager.Instance.GetAccessPreference(ConstPreferenceKeys.Order));
                }

                return UserOrder;
            }
            set
            {
                UserOrder = value;
            }
        }

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

        public static bool ChangeProductQuantity { get; set; }
        public static bool ChangeProductQuantityFromDetail { get; set; }
        public static bool FromProductsActivity { get; set; }
        public static string QRCode { get; set; }
        public static string SoatPlate { get; set; }
        public static bool ChangeAddress { get; set; }
        public static bool CallFromSummaryPayment { get; set; }
        public static BannerParameter ParameterActionBanner { get; set; }
        public static bool AddCreditCard { get; set; }
    }
}