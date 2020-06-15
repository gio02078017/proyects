using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Android.Utilities;
using GrupoExito.Entities;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class HistoricalProductViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvProduct { get; private set; }
        public CheckBox CkProduct { get; private set; }
        public LinearLayout LyInformationProduct { get; private set; }
        public LinearLayout LyHowLike { get; private set; }
        public TextView TvNameProductHistorical { get; private set; }
        public TextView TvQuantityProductHistorical { get; private set; }
        public TextView TvPriceProductHistorical { get; private set; }
        public TextView TvHowLikeHistorical { get; private set; }
        public TextView TvMessageHistorical { get; private set; }

        public HistoricalProductViewHolder(View itemView, IHistoricalProducts iProducts, IList<Product> products) : base(itemView)
        {
            IvProduct = itemView.FindViewById<ImageView>(Resource.Id.ivProduct);
            CkProduct = itemView.FindViewById<CheckBox>(Resource.Id.ckProduct);
            LyInformationProduct = itemView.FindViewById<LinearLayout>(Resource.Id.lyInformationProduct);
            LyHowLike = itemView.FindViewById<LinearLayout>(Resource.Id.lyHowLike);
            TvNameProductHistorical = itemView.FindViewById<TextView>(Resource.Id.tvNameProductHistorical);
            TvQuantityProductHistorical = itemView.FindViewById<TextView>(Resource.Id.tvQuantityProductHistorical);
            TvPriceProductHistorical = itemView.FindViewById<TextView>(Resource.Id.tvPriceProductHistorical);
            TvHowLikeHistorical = itemView.FindViewById<TextView>(Resource.Id.tvHowLikeHistorical);
            TvMessageHistorical = itemView.FindViewById<TextView>(Resource.Id.tvMessageHistorical);
            CkProduct.Click += delegate { iProducts.OnCheck(products[AdapterPosition], CkProduct.Checked); };
        }
    }
};