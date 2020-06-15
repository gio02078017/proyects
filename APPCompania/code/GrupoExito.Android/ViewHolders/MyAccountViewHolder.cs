using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class MyAccountViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvItemAccount { get; private set; }
        public TextView TvTitleItemAccount { get; private set; }
        public TextView TvBodyItemAccount { get; private set; }
        public LinearLayout LyQuantityMessage { get; private set; }
        public TextView TvQuantityMessage { get; private set; }
        public View ViewDivider { get; private set; }
        public LinearLayout LyItemMyAccount { get; private set; }

        public MyAccountViewHolder(View itemView, Adapters.IItemMenu myAccountItem, IList<MenuItem> listMenuItems) : base(itemView)
        {
            TvTitleItemAccount = itemView.FindViewById<TextView>(Resource.Id.tvTitleItemAccount);
            LyItemMyAccount = itemView.FindViewById<LinearLayout>(Resource.Id.lyItemMyAccount);
            IvItemAccount = itemView.FindViewById<ImageView>(Resource.Id.ivItemAccount);
            ViewDivider = itemView.FindViewById<View>(Resource.Id.viewDivider);
            LyItemMyAccount.Click += delegate { myAccountItem.OnMenuItemClicked(listMenuItems[AdapterPosition]); };
        }
    }
}