using System;
using System.Linq;
using System.Threading.Tasks;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;

namespace GrupoExito.iOS.Referentials
{
    public class BaseTicketController : UIViewControllerBase
    {
        protected CashDrawerTurnModel cashDrawerTurnModel;

        #region Constructors

        public BaseTicketController(IntPtr handle) : base(handle)
        {
            cashDrawerTurnModel = new CashDrawerTurnModel(new CashDrawerTurnService(DeviceManager.Instance));
        }

        #endregion

        #region Methods Async
        public async Task<TicketResponse> TurnStatusAsync(Ticket ticket)
        {
            try
            {
                TicketResponse response = await cashDrawerTurnModel.StatusTicket(ticket);

                if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
                {
                    if (response.Result.Messages.Any())
                    {
                        string message = MessagesHelper.GetMessage(response.Result);
                        StartActivityErrorMessage(response.Result.Messages[0].Code, message);
                    }
                }

                return response;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.TurnViewController, ConstantMethodName.TurnStatusAsync);
                ShowMessageException(exception);
                return null;
            }
        }

        #endregion
    }
}
