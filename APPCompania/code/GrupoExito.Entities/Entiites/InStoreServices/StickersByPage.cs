namespace GrupoExito.Entities.Entiites.InStoreServices
{
    using System.Collections.Generic;

    public class StickersByPage
    {
        public StickersByPage()
        {
            this.Stickers = new List<Sticker>();
        }

        public IList<Sticker> Stickers { get; set; }
    }
}
