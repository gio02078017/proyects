using System;
using Foundation;
using GrupoExito.iOS.Utilities.Constant;
using SDWebImage;
using UIKit;

namespace GrupoExito.iOS.ViewControllers.ProductControllers.Cells
{
    public partial class HeaderProductViewCell : UITableViewCell
    {
        #region Attributes
        public static readonly NSString Key = new NSString("HeaderProductViewCell");
        public static readonly UINib Nib;

        private EventHandler filterAction;
        private EventHandler orderAction;
        #endregion

        #region Properties
        public EventHandler FilterAction { get => filterAction; set => filterAction = value; }
        public EventHandler OrderAction { get => orderAction; set => orderAction = value; }
        #endregion

        #region Constructors
        static HeaderProductViewCell()
        {
            Nib = UINib.FromName("HeaderProductViewCell", NSBundle.MainBundle);
        }

        protected HeaderProductViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }
        #endregion

        #region Overrides Methods
        [Export("awakeFromNib")]
        public override void AwakeFromNib()
        {
            this.LoadHandlers();
            this.LoadCorners();
        }
        #endregion

        #region Private Methods
        private void LoadHandlers()
        {
            this.filterButton.TouchUpInside += FilterButtonTouchUpInside;
            this.orderByButton.TouchUpInside += OrderByButtonTouchUpInside;
        }

        private void LoadCorners()
        {
            orderByView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            orderByView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            orderByView.Layer.BorderColor = ConstantColor.UiFilterOrderTextNotSelected.ColorWithAlpha(0.3f).CGColor;

            filterByView.Layer.CornerRadius = ConstantStyle.CornerRadius;
            filterByView.Layer.BorderWidth = ConstantStyle.BorderWidth;
            filterByView.Layer.BorderColor = ConstantColor.UiFilterOrderTextNotSelected.ColorWithAlpha(0.3f).CGColor;
        }

        private  void DrawStatus(bool isFilterAction)
        {
            if (isFilterAction)
            {
                ClearButtonOrder();
            }
            else
            {
                ClearButtonFilter();
            }
        }

        #endregion

        #region Public Methods
        public void SetTitle(string urlImage, string category, bool isFilterAction)
        {
            this.categoryNameLabel.Text = category;
            this.categoryIconImageView.SetImage(new NSUrl(urlImage), UIImage.FromFile(ConstantImages.SinImagen) , HandleSDExternalCompletionHandler );
            DrawStatus(isFilterAction);
        }

        public void SetTitle(UIImage image, string category, bool isFilterAction)
        {
            this.categoryNameLabel.Text = category;
            this.categoryIconImageView.Image = image;
            DrawStatus(isFilterAction);
        }


        private void ClearButtonFilter()
        {
            filterByView.Layer.BackgroundColor = ConstantColor.UiFilterOrderButtonNotSelected.CGColor;
            filterTitleLabel.TextColor = ConstantColor.UiFilterOrderTextNotSelected;
            filterIconImageView.Image = UIImage.FromFile(ConstantImages.Filtro);

            orderByView.Layer.BackgroundColor = ConstantColor.UiFilterOrderButtonSelected.CGColor;
            orderByTitleLabel.TextColor = ConstantColor.UiFilterOrderTextSelected;
            orderByIconImageView.Image = UIImage.FromFile(ConstantImages.OrderPrimario);
        }

        private void ClearButtonOrder()
        {
            filterByView.Layer.BackgroundColor = ConstantColor.UiFilterOrderButtonSelected.CGColor;
            filterTitleLabel.TextColor = ConstantColor.UiFilterOrderTextSelected;
            filterIconImageView.Image = UIImage.FromFile(ConstantImages.FiltroSecundario);

            orderByView.Layer.BackgroundColor = ConstantColor.UiFilterOrderButtonNotSelected.CGColor;
            orderByTitleLabel.TextColor = ConstantColor.UiFilterOrderTextNotSelected;
            orderByIconImageView.Image = UIImage.FromFile(ConstantImages.Order);
        }
        #endregion

        #region Events
        private void FilterButtonTouchUpInside(object sender, EventArgs e)
        {
            ClearButtonOrder();
            filterAction?.Invoke(sender, e);
        }

        private void OrderByButtonTouchUpInside(object sender, EventArgs e)
        {
            ClearButtonFilter();
            orderAction?.Invoke(sender, e);
        }

        private void HandleSDExternalCompletionHandler(UIImage image, NSError error, SDImageCacheType cacheType, NSUrl imageUrl)
        {
        }
        #endregion
    }
}
