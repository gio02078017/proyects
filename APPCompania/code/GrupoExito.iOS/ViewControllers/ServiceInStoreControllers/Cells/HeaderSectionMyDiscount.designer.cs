// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ServiceInStoreControllers.Cells
{
    [Register ("HeaderSectionMyDiscount")]
    partial class HeaderSectionMyDiscount
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel headerCampaingLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton howToUseThemButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel infoMaxActiveDiscountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView statusHeaderView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (headerCampaingLabel != null) {
                headerCampaingLabel.Dispose ();
                headerCampaingLabel = null;
            }

            if (howToUseThemButton != null) {
                howToUseThemButton.Dispose ();
                howToUseThemButton = null;
            }

            if (infoMaxActiveDiscountLabel != null) {
                infoMaxActiveDiscountLabel.Dispose ();
                infoMaxActiveDiscountLabel = null;
            }

            if (statusHeaderView != null) {
                statusHeaderView.Dispose ();
                statusHeaderView = null;
            }
        }
    }
}