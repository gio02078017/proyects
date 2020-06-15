using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class HistoricalOrderViewHolder : RecyclerView.ViewHolder
    {
        public TextView TvIdOrder { get; private set; }
        public TextView TvNameOrder { get; private set; }
        public TextView TvOrder { get; private set; }
        public TextView TvNameDate { get; private set; }
        public TextView TvDateOrder { get; private set; }
        public LinearLayout LyHistoricalOrder { get; private set; }

        public HistoricalOrderViewHolder(View itemView, IOrders orderInterface, IList<Order> listOrder) : base(itemView)
        {
            TvIdOrder = itemView.FindViewById<TextView>(Resource.Id.tvIdOrder);
            TvNameOrder = itemView.FindViewById<TextView>(Resource.Id.tvNameOrder);
            TvOrder = itemView.FindViewById<TextView>(Resource.Id.tvOrder);
            TvNameDate = itemView.FindViewById<TextView>(Resource.Id.tvNameDate);
            TvDateOrder = itemView.FindViewById<TextView>(Resource.Id.tvDateOrder);
            LyHistoricalOrder = itemView.FindViewById<LinearLayout>(Resource.Id.lyHistoricalOrder);
            LyHistoricalOrder.Click += delegate { orderInterface.OnHistoricalOrderClicked(listOrder[AdapterPosition]); };
        }
    }
}