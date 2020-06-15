using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.HomeControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class AccessInitialSource : UICollectionViewSource
    {
        #region Attributes
        private IList<MenuItem> menuItems;
        #endregion


        #region Properties
        public IList<MenuItem> MenuItems { get => menuItems; set => menuItems = value; }
        #endregion

        public AccessInitialSource(IList<MenuItem> MenuItems)
        {
            this.menuItems = MenuItems;
        }

        #region Overrides Methods 
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 4;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {

            switch (section)
            {
                case 0:
                case 1:
                    return 1;
                case 2:
                    return menuItems.Count;
                default:
                    return 1;
            }
        }


        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
                switch (indexPath.Section)
                {
                    case 0:
                        HeaderInitialAccessViewCell Headercell = (HeaderInitialAccessViewCell)collectionView.DequeueReusableCell(HeaderInitialAccessViewCell.Key, indexPath);
                        return Headercell;
                    case 1:
                        TitleSectionInitialAccesViewCell Titlecell = (TitleSectionInitialAccesViewCell)collectionView.DequeueReusableCell(TitleSectionInitialAccesViewCell.Key, indexPath);
                        return Titlecell;
                    case 2:
                        AccessInitialViewCell cell = (AccessInitialViewCell)collectionView.DequeueReusableCell(AccessInitialViewCell.Key, indexPath);
                        cell.LoadData(MenuItems, indexPath);
                        return cell;
                    default:
                        FooterInitialAccessViewCell FooterCell = (FooterInitialAccessViewCell)collectionView.DequeueReusableCell(FooterInitialAccessViewCell.Key, indexPath);
                        return FooterCell;
                }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            try
            {
                nfloat width = (collectionView.Frame.Size.Width);
                switch (indexPath.Section)
                {
                    case 0:
                        return new CGSize(width, 225);
                    case 1:
                        return new CGSize(width, 60);
                    case 2:
                        if (indexPath.Row > 0 || menuItems.Count % 2 == 0)
                        {
                            width = width / 2;
                        }
                        return new CGSize(width, ConstantViewSize.AccountMenuHeightCell);
                    default:
                        return new CGSize(width, 70);
                }
            }
            catch (Exception)
            {
                return new CGSize(0, 0);
            }
        }




        #endregion
    }
}

