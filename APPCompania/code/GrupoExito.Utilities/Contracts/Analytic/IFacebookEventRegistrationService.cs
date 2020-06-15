using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Utilities.Contracts.Analytic
{
    public interface IFacebookEventRegistrationService
    {
        void InactivateDiscount(Discount discount);
        void ActivatedDiscount(Discount discount);
        void AddPaymentInfo(bool success);
        void AddProductToCart(Product product);
        void AddToWishlist(Product product);
        void CompleteRegistration(string registrationMethod);
        void Searched(string query, bool success, IList<Product> products);
        void ViewedContent(Product product);
        void InitiatedCheckout(bool paymentInfoAvailable, IList<Product> products);
        void Purchased(double purchaseAmount, IList<Product> products);
    }
}
