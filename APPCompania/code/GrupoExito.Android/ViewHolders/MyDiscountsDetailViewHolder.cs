using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Android.ViewHolders
{
    public class MyDiscountsDetailViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvDiscount { get; private set; }
        public TextView TvProductName { get; private set; }
        public TextView TvDiscount { get; private set; }
        public TextView TvPlu { get; private set; }
        public TextView TvValidity { get; private set; }
        public TextView TvViewMore { get; private set; }
        public LinearLayout LyInactivate { get; private set; }
        public TextView TvInactivate { get; private set; }

        public MyDiscountsDetailViewHolder(View itemView, Adapters.IDiscount item, IList<Discount> listDiscounts) : base(itemView)
        {
            IvDiscount = itemView.FindViewById<ImageView>(Resource.Id.ivDiscount);
            TvProductName = itemView.FindViewById<TextView>(Resource.Id.tvNameProducto);
            TvDiscount = itemView.FindViewById<TextView>(Resource.Id.tvDiscount);
            TvValidity = itemView.FindViewById<TextView>(Resource.Id.tvValidity);
            TvPlu = itemView.FindViewById<TextView>(Resource.Id.tvPlu);
            TvViewMore = itemView.FindViewById<TextView>(Resource.Id.tvViewMore);
            LyInactivate = itemView.FindViewById<LinearLayout>(Resource.Id.lyInactivate);
            TvInactivate = itemView.FindViewById<TextView>(Resource.Id.tvInactivate);
            TvViewMore.Click += delegate { item.OnTermsClicked(listDiscounts[AdapterPosition]); };
            LyInactivate.Click += delegate { item.OnInactivatedClicked(listDiscounts[AdapterPosition]); };
        }
    }
}