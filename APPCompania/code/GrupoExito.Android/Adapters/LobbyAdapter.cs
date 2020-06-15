using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class LobbyAdapter : RecyclerView.Adapter
    {
        private IList<MenuItem> ListMenuItems { get; set; }
        private Context Context { get; set; }
        private IItemMenu MenuInterface { get; set; }
        private int LayoutHeight { get; set; }

        public LobbyAdapter(IList<MenuItem> listMenuItems, Context context, IItemMenu MenuInterface)
        {
            this.ListMenuItems = listMenuItems;
            this.Context = context;
            this.MenuInterface = MenuInterface;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemLobby, parent, false);
            LobbyViewHolder _LobbyViewHolder = new LobbyViewHolder(itemView, MenuInterface, ListMenuItems);
            return _LobbyViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            LobbyViewHolder _LobbyViewHolder = holder as LobbyViewHolder;
            MenuItem menuItem = ListMenuItems[position];
            _LobbyViewHolder.IvItemLobby.SetImageResource(ConvertUtilities.ResourceId(menuItem.Icon));
            _LobbyViewHolder.TvTitleItemLobby.Text = menuItem.Title;
            
            LinearLayout.LayoutParams lastTxtParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);

            if (!menuItem.Ispair)
            {
                if (position == 0)
                {
                    _LobbyViewHolder.LyItemLobby.SetGravity(GravityFlags.CenterHorizontal);
                    _LobbyViewHolder.ViewDivider.Visibility = ViewStates.Gone;
                }

                if (position % 2 != 0)
                {
                    _LobbyViewHolder.LyItemLobby.SetBackgroundResource(Resource.Drawable.border_right_along);
                }
            }
            else
            {
                if (position == 0 || position % 2 == 0)
                {
                    _LobbyViewHolder.LyItemLobby.SetBackgroundResource(Resource.Drawable.border_right_along);
                }

                if (position <= 1)
                {
                    _LobbyViewHolder.ViewDivider.Visibility = ViewStates.Gone;
                }
            }

            _LobbyViewHolder.TvTitleItemLobby.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);           
        }

        public override int ItemCount
        {
            get { return ListMenuItems != null ? ListMenuItems.Count() : 0; }
        }
    }
}