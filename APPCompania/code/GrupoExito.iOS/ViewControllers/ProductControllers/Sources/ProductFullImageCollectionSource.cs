using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ProductControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Sources
{
    public partial class ProductFullImageCollectionSource : UICollectionViewSource
    {
        #region Attributes
        public IList<String> ProductsFullImages;
        #endregion

        #region Constructors
        public ProductFullImageCollectionSource(IList<String> productsFullImages)
        {
            this.ProductsFullImages = productsFullImages;
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
                return ProductsFullImages.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            try
            {
                var cell = collectionView.DequeueReusableCell(ConstantIdentifier.ImageFullIdentifier, indexPath) as ProductFullImageCollectionViewCell;
                String row = ProductsFullImages[indexPath.Row];
                cell.LoadFullImage(row);
                return cell;
            }
            catch (Exception e)
            {
                var data = e.Message;
                return null;
            }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            try
            {
                nfloat width = (collectionView.Superview.Frame.Size.Width);
                nfloat height = (collectionView.Superview.Frame.Size.Height);
                return new CGSize(width, height);
            }
            catch (Exception)
            {
                return new CGSize(100, 100);
            }
        }
        #endregion
    }
}

