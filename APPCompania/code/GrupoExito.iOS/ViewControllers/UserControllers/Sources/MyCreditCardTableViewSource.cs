using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public class MyCreditCardTableViewSource : UITableViewSource
    {
        #region Attributes
        private IList<CreditCard> Cards { get; set; }
        private EventHandler activeCaduceWarningAction;
        private Action<CreditCard> rowSelectedHandler;

        #endregion

        #region Properties
        public Action<CreditCard> RowSelectedHandler { get => rowSelectedHandler; set => rowSelectedHandler = value; }
        public Action<CreditCard> DeleteAction { get; set; }
        public EventHandler ActiveCaduceWarningAction { get => activeCaduceWarningAction; set => activeCaduceWarningAction = value; }
        private bool _isFromCheckout { get; set; }
        #endregion

        #region Constructors
        static MyCreditCardTableViewSource()
        {
            //Static default constructor this class
        }

        protected MyCreditCardTableViewSource(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }

        public MyCreditCardTableViewSource(IList<CreditCard> cardsref, bool isFromCheckout)
        {
            this.Cards = cardsref;
            this._isFromCheckout = isFromCheckout;
        }
        #endregion

        #region Methods Override
        public override nint NumberOfSections(UITableView tableView)
        {
            if (!_isFromCheckout)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (!_isFromCheckout)
            {
                switch (section)
                {
                    case 0:
                        return 1;
                    default:
                        return Cards == null ? 0 : Cards.Count == 0 ? 1 : Cards.Count;
                }
            }
            else return Cards == null ? 0 : Cards.Count == 0 ? 1 : Cards.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (!_isFromCheckout)
            {
                if (indexPath.Section == 0)
                {
                    CreditCardHeaderCell Headercell = (CreditCardHeaderCell)tableView.DequeueReusableCell(CreditCardHeaderCell.Key, indexPath);
                    return Headercell;
                }
                else
                {
                    if (Cards.Count == 0)
                    {
                        NotCreditCardViewCell notCreditCardViewCell = (NotCreditCardViewCell)tableView.DequeueReusableCell(NotCreditCardViewCell.Key, indexPath);
                        notCreditCardViewCell.HiddenButton();
                        return notCreditCardViewCell;
                    }
                    else
                    {
                        CreditCardViewCell cell = (CreditCardViewCell)tableView.DequeueReusableCell(CreditCardViewCell.Key, indexPath);
                        CreditCard card = Cards[indexPath.Row];
                        cell.LoadData(card, indexPath);
                        cell.ActiveCaduceWarningAction = CellActiveCaduceWarningAction;
                        return cell;
                    }
                }
            }
            else
            {
                if (Cards.Count == 0)
                {
                    NotCreditCardViewCell notCreditCardViewCell = (NotCreditCardViewCell)tableView.DequeueReusableCell(NotCreditCardViewCell.Key, indexPath);
                    notCreditCardViewCell.HiddenButton();
                    return notCreditCardViewCell;
                }
                else
                {
                    CreditCardViewCell cell = (CreditCardViewCell)tableView.DequeueReusableCell(CreditCardViewCell.Key, indexPath);
                    CreditCard card = Cards[indexPath.Row];
                    cell.LoadData(card, indexPath);
                    cell.HideIconDetail(_isFromCheckout);
                    cell.ActiveCaduceWarningAction = CellActiveCaduceWarningAction;
                    return cell;
                }
            }
        }

        void CellActiveCaduceWarningAction(object sender, EventArgs e)
        {
            ActiveCaduceWarningAction?.Invoke(sender, null);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            RowSelectedHandler?.Invoke(Cards[indexPath.Row]);
        }

        public override UITableViewRowAction[] EditActionsForRow(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewRowAction[] actions = new UITableViewRowAction[1];

            UITableViewRowAction delete = UITableViewRowAction.Create(UITableViewRowActionStyle.Destructive, string.Empty, (action, indexpath) =>
            {
                DeleteAction?.Invoke(Cards[indexPath.Row]);
            });

            if (Cards[indexPath.Row].IsNextCaduced && !Cards[indexPath.Row].Type.Equals(ConstCreditCardType.Exito))
            {
                delete.BackgroundColor = new UIColor(new UIImage(ConstantImages.AccionEliminarACaducir));
            }
            else
            {
                delete.BackgroundColor = new UIColor(new UIImage(ConstantImages.AccionEliminar));
            }

            actions[0] = delete;

            return actions;
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (!_isFromCheckout)
            {
                switch (indexPath.Section)
                {
                    case 0:
                        return false;
                    default:
                        return Cards.Count == 0 ? false : true;
                }
            }
            else return false;
        }
        #endregion
    }
}
