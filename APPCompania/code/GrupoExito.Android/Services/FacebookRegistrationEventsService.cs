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
using Java.Math;
using Java.Util;
using System.Collections.Generic;
using Xamarin.Facebook.AppEvents;

namespace GrupoExito.Android.Services
{
    public class FacebookRegistrationEventsService : IFacebookEventRegistrationService
    {
        #region Initialization

        private AppEventsLogger logger;
        private const string Items = "items";
        private const string COP = "COP";

        private static FacebookRegistrationEventsService instance;

        public static FacebookRegistrationEventsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FacebookRegistrationEventsService();
                }

                return instance;
            }
        }

        public FacebookRegistrationEventsService()
        {
            if (logger == null)
            {
                logger = AppEventsLogger.NewLogger(AndroidApplication.Current);
            }
        }

        #endregion

        #region Methods

        public void ActivatedDiscount(Discount discount)
        {
            if (discount != null)
            {
                Bundle bundle = new Bundle();
                bundle.PutString(AnalyticsParameter.Description, discount.Description);
                bundle.PutString(AnalyticsParameter.PLU, discount.Plu);
                bundle.PutString(AnalyticsParameter.Percentage, discount.DiscountType);
                bundle.PutString(AnalyticsParameter.UserId, CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
                logger.LogEvent(AnalyticsEvent.ActivateDiscount, bundle);
            }
        }

        public void InactivateDiscount(Discount discount)
        {
            if (discount != null)
            {
                Bundle bundle = new Bundle();
                bundle.PutString(AnalyticsParameter.Description, discount.Description);
                bundle.PutString(AnalyticsParameter.PLU, discount.Plu);
                bundle.PutString(AnalyticsParameter.Percentage, discount.DiscountType);
                bundle.PutString(AnalyticsParameter.UserId, CryptoHelper.Encrypt(ParametersManager.UserContext.DocumentNumber, AppConfigurations.EncrypKey));
                logger.LogEvent(AnalyticsEvent.InactivateDiscount, bundle);
            }
        }

        public void AddPaymentInfo(bool success)
        {
            Bundle bundle = new Bundle();
            bundle.PutBoolean(AppEventsConstants.EventParamSuccess, success);
            logger.LogEvent(AppEventsConstants.EventNameAddedPaymentInfo, bundle);
        }

        public void AddProductToCart(Product product)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutBundle(AppEventsConstants.EventParamContent, FormatProduct(product));
                generic.PutString(AppEventsConstants.EventParamContentId, product.Id);
                generic.PutString(AppEventsConstants.EventParamContentType, product.CategoryName ?? "");
                generic.PutString(AppEventsConstants.EventParamCurrency, COP);
                logger.LogEvent(AppEventsConstants.EventNameAddedToCart, (double)product.Price.ActualPrice, generic);
            }
        }

        public void AddToWishlist(Product product)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutBundle(AppEventsConstants.EventParamContent, FormatProduct(product));
                generic.PutString(AppEventsConstants.EventParamContentId, product.Id);
                generic.PutString(AppEventsConstants.EventParamContentType, product.CategoryName ?? "");
                generic.PutString(AppEventsConstants.EventParamCurrency, COP);
                logger.LogEvent(AppEventsConstants.EventNameAddedToWishlist, (double)product.Price.ActualPrice, generic);
            }
        }

        public void CompleteRegistration(string registrationMethod)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(AppEventsConstants.EventParamRegistrationMethod, registrationMethod);
            logger.LogEvent(AppEventsConstants.EventNameCompletedRegistration, bundle);
        }

        public void Searched(string query, bool success, IList<Product> products)
        {
            Bundle generic = new Bundle();
            generic.PutString(AppEventsConstants.EventParamContentType, ModelHelper.GetProductsCategories(products));
            generic.PutString(AppEventsConstants.EventParamSearchString, query);
            generic.PutBoolean(AppEventsConstants.EventParamSuccess, success);
            logger.LogEvent(AppEventsConstants.EventNameSearched, generic);
        }

        public void ViewedContent(Product product)
        {
            if (product != null)
            {
                Bundle generic = new Bundle();
                generic.PutString(AppEventsConstants.EventParamContentId, product.Id);
                generic.PutString(AppEventsConstants.EventParamContentType, product.CategoryName ?? "");
                generic.PutString(AppEventsConstants.EventParamCurrency, COP);
                logger.LogEvent(AppEventsConstants.EventNameViewedContent, (double)product.Price.ActualPrice, generic);
            }
        }

        public void InitiatedCheckout(bool paymentInfoAvailable, IList<Product> products)
        {
            Bundle generic = new Bundle();
            generic.PutString(AppEventsConstants.EventParamContentType, ModelHelper.GetProductsCategories(products));
            generic.PutString(AppEventsConstants.EventParamContentId, ModelHelper.GetProductsId(products));
            generic.PutString(AppEventsConstants.EventParamCurrency, COP);
            generic.PutInt(AppEventsConstants.EventParamNumItems, ParametersManager.Order.Products.Count);
            generic.PutBoolean(AppEventsConstants.EventParamPaymentInfoAvailable, paymentInfoAvailable);
            logger.LogEvent(AppEventsConstants.EventNameInitiatedCheckout, (double)ParametersManager.Order.SubTotal, generic);
        }

        public void Purchased(double purchaseAmount, IList<Product> products)
        {
            Bundle generic = new Bundle();
            generic.PutString(AppEventsConstants.EventParamContentType, ModelHelper.GetProductsCategories(products));
            generic.PutString(AppEventsConstants.EventParamContentId, ModelHelper.GetProductsId(products));
            generic.PutInt(AppEventsConstants.EventParamNumItems, ParametersManager.Order.Products.Count);
            generic.PutString(AppEventsConstants.EventParamPaymentInfoAvailable, COP);
            logger.LogPurchase(BigDecimal.ValueOf(purchaseAmount), Currency.GetInstance("COP"), generic);
        }

        #endregion

        #region Format parameters

        private Bundle FormatProduct(Product product)
        {
            Bundle bundle = new Bundle();
            bundle.PutString(FirebaseAnalytics.Param.ItemId, product.Id);
            bundle.PutString(FirebaseAnalytics.Param.ItemName, product.Name);
            bundle.PutString(FirebaseAnalytics.Param.ItemBrand, product.Brand);
            bundle.PutString(FirebaseAnalytics.Param.ItemCategory, product.CategoryName ?? "");
            bundle.PutString(FirebaseAnalytics.Param.Quantity, product.Quantity.ToString());
            bundle.PutString(FirebaseAnalytics.Param.Currency, COP);
            bundle.PutString(FirebaseAnalytics.Param.Price, ModelHelper.GetPrice(product.Price).ToString());
            return bundle;
        }

        #endregion
    }
}