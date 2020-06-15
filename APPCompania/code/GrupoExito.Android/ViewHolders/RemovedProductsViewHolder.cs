using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class RemovedProductsViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyLista { get; private set; }
        public ImageView IvLista { get; private set; }
        public TextView TvNameProduct { get; private set; }
        public TextView TvDescriptionProducts { get; private set; }
        public RemovedProductsViewHolder(View itemView, IList<Product> listProduct) : base(itemView)
        {
            LyLista = itemView.FindViewById<LinearLayout>(Resource.Id.lyLista);
            IvLista = itemView.FindViewById<ImageView>(Resource.Id.ivLista);
            TvNameProduct = itemView.FindViewById<TextView>(Resource.Id.tvNameProduct);
            TvDescriptionProducts = itemView.FindViewById<TextView>(Resource.Id.tvDescriptionProducts);
        }
    }
}