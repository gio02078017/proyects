using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class NotificationsAdapter : RecyclerView.Adapter
    {
        private IList<AppNotification> ListNotifications { get; set; }
        private INotifications InterfaceNotification { get; set; }
        private NotificationsViewHolder _NotificationsViewHolder { get; set; }
        private Context Context { get; set; }

        public NotificationsAdapter(IList<AppNotification> listNotifications, Context context, INotifications interfaceNotification)
        {
            this.InterfaceNotification = interfaceNotification;
            this.Context = context;
            this.ListNotifications = listNotifications;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemNotification, parent, false);
            NotificationsViewHolder _NotificationsViewHolder = new NotificationsViewHolder(itemView, InterfaceNotification, ListNotifications);
            return _NotificationsViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _NotificationsViewHolder = holder as NotificationsViewHolder;
            AppNotification itemNotification = ListNotifications[position];
            _NotificationsViewHolder.TvNotification.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            _NotificationsViewHolder.TvTimeInSide.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Italic);
            _NotificationsViewHolder.TvTimeOutSide.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Italic);
            _NotificationsViewHolder.TvNotification.Text = itemNotification.Text;
            _NotificationsViewHolder.TvTimeInSide.Visibility = ViewStates.Gone;
            _NotificationsViewHolder.TvTimeOutSide.Visibility = ViewStates.Gone;
            _NotificationsViewHolder.IvDelete.Visibility = ViewStates.Gone;

            if (itemNotification.Readed)
            {
                _NotificationsViewHolder.LyBorder.SetBackgroundResource(Resource.Drawable.button_light_grey);
                _NotificationsViewHolder.TvNotification.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
            }
            else
            {
                _NotificationsViewHolder.LyBorder.SetBackgroundResource(Resource.Drawable.button_card_caducity);
                _NotificationsViewHolder.TvNotification.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.black)));
            }
        }

        public override int ItemCount
        {
            get { return ListNotifications != null ? ListNotifications.Count() : 0; }
        }
    }
}