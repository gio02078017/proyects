using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Android.Adapters
{
    public interface IMyList
    {
        void OnEditItemClicked(ShoppingList shoppingList);

        void OnDelateItemClicked(ShoppingList shoppingList);

        void OnSelectItemClicked(ShoppingList shoppingList);
    }
}