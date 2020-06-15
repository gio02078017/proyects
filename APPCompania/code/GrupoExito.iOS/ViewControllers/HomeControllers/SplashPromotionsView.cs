using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using UIKit;
using Runtime = ObjCRuntime.Runtime;

namespace GrupoExito.iOS.ViewControllers.HomeControllers
{
    public partial class SplashPromotionsView : UIView
    {
        #region Attributes
        private IList<Promotion> promotions;
        private EventHandler removeEvent;
        private int NumberSlider = 0;
        #endregion

        #region Properties 
        public IList<Promotion> Promotions { get => promotions; set => promotions = value; }
        public EventHandler RemoveEvent { get => removeEvent; set => removeEvent = value; }
        #endregion

        #region Constructors
        public SplashPromotionsView(IntPtr handle) : base(handle) { }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            WireUpSwipeRight();
            WireUpSwipeLeft();
            LoadHandlers();
        }
        #endregion

        #region static Methods
        public static SplashPromotionsView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.SplashPromotionsView, null, null);
            var v = Runtime.GetNSObject<SplashPromotionsView>(arr.ValueAt(0));
            return v;
        }

        public void LoadData(IList<Promotion> Promotions)
        {
            this.promotions = Promotions;
            NumberSlider = 0;
            UIImage [] imageSplashLoading = Util.LoadAnimationImage(ConstantImages.FolderSplashLoad, ConstantViewSize.FolderSplashLoad);
            promotionImageView.Image = imageSplashLoading[0];
            promotionImageView.AnimationImages = imageSplashLoading;
            promotionImageView.AnimationDuration = ConstantDuration.AnimationImageSplashPromotionLoading;
            promotionImageView.StartAnimating();
            promotionImageView.ContentMode = UIViewContentMode.Center;
            promotionImageView.SetImage(new NSUrl(promotions[NumberSlider].UrlImage), HandleSDExternalCompletionHandler);
            promotionPageControl.CurrentPage = 0;
            promotionPageControl.Pages = promotions.Count;
            PageControlEditingChanged(promotionPageControl, null);
            ValidateDoNotShowAgain();
        }

        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
            this.BackgroundColor = ConstantColor.UiPrimary;
            promotionImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
            promotionImageView.StopAnimating();
        }
        #endregion

        #region Public Methods
        private void LoadHandlers()
        {
            promotionButton.TouchUpInside += PromotionButtonTouchUpInside;
            doNotShowAgainButton.TouchUpInside += DoNotShowAgainButton_TouchUpInside;
            promotionPageControl.EditingChanged += PageControlEditingChanged;;
            promotionPageControl.ValueChanged += PageControlEditingChanged;
        }

        private void WireUpSwipeRight()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            promotionButton.AddGestureRecognizer(gesture);
        }

        private void WireUpSwipeLeft()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            promotionButton.AddGestureRecognizer(gesture);
        }

        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
            if (recognizer.Direction == UISwipeGestureRecognizerDirection.Right)
            {
                if (promotions.Any())
                {
                    NumberSlider -= 1;
                    if (NumberSlider < 0)
                    {
                        NumberSlider = promotions.Count - 1;
                    }
                    promotionPageControl.CurrentPage = NumberSlider;
                    PageControlEditingChanged(promotionPageControl, null);
                    promotionImageView.SetImage(new NSUrl(promotions[NumberSlider].UrlImage));
                }
            }
            else if (recognizer.Direction == UISwipeGestureRecognizerDirection.Left)
            {
                if (promotions.Any())
                {
                    NumberSlider += 1;
                    if (NumberSlider > (promotions.Count - 1))
                    {
                        NumberSlider = 0;
                        removeEvent?.Invoke(recognizer, null);
                    }
                    else
                    {
                        promotionPageControl.CurrentPage = NumberSlider;
                        PageControlEditingChanged(promotionPageControl, null);
                    }
                    promotionImageView.SetImage(new NSUrl(promotions[NumberSlider].UrlImage));
                }
            }
            ValidateDoNotShowAgain();
        }
        #endregion

        #region Methods privates
        private void ValidateDoNotShowAgain()
        {
            if(NumberSlider == promotions.Count - 1)
            {
                doNotShowAgainButton.Hidden = false;
            }
            else
            {
                doNotShowAgainButton.Hidden = true;
            }
        }

        private void CustomPageControl()
        {
            try
            {
                var x = 0;
                var width = 15;
                var height = 15;
                for (int i = 0; i < promotionPageControl.Subviews.Length; i++)
                {
                    UIView dot = promotionPageControl.Subviews[i];
                    dot.Frame = new CGRect(x, dot.Frame.Y, width, height);
                    dot.Layer.BorderWidth = 2;
                    dot.Layer.CornerRadius = dot.Frame.Width / 2;
                    dot.Layer.BorderColor = UIColor.Clear.CGColor;
                    if (i == promotionPageControl.CurrentPage)
                    {
                        dot.BackgroundColor = ConstantColor.UiPageControlDot;
                    }
                    else
                    {
                        dot.BackgroundColor = UIColor.White;
                    }
                }
                promotionPageControl.ReloadInputViews();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.ConfigurePageControl);
            }
        }
        #endregion

        #region Events
        private void PageControlEditingChanged(object sender, EventArgs e)
        {
            CustomPageControl();
        }

        private void DoNotShowAgainButton_TouchUpInside(object sender, EventArgs e)
        {
            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.DoNotShowAgain, true);
            removeEvent?.Invoke(sender, null);
        }

        private void PromotionButtonTouchUpInside(object sender, EventArgs e)
        {
            if(NumberSlider < promotions.Count - 1)
            {
                NumberSlider++;
                promotionPageControl.CurrentPage = NumberSlider;
                PageControlEditingChanged(promotionPageControl, null);
                promotionImageView.SetImage(new NSUrl(promotions[NumberSlider].UrlImage));
            }
            else
            {
                removeEvent?.Invoke(sender, null);
            }
            ValidateDoNotShowAgain();
        }
        #endregion
    }
}

