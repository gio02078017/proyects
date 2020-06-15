using GrupoExito.Entities.Entiites;

namespace GrupoExito.Android.Adapters
{
    public interface IDiscount
    {
        void OnSelectItemClicked(Discount discount);
        void OnInactivatedClicked(Discount discount);
        void OnTermsClicked(Discount discount);
    }
}