using System;
using GrupoExito.DataAgent.Services.Products;
using GrupoExito.Entities.Responses.Products;
using GrupoExito.iOS.Utilities;
using GrupoExito.Logic.Models.Orders;
using GrupoExito.Logic.Models.Products;

namespace GrupoExito.iOS.Referentials
{
    public class BaseListController : UIViewControllerBase
    {
        #region Attributes
        protected ShoppingListModel ShoppingListModel { get; set; }
        protected OrderModel OrderModel { get; set; }
        protected SuggestedProductsResponse SuggestedProductsResponse;
        #endregion

        #region Constructors
        public BaseListController(IntPtr handle) : base(handle)
        {
            ShoppingListModel = new ShoppingListModel(new ShoppingListService(DeviceManager.Instance));
        }
        #endregion

        #region Life Cycle 

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
        #endregion

        #region Methods 


        #endregion
    }
}

