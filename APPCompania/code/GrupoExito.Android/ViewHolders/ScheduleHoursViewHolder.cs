using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    public class ScheduleHoursViewHolder : RecyclerView.ViewHolder
    {
        public ImageView IvPrime { get; private set; }
        public ImageView IvChecker { get; private set; }
        public TextView TvRangleTime { get; private set; }
        public TextView TvCostToSend { get; private set; }
        public LinearLayout LyExternalCircle { get; private set; }
        public LinearLayout LyInternalCircle { get; private set; }
        public LinearLayout LyChecker { get; private set; }

        public ScheduleHoursViewHolder(View itemView, Adapters.IScheduleHours item, IList<ScheduleHours> scheduleHours) : base(itemView)
        {
            IvPrime = itemView.FindViewById<ImageView>(Resource.Id.ivPrime);
            IvChecker = itemView.FindViewById<ImageView>(Resource.Id.ivChecker);
            TvRangleTime = itemView.FindViewById<TextView>(Resource.Id.tvRangleTime);
            TvRangleTime = itemView.FindViewById<TextView>(Resource.Id.tvRangleTime);
            TvCostToSend = itemView.FindViewById<TextView>(Resource.Id.tvCostToSend);
            LyExternalCircle = itemView.FindViewById<LinearLayout>(Resource.Id.lyExternalCircle);
            LyInternalCircle = itemView.FindViewById<LinearLayout>(Resource.Id.lyInternalCircle);
            LyChecker = itemView.FindViewById<LinearLayout>(Resource.Id.lyChecker);
            LyChecker.Click += delegate { item.OnSelectItemClicked(scheduleHours[AdapterPosition], AdapterPosition); };
        }
    }
}