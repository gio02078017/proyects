// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Views
{
    [Register ("PreviewEventHeaderMyDiscount")]
    partial class PreviewEventHeaderMyDiscount
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView containerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel headerCampaingLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (containerView != null) {
                containerView.Dispose ();
                containerView = null;
            }

            if (headerCampaingLabel != null) {
                headerCampaingLabel.Dispose ();
                headerCampaingLabel = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}