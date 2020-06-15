using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using GrupoExito.Android.Adapters;
using GrupoExito.Entities.Entiites;
using System.Collections.Generic;

namespace GrupoExito.Android.ViewHolders
{
    class MyCardViewHolder : RecyclerView.ViewHolder
    {
        public LinearLayout LyInformationCard { get; private set; }
        public ImageView IvNameCard { get; private set; }        
        public TextView TvNameCard { get; private set; }
        public TextView TvNumberCard { get; private set; }
        public ImageView IvDeleteCard { get; private set; }
        public LinearLayout LyMessageAlert { get; private set; }
        public TextView TvMessageCuducity { get; private set; }
        public TextView TvDateCuducity { get; private set; } 
        public LinearLayout LyQuantityMessage { get; private set; } 
        public TextView TvQuantityMessage { get; private set; }
        public LinearLayout LyMyCard { get; private set; }
        public LinearLayout LyDeleteCard { get; private set; }
        public LinearLayout LyCircle { get; private set; }

        public MyCardViewHolder(View itemView, IMyCards myCardsInterface, IList<CreditCard> myCards) : base(itemView)
        {
            LyInformationCard = itemView.FindViewById<LinearLayout>(Resource.Id.lyInformationCard);
            IvNameCard = itemView.FindViewById<ImageView>(Resource.Id.ivNameCard);
            TvNameCard = itemView.FindViewById<TextView>(Resource.Id.tvNameCard);
            TvNumberCard = itemView.FindViewById<TextView>(Resource.Id.tvNumberCard);
            IvDeleteCard = itemView.FindViewById<ImageView>(Resource.Id.ivDeleteCard);
            LyMessageAlert = itemView.FindViewById<LinearLayout>(Resource.Id.lyMessageAlert);
            TvMessageCuducity = itemView.FindViewById<TextView>(Resource.Id.tvMessageCuducity);
            TvDateCuducity = itemView.FindViewById<TextView>(Resource.Id.tvDateCuducity);
            IvDeleteCard.Click += delegate { myCardsInterface.OnDeleteMyCardsClicked(myCards[AdapterPosition]); };
            LyMyCard = itemView.FindViewById<LinearLayout>(Resource.Id.lyMyCard);
            LyDeleteCard = itemView.FindViewById<LinearLayout>(Resource.Id.lyDeleteCard);
            LyCircle = itemView.FindViewById<LinearLayout>(Resource.Id.lyCircle);
            LyMyCard.Click += delegate { myCardsInterface.OnCardSelected(myCards[AdapterPosition]); };
        }
    }
}