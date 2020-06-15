using System;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class ProductViewModel : BaseViewModel
    {
        private Product product { get; }

        public string ProductId { get { return product.Id; } }

        private string productDeletedId;
        public string ProductDeletedId
        {
            get { return productDeletedId; }
            set { SetProperty(ref productDeletedId, value); }
        }

        private Product howDoYouLikeItProduct;
        public Product HowDoYouLikeItProduct
        {
            get { return howDoYouLikeItProduct; }
            set { SetProperty(ref howDoYouLikeItProduct, value); }
        }

        private Product productUpdated;
        public Product ProductUpdated
        {
            get { return productUpdated; }
            set { SetProperty(ref productUpdated, value); }
        }

        private Product productToBeDeleted;
        public Product ProductToBeDeleted
        {
            get { return productToBeDeleted; }
            set { SetProperty(ref productToBeDeleted, value); }
        }

        private bool isAdd;
        public bool IsAdd { get => isAdd; set => isAdd = value; }

        public Command UpdateQuantityCommand { get; set; }
        public Command RequestDeleteCommand { get; set; }
        public Command DeleteCommand { get; set; }
        public Command HowDoYouLikeItCommand { get; set; }
        public Command UpdateCommand { get; set; }

        public string Name { get { return product.Name; } }
        public string Category { get { return product.CategoryName; } }
        public string CategoryImage { get { return product.CategoryImage; } }
        public string CategoryId { get { return product.CategoryId; } }
        public Price Price { get { return product.Price; } }
        public int Quantity { get { return product.Quantity; } }
        public string ImageUrl { get { return product.UrlMediumImage; } }
        public bool IsEstimatedWeight { get { return product.IsEstimatedWeight; } }
        public bool WeightSelected { get { return product.WeightSelected; } set { product.WeightSelected = value; } }
        public decimal Weight { get { return product.Weight; } }
        public string WeightUnits { get { return product.WeightUnits; } }
        public string Note { get { return product.Note; } set { product.Note = value; } }



        private ProductCarModel databaseModel;

        public ProductViewModel(Product item)
        {
            this.product = item;

            this.databaseModel = new ProductCarModel(ProductCarDataBase.Instance);

            UpdateQuantityCommand = new Command<bool>(ExecuteUpdateQuantityCommand);
            RequestDeleteCommand = new Command(ExecuteRequestDeleteCommand);
            DeleteCommand = new Command(ExecuteDeleteCommand);
            HowDoYouLikeItCommand = new Command(ExecuteHowDoYouLikeItCommand);
            UpdateCommand = new Command(ExecuteUpdateCommand);
        }

        public void AddQuantity(object sender, EventArgs e)
        {
            IsAdd = true;
            ExecuteUpdateQuantityCommand(IsAdd);
        }

        public void SubstractQuantity(object sender, EventArgs e)
        {
            IsAdd = false;
            ExecuteUpdateQuantityCommand(IsAdd);
        }

        public void DeleteProduct(object sender, EventArgs e)
        {
            ExecuteRequestDeleteCommand();
        }

        public void HowDoYouLikeIt(object sender, EventArgs e)
        {
            ExecuteHowDoYouLikeItCommand();
        }

        private void ExecuteUpdateQuantityCommand(bool add)
        {
            if (!add && product.Quantity <= 1)
            {
                ProductToBeDeleted = product;
            }
            else
            {
                var totalUpdated = databaseModel.UpSertProduct(product, add);
                ProductUpdated = product;
            }
        }

        private void ExecuteUpdateCommand()
        {
            databaseModel.UpSertProduct(product);
            ProductUpdated = product;
        }

        private void ExecuteRequestDeleteCommand()
        {
            ProductToBeDeleted = product;
        }

        private void ExecuteDeleteCommand()
        {
            databaseModel.DeleteProduct(product.Id);
            ProductDeletedId = product.Id;
        }

        private void ExecuteHowDoYouLikeItCommand()
        {
            HowDoYouLikeItProduct = product;
        }
    }
}
