using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    public partial class MarkerInfoView : UIView
    {
        #region Attributes
        private Store Store;
        #endregion

        #region Constructors 
        static MarkerInfoView(){}
        protected MarkerInfoView(IntPtr handle) : base(handle){}
        #endregion

        #region Override methods
        public override void AwakeFromNib()
        {
            closeButton.Hidden = true;
        }
        #endregion

        #region Methods
        public static MarkerInfoView Create()
        {
            return NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MarkerInfoView, null, null).GetItem<MarkerInfoView>(0);
        }

        public void LoadData(Store store){
            this.Store = store;
            storeNameLabel.Text = store.DependencyName ?? store.Name;
            addressLabel.Text = store.Address;
            telephoneLabel.Text = "Tel: " + store.PhoneNumber;
            foreach(DependencyService service in store.Services){
                servicesDescriptionLabel.Text += service.ServiceName + "\n";
            }
            scheduleDescriptionLabel.Text = store.Schedules;
        }
        #endregion
    }
}

