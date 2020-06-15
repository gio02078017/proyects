using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.DataAgent.Services.InStoreServices;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.OtherServiceControllers.Cells;
using GrupoExito.iOS.ViewControllers.OtherServiceControllers.Sources;
using GrupoExito.iOS.ViewControllers.OtherServiceControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.iOS.ViewControllers.UserControllers.Sources;
using GrupoExito.Logic.Models.Generic;
using GrupoExito.Logic.Models.InStoreServices;
using GrupoExito.Utilities.Helpers;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers
{
    public partial class OtherServiceViewController : UIViewControllerBase
    {
        #region Attributes
        private IList<MenuItem> MenuItems;
        private OtherServiceViewSource OtherServiceViewSource;
        private bool openFromInitialOption = false;
        private ConsultSoatView consultSoatView;
        #endregion

        #region Properties
        public ConsultSoatView ConsultSoatView { get => consultSoatView; set => consultSoatView = value; }
        public bool OpenFromInitialOption { get => openFromInitialOption; set => openFromInitialOption = value; }
        #endregion

        #region Constructors
        public OtherServiceViewController(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.OtherServices, nameof(OtherServiceViewController));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            try
            {             
                this.LoadExternalViews();
                this.LoadHandlers();
                this.LoadData();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OtherServiceViewController, ConstantMethodName.ViewDidLoad);
                ShowMessageException(exception);
            }
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            try
            {
                RegisterViewAppear();
                NavigationView = (NavigationHeaderView)this.NavigationController.NavigationBar.Subviews[this.NavigationController.NavigationBar.Subviews.Length - 1];
                NavigationView.LoadControllers(!OpenFromInitialOption, false, true, this);
                NavigationView.HiddenCarData();
                if (ParametersManager.UserContext == null || ParametersManager.UserContext.IsAnonymous)
                {
                    NavigationView.HiddenAccountProfile();
                }
                else
                {
                    NavigationView.ShowAccountProfile();
                }
                this.NavigationController.NavigationBarHidden = false;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OtherServiceViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods

        private void RegisterViewAppear()
        {
            //FirebaseEventRegistrationService.Instance.RegisterEventWithUserId(AnalyticsEvent.HomeMore);
        }

        private void LoadExternalViews()
        {
            try
            {
                servicesTableView.RegisterNibForCellReuse(HeaderOtherServiceViewCell.Nib, HeaderOtherServiceViewCell.Key);
                servicesTableView.RegisterNibForCellReuse(UINib.FromName(ConstantReusableViewName.MenuServicesTableViewCell, NSBundle.MainBundle), ConstantIdentifier.MenuServicesItemIdentifier);
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OtherServiceViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            if (DeviceManager.Instance.IsNetworkAvailable().Result)
            {
                StartActivityIndicatorCustom();
                MenuItems = JsonService.Deserialize<List<MenuItem>>(AppConfigurations.MenuOtherServicesSource);
                OtherServiceViewSource = new OtherServiceViewSource(MenuItems);
                servicesTableView.Source = OtherServiceViewSource;
                OtherServiceViewSource.SelectRowAction += OtherServiceViewSourceSelectRowAction;
                servicesTableView.EstimatedRowHeight = 190;
                servicesTableView.RowHeight = UITableView.AutomaticDimension;
                servicesTableView.ReloadData();
                StopActivityIndicatorCustom();
            }
            else
            {
                StartActivityErrorMessage(EnumErrorCode.InternetErrorMessage.ToString(), AppMessages.InternetErrorMessage);
            }
        }

        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            LoadData();
        }

        private async Task GetSoatAsync()
        {
            await GetSoat();
        }

        private async Task GetSoat()
        {
            StartActivityIndicatorCustom();
            Soat soat = GetSoatEntity();
            SoatResponse response = null;
            InsuranceModel _insuranceModel = new InsuranceModel(new InsuranceService(DeviceManager.Instance));
            try
            {
                string messageValidation = _insuranceModel.ValidateFields(soat);

                if (string.IsNullOrEmpty(messageValidation))
                {
                    response = await _insuranceModel.GetSoat(soat);
                    ValidateResponse(response);
                }
                else
                {
                    customSpinnerView.Hidden = false;
                    customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    _spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.MessageStatusViewSize);
                    spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                    spinnerActivityIndicatorView.StartAnimating();
                    MessageStatusView messageStatusView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                    CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                    messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageStatusView_.Frame = messageViewFrame;
                    messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageStatusView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                    messageStatusView_.Title.Text = AppMessages.SomethingsWrong;
                    messageStatusView_.Message.Text = messageValidation;
                    messageStatusView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                    messageStatusView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.ConsultSoatViewHeight);
                        messageStatusView_.RemoveFromSuperview();
                        ConsultSoatView.Hidden = false;
                        _spinnerActivityIndicatorView.Hidden = true;
                    };
                    messageStatusView_.Close.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.ConsultSoatViewHeight);
                        messageStatusView_.RemoveFromSuperview();
                        ConsultSoatView.Hidden = false;
                        _spinnerActivityIndicatorView.Hidden = true;
                    };
                    customSpinnerView.AddSubview(messageStatusView_);
                }
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OtherServiceViewController, ConstantMethodName.GetSoat);
            }
        }

        private Soat GetSoatEntity()
        {
            return new Soat()
            {
                DocumentType = ConsultSoatView.DocumentsType[(int)ConsultSoatView.documentType.SelectedRowInComponent(0)].Code,
                DocumentNumber = ConsultSoatView.documentNumber.Text.Trim(),
                ImageFormat = ConstImageFormat.Png,
                LicensePlate = ConsultSoatView.plateTruck.Text.Trim()
            };
        }

        private void ValidateResponse(SoatResponse response)
        {
            if (response.Result != null && response.Result.HasErrors && response.Result.Messages != null)
            {
                customSpinnerView.Hidden = false;
                customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                _spinnerActivityIndicatorView.Image.StopAnimating();
                _spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.MessageStatusViewSize);
                spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                spinnerActivityIndicatorView.StartAnimating();
                MessageStatusView messageStatusView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                messageStatusView_.Frame = messageViewFrame;
                messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                messageStatusView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                messageStatusView_.Title.Text = AppMessages.SomethingsWrong;
                messageStatusView_.Message.Text = MessagesHelper.GetMessage(response.Result);
                messageStatusView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                messageStatusView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                {
                    Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.ConsultSoatViewHeight);
                    messageStatusView_.RemoveFromSuperview();
                    ConsultSoatView.Hidden = false;
                };
                messageStatusView_.Close.TouchUpInside += (object sender, EventArgs e) =>
                {
                    Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.ConsultSoatViewHeight);
                    messageStatusView_.RemoveFromSuperview();
                    ConsultSoatView.Hidden = false;
                };
                customSpinnerView.AddSubview(messageStatusView_);
            }
            else
            {
                if (response.Error != null && response.Error.Equals("NoDataFound"))
                {
                    customSpinnerView.Hidden = false;
                    customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    _spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.MessageStatusViewSize);
                    spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                    spinnerActivityIndicatorView.StartAnimating();
                    MessageStatusView messageStatusView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                    CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                    messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageStatusView_.Frame = messageViewFrame;
                    messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageStatusView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                    messageStatusView_.Title.Text = AppMessages.SomethingsWrong;
                    messageStatusView_.Message.Text = response.MessageSoat;
                    messageStatusView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                    messageStatusView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.ConsultSoatViewHeight);
                        messageStatusView_.RemoveFromSuperview();
                        ConsultSoatView.Hidden = false;
                    };
                    customSpinnerView.AddSubview(messageStatusView_);
                }
                else if (!string.IsNullOrEmpty(response.QRCode))
                {
                    ConsultSoatView.RemoveFromSuperview();
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    spinnerActivityIndicatorView.StopAnimating();
                    spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                    customSpinnerView.Hidden = true;
                    Soat soatNew = new Soat()
                    {
                        DocumentNumber = ConsultSoatView.documentNumber.Text,
                        Plate = ConsultSoatView.plateTruck.Text, 
                        QRCode = response.QRCode
                    };
                    SoatViewController soatViewController = (SoatViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.SoatViewController);
                    soatViewController.Soat = soatNew;
                    this.NavigationController.PushViewController(soatViewController, true);
                }
                else
                {
                    customSpinnerView.Hidden = false;
                    customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    _spinnerActivityIndicatorView.Image.StopAnimating();
                    _spinnerActivityIndicatorView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.MessageStatusViewSize);
                    spinnerActivityIndicatorView.Layer.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.7f).CGColor;
                    spinnerActivityIndicatorView.StartAnimating();
                    MessageStatusView messageStatusView_ = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.MessageStatusView, Self, null).GetItem<MessageStatusView>(0);
                    CGRect messageViewFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.MessageStatusViewSize);
                    messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageStatusView_.Frame = messageViewFrame;
                    messageStatusView_.Layer.CornerRadius = ConstantStyle.CornerRadius;
                    messageStatusView_.ImageStatus.Image = UIImage.FromFile(ConstantImages.Problema);
                    messageStatusView_.Title.Text = AppMessages.SomethingsWrong;
                    messageStatusView_.Message.Text = MessagesHelper.GetMessage(response.Result);
                    messageStatusView_.Action.SetTitle(AppMessages.Return, UIControlState.Normal);
                    messageStatusView_.Action.TouchUpInside += (object sender, EventArgs e) =>
                    {
                        Util.SetConstraint(customSpinnerView, (int)customSpinnerView.Frame.Height, ConstantViewSize.ConsultSoatViewHeight);
                        messageStatusView_.RemoveFromSuperview();
                        ConsultSoatView.Hidden = false;
                        _spinnerActivityIndicatorView.SendSubviewToBack(ConsultSoatView);
                    };
                    customSpinnerView.AddSubview(messageStatusView_);
                }
            }
        }

        private void ModalSoatScreenView()
        {
            Utilities.Analytic.FirebaseEventRegistrationService.Instance.RegisterScreen(Entities.Constants.Analytic.AnalyticsScreenView.ResumeSoat, nameof(MySoatViewController));
        }
        #endregion

        #region Events 
        private void OtherServiceViewSourceSelectRowAction(object sender, EventArgs e)
        {
            NSIndexPath indexPath = (NSIndexPath)sender;
            if (indexPath.Section == 1)
            {
                switch (MenuItems[indexPath.Row].ActionName)
                {
                    case ConstMenuOtherServices.RechargePhone:
                        PhoneRechargeViewController phoneRechargeViewController = (PhoneRechargeViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.PhoneRechargeViewController);
                        phoneRechargeViewController.HidesBottomBarWhenPushed = true;
                        break;
                    case ConstMenuOtherServices.Soat:
                        DocumentsDataBaseModel documentsDataBaseModel = new DocumentsDataBaseModel(DocumentsDataBase.Instance);
                        List<Soat> soatList = documentsDataBaseModel.GetSoats();
                        if (soatList != null && soatList.Any())
                        {
                            MySoatViewController mySoatViewController = (MySoatViewController)this.Storyboard.InstantiateViewController(ConstantControllersName.MySoatViewController);
                            mySoatViewController.SoatList = soatList;
                            this.NavigationController.PushViewController(mySoatViewController, true);
                        }
                        else
                        {
                            if (DeviceManager.Instance.IsNetworkAvailable().Result)
                            {
                                Util.SetConstraint(customSpinnerView, ConstantViewSize.customSpinnerViewHeightDefault, ConstantViewSize.ConsultSoatViewHeight);
                                ConsultSoatView = NSBundle.MainBundle.LoadNib(ConstantReusableViewName.ConsultSoatView, Self, null).GetItem<ConsultSoatView>(0);
                                CGRect consultSoatFrame = new CGRect(0, 0, customSpinnerView.Frame.Size.Width, ConstantViewSize.ConsultSoatViewHeight);
                                ConsultSoatView.Frame = consultSoatFrame;
                                customSpinnerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                                ConsultSoatView.Layer.CornerRadius = ConstantStyle.CornerRadius;
                                _spinnerActivityIndicatorView.Hidden = true;
                                customSpinnerView.AddSubview(ConsultSoatView);
                                spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundSkipLogin.ColorWithAlpha(0.5f);
                                spinnerActivityIndicatorView.StartAnimating();
                                customSpinnerView.Hidden = false;
                                ConsultSoatView.cancel.TouchUpInside += CancelTouchUpInside;
                                ConsultSoatView.consult.TouchUpInside += ConsultSoatTouchUpInside;
                                ConsultSoatView.documentTypeB.TouchUpInside += DocumentTypeBTouchUpInside;
                                consultSoatView.ReloadDocumentEvent += DocumentTypeBTouchUpInside;
                                ModalSoatScreenView();
                            }
                            else
                            {
                                ParametersManager.ContainChanges = true;
                                StartActivityErrorMessage("", AppMessages.InternetErrorMessage);
                            }
                        }
                        break;
                }
            }
        }

        private void CancelTouchUpInside(object sender, EventArgs e)
        {
            try
            {
                Util.SetConstraint(customSpinnerView, ConstantViewSize.ConsultSoatViewHeight, ConstantViewSize.customSpinnerViewHeightDefault);
                spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
                spinnerActivityIndicatorView.StopAnimating();
                ConsultSoatView.DocumentsType.Clear();
                ConsultSoatView.RemoveFromSuperview();
                _spinnerActivityIndicatorView.Hidden = false;
                customSpinnerView.Hidden = true;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.OtherServiceViewController, ConstantEventName.CancelTouchUpInside);
            }
        }

        private async void ConsultSoatTouchUpInside(object sender, EventArgs e)
        {
            Util.SetConstraint(customSpinnerView, ConstantViewSize.messageCustomViewHeightDefault, ConstantViewSize.customSpinnerViewHeightDefault);
            spinnerActivityIndicatorView.BackgroundColor = ConstantColor.UiBackgroundWaitData;
            spinnerActivityIndicatorView.StopAnimating();
            ConsultSoatView.Hidden = true;
            customSpinnerView.BackgroundColor = UIColor.Clear;
            _spinnerActivityIndicatorView.Hidden = false;
            await GetSoatAsync();
        }

        private void DocumentTypeBTouchUpInside(object sender, EventArgs e)
        {
            ConsultSoatView.documentNumber.ResignFirstResponder();
            ConsultSoatView.plateTruck.ResignFirstResponder();
            genericPickerView.Delegate = new PickerViewDelegate(ConsultSoatView.DocumentsType, ConsultSoatView.documentType);
            genericPickerView.DataSource = new PickerViewSource(ConsultSoatView.DocumentsType);
            if (ConsultSoatView.DocumentsType.Any())
            {
                containerGenericPickerView.Hidden = false;
            }
            genericPickerView.ReloadAllComponents();
            genericPickerView.Select(0, 0, false);
            closeGenericPickerButton.TouchUpInside += ClosePickerViewButtonTouchUpInside;
        }

        private void ClosePickerViewButtonTouchUpInside(object sender, EventArgs e)
        {
            containerGenericPickerView.Hidden = true;
            closeGenericPickerButton.TouchUpInside -= ClosePickerViewButtonTouchUpInside;
        }
        #endregion
    }
}

