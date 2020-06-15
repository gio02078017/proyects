using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using Shimmer;
using UIKit;


namespace GrupoExito.iOS.ViewControllers.RecipesControllers.Cells
{
    public partial class MyRecipesCategoryTableViewCell : UITableViewCell
    {
        #region Properties
        private FBShimmeringView shimmeringView;
        #endregion

        #region Constructors 
        static MyRecipesCategoryTableViewCell() { }
        protected MyRecipesCategoryTableViewCell(IntPtr handle) : base(handle) { }
        #endregion

        #region Methods 
        public void LoadCategorysViewCell(RecipeCategory category)
        {
            if (shimmeringView == null)
            {
                shimmeringView = new FBShimmeringView()
                {
                    Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height)
                };
                ContentView.Add(shimmeringView);
            }
            shimmeringView.ContentView = containerView;
            shimmeringView.ShimmeringOpacity = 0.1f;
            shimmeringView.ShimmeringAnimationOpacity = 1f;
            shimmeringView.ShimmeringSpeed = 200;
            shimmeringView.ShimmeringHighlightLength = 1;
            shimmeringView.Shimmering = true;
            iconImageView.SetImage(new NSUrl(category.Image), UIImage.FromFile(ConstantImages.SinImagen), HandleSDExternalCompletionHandler);
            nameCategoryLabel.Text = category.Name;
            descriptionCategoryLabel.Text = category.Description;
        }
        #endregion

        #region Events
        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
            try
            {
                if (shimmeringView != null)
                {
                    shimmeringView.Shimmering = false;
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.MyRecipesCategoryTableViewCell, ConstantEventName.HandleSDExternalCompletionHandler);
            }
        }
        #endregion
    }
}
