using GrupoExito.Entities;
using GrupoExito.Android.Interfaces;

namespace GrupoExito.Android.Adapters
{
    public interface ISummaryProducts : IProducts
    {
        void OnHoWDoYouLikeTouched(Product product = null, bool error = false, string message = null);
        void OnRemoveTouched(Product product);
    }
}