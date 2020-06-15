using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Signature;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class MyCardsAdapter : RecyclerView.Adapter
    {
        private IList<CreditCard> ListMyCard { get; set; }
        private IMyCards MyCardInterface { get; set; }
        private MyCardViewHolder MyCardViewHolder { get; set; }
        private Context Context { get; set; }
        private bool ShowDelete { get; set; }

        public MyCardsAdapter(IList<CreditCard> listMyCard, Context context, IMyCards myCardInterface, bool showDelete = true)
        {
            this.MyCardInterface = myCardInterface;
            this.Context = context;
            this.ListMyCard = listMyCard;
            this.ShowDelete = showDelete;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemMyCards, parent, false);
            MyCardViewHolder myCardViewHolder = new MyCardViewHolder(itemView, MyCardInterface, ListMyCard);

            return myCardViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.None))
                .Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond)));

            MyCardViewHolder = holder as MyCardViewHolder;
            CreditCard myCard = ListMyCard[position];
            MyCardViewHolder.TvNameCard.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            MyCardViewHolder.TvNumberCard.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            MyCardViewHolder.TvMessageCuducity.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            MyCardViewHolder.TvDateCuducity.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewayMedium), TypefaceStyle.Normal);
            Glide.With(holder.ItemView.Context).Load(ConvertUtilities.ResourceId(myCard.Image)).Apply(requestOptions).Thumbnail(0.5f).Into(MyCardViewHolder.IvNameCard);
            MyCardViewHolder.TvNameCard.Text = myCard.Name.ToLower();
            MyCardViewHolder.TvNumberCard.Text = myCard.NumberCard.ToLower();

            MyCardViewHolder.LyDeleteCard.Visibility = ShowDelete ? ViewStates.Visible : ViewStates.Gone;
            

            if (!myCard.Type.Equals(ConstCreditCardType.Exito))
            {
                if (!string.IsNullOrEmpty(myCard.ExpirationMonth) && !string.IsNullOrEmpty(myCard.ExpirationYear))
                {
                    string monthName = myCard.ExpirationMonth.Equals("0") ? string.Empty :
                        DateTimeFormatInfo.CurrentInfo.GetMonthName(int.Parse(myCard.ExpirationMonth));

                    string Year = myCard.ExpirationYear.Equals("0") ? string.Empty : myCard.ExpirationYear;

                    MyCardViewHolder.TvDateCuducity.Text = string.Format(AppMessages.CreditCardDateMessage,
                        monthName + " " + Year);
                }

                MyCardViewHolder.TvMessageCuducity.Text = string.Format(AppMessages.CreditCardCaducedMessage, myCard.Image);
                MyCardViewHolder.LyMessageAlert.Visibility = myCard.IsNextCaduced ? ViewStates.Visible : ViewStates.Gone;                
            }
            else
            {
                MyCardViewHolder.LyMessageAlert.Visibility = ViewStates.Gone;
            }

            if (myCard.Selected)
            {
                MyCardViewHolder.LyMyCard.SetBackgroundResource(Resource.Drawable.credit_card_item_primary);
                MyCardViewHolder.LyCircle.Visibility = ViewStates.Visible;
            }
            else
            {
                MyCardViewHolder.LyMyCard.SetBackgroundResource(Resource.Drawable.credit_card_item);
                MyCardViewHolder.LyCircle.Visibility = ViewStates.Gone;
            }
        }

        public override int ItemCount
        {
            get { return ListMyCard != null ? ListMyCard.Count() : 0; }
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);
            RecyclerView.ViewHolder rvHolder = holder as RecyclerView.ViewHolder;
            MyCardViewHolder viewHolder = holder as MyCardViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvNameCard);
        }
    }
}