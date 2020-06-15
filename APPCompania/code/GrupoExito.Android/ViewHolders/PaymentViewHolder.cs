using Android.Support.Constraints;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class PaymentViewHolder : RecyclerView.ViewHolder
    {
        public ConstraintLayout ClyPaymentOptionItem { get; private set; }
        public LinearLayout LyOnDeliver { get; private set; }
        public LinearLayout LyInformationCard { get; private set; }
        public LinearLayout LyChecker { get; private set; }
        public LinearLayout LyExternalCircle { get; private set; }
        public LinearLayout LyInternalCircle { get; private set; }
        public LinearLayout LyActionsOnDeliver { get; private set; }
        public LinearLayout LyInsideUseDataPhone { get; private set; }
        public LinearLayout LyInsideUseCash { get; private set; }
        public ImageView IvChecker { get; private set; }
        public ImageView IvNameCard { get; private set; }     
        public TextView TvNumberCard { get; private set; }
        public TextView TvTypeDeliver { get; private set; }
        public TextView TvUseCash { get; private set; }
        public TextView TvUseDataPhone { get; private set; }

        public PaymentViewHolder(View itemView, IPayment paymentInterface, IList<CreditCard> listMyCards) : base(itemView)
        {
            ClyPaymentOptionItem = itemView.FindViewById<ConstraintLayout>(Resource.Id.clyPaymentOptionItem);
            LyChecker = itemView.FindViewById<LinearLayout>(Resource.Id.lyChecker);
            LyOnDeliver = itemView.FindViewById<LinearLayout>(Resource.Id.lyOnDeliver);
            LyInformationCard = itemView.FindViewById<LinearLayout>(Resource.Id.lyInformationCard);
            LyExternalCircle = itemView.FindViewById<LinearLayout>(Resource.Id.lyExternalCircle);
            LyInternalCircle = itemView.FindViewById<LinearLayout>(Resource.Id.lyInternalCircle);
            LyActionsOnDeliver = itemView.FindViewById<LinearLayout>(Resource.Id.lyActionsOnDeliver);
            LyInsideUseDataPhone = itemView.FindViewById<LinearLayout>(Resource.Id.lyInsideUseDataPhone);
            LyInsideUseCash = itemView.FindViewById<LinearLayout>(Resource.Id.lyInsideUseCash);
            IvChecker = itemView.FindViewById<ImageView>(Resource.Id.ivChecker);
            IvNameCard = itemView.FindViewById<ImageView>(Resource.Id.ivNameCard);
            TvNumberCard = itemView.FindViewById<TextView>(Resource.Id.tvNumberCard);
            TvTypeDeliver = itemView.FindViewById<TextView>(Resource.Id.tvTypeDeliver);
            TvUseCash = itemView.FindViewById<TextView>(Resource.Id.tvUseCash);
            TvUseDataPhone = itemView.FindViewById<TextView>(Resource.Id.tvUseDataPhone);
            ClyPaymentOptionItem.Click += delegate { paymentInterface.OnCardSelected(listMyCards[AdapterPosition]); };
        }
    }
}