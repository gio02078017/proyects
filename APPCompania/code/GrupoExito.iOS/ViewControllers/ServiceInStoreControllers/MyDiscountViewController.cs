using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlobalToast;
using GrupoExito.DataAgent.Services.Generic;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.DataAgent.Services.Users;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Constants.Generic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Entiites.Generic.Contents;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Parameters;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells;
using GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Sources;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Logic.Models.Users;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers
{
    public partial class MyDiscountViewController : UIViewControllerBase
    {
        #region Attributes 
        private DiscountsModel myDiscountsModel;
        private MyAccountModel accountModel;
        private DiscountsResponse discounts;
        private TutorialView tutorialView;
        private bool userValidated;
        private int itemSelected = 0;
        MyDiscountViewSource myDiscountViewSource;
        #endregion

        #region Constructors 
        public MyDiscountViewController(IntPtr handle) : base(handle)
        {
            myDiscountsModel = new DiscountsModel(new DiscountsService(DeviceManager.Instance));
            accountModel = new MyAccountModel(new UserService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.MyDiscounts, nameof(MyDiscountViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            try
            {               
                base.StartActivityIndicatorCustom();
                this.LoadExternalViews();
                this.LoadHandlers();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyDiscountViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            try
            {
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(false, false, true, this);
                NavigationView.HiddenCarData();
                NavigationView.ShowAccountProfile();

                if (this.discounts == null || discounts.ActivateCoupons == 0)
                {
                    GetUserAndDiscounts();
                }

                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyDiscountViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Private Methods 
        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
            WireUpSwipeRight();
            WireUpSwipeLeft();
        }

        private void LoadExternalViews()
        {
            try
            {
                myDiscountCollectionView.RegisterNibForCell(HeaderSectionMyDiscount.Nib, HeaderSectionMyDiscount.Key);
                myDiscountCollectionView.RegisterNibForCell(MyDiscountCollectionViewCell.Nib, MyDiscountCollectionViewCell.Key);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.HomeViewController, ConstantMethodName.LoadExternalViews);
            }
        }

        private async Task GetUserAndDiscounts()
        {
            await this.GetUser();
            await this.GetDiscounts();
        }

        private void DrawDiscounts()
        {
            discounts = myDiscountsModel.ValidateRedeemedDiscounts(discounts);
            discounts.RedeemedDiscounts.ToList().ForEach(k => k.Redeemable = false);
            myDiscountViewSource = new MyDiscountViewSource(discounts);
            myDiscountViewSource.DiscountToActivateEvent += HeaderSectionMyDiscountToActivateEvent;
            myDiscountViewSource.DiscountActivatedEvent += HeaderSectionMyDiscountActivatedEvent;
            myDiscountViewSource.DiscountRedeemedEvent += HeaderSectionMyDiscountRedeemedEvent;
            myDiscountViewSource.TutorialAction += MyDiscountViewSourceTutorialAction;
            myDiscountViewSource.LegalAction += LegalAction;
            myDiscountViewSource.CategoryOfTypeSelected += MyDiscountViewSourceCategoryOfTypeSelected;
            myDiscountViewSource.ActiveAction += MyDiscountViewSourceActiveAction;
            myDiscountViewSource.DesactiveAction += MyDiscountViewSourceDesactiveAction;

            myDiscountCollectionView.Source = myDiscountViewSource;
            StopActivityIndicatorCustom();
        }

        private void RemoveChild(UIViewController vc)
        {
            vc.WillMoveToParentViewController(null);
            vc.View.RemoveFromSuperview();
            vc.RemoveFromParentViewController();
        }

        private void ShowError(string message)
        {
            InvokeOnMainThread(() =>
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure("", message,
                    (errorSender, ea) => errorView.RemoveFromSuperview());
                errorView.Frame = View.Bounds;
                View.AddSubview(errorView);
            });
        }

        private void WireUpSwipeRight()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Right
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            myDiscountCollectionView.AddGestureRecognizer(gesture);
        }

        private void WireUpSwipeLeft()
        {
            UISwipeGestureRecognizer gesture = new UISwipeGestureRecognizer
            {
                Direction = UISwipeGestureRecognizerDirection.Left
            };
            gesture.AddTarget(() => HandleDrag(gesture));
            myDiscountCollectionView.AddGestureRecognizer(gesture);
        }
        #endregion

        #region Methods Async 
        private async Task GetDiscounts()
        {
            try
            {
                StartActivityIndicatorCustom();
                discounts = await myDiscountsModel.GetDiscounts();
                if (discounts.Result != null && discounts.Result.HasErrors && discounts.Result.Messages != null)
                {
                    bool errorServiceUnavailable = discounts.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.ErrorServiceUnavailable)).Any();
                    if (errorServiceUnavailable)
                    {
                        StopActivityIndicatorCustom();
                        contingencyStackView.Hidden = false;
                        myDiscountCollectionView.RemoveFromSuperview();
                        ContingencyLabel.Text = discounts.Result.Messages[0].Description;
                    }
                    else
                    {
                        var errorCouponsNotFound = discounts.Result.Messages.Where(x => Enum.Parse(typeof(EnumErrorCode), x.Code).Equals(EnumErrorCode.CouponsNotFound)).Any();
                        if (errorCouponsNotFound)
                        {
                            StopActivityIndicatorCustom();
                            contingencyStackView.Hidden = false;
                            myDiscountCollectionView.RemoveFromSuperview();
                            ContingencyImageView.Image = UIImage.FromFile(ConstantImages.SinInformacion);
                            ContingencyLabel.Text = discounts.Result.Messages[0].Description;
                        }
                        else
                        {
                            StartActivityErrorMessage(discounts.Result.Messages[0].Code, discounts.Result.Messages[0].Description);
                        }
                    }
                }
                else
                {
                    if (discounts.ActiveDiscounts != null)
                    {
                        if (discounts.ActivateCoupons == 0 && discounts.TotalActiveDiscounts == 0 && discounts.TotalRedeemedDiscounts == 0 && discounts.TotalActivatedDiscounts == 0)
                        {
                            StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.NoDiscountsMessage);
                            _spinnerActivityIndicatorView.Retry.Hidden = true;
                        }
                        else
                        {
                            Entities.UserContext userContext = ParametersManager.UserContext;
                            DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));

                            //if (!ParametersManager.UserContext.UserActivate)
                            if(!userValidated)
                            {
                                RegistrationValidationViewController validationViewController = new RegistrationValidationViewController();

                                validationViewController.OperationDoneAction += (result) =>
                                {
                                    RemoveChild(validationViewController);
                                    //this.NavigationController.NavigationBarHidden = false;

                                    if (!result)
                                    {
                                        ShowError(AppMessages.VerifyUserError);
                                    }
                                    else
                                    {
                                        userValidated = result;
                                        userContext = ParametersManager.UserContext;
                                        DeviceManager.Instance.SaveAccessPreference(ConstPreferenceKeys.User, JsonService.Serialize(userContext));
                                    }
                                };

                                validationViewController.OperationCanceledAction += () =>
                                {
                                    RemoveChild(validationViewController);
                                    //this.NavigationController.NavigationBarHidden = false;
                                };

                                AddChildViewController(validationViewController);
                                //this.NavigationController.NavigationBarHidden = true;
                                validationViewController.View.Frame = View.Bounds;
                                validationViewController.Cellphone = ParametersManager.UserContext.CellPhone;
                                validationViewController.FromMyDiscount = true;
                                View.AddSubview(validationViewController.View);
                                validationViewController.DidMoveToParentViewController(this);
                            }
                            DrawDiscounts();
                        }
                    }
                    else
                    {
                        StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), AppMessages.NoDiscountsMessage);
                        _spinnerActivityIndicatorView.Retry.Hidden = true;
                    }
                }
            }
            catch (Exception exception)
            {
                StartActivityErrorMessage(EnumErrorCode.UnexpectedErrorMessage.ToString(), exception.Message);
            }
        }

        private async Task GetUser()
        {
            StartActivityIndicatorCustom();
            var response = await accountModel.GetUser();
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                string message = MessagesHelper.GetMessage(response.Result);
                StartActivityErrorMessage(response.Result.Messages[0].Code, message);
            }
            else
            {
                SetUser(response.User);
                userValidated = response.User.UserActivate;
            }
        }

        private async Task RedeemDiscount(Discount discount, bool ActivateCupon)
        {
            try
            {
                if (!userValidated)
                {
                    RegistrationValidationViewController validationViewController = new RegistrationValidationViewController();

                    validationViewController.OperationDoneAction += async (result) =>
                    {
                        RemoveChild(validationViewController);

                        if (result)
                        {
                            //this.NavigationController.NavigationBarHidden = false;
                            userValidated = result;
                            StartActivityIndicatorCustom();
                            if (ActivateCupon)
                            {
                                await ActivateDiscount(discount);
                            }
                            else
                            {
                                await DesactivateDiscount(discount);
                            }
                        }
                        else
                        {
                            ShowError(AppMessages.VerifyUserError);
                        }
                    };

                    validationViewController.OperationCanceledAction += () =>
                    {
                        RemoveChild(validationViewController);
                        //this.NavigationController.NavigationBarHidden = false;
                    };

                    AddChildViewController(validationViewController);

                    //this.NavigationController.NavigationBarHidden = true;
                    validationViewController.View.Frame = View.Bounds;
                    validationViewController.Cellphone = ParametersManager.UserContext.CellPhone;
                    validationViewController.FromMyDiscount = true;
                    View.AddSubview(validationViewController.View);
                    validationViewController.DidMoveToParentViewController(this);
                }
                else
                {
                    StartActivityIndicatorCustom();
                    if (ActivateCupon)
                    {
                        await ActivateDiscount(discount);
                    }
                    else
                    {
                        await DesactivateDiscount(discount);
                    }
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.MyDiscountViewController, ConstantMethodName.RedeemDiscount);
            }
        }

        private async Task ActivateDiscount(Discount discount)
        {
            DiscountParameters discountParameters = new DiscountParameters
            {
                PosCode = discount.PosCode,
                StartDate = discount.StartDate
            };

            DisccountResponse activeDisccountResponse = await myDiscountsModel.ActiveDisccount(discountParameters);

            if (activeDisccountResponse.Result != null && activeDisccountResponse.Result.HasErrors && activeDisccountResponse.Result.Messages != null)
            {
                if (activeDisccountResponse.Result.Messages.Any())
                {
                    string message = MessagesHelper.GetMessage(activeDisccountResponse.Result);
                    StartActivityErrorMessage(activeDisccountResponse.Result.Messages[0].Code, message);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(activeDisccountResponse.Message) && activeDisccountResponse.Message.Equals("success"))
                {
                    discount.Active = true;
                    discounts.ActivatedDiscounts.Add(discount);
                    discounts.TotalActivatedDiscounts += 1;
                    StopActivityIndicatorCustom();
                    RegisterActivateDiscount(discount);
                    discounts = myDiscountsModel.ValidateRedeemedDiscounts(discounts);
                    myDiscountCollectionView.ReloadData();
                    ToastAppearance appearance = new ToastAppearance
                    {
                        MessageColor = UIColor.FromRGB(255, 255, 255),
                        Color = ConstantColor.UiBackgroundToastMake,
                        MessageTextAlignment = UITextAlignment.Center,
                        TitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric),
                        CornerRadius = ConstantStyle.CornerRadius
                    };

                    Toast.MakeToast(AppMessages.DiscountRedeemed)
                        .SetPosition(ToastPosition.Center)
                        .SetLayout(new ToastLayout())
                        .SetAppearance(appearance)
                        .SetDuration(2000)
                        .Show();
                }
            }
        }

        private async Task DesactivateDiscount(Discount discount)
        {
            DiscountParameters discountParameters = new DiscountParameters
            {
                PosCode = discount.PosCode,
                StartDate = discount.StartDate
            };

            DisccountResponse inactiveDisccountResponse = await myDiscountsModel.InactiveDisccount(discountParameters);
            if (inactiveDisccountResponse.Result != null && inactiveDisccountResponse.Result.HasErrors && inactiveDisccountResponse.Result.Messages != null)
            {
                if (inactiveDisccountResponse.Result.Messages.Any())
                {
                    string message = MessagesHelper.GetMessage(inactiveDisccountResponse.Result);
                    StartActivityErrorMessage(inactiveDisccountResponse.Result.Messages[0].Code, message);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(inactiveDisccountResponse.Message) && inactiveDisccountResponse.Message.Equals("success"))
                {
                    myDiscountsModel.UpdatActivatedDiscounts(discounts, discount);
                    RegisterDesactivateDiscount(discount);
                    discounts = myDiscountsModel.ValidateRedeemedDiscounts(discounts);
                    myDiscountCollectionView.ReloadData();
                    ToastAppearance appearance = new ToastAppearance
                    {
                        MessageColor = UIColor.FromRGB(255, 255, 255),
                        Color = ConstantColor.UiBackgroundToastMake,
                        MessageTextAlignment = UITextAlignment.Center,
                        TitleFont = UIFont.FromName(ConstantFontSize.LetterSubtitle, ConstantFontSize.SubtitleGeneric),
                        CornerRadius = ConstantStyle.CornerRadius
                    };

                    Toast.MakeToast(AppMessages.DiscountDesactivated)
                        .SetPosition(ToastPosition.Center)
                        .SetLayout(new ToastLayout())
                        .SetAppearance(appearance)
                        .SetDuration(2000)
                        .Show();
                }
                StopActivityIndicatorCustom();
            }
        }

        private void RegisterActivateDiscount(Discount discount)
        {
            FirebaseEventRegistrationService.Instance.ActivatedDiscount(discount);
            FacebookEventRegistrationService.Instance.ActivatedDiscount(discount);
        }

        private void RegisterDesactivateDiscount(Discount discount)
        {
            FirebaseEventRegistrationService.Instance.InactivateDiscount(discount);
            FacebookEventRegistrationService.Instance.InactivateDiscount(discount);
        }

        private void ModalTermsConditionsDiscountScreenView()
        {
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ModalTermsConditionsDiscount, nameof(MyDiscountViewController));
        }

        private void ActivatedDiscountsScreenView()
        {
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ActivatedDiscounts, nameof(MyDiscountViewController));
        }
        #endregion

        #region Events
        private async void MyDiscountViewSourceActiveAction(object sender, EventArgs e)
        {
            Discount discount = (Discount)sender;
            await RedeemDiscount(discount, true);
        }

        private async void MyDiscountViewSourceDesactiveAction(object sender, EventArgs e)
        {
            Discount discount = (Discount)sender;
            await RedeemDiscount(discount, false);
        }

        private void HeaderSectionMyDiscountRedeemedEvent(object sender, EventArgs e)
        {
            myDiscountViewSource.SetDiscountActive(ConstantCuponType.Redeemed);
            myDiscountCollectionView.ReloadSections(new Foundation.NSIndexSet(1));
        }

        private void HeaderSectionMyDiscountActivatedEvent(object sender, EventArgs e)
        {
            myDiscountViewSource.SetDiscountActive(ConstantCuponType.Activated);
            myDiscountCollectionView.ReloadSections(new Foundation.NSIndexSet(1));
            ActivatedDiscountsScreenView();
        }

        private void HeaderSectionMyDiscountToActivateEvent(object sender, EventArgs e)
        {
            ActiveDiscounts activeDiscounts = discounts.ActiveDiscounts;
            if (activeDiscounts.AlreadyPurchased.Count > 0)
            {
                myDiscountViewSource.SetDiscountActive(ConstantCuponType.AlreadyPurchased);
            }else if(activeDiscounts.CouldLike.Count >0)
            {
                myDiscountViewSource.SetDiscountActive(ConstantCuponType.CouldLike);
            }
            else
            {
                myDiscountViewSource.SetDiscountActive(ConstantCuponType.Killers);
            }
            myDiscountCollectionView.ReloadData();
        }

        private void LegalAction(object sender, EventArgs e)
        {
            if (sender is Discount discount)
            {
                PopUpInformationView popUpView = PopUpInformationView.Create(AppMessages.TermsAndConditions, discount.Legal);
                this.NavigationController.SetNavigationBarHidden(true, true);
                popUpView.Frame = NavigationController.View.Bounds;
                popUpView.LayoutIfNeeded();
                View.AddSubview(popUpView);

                popUpView.AcceptButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    popUpView.RemoveFromSuperview();
                };
                popUpView.CloseButtonHandler += () =>
                {
                    this.NavigationController.SetNavigationBarHidden(false, true);
                    popUpView.RemoveFromSuperview();
                };

                ModalTermsConditionsDiscountScreenView();
            }
        }

        private void HandleDrag(UISwipeGestureRecognizer recognizer)
        {
            if (recognizer.Direction == UISwipeGestureRecognizerDirection.Right)
            {
                if (itemSelected <= 0)
                {
                    itemSelected = 2;
                    MyDiscountViewSourceCategoryOfTypeSelected(itemSelected);
                }
                else
                {
                    itemSelected--;
                    MyDiscountViewSourceCategoryOfTypeSelected(itemSelected);
                }
            }
            else if (recognizer.Direction == UISwipeGestureRecognizerDirection.Left)
            {
                if (itemSelected >= 2)
                {
                    itemSelected = 0;
                    MyDiscountViewSourceCategoryOfTypeSelected(itemSelected);
                }
                else
                {
                    itemSelected++;
                    MyDiscountViewSourceCategoryOfTypeSelected(itemSelected);
                }
            }
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            if (_spinnerActivityIndicatorView.Retry.Title(UIControlState.Normal).Equals(AppMessages.GoBack))
            {
                this.NavigationController.PopViewController(true);
            }
            else if (_spinnerActivityIndicatorView.Retry.Title(UIControlState.Normal).Equals(AppMessages.DiscountsAction))
            {
                this.NavigationController.PopViewController(true);
            }
        }

        private void MyDiscountViewSourceCategoryOfTypeSelected(int obj)
        {
            itemSelected = obj;
            switch (obj)
            {
                case 0:
                    myDiscountViewSource.SetDiscountActive(ConstantCuponType.AlreadyPurchased);
                    myDiscountCollectionView.ReloadData();
                    break;
                case 1:
                    myDiscountViewSource.SetDiscountActive(ConstantCuponType.CouldLike);
                    myDiscountCollectionView.ReloadData();
                    break;
                case 2:
                    myDiscountViewSource.SetDiscountActive(ConstantCuponType.Killers);
                    myDiscountCollectionView.ReloadData();
                    break;
            }
        }

        private void MyDiscountViewSourceTutorialAction(object sender, EventArgs e)
        {
            IList<Tutorial> tutorialesList = new ContentsModel(new ContentsService(DeviceManager.Instance)).GetTutorials();
            foreach (Tutorial current in tutorialesList)
            {
                if (ConstNameViewTutorial.Discount.Equals(current.Name))
                {
                    this.CreateTutorialView(this.View, tutorialView, current.ImagesTutorial, 0, true);
                    break;
                }
            }
        }
        #endregion
    }
}

