using System;
using GrupoExito.Entities.Entiites;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    public partial class NameListViewCell : UITableViewCell
    {
        #region Constructors
        public NameListViewCell(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Public Methods
        public void LoadNameList(ShoppingList shopping)
        {
            nameListLabel.Text = shopping.Name;
            if (!string.IsNullOrEmpty(ParametersManager.ShoppingListSelectedId) && ParametersManager.ShoppingListSelectedId.Equals(shopping.Id))
            {
                SetSelected(true, true);
            }
            else
            {
                SetSelected(false, true);
            }
        }
        #endregion

        #region Overrides Methods
        public override void SetSelected(bool selected, bool animated)
        {
            if (selected)
            {
                ImageView.Image = UIImage.FromFile(ConstantImages.Checkbox_primario_lleno);
            }
            else
            {
                ImageView.Image = UIImage.FromFile(ConstantImages.Checkbox_primario);
            }
        }
        #endregion
    }
}

