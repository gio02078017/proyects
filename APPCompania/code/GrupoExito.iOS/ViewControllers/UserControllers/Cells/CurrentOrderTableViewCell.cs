using System;
using GrupoExito.Entities;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Interfaces;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class CurrentOrderTableViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly UINib Nib;
        private Order _order;
        private ICurrentOrderCell handler;
        #endregion

        #region Properties
        public ICurrentOrderCell Handler { get => handler; set => handler = value; }
        #endregion

        #region Constructors
        static CurrentOrderTableViewCell()
        {
            //Static default constructor this class
        }

        protected CurrentOrderTableViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Methods
        public void Configure(bool hideRecipientInfo)
        {
            recipientInfoView.Hidden = hideRecipientInfo;
        }

        public void LoadData(Order order, ICurrentOrderCell parentHandler)
        {
            _order = order;
            Handler = parentHandler;
            orderValueLabel.Text = order.Id;
            stateValueLabel.Text = order.Status;
            dateValueLabel.Text = order.Date;
            deliveryOnValueLabel.Text = order.Address;
            seeOrderButton.TouchUpInside += (o, s) =>
            {
                if(Handler != null)
                {
                    Handler.ShowOrderSelected(_order);
                }
            };
        }
        #endregion
    }
}
