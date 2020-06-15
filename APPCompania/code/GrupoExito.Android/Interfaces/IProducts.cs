using GrupoExito.Entities;

namespace GrupoExito.Android.Interfaces
{
    public interface IProducts
    {
        void OnProductClicked(Product product);
        void OnAddPressed(Product product);
        void OnSubstractPressed(Product product);
        void OnAddProduct(Product product);

        void OnAddToListClicked(Product product);
    }
}