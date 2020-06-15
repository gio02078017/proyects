using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Resources;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class MyDiscountsAdapter : RecyclerView.Adapter
    {
        private IList<Discount> ListDiscount { get; set; }
        private IDiscount InterfaceDiscount { get; set; }
        private MyDiscountsViewHolder MyDiscountViewHolder { get; set; }
        private Context Context { get; set; }
        private bool IsCouponMania { get; set; }

        public MyDiscountsAdapter(IList<Discount> listDiscount, Context context, IDiscount interfaceDiscount, bool isCouponMania)
        {
            this.InterfaceDiscount = interfaceDiscount;
            this.Context = context;
            this.ListDiscount = listDiscount;
            this.IsCouponMania = isCouponMania;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = IsCouponMania ? LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemCouponMania, parent, false)
                : LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemMyDiscount, parent, false);
            MyDiscountsViewHolder myDiscountViewHolder = new MyDiscountsViewHolder(itemView, InterfaceDiscount, ListDiscount);

            myDiscountViewHolder.LyActivate.Click += delegate
            {
                InterfaceDiscount.OnSelectItemClicked(ListDiscount[myDiscountViewHolder.AdapterPosition]);
            };

            myDiscountViewHolder.LyInactivate.Click += delegate
            {
                InterfaceDiscount.OnInactivatedClicked(ListDiscount[myDiscountViewHolder.AdapterPosition]);
            };

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

            MyDiscountViewHolder = holder as MyDiscountsViewHolder;
            Discount discount = ListDiscount[position];
            MyDiscountViewHolder.TvProductName.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvDiscount.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvActivate.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvInactivate.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamBlack), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvPlu.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamMedium), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvViewMore.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.GothamMedium), TypefaceStyle.Normal);
            MyDiscountViewHolder.TvValidity.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            Glide.With(holder.ItemView.Context).Load(discount.Image).Apply(requestOptions).Thumbnail(0.5f).Into(MyDiscountViewHolder.IvDiscount);
            MyDiscountViewHolder.TvProductName.Text = discount.Description;
            MyDiscountViewHolder.TvDiscount.Text = discount.EventMode;
            MyDiscountViewHolder.TvPlu.Text = AppMessages.PluTitle + " " + discount.Plu;
            MyDiscountViewHolder.TvPlu.Visibility = (!string.IsNullOrEmpty(discount.Plu) && !discount.Plu.Equals("0")) ? ViewStates.Visible : ViewStates.Invisible;
            MyDiscountViewHolder.TvValidity.Visibility = ViewStates.Invisible;

            if (discount.Future)
            {
                DrawLayoutActive(false);
            }
            else
            if (discount.Active)
            {
                MyDiscountViewHolder.LyActivate.Visibility = ViewStates.Gone;
                MyDiscountViewHolder.LyInactivate.Visibility = ViewStates.Visible;
                MyDiscountViewHolder.LyInactivate.SetBackgroundResource(Resource.Drawable.button_discount_transparent_with_border);
                MyDiscountViewHolder.TvInactivate.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDiscounts)));
                MyDiscountViewHolder.TvValidity.Visibility = ViewStates.Visible;
                MyDiscountViewHolder.TvValidity.Text = discount.DaysRedeemLeft == 0 ? AppMessages.ExpiresToday : (AppMessages.LeftDays + " " + discount.DaysRedeemLeft + " " + AppMessages.Days);
            }
            else
            if (discount.Redeemable)
            {
                if (!discount.Active)
                {
                    DrawLayoutActive(true);
                }
                else
                {
                    DrawLayoutActive(false);
                }
            }
            else
            {
                DrawLayoutActive(false);
            }

            if (discount.Future)
            {
                MyDiscountViewHolder.TvActivate.Text = Context.GetString(Resource.String.str_soon);
                MyDiscountViewHolder.TvActivate.SetTextSize(ComplexUnitType.Sp, 14);
            }
            else
            {
                MyDiscountViewHolder.TvActivate.Text = Context.GetString(Resource.String.str_activate);
                MyDiscountViewHolder.TvActivate.SetTextSize(ComplexUnitType.Sp, 14);
            }
        }

        private void DrawLayoutActive(bool active)
        {
            if (active)
            {
                MyDiscountViewHolder.LyActivate.Background = ConvertUtilities.ChangeColorButtonDrawable(Context, 10, new Color(ContextCompat.GetColor(Context, Resource.Color.colorDiscounts)));
                MyDiscountViewHolder.TvActivate.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
                MyDiscountViewHolder.LyActivate.Enabled = true;
                MyDiscountViewHolder.TvActivate.Text = AppMessages.Activate;
                MyDiscountViewHolder.LyActivate.Visibility = ViewStates.Visible;
            }
            else
            {
                MyDiscountViewHolder.LyActivate.Background = ConvertUtilities.ChangeColorButtonDrawable(Context, 10, new Color(ContextCompat.GetColor(Context, Resource.Color.colorGray)));
                MyDiscountViewHolder.TvActivate.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.colorDescription)));
                MyDiscountViewHolder.LyActivate.Enabled = false;
                MyDiscountViewHolder.TvActivate.Text = AppMessages.Activate;
                MyDiscountViewHolder.LyActivate.Visibility = ViewStates.Visible;
            }
        }

        public override int ItemCount
        {
            get { return ListDiscount != null ? ListDiscount.Count() : 0; }
        }
    }
}