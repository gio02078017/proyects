using System;

using Foundation;
using GrupoExito.Entities.Entiites.InStoreServices;
using GrupoExito.iOS.Utilities.Constant;
using UIKit;

namespace GrupoExito.iOS.Views.UserViews.Cells
{
    public partial class StickersViewCell : UICollectionViewCell
    {
       
        static StickersViewCell()
        {
       
        }

        protected StickersViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void LoadSticker( Sticker stricker)
        {

            stickersImageView.Image = stricker.Fill ? UIImage.FromFile(ConstantImages.Sticker) : UIImage.FromFile(ConstantImages.StickerPrimario);
            numberStickerLabel.Text = stricker.Numer.ToString();
        }
    }
}
