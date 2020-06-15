using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites.InStoreServices;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.Views.UserViews.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class StickersViewSource : UICollectionViewSource
    {
        #region attributes
        private IList<Sticker> stickers;

        #endregion

        public StickersViewSource(IList<Sticker> stickers ) 
        {
            this.stickers = stickers;
        }

        #region Overrides Methods 
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            try
            {
                return stickers.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            StickersViewCell cell = (StickersViewCell)collectionView.DequeueReusableCell(ConstantIdentifier.StickersViewCellIdentifier, indexPath);
            Sticker row = stickers[indexPath.Row];
            cell.LoadSticker(stickers[indexPath.Row]);
            return cell;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            nfloat width = collectionView.Frame.Width / 5 ;
            return new CGSize(width, width);
        }
        #endregion


    }
}

