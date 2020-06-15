using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites.Products;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ListControllers;
using GrupoExito.iOS.ViewControllers.ProductControllers.Cells;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Sources
{
    public class ProductsCollectionViewSource : UICollectionViewSource
    {
        #region Attributes
        private IList<Product> products;
        private String TypeCollection;
        private bool IsSearcherProduct { get; set; }
        private BaseProductController ControllerBase;
        #endregion

        #region Attributes Events
        private EventHandler actionScrolled;
        private EventHandler actionAddingRemoveProducts;
        private EventHandler actionSummaryProducts;
        #endregion

        #region Properties 
        public IList<Product> Products { get => products; set => products = value; }
        public EventHandler ActionScrolled { get => actionScrolled; set => actionScrolled = value; }
        public EventHandler ActionAddingRemoveProducts { get => actionAddingRemoveProducts; set => actionAddingRemoveProducts = value; }
        public EventHandler ActionSummaryProducts { get => actionSummaryProducts; set => actionSummaryProducts = value; }
        #endregion

        #region Constructors
        public ProductsCollectionViewSource(IList<Product> products, String typeCollection, BaseProductController controllerBase, bool isSearcherProduct)
        {
            this.Products = products;
            this.TypeCollection = typeCollection;
            this.ControllerBase = controllerBase;
            this.IsSearcherProduct = isSearcherProduct;
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
                return Products.Count;
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
                ProductViewCell cell = collectionView.DequeueReusableCell(ConstantIdentifier.ProductsIdentifier, indexPath) as ProductViewCell;
                Product row = Products[indexPath.Row];
                cell.IsSearcherProduct = IsSearcherProduct;
                cell.ActionSelected = CellActionSelected;
                cell.ActionAddProductToList = CellActionAddToList;
                cell.ActionAddingAndRemoveProducts = CellActionAddingAndRemoveProducts;
                cell.ActionSummary = CellActionSummary;
                cell.LoadProductViewCell(row);
                cell.DrawBorderForType(TypeCollection, indexPath, products.Count);
                return cell;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantEventName.GetCell);
                return null;
            }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            nfloat width = (collectionView.Frame.Width) / 2;
            try
            {
                Product currentProduct = Products[indexPath.Row];
                if (currentProduct != null && currentProduct.SiteId != null && currentProduct.SiteId.Equals(AppMessages.Template))
                {
                    return new CGSize(width, ConstantViewSize.ProductHeightCell);
                }
                else
                {
                    if (TypeCollection.Equals(ConstantIdentifier.Discount_products))
                    {
                        int countAdded = products.Where((arg) => arg.Quantity > 0).Count();
                        if(countAdded > 0)
                        {
                            return new CGSize(width, ConstantViewSize.ProductHeightWithProductsAdded);
                        }
                        return new CGSize(width, ConstantViewSize.ProductHeightCell);
                    }
                    else
                    {
                        Product productNext = null;
                        int row = indexPath.Row;
                        bool containProductAdded = false;
                        if (row % 2 == 0)
                        {
                            if (((row + 1) <= products.Count - 1))
                            {
                                productNext = Products[indexPath.Row + 1];
                            }
                        }
                        else
                        {
                            if (((row - 1) >= 0))
                            {
                                productNext = Products[indexPath.Row - 1];
                            }
                        }
                        if (currentProduct.Quantity > 0 || (productNext != null && productNext.Quantity > 0))
                        {
                            containProductAdded = true; ;
                        }
                        if (containProductAdded)
                        {
                            return new CGSize(width, ConstantViewSize.ProductHeightWithProductsAdded);
                        }
                        else
                        {
                            return new CGSize(width, ConstantViewSize.ProductHeightCell);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.BaseProductController, ConstantMethodName.DrawProducts);
                return new CGSize(width, ConstantViewSize.ProductHeightCell);
            }
        }
        #endregion

        #region Events
        private void CellActionSelected(object sender, EventArgs e)
        {
            ParametersManager.Products = Products;
            ProductDetailViewController productDetailViewController = (ProductDetailViewController)this.ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.ProductDetailViewController);
            if (productDetailViewController != null)
            {
                if (TypeCollection.Equals(ConstantIdentifier.Discount_products) || TypeCollection.Equals(ConstantIdentifier.Favorite_products))
                {
                    //RegisterFromHomeViewAppear(AnalyticsEvent.HomeProductDetail, (Product)sender);
                }
                //else if (TypeCollection.Equals(ConstantIdentifier.Products_by_category))
                //{
                //    RegisterFromCategoryViewAppear(AnalyticsEvent.ProductListProductDetail, (Product)sender);
                //}

                RegisterEvent((Product)sender);

                productDetailViewController.ProductCurrent = (Product)sender;
                productDetailViewController.HidesBottomBarWhenPushed = true;
                productDetailViewController.IsSearcherProduct = IsSearcherProduct;
                ControllerBase.NavigationController.PushViewController(productDetailViewController, true);
            }
        }

        private void CellActionAddToList(object sender, EventArgs e)
        {
            MyCustomListViewController myCustomListViewController = (MyCustomListViewController)ControllerBase.Storyboard.InstantiateViewController(ConstantControllersName.MyCustomListViewController);
            string value = JsonService.Serialize<Product>((Product)sender);
            ProductList productList = new ProductList();
            productList = JsonService.Deserialize<ProductList>(value);
            myCustomListViewController.Product = productList;
            myCustomListViewController.HidesBottomBarWhenPushed = true;
            ParametersManager.ShoppingListSelectedId = "";
            ControllerBase.NavigationController.PushViewController(myCustomListViewController, true);
        }

        private void RegisterEvent(Product product)
        {
            FirebaseEventRegistrationService.Instance.ProductClic(product, product.CategoryName);
            FirebaseEventRegistrationService.Instance.ProductDetail(product, product.CategoryName);
            FacebookEventRegistrationService.Instance.ViewedContent(product);
        }

        private void RegisterFromCategoryViewAppear(string eventName, Product product)
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventProductRelated(eventName, product, ConstantControllersName.ProductByCategoryViewController);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEventProductRelated(eventName, product, ConstantControllersName.ProductByCategoryViewController);
        }

        private void RegisterFromHomeViewAppear(string eventName, Product product)
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventProductRelated(eventName, product, ConstantControllersName.HomeViewController);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEventProductRelated(eventName, product, ConstantControllersName.HomeViewController);
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            ActionScrolled?.Invoke(scrollView, null);
        }

        private void CellActionAddingAndRemoveProducts(object sender, EventArgs e)
        {
            ActionAddingRemoveProducts?.Invoke(sender, e);
        }

        private void CellActionSummary(object sender, EventArgs e)
        {
            ActionSummaryProducts?.Invoke(sender, e);
        }
        #endregion
    }
}

