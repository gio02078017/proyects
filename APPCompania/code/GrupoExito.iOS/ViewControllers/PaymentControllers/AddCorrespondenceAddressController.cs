using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using GrupoExito.Entities;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Models;
using GrupoExito.iOS.ViewControllers.PaymentControllers.Views;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Models.Contracts;
using GrupoExito.Models.Enumerations;
using GrupoExito.Models.ViewModels.Payments;
using GrupoExito.Utilities.Resources;
using ObjCRuntime;
using UIKit;

namespace GrupoExito.iOS.Views.PaymentViews
{
    public partial class AddCorrespondenceAddressController : UIViewController
    {
        private AddressCorrespondenceViewModel addressViewModel;
        private CityAddressPickerViewModel citiesPickerViewModel;
        private GenericPickerViewController genericPickerViewController;
        private CustomSpinnerViewController spinnerController;
        private string mobile;
        private City city;
        private IList<City> cities;
        private bool isSpinnerAdded = false;

        public AddCorrespondenceAddressController() : base("AddCorrespondenceAddressController", null)
        {
        }

        public override void LoadView()
        {
            base.LoadView();
            var arr = NSBundle.MainBundle.LoadNib(nameof(AddCorrespondenceAddressController), this, null);
            var v = Runtime.GetNSObject<UIView>(arr.ValueAt(0));

            View = v;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            cities = new List<City>();

            addressViewModel = new AddressCorrespondenceViewModel(DeviceManager.Instance)
            {
                Delegate = this
            };

            spinnerController = new CustomSpinnerViewController();
            Initialize();
            SetHandlers();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationController?.SetNavigationBarHidden(true, false);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            this.NavigationController?.SetNavigationBarHidden(false, false);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void Initialize()
        {
            continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
            continueButton.Enabled = false;
            contentView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            continueButton.Layer.CornerRadius = ConstantStyle.CornerRadius / 2;
            descriptionLabel.Text = AppMessages.AddCorrespondenceMessage;

            phoneTextField.Text = ParametersManager.UserContext.CellPhone;

            if (!string.IsNullOrEmpty(ParametersManager.UserContext.CellPhone))
            {
                string validationResult = addressViewModel.ValidateMobile(ParametersManager.UserContext.CellPhone);
                if (string.IsNullOrEmpty(validationResult))
                {
                    phoneTextField.RemoveFromSuperview();
                    phoneTitleLabel.RemoveFromSuperview();
                }
            }
        }

        private void SetHandlers()
        {
            continueButton.TouchUpInside += ((o, s) =>
            {
                UserAddress userAddress = new UserAddress()
                {
                    AddressComplete = addressTextField.Text,
                    CellPhone = mobile,
                    Region = city.Region,
                    StateId = city.State,
                    CityId = city.Id
                };
                addressViewModel.SaveAddressCommand.Execute(userAddress);
                ShowSpinner();
            });

            closeButton.TouchUpInside += ((o, s) =>
            {
                this.NavigationController.SetNavigationBarHidden(false, false);
                this.NavigationController.PopViewController(false);
            });

            cityButton.TouchUpInside += ((o, s) =>
            {
                LoadCities();
            });

            if (phoneTextField != null)
            {
                phoneTextField.EditingChanged += ((o, s) =>
                {
                    ContinueButtonEnableValidation();
                });
            }

            addressTextField.EditingChanged += ((o, s) =>
            {
                ContinueButtonEnableValidation();
            });
        }

        private void LoadCities()
        {
            ShowSpinner();
            addressViewModel.GetCitiesCommand.Execute(null);
        }

        void GenericPickerViewController_AcceptAction(nint rowSelected)
        {
            if (rowSelected != null)
            {
                if (rowSelected != 0)
                {
                    city = cities[(int)rowSelected];
                    cityTextField.Text = city.Name;
                }
            }

            HideCitiesPicker();
        }

        void GenericPickerViewController_CancelAction()
        {
            HideCitiesPicker();
        }

        private void ShowCitiesPicker()
        {
            citiesPickerViewModel = new CityAddressPickerViewModel(cities);
            genericPickerViewController = new GenericPickerViewController("Ciudades", citiesPickerViewModel);
            genericPickerViewController.AcceptAction += GenericPickerViewController_AcceptAction;
            genericPickerViewController.CancelAction += GenericPickerViewController_CancelAction;

            this.AddChildViewController(genericPickerViewController);
            genericPickerViewController.DidMoveToParentViewController(this);

            CGRect frame = new CGRect(0, 0, this.View.Frame.Width, this.View.Frame.Height);
            genericPickerViewController.View.Frame = frame;
            View.AddSubview(genericPickerViewController.View);
        }

        private void HideCitiesPicker()
        {
            genericPickerViewController.WillMoveToParentViewController(null);
            genericPickerViewController.RemoveFromParentViewController();
            genericPickerViewController.View.RemoveFromSuperview();
            genericPickerViewController.AcceptAction -= GenericPickerViewController_AcceptAction;
            genericPickerViewController.CancelAction -= GenericPickerViewController_CancelAction;
            genericPickerViewController = null;
            citiesPickerViewModel = null;
        }

        private void ShowSpinner()
        {
            if (!isSpinnerAdded)
            {
                spinnerController.HidesBottomBarWhenPushed = true;
                this.NavigationController.PushViewController(spinnerController, false);
                isSpinnerAdded = true;
            }
        }

        private void HideSpinner()
        {
            if (isSpinnerAdded)
            {
                spinnerController.PopFromNavigationController();
                isSpinnerAdded = false;
            }
        }

        private void ContinueButtonEnableValidation()
        {
            if (!cityTextField.Text.TrimEnd().TrimStart().Equals(string.Empty) &&
                   !addressTextField.Text.TrimStart().TrimEnd().Equals(string.Empty) &&
               IsPhoneSet())
            {
                string cellValidationResult = addressViewModel.ValidateMobile(phoneTextField.Text);

                if (string.IsNullOrEmpty(cellValidationResult))
                {
                    UserAddress userAddress = new UserAddress
                    {
                        AddressComplete = addressTextField.Text,
                        CellPhone = mobile,
                        CityId = (city != null) ? city.Id : string.Empty
                    };

                    string addressValidationResult = addressViewModel.ValidateAddress(userAddress);

                    if (string.IsNullOrEmpty(addressValidationResult))
                    {
                        continueButton.Enabled = true;
                        continueButton.Layer.BackgroundColor = ConstantColor.UiPrimary.CGColor;
                    }
                }
                else
                {
                    continueButton.Enabled = false;
                    continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
                }
            }
            else
            {
                continueButton.Enabled = false;
                continueButton.Layer.BackgroundColor = ConstantColor.UiWelcomeContinueButton.ColorWithAlpha(0.3f).CGColor;
            }
        }

        private bool IsPhoneSet()
        {
            if (phoneTextField.Superview != null)
            {
                if (string.IsNullOrEmpty(phoneTextField.Text))
                {
                    return false;
                }
                else
                {
                    mobile = phoneTextField.Text;
                    return true;
                }
            }
            else
            {
                mobile = ParametersManager.UserContext.CellPhone;
                return true;
            }
        }
    }

    public partial class AddCorrespondenceAddressController : ICorrespondenceAddressModel
    {
        public void AddressSaved()
        {
            HideSpinner();
            //TODO
            //Continuar con el pago (Programada ¿?)
            this.NavigationController.PopViewController(false);
        }

        public void CitiesFetched(IList<City> citiesFetched)
        {
            HideSpinner();

            cities = citiesFetched;
            if (cities.Any())
            {
                ShowCitiesPicker();
            }
        }

        public void HandleError(Exception ex)
        {
            try
            {
                GenericErrorView errorView = GenericErrorView.Create();
                errorView.Configure(ex.Data[nameof(EnumExceptionDataKeys.Code)].ToString(), ex.Data[nameof(EnumExceptionDataKeys.Message)].ToString(), (sender, e) =>
                {
                    errorView.RemoveFromSuperview();
                });
                errorView.Frame = View.Frame;
                View.AddSubview(errorView);
            }
            catch (Exception exception)
            {
                Util.LogException(exception, nameof(AddCorrespondenceAddressController), "");
            }
        }
    }
}

