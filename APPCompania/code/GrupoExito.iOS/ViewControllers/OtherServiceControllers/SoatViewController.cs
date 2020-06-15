using System;
using Foundation;
using GrupoExito.DataAgent.DataBase;
using GrupoExito.Entities.Constants.Analytic;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Analytic;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.iOS.ViewControllers.ReusableControllers;
using GrupoExito.Logic.Models.Generic;

namespace GrupoExito.iOS.ViewControllers.OtherServiceControllers
{
    public partial class SoatViewController : UIViewControllerBase
    {
        #region Attributes
        private Soat soat;
        #endregion

        #region Properties
        public Soat Soat { get => soat; set => soat = value; }
        #endregion

        #region Constructors
        public SoatViewController(IntPtr handle) : base(handle)
        {
            //Default Constructor this class
        }
        #endregion

        #region Life Cycle 
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
                Util.LogException(exception, ConstantControllersName.SoatViewController, ConstantMethodName.ViewDidLoad);
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
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SoatViewController, ConstantMethodName.ViewWillAppear);
                ShowMessageException(exception);
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 
        private void LoadHandlers()
        {
            _spinnerActivityIndicatorView.Retry.TouchUpInside += RetryTouchUpInside;
        }

        private void LoadExternalViews()
        {
            try
            {
                this.NavigationController.NavigationBarHidden = false;
                LoadNavigationView(this.NavigationController.NavigationBar);
                LoadCustomSpinnerView(customSpinnerView, ref _spinnerActivityIndicatorView);
                CustomSpinnerViewFromBase = customSpinnerView;
                SpinnerActivityIndicatorViewFromBase = spinnerActivityIndicatorView;
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SoatViewController, ConstantMethodName.LoadExternalViews);
                ShowMessageException(exception);
            }
        }

        private void LoadData()
        {
            try
            {
                StartActivityIndicatorCustom();
                var imageData = new NSData(Soat.QRCode, NSDataBase64DecodingOptions.IgnoreUnknownCharacters);
                soatPreviewImageView.Image = new UIKit.UIImage(imageData);
                StopActivityIndicatorCustom();
                DocumentsDataBaseModel documentsDataBaseModel = new DocumentsDataBaseModel(DocumentsDataBase.Instance);
                documentsDataBaseModel.UpSertSoat(Soat);
                ResumeSoatScreenView();
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantControllersName.SoatViewController, ConstantMethodName.LoadData);
                StartActivityErrorMessage(EnumErrorCode.NoDataFound.ToString(), "No se puede descargar el SOAT, inténtalo de nuevo");
                _spinnerActivityIndicatorView.Retry.SetTitle("REGRESAR", UIKit.UIControlState.Normal);
            }
        }

        private void ResumeSoatScreenView()
        {
            FirebaseEventRegistrationService.Instance.RegisterScreen(AnalyticsScreenView.ResumeSoat, nameof(SoatViewController));
        }
        #endregion

        #region Events 
        private void RetryTouchUpInside(object sender, EventArgs e)
        {
            if (_spinnerActivityIndicatorView.Retry.Title(UIKit.UIControlState.Normal).Equals("REGRESAR"))
            {
                this.NavigationController.PopViewController(true);
            }
            else
            {
                this.LoadData();
            }
        }
        #endregion
    }
}

