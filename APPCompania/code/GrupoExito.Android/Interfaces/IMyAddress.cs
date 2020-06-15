using GrupoExito.Entities;

namespace GrupoExito.Android.Adapters
{
    public interface IMyAddress
    {
        void OnEditItemClicked(UserAddress userAddress);

        void OnDelateItemClicked(UserAddress userAddress);

        void OnSelectDefaultItemClicked(UserAddress userAddress);
    }
}