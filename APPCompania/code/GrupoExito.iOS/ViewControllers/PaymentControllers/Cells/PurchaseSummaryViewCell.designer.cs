// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.Views.PaymentViews.SummaryViews
{
    [Register ("PurchaseSummaryViewCell")]
    partial class PurchaseSummaryViewCell
    {
        [Outlet]
        UIKit.UIStackView stackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView contentViewStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView dashedLineView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel titleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel valueLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (contentViewStackView != null) {
                contentViewStackView.Dispose ();
                contentViewStackView = null;
            }

            if (dashedLineView != null) {
                dashedLineView.Dispose ();
                dashedLineView = null;
            }

            if (titleLabel != null) {
                titleLabel.Dispose ();
                titleLabel = null;
            }

            if (valueLabel != null) {
                valueLabel.Dispose ();
                valueLabel = null;
            }
        }
    }
}