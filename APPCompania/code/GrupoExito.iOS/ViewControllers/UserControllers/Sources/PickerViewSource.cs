using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class PickerViewSource : UIPickerViewDataSource
    {
        #region Attributes
        private List<Item> listItems;
        private IList<DocumentType> documentsTypes;
        private int type = 0;
        #endregion

        #region Constructors 
        public PickerViewSource(IList<DocumentType> documentsType)
        {
            this.documentsTypes = documentsType;
            type = 2;
        }

        public PickerViewSource(List<Item> listItems)
        {
            this.listItems = listItems;
            type = 1;
        }
        public PickerViewSource()
        {
        
            type = 3;
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
                if (type == 1)
                {
                    return this.listItems.Count;
                }
                else
                {
                    return this.documentsTypes.Count;
                }
            }catch(Exception){
                return 0;
            }
        }
        #endregion
    }
}

