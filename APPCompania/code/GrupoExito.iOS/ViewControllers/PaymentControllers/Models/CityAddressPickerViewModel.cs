using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class CityAddressPickerViewModel : UIPickerViewModel
    {
        #region Attributes
        public event EventHandler<nint> ValueChanged;
        private IList<City> CitiesAddresses;
        #endregion

        #region Constructors
        public CityAddressPickerViewModel(IList<City> cities)
        {
            CitiesAddresses = cities;
        }
        #endregion

        #region Overrides Methods
        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return CitiesAddresses.Count;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            return CitiesAddresses[(int)row].Name;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            ValueChanged?.Invoke(this, row);
        }
        #endregion
    }
}