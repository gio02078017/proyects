namespace GrupoExito.Entities.Responses.Products
{
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class ProductSearcherResponse : ProductResponseBase
    {
        public ProductSearcherResponse()
        {
            BrandSuggest = new List<Item>();
            CategorySuggest = new List<Item>();
            NameSuggest = new List<Item>();
        }

        public IList<Item> BrandSuggest { get; set; }
        public IList<Item> CategorySuggest { get; set; }
        public IList<Item> NameSuggest { get; set; }
    }
}
