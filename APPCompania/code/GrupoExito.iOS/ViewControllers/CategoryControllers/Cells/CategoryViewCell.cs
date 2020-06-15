using System;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using SDWebImage;
using Shimmer;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.CategoryControllers.Cells
{
    public partial class CategoryViewCell : UICollectionViewCell
    {
        #region Attributes
        private Category _category;
        public Category Category { get => _category; set => _category = value; }
        FBShimmeringView shimmeringView;

        public static readonly NSString Key = new NSString("CategoryViewCell");
        public static readonly UINib Nib;
        #endregion

        #region Constructors 
        static CategoryViewCell()
        {
            Nib = UINib.FromName("CategoryViewCell", NSBundle.MainBundle);
        }

        protected CategoryViewCell(IntPtr handle) : base(handle) { }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib(){}
        #endregion

        #region Methods 
        public void LoadCategoryViewCell(Category category)
        {
            shimmeringView = new FBShimmeringView
            {
                Frame = new CGRect(0, 0, ContentView.Frame.Width, ContentView.Frame.Height)
            };
            ContentView.Add(shimmeringView);
            shimmeringView.ContentView = containerView;
            shimmeringView.ShimmeringOpacity = ConstantStyle.ShimmeringOpacity;
            shimmeringView.ShimmeringAnimationOpacity = ConstantStyle.AnimationShimmeringOpacity;
            shimmeringView.ShimmeringSpeed = ConstantDuration.SpeedShimmering;
            shimmeringView.ShimmeringHighlightLength = ConstantStyle.ShimmeringHighlightLength;
            shimmeringView.Shimmering = true;
            shimmeringView.ShimmeringDirection = FBShimmerDirection.Left;
            if (category.SiteId.Equals(AppMessages.Template))
            {
                DrawTemplate();
            }
            else
            {
                nameCategoryLabel.Text = category.Name;
                iconCategoryGrayImageView.SetImage(new NSUrl(category.IconCategory), HandleSDExternalCompletionHandler);
                iconCategoryImageView.SetImage(new NSUrl(category.ImageCategory), HandleSDExternalCompletionHandler);
                iconCategoryImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            }
        }

        private void DrawTemplate()
        {
            iconCategoryImageView.BackgroundColor = UIColor.Clear;
            iconCategoryGrayImageView.BackgroundColor = UIColor.Clear;
            containerView.BackgroundColor = ConstantColor.UIBackgroundShimmer;
        }

        private void LoadFonts()
        {
            nameCategoryLabel.Font = UIFont.FromName(ConstantFontSize.LetterTitle, ConstantFontSize.TitleGeneric);
        }
        #endregion

        #region Events
        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
            try
            {
                if (error == null && shimmeringView != null)
                {
                    containerView.BackgroundColor = UIColor.Clear;
                    containerView.Layer.BorderColor = UIColor.Clear.CGColor;
                    containerView.Layer.BorderWidth = 0;
                    shimmeringView.Shimmering = false;
                }
            }
            catch(Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.CategoryViewCell, ConstantEventName.HandleSDExternalCompletionHandler);
            }
        }
        #endregion
    }
}
