using Android.App;
using Android.OS;
using Firebase.Analytics;
using GrupoExito.Android.Activities.Generic;
using GrupoExito.Android.Utilities;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Contracts.Analytic;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Services
{
    public class FirebaseRegistrationEventsService : IFirebaseEventRegistrationService
    {
        #region Initialization

        private const string Items = "items";
        private const string COP = "COP";

        private FirebaseAnalytics firebaseAnalytics;

        private static FirebaseRegistrationEventsService instance;

        public static FirebaseRegistrationEventsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FirebaseRegistrationEventsService();
                }

                return instance;
            }
        }

        public FirebaseRegistrationEventsService()
        {
            if (firebaseAnalytics == null)
            {
                firebaseAnalytics = FirebaseAnalytics.GetInstance(AndroidApplication.Current);
                firebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                firebaseAnalytics.SetMinimumSessionDuration(20000);
            }
        }

        #endregion

        #region Methods Screen view

        public void RegisterScreen(Activity activity, string screenName, string screenClass)
        {
            firebaseAnalytics.SetCurrentScreen(activity, screenName, screenClass);
        }

        #endregion

        #region Methods  Events     

        public void SignUp()
        {
            var parameters = new Dictionary<string, string>();
            firebaseAnalytics.SetUserProperty(AnalyticsParameter.UserType, ModelHelper.GetUserType(ParametersManager.UserContext));
            firebaseAnalytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
            firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.SignUp, FormatParameters(parameters));
        }

        public void SignIn()
        {
            var parameters = new Dictionary<string, string>();
            firebaseAnalytics.SetUserProperty(AnalyticsParameter.UserType, ModelHelper.GetUserType(ParametersManager.UserContext));
            firebaseAnalytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
            firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.Login, FormatParameters(parameters));
        }

        public void ProductImpression(IList<Product> products, string category)
        {
            if (products != null && products.Any())
            {
                Bundle generic = new Bundle();
                IList<IParcelable> productsList = new List<IParcelable>();

                foreach (var item in products)
                {
                    productsList.Add(FormatProduct(item, "", true));
                }

                generic.PutParcelableArrayList(Items, productsList);
                generic.PutString(FirebaseAnalytics.Param.ItemList, category ?? "");
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.ViewSearchResults, generic);
            }
        }

        public void ProductClic(Product product, string category)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutBundle(Items, FormatProduct(product, category ?? ""));
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.SelectContent, generic);
            }
        }

        public void ProductDetail(Product product, string category)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutBundle(Items, FormatProduct(product, category ?? ""));
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.ViewItem, generic);
            }
        }

        public void AddProductToCart(Product product, string category)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutBundle(Items, FormatProduct(product, category ?? "", true));
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.AddToCart, generic);
            }
        }

        public void DeleteProductFromCart(Product product, string category)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutBundle(Items, FormatProduct(product, category ?? "", true));
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.RemoveFromCart, generic);
            }
        }

        public void DeleteProductsFromCart(IList<Product> products)
        {
            if (products != null && products.Any())
            {
                Bundle generic = new Bundle();
                IList<IParcelable> productsDelete = new List<IParcelable>();

                foreach (var item in ParametersManager.Order.Products)
                {
                    productsDelete.Add(FormatProduct(item, item.CategoryName ?? "", true));
                }

                generic.PutParcelableArrayList(Items, productsDelete);
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.RemoveFromCart, generic);
            }
        }

        public void SummaryPayment()
        {
            if (ParametersManager.Order != null && ParametersManager.Order.Products != null && ParametersManager.Order.Products.Any())
            {
                Bundle generic = new Bundle();
                IList<IParcelable> products = new List<IParcelable>();

                foreach (var item in ParametersManager.Order.Products)
                {
                    products.Add(FormatProduct(item, item.CategoryName ?? "", true));
                }

                generic.PutParcelableArrayList(Items, products);
                generic.PutString(FirebaseAnalytics.Param.CheckoutStep, AnalyticsParameter.StepPayFour);
                generic.PutString(FirebaseAnalytics.Param.CheckoutOption, ParametersManager.Order.TypePayment ?? "");
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.BeginCheckout, generic);
            }
        }

        public void Summary()
        {
            if (ParametersManager.Order != null && ParametersManager.Order.Products != null && ParametersManager.Order.Products.Any())
            {
                Bundle generic = new Bundle();
                IList<IParcelable> products = new List<IParcelable>();

                foreach (var item in ParametersManager.Order.Products)
                {
                    products.Add(FormatProduct(item, item.CategoryName ?? "", true));
                }

                generic.PutParcelableArrayList(Items, products);
                generic.PutString(FirebaseAnalytics.Param.CheckoutStep, AnalyticsParameter.StepPayOne);
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.BeginCheckout, generic);
            }
        }

        public void Schedule()
        {
            if (ParametersManager.Order != null && ParametersManager.Order.Products != null && ParametersManager.Order.Products.Any())
            {
                Bundle generic = new Bundle();
                IList<IParcelable> products = new List<IParcelable>();

                foreach (var item in ParametersManager.Order.Products)
                {
                    products.Add(FormatProduct(item, "", true));
                }

                generic.PutParcelableArrayList(Items, products);
                generic.PutString(FirebaseAnalytics.Param.CheckoutStep, AnalyticsParameter.StepPayTwo);
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.BeginCheckout, generic);
            }
        }

        public void Payment()
        {
            var parameters = new Dictionary<string, string>
            {
                { FirebaseAnalytics.Param.CheckoutOption, ParametersManager.Order.TypePayment },
                { FirebaseAnalytics.Param.CheckoutStep, AnalyticsParameter.StepPayThree}
            };

            firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.BeginCheckout, FormatParameters(parameters));
        }

        public void SuccessPayment(Order order)
        {
            if (order != null && order.Products != null && order.Products.Any())
            {
                Bundle generic = new Bundle();
                IList<IParcelable> products = new List<IParcelable>();

                foreach (var item in order.Products)
                {
                    products.Add(FormatProduct(item, "", true));
                }

                generic.PutParcelableArrayList(Items, products);
                generic.PutString(FirebaseAnalytics.Param.TransactionId, order.Id);
                generic.PutString(FirebaseAnalytics.Param.Value, order.Total.ToString());
                generic.PutString(FirebaseAnalytics.Param.Tax, order.CountryTax.ToString());
                generic.PutString(FirebaseAnalytics.Param.Shipping, order.shippingCost);
                generic.PutString(FirebaseAnalytics.Param.Currency, COP);
                firebaseAnalytics.LogEvent(FirebaseAnalytics.Event.EcommercePurchase, generic);
            }
        }

        public void ActivatedDiscount(Discount discount)
        {
            if (discount != null)
            {
                var parameters = new Dictionary<string, string>
                {
                    { AnalyticsParameter.Description, discount.Description },
                    { AnalyticsParameter.PLU, discount.Plu },
                    { AnalyticsParameter.Percentage, discount.DiscountType },
                    { AnalyticsParameter.UserId, CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey) }
                };

                firebaseAnalytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
                firebaseAnalytics.LogEvent(AnalyticsEvent.ActivateDiscount, FormatParameters(parameters));
            }
        }

        public void InactivateDiscount(Discount discount)
        {
            if (discount != null)
            {
                var parameters = new Dictionary<string, string>
                {
                    { AnalyticsParameter.Description, discount.Description },
                    { AnalyticsParameter.PLU, discount.Plu },
                    { AnalyticsParameter.Percentage, discount.DiscountType },
                    { AnalyticsParameter.UserId, CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey) }
                };

                firebaseAnalytics.SetUserId(CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
                firebaseAnalytics.LogEvent(AnalyticsEvent.InactivateDiscount, FormatParameters(parameters));
            }
        }

        #endregion

        #region Format parameters

        private Bundle FormatParameters(Dictionary<string, string> parameters)
        {
            Bundle bundle = new Bundle();

            foreach (var item in parameters)
            {
                bundle.PutString(item.Key, item.Value);
            }

            return bundle;
        }

        private Bundle FormatProduct(Product product, string category, bool sendQuantity = false)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(FirebaseAnalytics.Param.ItemId, product.Id);
            bundle.PutString(FirebaseAnalytics.Param.ItemName, product.Name);
            bundle.PutString(FirebaseAnalytics.Param.ItemBrand, product.Brand);
            bundle.PutString(FirebaseAnalytics.Param.Price, ModelHelper.GetPrice(product.Price).ToString());
            bundle.PutString(FirebaseAnalytics.Param.Currency, COP);
            bundle.PutString(FirebaseAnalytics.Param.ItemList, category);

            if (!string.IsNullOrEmpty(category))
            {
                bundle.PutString(FirebaseAnalytics.Param.ItemCategory, category);
            }
            else if (!string.IsNullOrEmpty(product.CategoryName))
            {
                bundle.PutString(FirebaseAnalytics.Param.ItemCategory, product.CategoryName);
            }

            if (sendQuantity)
            {
                bundle.PutString(FirebaseAnalytics.Param.Quantity, product.Quantity.ToString());
            }

            return bundle;
        }

        #endregion
    }
}