using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Resources;
using android = Android;

namespace GrupoExito.Android.Adapters
{
    public class MyDiscountsDetailAdapter : RecyclerView.Adapter
    {
        private IList<Discount> ListDiscount { get; set; }
        private IDiscount InterfaceDiscount { get; set; }
        private MyDiscountsDetailViewHolder MyDiscountViewHolder { get; set; }
        private Context Context { get; set; }
        private bool IsRedeemed { get; set; }

        public MyDiscountsDetailAdapter(IList<Discount> listDiscount, Context context, IDiscount interfaceDiscount, bool IsRedeemed)
        {
            this.InterfaceDiscount = interfaceDiscount;
            this.Context = context;
            this.ListDiscount = listDiscount;
            this.IsRedeemed = IsRedeemed;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemMyDiscountDetail, parent, false);
            MyDiscountsDetailViewHolder myDiscountViewHolder = new MyDiscountsDetailViewHolder(itemView, InterfaceDiscount, ListDiscount);
            return myDiscountViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                 .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.All)
                 .DontAnimate()
                 .Placeholder(Resource.Drawable.sin_imagen)
                 .Error(Resource.Drawable.sin_imagen_descuento)
                 .DontTransform());

            MyDiscountViewHolder = holder as MyDiscountsDetailViewHolder;
            Discount discount = ListDiscount[position];

            MyDiscountViewHolder.TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvDiscount.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvValidity.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamMedium), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvPlu.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamMedium), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvViewMore.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamMedium), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvInactivate.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            Glide.With(holder.ItemView.Context).Load(discount.Image).Apply(requestOptions).Thumbnail(0.5f).Into(MyDiscountViewHolder.IvDiscount);
            MyDiscountViewHolder.TvProductName.Text = discount.Description;
            MyDiscountViewHolder.TvDiscount.Text = discount.EventMode;
            MyDiscountViewHolder.TvPlu.Text = AppMessages.PluTitle + " " + discount.Plu;
            MyDiscountViewHolder.TvPlu.Visibility = (!string.IsNullOrEmpty(discount.Plu) && !discount.Plu.Equals("0")) ? ViewStates.Visible : ViewStates.Invisible;            
            MyDiscountViewHolder.TvValidity.Visibility = ViewStates.Invisible;


            if (IsRedeemed)
            {
                MyDiscountViewHolder.TvValidity.Visibility = ViewStates.Invisible;
                MyDiscountViewHolder.TvViewMore.Visibility = ViewStates.Gone;
                MyDiscountViewHolder.TvDiscount.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
                MyDiscountViewHolder.TvProductName.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
                MyDiscountViewHolder.TvPlu.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
                MyDiscountViewHolder.LyInactivate.Background = ConvertUtilities.ChangeColorButtonDrawable(Context, 10, new Color(ContextCompat.GetColor(Context, Resource.Color.colorGray)));
                MyDiscountViewHolder.TvInactivate.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
                MyDiscountViewHolder.LyInactivate.Enabled = false;
                MyDiscountViewHolder.TvInactivate.Text = AppMessages.Redeemed;

            }
            else
            {
                MyDiscountViewHolder.TvValidity.Visibility = ViewStates.Visible;
                MyDiscountViewHolder.TvValidity.Text = discount.DaysRedeemLeft == 0 ? AppMessages.ExpiresToday : (AppMessages.LeftDays + " " + discount.DaysRedeemLeft + " " + AppMessages.Days);
                MyDiscountViewHolder.LyInactivate.SetBackgroundResource(Resource.Drawable.button_discount_transparent_with_border);
                MyDiscountViewHolder.TvInactivate.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDiscounts)));
                MyDiscountViewHolder.LyInactivate.Enabled = true;
            }

        }

        public override int ItemCount
        {
            get { return ListDiscount != null ? ListDiscount.Count() : 0; }
        }
    }
}