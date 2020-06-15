using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.iOS.Models;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Cells;
using GrupoExito.iOS.Views.PaymentViews.SummaryViews;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Sources
{
    public class PurchaseSummarySource : UITableViewSource
    {
        #region Attributes
        List<PurchaseSummary> purchaseSummaries;
        #endregion

        #region Constructors
        public PurchaseSummarySource(List<PurchaseSummary> purchaseSummaries)
        {
            this.purchaseSummaries = purchaseSummaries;
        }
        #endregion

        #region Overrides Methods
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    HeaderPurchaseSummaryViewCell headerViewCell = (HeaderPurchaseSummaryViewCell)tableView.DequeueReusableCell(HeaderPurchaseSummaryViewCell.Key, indexPath);
                    headerViewCell.DrawCell("Resumen de compra", string.Empty);
                    return headerViewCell;
                case 1:
                    PurchaseSummaryViewCell cell = (PurchaseSummaryViewCell)tableView.DequeueReusableCell(PurchaseSummaryViewCell.Key, indexPath);
                    cell.LoadData(purchaseSummaries[indexPath.Row]);
                    return cell;
                default:
                    HeaderPurchaseSummaryViewCell footerViewCell = (HeaderPurchaseSummaryViewCell)tableView.DequeueReusableCell(HeaderPurchaseSummaryViewCell.Key, indexPath);
                    footerViewCell.DrawCell(purchaseSummaries[purchaseSummaries.Count - 1].title, purchaseSummaries[purchaseSummaries.Count - 1].value);
                    return footerViewCell;
            }
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 3;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            switch (section)
            {
                case 0:
                    return 1;
                case 1:
                    return purchaseSummaries.Count - 1;
                default:
                    return 1;
            }
        }
        #endregion
    }
}
