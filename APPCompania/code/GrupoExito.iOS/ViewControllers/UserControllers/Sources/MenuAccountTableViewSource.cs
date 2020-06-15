using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.UserControllers.Collections;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers.Sources
{
    public partial class MenuAccountTableViewSource : UICollectionViewSource
    {
        #region Attributes
        private UIViewControllerBase controllerBase;
        private IList<MenuItem> menuItems;
        private string identifier;
        #endregion

        #region Constructors
        public MenuAccountTableViewSource(IList<MenuItem> menuItems, string identifier, UIViewControllerBase viewController)
        {
            this.menuItems = menuItems;
            this.identifier = identifier;
            this.controllerBase = viewController;
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
                return menuItems.Count;
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
                AccountUserViewCell cell = (AccountUserViewCell)collectionView.DequeueReusableCell(ConstantIdentifier.profileCollectionViewCellIdentifier, indexPath);
                MenuItem item = menuItems[indexPath.Row];
                cell.LoadProfileIconViewCell(item);
                if (indexPath.Row == 5 && menuItems.Count % 2 != 0)
                {
                    cell.viewHorizontal.Hidden = true;

                }
                if (indexPath.Row == 6 && menuItems.Count % 2 != 0)
                {
                    cell.viewHorizontal.Hidden = true;
                }
                if (indexPath.Row == 0 && menuItems.Count % 2 != 0) 
                {
                    cell.Container.Axis = UILayoutConstraintAxis.Horizontal;
                    cell.viewVertical.Hidden = true;
                    
                }else{
                    cell.Container.Axis = UILayoutConstraintAxis.Vertical;
                }
                return cell;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {

            switch (menuItems[indexPath.Row].ActionName)
            {
                case ConstMenuMyAccount.MyOrders:
                    MyOrdersViewController myOrdersViewController = (MyOrdersViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.MyOrdersViewController);
                    myOrdersViewController.HidesBottomBarWhenPushed = true;
                    controllerBase.NavigationController.PushViewController(myOrdersViewController, true);
                    break;
                case ConstMenuMyAccount.Notifications:
                    NotificationsViewController notificationsViewController = (NotificationsViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.NotificationsViewController);
                    notificationsViewController.HidesBottomBarWhenPushed = true;
                    controllerBase.NavigationController.PushViewController(notificationsViewController, true);
                    break;
                case ConstMenuMyAccount.MyCards:
                    MyCreditCardViewController creditCardViewController = (MyCreditCardViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.MyCreditCardViewController);
                    creditCardViewController.HidesBottomBarWhenPushed = true;
                    controllerBase.NavigationController.PushViewController(creditCardViewController, true);
                    break;
                case ConstMenuMyAccount.MyAddresses:
                    MyAddressViewController addressViewController = (MyAddressViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.MyAddressViewController);
                    addressViewController.HidesBottomBarWhenPushed = true;
                    controllerBase.NavigationController.PushViewController(addressViewController, true);
                    break;
                case ConstMenuMyAccount.MyStickers:

                    RegisterMyStickersEvent();

                    StickersViewController stickersViewController = (StickersViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.StickersViewController);
                    stickersViewController.HidesBottomBarWhenPushed = true;
                    controllerBase.NavigationController.PushViewController(stickersViewController, true);
                    break;
                case ConstMenuMyAccount.Prime:
                    UserContext user = ParametersManager.UserContext;
                    if (user.Prime)
                    {
                        PrimeCustomerViewController primeCustomerViewController = (PrimeCustomerViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.PrimeCustomerViewController);
                        primeCustomerViewController.HidesBottomBarWhenPushed = true;
                        controllerBase.NavigationController.PushViewController(primeCustomerViewController, true);
                        }
                    else
                    {
                        NotPrimeViewController notPrimeViewController = (NotPrimeViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.NotPrimeViewController);
                        notPrimeViewController.HidesBottomBarWhenPushed = true;
                        controllerBase.NavigationController.PushViewController(notPrimeViewController, true);
                    }
                    break;
                case ConstMenuMyAccount.MyPoints:
                    ConsultPointsViewController consultPointsViewController = (ConsultPointsViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.ConsultPointsViewController);
                    controllerBase.NavigationController.PushViewController(consultPointsViewController, true);
                    break;
                case ConstMenuMyAccount.ChangeKey:
                    ChangePasswordUserViewController changePasswordViewController = (ChangePasswordUserViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.ChangeKeyUserViewController);
                    controllerBase.NavigationController.PushViewController(changePasswordViewController, true);
                    break;
                case ConstMenuMyAccount.ContactUs:
                    ContactUsViewController contactUsViewController = (ContactUsViewController)controllerBase.Storyboard.InstantiateViewController(ConstantControllersName.ContactUsViewController);
                    controllerBase.NavigationController.PushViewController(contactUsViewController, true);
                    break;
                default:
                    break;
            }
        }

        private void RegisterMyStickersEvent()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserIdAndLaunchedFrom(AnalyticsEvent.Stickers, AnalyticsReferenceAction.Lobby);
            //FacebookRegistrationEventsService_Deprecated.Instance.RegisterEventWithUserIdAndLaunchedFrom(AnalyticsEvent.Stickers, AnalyticsReferenceAction.Lobby);
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            try
            {
                nfloat width = (collectionView.Frame.Size.Width);
                nfloat divider = (menuItems.Count / 2);
                if (indexPath.Row > 0 || menuItems.Count % 2 == 0){
                    width = width / 2;
                }
                if (menuItems.Count % 2 != 0)
                {
                    divider += 1;
                }
                nfloat height = (collectionView.Frame.Size.Height / divider );
                return new CGSize(width, height);
            }
            catch (Exception)
            {
                return new CGSize(0, 0);
            }
        }
        #endregion
    }
}

