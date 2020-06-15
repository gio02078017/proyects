using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class CurrentOrderViewHolder : RecyclerView.ViewHolder
    {
        public TextView TvNameOrder { get; private set; }
        public TextView TvOrder { get; private set; }
        public TextView TvSeeOrder { get; private set; }       
        public TextView TvTypeAddressDeliver { get; private set; }
        public TextView TvAddressDeliver { get; private set; }
        public TextView TvDateDeliver { get; private set; }
        public ConstraintLayout LyMessage { get; private set; }
        public TextView TvMessage { get; private set; }

        public CurrentOrderViewHolder(View itemView, IOrders orderInterface, IList<Order> listOrder) : base(itemView)
        {
            TvNameOrder = itemView.FindViewById<TextView>(Resource.Id.tvNameOrder);
            TvOrder = itemView.FindViewById<TextView>(Resource.Id.tvOrder);
            TvSeeOrder = itemView.FindViewById<TextView>(Resource.Id.tvSeeOrder);
            TvTypeAddressDeliver = itemView.FindViewById<TextView>(Resource.Id.tvTypeAddressDeliver);
            TvAddressDeliver = itemView.FindViewById<TextView>(Resource.Id.tvAddressDeliver);
            TvDateDeliver = itemView.FindViewById<TextView>(Resource.Id.tvDateDeliver);
            LyMessage = itemView.FindViewById<ConstraintLayout>(Resource.Id.lyMessage);
            TvMessage = itemView.FindViewById<TextView>(Resource.Id.tvMessage);
            TvSeeOrder.Click += delegate { orderInterface.OnCurrentOrderClicked(listOrder[AdapterPosition]); };
        }
    }
}