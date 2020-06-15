namespace GrupoExito.DataAgent.DataBase
{
    using Couchbase.Lite;
    using Couchbase.Lite.Query;
    using GrupoExito.Entities;
    using GrupoExito.Entities.Constants.Products;
    using GrupoExito.Entities.Entiites.Generic;
    using GrupoExito.Utilities.Contracts.Products;
    using GrupoExito.Utilities.Helpers;
    using System;
    using System.Collections.Generic;

    public class ProductCarDataBase : IProductCarDataBase, IDisposable
    {
        #region Attributes

        private Database database;

        private static ProductCarDataBase instance;

        public static ProductCarDataBase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProductCarDataBase();
                }

                return instance;
            }
        }

        #endregion

        #region Constructor

        public ProductCarDataBase()
        {
            if (database == null)
            {
                database = new Database(ConstDataBase.Car);
            }
        }

        #endregion

        #region Methods CRUDS

        public Product GetProduct(string productId)
        {
            var document = database.GetDocument(productId);

            if (document != null)
            {
                return JsonService.Deserialize<Product>(document.GetString(ConstDataBase.Product));
            }
            return null;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();

            using (var query = QueryBuilder.Select(SelectResult.All()).From(DataSource.Database(database)))
            {
                foreach (Result item in query.Execute())
                {
                    var dict = item.GetDictionary(database.Name);

                    string value = dict?.GetString(ConstDataBase.Product);

                    if (!string.IsNullOrEmpty(value))
                    {
                        products.Add(JsonService.Deserialize<Product>(value));
                    }
                }
            }

            return products;
        }

        public Dictionary<string, object> GetSummary()
        {
            var document = database.GetDocument(ConstDataBase.TotalCar);

            if (document != null)
            {
                return document.GetDictionary(ConstDataBase.Summary).ToDictionary();
            }

            return null;
        }

        public void DeleteProduct(string productId)
        {
            var document = database.GetDocument(productId);
            document = document ?? new MutableDocument(productId);

            using (var mutableDoc = document.ToMutable())
            {
                database.Delete(mutableDoc);
            }
        }

        private string GetTotalPrice(Product product, bool add, string totalPrice)
        {
            decimal productPrice = GetCurrentPrice(product);

            if (string.IsNullOrEmpty(totalPrice))
            {
                return productPrice.ToString();
            }
            else
            {
                decimal totalConverted = decimal.Parse(totalPrice);
                return add ? (totalConverted + productPrice).ToString() : (totalConverted - productPrice).ToString();
            }
        }

        private decimal GetCurrentPrice(Product product)
        {
            return product.Price.PriceOtherMeans > 0 ? product.Price.PriceOtherMeans : product.Price.ActualPrice;
        }

        #endregion

        #region Methods Change price, change dependency , recalculate summary

        public void UpSertProduct(Product product)
        {
            var document = database.GetDocument(product.Id);
            document = document ?? new MutableDocument(product.Id);
            using (var mutableDoc = document.ToMutable())
            {
                if (product.Quantity == 0)
                {
                    DeleteProduct(product.Id);
                }
                else
                {
                    mutableDoc.SetString(ConstDataBase.Product, JsonService.Serialize<Product>(product));
                    database.Save(mutableDoc);
                }
            }
        }

        public Dictionary<string, object> RecalculateSummary()
        {
            MutableDictionaryObject recalculatedSummary = new MutableDictionaryObject();
            decimal totalPrice = 0;
            int productQuantity = 0;

            using (var query = QueryBuilder.Select(SelectResult.All()).From(DataSource.Database(database)))
            {
                foreach (Result item in query.Execute())
                {
                    var dict = item.GetDictionary(database.Name);

                    string value = dict?.GetString(ConstDataBase.Product);

                    if (!string.IsNullOrEmpty(value))
                    {
                        Product product = JsonService.Deserialize<Product>(value);
                        totalPrice += GetCurrentPrice(product) * product.Quantity;
                        productQuantity += product.Quantity;
                    }
                }

                recalculatedSummary.SetInt(ConstDataBase.ProductQuantity, productQuantity);
                recalculatedSummary.SetDouble(ConstDataBase.TotalTaxBag, GetTotalTaxBag(productQuantity));
                recalculatedSummary.SetString(ConstDataBase.TotalPrice, totalPrice.ToString());
                UpdateLocalSummary(recalculatedSummary);
                return recalculatedSummary.ToDictionary();
            }
        }

        private void UpdateLocalSummary(MutableDictionaryObject summary)
        {
            var document = database.GetDocument(ConstDataBase.TotalCar);
            document = document ?? new MutableDocument(ConstDataBase.TotalCar);

            using (var mutableDoc = document.ToMutable())
            {
                MutableDictionaryObject localSummary = mutableDoc.GetDictionary(ConstDataBase.Summary);

                if (localSummary == null)
                {
                    localSummary = new MutableDictionaryObject();
                }

                localSummary.SetDouble(ConstDataBase.TotalTaxBag, summary.GetDouble(ConstDataBase.TotalTaxBag));
                localSummary.SetInt(ConstDataBase.ProductQuantity, summary.GetInt(ConstDataBase.ProductQuantity));
                localSummary.SetString(ConstDataBase.TotalPrice, summary.GetString(ConstDataBase.TotalPrice));
                mutableDoc.SetDictionary(ConstDataBase.Summary, localSummary);
                database.Save(mutableDoc);
            }
        }

        #endregion

        #region Methods Add , Substract, Calculate summary 

        public Dictionary<string, object> UpSertProduct(Product product, bool add)
        {
            var document = database.GetDocument(product.Id);
            document = document ?? new MutableDocument(product.Id);

            using (var mutableDoc = document.ToMutable())
            {
                product.Quantity = add ? product.Quantity + 1 : product.Quantity - 1;

                if (product.Quantity == 0)
                {
                    DeleteProduct(product.Id);
                }
                else
                {
                    mutableDoc.SetString(ConstDataBase.Product, JsonService.Serialize<Product>(product));
                    database.Save(mutableDoc);
                }
            }
            return UpsertSummary(product, add);
        }

        private Dictionary<string, object> UpsertSummary(Product product, bool add)
        {
            var document = database.GetDocument(ConstDataBase.TotalCar);
            document = document ?? new MutableDocument(ConstDataBase.TotalCar);

            using (var mutableDoc = document.ToMutable())
            {
                int quantity = 0;
                string totalPrice = string.Empty;
                MutableDictionaryObject summary = mutableDoc.GetDictionary(ConstDataBase.Summary);

                if (summary != null)
                {
                    quantity = summary.GetInt(ConstDataBase.ProductQuantity);
                    totalPrice = summary.GetString(ConstDataBase.TotalPrice);
                }
                else
                {
                    summary = new MutableDictionaryObject();
                }

                quantity = add ? quantity + 1 : quantity - 1;
                summary.SetInt(ConstDataBase.ProductQuantity, quantity);
                summary.SetDouble(ConstDataBase.TotalTaxBag, GetTotalTaxBag(quantity));
                summary.SetString(ConstDataBase.TotalPrice, GetTotalPrice(product, add, totalPrice));
                mutableDoc.SetDictionary(ConstDataBase.Summary, summary);
                database.Save(mutableDoc);

                return summary.ToDictionary();
            }
        }

        private double GetTotalTaxBag(int quantity)
        {
            double tax = 0;

            if (quantity > 0)
            {
                List<TaxBag> Taxbaglist = GetTaxesBag();
                TaxBag response = Taxbaglist.Find(c => c.MaxItems >= quantity);
                tax = response != null ? response.Tax : 0;
            }

            return tax;
        }

        #endregion

        #region TaxBag

        public void UpSertTaxBag(IList<TaxBag> taxesBag)
        {
            var document = database.GetDocument(ConstDataBase.TaxBag);
            document = document ?? new MutableDocument(ConstDataBase.TaxBag);

            using (var mutableDoc = document.ToMutable())
            {
                mutableDoc.SetString(ConstDataBase.TaxBag, JsonService.Serialize(taxesBag));
                database.Save(mutableDoc);
            }
        }

        private List<TaxBag> GetTaxesBag()
        {
            List<TaxBag> taxesBag = new List<TaxBag>();
            var document = database.GetDocument(ConstDataBase.TaxBag);

            if (document != null)
            {
                taxesBag = JsonService.Deserialize<List<TaxBag>>(document.GetString(ConstDataBase.TaxBag));
            }

            return taxesBag;
        }

        #endregion

        #region Dispose Database

        public void FlushCar()
        {
            List<Product> products = GetProducts();

            foreach (Product product in products)
            {
                DeleteProduct(product.Id);
            }

            var documentSummary = database.GetDocument(ConstDataBase.TotalCar);

            if (documentSummary != null)
            {
                database.Delete(documentSummary);
            }
        }

        public void Dispose()
        {
            if (database != null)
            {
                database.Dispose();
            }
        }

        #endregion
    }
}
