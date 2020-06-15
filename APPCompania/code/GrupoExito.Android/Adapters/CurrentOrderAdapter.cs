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
    public class CurrentOrderAdapter : RecyclerView.Adapter
    {
        private IList<Order> ListOrder { get; set; }
        private Context Context { get; set; }
        private IOrders OrderInterface { get; set; }

        public CurrentOrderAdapter(IList<Order> listOrder, Context context, IOrders orderInterface)
        {
            this.OrderInterface = orderInterface;
            this.Context = context;
            this.ListOrder = listOrder;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemCurrentOrder, parent, false);
            CurrentOrderViewHolder currentOrderViewHolder = new CurrentOrderViewHolder(itemView, OrderInterface, ListOrder);
            return currentOrderViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CurrentOrderViewHolder currentOrderHolder = holder as CurrentOrderViewHolder;
            Order order = ListOrder[position];
            currentOrderHolder.TvNameOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            currentOrderHolder.TvOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            currentOrderHolder.TvSeeOrder.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            currentOrderHolder.TvTypeAddressDeliver.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            currentOrderHolder.TvAddressDeliver.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            currentOrderHolder.TvDateDeliver.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            currentOrderHolder.TvMessage.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            currentOrderHolder.TvOrder.Text = order.Id;
            currentOrderHolder.TvDateDeliver.Text = order.Date;
            currentOrderHolder.TvAddressDeliver.Text = order.Address;
        }

        public override int ItemCount
        {
            get { return ListOrder != null ? ListOrder.Count() : 0; }
        }
    }
}