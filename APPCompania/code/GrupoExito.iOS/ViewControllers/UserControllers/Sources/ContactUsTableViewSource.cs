using System;
using System.Collections.Generic;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public class ContactUsTableViewSource : UITableViewSource
    {
        #region Attributes
        private EventHandler callStorePhoneAction;
        private UIViewControllerBase ControllerBase { get; set; }
        private IList<Contact> ContactsItem { get; set; }
        #endregion

        #region Properties
        public EventHandler CallStorePhoneAction { get => callStorePhoneAction; set => callStorePhoneAction = value; }
        #endregion

        #region Constructors
        public ContactUsTableViewSource(IList<Contact> contactsItem, UIViewControllerBase viewController)
        {
            this.ContactsItem = contactsItem;
            this.ControllerBase = viewController;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return ContactsItem.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            try
            {
                ContactUsItemViewCell cell = (ContactUsItemViewCell)tableView.DequeueReusableCell(ConstantIdentifier.ContactUsItemIdentifier, indexPath);
                Contact currentCell = ContactsItem[indexPath.Row];
                cell.LoadData(currentCell);
                cell.ActionCallStorePhone += CellActionCallStorePhone;
                return cell;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return ConstantViewSize.ContactUsMenuHeightCell;
        }
        #endregion

        #region Events
        private void CellActionCallStorePhone(object sender, EventArgs e)
        {
            CallStorePhoneAction(sender, e);
        }
        #endregion
    }
}
