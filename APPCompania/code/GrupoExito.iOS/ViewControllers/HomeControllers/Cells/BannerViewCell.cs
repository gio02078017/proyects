using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using Shimmer;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Cells
{
    public partial class BannerViewCell : UICollectionViewCell
    {
        #region Attributes
        private BannerPromotion banner;
        private FBShimmeringView shimmeringView;
        #endregion

        #region Properties
        #endregion

        #region Constructors 
        static BannerViewCell()
        {
            //Static default Constructor
        }
        protected BannerViewCell(IntPtr handle) : base(handle)
        {
            //Default constructor with parameter
        }
        #endregion

        #region Overrides methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
        }
        #endregion

        #region Methods 
        public void LoadBanner(BannerPromotion banner)
        {
            this.banner = banner;
            if (shimmeringView == null)
            {
                shimmeringView = new FBShimmeringView()
                {
                    Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height)
                };
                ContentView.Add(shimmeringView);
            }
            shimmeringView.ContentView = containerView;
            shimmeringView.ShimmeringOpacity = ConstantStyle.ShimmeringOpacity;
            shimmeringView.ShimmeringAnimationOpacity = ConstantStyle.AnimationShimmeringOpacity;
            shimmeringView.ShimmeringSpeed = ConstantDuration.SpeedShimmering;
            shimmeringView.ShimmeringHighlightLength = ConstantStyle.ShimmeringHighlightLength;
            shimmeringView.Shimmering = true;
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;

            if (string.IsNullOrEmpty(banner.Image))
            {
                DrawTemplate();
            }
            else
            {
                ClearTemplate();
                bannerImageView.SetImage(new NSUrl(banner.Image), HandleSDExternalCompletionHandler);
            }
        }

        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
            shimmeringView.Shimmering = false;
        }

        private void DrawTemplate()
        {
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            bannerImageView.BackgroundColor = UIColor.Clear;
            containerView.BackgroundColor = ConstantColor.UIBackgroundShimmer;
        }

        private void ClearTemplate()
        {
            containerView.BackgroundColor = UIColor.White;
        }
        #endregion
    }
}
