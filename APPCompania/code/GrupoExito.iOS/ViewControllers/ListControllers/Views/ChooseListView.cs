using System;
using GrupoExito.iOS.Utilities;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ListControllers.Views
{
    public partial class ChooseListView : UIView
    {
        #region Constructors 
        static ChooseListView()
        {
            //Static constructor this class
        }

        protected ChooseListView(IntPtr handle) : base(handle)
        {
            //Default Constructor with Argument handle
        }
        #endregion

        #region Override methods
        public override void AwakeFromNib()
        {
            //Method override for load View 
            this.LoadFonts();
            this.LoadCorners();
            this.LoadHandlers();
        }
        #endregion

        #region Methods 
        public void LoadFonts()
        {
            try
            {
                //Load to font size and style
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ConsultSoatView, ConstantMethodName.LoadFonts);
            }
        }

        public void LoadCorners()
        {
            //Load Corners in views 
        }

        private void LoadHandlers()
        {
            try
            {
                //Load handlers of controls
            }
            catch (Exception exception)
            {
                Util.LogException(exception, ConstantReusableViewName.ChooseListView, ConstantMethodName.LoadHandler);
            }
        }
        #endregion
    }
}