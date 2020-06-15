using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Cells;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Sources
{
    public class FilterProductTableViewSource : UITableViewSource
    {
        #region Attributes
        private List<ProductFilter> Categories { get; set; }
        private List<ProductFilter> Brands { get; set; }
        private bool[] _isSectionOpen;
        private EventHandler _headerButtonCommand { get; set; }
        private Category category;

        private EventHandler filterAction;
        private EventHandler orderAction;
        #endregion

        #region Properties
        private List<string> HeaderSection = new List<string>
        {
            "",
            "Categoría",
            "Marca"
        };

        public EventHandler FilterAction { get => filterAction; set => filterAction = value; }
        public EventHandler OrderAction { get => orderAction; set => orderAction = value; }
        #endregion

        #region Constructors
        public FilterProductTableViewSource(List<ProductFilter> categories, List<ProductFilter> brands, UITableView tableView, Category category)
        {
            try
            {
                this.Categories = categories;
                this.Brands = brands;
                _isSectionOpen = new bool[HeaderSection.Count];
                _headerButtonCommand = (sender, e) =>
                {
                    var button = sender as UIButton;
                    var section = this.HeaderSection.FindIndex((obj) => obj == button.TitleLabel.Text);
                    if (section >= 0)
                    {
                        _isSectionOpen[(int)section] = !_isSectionOpen[(int)section];
                    }
                    tableView.ReloadData();
                    var paths = new NSIndexPath[RowsInSection(tableView, section)];
                    for (int i = 0; i < paths.Length; i++)
                    {
                        paths[i] = NSIndexPath.FromItemSection(i, section);
                    }
                    tableView.ReloadRows(paths, UITableViewRowAnimation.Automatic);
                };
                this.category = category;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.FilterProductTableViewSource);
            }
        }

        #endregion

        #region Overrides Methods 

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (section >= 0)
            {
                switch (section)
                {
                    case 0:
                        return 1;
                    case 1:
                        return _isSectionOpen[section] ? this.Categories.Count : 0;
                    default:
                        return _isSectionOpen[section] ? this.Brands.Count : 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return HeaderSection.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            ProductFilter Data;
            switch (indexPath.Section)
            {
                case 0:
                    HeaderProductViewCell headerProductViewCell = (HeaderProductViewCell)tableView.DequeueReusableCell(HeaderProductViewCell.Key, indexPath);
                    if (headerProductViewCell.FilterAction == null)
                    {
                        headerProductViewCell.FilterAction += HeaderProductViewCellFilterAction;
                    }
                    if (headerProductViewCell.OrderAction == null)
                    {
                        headerProductViewCell.OrderAction += HeaderProductViewCellOrderAction;
                    }
                    if (category != null)
                    {
                        headerProductViewCell.SetTitle(category.IconCategoryGray, category.Name, true);
                    }
                    else
                    {
                        headerProductViewCell.SetTitle(UIImage.FromFile(ConstantImages.Buscar), ParametersManager.UserQuery, true);
                    }
                    return headerProductViewCell;
                case 1:
                    Data = ParametersManager.Categories[indexPath.Row];
                    break;
               default:
                    Data = ParametersManager.Brands[indexPath.Row];
                    break;
            }
            FilterByViewCell cell = (FilterByViewCell)tableView.DequeueReusableCell(FilterByViewCell.Key, indexPath);
            try
            {
                cell.Accessory = (Data.Checked) ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
                cell.SetData(Data.Key, Data.Quantity);
                if (Data.Checked)
                {
                    cell.GetTitle().Font = UIFont.BoldSystemFontOfSize(ConstantFontSize.FilterTableRow);
                    cell.GetQuantity().Font = UIFont.BoldSystemFontOfSize(ConstantFontSize.FilterTableRow);
                }
                else
                {
                    cell.GetTitle().Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.FilterTableRow);
                    cell.GetQuantity().Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.FilterTableRow);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.FilterProductTableViewSource);
            }
            return cell;
        }

        public override nfloat EstimatedHeightForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case 0:
                    return 0;
                default:
                    return 40;
            }
        }

        public override nfloat EstimatedHeightForFooter(UITableView tableView, nint section)
        {
            return 0;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return HeaderSection[(int)section];
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            try
            {
                if (section != 0)
                {
                    FilterHeaderView filterHeaderView = FilterHeaderView.Create();
                    filterHeaderView.Header.SetTitle(HeaderSection[(int)section], UIControlState.Normal);
                    filterHeaderView.Header.TouchUpInside += this._headerButtonCommand;
                    return filterHeaderView;
                }
                else
                {
                    return new UIView();
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.FilterProductTableViewSource);
                return null;
            }
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            try
            {
                if (indexPath.Section != 0)
                {
                    ProductFilter Data;
                    switch (indexPath.Section)
                    {
                        case 1:
                            Data = ParametersManager.Categories[indexPath.Row];
                            break;
                        default:
                            Data = ParametersManager.Brands[indexPath.Row];
                            break;
                    }
                    FilterByViewCell cell = (FilterByViewCell)tableView.CellAt(indexPath);
                    UpdateStatusFilter(indexPath);
                    if (cell.Accessory == UITableViewCellAccessory.Checkmark)
                    {
                        cell.Accessory = UITableViewCellAccessory.None;
                        cell.GetTitle().Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.FilterTableRow);
                        cell.GetQuantity().Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.FilterTableRow);
                        cell.TextLabel.Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.FilterTableRow);
                        Data.Checked = false;
                    }
                    else
                    {
                        cell.Accessory = UITableViewCellAccessory.Checkmark;
                        cell.GetTitle().Font = UIFont.BoldSystemFontOfSize(ConstantFontSize.FilterTableRow);
                        cell.GetQuantity().Font = UIFont.BoldSystemFontOfSize(ConstantFontSize.FilterTableRow);
                        Data.Checked = true;
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantEventName.RowSelected);
            }
        }

        public void ClearAll(UITableView tableView)
        {
            try
            {
                Brands.ForEach(k => k.Checked = false);
                Categories.ForEach(k => k.Checked = false);
                tableView.ReloadData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.FilterProductTableViewSource);
            }
        }

        #endregion

        #region Publics Methods
        public void SetParameterFilters()
        {
            try
            {
                var categoryCheched = ParametersManager.Categories as List<ProductFilter>;
                var brandsChecked = ParametersManager.Brands as List<ProductFilter>;
                ParametersManager.CategoriesName = categoryCheched.Where((arg) => arg.Checked).Select((arg) => arg.Key).ToList();
                ParametersManager.BrandsName = brandsChecked.Where((arg) => arg.Checked).Select((arg) => arg.Key).ToList(); ;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantEventName.RowSelected);
            }
        }

        private void UpdateStatusFilter(NSIndexPath indexPath)
        {
            try
            {
                if (indexPath.Section == 0)
                {
                    ParametersManager.Categories[indexPath.Row].Checked = !ParametersManager.Categories[indexPath.Row].Checked;
                }
                else if (indexPath.Section == 1)
                {
                    ParametersManager.Brands[indexPath.Row].Checked = !ParametersManager.Brands[indexPath.Row].Checked;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantEventName.RowSelected);
            }
        }
        #endregion

        #region Events
        private void HeaderProductViewCellOrderAction(object sender, EventArgs e)
        {
            orderAction?.Invoke(sender, e);
        }


        private void HeaderProductViewCellFilterAction(object sender, EventArgs e)
        {
            filterAction?.Invoke(sender, e);
        }
        #endregion
    }
}
