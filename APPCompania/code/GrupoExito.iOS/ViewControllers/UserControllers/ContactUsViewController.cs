using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Foundation;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Contacts;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class ContactUsViewController : UIViewControllerBase
    {
        #region Attributes
        private ContactsModel contactsModel;
        private IList<Contact> contactsItem;
        #endregion

        #region Constructors
        public ContactUsViewController(IntPtr handle) : base(handle)
        {
            contactsModel = new ContactsModel();
        }
        #endregion

        #region Life Cycle

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ContactUs, nameof(ContactUsViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {                
                this.LoadExternalViews();
                base.StartActivityIndicatorCustom();
                this.LoadFonts();
                this.LoadHandlers();
                this.LoadData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ContactUsViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ContactUsViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        #endregion

        #region Methods 
        private void LoadFonts()
        {
            //Load font size and style
        }

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void LoadExternalViews()
        {
            try
            {
                contactStoreTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.ContactUsItemViewCell, NSBundle.MainBundle), ConstantIdentifier.ContactUsItemIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            contactsItem = new List<Contact>();
            contactsItem = contactsModel.GetContacts();
            if (contactsItem != null && contactsItem.Any())
            {
                contactStoreHeightTableViewConstraint.Constant = contactsItem.Count * ConstantViewSize.ContactUsMenuHeightCell;
                ContactUsTableViewSource contactUsTableViewSource = new ContactUsTableViewSource(contactsItem, this);
                contactStoreTableView.Source = contactUsTableViewSource;
                contactUsTableViewSource.CallStorePhoneAction += ContactUsTableViewSourceCallStorePhoneAction;
                contactStoreTableView.ReloadData();
            }
            else
            {
                contactStoreHeightTableViewConstraint.Constant = 0;
            }
            NSMutableAttributedString boldText = new NSMutableAttributedString("Horario de atención: ", UIFont.FromName(ConstantFontSize.LetterTitle, 17f));
            NSMutableAttributedString titleTextAttributes = new NSMutableAttributedString(AppConfigurations.AttentionSchedule, UIFont.FromName(ConstantFontSize.LetterSubtitle, 16f));
            boldText.Append(titleTextAttributes);
            scheduleOfAttentionInfoLabel.AttributedText = boldText;
            base.StopActivityIndicatorCustom();
        }
        #endregion

        #region Events
        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            //Retry load data view
        }

        private void ContactUsTableViewSourceCallStorePhoneAction(object sender, EventArgs e)
        {
            try
            {
                UIButton numberPhoneSelected = (UIButton)sender;
                String numberPhone = numberPhoneSelected.Title(UIControlState.Normal);
                var trimmedName = Regex.Replace(numberPhone, @"\s+", "");
                string phoneURLString = string.Format("tel:{0}", trimmedName);
                NSUrl phoneURL = new NSUrl(phoneURLString);
                UIApplication.SharedApplication.OpenUrl(phoneURL);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.ContactUsViewController, ConstantEventName.ContactUsTableViewSourceCallStorePhoneAction);
            }
        }
        #endregion
    }
}

