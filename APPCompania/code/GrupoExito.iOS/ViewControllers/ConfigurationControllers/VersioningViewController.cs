using System;
using Foundation;
using GrupoExito.Entities.Responses.Generic;
using GrupoExito.iOS.Referentials;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using GrupoExito.Utilities.Resources;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ConfigurationControllers
{
    public partial class VersioningViewController : UIViewControllerBase
    {

        #region Attributes
        private AppVersionResponse appVersionResponse;
        #endregion

        #region Properties
        public AppVersionResponse AppVersionResponse { get => appVersionResponse; set => appVersionResponse = value; }
        #endregion

        #region Constructors
        public VersioningViewController(IntPtr handle) : base(handle)
        {
            //Constructor default this class
        }
        #endregion

        #region LifeCycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            LoadCorners();
            LoadHandlers();
            LoadData();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods
        private void LoadCorners()
        {
            containerView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            refuseButton.Layer.BorderWidth = ConstantStyle.BorderWidth;
            refuseButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
            refuseButton.Layer.BorderColor = ConstantColor.UiPrimary.CGColor;
            updateButton.Layer.CornerRadius = ConstantStyle.CornerRadius;
        }

        private void LoadHandlers()
        {
            refuseButton.TouchUpInside += RefuseButtonTouchUpInside;
            updateButton.TouchUpInside += UpdateButtonTouchUpInside;
        }

        private void LoadData()
        {
            refuseButton.Hidden = true;
        }

        #endregion
        private void RefuseButtonTouchUpInside(object sender, EventArgs e)
        {
            if (ParametersManager.UserContext != null && !ParametersManager.UserContext.IsAnonymous)
            {
                PresentLobbyView();
            }
            else
            {
                PresentLoginView();
            }
        }

        private void UpdateButtonTouchUpInside(object sender, EventArgs e)
        {
            var itunesLink = AppServiceConfiguration.SiteId.Equals("exito") ? new NSUrl(AppConfigurations.AppStoreUrlExito) : 
                new NSUrl(AppConfigurations.AppStoreUrlCarulla);
            UIApplication.SharedApplication.OpenUrl(itunesLink, new NSDictionary() { }, null);
        }
    }
}

