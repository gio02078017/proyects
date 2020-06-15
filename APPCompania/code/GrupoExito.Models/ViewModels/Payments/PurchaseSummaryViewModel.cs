using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Parameters;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Models;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class PurchaseSummaryViewModel : BaseViewModel
    {
        public IPaymentModel Delegate { get; set; }
        public Command PaymentCommand { get; set; }
        public Command FetchSummaryCheckoutCommand { get; set; }
        public Command UpdateProductsInOrderCommand { get; set; }
        public Command DeleteCreditCardCommand { get; set; }
        public Command GetCreditCardsCommand { get; set; }
        //public Command GetPaymentezCreditCardsCommand { get; set; }

        private bool productsUpdatedInOrder;
        public bool ProductsUpdatedInOrder
        {
            get { return productsUpdatedInOrder; }
            set { SetProperty(ref productsUpdatedInOrder, value); }
        }

        private string schedule;
        public string Schedule
        {
            get { return schedule; }
            set { SetProperty(ref schedule, value); }
        }

        private bool paymentezCreditCardDeleted;
        public bool PaymentezCreditCardDeleted
        {
            get { return paymentezCreditCardDeleted; }
            set { SetProperty(ref paymentezCreditCardDeleted, value); }
        }

        private bool creditCardDeleted;
        public bool CreditCardDeleted
        {
            get { return creditCardDeleted; }
            set { SetProperty(ref creditCardDeleted, value); }
        }

        private IList<CreditCard> creditCards;
        public IList<CreditCard> CreditCards
        {
            get { return creditCards; }
            set { SetProperty(ref creditCards, value); }
        }

        private IList<CreditCard> paymentezCreditCards;
        public IList<CreditCard> PaymentezCreditCards
        {
            get { return paymentezCreditCards; }
            set { SetProperty(ref paymentezCreditCards, value); }
        }

        private Order paidOrder;
        public Order PaidOrder
        {
            get { return paidOrder; }
            set { SetProperty(ref paidOrder, value); }
        }

        private BaseScheduleHelper baseScheduleHelper;
        private BasePaymentHelper basePaymentHelper;
        private BaseOrderHelper baseOrderHelper;
        private BaseProductsHelper baseProductsHelper;
        private BaseCreditCardHelper baseCreditCardHelper;
        private UserContext userContext;
        private IDeviceManager deviceManager;
        private ProductCarModel databaseModel;

        public PurchaseSummaryViewModel(UserContext userContext, IDeviceManager deviceManager)
        {
            baseScheduleHelper = new BaseScheduleHelper(deviceManager);
            basePaymentHelper = new BasePaymentHelper(deviceManager);
            baseOrderHelper = new BaseOrderHelper(deviceManager, userContext);
            baseProductsHelper = new BaseProductsHelper(deviceManager, userContext);
            baseCreditCardHelper = new BaseCreditCardHelper(deviceManager);

            this.databaseModel = new ProductCarModel(ProductCarDataBase.Instance);

            this.userContext = userContext;
            this.deviceManager = deviceManager;

            PaymentCommand = new Command<Order>(async (order) => await ExecutePaymentCommand(order));
            FetchSummaryCheckoutCommand = new Command<Order>(async (order) => await ExecuteFetchSummaryCheckoutCommand(order));
            UpdateProductsInOrderCommand = new Command<Order>(ExecuteUpdateProductsInOrderCommand);
            DeleteCreditCardCommand = new Command<CreditCard>(async (creditCard) => await ExecuteDeleteCreditCardCommand(creditCard));
            GetCreditCardsCommand = new Command(async () => await ExecuteGetCreditCardsCommand());
            //GetPaymentezCreditCardsCommand = new Command(async () => await ExecuteGetPaymentezCreditCardsCommand());
        }

        private async Task ExecutePaymentCommand(Order order)
        {
            if (order == null) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (order.TypeModality == ConstTypeModality.Express)
                {
                    await Pay(order);
                }
                else if (order.Contingency)
                {
                    await Pay(order);
                }
                else
                {
                    await ValidateScheduleReservation(order);
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

        private async Task ExecuteFetchSummaryCheckoutCommand(Order order)
        {
            if (order == null) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var summaryResponse = await basePaymentHelper.SummaryCheckout(order);
                if (summaryResponse.ProductsRemoved != null && summaryResponse.ProductsRemoved.Any())
                    databaseModel.DeleteProductSouldOut(summaryResponse.ProductsRemoved);
                if (summaryResponse.ProductsChanged != null && summaryResponse.ProductsChanged.Any())
                    databaseModel.UpdateProductStockOut(summaryResponse.ProductsChanged);

                ExecuteUpdateProductsInOrderCommand(order);
                if (order.Products != null && order.Products.Any())
                {
                    await UploadOrder(order);
                }

                Delegate?.CheckoutSummaryFetched(summaryResponse);
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

        private async Task ExecuteGetCreditCardsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var response = await baseCreditCardHelper.GetAllPaymentMethods();

                if (response.CreditCards != null)
                {
                    CreditCards = response.CreditCards;
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

        //private async Task ExecuteGetPaymentezCreditCardsCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        var response = await baseCreditCardHelper.GetPaymentezCreditCards();
        //        if (response.CreditCards != null)
        //        {
        //            PaymentezCreditCards = response.CreditCards;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Delegate?.HandleError(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        private async Task ExecuteDeleteCreditCardCommand(CreditCard creditCard)
        {
            if (creditCard == null) return;

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (creditCard.Paymentez)
                {
                    await DeletePaymentezCreditCard(creditCard);
                    PaymentezCreditCardDeleted = true;
                }
                else
                {
                    await DeleteCreditCard(creditCard);
                    CreditCardDeleted = true;
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

        public async Task DeleteCreditCard(CreditCard creditCard)
        {
            var response = await baseCreditCardHelper.DeleteCreditCard(creditCard);

            //var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteCreditCardMessage, UIAlertControllerStyle.Alert);
            //alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
            //this.PresentViewController(alertController, true, null);
            //creditCards.Remove(creditCard);
            //int countWithExpireData = 0;
            //int countWithoutExpireData = 0;
            //foreach (CreditCard card in creditCards)
            //{
            //    if (card.IsNextCaduced && !card.Type.Equals(ConstCreditCardType.Exito))
            //    {
            //        countWithExpireData++;
            //    }
            //    else
            //    {
            //        countWithoutExpireData++;
            //    }
            //}
            //myCreditCardTableView.ReloadData();

        }


        public async Task DeletePaymentezCreditCard(CreditCard creditCard)
        {
            var response = await baseCreditCardHelper.DeletePaymentezCreditCard(creditCard);

            //spinnerAcitivityIndicatorView.StopAnimating();
            //spinnerAcitivityIndicatorView.HidesWhenStopped = true;
            //spinnerAcitivityIndicatorView.Hidden = true;
            //_spinnerActivityIndicatorView.Image.StopAnimating();
            //customSpinnerView.Hidden = true;
            //var alertController = UIAlertController.Create(AppMessages.ApplicationName, AppMessages.DeleteCreditCardMessage, UIAlertControllerStyle.Alert);
            //alertController.AddAction(UIAlertAction.Create(AppMessages.AcceptButtonText, UIAlertActionStyle.Default, action => { }));
            //this.PresentViewController(alertController, true, null);
            //creditCards.Remove(creditCard);
            //int countWithExpireData = 0;
            //int countWithoutExpireData = 0;
            //foreach (CreditCard card in creditCards)
            //{
            //    if (card.IsNextCaduced && !card.Type.Equals(ConstCreditCardType.Exito))
            //    {
            //        countWithExpireData++;
            //    }
            //    else
            //    {
            //        countWithoutExpireData++;
            //    }
            //}
            //myCreditCardTableView.ReloadData();
        }

        private void ExecuteUpdateProductsInOrderCommand(Order order)
        {
            try
            {
                List<Product> products = databaseModel.GetProducts();
                order.Products = products;
                order.TotalProducts = products.Count();
                deviceManager.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));

                ProductsUpdatedInOrder = true;
            }
            catch (Exception ex)
            {
                Delegate?.HandleError(ex);
            }
        }

        private async Task<bool> UploadOrder(Order order)
        {
            var response = await baseOrderHelper.GetOrder();

            if (!order.Id.Equals(response.ActiveOrderId))
            {
                order.Id = response.ActiveOrderId;
                deviceManager.SaveAccessPreference(ConstPreferenceKeys.Order, JsonService.Serialize(order));
            }

            return await baseProductsHelper.UploadOrder(order);
        }

        private async Task ValidateScheduleReservation(Order order)
        {
            ScheduleReservationParameters parameters = GetScheduleReservationParameters(order);

            var response = await baseScheduleHelper.ValidateScheduleReservation(parameters);
            if (response.ActiveReservation)
            {
                await Pay(order);
            }
            else
            {
                Delegate?.ThereIsNotActiveReservation();
            }
        }

        private ScheduleReservationParameters GetScheduleReservationParameters(Order order)
        {
            if (order == null) return null;

            bool selectedStore = userContext.Store != null ? true : false;
            string dependencyId = userContext.DependencyId;

            ScheduleReservationParameters parameters = new ScheduleReservationParameters()
            {
                OrderId = order.Id,
                ShippingCost = order.shippingCost,
                DeliveryMode = selectedStore ? ConstDeliveryMode.Pe : ConstDeliveryMode.Do,
                QuantityUnits = order.TotalProducts,
                DependencyId = dependencyId.Length == 2 ? "0" + dependencyId : dependencyId,
                Schedule = order.Schedule,
                TypeModality = order.TypeModality,
                DateSelected = order.DateSelected
            };

            return parameters;
        }

        private async Task Pay(Order order)
        {
            bool orderValid = await UploadOrder(order);
            if (orderValid)
            {
                var summaryResponse = await basePaymentHelper.SummaryCheckout(order);

                if (summaryResponse.ProductsChanged.Any() || summaryResponse.ProductsRemoved.Any())
                {
                    databaseModel.DeleteProductSouldOut(summaryResponse.ProductsRemoved);
                    databaseModel.UpdateProductStockOut(summaryResponse.ProductsChanged);

                    Delegate?.ProductNews(summaryResponse);
                }
                else
                {
                    var response = await basePaymentHelper.Pay(order);
                    if (response.FinishProcess)
                    {
                        if (response.StatusCode == null ||
                        (response.StatusCode != null &&
                            (response.StatusCode.Equals(ConstStatusPay.Approved) || response.StatusCode.Equals(ConstStatusPay.Pending))))
                        {
                            if (order.TypeModality == ConstTypeModality.ScheduledPickup)
                            {
                                Schedule = order.DateSelected + " entre " + order.Schedule;
                            }

                            PaidOrder = order;
                            PaidOrder.shippingCost = response.Shipping.ToString();
                            deviceManager.DeleteAccessPreference(ConstPreferenceKeys.Order);
                            databaseModel.FlushCar();
                            await baseOrderHelper.SetOrder(new List<Product>());
                        }
                    }

                    Delegate?.PaymentFinished(response);
                }
            }
            else Delegate?.OrderNotValid();
        }
    }
}
