using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.Logic.Models;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.RecipesControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.RecipesControllers.Sources
{


    public class MyRecipesViewSource : UICollectionViewSource
    {
        #region Attributes
        private IList<Recipe> Recipes;
        public UIViewControllerBase controllerBase;
        #endregion

        #region Constructors
        public MyRecipesViewSource(IList<Recipe> recipes, MyRecipesViewController myRecipesViewController)
        {
            this.Recipes = recipes;
            this.controllerBase = myRecipesViewController;
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
             return Recipes.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
                MyRecipesCollectionViewCell cell = collectionView.DequeueReusableCell(ConstantIdentifier.MyRecipesIdentifier, indexPath) as MyRecipesCollectionViewCell;
                Recipe row = Recipes[indexPath.Row];
                try
                {
                    cell.LoadCategorysViewCollection(row);

                }
            catch(Exception){

                }
                return cell;
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {

            DetailRecipeViewController detailRecipeViewController = (DetailRecipeViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.DetailRecipeViewController);
            detailRecipeViewController.HidesBottomBarWhenPushed = true;
            detailRecipeViewController.Recipe = Recipes[indexPath.Row];
            controllerBase.NavigationController.PushViewController(detailRecipeViewController, true);

        }

        public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
           
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
          
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            try
            {
                nfloat value = collectionView.Frame.Width; 
                return new CGSize(value, ConstantViewSize.CollectionRecipeHeightCell);
            }
            catch (Exception)
            {
                return new CGSize(100, 100);
            }
        }
        #endregion
    }
}
