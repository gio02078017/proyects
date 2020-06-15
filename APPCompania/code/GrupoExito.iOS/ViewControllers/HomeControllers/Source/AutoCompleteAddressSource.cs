using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Source
{
    public class AutoCompleteAddressSource : UITableViewSource
    {
        #region Attributes 

        private List<string> Predictions;
        private UITextField AddressTextField;
        private string AddressSelected;

        #endregion

        #region Constructors 

        public AutoCompleteAddressSource(List<string> predictions, UITextField addressTextField)
        {
            this.Predictions = predictions;
            this.AddressTextField = addressTextField;
        }

        #endregion

        #region Override Methods 

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.AutocompleteHeightCell;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = Predictions[indexPath.Row];
            var cell = new UITableViewCell();
            cell.TextLabel.Text = item;
            cell.TextLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.WelcomeAutocompleteCell);
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return Predictions.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var rowSelected = this.Predictions[indexPath.Row];

            if (rowSelected != null)
            {
                AddressSelected = rowSelected;
                AddressTextField.Text = AddressSelected;
                tableView.Superview.Hidden = true;
            }
        }

        #endregion
    }
}
