using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Source
{
    public class CityStorePickerViewDelegate : UIPickerViewDelegate
    {
        #region Attributes

        private IList<City> CityItems { get; set; }
        private IList<Store> StoreItems { get; set; }
        private IList<StoreCashDrawerTurn> GetStoreCashDrawerTurns { get; set; }
        private UITextField CityTextField { get; set; }
        private EventHandler action;
        public EventHandler Action { get => action; set => action = value; }
        private int Type { get; set; }

        #endregion

        #region Constructors 
        public CityStorePickerViewDelegate(IList<City> Items, UITextField textField)
        {
            this.CityItems = Items;
            this.CityTextField = textField;
            Type = 1;
        }

        public CityStorePickerViewDelegate(IList<Store> storeItems, UITextField textField)
        {
            this.StoreItems = storeItems;
            this.CityTextField = textField;
            Type = 2;
        }

        public CityStorePickerViewDelegate(IList<StoreCashDrawerTurn> GetStoreCashDrawerTurns, UITextField textField)
        {
            this.GetStoreCashDrawerTurns = GetStoreCashDrawerTurns;
            this.CityTextField = textField;
            Type = 3;
        }

        #endregion

        #region Override Methods 

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (Type == 1)
            {
                return CityItems[(int)row].Name;
            }
            else if (Type == 2)
            {
                return String.IsNullOrEmpty(StoreItems[(int)row].Name) ? StoreItems[(int)row].DependencyName : StoreItems[(int)row].Name;
            }
            else
            {
                return GetStoreCashDrawerTurns[(int)row].Name;
            }
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (Type == 1)
            {
                if (CityItems != null && CityItems.Count > row)
                {
                    CityTextField.Text = CityItems[(int)row].Name;
                    pickerView.Select(row, component, true);
                    if (Action != null)
                    {
                        Action.Invoke(ConstantReusableViewName.ToHomeSelected, null);
                    }
                }
            }
            else if (Type == 2)
            {
                if (StoreItems != null && StoreItems.Count > row)
                {
                    CityTextField.Text = (StoreItems[(int)row].Name ?? StoreItems[(int)row].DependencyName);
                    pickerView.Select(row, component, true);
                    if (Action != null)
                    {
                        Action.Invoke(ConstantReusableViewName.ToStoreSelected, null);
                    }
                }
            }
            else
            {
                CityTextField.Text = GetStoreCashDrawerTurns[(int)row].Name;
                pickerView.Select(row, component, true);
                if (Action != null)
                {
                    Action.Invoke(ConstantReusableViewName.ToStoreSelected, null);
                }
            }
        }

        #endregion
    }
}
