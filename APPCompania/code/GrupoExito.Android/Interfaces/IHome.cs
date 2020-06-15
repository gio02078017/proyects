using GrupoExito.Entities;

namespace GrupoExito.Android.Interfaces
{
    public interface IHome
    {
        void RefreshProductList(Product ActualProduct);
        void RetryPermmision(bool retry);
        void GoToLobby();
        void CallShowAddressDialog();

    }
        
}