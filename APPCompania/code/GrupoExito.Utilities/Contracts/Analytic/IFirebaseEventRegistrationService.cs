using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Utilities.Contracts.Analytic
{
    public interface IFirebaseEventRegistrationService
    {
        void SignIn();
        void SignUp();
        void ProductImpression(IList<Product> products, string category);
        void ProductClic(Product product, string category);
        void ProductDetail(Product product, string category);
        void AddProductToCart(Product product, string category);
        void DeleteProductFromCart(Product product, string category);
        void Summary();
        void Schedule();
        void Payment();
        void SuccessPayment(Order order);
        void SummaryPayment();
        void ActivatedDiscount(Discount discount);
        void InactivateDiscount(Discount discount);
    }
}
