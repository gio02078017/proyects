using System;
using Foundation;
using GrupoExito.iOS.Utilities;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Cells
{
    public partial class HeaderPurchaseSummaryViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("HeaderPurchaseSummaryViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Constructors
        static HeaderPurchaseSummaryViewCell()
        {
            Nib = UINib.FromName("HeaderPurchaseSummaryViewCell", NSBundle.MainBundle);
        }

        protected HeaderPurchaseSummaryViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion


        #region Methods
        public void DrawCell(string title, string value)
        {
            titleLabel.Text = title;
            if (!string.IsNullOrEmpty(value))
            {
                titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, 19f);
                valueLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, 19f);
                valueLabel.Hidden = false;
                valueLabel.Text = value;
            }
            else
            {
                titleLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, 21f);
                valueLabel.Hidden = true;
            }
        }
        #endregion
    }
}
