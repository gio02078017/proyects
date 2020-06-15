using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Parameters.Orders;
using GrupoExito.Entities.Responses.Orders;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.Models;
using GrupoExito.Utilities.Contracts.Generic;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class ScheduleViewModel : BaseViewModel
    {
        public IScheduleModel Delegate { get; set; }

        private OrderScheduleResponse scheduleResponse;
        public OrderScheduleResponse ScheduleResponse
        {
            get { return scheduleResponse; }
            set { SetProperty(ref scheduleResponse, value); }
        }

        private ScheduleContingencyResponse contingencyResponse;
        public ScheduleContingencyResponse ContingencyResponse
        {
            get { return contingencyResponse; }
            set { SetProperty(ref contingencyResponse, value); }
        }

        private bool successReservation;
        public bool SuccessReservation
        {
            get { return successReservation; }
            set { SetProperty(ref successReservation, value); }
        }

        public Command GetScheduleCommand { get; set; }
        public Command ScheduleReservationCommand { get; set; }

        private BaseScheduleHelper baseScheduleHelper;
        private ProductCarModel databaseModel;
        private UserContext userContext;
        private IDeviceManager DeviceManager { get; set; }

        public ScheduleViewModel(UserContext userContext, IDeviceManager deviceManager)
        {
            this.databaseModel = new ProductCarModel(ProductCarDataBase.Instance);
            this.userContext = userContext;
            this.DeviceManager = deviceManager;

            this.baseScheduleHelper = new BaseScheduleHelper(deviceManager);

            GetScheduleCommand = new Command<int>(async (totalProducts) => await ExecuteGetScheduleCommand(totalProducts));
            ScheduleReservationCommand = new Command<Order>(async (order) => await ExecuteScheduleReservationCommand(order));
        }

        private async Task ExecuteGetScheduleCommand(int totalProducts)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                bool selectedStore = userContext.Store != null ? true : false;
                string dependencyId = userContext.DependencyId;

                OrderScheduleParameters parameters = new OrderScheduleParameters()
                {
                    DependencyId = dependencyId.Length == 2 ? "0" + dependencyId : dependencyId,
                    IdCity = selectedStore ? userContext.Store.IdPickup
                    : userContext.Address.IdPickup,
                    DeliveryMode = selectedStore ? ConstDeliveryMode.Pe : ConstDeliveryMode.Do,
                    QuantityUnits = totalProducts
                };

                ScheduleResponse = await baseScheduleHelper.GetSchedule(parameters);
            }
            catch (Exception ex)
            {
                var errorCode = (EnumErrorCode)Enum.Parse(typeof(EnumErrorCode), ex.Data[nameof(EnumExceptionDataKeys.Code)].ToString());
                if (errorCode == EnumErrorCode.AwsServiceUnavailable)
                {
                    await GetContingencySchedule();
                }
                else
                {
                    Delegate?.HandleError(ex);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetContingencySchedule()
        {
            List<Product> products = databaseModel.GetProducts();

            ScheduleContingencyParameters contingencyParametersarameters = new ScheduleContingencyParameters()
            {
                Quantity = products != null && products.Any() ? products.Count() : 0
            };

            ContingencyResponse = await baseScheduleHelper.GetScheduleContingency(contingencyParametersarameters);
        }

        private async Task ExecuteScheduleReservationCommand(Order order)
        {
            if (order == null) return;
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await ScheduleReservation(order);
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

        private async Task ScheduleReservation(Order order)
        {
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

            var response = await baseScheduleHelper.ScheduleReservation(parameters);

            SuccessReservation = response.Code == (int)EnumErrorCode.ScheduleSuccessReservation;
        }
    }
}
