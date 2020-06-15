using GrupoExito.Entities;
using GrupoExito.Entities.Entiites.Generic;
using System.Collections.Generic;

namespace GrupoExito.Utilities.Contracts.Products
{
    public interface IProductCarDataBase
    {
        void UpSertProduct(Product product);
        Dictionary<string, object> UpSertProduct(Product product, bool add);
        Product GetProduct(string productId);
        List<Product> GetProducts();
        void DeleteProduct(string productId);
        Dictionary<string, object> GetSummary();
        Dictionary<string, object> RecalculateSummary();
        void FlushCar();
        void UpSertTaxBag(IList<TaxBag> taxesBag);
    }
}
