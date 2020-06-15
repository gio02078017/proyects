using System;
using System.Collections.Generic;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class PickerViewDelegate : UIPickerViewDelegate
    {
        #region Attributes
        private List<Item> listItems;
        private IList<DocumentType> documentsTypes;
        private int type = 0;
        private UIPickerView selectedPickerView;
        private UITextField selectecTextField;
        private EventHandler action;
        #endregion

        #region Properties
        public EventHandler Action { get => action; set => action = value; }
        #endregion

        #region Constructors 
        public PickerViewDelegate(IList<DocumentType> documentsType, UIPickerView selectedPickerView)
        {
            this.documentsTypes = documentsType;
            this.selectedPickerView = selectedPickerView;
            type = 2;
        }

        public PickerViewDelegate(List<Item> listItems, UIPickerView selectedPickerView)
        {
            this.listItems = listItems;
            this.selectedPickerView = selectedPickerView;
            type = 1;
        }

        public PickerViewDelegate(IList<DocumentType> documentsType, UITextField selectecTextField)
        {
            this.documentsTypes = documentsType;
            this.selectecTextField = selectecTextField;
            type = 3;
        }

        public PickerViewDelegate(UITextField selectecTextField)
        {
            this.selectecTextField = selectecTextField;
            type = 4;
        }
        #endregion

        #region Override Methods 
        public override void Selected(UIPickerView pickerView, nint row, nint component)
        { 
           if (type == 3)
            {
                if (documentsTypes != null && documentsTypes.Count > row)
                {
                    selectecTextField.Text = documentsTypes[(int)row].Name;
                    if (Action != null)
                    {
                        Action.Invoke(ConstantReusableViewName.ToHomeSelected, null);
                    }
                }
            }else if (type == 4) {

       
            }else{
                this.selectedPickerView.Select(row, component, true);
            }
        }


        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            if (type == 1)
            {
                return listItems[(int)row].Text;
            }else{
                return documentsTypes[(int)row].Description;
            }
        }
        #endregion

    }
}

