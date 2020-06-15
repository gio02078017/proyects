using UIKit;

namespace GrupoExito.iOS.ViewControllers.PaymentControllers.Models
{
    public class TwoCellSelectorModel
    {
        #region Attributes
        private string title;
        private string titleFirstOption;
        private string valueFirstOption;
        private string titleSecondOption;
        private string valueSecondOption;
        private UIColor backgroundColor;
        private UIColor selectedColor;
        private UIColor unselectedColor;
        private int leadingLength;
        #endregion

        #region Properties
        public string Title { get => title; set => title = value; }
        public string TitleFirstOption { get => titleFirstOption; set => titleFirstOption = value; }
        public string ValueFirstOption { get => valueFirstOption; set => valueFirstOption = value; }
        public string TitleSecondOption { get => titleSecondOption; set => titleSecondOption = value; }
        public string ValueSecondOption { get => valueSecondOption; set => valueSecondOption = value; }
        public UIColor BackgroundColor { get => backgroundColor; set => backgroundColor = value; }
        public UIColor SelectedColor { get => selectedColor; set => selectedColor = value; }
        public UIColor UnselectedColor { get => unselectedColor; set => unselectedColor = value; }
        public int LeadingLength { get => leadingLength; set => leadingLength = value; }
        #endregion

        #region Constructors
        public TwoCellSelectorModel()
        {
            //Default constructor this class
        }
        #endregion
    }
}
