using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class MenuServicesAdapter : RecyclerView.Adapter
    {
        private IList<MenuItem> ListMenuItems { get; set; }
        private Context Context { get; set; }
        private IItemMenu ItemMenuInterface { get; set; }

        public MenuServicesAdapter(IList<MenuItem> listMenuItems, Context context, IItemMenu itemMenuInterface)
        {
            this.ListMenuItems = listMenuItems;
            this.Context = context;
            this.ItemMenuInterface = itemMenuInterface;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuServicesViewHolder menuServicesViewHolder = holder as MenuServicesViewHolder;
            MenuItem menuItem = ListMenuItems[position];
            menuServicesViewHolder.IvItem.SetImageResource(ConvertUtilities.ResourceId(menuItem.Icon));
            menuServicesViewHolder.TvTitleItem.Text = menuItem.Title;

            if (!string.IsNullOrEmpty(menuItem.Subtitle))
            {
                menuServicesViewHolder.TvBodyItem.Text = menuItem.Subtitle;
                menuServicesViewHolder.TvBodyItem.Visibility = ViewStates.Visible;
            }

            menuServicesViewHolder.TvTitleItem.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            menuServicesViewHolder.TvBodyItem.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemOtherServices, parent, false);
            MenuServicesViewHolder menuServicesViewHolder = new MenuServicesViewHolder(itemView, ItemMenuInterface, ListMenuItems);
            return menuServicesViewHolder;
        }

        public override int ItemCount
        {
            get { return ListMenuItems != null ? ListMenuItems.Count() : 0; }
        }
    }
}