using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Source
{
    public class AutoCompleteSource : UITableViewSource
    {
        #region Attributes 

        private List<City> CitiesSugested { get; set; }
        private City CitySelected { get; set; }
        private UITextField CityTextField { get; set; }

        #endregion

        #region Constructors 

        public AutoCompleteSource(List<City> citiesSugested, UITextField city)
        {
            this.CitiesSugested = citiesSugested;
            this.CityTextField = city;
        }

        #endregion

        #region Override Methods 
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = CitiesSugested[indexPath.Row];
            var cell = new UITableViewCell();
            cell.TextLabel.Text = item.Name;
            return cell;
        }

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.AutocompleteHeightCell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return this.CitiesSugested.Count;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var rowSelected = this.CitiesSugested[indexPath.Row];

            if (rowSelected != null)
            {
                CitySelected = rowSelected;
                CityTextField.Text = CitySelected.Name;
                tableView.Superview.Hidden = true;
            }
        }

        #endregion
    }
}
