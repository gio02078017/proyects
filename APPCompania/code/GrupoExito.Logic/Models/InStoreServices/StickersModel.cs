using GrupoExito.Entities.Entiites.InStoreServices;
using GrupoExito.Entities.Responses.InStoreServices;
using GrupoExito.Utilities.Contracts.InStoreServices;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoExito.Logic.Models.InStoreServices
{
    public class StickersModel
    {
        private IStickersService _stickersService { get; set; }
        private StickersResponse Response { get; set; }

        public StickersModel(IStickersService stickersService)
        {
            this._stickersService = stickersService;
        }

        public async Task<StickersResponse> GetSckers()
        {
            Response = await this._stickersService.GetSckers();
            GetStickersList();
            return Response;
        }

        private void GetStickersList()
        {
            if (Response.QuantityStickersByPage > 0 && Response.QuantityStickers > 0)
            {
                int modNumberOfPages = Response.QuantityStickers % Response.QuantityStickersByPage;
                int divNumberOfPages = Response.QuantityStickers / Response.QuantityStickersByPage;
                Response.NumberOfPages = modNumberOfPages > 0 ? divNumberOfPages + 1 : divNumberOfPages;
                List<Sticker> stickers = null;
                Sticker sticker = null;
                int quantityStickers = 0;

                for (int i = 0; i < Response.NumberOfPages; i++)
                {
                    stickers = new List<Sticker>();

                    for (int j = 0; j < Response.QuantityStickersByPage; j++)
                    {
                        sticker = new Sticker
                        {
                            Fill = (Response.QuantityStickers > 0 && quantityStickers < Response.QuantityStickers) ? true : false,
                            Numer = quantityStickers + 1
                        };

                        quantityStickers += 1;
                        stickers.Add(sticker);
                    }

                    Response.StickersPage.Add(new StickersByPage { Stickers = stickers });
                }
            }
            else
            {
                Response.StickersPage = GetStickersEmpty();
            }
        }

        private IList<StickersByPage> GetStickersEmpty()
        {
            IList<StickersByPage> StickersPage = new List<StickersByPage>();
            int numberOfPages = int.Parse(AppConfigurations.NumberOfPageSticker);
            int quantityStickers = 0;
            List<Sticker> stickers = null;
            Sticker sticker = null;

            for (int i = 0; i < numberOfPages; i++)
            {
                stickers = new List<Sticker>();

                for (int j = 0; j < Response.QuantityStickersByPage; j++)
                {
                    quantityStickers += 1;

                    sticker = new Sticker
                    {
                        Fill = false,
                        Numer = quantityStickers
                    };

                    stickers.Add(sticker);
                };

                StickersPage.Add(new StickersByPage { Stickers = stickers });
            }

            return StickersPage;
        }
    }
}
