using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Products;

namespace GrupoExito.Android.Interfaces
{
    public interface IGetSoats
    {
        void OnItemSelected(Soat soat, int position);
        void OnItemDeleted(Soat soat, int position);

    }
}