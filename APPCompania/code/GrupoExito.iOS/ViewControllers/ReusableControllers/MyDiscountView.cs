using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    public partial class MyDiscountView : UIView
    {
        #region Properties 
        public UILabel Title{
            get { return titleLabel; }
        }

        public UIView ContainerView
        {
            get { return containerView; }
        }

        public UICollectionView DiscountCollection{
            get { return myDiscountCollectionView; }
        }

        public UIButton Next{
            get { return nextButton; }
        }

        public UIButton Before{
            get { return beforeButton; }
        }
        #endregion

        #region Constructors
        public MyDiscountView(IntPtr handle) : base(handle)
        {
            //Default constructor this class
        }
        public MyDiscountView() 
        {
            //Default constructor this class 
        }
        #endregion

        #region Overrides Methods
        public override void AwakeFromNib()
        {
            myDiscountCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.MyDiscountCollectionViewCell, NSBundle.MainBundle), ConstantIdentifier.MyDiscountIdentifier);
            beforeImageView.Transform = CGAffineTransform.MakeRotation((float)Math.PI);
        }
        #endregion

        #region Methods
        public void HiddenNext(){
            nextImageView.Hidden = true;
            nextButton.Hidden = true;
        }

        public void HiddenBefore(){
            beforeImageView.Hidden = true;
            beforeButton.Hidden = true;
        }

        public void ShowNext()
        {
            nextImageView.Hidden = false;
            nextButton.Hidden = false;
        }

        public void ShowBefore()
        {
            beforeImageView.Hidden = false;
            beforeButton.Hidden = false;
        }
        #endregion
    }
}

