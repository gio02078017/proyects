using System;

using Foundation;
using GrupoExito.Models.ViewModels.Payments;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Views
{
    public partial class SearchCell : GenericCell
    {
        public static readonly NSString Key = new NSString("SearchCell");
        public static readonly UINib Nib;

        static SearchCell()
        {
            Nib = UINib.FromName("SearchCell", NSBundle.MainBundle);
        }

        protected SearchCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void Setup(object v)
        {
            if (v is SearchViewModel model)
            {

            }
        } 
    }
}
