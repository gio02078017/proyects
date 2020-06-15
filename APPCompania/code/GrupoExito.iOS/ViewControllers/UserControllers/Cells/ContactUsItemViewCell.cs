using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Cells
{
    public partial class ContactUsItemViewCell : UITableViewCell
    {
        #region Attributes 
        private EventHandler actionCallStorePhone;
        #endregion

        #region Properties
        public EventHandler ActionCallStorePhone { get => actionCallStorePhone; set => actionCallStorePhone = value; }
        #endregion

        #region Constructors 
        static ContactUsItemViewCell()
        {
            // Implementation off ContactUsItemViewCell
        }

        protected ContactUsItemViewCell(IntPtr handle) : base(handle)
        {
            // Implementation off ContactUsItemViewCell
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadHandlers();
        }
        #endregion

        #region Methods 
        public void LoadData(Contact item)
        {
            storeNameLabel.Text = item.Name;

            if(item.Number.StartsWith("01 8000", StringComparison.CurrentCulture)){
                storePhoneButton.Enabled = false;
                storePhoneButton.SetTitleColor(UIColor.Black, UIControlState.Normal);
            }

            storePhoneButton.SetTitle(item.Number, UIControlState.Normal);
        }

        private void LoadHandlers()
        {
            storePhoneButton.TouchUpInside += StorePhoneButton_TouchUpInside;
        }
        #endregion

        #region Events
        private void StorePhoneButton_TouchUpInside(object sender, EventArgs e)
        {
            ActionCallStorePhone(sender, e);
        }
        #endregion
    }
}
