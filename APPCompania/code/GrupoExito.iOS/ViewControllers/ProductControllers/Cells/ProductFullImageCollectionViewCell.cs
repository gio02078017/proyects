using System;
using Foundation;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    public partial class ProductFullImageCollectionViewCell : UICollectionViewCell
    {
        #region Constructors
        static ProductFullImageCollectionViewCell() 
        {
            //Static default constructor this class 
        }

        protected ProductFullImageCollectionViewCell(IntPtr handle) : base(handle) 
        {
            //Default constructor this class 
        }
        #endregion

        #region Methods
        public void LoadFullImage(string url)
        {
            productFullImageImageView.SetImage(new NSUrl(url));
        }
        #endregion
    }
}
