using System;
using Foundation;
using GrupoExito.Entities.Entiites;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Cells
{
    public partial class MyListViewCell : UITableViewCell
    {
        #region Constructors
        static MyListViewCell()
        {
            //Constructors this class without arguments
        }

        protected MyListViewCell(IntPtr handle) : base(handle)
        {
            //Constructor this default class 
        }
        #endregion

        #region Override Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadCorners();
            this.LoadHandlers();
        }
        #endregion

        #region Methods 
        public void loadListViewCell(ShoppingList shoppingList)
        {
            nameListLabel.Text = shoppingList.Name;
            int quantity = int.Parse(shoppingList.QuantityProducts);
            if (quantity > 1 || quantity == 0)
            {
                productsLabel.Text = quantity + " Productos";
            }
            else
            {
                productsLabel.Text = quantity + " Producto";
            }
        }


        private void LoadCorners()
        {
            //Redondeo de las vistas necesarias
        }

        private void LoadHandlers()
        {
            //Carga de eventos
        }
        #endregion
    }
}
