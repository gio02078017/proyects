using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Cells;
using GrupoExito.Utilities.Helpers;
using UIKit;


namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public class MyStoresTableViewSource : UITableViewSource
    {
        #region Attributes
        private List<Store> stores;
        private UIViewControllerBase ControllerBase;
        private UIActivityIndicatorView spinnerAcitivityIndicatorView;
        private CustomActivityIndicatorView spinnerActivityIndicatorView_;
        private UIView customSpinnerView;

        private EventHandler selectStoreAction;
        #endregion

        #region Properties
        public EventHandler SelectStoreAction { get => selectStoreAction; set => selectStoreAction = value; }
        #endregion

        #region Constructors
        static MyStoresTableViewSource()
        {
            //Static default constructor this class
        }

        protected MyStoresTableViewSource(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }

        public MyStoresTableViewSource(List<Store> stores, UIViewControllerBase viewControllerBase, UIActivityIndicatorView spinnerAcitivityIndicatorView, UIView customSpinnerView, ref CustomActivityIndicatorView spinnerActivityIndicatorView_)
        {
            this.stores = stores;
            this.ControllerBase = viewControllerBase;
            this.customSpinnerView = customSpinnerView;
            this.spinnerAcitivityIndicatorView = spinnerAcitivityIndicatorView;
            this.spinnerActivityIndicatorView_ = spinnerActivityIndicatorView_;
        }
        #endregion

        #region Methods Override
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return stores.Count == 0 ? 1 : stores.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (stores.Count > 0)
            {
                AddressTableViewCell cell = (AddressTableViewCell)tableView.DequeueReusableCell(ConstantIdentifier.MyAddressIdentifier, indexPath);
                Store store = stores[indexPath.Row];
                if (store != null)
                {
                    cell.TitleLabel.Text = store.Name;
                    cell.Address.Text = store.Address;
                    cell.IconAddress.Hidden = true;
                }
                return cell;
            }
            else
            {
                NotCreditCardViewCell cell = (NotCreditCardViewCell)tableView.DequeueReusableCell(NotCreditCardViewCell.Key, indexPath);
                cell.AddAction += SelectStoreAddAction;
                cell.SetTitle("No tienes ningún almacén guardado");
                cell.HiddenDescription();
                return cell;
            }
        }

        private void SelectStoreAddAction(object sender, EventArgs e)
        {
            SelectStoreAction?.Invoke(sender, e);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (stores.Count > 0)
            {
                MessageConfirmView messageConfirmView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageConfirmView, Self, null).GetItem<MessageConfirmView>(0);
                CGRect messageConfirmViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, customSpinnerView.Frame.Size.Height);
                messageConfirmView_.Message.Text = "¿Realmente desea recoger en la tienda escogida?";
                messageConfirmView_.Frame = messageConfirmViewFrame;
                customSpinnerView.AddSubview(messageConfirmView_);
                spinnerActivityIndicatorView_.Hidden = true;
                spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.3f);
                spinnerAcitivityIndicatorView.StartAnimating();
                customSpinnerView.Hidden = false;
                customSpinnerView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                messageConfirmView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                Util.SetConstraint(customSpinnerView, ConstantViewSize.customSpinnerViewHeightDefault, ConstantViewSize.messageCustomViewHeightDefault);
                messageConfirmView_.Negation.TouchUpInside += (object sender, EventArgs e) =>
                {
                    spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    spinnerAcitivityIndicatorView.StopAnimating();
                    messageConfirmView_.RemoveFromSuperview();
                    spinnerActivityIndicatorView_.Image.StopAnimating();
                    customSpinnerView.BackgroundColor = UIColor.Clear;
                    customSpinnerView.Hidden = true;
                    Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
                };

                messageConfirmView_.Afirmation.TouchUpInside += (object sender, EventArgs e) =>
                {
                    spinnerAcitivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    spinnerAcitivityIndicatorView.StopAnimating();
                    spinnerActivityIndicatorView_.Image.StopAnimating();
                    spinnerActivityIndicatorView_.Hidden = true;
                    messageConfirmView_.RemoveFromSuperview();
                    customSpinnerView.BackgroundColor = UIColor.Clear;
                    customSpinnerView.Hidden = true;
                    Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
                    UserContext user = ParametersManager.UserContext;
                    user.Store = stores[indexPath.Row];
                    DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(user));
                    ParametersManager.ContainChanges = true;
                    ControllerBase.NavigationController.PopViewController(true);
                };
            }
        }

        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            return true;
        }
        #endregion
    }
}
