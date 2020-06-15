using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Android.ViewHolders
{
    public class MyDiscountsViewHolder : RecyclerView.ViewHolder
    {
        public TextView TvValidity { get; private set; }
        public ImageView IvDiscount { get; private set; }
        public TextView TvProductName { get; private set; }
        public TextView TvDiscount { get; private set; }
        public LinearLayout LyActivate { get; private set; }
        public TextView TvActivate { get; private set; }
        public LinearLayout LyInactivate { get; private set; }
        public TextView TvInactivate { get; private set; }
        public TextView TvPlu { get; private set; }
        public TextView TvTermsAndConditions { get; private set; }
        public TextView TvViewMore { get; private set; }

        public MyDiscountsViewHolder(View itemView, Adapters.IDiscount item, IList<Discount> listDiscounts) : base(itemView)
        {
            IvDiscount = itemView.FindViewById<ImageView>(Resource.Id.ivDiscount);
            TvProductName = itemView.FindViewById<TextView>(Resource.Id.tvNameProducto);
            TvDiscount = itemView.FindViewById<TextView>(Resource.Id.tvDiscount);
            LyActivate = itemView.FindViewById<LinearLayout>(Resource.Id.lyActivate);
            TvActivate = itemView.FindViewById<TextView>(Resource.Id.tvActivate);
            LyInactivate = itemView.FindViewById<LinearLayout>(Resource.Id.lyInactivate);
            TvInactivate = itemView.FindViewById<TextView>(Resource.Id.tvInactivate);
            TvPlu = itemView.FindViewById<TextView>(Resource.Id.tvPlu);
            TvViewMore = itemView.FindViewById<TextView>(Resource.Id.tvViewMore);
            TvValidity = itemView.FindViewById<TextView>(Resource.Id.tvValidity);

            TvViewMore.Click += delegate { item.OnTermsClicked(listDiscounts[AdapterPosition]); };
        }
    }
}