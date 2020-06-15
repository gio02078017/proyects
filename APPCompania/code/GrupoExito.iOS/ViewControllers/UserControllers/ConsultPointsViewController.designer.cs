// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.UserControllers
{
    [Register ("ConsultPointsViewController")]
    partial class ConsultPointsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel accumulatedDateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel accumulatedLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView accumulatedStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView customSpinnerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView DrawsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton movesButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel overcomeDateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel overcomeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView overcomeStackView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView productsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView RefillsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView spinnerActivityIndicatorView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (accumulatedDateLabel != null) {
                accumulatedDateLabel.Dispose ();
                accumulatedDateLabel = null;
            }

            if (accumulatedLabel != null) {
                accumulatedLabel.Dispose ();
                accumulatedLabel = null;
            }

            if (accumulatedStackView != null) {
                accumulatedStackView.Dispose ();
                accumulatedStackView = null;
            }

            if (customSpinnerView != null) {
                customSpinnerView.Dispose ();
                customSpinnerView = null;
            }

            if (DrawsView != null) {
                DrawsView.Dispose ();
                DrawsView = null;
            }

            if (movesButton != null) {
                movesButton.Dispose ();
                movesButton = null;
            }

            if (overcomeDateLabel != null) {
                overcomeDateLabel.Dispose ();
                overcomeDateLabel = null;
            }

            if (overcomeLabel != null) {
                overcomeLabel.Dispose ();
                overcomeLabel = null;
            }

            if (overcomeStackView != null) {
                overcomeStackView.Dispose ();
                overcomeStackView = null;
            }

            if (productsView != null) {
                productsView.Dispose ();
                productsView = null;
            }

            if (RefillsView != null) {
                RefillsView.Dispose ();
                RefillsView = null;
            }

            if (spinnerActivityIndicatorView != null) {
                spinnerActivityIndicatorView.Dispose ();
                spinnerActivityIndicatorView = null;
            }
        }
    }
}