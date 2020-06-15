using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.iOS.Models;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Sources
{
    public partial class OrderProductTableViewSource : UITableViewSource
    {
        #region Attributes
        private Category category;

        private List<string> Sections = new List<string>()
        {
            "",
            "Nombre",
            "Precio",
            "Relevancia"
        };

        private List<OrderByModel> Options = new List<OrderByModel>(){
            new OrderByModel(){ NameOption = "A-Z", Status = false, Section = 1, OrderBy = ConstOrder.Name, OrderType = ConstOrderType.Asc},
            new OrderByModel(){ NameOption = "Z-A", Status = false, Section = 1, OrderBy = ConstOrder.Name, OrderType = ConstOrderType.Desc},
            new OrderByModel(){ NameOption = "Precio de menor a mayor", Status = false, Section = 2, OrderBy = ConstOrder.Price, OrderType = ConstOrderType.Asc},
            new OrderByModel(){ NameOption = "Precio de mayor a menor", Status = false, Section = 2, OrderBy = ConstOrder.Price, OrderType = ConstOrderType.Desc},
            new OrderByModel(){ NameOption = "Relevancia", Status = false, Section = 3, OrderBy = ConstOrder.Relevance, OrderType = ConstOrderType.Desc}
        };

        private EventHandler filterAction;
        private EventHandler orderAction;
        #endregion

        #region Properties
        public EventHandler FilterAction { get => filterAction; set => filterAction = value; }
        public EventHandler OrderAction { get => orderAction; set => orderAction = value; }
        #endregion

        #region Constructors
        public OrderProductTableViewSource(Category category)
        {
            this.category = category;
            UpdateOptionCheck();
        }
        #endregion

        #region Overrides Methods
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case 0:
                case 3:
                    return 1;
                default:
                    return 2;
            }
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return Sections.Count;
        }


        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {

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
                        headerProductViewCell.SetTitle(category.IconCategoryGray, category.Name, false);
                    }
                    else
                    {
                        headerProductViewCell.SetTitle(UIImage.FromFile(ConstantImages.Buscar), ParametersManager.UserQuery, false);
                    }
                    return headerProductViewCell;
                default:
                    OrderByViewCell cell = (OrderByViewCell)tableView.DequeueReusableCell(OrderByViewCell.Key, indexPath);
                    try
                    {
                        var data = this.Options.FindAll((obj) => (obj.Section == indexPath.Section))[indexPath.Row];
                        if (data != null)
                        {
                            cell.SetData(data.NameOption, data.Status); 
                        }
                    }
                    catch (Exception exception)
                    {
                        Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.OrderProductTableViewSource);
                    }
                    return cell;
            }
        }


        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return Sections[(int)section];
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section != 0)
            {
                OrderByModel data = this.Options.FindAll((obj) => (obj.Section == indexPath.Section))[indexPath.Row];
                data.Status = !data.Status;
                ((OrderByViewCell)tableView.CellAt(indexPath)).SetStatus(data.Status);
                UpdateStatusFilter(data);
                SetParameters(data);
                tableView.ReloadData();
            }
        }
        #endregion

        #region Methods
        private void UpdateOptionCheck()
        {
            if (ParametersManager.OrderByFilter != null)
            {
                var option = this.Options.Find((obj) => obj.OrderBy == ParametersManager.OrderByFilter && obj.OrderType == ParametersManager.OrderType);
                if (option != null)
                {
                    option.Status = true;
                }
            }
        }

        private void UpdateStatusFilter(OrderByModel data)
        {
            Options.Where(x => x.OrderBy != data.OrderBy || x.OrderType != data.OrderType).ToList().ForEach(y => y.Status = false);
        }

        private void SetParameters(OrderByModel data)
        {
            if (data != null && data.Status)
            {
                ParametersManager.OrderByFilter = data.OrderBy;
                ParametersManager.OrderType = data.OrderType;
            }
            else
            {
                ParametersManager.OrderByFilter = ConstOrder.Relevance;
                ParametersManager.OrderType = ConstOrder.Name;
            }
        }

        public void ClearAll(UITableView orderByTableView)
        {
            try
            {
                Options.ForEach((obj) => obj.Status = false);
                orderByTableView.ReloadData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ProductFilterAndOrderByViewController, ConstantMethodName.OrderProductTableViewSource);
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

