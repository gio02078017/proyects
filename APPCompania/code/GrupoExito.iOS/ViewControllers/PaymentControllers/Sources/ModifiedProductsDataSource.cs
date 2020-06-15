using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Cells;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Sources
{
    public class ModifiedProductsDataSource : UITableViewSource
    {
        #region Attributes
        private IList<SoldOut> ProductsRemoved { get; set; }
        private IList<SoldOut> ProductsChanged { get; set; }
        #endregion

        #region Constructors
        public ModifiedProductsDataSource(IList<SoldOut> listSoldOut, IList<SoldOut> listSotckOut)
        {
            ProductsRemoved = listSoldOut;
            ProductsChanged = listSotckOut;
        }
        #endregion

        #region Overrides Methods
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            ProductModifiedTableViewCell cell = (ProductModifiedTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.ProductModifiedTableViewCell, indexPath);
            if (indexPath.Section == 0)
            {
                cell.Configure(ProductModifiedTableViewCell.SoldOutType.ProductDeleted);
                cell.LoadData(ProductsRemoved[indexPath.Row]);
            }
            else
            {
                cell.Configure(ProductModifiedTableViewCell.SoldOutType.ProductChanged);
                cell.LoadData(ProductsChanged[indexPath.Row]);
            }

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return (section == 0) ? ProductsRemoved.Count : ProductsChanged.Count;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 2;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 88;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            UIView view = new UIView();
            UILabel headerLabel = new UILabel
            {
                Font = UIFont.FromName(ConstantFontSize.LetterBody, ConstantFontSize.TextSubtitle2Size),
                TranslatesAutoresizingMaskIntoConstraints = false
            };
            switch (section)
            {
                case 0:
                    headerLabel.Text = ProductsRemoved.Count > 0 ? AppMessages.DeletedProductsText : string.Empty;
                    break;
                case 1:
                    headerLabel.Text = ProductsChanged.Count > 0 ? AppMessages.ModifiedUnitsText : string.Empty;
                    break;
            }
            view.AddSubview(headerLabel);
            var margins = view.LayoutMarginsGuide;
            headerLabel.LeadingAnchor.ConstraintEqualTo(margins.LeadingAnchor).Active = true;
            headerLabel.TrailingAnchor.ConstraintEqualTo(margins.TrailingAnchor).Active = true;
            headerLabel.TopAnchor.ConstraintEqualTo(margins.TopAnchor).Active = true;
            headerLabel.BottomAnchor.ConstraintEqualTo(margins.BottomAnchor, 10.0f).Active = true;

            UIView lineView = new UIView(new CGRect(tableView.Frame.X, headerLabel.Frame.Y + 8, tableView.Frame.Width, 5))
            {
                BackgroundColor = UIColor.LightGray
            };

            view.AddSubview(lineView);
            lineView.TranslatesAutoresizingMaskIntoConstraints = false;
            lineView.LeadingAnchor.ConstraintEqualTo(margins.LeadingAnchor).Active = true;
            lineView.TrailingAnchor.ConstraintEqualTo(margins.TrailingAnchor).Active = true;
            lineView.BottomAnchor.ConstraintEqualTo(margins.BottomAnchor).Active = true;
            headerLabel.LayoutIfNeeded();
            lineView.LayoutIfNeeded();
            return view;
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return (section == 0) ? AppMessages.DeletedProductsText : AppMessages.ModifiedUnitsText;
        }
        #endregion
    }
}
