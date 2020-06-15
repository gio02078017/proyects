using System.Collections.Generic;
using System.Linq;
using Firebase.Analytics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Contracts.Analytic;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;

namespace GrupoExito.iOS.Utilities.Analytic
{
    public class FirebaseEventRegistrationService : IFirebaseEventRegistrationService
    {
        #region Attributes
        private static FirebaseEventRegistrationService instance;
        #endregion

        #region Properties
        public static FirebaseEventRegistrationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FirebaseEventRegistrationService();
                }

                return instance;
            }
        }

        public void RegisterScreen(string screenName, string screenClass)
        {
            Analytics.SetScreenNameAndClass(screenName, screenClass);
        }

        public void SignUp()
        {
            var parameters = new Dictionary<string, string>();
            string userType = ModelHelper.GetUserType(ParametersManager.UserContext);

            Analytics.SetUserProperty(AnalyticsParameter.UserType, userType);
            Analytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
            Analytics.LogEvent(EventNamesConstants.SignUp, FormatParameters(new Dictionary<string, object>()));
        }

        public void SignIn()
        {
            var parameters = new Dictionary<string, string>();
            string userType = ModelHelper.GetUserType(ParametersManager.UserContext);

            Analytics.SetUserProperty(AnalyticsParameter.UserType, userType);
            Analytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
            Analytics.LogEvent(EventNamesConstants.Login, FormatParameters(new Dictionary<string, object>()));
        }

        public void ProductImpression(IList<Product> products, string category)
        {
            if (products != null && products.Any())
            {
                NSMutableArray productsArray = new NSMutableArray();
                foreach (var product in products)
                {
                    var productDict = FormatProduct(product, category ?? "");
                    productsArray.Add(productDict);
                }

                NSDictionary<NSString, NSObject> ecommerce = CreateProductParameters(category ?? "", productsArray);
                Analytics.LogEvent(EventNamesConstants.ViewSearchResults, ecommerce);
            }
        }

        public void ProductClic(Product product, string category)
        {
            if (product != null)
            {
                var productDict = FormatProduct(product, category ?? "");

                NSMutableArray productsArray = new NSMutableArray();
                productsArray.Add(productDict);

                NSDictionary<NSString, NSObject> ecommerce = CreateProductParameters(category ?? "", productsArray);

                Analytics.LogEvent(EventNamesConstants.SelectContent, ecommerce);
            }
        }

        public void ProductDetail(Product product, string category)
        {
            if (product != null)
            {
                var productDict = FormatProduct(product, category ?? "");

                NSMutableArray productsArray = new NSMutableArray();
                productsArray.Add(productDict);

                NSDictionary<NSString, NSObject> ecommerce = CreateProductParameters(category ?? "", productsArray);

                Analytics.LogEvent(EventNamesConstants.ViewItem, ecommerce);
            }
        }

        public void AddProductToCart(Product product, string category)
        {
            var productDict = FormatProduct(product, category ?? "", true);

            NSMutableArray productsArray = new NSMutableArray();
            productsArray.Add(productDict);

            NSDictionary<NSString, NSObject> ecommerce = CreateProductParameters(category ?? "", productsArray);

            Analytics.LogEvent(EventNamesConstants.AddToCart, ecommerce);
        }

        public void DeleteProductFromCart(Product product, string category)
        {
            var productDict = FormatProduct(product, category ?? "", true);

            NSMutableArray productsArray = new NSMutableArray();
            productsArray.Add(productDict);

            NSDictionary<NSString, NSObject> ecommerce = CreateProductParameters(category ?? "", productsArray);

            Analytics.LogEvent(EventNamesConstants.RemoveFromCart, ecommerce);
        }

        public void DeleteProductsFromCart(IList<Product> products)
        {
            if (products != null && products.Any())
            {
                NSMutableArray productsArray = new NSMutableArray();
                foreach (var product in products)
                {
                    var productDict = FormatProduct(product, product.CategoryName ?? string.Empty);
                    productsArray.Add(productDict);
                }
                NSDictionary<NSString, NSObject> ecommerce = CreateProductParameters(string.Empty, productsArray);
                Analytics.LogEvent(EventNamesConstants.RemoveFromCart, ecommerce);
            }
        }

        public void Summary()
        {
            NSMutableArray productsArray = new NSMutableArray();
            foreach (var product in ParametersManager.Order.Products)
            {
                var productDict = FormatProduct(product, product.CategoryName ?? "");
                productsArray.Add(productDict);
            }

            NSDictionary<NSString, NSObject> ecommerce = CreateCheckoutParameters(productsArray, AnalyticsParameter.StepPayOne);

            Analytics.LogEvent(EventNamesConstants.BeginCheckout, ecommerce);
        }

        public void Schedule()
        {
            NSMutableArray productsArray = new NSMutableArray();
            foreach (var product in ParametersManager.Order.Products)
            {
                var productDict = FormatProduct(product, product.CategoryName ?? "");
                productsArray.Add(productDict);
            }

            NSDictionary<NSString, NSObject> ecommerce = CreateCheckoutParameters(productsArray, AnalyticsParameter.StepPayTwo);

            Analytics.LogEvent(EventNamesConstants.BeginCheckout, ecommerce);
        }

        public void Payment()
        {
            NSMutableArray productsArray = new NSMutableArray();
            foreach (var product in ParametersManager.Order.Products)
            {
                var productDict = FormatProduct(product, product.CategoryName ?? "");
                productsArray.Add(productDict);
            }

            NSDictionary<NSString, NSObject> ecommerce = CreateCheckoutParameters(productsArray, AnalyticsParameter.StepPayThree);

            Analytics.LogEvent(EventNamesConstants.BeginCheckout, ecommerce);
        }

        public void SummaryPayment()
        {
            NSMutableArray productsArray = new NSMutableArray();
            foreach (var product in ParametersManager.Order.Products)
            {
                var productDict = FormatProduct(product, product.CategoryName ?? "");
                productsArray.Add(productDict);
            }

            NSDictionary<NSString, NSObject> ecommerce = CreateCheckoutParameters(productsArray, AnalyticsParameter.StepPayFour, ParametersManager.Order.TypePayment);
            Analytics.LogEvent(EventNamesConstants.BeginCheckout, ecommerce);
        }

        public void SuccessPayment(Order order)
        {
            NSMutableArray productsArray = new NSMutableArray();
            foreach (var product in order.Products)
            {
                IList<NSString> keys = new List<NSString>()
            {
                ParameterNamesConstants.ItemId,
                ParameterNamesConstants.ItemName,
                ParameterNamesConstants.ItemCategory,
                ParameterNamesConstants.ItemBrand,
                ParameterNamesConstants.Price,
                ParameterNamesConstants.Currency,
                ParameterNamesConstants.Quantity
            };

                IList<NSString> values = new List<NSString>()
            {
                new NSString(product.Id),
                new NSString(product.Name),
                new NSString(product.CategoryName ?? ""),
                new NSString(product.Brand),
                new NSString(ModelHelper.GetPrice(product.Price).ToString()),
                new NSString("COP"),
                new NSString(product.Quantity.ToString())
            };

                var productDict = NSDictionary<NSString, NSString>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
                productsArray.Add(productDict);
            }

            NSDictionary<NSString, NSObject> ecommerce = CreateSuccessPaymentParameters(productsArray, order);
            Analytics.LogEvent(EventNamesConstants.EcommercePurchase, ecommerce);
        }

        public void ActivatedDiscount(Discount discount)
        {
            if (discount != null)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { AnalyticsParameter.Description, discount.Description },
                    { AnalyticsParameter.PLU, discount.Plu },
                    { AnalyticsParameter.Percentage, discount.DiscountType },
                    { AnalyticsParameter.UserId, CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey) }
                };

                Analytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
                Analytics.LogEvent(AnalyticsEvent.ActivateDiscount, FormatParameters(parameters));
            }
        }

        public void InactivateDiscount(Discount discount)
        {
            if (discount != null)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { AnalyticsParameter.Description, discount.Description },
                    { AnalyticsParameter.PLU, discount.Plu },
                    { AnalyticsParameter.Percentage, discount.DiscountType },
                    { AnalyticsParameter.UserId, CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey) }
                };

                Analytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
                Analytics.LogEvent(AnalyticsEvent.InactivateDiscount, FormatParameters(parameters));
            }
        }

        #endregion

        private NSDictionary<NSString, NSObject> FormatParameters(Dictionary<string, object> parameters)
        {
            if (parameters.Count > 0)
            {
                NSMutableDictionary<NSString, NSObject> eventParameters = new NSMutableDictionary<NSString, NSObject>();
                foreach (var item in parameters)
                {
                    eventParameters.Add((NSString)item.Key, NSObject.FromObject(item.Value != null ? item.Value.ToString() : string.Empty));
                }
                return NSDictionary<NSString, NSObject>.FromObjectsAndKeys(eventParameters.Values.ToArray(), eventParameters.Keys.ToArray(), (System.nint)eventParameters.Count);
            }
            else
            {
                return new NSDictionary<NSString, NSObject>();
            }
        }

        private static NSDictionary<NSString, NSObject> CreateProductParameters(string category, NSMutableArray productsArray)
        {
            IList<NSString> keys;
            IList<NSObject> values;

            if (string.IsNullOrEmpty(category))
            {
                keys = new List<NSString>()
                {
                    new NSString("items")
                };

                values = new List<NSObject>()
                {
                    productsArray
                };
            }
            else
            {
                keys = new List<NSString>()
                {
                    new NSString("items"),
                    ParameterNamesConstants.ItemList
                };

                values = new List<NSObject>()
                {
                    productsArray,
                    new NSString(category)
                };
            }

            NSDictionary<NSString, NSObject> ecommerce = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            return ecommerce;
        }

        private static NSDictionary<NSString, NSObject> CreateCheckoutParameters(NSMutableArray productsArray, string step, string checkoutOption = null)
        {
            IList<NSString> keys;
            IList<NSObject> values;

            if (string.IsNullOrEmpty(checkoutOption))
            {
                keys = new List<NSString>()
                {
                    new NSString("items"),
                    ParameterNamesConstants.CheckoutStep
                };

                values = new List<NSObject>()
                {
                    productsArray,
                    new NSString(step)
                };
            }
            else
            {
                keys = new List<NSString>()
                {
                    new NSString("items"),
                    ParameterNamesConstants.CheckoutStep,
                    ParameterNamesConstants.CheckoutOption
                };

                values = new List<NSObject>()
                {
                    productsArray,
                    new NSString(step),
                    new NSString(checkoutOption)
                };
            }

            NSDictionary<NSString, NSObject> ecommerce = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            return ecommerce;
        }

        private static NSDictionary<NSString, NSObject> CreateSuccessPaymentParameters(NSMutableArray productsArray, Order order, string coupon = "")
        {
            IList<NSString> keys = new List<NSString>()
                {
                    new NSString("items"),
                    ParameterNamesConstants.TransactionId,
                    ParameterNamesConstants.Affiliation,
                    ParameterNamesConstants.Value,
                    ParameterNamesConstants.Tax,
                    ParameterNamesConstants.Shipping,
                    ParameterNamesConstants.Currency,
                    ParameterNamesConstants.Coupon
            };

            IList<NSObject> values = new List<NSObject>()
                {
                    productsArray,
                    new NSString(order.Id),
                    new NSString(AppServiceConfiguration.SiteId),
                    new NSString(order.Total.ToString()),
                    new NSString(order.CountryTax.ToString()),
                    new NSString(order.shippingCost),
                    new NSString("COP"),
                    new NSString(coupon)
                };

            NSDictionary<NSString, NSObject> ecommerce = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            return ecommerce;
        }

        private NSDictionary<NSString, NSString> FormatProduct(Product product, string category, bool sendQuantity = false)
        {
            if (product == null) return null;

            try
            {
                IList<NSString> keys = new List<NSString>()
            {
                ParameterNamesConstants.ItemId,
                ParameterNamesConstants.ItemName,
                ParameterNamesConstants.ItemBrand,
                ParameterNamesConstants.Price,
                ParameterNamesConstants.Currency,
                ParameterNamesConstants.ItemList
            };

                IList<NSString> values = new List<NSString>()
            {
                new NSString(product.Id),
                new NSString(product.Name),
                new NSString(product.Brand),
                new NSString(ModelHelper.GetPrice(product.Price).ToString()),
                new NSString("COP"),
                new NSString(category)
            };

                if (!string.IsNullOrEmpty(category))
                {
                    keys.Add(new NSString(ParameterNamesConstants.ItemCategory));
                    values.Add(new NSString(category));
                }
                else if (!string.IsNullOrEmpty(product.CategoryName))
                {
                    keys.Add(new NSString(ParameterNamesConstants.ItemCategory));
                    values.Add(new NSString(product.CategoryName));
                }

                if (sendQuantity)
                {
                    keys.Add(new NSString(ParameterNamesConstants.Quantity));
                    values.Add(new NSString(product.Quantity.ToString()));
                }

                return NSDictionary<NSString, NSString>.FromObjectsAndKeys(values.ToArray(), keys.ToArray(), keys.Count);
            }
            catch (System.Exception ex)
            {
                Util.LogException(ex, nameof(FirebaseEventRegistrationService), nameof(FormatProduct));
                return new NSDictionary<NSString, NSString>();
            }
        }    
    }
}
