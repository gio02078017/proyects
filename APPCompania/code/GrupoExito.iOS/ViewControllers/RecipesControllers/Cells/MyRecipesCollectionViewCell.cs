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
    public partial class MyRecipesCollectionViewCell : UICollectionViewCell
    {
        #region Properties
        private FBShimmeringView shimmeringView;
        #endregion

        #region Constructors
        static MyRecipesCollectionViewCell() { }
        protected MyRecipesCollectionViewCell(IntPtr handle) : base(handle) { }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadFonts();
            this.LoadCorners();
        }
        #endregion

        #region Methods
        private void LoadFonts()
        {
            titleRecipeLabel.Layer.ShadowRadius = 3;
            titleRecipeLabel.Layer.ShadowOpacity = 5;
            timeLabel.Layer.ShadowRadius = 3;
            timeLabel.Layer.ShadowOpacity = 5;
            DifficultyLabel.Layer.ShadowRadius = 3;
            DifficultyLabel.Layer.ShadowOpacity = 5;
            backgroundImageView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadCorners()
        {
            this.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        public void LoadCategorysViewCollection(Recipe recipe)
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
            backgroundImageView.SetImage(new NSUrl(recipe.ShortImage), UIImage.FromFile(ConstantImages.SinImagen), HandleSDExternalCompletionHandler);
            titleRecipeLabel.Text = recipe.Title;
            timeLabel.Text = recipe.PreparationTime;
            DifficultyLabel.Text = recipe.Difficulty;
        }
        #endregion

        #region Events
        void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
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
                Util.LogException(exception, ConstantReusableViewName.MyRecipesCollectionViewCell, ConstantEventName.HandleSDExternalCompletionHandler);
            }
        }
        #endregion
    }
}