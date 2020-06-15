using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class MenuServicesViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvItem { get; private set; }
        public TextView TvTitleItem { get; private set; }
        public TextView TvBodyItem { get; private set; }
        public View ViewDivider { get; private set; }
        public LinearLayout LyItem { get; private set; }

        public MenuServicesViewHolder(View itemView, Adapters.IItemMenu item, IList<MenuItem> listMenuItems) : base(itemView)
        {
            TvTitleItem = itemView.FindViewById<TextView>(Resource.Id.tvTitleItem);
            TvBodyItem = itemView.FindViewById<TextView>(Resource.Id.tvBodyItem);
            IvItem = itemView.FindViewById<ImageView>(Resource.Id.ivItem);
            LyItem = itemView.FindViewById<LinearLayout>(Resource.Id.lyItemOtherServices);
            LyItem.Click += delegate { item.OnMenuItemClicked(listMenuItems[AdapterPosition]); };
        }
    }
}