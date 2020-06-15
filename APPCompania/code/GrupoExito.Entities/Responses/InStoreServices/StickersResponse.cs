namespace GrupoExito.Entities.Responses.InStoreServices
{
    using GrupoExito.Entities.Entiites.InStoreServices;
    using GrupoExito.Entities.Responses.Base;
    using System.Collections.Generic;

    public class StickersResponse : ResponseBase
    {
        public StickersResponse()
        {
            StickersPage = new List<StickersByPage>();
        }

        public string LastUpdateStickers { get; set; }
        public int QuantityStickers { get; set; }
        public int QuantityStickersByPage { get; set; }
        public int NumberOfPages { get; set; }
        public IList<StickersByPage> StickersPage { get; set; }
    }
}
