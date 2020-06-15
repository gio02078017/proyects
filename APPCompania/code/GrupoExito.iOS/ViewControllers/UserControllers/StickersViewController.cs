using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities.Entiites.InStoreServices;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    public partial class StickersViewController : UIViewControllerBase
    {
        #region Attributes
        private StickersModel stickersModel;
        private IList<StickersByPage> stickersbyPage;
        private bool openFromInitialOption = false;
        private int NumberSlider = 0;
        //private PopUpInformationView howDoYouLikeItView;
        private MessageStatusView messageStatusView;
        private NSString termsAndConditions = new NSString("TermsAndConditionStickers");
        #endregion

        #region Properties
        public bool OpenFromInitialOption { get => openFromInitialOption; set => openFromInitialOption = value; }
        #endregion

        public StickersViewController(IntPtr handle) : base(handle)
        {
            stickersModel = new StickersModel(new StickersService(DeviceManager.Instance));
        }

        #region Life Cycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            this.LoadExternalViews();
            this.LoadHandlers();
            this.GetTickers();
            this.LoadBold();

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(!OpenFromInitialOption, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.HiddenAccountProfile();
                NavigationView.EnableBackButton(true);
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StickersViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }
        #endregion

        #region Methods

        private void LoadBold()
        {
            var attributedText = new NSMutableAttributedString("Acumula stickers por cada compra que realices en nuestros almacenes, máximo 3 stickers por transacción. ");
            var infoText = new NSMutableAttributedString("Ver más", UIFont.BoldSystemFontOfSize(17f), ConstantColor.UiBorderColorButton);


            attributedText.Append(infoText);


            termsLabel.AttributedText = attributedText;

        }

        private void LoadExternalViews()
        {
            try
            {
                strickersCollectionView.RegisterNibForCell(UINib.FromName(ConstantReusableViewName.StickersViewCell, NSBundle.MainBundle), ConstantIdentifier.StickersViewCellIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                this.NavigationController.NavigationBarHidden = false;
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
                CustomSpinnerViewFromBase = customSpinnerView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StickersViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadHandlers()
        {
            stickersPagination.EditingChanged += StickersPaginationEditingChanged;
            termsButton.TouchUpInside += TermsButtonTouchUpInside;
            WireUpSwipeRight();
            WireUpSwipeLeft();

        }

        private void LoadDateSticker(string date)
        {
            var attributedText2 = new NSMutableAttributedString("Actualizado: ");
            var infoText2 = new NSMutableAttributedString(date, UIFont.BoldSystemFontOfSize(17f));

            attributedText2.Append(infoText2);
            label.AttributedText = attributedText2;
        }

        private void CustomPageControl()
        {
            try
            {
                var x = 0;
                var width = 15;
                var height = 15;

                for (int i = 0; i < stickersPagination.Subviews.Length; i++)
                {
                    UIView dot = stickersPagination.Subviews[i];
                    dot.Frame = new CGRect(x, dot.Frame.Y, width, height);
                    dot.Layer.BorderWidth = 1;
                    dot.Layer.CornerRadius = dot.Frame.Width / 2;
                    dot.Layer.BorderColor = new CGColor(0, 0, 0, 1);

                    if (i == stickersPagination.CurrentPage)
                    {
                        dot.BackgroundColor = ConstantColor.UiPageControlDot;
                    }
                    else
                    {
                        dot.BackgroundColor = UIColor.Clear;
                    }
                }

                stickersPagination.ReloadInputViews();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.StickersViewController, ConstantMethodName.ConfigurePageControl);
                ShowMessageException(exception);
            }
        }

        #endregion

        #region method async

        private async Task GetTickers()
        {
            StartActivityIndicatorCustom();
            StickersResponse response = await stickersModel.GetSckers();
            stickersbyPage = response.StickersPage;
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                if (response.Result.Messages.Any())
                {
                    string message = MessagesHelper.GetMessage(response.Result);
                    PopUpInformationView informationView = PopUpInformationView.Create("", message);
                    this.NavigationController.SetNavigationBarHidden(true, true);
                    informationView.Frame = View.Bounds;
                    informationView.LayoutIfNeeded();
                    View.AddSubview(informationView);

                    informationView.AcceptButtonHandler += () =>
                    {
                        this.NavigationController.SetNavigationBarHidden(false, true);
                        informationView.RemoveFromSuperview();
                    };
                    informationView.CloseButtonHandler += () =>
                    {
                        this.NavigationController.SetNavigationBarHidden(false, true);
                        informationView.RemoveFromSuperview();
                    };

                    stickersPagination.Hidden = true;
                    if (stickersbyPage[NumberSlider] != null && stickersbyPage[NumberSlider].Stickers.Any())
                    {
                        strickersCollectionView.Source = new StickersViewSource(stickersbyPage[NumberSlider].Stickers);
                        strickersCollectionView.ReloadData();
                    }
                }
              
                if (stickersbyPage != null && stickersbyPage.Any())
                {
                    if (string.IsNullOrEmpty(response.LastUpdateStickers))
                    {
                        label.Hidden = true;
                    }
                    else
                    {
                        this.LoadDateSticker(response.LastUpdateStickers);
                    }
                   
                    stickersPagination.Pages = stickersbyPage.Count;
                    stickersPagination.CurrentPage = NumberSlider;
                    StickersPaginationEditingChanged(stickersPagination, null);
                    strickersCollectionView.Source = new StickersViewSource(stickersbyPage[NumberSlider].Stickers);
                    strickersCollectionView.ReloadData();
                }
                StopActivityIndicatorCustom();
            }

            if (string.IsNullOrEmpty(response.LastUpdateStickers))
            {
                label.Hidden = true;
            }
            else
            {
                this.LoadDateSticker(response.LastUpdateStickers);
            }
            stickersPagination.Pages = stickersbyPage.Count;
            stickersPagination.CurrentPage = NumberSlider;
            StickersPaginationEditingChanged(stickersPagination, null);
            strickersCollectionView.Source = new StickersViewSource(stickersbyPage[NumberSlider].Stickers);
            strickersCollectionView.ReloadData();
            StopActivityIndicatorCustom();
        }

        #endregion

        #region Events Actions
        private void StickersPaginationEditingChanged(object sender, EventArgs e)
        {
            CustomPageControl();
        }

        private void WireUpSwipeRight()
        {

            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            stickersPagination.AddGestureRecognizer(gesture);
            strickersCollectionView.AddGestureRecognizer(gesture);
        }

        private void WireUpSwipeLeft()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            stickersPagination.AddGestureRecognizer(gesture);
            strickersCollectionView.AddGestureRecognizer(gesture);
        }


        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
            if (recognizer.Direction == UISwipeGestureRecognizerDirection.Right)
            {
                if (stickersbyPage.Any())
                {
                    NumberSlider -= 1;

                    if (NumberSlider < 0)
                    {
                        NumberSlider = stickersbyPage.Count - 1;
                    }

                    stickersPagination.CurrentPage = NumberSlider;
                    StickersPaginationEditingChanged(stickersPagination, null);

                    strickersCollectionView.Source = new StickersViewSource(stickersbyPage[NumberSlider].Stickers);
                    strickersCollectionView.ReloadData();


                }
            }
            else if (recognizer.Direction == UISwipeGestureRecognizerDirection.Left)
            {
                if (stickersbyPage.Any())
                {
                    NumberSlider += 1;

                    if (NumberSlider > (stickersbyPage.Count - 1))
                    {
                        NumberSlider = 0;
                    }

                    stickersPagination.CurrentPage = NumberSlider;
                    StickersPaginationEditingChanged(stickersPagination, null);

                    strickersCollectionView.Source = new StickersViewSource(stickersbyPage[NumberSlider].Stickers);
                    strickersCollectionView.ReloadData();
                }
            }
        }

        private void TermsButtonTouchUpInside(object sender, EventArgs e)
        {
            ConditionsStickersViewController termsAndConditionsStickers = (ConditionsStickersViewController)Storyboard.InstantiateViewController(ConstantControllersName.ConditionsStickersViewController);
            termsAndConditionsStickers.HidesBottomBarWhenPushed = true;
            NavigationController.PushViewController(termsAndConditionsStickers, true);
        }

        #endregion
    }
}

