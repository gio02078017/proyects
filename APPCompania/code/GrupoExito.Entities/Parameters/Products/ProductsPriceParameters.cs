namespace GrupoExito.Entities.Parameters.Products
{
    using System.Collections.Generic;

    public class ProductsPriceParameters
    {
        public ProductsPriceParameters()
        {
            SkuIds = new List<string>();
            ProductsType = new List<string>();
            Pums = new List<string>();
            Factors = new List<string>();
        }

        public string DependencyId { get; set; }
        public List<string> SkuIds { get; set; }
        public List<string> ProductsType { get; set; }
        public List<string> Pums { get; set; }
        public List<string> Factors { get; set; }        
    }
}
