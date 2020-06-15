namespace GrupoExito.Utilities.Contracts.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Products;
    using System.Threading.Tasks;

    public interface IShoppingListService
    {
        Task<SuggestedProductsResponse> GetSuggestedProducts(ProductParameters parameters);
        Task<ShoppingListsResponse> GetShoppingLists();
        Task<ShoppingListsResponse> AddShoppingList(ShoppingList shoppingList);
        Task<ResponseBase> UpdateShoppingList(ShoppingList shoppingList);
        Task<ResponseBase> DeleteShoppingList(string shoppingListId);
        Task<ResponseBase> AddProductShoppingList(Product product);
        Task<ResponseBase> DeleteProductShoppingList(string productId, string shoppingListId);
        Task<SuggestedProductsResponse> GetProductsShoppingList(string shoppingListId);
        Task<ResponseBase> UpdateQuantityItemList(ShoppingList shoppingList);
    }
}
