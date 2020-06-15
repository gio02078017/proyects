using System;
using Foundation;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    public partial class PreviewEventHeaderMyDiscount : UIView
    {
        #region Attributes
        public static readonly NSString Key = new NSString("PreviewEventHeaderMyDiscount");
        public static readonly UINib Nib;
        #endregion

        #region Constructors
        static PreviewEventHeaderMyDiscount()
        {
            Nib = UINib.FromName("PreviewEventHeaderMyDiscount", NSBundle.MainBundle);
        }

        protected PreviewEventHeaderMyDiscount(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static PreviewEventHeaderMyDiscount Create()
        {
            return NSBundle.MainBundle.LoadNib(nameof(PreviewEventHeaderMyDiscount), null, null).GetItem<PreviewEventHeaderMyDiscount>(0);
        }
        #endregion

        #region Public Methods
        public void SetHeaderCampaing(string text)
        {
            if (string.IsNullOrEmpty(text)){
                headerCampaingLabel.Hidden = true;
            }
            else
            {
                headerCampaingLabel.Hidden = false;
                headerCampaingLabel.Text = text;
            }
        }
        #endregion
    }
}
