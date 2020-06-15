using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Interfaces;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class GetSoatViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyGetSoat { get; private set; }
        public LinearLayout LyIvSoat { get; private set; }
        public ImageView IvSoat { get; private set; }        
        public TextView TvPlateSoat { get; private set; }
        public LinearLayout LyDeleteSoat { get; private set; }
        public ImageView IvDeleteSoat { get; private set; }

        public GetSoatViewHolder(View itemView, IGetSoats getSoatInterface, IList<Soat> ListSoat) : base(itemView)
        {
            LyGetSoat = itemView.FindViewById<LinearLayout>(Resource.Id.lyGetSoat);
            LyIvSoat = itemView.FindViewById<LinearLayout>(Resource.Id.lyIvSoat);
            IvSoat = itemView.FindViewById<ImageView>(Resource.Id.ivSoat);
            TvPlateSoat = itemView.FindViewById<TextView>(Resource.Id.tvPlateSoat);
            LyDeleteSoat = itemView.FindViewById<LinearLayout>(Resource.Id.lyDeleteSoat);
            IvDeleteSoat = itemView.FindViewById<ImageView>(Resource.Id.ivDeleteSoat);
            LyDeleteSoat.Click += delegate { getSoatInterface.OnItemDeleted(ListSoat[AdapterPosition], AdapterPosition); };
            LyGetSoat.Click += delegate { getSoatInterface.OnItemSelected(ListSoat[AdapterPosition], AdapterPosition); };
        }
    }
}