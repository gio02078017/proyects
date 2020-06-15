using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class ScheduleHoursAdapter : RecyclerView.Adapter
    {
        private IList<ScheduleHours> ListScheduleHours { get; set; }
        private IScheduleHours InterfaceScheduleHoursInterface { get; set; }
        private ScheduleHoursViewHolder _ScheduleHoursViewHolder { get; set; }
        private Context Context { get; set; }

        public ScheduleHoursAdapter(IList<ScheduleHours> listScheduleHours, Context context, IScheduleHours interfaceScheduleHoursInterface)
        {
            this.InterfaceScheduleHoursInterface = interfaceScheduleHoursInterface;
            this.Context = context;
            this.ListScheduleHours = listScheduleHours;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemScheduleHours, parent, false);
            ScheduleHoursViewHolder _ScheduleHoursViewHolder = new ScheduleHoursViewHolder(itemView, InterfaceScheduleHoursInterface, ListScheduleHours);
            return _ScheduleHoursViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _ScheduleHoursViewHolder = holder as ScheduleHoursViewHolder;
             ScheduleHours _ScheduleHours = ListScheduleHours[position];
            _ScheduleHoursViewHolder.TvRangleTime.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ScheduleHoursViewHolder.TvCostToSend.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            _ScheduleHoursViewHolder.TvRangleTime.Text = _ScheduleHours.Shedule;

            if (!_ScheduleHours.Store)
            {
                _ScheduleHoursViewHolder.TvCostToSend.Text = StringFormat.ToPrice(Convert.ToDecimal(_ScheduleHours.ShippingCostValue));
            }

            if (_ScheduleHours.Active)
            {
                _ScheduleHoursViewHolder.LyExternalCircle.Visibility = ViewStates.Visible;
                _ScheduleHoursViewHolder.IvChecker.Visibility = ViewStates.Visible;
                _ScheduleHoursViewHolder.LyExternalCircle.Background = ConvertUtilities.ChangeColorCircleDrawable(Context, 0, new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
                _ScheduleHoursViewHolder.LyInternalCircle.SetBackgroundResource(Resource.Drawable.circle_yellow);
                _ScheduleHoursViewHolder.LyChecker.SetBackgroundResource(Resource.Drawable.button_little_primary);
                _ScheduleHoursViewHolder.TvRangleTime.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                _ScheduleHoursViewHolder.TvCostToSend.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
                _ScheduleHoursViewHolder.TvCostToSend.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
                _ScheduleHoursViewHolder.TvRangleTime.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
            }
            else
            {
                _ScheduleHoursViewHolder.LyExternalCircle.Visibility = ViewStates.Visible;
                _ScheduleHoursViewHolder.LyChecker.Background = ConvertUtilities.ChangeColorButtonDrawable(Context, 2, new Color(ContextCompat.GetColor(Context, Resource.Color.colorGrayLight)));
                _ScheduleHoursViewHolder.IvChecker.Visibility = ViewStates.Gone;
                _ScheduleHoursViewHolder.LyExternalCircle.Background = ConvertUtilities.ChangeColorCircleDrawable(Context, 0, new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
                _ScheduleHoursViewHolder.LyInternalCircle.Background = ConvertUtilities.ChangeColorCircleDrawable(Context, 0, new Color(ContextCompat.GetColor(Context, Resource.Color.colorGrayLight)));
                _ScheduleHoursViewHolder.TvRangleTime.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                _ScheduleHoursViewHolder.TvCostToSend.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
                _ScheduleHoursViewHolder.TvCostToSend.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.black)));
                _ScheduleHoursViewHolder.TvRangleTime.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.black)));
            }           
        }

        public override int ItemCount
        {
            get { return ListScheduleHours != null ? ListScheduleHours.Count() : 0; }
        }
    }
}