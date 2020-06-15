using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.CategoryControllers.Cells;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.CategoryControllers.Sources
{
    public partial class CategoryCollectionViewSource : UICollectionViewSource
    {
        #region Attributtes
        private IList<Category> categories;
        private EventHandler selectAction;
        #endregion

        #region Properties
        public IList<Category> Categories { get => categories; set => categories = value; }
        public EventHandler SelectAction { get => selectAction; set => selectAction = value; }
        #endregion

        #region Constructors 
        public CategoryCollectionViewSource(IList<Category> categories)
        {
            this.Categories = categories;
        }
        #endregion

        #region Override Methods 
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 2;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            switch(section)
            {
                case 0:
                    return 1;
                default:
                    return categories.Count;
            }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            nfloat value = collectionView.Frame.Width;
            switch (indexPath.Section)
            {
                case 0:
                    return new CGSize(value, 80);
                default:
                    value /= 2;
                    return new CGSize(value, ConstantViewSize.CategoryHeightCell);
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    HeaderCategoryViewCell headerCell = (HeaderCategoryViewCell)collectionView.DequeueReusableCell(HeaderCategoryViewCell.Key, indexPath);
                    return headerCell;
                default:
                    CategoryViewCell cell = (CategoryViewCell)collectionView.DequeueReusableCell(CategoryViewCell.Key, indexPath);
                    Category row = Categories[indexPath.Row];
                    cell.LoadCategoryViewCell(row);
                    return cell;
            }
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 1)
            {
                Category category = this.Categories[(int)indexPath.Row];
                if (category.SiteId == null || !category.SiteId.Equals(AppMessages.Template))
                {
                    selectAction?.Invoke(category, null);
                }
            }
        }
        #endregion
    }
}