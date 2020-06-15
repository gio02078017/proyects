using System;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class CreditCardInstallmentModel : UIPickerViewModel
    {
        #region Attributes
        private readonly int installmentNumber = 24;
        public event EventHandler<nint> ValueChanged;
        #endregion

        #region Properties

        #endregion

        #region Constructors
        public CreditCardInstallmentModel()
        {
            //Default Constructor withOut Arguments
        }
        #endregion

        #region Overrides Methods
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return installmentNumber;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return (row + 1).ToString();
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            ValueChanged?.Invoke(this, row);
        }
        #endregion
    }
}
