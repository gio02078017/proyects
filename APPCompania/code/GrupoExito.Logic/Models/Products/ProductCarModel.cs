namespace GrupoExito.Logic.Models.Products
{
    using GrupoExito.Entities;
    using GrupoExito.Entities.Entiites;
    using GrupoExito.Entities.Entiites.Generic;
    using GrupoExito.Entities.Entiites.Products;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductCarModel
    {
        private IProductCarDataBase _productCar { get; set; }

        public ProductCarModel(IProductCarDataBase productCar)
        {
            _productCar = productCar;
        }

        public void UpSertProduct(Product product)
        {
            _productCar.UpSertProduct(product);
        }

        public void UpsertProducts(List<ProductList> products)
        {
            foreach (var product in products)
            {
                _productCar.UpSertProduct(product);
            }
        }

        public void UpsertProducts(List<Product> products)
        {
            foreach (var product in products)
            {
                _productCar.UpSertProduct(product);
            }
        }

        public Dictionary<string, object> UpSertProduct(Product product, bool add)
        {
            return _productCar.UpSertProduct(product, add);
        }

        public Product GetProduct(string productId)
        {
            return _productCar.GetProduct(productId);
        }

        public decimal GetTotalValueProduct(string productId)
        {
            Product product = _productCar.GetProduct(productId);
            return product.Quantity * ModelHelper.GetPrice(product.Price);           
        }

        public List<Product> GetProducts()
        {
            return _productCar.GetProducts();
        }

        public void DeleteProduct(string productId)
        {
            _productCar.DeleteProduct(productId);
        }

        public Dictionary<string, object> GetSummary()
        {
            return _productCar.GetSummary();
        }

        public void FlushCar()
        {
            _productCar.FlushCar();
        }

        public Dictionary<string, object> RecalculateSummary()
        {
            return _productCar.RecalculateSummary();
        }

        public void UpSertTaxBag(IList<TaxBag> taxesBag)
        {
            _productCar.UpSertTaxBag(taxesBag);
        }

        public List<Product> UpdateProductsPrice(List<Price> Prices, List<Product> products)
        {
            List<Product> productsDeleted = new List<Product>();
            if (products.Any())
            {
                Price price = null;
                foreach (var item in products)
                {
                    price = Prices.Find(x => x.SkuId.Equals(item.SkuId));

                    if (price != null)
                    {
                        item.Price = price;
                        UpSertProduct(item);
                    }
                    else
                    {
                        productsDeleted.Add(item);
                        DeleteProduct(item.Id);
                    }
                }
            }
            return productsDeleted;
        }

        public void UpdateProductStockOut(IList<SoldOut> ListStockOut)
        {
            List<Product> products = GetProducts();
            List<Product> productsDeleted = new List<Product>();
            Product product = null;

            foreach (var item in ListStockOut)
            {
                product = products.Find(x => x.SkuId.Equals(item.SkuId));

                if (product != null)
                {
                    product.Quantity = item.Quantity;
                    UpSertProduct(product);
                }
            }
        }

        public void DeleteProductSouldOut(IList<SoldOut> listSoldOut)
        {
            foreach (var item in listSoldOut)
            {
                if (GetProduct(item.Id) != null)
                {
                    DeleteProduct(item.Id);
                }
            }
        }
    }
}

