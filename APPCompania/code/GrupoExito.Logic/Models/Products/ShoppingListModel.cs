namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Entiites.Products;
    using GrupoExito.Entities.Responses.Base;
    using GrupoExito.Entities.Responses.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using GrupoExito.Utilities.Resources;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ShoppingListModel
    {
        private IShoppingListService _shpoppingListService { get; set; }

        public ShoppingListModel(IShoppingListService shpoppingListService)
        {
            _shpoppingListService = shpoppingListService;
        }

        public async Task<SuggestedProductsResponse> GetSuggestedProducts(ProductParameters parameters)
        {
            return await _shpoppingListService.GetSuggestedProducts(parameters);
        }

        public async Task<ShoppingListsResponse> GetShoppingLists()
        {
            return await _shpoppingListService.GetShoppingLists();
        }

        public async Task<ShoppingListsResponse> AddShoppingList(ShoppingList shoppingList)
        {
            return await _shpoppingListService.AddShoppingList(shoppingList);
        }

        public async Task<ResponseBase> UpdateShoppingList(ShoppingList shoppingList)
        {
            return await _shpoppingListService.UpdateShoppingList(shoppingList);
        }

        public async Task<ResponseBase> DeleteShoppingList(string shoppingListId)
        {
            return await _shpoppingListService.DeleteShoppingList(shoppingListId);
        }

        public async Task<ResponseBase> AddProductShoppingList(Product product)
        {
            return await _shpoppingListService.AddProductShoppingList(product);
        }

        public async Task<ResponseBase> DeleteProductShoppingList(string productId, string shoppingListId)
        {
            return await _shpoppingListService.DeleteProductShoppingList(productId, shoppingListId);
        }

        public async Task<SuggestedProductsResponse> GetProductsShoppingList(string shoppingListId)
        {
            return await _shpoppingListService.GetProductsShoppingList(shoppingListId);
        }

        public string GetSelectedProductsQuantity(IList<ProductList> productsClient)
        {
            return GetSelectedProducts(productsClient) != null ? GetSelectedProducts(productsClient).Count().ToString() : "0";
        }

        public string GetProductTotal(IList<ProductList> productsClient)
        {
            decimal total = 0;
            var products = GetSelectedProducts(productsClient);
            foreach(var product in products)
            {
                total += product.Price.ActualPrice * product.Quantity;
            }

            return StringFormat.ToPrice(total);
        }

        public void SelectAll(IList<ProductList> productsClient, bool @checked)
        {
            foreach (var product in productsClient)
            {
                product.Selected = @checked;

                if (@checked)
                {
                    product.Quantity = product.Quantity < 1 ? 1 : product.Quantity;
                }
                else
                {
                    product.Quantity = 0;
                }
            }
        }

        public async Task<ResponseBase> UpdateQuantityItemList(ShoppingList shoppingList)
        {
            return await _shpoppingListService.UpdateQuantityItemList(shoppingList);
        }

        private List<ProductList> GetSelectedProducts(IList<ProductList> productsClient)
        {
            return productsClient.Where(product => product.Selected).ToList();
        }

        public string ValidateFields(ShoppingList shoppingList)
        {
            if (string.IsNullOrEmpty(shoppingList.Name) )
            {
                return AppMessages.RequiredFieldName;
            }
            ////else
            ////{
            ////    Regex regexString = new Regex(@"^[a-zA-ZÀ-ÿ\u00f1\u00d1]+(\s*[a-zA-ZÀ-ÿ\u00f1\u00d1]*)*[a-zA-ZÀ-ÿ\u00f1\u00d1]+$");

            ////    if (!regexString.IsMatch(shoppingList.Name.ToUpper().TrimStart().TrimEnd()))
            ////    {
            ////        return string.Format(AppMessages.SpecialCharactersMessage, AppMessages.NameText);
            ////    }                
            ////}

            return string.Empty;
        }
    }
}
