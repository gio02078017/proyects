using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Source
{
    public class CityStorePickerViewDataSource : UIPickerViewDataSource
    {
        #region Attributes

        private IList<City> CityItems { get; set; }
        private IList<Store> StoreItems { get; set; }
        private IList<StoreCashDrawerTurn> GetStoreCashDrawerTurns { get; set; }
        public int Type { get => type; set => type = value; }
        private int type = 0;

        #endregion

        #region Constructors 

        public CityStorePickerViewDataSource(IList<City> cityItems)
        {
            this.CityItems = cityItems;
            Type = 1;
        }

        public CityStorePickerViewDataSource(IList<Store> storeItems)
        {
            this.StoreItems = storeItems;
            Type = 2;
        }

        public CityStorePickerViewDataSource(IList<StoreCashDrawerTurn> GetStoreCashDrawerTurns)
        {
            this.GetStoreCashDrawerTurns = GetStoreCashDrawerTurns;
            Type = 3;
        }

        #endregion

        #region Override Methods 

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            try
            {
                if (Type == 1)
                {
                    return this.CityItems.Count;
                }
                else if (Type == 2)
                {
                    return this.StoreItems.Count;
                }
                else
                {
                    return this.GetStoreCashDrawerTurns.Count;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        #endregion
    }
}
