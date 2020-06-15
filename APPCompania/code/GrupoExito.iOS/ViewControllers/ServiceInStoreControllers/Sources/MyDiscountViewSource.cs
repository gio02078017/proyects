using System;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Sources
{
    public class MyDiscountViewSource : UICollectionViewSource
    {
        #region Attributes
        private bool IsCuponMania { get; set; }
        private string discountActive = ConstantCuponType.AlreadyPurchased;


        private EventHandler activeAction;
        private EventHandler desactiveAction;
        private EventHandler legalAction;
        private EventHandler tutorialAction;
        private EventHandler discountToActivateEvent;
        private EventHandler discountActivatedEvent;
        private EventHandler discountRedeemedEvent;
        private DiscountsResponse discounts;
        #endregion

        #region Properties
        public EventHandler ActiveAction { get => activeAction; set => activeAction = value; }
        public EventHandler DesactiveAction { get => desactiveAction; set => desactiveAction = value; }
        public EventHandler TutorialAction { get => tutorialAction; set => tutorialAction = value; }
        public EventHandler LegalAction { get => legalAction; set => legalAction = value; }
        public DiscountsResponse Discounts { get => discounts; set => discounts = value; }
        public EventHandler DiscountToActivateEvent { get => discountToActivateEvent; set => discountToActivateEvent = value; }
        public EventHandler DiscountActivatedEvent { get => discountActivatedEvent; set => discountActivatedEvent = value; }
        public EventHandler DiscountRedeemedEvent { get => discountRedeemedEvent; set => discountRedeemedEvent = value; }

        public Action<int> CategoryOfTypeSelected { get; set; }
        public string DiscountActive { get => discountActive; set => discountActive = value; }

        #endregion

        #region Constructors
        public MyDiscountViewSource(DiscountsResponse discounts, bool isCuponMania = false)
        {
            this.Discounts = discounts;
            this.IsCuponMania = isCuponMania;
            ActiveDiscounts activeDiscounts = Discounts.ActiveDiscounts;
            if (activeDiscounts.AlreadyPurchased.Count > 0)
            {
                discountActive = ConstantCuponType.AlreadyPurchased;
            }else if(activeDiscounts.CouldLike.Count > 0)
            {
                discountActive = ConstantCuponType.CouldLike;
            }
            else
            {
                discountActive = ConstantCuponType.Killers;
            }
        }
        #endregion

        #region Overrides Methods 
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 2;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            try
            {
                switch (section)
                {
                    case 0:
                        return 1;
                    default:
                        switch (DiscountActive)
                        {
                            case ConstantCuponType.AlreadyPurchased:
                                return Discounts.ActiveDiscounts.AlreadyPurchased.Count;
                            case ConstantCuponType.CouldLike:
                                return Discounts.ActiveDiscounts.CouldLike.Count;
                            case ConstantCuponType.Killers:
                                return Discounts.ActiveDiscounts.Killers.Count;
                            case ConstantCuponType.Activated:
                                return Discounts.ActivatedDiscounts.Count;
                            case ConstantCuponType.Redeemed:
                                return Discounts.RedeemedDiscounts.Count;
                            default:
                                return 0;
                        }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case 0:
                    HeaderSectionMyDiscount headerSectionMyDiscount = collectionView.DequeueReusableCell(HeaderSectionMyDiscount.Key, indexPath) as HeaderSectionMyDiscount;
                    headerSectionMyDiscount.OptionSelected = GetOptionSelected();
                    headerSectionMyDiscount.LoadHeaderView(discounts.PreviewCampaign);
                    headerSectionMyDiscount.SetHeaderCampaing(discounts.HeaderCampaign);
                    if (!discountActive.Equals(ConstantCuponType.Activated) && !discountActive.Equals(ConstantCuponType.Redeemed))
                    {
                        headerSectionMyDiscount.SetCountersToActivate(discounts.ActiveDiscounts.AlreadyPurchased.Count, discounts.ActiveDiscounts.CouldLike.Count, discounts.ActiveDiscounts.Killers.Count);
                    }
                    if (!discounts.PreviewCampaign)
                    {
                        headerSectionMyDiscount.SetCounters(discounts.TotalActiveDiscounts, discounts.TotalActivatedDiscounts, discounts.TotalRedeemedDiscounts, discounts.ActivateCoupons);
                        if (headerSectionMyDiscount.ToActivateEvent == null)
                        {
                            headerSectionMyDiscount.ToActivateEvent = HeaderSectionMyDiscountToActivateEvent;
                        }
                        if (headerSectionMyDiscount.ActivatedEvent == null)
                        {
                            headerSectionMyDiscount.ActivatedEvent = HeaderSectionMyDiscountActivatedEvent;
                        }
                        if (headerSectionMyDiscount.RedeemedEvent == null)
                        {
                            headerSectionMyDiscount.RedeemedEvent = HeaderSectionMyDiscountRedeemedEvent;
                        }
                        if (headerSectionMyDiscount.TutorialEvent == null)
                        {
                            headerSectionMyDiscount.TutorialEvent = HeaderSectionMyDiscountTutorialEvent;
                        }
                        if (headerSectionMyDiscount.CategoryOfTypeChanged == null)
                        {
                            headerSectionMyDiscount.CategoryOfTypeChanged = HeaderSectionMyDiscountCategoryOfTypeChanged;
                        }
                    }
                    return headerSectionMyDiscount;
                default:
                    MyDiscountCollectionViewCell myDiscountCollectionViewCell = collectionView.DequeueReusableCell(MyDiscountCollectionViewCell.Key, indexPath) as MyDiscountCollectionViewCell;
                    Discount row = GetCurrentDiscount(indexPath.Row);
                    if (DiscountActive == ConstantCuponType.Activated)
                    {
                        myDiscountCollectionViewCell.SetRedeemed(false);
                        myDiscountCollectionViewCell.SetProportionalConstraint(1.1f);
                    }
                    else if (DiscountActive == ConstantCuponType.Redeemed)
                    {
                        myDiscountCollectionViewCell.SetRedeemed(true);
                        myDiscountCollectionViewCell.SetProportionalConstraint(1.1f);
                    }
                    if (IsCuponMania)
                    {
                        myDiscountCollectionViewCell.BackgroundColor = ConstantColor.UIColorBackgroundCuponmania;
                        myDiscountCollectionViewCell.ContainerAllViews.BackgroundColor = ConstantColor.UIColorBackgroundCuponmania;
                        myDiscountCollectionViewCell.Plu.Hidden = true;
                    }
                    myDiscountCollectionViewCell.LoadDiscountViewCell(row, discounts.PreviewCampaign);
                    if (myDiscountCollectionViewCell.ActiveAction == null)
                    {
                        myDiscountCollectionViewCell.ActiveAction = CellActiveAction;
                    }
                    if (myDiscountCollectionViewCell.DesactiveAction == null)
                    {
                        myDiscountCollectionViewCell.DesactiveAction = CellDesactiveAction;
                    }

                    if (myDiscountCollectionViewCell.LegalAction == null)
                    {
                        myDiscountCollectionViewCell.LegalAction = CellLegalAction;
                    }

                    return myDiscountCollectionViewCell;
            }
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            try
            {
                switch (indexPath.Section)
                {
                    case 0:
                        nfloat headerWidth = (collectionView.Superview.Frame.Size.Width);
                        if (discounts.PreviewCampaign)
                        {
                            return new CGSize(headerWidth, 220);
                        }
                        else
                        {
                            return new CGSize(headerWidth, 250);
                        }
                    default:
                        nfloat width = ((collectionView.Frame.Width) / 2);
                        nfloat height = ConstantViewSize.MyDiscountCellHeight;
                        if(DiscountActive == ConstantCuponType.Activated || DiscountActive == ConstantCuponType.Redeemed)
                        {
                            height = DiscountActive == ConstantCuponType.Activated ? ConstantViewSize.MyDiscountCellActivateHeight : ConstantViewSize.MyDiscountCellRedeemedHeight;
                        }
                        return new CGSize(width, height);
                }
            }
            catch (Exception)
            {
                return new CGSize(100, 100);
            }
        }
        #endregion

        #region Private Methods
        public Discount GetCurrentDiscount(int position)
        {
            switch (DiscountActive)
            {
                case ConstantCuponType.AlreadyPurchased:
                    return Discounts.ActiveDiscounts.AlreadyPurchased[position];
                case ConstantCuponType.CouldLike:
                    return Discounts.ActiveDiscounts.CouldLike[position];
                case ConstantCuponType.Killers:
                    return Discounts.ActiveDiscounts.Killers[position];
                case ConstantCuponType.Activated:
                    return Discounts.ActivatedDiscounts[position];
                default:
                    return Discounts.RedeemedDiscounts[position];
            }
        }

        private int GetOptionSelected()
        {
            switch (DiscountActive)
            {
                case ConstantCuponType.AlreadyPurchased:
                    return 0;
                case ConstantCuponType.CouldLike:
                    return 1;
                case ConstantCuponType.Killers:
                    return 2;
                case ConstantCuponType.Activated:
                    return 3;
                default:
                    return 4;
            }
        }
        #endregion

        #region Public Methods
        public void SetDiscountActive(string discountActive)
        {
            this.DiscountActive = discountActive;
        }
        #endregion

        #region Events
        private void CellActiveAction(object sender, EventArgs e)
        {
            ActiveAction?.Invoke(sender, e);
        }

        private void CellDesactiveAction(object sender, EventArgs e)
        {
            DesactiveAction?.Invoke(sender, e);
        }

        private void CellLegalAction(object sender, EventArgs e)
        {
            LegalAction?.Invoke(sender, e);
        }

        private void HeaderSectionMyDiscountToActivateEvent(object sender, EventArgs e)
        {
            DiscountToActivateEvent?.Invoke(null, null);
        }

        private void HeaderSectionMyDiscountActivatedEvent(object sender, EventArgs e)
        {
            DiscountActivatedEvent?.Invoke(null, null);
        }

        private void HeaderSectionMyDiscountRedeemedEvent(object sender, EventArgs e)
        {
            DiscountRedeemedEvent?.Invoke(null, null);
        }

        private void HeaderSectionMyDiscountCategoryOfTypeChanged(int type)
        {
            CategoryOfTypeSelected?.Invoke(type);
        }

        private void HeaderSectionMyDiscountTutorialEvent(object sender, EventArgs e)
        {
            TutorialAction?.Invoke(sender, e);
        }
        #endregion
    }
}
