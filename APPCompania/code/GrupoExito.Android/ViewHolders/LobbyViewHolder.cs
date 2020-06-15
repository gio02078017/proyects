using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class LobbyViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvItemLobby { get; private set; }
        public TextView TvTitleItemLobby { get; private set; }
        public View ViewDivider { get; private set; }
        public LinearLayout LyItemLobby { get; private set; }

        public LobbyViewHolder(View itemView, Adapters.IItemMenu MenuInterface, IList<MenuItem> listMenuItems) : base(itemView)
        {
            TvTitleItemLobby = itemView.FindViewById<TextView>(Resource.Id.tvTitleItemLobby);
            LyItemLobby = itemView.FindViewById<LinearLayout>(Resource.Id.lyItemLobby);
            IvItemLobby = itemView.FindViewById<ImageView>(Resource.Id.ivItemLobby);
            ViewDivider = itemView.FindViewById<View>(Resource.Id.viewDivider);
            LyItemLobby.Click += delegate { MenuInterface.OnMenuItemClicked(listMenuItems[AdapterPosition]); };
        }
    }
}