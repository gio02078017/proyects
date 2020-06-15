using System;
using Foundation;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    public partial class ShiftInBoxView : UIView
    {
        #region Properties

        public UILabel ShiftNumber
        {
            get { return shiftNumberLabel; }
        }

        public UILabel TimeTurn
        {
            get { return timeTurnLabel; }
        }

        public UIImageView TimeImage
        {
            get { return timeImageView; }

        }

        public UIImageView TurnImage
        {
            get { return turnImageView; }
        }

        #endregion

        #region Constructors 

        static ShiftInBoxView() { }
        protected ShiftInBoxView(IntPtr handle) : base(handle) { }

        #endregion

        #region Override methods

        public override void AwakeFromNib()
        {
        }

        #endregion


        #region Methods 

        public void LoadData(StatusCashDrawerTurnResponse data)
        {
            timeTurnLabel.Text = string.Format(AppMessages.TimeTurnText, data.AvgWaitServ, data.AvgWaitTime);
            ShiftNumber.Text = data.WaitingTickets.ToString();
        }

        public void Loadticket(TicketResponse data)
        {
            if (data.AvgWaitTime != 0)
            {
                timeTurnLabel.Text = string.Format(AppMessages.TimeTurnText, data.AvgWaitServ, data.AvgWaitTime);
            }
            else
            {
                timeTurnLabel.Text = data.WaitEstimate + " " +  AppMessages.MinutesText;
            }

            if (data.WaitingTickets != 0)
            {
                ShiftNumber.Text = data.WaitingTickets.ToString();
            }
            else
            {
                ShiftNumber.Text = data.TicketInFront.ToString();
            }
        }

        public void TurnFinished()
        {
            turnsLabel.TextColor = UIColor.Gray;
            averagelabel.TextColor = UIColor.Gray;
            ShiftNumber.Text = "0";
            ShiftNumber.TextColor = UIColor.Gray;
            TimeTurn.Text = "0";
            TimeTurn.TextColor = UIColor.Gray;
            turnImageView.Image = UIImage.FromFile(ConstantImages.PersonasEnTurno);
            timeImageView.Image = UIImage.FromFile(ConstantImages.TiempoEspera);
        }
        #endregion

        #region Events 

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);
        }

        #endregion
    }
}

