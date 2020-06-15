using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class ProductFilteredViewCell : UITableViewCell
    {
        #region Attributes 
        public UIImageView Icon
        {
            get { return iconImageView; }
        }

        public UILabel ProductName
        {
            get { return productNameLabel; }
        }
        #endregion

        #region Constructors
        static ProductFilteredViewCell()
        {
            //Static default Constructor this class
        }

        protected ProductFilteredViewCell(IntPtr handle) : base(handle)
        {
            //Default Constructor this class
        }
        #endregion
    }
}