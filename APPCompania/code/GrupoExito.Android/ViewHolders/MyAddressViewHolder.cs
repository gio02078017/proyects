using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class MyAddressViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvItemEditAddress { get; private set; }
        public ImageView IvItemDeleteAddress { get; private set; }
        public ImageView IvItemAddress { get; private set; }
        public TextView TvItemTypeAddress { get; private set; }
        public TextView TvItemAddress { get; private set; }
        public View ViewDivider { get; private set; }
        public View ViewTopDivider { get; private set; }
        public LinearLayout LyImageAddress { get; private set; }
        public LinearLayout LyDataAddress { get; private set; }
        public LinearLayout LyAddress { get; private set; }

        public MyAddressViewHolder(View itemView, IMyAddress addressInterface, IList<UserAddress> listUserAddress) : base(itemView)
        {
            TvItemTypeAddress = itemView.FindViewById<TextView>(Resource.Id.tvItemTypeAddress);
            TvItemAddress = itemView.FindViewById<TextView>(Resource.Id.tvItemAddress);
            IvItemEditAddress = itemView.FindViewById<ImageView>(Resource.Id.ivItemEditAddress);
            IvItemDeleteAddress = itemView.FindViewById<ImageView>(Resource.Id.ivItemDeleteAddress);
            IvItemAddress = itemView.FindViewById<ImageView>(Resource.Id.ivItemAddress);
            LyImageAddress = itemView.FindViewById<LinearLayout>(Resource.Id.lyImageAddress);
            LyDataAddress = itemView.FindViewById<LinearLayout>(Resource.Id.lyDataAddress);
            LyAddress = itemView.FindViewById<LinearLayout>(Resource.Id.lyAddress);
            ViewDivider = itemView.FindViewById<View>(Resource.Id.viewDivider);
            ViewTopDivider = itemView.FindViewById<View>(Resource.Id.viewTopDivider);
            IvItemEditAddress.Click += delegate { addressInterface.OnEditItemClicked(listUserAddress[AdapterPosition]); };
            IvItemDeleteAddress.Click += delegate { addressInterface.OnDelateItemClicked(listUserAddress[AdapterPosition]); };
            LyImageAddress.Click += delegate { addressInterface.OnSelectDefaultItemClicked(listUserAddress[AdapterPosition]); };
            LyDataAddress.Click += delegate { addressInterface.OnSelectDefaultItemClicked(listUserAddress[AdapterPosition]); };

        }
    }
}