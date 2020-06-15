using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites.InStoreServices;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class StickersViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvSticker { get; private set; }
        public TextView TvNameSticker { get; private set; }
        public StickersViewHolder(View itemView, IList<Sticker> listSticker) : base(itemView)
        {
            IvSticker = itemView.FindViewById<ImageView>(Resource.Id.ivSticker);
            TvNameSticker = itemView.FindViewById<TextView>(Resource.Id.tvNameSticker);
        }
    }
}