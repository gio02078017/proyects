using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Interfaces;
using GrupoExito.Entities;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class AddProductsListViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyFather { get; private set; }
        public LinearLayout LyCheck { get; private set; }
        public ImageView IvCheck { get; private set; }
        public LinearLayout LyIcon { get; private set; }
        public ImageView IvIcon { get; private set; }
        public TextView TvNameProduct { get; private set; }
        public TextView TvDescriptionProducts { get; private set; }
        public TextView TvPriceProducts { get; private set; }
        public View ViewLineEnd { get; private set; }

        public AddProductsListViewHolder(View itemView, IAddProductsList IAddProductsList, IList<Product> listProduct) : base(itemView)
        {
            LyFather = itemView.FindViewById<LinearLayout>(Resource.Id.lyFather);
            LyCheck = itemView.FindViewById<LinearLayout>(Resource.Id.lyCheck);
            IvCheck = itemView.FindViewById<ImageView>(Resource.Id.ivCheck);
            LyIcon = itemView.FindViewById<LinearLayout>(Resource.Id.lyIcon);
            IvIcon = itemView.FindViewById<ImageView>(Resource.Id.ivIcon);
            TvNameProduct = itemView.FindViewById<TextView>(Resource.Id.tvNameProduct);
            TvDescriptionProducts = itemView.FindViewById<TextView>(Resource.Id.tvDescriptionProducts);
            TvPriceProducts = itemView.FindViewById<TextView>(Resource.Id.tvPriceProducts);
            ViewLineEnd = itemView.FindViewById<View>(Resource.Id.viewLineEnd);
            LyCheck.Click += delegate { IAddProductsList.OnItemSelected(listProduct[AdapterPosition], AdapterPosition); };
        }
    }
}