using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Load.Engine;
using Com.Bumptech.Glide.Request;
using Com.Bumptech.Glide.Signature;
using GrupoExito.Android.Utilities;
using GrupoExito.Android.ViewHolders;
using GrupoExito.Entities.Constants;
using GrupoExito.Entities.Entiites;
using GrupoExito.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoExito.Android.Adapters
{
    public class PaymentAdapter : RecyclerView.Adapter
    {
        private IList<CreditCard> ListMyCard { get; set; }
        private IPayment PaymentInterface { get; set; }
        private PaymentViewHolder paymentViewHolder { get; set; }
        private Context Context { get; set; }
        private bool ShowDelete { get; set; }

        public PaymentAdapter(IList<CreditCard> listMyCard, Context context, IPayment PaymentInterface)
        {
            this.PaymentInterface = PaymentInterface;
            this.Context = context;
            this.ListMyCard = listMyCard;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ListItemPayment, parent, false);
            PaymentViewHolder paymentViewHolder = new PaymentViewHolder(itemView, PaymentInterface, ListMyCard);
            SetViewHolderEvents(paymentViewHolder);
            return paymentViewHolder;
        }

        private void SetViewHolderEvents(PaymentViewHolder paymentViewHolder)
        {
            paymentViewHolder.LyInsideUseCash.Click += delegate
            {
                this.DrawButton(paymentViewHolder.LyInsideUseCash,paymentViewHolder.TvUseCash, true);
                this.DrawButton(paymentViewHolder.LyInsideUseDataPhone, paymentViewHolder.TvUseDataPhone, false);
                PaymentInterface.OnCardSelectedTypeOnDeliver((int)EnumPaymentType.Cash);
            };

            paymentViewHolder.LyInsideUseDataPhone.Click += delegate
            {
                this.DrawButton(paymentViewHolder.LyInsideUseCash, paymentViewHolder.TvUseCash, false);
                this.DrawButton(paymentViewHolder.LyInsideUseDataPhone, paymentViewHolder.TvUseDataPhone, true);
                PaymentInterface.OnCardSelectedTypeOnDeliver((int)EnumPaymentType.Dataphone);
            };
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var requestOptions = new RequestOptions()
                .Apply(RequestOptions.SkipMemoryCacheOf(true))
                .Apply(RequestOptions.DiskCacheStrategyOf(DiskCacheStrategy.None))
                .Apply(RequestOptions.SignatureOf(new ObjectKey(DateTime.Now.Millisecond)));

            paymentViewHolder = holder as PaymentViewHolder;
            CreditCard myCard = ListMyCard[position];
            paymentViewHolder.TvNumberCard.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            paymentViewHolder.TvTypeDeliver.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            paymentViewHolder.TvUseCash.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            paymentViewHolder.TvUseDataPhone.SetTypeface(FontManager.Instance.GetTypeFace(Context, FontManager.RalewaySemiBold), TypefaceStyle.Normal);
            paymentViewHolder.LyInsideUseCash.Enabled = false;
            paymentViewHolder.LyInsideUseDataPhone.Enabled = false;
            this.DrawButton(paymentViewHolder.LyInsideUseCash, paymentViewHolder.TvUseCash, false);
            this.DrawButton(paymentViewHolder.LyInsideUseDataPhone, paymentViewHolder.TvUseDataPhone, false);
            paymentViewHolder.LyActionsOnDeliver.Visibility = ViewStates.Gone;

            if (myCard.Bin.Equals(ConstTypePayment.OnDeliver))
            {
                paymentViewHolder.LyOnDeliver.Visibility = ViewStates.Visible;
                paymentViewHolder.LyInformationCard.Visibility = ViewStates.Gone;
                paymentViewHolder.TvTypeDeliver.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.black)));
            }
            else
            {
                paymentViewHolder.LyOnDeliver.Visibility = ViewStates.Gone;
                paymentViewHolder.LyInformationCard.Visibility = ViewStates.Visible;
                Glide.With(holder.ItemView.Context).Load(ConvertUtilities.ResourceId(myCard.Image)).Apply(requestOptions).Thumbnail(0.5f).Into(paymentViewHolder.IvNameCard);
                paymentViewHolder.TvNumberCard.Text = myCard.NumberCard.Substring(myCard.NumberCard.Length-4, 4);
            }

            if (myCard.Selected)
            {
                paymentViewHolder.LyExternalCircle.Visibility = ViewStates.Visible;
                paymentViewHolder.IvChecker.Visibility = ViewStates.Visible;
                paymentViewHolder.LyInternalCircle.SetBackgroundResource(Resource.Drawable.circle_yellow);
                paymentViewHolder.LyChecker.SetBackgroundResource(Resource.Drawable.credit_card_item_primary_payment);

                if (myCard.Bin.Equals(ConstTypePayment.OnDeliver))
                {
                    paymentViewHolder.LyInsideUseCash.Enabled = true;
                    paymentViewHolder.LyInsideUseDataPhone.Enabled = true;
                    paymentViewHolder.LyActionsOnDeliver.Visibility = ViewStates.Visible;
                    paymentViewHolder.TvTypeDeliver.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
                }
                else
                {
                    paymentViewHolder.TvNumberCard.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
                }
            }
            else
            {
                paymentViewHolder.LyExternalCircle.Visibility = ViewStates.Visible;
                paymentViewHolder.LyChecker.SetBackgroundResource(Resource.Drawable.credit_card_item_gray_payment);
                paymentViewHolder.IvChecker.Visibility = ViewStates.Gone;
                paymentViewHolder.LyInternalCircle.SetBackgroundResource(Resource.Drawable.circle_gray);
                paymentViewHolder.TvNumberCard.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.black)));
            }

        }

        private void DrawButton(LinearLayout botton, TextView textView, bool enabled)
        {
            if (enabled)
            {
                botton.SetBackgroundResource(Resource.Drawable.button_yellow);
                textView.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.white)));
            }
            else
            {
                botton.SetBackgroundResource(Resource.Drawable.button_gray);
                textView.SetTextColor(new Color(ContextCompat.GetColor(Context, Resource.Color.black)));
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
            PaymentViewHolder viewHolder = holder as PaymentViewHolder;
            Glide.With(rvHolder.ItemView.Context).Clear(viewHolder.IvNameCard);
        }
    }
}