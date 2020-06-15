using Android.Content;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class HistoricalOrderAdapter : RecyclerView.Adapter
    {
        private IList<Order> ListOrder { get; set; }
        private Context Context { get; set; }
        private IOrders OrderInterface { get; set; }

        public HistoricalOrderAdapter(IList<Order> listOrder, Context context, IOrders orderInterface)
        {
            this.OrderInterface = orderInterface;
            this.Context = context;
            this.ListOrder = listOrder;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemHistoricalOrder, parent, false);
            HistoricalOrderViewHolder HistoricalOrderHolder = new HistoricalOrderViewHolder(itemView, OrderInterface, ListOrder);
            return HistoricalOrderHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            HistoricalOrderViewHolder historicalOrderHolder = holder as HistoricalOrderViewHolder;
            Order order = ListOrder[position];
            historicalOrderHolder.TvIdOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            historicalOrderHolder.TvNameOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            historicalOrderHolder.TvOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayExtraBold), TypefaceStyle.Normal);
            historicalOrderHolder.TvNameDate.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            historicalOrderHolder.TvDateOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            historicalOrderHolder.TvIdOrder.Text = (position + 1) + ".";
            historicalOrderHolder.TvOrder.Text = order.Id;
            historicalOrderHolder.TvDateOrder.Text = order.Date;
        }

        public override int ItemCount
        {
            get { return ListOrder != null ? ListOrder.Count() : 0; }
        }
    }
}