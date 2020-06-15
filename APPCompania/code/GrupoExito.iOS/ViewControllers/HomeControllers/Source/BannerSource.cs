using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.HomeControllers.Source
{
    public class BannerSource : UICollectionViewSource
    {
        #region Attributes
        private IList<BannerPromotion> banners;
        #endregion

        #region Properties
        public IList<BannerPromotion> Banners { get => banners; set => banners = value; }
        public Action<BannerPromotion> BannerAction { get; set; }
        #endregion

        #region Constructors
        public BannerSource(IList<BannerPromotion> banners)
        {
            this.Banners = banners;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            try
            {
                return Banners.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            BannerViewCell cell = collectionView.DequeueReusableCell(ConstantIdentifier.BannersIdentifier, indexPath) as BannerViewCell;
            cell.LoadBanner(Banners[indexPath.Row]);
            return cell;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            nfloat width = (collectionView.Frame.Width) / 1.2f;
            return new CGSize(width, collectionView.Frame.Height);
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            BannerPromotion banner = Banners[indexPath.Row];

            if(!banner.ActionIos.Equals("NotAction"))
            {
                BannerAction?.Invoke(banner);
            }
        }
        #endregion
    }
}
