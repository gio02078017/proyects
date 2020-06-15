using System.Collections.Generic;
using System.Linq;
using Facebook.CoreKit;
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
    public class FacebookEventRegistrationService : IFacebookEventRegistrationService
    {
        #region Attributes

        private static FacebookEventRegistrationService instance;

        #endregion

        #region Properties
        public static FacebookEventRegistrationService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FacebookEventRegistrationService();
                }

                return instance;
            }
        }

        #endregion

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

                AppEvents.LogEvent(AnalyticsEvent.InactivateDiscount, FormatParameters(parameters));
            }
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

                AppEvents.LogEvent(AnalyticsEvent.ActivateDiscount, FormatParameters(parameters));
            }
        }

        public void AddPaymentInfo(bool success)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.Success, success ? 1 : 0 }
            };

            AppEvents.LogEvent(AppEventName.AddedPaymentInfo, FormatParameters(parameters));
        }

        public void AddProductToCart(Product product)
        {
            //NSMutableArray productsArray = new NSMutableArray();
            //if (ParametersManager.Order != null)
            //{
            //    foreach (var item in ParametersManager.Order.Products)
            //    {
            //        var productDict = FormatProduct(product);
            //        productsArray.Add(productDict);
            //    }
            //}

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.Content, FormatProduct(product) },
                { AppEventParameterName.ContentID, product.Id },
                { AppEventParameterName.ContentType, product.CategoryName ?? "" },
                { AppEventParameterName.Currency, "COP" }
            };

            AppEvents.LogEvent(AppEventName.AddedToCart, (double)product.Price.ActualPrice, FormatParameters(parameters));
        }

        public void AddToWishlist(Product product)
        {
            NSMutableArray productsArray = new NSMutableArray();
            if (ParametersManager.Order != null)
            {
                foreach (var item in ParametersManager.Order.Products)
                {
                    var productDict = FormatProduct(product);
                    productsArray.Add(productDict);
                }
            }

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.Content, FormatProduct(product) },
                { AppEventParameterName.ContentID, product.Id },
                { AppEventParameterName.ContentType, product.CategoryName ?? "" },
                { AppEventParameterName.Currency, "COP" }
            };

            AppEvents.LogEvent(AppEventName.AddedToWishlist, (double)product.Price.ActualPrice, FormatParameters(parameters));
        }

        public void CompleteRegistration(string registrationMethod)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.RegistrationMethod, registrationMethod }
            };

            AppEvents.LogEvent(AppEventName.CompletedRegistration, FormatParameters(parameters));
        }

        public void Searched(string query, bool success, IList<Product> products)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.ContentType, ModelHelper.GetProductsCategories(products) },
                { AppEventParameterName.SearchString, query },
                { AppEventParameterName.Success, success ? 1 : 0 }
            };

            AppEvents.LogEvent(AppEventName.Searched, FormatParameters(parameters));
        }

        public void ViewedContent(Product product)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.ContentType, product.CategoryName ?? "" },
                { AppEventParameterName.ContentID, product.Id },
                { AppEventParameterName.Currency, "COP" }
            };

            AppEvents.LogEvent(AppEventName.ViewedContent, (double)product.Price.ActualPrice, FormatParameters(parameters));
        }

        public void InitiatedCheckout(bool paymentInfoAvailable, IList<Product> products)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.ContentType, ModelHelper.GetProductsCategories(products) },
                { AppEventParameterName.ContentID, ModelHelper.GetProductsId(products) },
                { AppEventParameterName.Currency, "COP" },
                { AppEventParameterName.NumItems, ParametersManager.Order != null ?  ParametersManager.Order.Products.Count : 0 },
                { AppEventParameterName.PaymentInfoAvailable, paymentInfoAvailable ? 1 : 0 }
            };

            AppEvents.LogEvent(AppEventName.InitiatedCheckout, (double)ParametersManager.Order.SubTotal, FormatParameters(parameters));
        }

        public void Purchased(double purchaseAmount, IList<Product> products)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { AppEventParameterName.ContentType, ModelHelper.GetProductsCategories(products) },
                { AppEventParameterName.ContentID, ModelHelper.GetProductsId(products) },
                { AppEventParameterName.NumItems,ParametersManager.Order != null ?  ParametersManager.Order.Products.Count : 0},
            };

            AppEvents.LogPurchase(purchaseAmount, "COP", FormatParameters(parameters));
        }

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

        private NSDictionary<NSString, NSString> FormatProduct(Product product)
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
                ParameterNamesConstants.ItemList,
                ParameterNamesConstants.Quantity
            };

                IList<NSString> values = new List<NSString>()
            {
                new NSString(product.Id),
                new NSString(product.Name),
                new NSString(product.Brand),
                new NSString(ModelHelper.GetPrice(product.Price).ToString()),
                new NSString("COP"),
                new NSString(product.CategoryName ?? ""),
                new NSString(product.Quantity.ToString())
            };
            
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
