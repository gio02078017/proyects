using System;
using GrupoExito.Entities;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class HistoricalOrderTableViewCell : UITableViewCell
    {
        #region Constructors
        public HistoricalOrderTableViewCell (IntPtr handle) : base (handle)
        {
            //Default constructor this class
        }
        #endregion

        #region Methods
        public void LoadData(Order order)
        {
            orderValueLabel.Text = order.Id;
            dateValueLabel.Text = order.Date;
        }
        #endregion
    }
}