using GrupoExito.Entities;
using GrupoExito.Entities.Entiites.Products;

namespace GrupoExito.Android.Interfaces
{
    public interface IRecommendedAdapter
    {
        void OnItemSelected(ProductList products, int position);
        void OnAddPressed(ProductList product, int position);
        void OnSubstractPressed(ProductList product, int position);
        void OnItemDeleted(ProductList product, int position);

    }
}