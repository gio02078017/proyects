using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Products;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Models;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public partial class SummaryViewModel : BaseViewModel
    {
        public ISummaryViewModel Delegate { get; set; }

        private List<Product> products;
        public List<Product> Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
        }

        public string Dependency;

        public Command LoadSummaryCommand { get; set; }
        public Command ValidateDependencyChangeCommand { get; set; }
        public Command EmptyCarCommand { get; set; }
        public Command BuyCommand { get; set; }

        private ProductCarModel databaseModel;

        private BaseAddressModel baseAddressModel;
        private BaseOrderHelper baseOrderModel;
        private BaseProductsHelper baseProductModel;
        private IDeviceManager deviceManager;

        public SummaryViewModel(UserContext userContext, IDeviceManager deviceManager)
        {
            this.products = new List<Product>(); 

            this.databaseModel = new ProductCarModel(ProductCarDataBase.Instance);
            this.deviceManager = deviceManager;
            this.baseAddressModel = new BaseAddressModel(deviceManager);
            this.baseOrderModel = new BaseOrderHelper(deviceManager, userContext);
            this.baseProductModel = new BaseProductsHelper(deviceManager, userContext);

            LoadSummaryCommand = new Command(ExecuteGetSummaryCommand);
            EmptyCarCommand = new Command(ExecuteEmptyCarCommand);
            BuyCommand = new Command(async () => await ExecuteBuyCommand());
            ValidateDependencyChangeCommand = new Command(async () => await ExecuteDependencyChangeCommand());
        }

        private void ExecuteGetSummaryCommand()
        {
            try
            {
                Products = databaseModel.GetProducts();
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
        }

        private void ExecuteEmptyCarCommand()
        {
            try
            {
                databaseModel.FlushCar();
                Products.Clear();
                Products = new List<Product>();
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
        }

        private async Task ExecuteBuyCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (deviceManager.IsNetworkAvailable().Result)
                {
                    bool hasCorrespondenceAddress = await baseAddressModel.ValidateCorrespondence();
                    if (hasCorrespondenceAddress)
                    {
                        bool result = await UploadProducts();
                        if (result)
                        {
                            Delegate?.OrderUploaded();
                        }
                    }
                    else
                    {
                        Delegate?.HasNotCorrespondenceAddress();
                    }
                }
                else
                {
                    Delegate?.ConnectionUnavailable();
                }
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ExecuteDependencyChangeCommand()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                List<Product> localProducts = databaseModel.GetProducts();
                List<Price> prices = await baseProductModel.GetPrices(localProducts, Dependency);
                List<Product> deletedProducts = databaseModel.UpdateProductsPrice(prices, localProducts);
                if (deletedProducts.Any())
                {
                    Delegate?.ProductsDeletedDueToDependencyChange(deletedProducts);
                }
                OnPropertyChanged(nameof(SummaryViewModel.Products));
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<bool> UploadProducts()
        {
            List<Product> dbProducts = databaseModel.GetProducts();
            bool result = false;

            if (dbProducts.Any())
            {
                Order order = await baseOrderModel.SetOrder(dbProducts);

                order.SubTotal = GetSubTotalPrice();
                deviceManager.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));

                result = await baseProductModel.UploadOrder(order);
            }

            return result;
        }

        private decimal GetSubTotalPrice()
        {

            var productCarModel = new ProductCarModel(ProductCarDataBase.Instance);
            var summary = productCarModel.GetSummary();
            if (summary != null)
            {
                return decimal.Parse(summary[ConstDataBase.TotalPrice].ToString());
            }
            return 0;
        }
    }
}
