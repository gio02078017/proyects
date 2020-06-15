using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class NotificationsViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyItemNotification { get; private set; }
        public ImageView IvDelete { get; private set; }
        public LinearLayout LyBorder { get; private set; }
        public TextView TvNotification { get; private set; }
        public TextView TvTimeInSide { get; private set; }
        public TextView TvTimeOutSide { get; private set; } 

        public NotificationsViewHolder(View itemView, Adapters.INotifications item, IList<AppNotification> listNotifications) : base(itemView)
        {
            LyItemNotification = itemView.FindViewById<LinearLayout>(Resource.Id.lyItemNotification);
            IvDelete = itemView.FindViewById<ImageView>(Resource.Id.ivDelete);
            LyBorder = itemView.FindViewById<LinearLayout>(Resource.Id.lyBorder);
            TvNotification = itemView.FindViewById<TextView>(Resource.Id.tvNotification);
            TvTimeInSide = itemView.FindViewById<TextView>(Resource.Id.tvTimeInSide);
            TvTimeOutSide = itemView.FindViewById<TextView>(Resource.Id.tvTimeOutSide);
            LyItemNotification.Click += delegate { item.OnViewItemClicked(listNotifications[AdapterPosition]); };
            IvDelete.Click += delegate { item.OnDelateItemClicked(listNotifications[AdapterPosition]); };

        }
    }
}