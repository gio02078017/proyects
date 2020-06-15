// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace GrupoExito.iOS.ViewControllers.ReusableControllers
{
    [Register ("ModifiedProductsViewController")]
    partial class ModifiedProductsViewController
    {
        [Outlet]
        UIKit.UILabel descriptionLabel { get; set; }


        [Outlet]
        UIKit.UITableView productsTableView { get; set; }


        [Outlet]
        UIKit.NSLayoutConstraint productsTableViewHeightConstraint { get; set; }


        [Outlet]
        UIKit.UILabel weSorryTitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (descriptionLabel != null) {
                descriptionLabel.Dispose ();
                descriptionLabel = null;
            }

            if (productsTableView != null) {
                productsTableView.Dispose ();
                productsTableView = null;
            }

            if (productsTableViewHeightConstraint != null) {
                productsTableViewHeightConstraint.Dispose ();
                productsTableViewHeightConstraint = null;
            }

            if (weSorryTitleLabel != null) {
                weSorryTitleLabel.Dispose ();
                weSorryTitleLabel = null;
            }
        }
    }
}