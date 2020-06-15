namespace GrupoExito.Utilities.Helpers
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants;
    using GrupoExito.Entities.Parameters.Users;
    using GrupoExito.Entities.Responses.Addresses;
    using GrupoExito.Utilities.Resources;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ModelHelper
    {
        public static decimal GetPrice(Price price)
        {
            return price.PriceOtherMeans > 0 ? price.PriceOtherMeans : price.ActualPrice;
        }

        public static UserContext SetUserContext(User user)
        {
            return new UserContext()
            {
                AcceptTerms = user.AcceptTerms,
                CellPhone = user.CellPhone,
                DateOfBirth = user.DateOfBirth,
                DocumentNumber = user.DocumentNumber,
                DocumentType = user.DocumentType,
                Email = user.Email,
                FirstName = user.FirstName,
                Gender = user.Gender,
                Id = user.Id,
                LastName = user.LastName,
                Prime = user.Prime,
                Points = user.Points,
                ActiveOrder = user.ActiveOrder,
                ActiveOrderCount = user.ActiveOrderCount,
                PaymentMethodPrime = user.PaymentMethodPrime,
                StartDatePrime = user.StartDatePrime,
                EndDatePrime = user.EndDatePrime,
                PeriodicityPrime = user.PeriodicityPrime,
                UserType = user.SegmentClient
            };
        }

        public static UserContext UpdateUserContext(UserContext userContext, User user)
        {
            userContext.AcceptTerms = user.AcceptTerms;
            userContext.CellPhone = user.CellPhone;
            userContext.DateOfBirth = user.DateOfBirth;
            userContext.DocumentNumber = user.DocumentNumber;
            userContext.DocumentType = user.DocumentType;
            userContext.Email = user.Email;
            userContext.FirstName = user.FirstName;
            userContext.Gender = user.Gender;
            userContext.Id = user.Id;
            userContext.LastName = user.LastName;
            userContext.Prime = user.Prime;
            userContext.Points = user.Points;
            userContext.ActiveOrder = user.ActiveOrder;
            userContext.ActiveOrderCount = user.ActiveOrderCount;
            userContext.PaymentMethodPrime = user.PaymentMethodPrime;
            userContext.StartDatePrime = user.StartDatePrime;
            userContext.EndDatePrime = user.EndDatePrime;
            userContext.PeriodicityPrime = user.PeriodicityPrime;
            userContext.UserActivate = user.UserActivate;
            return userContext;
        }

        public static RegisterCostumerParameters RegisterCostumerParameters(UserContext user)
        {
            return new RegisterCostumerParameters()
            {
                CellPhone = user.CellPhone,
                DateOfBirth = user.DateOfBirth,
                DocumentNumber = user.DocumentNumber,
                DocumentType = user.DocumentType,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public static List<Item> GetYears()
        {
            List<Item> itemsYears = new List<Item>();
            int startYear = DateTime.Now.Year - 18;
            int yearEnd = startYear - 80;

            for (int i = startYear; i >= yearEnd; i--)
            {
                itemsYears.Add(new Item() { Text = i.ToString() });
            }

            itemsYears.Insert(0, new Item() { Text = "Año" });

            return itemsYears;
        }

        public static UserContext SetUserEditContext(UserContext BeforeUserContext, User user)
        {
            UserContext userContext = BeforeUserContext;
            userContext.AcceptTerms = user.AcceptTerms;
            userContext.FirstName = user.FirstName;
            userContext.LastName = user.LastName;
            userContext.DateOfBirth = user.DateOfBirth;
            userContext.DocumentType = user.DocumentType;
            userContext.DocumentNumber = user.DocumentNumber;
            userContext.Email = user.Email;
            userContext.CellPhone = user.CellPhone;
            return userContext;
        }

        public static UserAddress SetCoverageInformation(CoverageAddressResponse response, UserAddress userAddress)
        {
            userAddress.DependencyId = response.DependencyId;
            userAddress.Neighborhood = response.Neighborhood;
            userAddress.Location = response.Location;
            userAddress.Longitude = response.Longitude;
            userAddress.Latitude = response.Latitude;
            userAddress.Description = string.IsNullOrEmpty(userAddress.Description) ? ConstAddressType.Home : userAddress.Description;

            return userAddress;
        }

        public static List<string> GetCreditCardQuotes()
        {
            List<string> quotes = new List<string>();

            for (int i = 1; i <= 24; i++)
            {
                quotes.Add(i.ToString());
            }

            quotes.Insert(0, AppMessages.Choose);

            return quotes;
        }

        public static Product SetProduct(Product selectedProduct)
        {
            return new Product()
            {
                Id = selectedProduct.Id,
                SkuId = selectedProduct.SkuId,
                Quantity = selectedProduct.Quantity,
                Note = selectedProduct.Note
            };
        }

        public static string GetUserType(UserContext userContext)
        {
            if (AppServiceConfiguration.SiteId.Equals("carulla"))
            {
                return userContext.UserType != null ? userContext.UserType.Name : string.Empty;
            }
            else
            {
                return userContext.Prime ? "Prime" : "No Prime"; ;
            }
        }

        public static string GetProductsId(IList<Product> products)
        {
            return string.Join(",", products.Select(x => x.Id));
        }

        public static string GetProductsCategories(IList<Product> products)
        {
            return string.Join(",", products.Select(x => x.CategoryName).Distinct());
        }
    }
}
