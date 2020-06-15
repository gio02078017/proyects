using System;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class FilterHeaderView : UITableViewHeaderFooterView
    {
        #region Attributes
        public static readonly NSString Key = new NSString("FilterHeaderView");
        public static readonly UINib Nib;
        #endregion

        #region Properties 
        public UIButton Header
        {
            get { return headerButton; }
            set { headerButton = value; }
        }
        #endregion

        #region Constructors
        static FilterHeaderView()
        {
            Nib = UINib.FromName("FilterHeaderView", NSBundle.MainBundle);
        }

        protected FilterHeaderView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static FilterHeaderView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(nameof(FilterHeaderView), null, null);
            var v = Runtime.GetNSObject<FilterHeaderView>(arr.ValueAt(0));
            return v;
        }
        #endregion

        #region Public Methods
        #endregion
    }
}

