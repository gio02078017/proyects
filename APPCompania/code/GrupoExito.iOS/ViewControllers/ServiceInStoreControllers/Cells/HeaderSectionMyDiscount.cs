using System;
using CoreGraphics;
using Foundation;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells
{
    public partial class HeaderSectionMyDiscount : UICollectionViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("HeaderSectionMyDiscount");
        public static readonly UINib Nib;

        private bool isPreviewEvent;
        private int optionSelected;
        private StatusHeaderMyDiscount statusHeaderMyDiscount;
        private PreviewEventHeaderMyDiscount PreviewEventHeaderMyDiscount;

        private EventHandler toActivateEvent;
        private EventHandler activatedEvent;
        private EventHandler redeemedEvent;
        private EventHandler tutorialEvent;
        #endregion

        #region Properties
        public EventHandler ToActivateEvent { get => toActivateEvent; set => toActivateEvent = value; }
        public EventHandler ActivatedEvent { get => activatedEvent; set => activatedEvent = value; }
        public EventHandler RedeemedEvent { get => redeemedEvent; set => redeemedEvent = value; }
        public EventHandler TutorialEvent { get => tutorialEvent; set => tutorialEvent = value; }
        public int OptionSelected { get => optionSelected; set => optionSelected = value; }
        public Action<int> CategoryOfTypeChanged { get; set; }
        #endregion

        #region Constructors
        static HeaderSectionMyDiscount()
        {
            Nib = UINib.FromName("HeaderSectionMyDiscount", NSBundle.MainBundle);
        }

        protected HeaderSectionMyDiscount(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public static HeaderSectionMyDiscount Create()
        {
            return NSBundle.MainBundle.LoadNib(nameof(HeaderSectionMyDiscount), null, null).GetItem<HeaderSectionMyDiscount>(0);
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            LoadHandlers();
        }
        #endregion

        #region Private Methods
        private void LoadHandlers()
        {
            howToUseThemButton.TouchUpInside += HowToUseThemButtonTouchUpInside;
        }

        private void DrawTemplate()
        {
            statusHeaderView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            statusHeaderView.BackgroundColor = ConstantColor.UIBackgroundShimmer;
        }

        private void ClearTemplate()
        {
            statusHeaderView.BackgroundColor = UIColor.Clear;
        }
        #endregion

        #region public Methods 
        public void LoadHeaderView(bool isPreviewEvent)
        {
            this.statusHeaderView.LayoutIfNeeded();
            this.isPreviewEvent = isPreviewEvent;
            if (isPreviewEvent)
            {
                Util.SetConstraint(statusHeaderView, statusHeaderView.Layer.Frame.Height, 120);
                if (PreviewEventHeaderMyDiscount == null)
                {
                    PreviewEventHeaderMyDiscount = PreviewEventHeaderMyDiscount.Create();
                    CGRect PreviewEventHeaderMyDiscountFrame = new CGRect(0, 0, this.statusHeaderView.Frame.Size.Width, 120);
                    PreviewEventHeaderMyDiscount.Frame = PreviewEventHeaderMyDiscountFrame;
                    statusHeaderView.AddSubview(PreviewEventHeaderMyDiscount);
                }
            }
            else
            {
                Util.SetConstraint(statusHeaderView, statusHeaderView.Layer.Frame.Height, 140);
                if (statusHeaderMyDiscount == null)
                {
                    statusHeaderMyDiscount = StatusHeaderMyDiscount.Create();
                    if (statusHeaderMyDiscount.ToActivateEvent == null)
                    {
                        statusHeaderMyDiscount.ToActivateEvent = StatusHeaderMyDiscountToActivateEvent;
                    }
                    if (statusHeaderMyDiscount.ActivatedEvent == null)
                    {
                        statusHeaderMyDiscount.ActivatedEvent = StatusHeaderMyDiscountActivatedEvent;
                    }
                    if (statusHeaderMyDiscount.RedeemedEvent == null)
                    {
                        statusHeaderMyDiscount.RedeemedEvent = StatusHeaderMyDiscountRedeemedEvent;
                    }
                    if (statusHeaderMyDiscount.SelectedChanged == null)
                    {
                        statusHeaderMyDiscount.SelectedChanged = CategoryOfTypeSelectedChanged;
                    }
                    CGRect statusHeaderMyDiscountFrame = new CGRect(0, 0, this.statusHeaderView.Frame.Size.Width, 140);
                    statusHeaderMyDiscount.Frame = statusHeaderMyDiscountFrame;
                    statusHeaderView.AddSubview(statusHeaderMyDiscount);
                }
                statusHeaderMyDiscount.SetOptionSelected(optionSelected);
            }
        }

        public void SetCountersToActivate(int AlreadyPurchased, int CouldLike, int Killers)
        {
            statusHeaderMyDiscount.SetCountersToActivate(AlreadyPurchased, CouldLike, Killers);
        }

        public string GetInfoMaxActiveDiscount()
        {
           return infoMaxActiveDiscountLabel.Text;
        }

        public void SetInfoMaxActiveDiscount(string text)
        {
            infoMaxActiveDiscountLabel.Text = text;
        }

        public void SetHeaderCampaing(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                headerCampaingLabel.Hidden = true;
            }
            else
            {
                headerCampaingLabel.Hidden = false;
                headerCampaingLabel.Text = text;
            }
            if (isPreviewEvent)
            {
                PreviewEventHeaderMyDiscount.SetHeaderCampaing(text);
            }
        }

        public void SetCounters(int toActivate, int activated, int redeemed, int totalMaxDiscountActivated)
        {
            if(statusHeaderMyDiscount != null)
            {
                statusHeaderMyDiscount.SetCounters(toActivate, activated, redeemed, totalMaxDiscountActivated);
            }
        }
        #endregion

        #region Events
        private void StatusHeaderMyDiscountRedeemedEvent(object sender, EventArgs e)
        {
            redeemedEvent?.Invoke(sender, e);
        }

        private void StatusHeaderMyDiscountActivatedEvent(object sender, EventArgs e)
        {
            activatedEvent?.Invoke(sender, e);
        }

        private void StatusHeaderMyDiscountToActivateEvent(object sender, EventArgs e)
        {
            toActivateEvent?.Invoke(sender, e);
        }

        private void CategoryOfTypeSelectedChanged(int type)
        {
            CategoryOfTypeChanged?.Invoke(type);
        }

        private void HowToUseThemButtonTouchUpInside(object sender, EventArgs e)
        {
            tutorialEvent?.Invoke(sender, e);
        }
        #endregion
    }
}
